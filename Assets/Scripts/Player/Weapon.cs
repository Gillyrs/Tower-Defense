using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cysharp.Threading.Tasks;
using System;
using Random = UnityEngine.Random;

public class Weapon : MonoBehaviour
{
    [Header("Stats")]
    public bool isShooting;
    [SerializeField] private float shotDelay;
    [SerializeField] private float reloadDelay;
    [SerializeField] private int damage;
    [SerializeField] private int maxAmmo;
    [SerializeField] private int currentAmmo;

    [Header("Fire")]
    [SerializeField] private Transform firePosition;
    [SerializeField] private List<Sprite> fireSprites;
    [SerializeField] private GameObject shootObject;
    private SpriteRenderer shootObjectRenderer;

    [Header("Sounds")]
    [SerializeField] private AudioSource shootingSound;
    [SerializeField] private AudioSource hittingSound;

    [Header("Prefab")]
    [SerializeField] private GameObject bloodPrefab;  
    private IObjectPool objectPool;

    private void Start()
    {
        shootObjectRenderer = shootObject.GetComponent<SpriteRenderer>();
        currentAmmo = maxAmmo;
        objectPool = ObjectPoolSpawner.GetObjectPool(bloodPrefab);
    }
    public void StartShooting()
    {
        if (isShooting)
            return;
        StartCoroutine(Shoot());
    }
    public async void EndShooting()
    {
        await UniTask.Delay(Convert.ToInt32(shotDelay * 1000));
        isShooting = false;
        StopAllCoroutines();
        shootObject.SetActive(false);
    }
    private void Update()
    {
        Debug.DrawRay(firePosition.position, firePosition.up);
    }
    private IEnumerator Shoot()
    {
        isShooting = true;
        while (currentAmmo != 0)
        {
            shootObjectRenderer.sprite = fireSprites[Random.Range(0, fireSprites.Count - 1)];
            shootObject.SetActive(true);
            shootingSound.pitch = Random.Range(1.7f, 1.9f);
            shootingSound.Play();
            currentAmmo--;
            var hit = Physics2D.Raycast(firePosition.position, firePosition.up);
            GameObject obj = null;
            if (hit.transform != null)
            {
                if (hit.transform.TryGetComponent(out IDamagable damagable))
                {
                    obj = objectPool.Instantiate(hit.point, Quaternion.identity);
                    var controller = obj.GetComponent<AnimationController>();
                    controller.OnAnimationEnded += (obj) => objectPool.Destroy(obj);
                    controller.Animator.Play("Hit");
                    hittingSound.pitch = Random.Range(0.6f, 0.8f);
                    hittingSound.Play();
                    damagable.TakeDamage(damage);
                }               
            }
            yield return new WaitForSeconds(0.05f);
            shootObject.SetActive(false);
            yield return new WaitForSeconds(shotDelay);           
        }
        isShooting = false;
        StartReloading();
    }
    private async void StartReloading()
    {
        await UniTask.Delay(Convert.ToInt32(reloadDelay * 1000));
        currentAmmo = maxAmmo;
    }

}
