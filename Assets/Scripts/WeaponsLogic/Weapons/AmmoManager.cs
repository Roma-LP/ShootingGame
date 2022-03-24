using System;
using UnityEngine;

public abstract class AmmoManager : BaseWeapon
{
    [SerializeField, Min(0f)] protected int currentAmmo = 30;
    [SerializeField, Min(0f)] protected int ammoMagazine = 30;
    [SerializeField, Min(0f)] protected int ammoTotal = 90;

    private int needAmmo;

    private void Awake()
    {
       // currentAmmo = ammoMagazine;
        print("currentAmmo - " + currentAmmo + "   kek");
    }

    public bool CheckCountAmmo() // protected
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

    public virtual void ReloadCurrentWeapon()
    {
        //if (ammoMagazine == 0) return; // for grenade
        needAmmo = ammoMagazine - currentAmmo;
        if (ammoMagazine <= ammoTotal)
        {
            ammoTotal = ammoTotal - needAmmo;
            currentAmmo = ammoMagazine;
        }
        else
        {
            if (ammoTotal - needAmmo > 0)
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