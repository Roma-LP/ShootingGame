using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using System;

public enum MapType
{
    Dust2, Mirage, Inferno
}

[CreateAssetMenu(fileName = "MapIconStore", menuName = "Stores/MapIcon")]
public class MapIconsStore : Store<MapType, Sprite>
{
    
}