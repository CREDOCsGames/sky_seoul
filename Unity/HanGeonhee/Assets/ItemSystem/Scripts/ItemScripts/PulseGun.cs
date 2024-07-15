using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PulseGun : WeaponItem, IWeaponReload
{
    public PulseGun()
    {
        maxAmmo = 12;

        currentAmmo = maxAmmo;
    }

    public override void AttackEffect()
    {
        if (currentAmmo <= 0)
        {
            Debug.Log("펄스 건을 발사하려 했으나, 장전되어 있는 탄약이 없습니다.");
            ReloadEffect();
            return;
        }

        currentAmmo--;

        Debug.Log("펄스 건을 발사 했습니다." + currentAmmo + "발 남았습니다.");
    }

    public void ReloadEffect()
    {
        Debug.Log("재장전 합니다.");

        currentAmmo = maxAmmo;
    }
}
