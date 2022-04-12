using UnityEngine;
using StarterAssets;
using Photon.Pun;

public class SwitchWeaponsLogic : MonoBehaviourPun
{
    [SerializeField] private Firearms firstWeapon;
    [SerializeField] private Firearms secondWeapon;
    [SerializeField] private ColdWeapon thirdWeapon;
    [SerializeField] private GrenadeManager prefabGrenade;
    [SerializeField] private StarterAssetsInputs starterAssetsInputs;
    [SerializeField] private MagazineAmmos magazineAmmos;
    [SerializeField] private PhotonView PV;

    public BaseWeapon CurrentWeapon { get;private set; }

    private void Awake()
    {
        starterAssetsInputs.OnPickWeaponCustom += RPC_SetWeapon;
        RPC_SetWeapon(Weapons.FirstWeapon);
    }

    public void RPC_SetWeapon(Weapons weapons)
    {
        PV.RPC("SetWeapon",RpcTarget.All,weapons);
    }


    [PunRPC]
    public void SetWeapon(Weapons weapons)
    {
        firstWeapon.gameObject.SetActive(false);
        secondWeapon.gameObject.SetActive(false);
        thirdWeapon.gameObject.SetActive(false);
        prefabGrenade.gameObject.SetActive(false);
        switch (weapons)
        {
            case Weapons.FirstWeapon:
                firstWeapon.gameObject.SetActive(true);
                CurrentWeapon = firstWeapon;
                break;
            case Weapons.SecondWeapon:
                secondWeapon.gameObject.SetActive(true);
                CurrentWeapon = secondWeapon;
                break;
            case Weapons.ThirdWeapon:
                thirdWeapon.gameObject.SetActive(true);
                CurrentWeapon = thirdWeapon;
                break;
            case Weapons.FourthWeapon:
                prefabGrenade.gameObject.SetActive(true);
                CurrentWeapon = prefabGrenade;
                break;
        }

        if (PV.IsMine)
            magazineAmmos.SetAmmoWeapon(CurrentWeapon);
    }

    private void OnDestroy()
    {
        starterAssetsInputs.OnPickWeaponCustom -= RPC_SetWeapon;
    }
}
