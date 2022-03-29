using UnityEngine;
using TMPro;

public class MagazineAmmos : MonoBehaviour
{
    [SerializeField] private TMP_Text currentAmmo;
    [SerializeField] private TMP_Text countAmmoTotal;

    public void SetCurrentAmmo(int currentAmmo)
    {
        this.currentAmmo.text = currentAmmo.ToString();
    }

    public void SetCountAmmoTotal(int countAmmoTotal)
    {
        this.countAmmoTotal.text = countAmmoTotal.ToString();
    }

    public void SetEmptyFields()
    {
        this.currentAmmo.text = "-";
        this.countAmmoTotal.text = "-";
    }
}
