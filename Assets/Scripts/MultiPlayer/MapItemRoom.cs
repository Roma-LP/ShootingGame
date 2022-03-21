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


    private void Awake()
    {
        SetCurrentMapByIndex();
    }
    public void OnClickLeftArrow()
    {
        currentIndex = (int)Mathf.Repeat(--currentIndex, iconsStore.MapIcons.Length);
        SetCurrentMapByIndex();

    }
    public void OnClickRightArrow()
    {
        currentIndex = (int)Mathf.Repeat(++currentIndex, iconsStore.MapIcons.Length);
        SetCurrentMapByIndex();
    }

    private void SetCurrentMapByIndex()
    {
        mapIcon.sprite = iconsStore.MapIcons[currentIndex].iconSprite;
        mapName.text = iconsStore.MapIcons[currentIndex].mapName;
        if (PhotonNetwork.CurrentRoom != null)
        {
            var prop = PhotonNetwork.CurrentRoom.CustomProperties;
            prop["MapName"] = iconsStore.MapIcons[currentIndex].mapName;
            PhotonNetwork.CurrentRoom.SetCustomProperties(prop);
        }
    }

    
}
