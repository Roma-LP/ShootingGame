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

    public void SetFirearms(int currentAmmo, int countAmmoTotal)
    {
        SetActiveUIAmmos(true);
        this.currentAmmo.text = currentAmmo.ToString();
        this.countAmmoTotal.text = countAmmoTotal.ToString();
    }

    public void SetGrenade(int currentAmmo)
    {
        SetActiveUIAmmos(false);
        this.currentAmmo.text = currentAmmo.ToString();
        this.currentAmmo.gameObject.SetActive(true);
    }

    public void SetColdWeapon()
    {
        SetActiveUIAmmos(false);
    }
}
