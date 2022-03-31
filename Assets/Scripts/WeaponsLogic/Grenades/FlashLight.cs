using System.Collections;
using UnityEngine;
using System.Linq;

public class FlashLight : ThrowingGrenade
{
    [SerializeField, Min(1f)] private float flashTime;
    [SerializeField] private Animator anim;

    private CapsuleCollider capsuleCollider;

    protected override void Awake()
    {
        base.Awake();
        capsuleCollider = GetComponent<CapsuleCollider>();
        //var x = Camera.main.GetComponentInChildren<FlashCanvasManager>();
        //var y = Camera.main.GetComponentInParent<FlashCanvasManager>();
    }

    private bool CalculateExplosure()
    {
        var planes = GeometryUtility.CalculateFrustumPlanes(Camera.main);
        Vector3[] origins = new Vector3[]
        {
            Camera.main.ScreenPointToRay(new Vector2(Screen.width, Screen.height)).origin,
            Camera.main.ScreenPointToRay(new Vector2(0, Screen.height)).origin,
            Camera.main.ScreenPointToRay(new Vector2(Screen.width, 0)).origin,
            Camera.main.ScreenPointToRay(new Vector2(0, 0)).origin,
        };
        if (!origins.Any(origin =>
        {
            Ray[] rays = new Ray[]
            {
                new Ray(origin, transform.position + capsuleCollider.bounds.size.x / 2 * Vector3.left - origin),
                new Ray(origin, (transform.position + capsuleCollider.bounds.size.x / 2 * Vector3.right) - origin),
                new Ray(origin, (transform.position + capsuleCollider.bounds.size.y / 2 * Vector3.up) - origin),
                new Ray(origin, (transform.position + capsuleCollider.bounds.size.y / 2 * Vector3.down) - origin),
                new Ray(origin, (transform.position + capsuleCollider.bounds.size.z / 2 * Vector3.forward) - origin),
            };
            return rays.Any(ray =>
            {
                if (Physics.Raycast(ray, out RaycastHit hit))
                    return hit.transform.gameObject == gameObject;
                else
                    return false;
            });
        }))
            return false;
        return GeometryUtility.TestPlanesAABB(planes, capsuleCollider.bounds);
    }

    protected override IEnumerator Explosion()
    {
        if(CalculateExplosure())
        {
            GameManager.Instance.ShowFlashBagEffect(flashTime);
        }
        Destroy(gameObject);    
        yield break;
    }
}
