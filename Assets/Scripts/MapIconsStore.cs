using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

[CreateAssetMenu(fileName = "MapIconStore", menuName = "MapIcon")]
public class MapIconsStore : ScriptableObject
{
    [SerializeField] private MapIcon[] mapIcons;

    public MapIcon[] MapIcons { get => mapIcons; }

    [Serializable]
    public class MapIcon
    {
        public string mapName;
        public Sprite iconSprite;
    }
}