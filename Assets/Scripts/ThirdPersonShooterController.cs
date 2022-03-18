using Cinemachine;
using StarterAssets;
using System.Collections;
using UnityEngine;

public class ThirdPersonShooterController : MonoBehaviour
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
    [SerializeField] private Grenade prefabGrenade;
    [SerializeField] private StarterAssetsInputs starterAssetsInputs;
    [SerializeField] private MagazineAmmos magazineAmmos;


    private ThirdPersonController thirdPersonController;
    private Cinemachine3rdPersonFollow personFollowComponent;
    private bool isPlaying;
    private Vector2 screenCenterPoint;
    private Ray ray;
    private BaseWeapon currentWeapon_v2;


    private void Awake()
    {
        thirdPersonController = GetComponent<ThirdPersonController>();
        personFollowComponent = virtualCamera.GetCinemachineComponent<Cinemachine3rdPersonFollow>();
        currentWeapon_v2 = firstWeapon;
        SetAmmoWeapon();

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
            currentWeapon_v2.UseWepon(ray);
            SetAmmoWeapon();
            crosshair.ChangeSizeCrosshairOnShoot();
         

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

    private void SetWeapon(Weapons weapons)
    {
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
        if (currentWeapon_v2 is AmmoManager)
        {
            var ammoManagerWeapon = currentWeapon_v2 as AmmoManager;
            magazineAmmos.SetCurrentAmmo(ammoManagerWeapon.GetCurrentAmmo());
            magazineAmmos.SetCountAmmoTotal(ammoManagerWeapon.GetAmmoTotal());
        }
        else
        {
            magazineAmmos.SetEmptyFields();
        }
    }

    private void ReloadWeapon()
    {
        if (currentWeapon_v2 is AmmoManager)
        {
            var ammoManagerWeapon = currentWeapon_v2 as AmmoManager;
            ammoManagerWeapon.ReloadCurrentWeapon();
            SetAmmoWeapon();
        }
    }

    IEnumerator Grenade()
    {
        isPlaying = true;
        Grenade grenade = Instantiate(prefabGrenade, spawnPointer.position, Quaternion.identity);
        grenade.Throw(ray.direction * forceThrow);
        yield return new WaitForSeconds(1.5f);
        isPlaying = false;
    }

    private void OnDestroy()
    {
        starterAssetsInputs.OnPickWeaponCustom -= SetWeapon;
        starterAssetsInputs.OnReloadWeapon -= ReloadWeapon;
    }
}
