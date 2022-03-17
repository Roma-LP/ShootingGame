using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class MagazineAmmos : MonoBehaviour
{
    [SerializeField] private TMP_Text currentAmmo;
    [SerializeField] private TMP_Text countMagazine;

    public void SetCurrentAmmo(uint currentAmmo)
    {
        this.currentAmmo.text = currentAmmo.ToString();
    }

    public void SetCountMagazine(uint countMagazine)
    {
        this.countMagazine.text = countMagazine.ToString();
    }
}
