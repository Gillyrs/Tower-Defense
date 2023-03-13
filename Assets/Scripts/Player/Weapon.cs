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
    [SerializeField] private bool shouldPitch;

    [Header("Fire")]
    [SerializeField] private List<Transform> firePositions;
    [SerializeField] private List<Sprite> fireSprites;
    [SerializeField] private List<GameObject> shootObjects;
    private List<SpriteRenderer> shootObjectsRenderers;

    [Header("Sounds")]
    [SerializeField] private AudioSource shootingSound;
    [SerializeField] protected AudioSource hittingSound;

    [Header("Prefab")]
    [SerializeField] private GameObject bloodPrefab;  
    private IObjectPool objectPool;

    [SerializeField] private Coroutine shootCoroutine;

    private void Start()
    {
        shootObjectsRenderers = new List<SpriteRenderer>();
        for (int i = 0; i < shootObjects.Count; i++)
        {
            shootObjectsRenderers.Add(shootObjects[i].GetComponent<SpriteRenderer>());
        }
        currentAmmo = maxAmmo;
        objectPool = ObjectPoolSpawner.GetObjectPool(bloodPrefab);
    }
    private void OnDestroy()
    {
        StopAllCoroutines();
        shootObjectsRenderers = null;
        objectPool = null;
        shootCoroutine = null;
    }
    public void StartShooting()
    {
        if (isShooting)
            return;
        if (currentAmmo == 0)
            return;
        shootCoroutine = StartCoroutine(Shoot());
    }
    public async void EndShooting()
    {
        if (!isActiveAndEnabled)
            return;
        await UniTask.Delay(Convert.ToInt32(shotDelay * 1000));
        isShooting = false;
        if(shootCoroutine != null)
            StopCoroutine(shootCoroutine);
        foreach (var item in shootObjects)
        {
            if(item != null)
                item.SetActive(false);
        }
        
    }
    private IEnumerator Shoot()
    {
        isShooting = true;
        while (currentAmmo != 0)
        {
            foreach (var item in shootObjectsRenderers)
            {
                item.sprite = fireSprites[Random.Range(0, fireSprites.Count - 1)];
            }

            foreach (var item in shootObjects)
            {
                item.SetActive(true);
            }
            if (shouldPitch)
                shootingSound.pitch = Random.Range(1.7f, 1.9f);
            shootingSound.Play();
            currentAmmo--;
            for (int i = 0; i < firePositions.Count; i++)
            {
                var hit = Physics2D.RaycastAll(firePositions[i].position, firePositions[i].up);
                GameObject obj = null;
                foreach (var item in hit)
                {
                    if (item.transform != null)
                    {
                        if (item.transform.TryGetComponent(out IDamagable damagable))
                        {
                            obj = objectPool.Instantiate(item.point, Quaternion.identity);
                            var controller = obj.GetComponent<AnimationController>();
                            controller.OnAnimationEnded += (obj) => objectPool.Destroy(obj);
                            controller.Animator.Play("Hit");
                            hittingSound.pitch = Random.Range(0.6f, 0.8f);
                            hittingSound.Play();
                            damagable.TakeDamage(damage);
                            break;
                        }
                    }
                }
            }                      
            yield return new WaitForSeconds(0.05f);
            foreach (var item in shootObjects)
            {
                item.SetActive(false);
            }
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
