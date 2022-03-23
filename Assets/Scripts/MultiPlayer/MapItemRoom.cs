using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using Photon.Pun;

public class MapItemRoom : MonoBehaviour
{
    private int currentIndex = 0;

    [SerializeField] public TMP_Text mapName;
    [SerializeField] private Image mapIcon;
    [SerializeField] private MapIconsStore iconsStore;
    [SerializeField] private GameObject[] arrows;


    public bool IsEnableToEdit
    {
        set
        {
            foreach (var arrow in arrows)
                arrow.SetActive(value);
        }
    }
    private void Awake()
    {
        SetCurrentMapByIndex();
    }
    public void OnClickLeftArrow()
    {
        currentIndex = (int)Mathf.Repeat(--currentIndex, iconsStore.Length);
        SetCurrentMapByIndex();

    }
    public void OnClickRightArrow()
    {
        currentIndex = (int)Mathf.Repeat(++currentIndex, iconsStore.Length);
        SetCurrentMapByIndex();
    }

    private void SetCurrentMapByIndex()
    {
        mapIcon.sprite = iconsStore[currentIndex].iconSprite;
        mapName.text = iconsStore[currentIndex].enumValue.ToString();
        if (PhotonNetwork.CurrentRoom != null)
        {
            var prop = PhotonNetwork.CurrentRoom.CustomProperties;
            prop["MapName"] = iconsStore[currentIndex].enumValue.ToString();
            PhotonNetwork.CurrentRoom.SetCustomProperties(prop);
        }
    }

    public void SetMap(MapType map)
    {
        currentIndex = iconsStore.GetIndexByEnum(map);
        mapName.text = map.ToString();
        mapIcon.sprite = iconsStore[currentIndex].iconSprite;
    }
}
