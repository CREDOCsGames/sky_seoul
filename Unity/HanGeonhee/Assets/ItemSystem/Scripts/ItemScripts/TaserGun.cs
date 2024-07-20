using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TaserGun : WeaponItem, IWeaponReload
{
    public TaserGun()
    {
        maxAmmo = 4;
        currentAmmo = maxAmmo;

        attackSpeed = 4f;
        attackDamage = 1f;
    }

    public override void AttackEffect()
    {
        if(currentAmmo <= 0)
        {
            Debug.Log("테이저 건을 발사하려 했으나, 장전되어 있는 탄약이 없습니다.");
            ReloadEffect();
            return;
        }

        currentAmmo--;

        Debug.Log("테이저 건을 발사 했습니다." + currentAmmo + "발 남았습니다.");
    }

    public void ReloadEffect()
    {
        Debug.Log("재장전 합니다.");

        currentAmmo = maxAmmo;
    }
}
