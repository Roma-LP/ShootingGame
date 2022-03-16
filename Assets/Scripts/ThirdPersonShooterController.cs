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
    [SerializeField] private LayerMask aimColliderLayerMask;
    [SerializeField] private float distanceRay;
    [SerializeField] private Transform hitPartical;
    [SerializeField] private CrosshairScatter crosshair;
    [SerializeField] private Transform bulletHole;

    [SerializeField] private Transform spawnPointer;
    [SerializeField, Range(0f, 2500f)] protected float forceThrow;
    [SerializeField] private Weapon firstWeapon;
    [SerializeField] private Weapon secondWeapon;
    [SerializeField] private ColdWeapon thirdWeapon;
    [SerializeField] private Grenade prefabGrenade;
    [SerializeField] private StarterAssetsInputs starterAssetsInputs;


    private ThirdPersonController thirdPersonController;
    private Cinemachine3rdPersonFollow personFollowComponent;
    private bool isPlaying;
    private Vector2 screenCenterPoint;
    private Ray ray;
    private Weapons currentWeapon;


    private void Awake()
    {
        thirdPersonController = GetComponent<ThirdPersonController>();
        personFollowComponent = virtualCamera.GetCinemachineComponent<Cinemachine3rdPersonFollow>();

        starterAssetsInputs.OnFirstWeaponCustom += SetWeapon;
        starterAssetsInputs.OnSecondWeaponCustom += SetWeapon;
        starterAssetsInputs.OnThirdWeaponCustom += SetWeapon;
        starterAssetsInputs.OnFourthWeaponCustom += SetWeapon;
    }

    private void Update()
    {
        Vector3 mouseWorldPosition = Vector3.zero;
        screenCenterPoint = new Vector2(Screen.width / 2, Screen.height / 2);
        ray = Camera.main.ScreenPointToRay(screenCenterPoint);
        Transform hitTransform = null;
        if (Physics.Raycast(ray, out RaycastHit raycastHit, distanceRay, aimColliderLayerMask))
        {
            Debug.DrawRay(ray.origin, ray.direction * distanceRay, Color.blue);
            mouseWorldPosition = raycastHit.point;
            hitTransform = raycastHit.transform;

        }
        Vector3 worldAimTarget = mouseWorldPosition;
        worldAimTarget.y = transform.position.y;
        Vector3 aimDirection = (worldAimTarget - transform.position).normalized;

        if (starterAssetsInputs.shoot)
        {
            switch (thirdPersonController.CurrentWeapon)
            {
                case Weapons.FirstWeapon:
                    print("FirstWeapon do");
                    firstWeapon.Shoot(raycastHit);
                    print("FirstWeapon posle");
                    break;
                case Weapons.SecondWeapon:
                    secondWeapon.Shoot(raycastHit);
                    print("SecondWeapon");
                    break;
                case Weapons.ThirdWeapon:
                    print("ThirdWeapon");
                    break;
                case Weapons.FourthWeapon:
                    if (isPlaying == false) StartCoroutine(Grenade());
                    break;
            }





            //if (thirdPersonController.CurrentWeapon == Weapons.FourthWeapon)
            //{
            //    if(isPlaying == false) StartCoroutine(Grenade());
            //}
            //crosshair.ChangeSizeCrosshairOnShoot();
            //if (hitTransform != null)
            //{
            //    Instantiate(hitPartical, raycastHit.point, Quaternion.identity);
            
            //    var hole = Instantiate(bulletHole, raycastHit.point + raycastHit.normal * 0.001f, Quaternion.identity);
            //    hole.transform.position = raycastHit.point + raycastHit.normal * 0.01f;
            //    hole.transform.rotation = Quaternion.LookRotation(raycastHit.normal);
            //    hole.transform.Rotate(new Vector3(0, 0, 0));
            //}
            Debug.DrawRay(ray.origin, ray.direction * distanceRay, Color.red);
        }
        else
        {
            crosshair.ChangeSizeCrosshairOnNormal();
        }
        transform.forward = Vector3.Lerp(transform.forward, aimDirection, Time.deltaTime * 20f);
        if (starterAssetsInputs.aim)
        {
            if (personFollowComponent.CameraDistance > aimCameraDistance)
                personFollowComponent.CameraDistance -= aimChangeSpeed * Time.deltaTime;
            else
                personFollowComponent.CameraDistance = aimCameraDistance;
            thirdPersonController.SetSensitivity(aimSensitivity);
            thirdPersonController.SetRotateOnMove(false);
        }
        else
        {
            if (personFollowComponent.CameraDistance < normalCameraDistance)
                personFollowComponent.CameraDistance += aimChangeSpeed * Time.deltaTime;
            else
                personFollowComponent.CameraDistance = normalCameraDistance;
            thirdPersonController.SetSensitivity(normalSensitivity);
            thirdPersonController.SetRotateOnMove(true);
        }
    }

    private void SetWeapon(Weapons weapons)
    {
        currentWeapon = weapons;
        firstWeapon.gameObject.SetActive(false);
        secondWeapon.gameObject.SetActive(false);
        thirdWeapon.gameObject.SetActive(false);
        prefabGrenade.gameObject.SetActive(false);
        switch (currentWeapon)
        {
            case Weapons.FirstWeapon:
                firstWeapon.gameObject.SetActive(true);
                break;
            case Weapons.SecondWeapon:
                secondWeapon.gameObject.SetActive(true);
                break;
            case Weapons.ThirdWeapon:
                thirdWeapon.gameObject.SetActive(true);
                break;
            case Weapons.FourthWeapon:
                prefabGrenade.gameObject.SetActive(true);
                break;
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
        starterAssetsInputs.OnFirstWeaponCustom -= SetWeapon;
        starterAssetsInputs.OnSecondWeaponCustom -= SetWeapon;
        starterAssetsInputs.OnThirdWeaponCustom -= SetWeapon;
        starterAssetsInputs.OnFourthWeaponCustom -= SetWeapon;
    }
}
