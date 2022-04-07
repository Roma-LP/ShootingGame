using Cinemachine;
using Photon.Pun;
using Photon.Realtime;
using StarterAssets;
using System;
using System.Linq;
using UnityEngine;
using Hashtable = ExitGames.Client.Photon.Hashtable;

public class ThirdPersonShooterController : MonoBehaviourPunCallbacks
{
    //[SerializeField] private CinemachineFramingTransposer VirtualCamera;
    [SerializeField] private CinemachineVirtualCamera virtualCamera;
    [SerializeField] private float normalSensitivity;
    [SerializeField] private float aimSensitivity;
    [SerializeField] private float normalCameraDistance;
    [SerializeField] private float aimCameraDistance;
    [SerializeField, Min(0.01f)] private float aimChangeSpeed;
    [SerializeField] private CrosshairScatter crosshair;

    [SerializeField] private Transform spawnPointer;
    [SerializeField, Range(0f, 2500f)] protected float forceThrow;
    [SerializeField] private Firearms firstWeapon;
    [SerializeField] private Firearms secondWeapon;
    [SerializeField] private ColdWeapon thirdWeapon;
    [SerializeField] private GrenadeManager prefabGrenade;
    [SerializeField] private StarterAssetsInputs starterAssetsInputs;
    [SerializeField] private MagazineAmmos magazineAmmos;
    [SerializeField, Min(1f)] private float healthPoint = 150;

    private ThirdPersonController thirdPersonController;
    private Cinemachine3rdPersonFollow personFollowComponent;
    private bool isPlaying;
    private Vector2 screenCenterPoint;
    private Ray ray;
    private BaseWeapon currentWeapon_v2;
    private PhotonView PV;

    public float HP
    {
        private set
        {
            healthPoint = value;
            if (healthPoint < 0)
                healthPoint = 0;
        }
        get
        {
            return healthPoint;
        }
    }

    private void Awake()
    {
        thirdPersonController = GetComponent<ThirdPersonController>();
        personFollowComponent = virtualCamera.GetCinemachineComponent<Cinemachine3rdPersonFollow>();
        PV = GetComponent<PhotonView>();
        currentWeapon_v2 = firstWeapon;
        SetAmmoWeapon();
        if (PV.IsMine)
            GameManager.Instance.SetNewHP(HP);
        starterAssetsInputs.OnReloadWeapon += ReloadWeapon;
        starterAssetsInputs.OnPickWeaponCustom += SetWeapon;
    }


    private void Update()
    {
        //Vector3 mouseWorldPosition = Vector3.zero;
        screenCenterPoint = new Vector2(Screen.width / 2, Screen.height / 2);
        ray = Camera.main.ScreenPointToRay(screenCenterPoint);
        Debug.DrawRay(ray.origin, ray.direction, Color.red);
        //Transform hitTransform = null;
        //if (Physics.Raycast(ray, out RaycastHit raycastHit, distanceRay, aimColliderLayerMask))
        //{
        //    Debug.DrawRay(ray.origin, ray.direction * distanceRay, Color.blue);
        //    mouseWorldPosition = raycastHit.point;
        //    hitTransform = raycastHit.transform;

        //}
        //Vector3 worldAimTarget = mouseWorldPosition;
        //worldAimTarget.y = transform.position.y;
        //Vector3 aimDirection = (worldAimTarget - transform.position).normalized;

        if (starterAssetsInputs.shoot)
        {

            currentWeapon_v2.UseWepon(ray, PhotonNetwork.LocalPlayer.ActorNumber);
            crosshair.ChangeSizeCrosshairOnShoot();
            SetAmmoWeapon();


            //if (currentWeapon_v2 is Grenade)
            //{
            //    if (isPlaying == false) StartCoroutine(Grenade());
            //}
            //else
            //{
            //    currentWeapon_v2.UseWepon(ray);
            //    UI_Canvas.ChangeSizeCrosshairOnShoot();
            //}
            //SetAmmoWeapon();


            //if (thirdPersonController.CurrentWeapon == Weapons.FourthWeapon)
            //{
            //    if(isPlaying == false) StartCoroutine(Grenade());
            //}
            //if (hitTransform != null)
            //{
            //    Instantiate(hitPartical, raycastHit.point, Quaternion.identity);

            //    var hole = Instantiate(bulletHole, raycastHit.point + raycastHit.normal * 0.001f, Quaternion.identity);
            //    hole.transform.position = raycastHit.point + raycastHit.normal * 0.01f;
            //    hole.transform.rotation = Quaternion.LookRotation(raycastHit.normal);
            //    hole.transform.Rotate(new Vector3(0, 0, 0));
            //}
        }
        else
        {
            crosshair.ChangeSizeCrosshairOnNormal();
        }
        if (starterAssetsInputs.aim)
        {
            if (personFollowComponent.CameraDistance > aimCameraDistance)
                personFollowComponent.CameraDistance -= aimChangeSpeed * Time.deltaTime;
            else
                personFollowComponent.CameraDistance = aimCameraDistance;
            thirdPersonController.SetSensitivity(aimSensitivity);
        }
        else
        {
            if (personFollowComponent.CameraDistance < normalCameraDistance)
                personFollowComponent.CameraDistance += aimChangeSpeed * Time.deltaTime;
            else
                personFollowComponent.CameraDistance = normalCameraDistance;
            thirdPersonController.SetSensitivity(normalSensitivity);
        }
    }

