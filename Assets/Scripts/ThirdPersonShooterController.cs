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
    [SerializeField] private CinemachineVirtualCamera virtualCamera;
    [SerializeField] private float normalSensitivity;
    [SerializeField] private float aimSensitivity;
    [SerializeField] private float normalCameraDistance;
    [SerializeField] private float aimCameraDistance;
    [SerializeField, Min(0.01f)] private float aimChangeSpeed;
    [SerializeField] private CrosshairScatter crosshair;

    [SerializeField] private SwitchWeaponsLogic switchWeaponsLogic;
    [SerializeField] private StarterAssetsInputs starterAssetsInputs;
    [SerializeField] private MagazineAmmos magazineAmmos;
    [SerializeField] private HealthBarManager healthBarManager;
    [SerializeField, Min(1f)] private float healthPoint = 150;

    private ThirdPersonController thirdPersonController;
    private Cinemachine3rdPersonFollow personFollowComponent;
    private Vector2 screenCenterPoint;
    private Ray ray;
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
        healthBarManager.SetMaxHP(HP);
        starterAssetsInputs.OnReloadWeapon += ReloadWeapon;
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

            switchWeaponsLogic.CurrentWeapon.UseWepon(ray, PhotonNetwork.LocalPlayer.ActorNumber);
            crosshair.ChangeSizeCrosshairOnShoot();
            magazineAmmos.SetAmmoWeapon(switchWeaponsLogic.CurrentWeapon);
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
    private void ReloadWeapon()
    {
        if (switchWeaponsLogic.CurrentWeapon is AmmoManagerFirearms)
        {
            var ammoManagerWeapon = switchWeaponsLogic.CurrentWeapon as AmmoManagerFirearms;
            ammoManagerWeapon.ReloadCurrentWeapon();
            magazineAmmos.SetAmmoWeapon(switchWeaponsLogic.CurrentWeapon);
        }
    }

    public void ReducingLife(float damage, int opponentID)
    {
        PV.RPC("TakeDamage", RpcTarget.All, damage, opponentID);
    }

    [PunRPC]
    public void TakeDamage(float damage, int opponentID)
    {
        if (!PV.IsMine) return;

        HP = HP - damage;
        healthBarManager.SetCurrentHP(HP);
        if (HP <= 0f)
        {
            Dead();
            var props = PhotonNetwork.PlayerList.First(x => x.ActorNumber == opponentID).CustomProperties;
            int countKilss = props.GetIntInProperties("Kills");
            props.ResetPropertyValue("Kills", ++countKilss);
        }
    }

    public void Dead()
    {
        var props = PhotonNetwork.LocalPlayer.CustomProperties;
        int countDeaths = props.GetIntInProperties("Deaths");
        props.ResetPropertyValue("Deaths", ++countDeaths);
        PhotonNetwork.Destroy(gameObject);
        GameManager.Instance.Spawn();
    }

    private void OnDestroy()
    {
        starterAssetsInputs.OnReloadWeapon -= ReloadWeapon;
    }
}