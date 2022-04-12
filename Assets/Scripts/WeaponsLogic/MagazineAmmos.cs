using UnityEngine;
using TMPro;

public class MagazineAmmos : MonoBehaviour
{
    [SerializeField] private TMP_Text currentAmmo;
    [SerializeField] private TMP_Text slash;
    [SerializeField] private TMP_Text countAmmoTotal;

    private void SetActiveUIAmmos(bool mod)
    {
        this.currentAmmo.gameObject.SetActive(mod);
        this.slash.gameObject.SetActive(mod);
        this.countAmmoTotal.gameObject.SetActive(mod);
    }

    private void SetFirearms(int currentAmmo, int countAmmoTotal)
    {
        SetActiveUIAmmos(true);
        this.currentAmmo.text = currentAmmo.ToString();
        this.countAmmoTotal.text = countAmmoTotal.ToString();
    }

    private void SetGrenade(int currentAmmo)
    {
        SetActiveUIAmmos(false);
        this.currentAmmo.text = currentAmmo.ToString();
        this.currentAmmo.gameObject.SetActive(true);
    }

    private void SetColdWeapon()
    {
        SetActiveUIAmmos(false);
    }
    public void SetAmmoWeapon(BaseWeapon currentWeapon)
    {
        switch (currentWeapon)
        {
            case AmmoManagerFirearms:
                {
                    var ammoManagerWeapon = currentWeapon as AmmoManagerFirearms;
                    SetFirearms(ammoManagerWeapon.CurrentAmmo, ammoManagerWeapon.AmmoTotal);
                    break;
                }
            case AmmoManagerGrenade:
                {
                    var ammoManagerWeapon = currentWeapon as AmmoManagerGrenade;
                    SetGrenade(ammoManagerWeapon.CurrentAmmo);
                    break;
                }
            case ColdWeapon:
                {
                    SetColdWeapon();
                    break;
                }
        }
    }
}