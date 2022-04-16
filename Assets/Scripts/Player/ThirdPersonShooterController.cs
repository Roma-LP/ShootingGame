using Cinemachine;
using Photon.Pun;
using Photon.Realtime;
using StarterAssets;
using UnityEngine;

public class ThirdPersonShooterController : MonoBehaviour
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

    private ThirdPersonController thirdPersonController;
    private Cinemachine3rdPersonFollow personFollowComponent;
    private Vector2 screenCenterPoint;
    private Ray ray;

    private void Awake()
    {
        thirdPersonController = GetComponent<ThirdPersonController>();
        personFollowComponent = virtualCamera.GetCinemachineComponent<Cinemachine3rdPersonFollow>();
        starterAssetsInputs.OnReloadWeapon += ReloadWeapon;
    }

    private void Update()
    {
        screenCenterPoint = new Vector2(Screen.width / 2, Screen.height / 2);
        ray = Camera.main.ScreenPointToRay(screenCenterPoint);
        Debug.DrawRay(ray.origin, ray.direction, Color.red);

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

    private void OnDestroy()
    {
        starterAssetsInputs.OnReloadWeapon -= ReloadWeapon;
    }
}