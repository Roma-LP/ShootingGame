using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class AmmoManager : BaseWeapon
{
    private int currentAmmo;

    [SerializeField, Range(1f, 60f)] protected int ammoMagazine = 30;
    [SerializeField, Range(1f, 90f)] protected int ammoTotal = 90;


    private void Awake()
    {
        currentAmmo = ammoMagazine;
    }

    protected bool CheckCountAmmo()
    {
        if (currentAmmo == 0 || ammoTotal == 0)
            return false;
        else
        {
            currentAmmo--;
            return true;
        }
    }

    public void ReloadCurrentWeapon()
    {
        if (ammoMagazine <= ammoTotal)
        {
            ammoTotal = ammoTotal - (ammoMagazine - currentAmmo);
            currentAmmo = ammoMagazine;
        }
        else
        {
            ammoTotal = ammoTotal - (ammoMagazine - currentAmmo);
            if (ammoTotal < 0)
            {
                currentAmmo = ammoMagazine;
                currentAmmo = currentAmmo - ammoTotal;
                ammoTotal = 0;
            }
            else
            {
                currentAmmo = ammoMagazine;
            }
        }
    }
    public int GetCurrentAmmo() => currentAmmo;
    public int GetAmmoTotal() => ammoTotal;

}
