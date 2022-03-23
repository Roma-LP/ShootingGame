using System;
using UnityEngine;

public abstract class AmmoManager : BaseWeapon
{
    [SerializeField, Min(0f)] protected int ammoMagazine = 30;
    [SerializeField, Min(0f)] protected int ammoTotal = 90;

    protected int currentAmmo;

    private int needAmmo;

    private void Awake()
    {
        currentAmmo = ammoMagazine;
    }

    private bool CheckCountAmmo()
    {
        return currentAmmo == 0 ? false : true; 
    }

    protected override void Shot()
    {
        currentAmmo--;
    }

    public override void UseWepon(Ray ray)
    {
        if (CheckCountAmmo())
        {
            base.UseWepon(ray);
        }
    }

    public void ReloadCurrentWeapon()
    {
        needAmmo = ammoMagazine - currentAmmo;
        if (ammoMagazine <= ammoTotal)
        {
            ammoTotal = ammoTotal - needAmmo;
            currentAmmo = ammoMagazine;
        }
        else
        {
            if(ammoTotal - needAmmo > 0)
            {
                ammoTotal = ammoTotal - needAmmo;
                currentAmmo = ammoMagazine;
            }
            else
            {
                currentAmmo = currentAmmo + ammoTotal;
                ammoTotal = 0;
            }
        }
    }

    public int GetAmmoTotal() => ammoTotal;
    public int GetCurrentAmmo() => currentAmmo;
}