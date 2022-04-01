using UnityEngine;

public enum MapType
{
    Dust2, Mirage, Inferno
}

[CreateAssetMenu(fileName = "MapIconStore", menuName = "Stores/MapIcon")]
public class MapIconsStore : Store<MapType, Sprite>
{
    
}