using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cysharp.Threading.Tasks;
using System;
public class Weapon : MonoBehaviour
{
    public bool isShooting;
    [SerializeField] private float shotDelay;
    [SerializeField] private float reloadDelay;
    [SerializeField] private int damage;
    [SerializeField] private int maxAmmo;
    [SerializeField] private GameObject shootObject;
    [SerializeField] private Transform firePosition;
    [SerializeField] private Transform direction;
    [SerializeField] private int currentAmmo;

    private void Start()
    {
        currentAmmo = maxAmmo;
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
            shootObject.SetActive(true);
            currentAmmo--;
            var hit = Physics2D.Raycast(firePosition.position, firePosition.up);
            if (hit.transform != null)
            {
                if (hit.transform.TryGetComponent(out IDamagable damagable))
                {
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