    //public void SetWeapon(Weapons weapons)
    //{
    //    print("do RPC");
    //    photonView.RPC("SetingWeapon", RpcTarget.All, weapons);
    //    print("posle RPC");
    //}

    public override void OnPlayerPropertiesUpdate(Player targetPlayer, Hashtable changedProps)
    {
        if (!PV.IsMine && targetPlayer == PV.Owner)
        {
            SetWeapon((Weapons)changedProps["weapon"]);
        }
    }

    public void SetWeapon(Weapons weapons)
    {
        if (PV.IsMine)
        {
            var prop = PhotonNetwork.LocalPlayer.CustomProperties;
            prop.ResetPropertyValue("weapon", weapons);
            //prop.Add("weapon", weapons);
            PhotonNetwork.LocalPlayer.SetCustomProperties(prop);
        }
        //if (!photonView.IsMine)  return;
        //currentWeapon = weapons;
        firstWeapon.gameObject.SetActive(false);
        secondWeapon.gameObject.SetActive(false);
        thirdWeapon.gameObject.SetActive(false);
        prefabGrenade.gameObject.SetActive(false);
        switch (weapons)
        {
            case Weapons.FirstWeapon:
                firstWeapon.gameObject.SetActive(true);
                currentWeapon_v2 = firstWeapon;
                break;
            case Weapons.SecondWeapon:
                secondWeapon.gameObject.SetActive(true);
                currentWeapon_v2 = secondWeapon;
                break;
            case Weapons.ThirdWeapon:
                thirdWeapon.gameObject.SetActive(true);
                currentWeapon_v2 = thirdWeapon;
                break;
            case Weapons.FourthWeapon:
                prefabGrenade.gameObject.SetActive(true);
                currentWeapon_v2 = prefabGrenade;
                break;
        }
        SetAmmoWeapon();
    }

    private void SetAmmoWeapon()
    {
        switch (currentWeapon_v2)
        {
            case AmmoManagerFirearms:
                {
                    var ammoManagerWeapon = currentWeapon_v2 as AmmoManagerFirearms;
                    magazineAmmos.SetFirearms(ammoManagerWeapon.CurrentAmmo, ammoManagerWeapon.AmmoTotal);
                    break;
                }
            case AmmoManagerGrenade:
                {
                    var ammoManagerWeapon = currentWeapon_v2 as AmmoManagerGrenade;
                    magazineAmmos.SetGrenade(ammoManagerWeapon.CurrentAmmo);
                    if (ammoManagerWeapon.CurrentAmmo == 0)
                        prefabGrenade.gameObject.SetActive(false);
                    break;
                }
            case ColdWeapon:
                {
                    magazineAmmos.SetColdWeapon();
                    break;
                }
        }
    }

    private void ReloadWeapon()
    {
        if (currentWeapon_v2 is AmmoManagerFirearms)
        {
            var ammoManagerWeapon = currentWeapon_v2 as AmmoManagerFirearms;
            ammoManagerWeapon.ReloadCurrentWeapon();
            SetAmmoWeapon();
        }
    }

    //IEnumerator Grenade()
    //{
    //    isPlaying = true;
    //    var x = currentWeapon_v2 as Grenade;
    //    if (!x.CheckCountAmmo()) yield break;
    //    x.kek();
    //    Grenade grenade = Instantiate(prefabGrenade, spawnPointer.position, Quaternion.identity);
    //    grenade.Throw(ray.direction * forceThrow);
    //    prefabGrenade.gameObject.SetActive(false);
    //    yield return new WaitForSeconds(1.3f);
    //    if (x.GetCurrentAmmo() != 0)
    //    {
    //        prefabGrenade.gameObject.SetActive(true);
    //    }
    //    isPlaying = false;
    //}

    public void ReducingLife(float damage, int opponentID)
    {
        PV.RPC("TakeDamage", RpcTarget.All, damage, opponentID);
    }

    [PunRPC]
    public void TakeDamage(float damage, int opponentID)
    {
        if (!PV.IsMine) return;

        HP = HP - damage;
        GameManager.Instance.SetNewHP(HP);
        if (HP <= 0f)
        {
            Dead();
            var props = PhotonNetwork.PlayerList.First(x => x.ActorNumber == opponentID).CustomProperties;
            var countKilss = props.GetIntInProperties("Kills");
            props.ResetPropertyValue("Kills", ++countKilss);
        }
    }

    public void Dead()
    {
        var props = PhotonNetwork.LocalPlayer.CustomProperties;
        var countDeaths = props.GetIntInProperties("Deaths");
        props.ResetPropertyValue("Deaths", ++countDeaths);
        PhotonNetwork.Destroy(gameObject);
        GameManager.Instance.Spawn();
    }

    private void OnDestroy()
    {
        starterAssetsInputs.OnPickWeaponCustom -= SetWeapon;
        starterAssetsInputs.OnReloadWeapon -= ReloadWeapon;
    }
}
