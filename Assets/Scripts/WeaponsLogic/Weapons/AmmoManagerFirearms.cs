using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class AmmoManagerFirearms : AmmoManager
{
    [SerializeField, Min(0f)] protected int ammoMagazine = 30;
    [SerializeField, Min(0f)] protected int ammoTotal = 90;

    private int needAmmo;

    public virtual void ReloadCurrentWeapon()
    {
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

    public int AmmoTotal => ammoTotal;
}
