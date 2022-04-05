using UnityEngine;

public abstract class AmmoManager : BaseWeapon
{
    [SerializeField, Min(0f)] protected int currentAmmo = 30;
    //[SerializeField, Min(0f)] protected int ammoMagazine = 30;
    //[SerializeField, Min(0f)] protected int ammoTotal = 90;

    //private int needAmmo;

    public bool CheckCountAmmo() // protected for grenade
    {
        return currentAmmo != 0;
    }

    protected override void Shot()
    {
        currentAmmo--;
    }

    public override void UseWepon(Ray ray, int opponentID)
    {
        if (CheckCountAmmo())
        {
            base.UseWepon(ray,opponentID);
        }
    }

    //public virtual void ReloadCurrentWeapon()
    //{
    //    //if (ammoMagazine == 0) return; // for grenade
    //    needAmmo = ammoMagazine - currentAmmo;
    //    if (ammoMagazine <= ammoTotal)
    //    {
    //        ammoTotal = ammoTotal - needAmmo;
    //        currentAmmo = ammoMagazine;
    //    }
    //    else
    //    {
    //        if (ammoTotal - needAmmo > 0)
    //        {
    //            ammoTotal = ammoTotal - needAmmo;
    //            currentAmmo = ammoMagazine;
    //        }
    //        else
    //        {
    //            currentAmmo = currentAmmo + ammoTotal;
    //            ammoTotal = 0;
    //        }
    //    }
    //}

    //public int GetAmmoTotal => ammoTotal;
    public int CurrentAmmo => currentAmmo;
}