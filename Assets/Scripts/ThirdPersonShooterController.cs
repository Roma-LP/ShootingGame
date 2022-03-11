using Cinemachine;
using StarterAssets;
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



    private ThirdPersonController thirdPersonController;
    private StarterAssetsInputs starterAssetsInputs;
    private Cinemachine3rdPersonFollow personFollowComponent;


    private void Awake()
    {
        thirdPersonController = GetComponent<ThirdPersonController>();
        starterAssetsInputs = GetComponent<StarterAssetsInputs>();
        personFollowComponent = virtualCamera.GetCinemachineComponent<Cinemachine3rdPersonFollow>();

    }

    private void Update()
    {
        Vector3 mouseWorldPosition = Vector3.zero;
        Vector2 screenCenterPoint = new Vector2(Screen.width / 2, Screen.height / 2);
        Ray ray = Camera.main.ScreenPointToRay(screenCenterPoint);
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
            crosshair.ChangeSizeCrosshairOnShoot();
            if (hitTransform != null)
            {
                Instantiate(hitPartical, raycastHit.point, Quaternion.identity);
            
                var hole = Instantiate(bulletHole, raycastHit.point + raycastHit.normal * 0.001f, Quaternion.identity);
                hole.transform.position = raycastHit.point + raycastHit.normal * 0.01f;
                hole.transform.rotation = Quaternion.LookRotation(raycastHit.normal);
                hole.transform.Rotate(new Vector3(0, 0, 0));
            }
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
}
