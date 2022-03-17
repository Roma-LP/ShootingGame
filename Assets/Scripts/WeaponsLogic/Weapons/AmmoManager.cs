using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class AmmoManager : BaseWeapon
{
    private uint currentAmmo;

    [SerializeField, Range(1f, 60f)] protected uint ammoMagazine = 30;


    private void Awake()
    {
        currentAmmo = ammoMagazine;
    }

    protected bool CheckCountAmmo()
    {
        if (currentAmmo == 0)
            return false;
        else
        {
            currentAmmo--;
            return true;
        }
    }

    public uint GetCurrentAmmo() => currentAmmo;
    public uint GetAmmoMagazine() => ammoMagazine;
}
