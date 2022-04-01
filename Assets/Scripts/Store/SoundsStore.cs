using UnityEngine;

public enum Sounds
{
    AWP,AWP_Empty, AWP_Reload, M14, M14_Empty, M14_Reload, Pistol, Pistol_Empty, Pistol_Reload, Shotgun, Shotgun_Empty, Shotgun_Reload, Knife_Hit, Smoke_Explode, HE_Explode, Flash_Explode, Draw_Grenade
}

[CreateAssetMenu(fileName = "SoundsStore", menuName = "Stores/SoundsStore")]
public class SoundsStore : Store<Sounds, AudioClip>
{
    
}
