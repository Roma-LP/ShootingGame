using UnityEngine;
using System.Linq;

public class NewBehaviourScript : MonoBehaviour
{
    public Collider collider1;

    public bool CalculateExplosure()
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
                new Ray(origin, transform.position + collider1.bounds.size.x / 2 * Vector3.left - origin),
                new Ray(origin, (transform.position + collider1.bounds.size.x / 2 * Vector3.right) - origin),
                new Ray(origin, (transform.position + collider1.bounds.size.y / 2 * Vector3.up) - origin),
                new Ray(origin, (transform.position + collider1.bounds.size.y / 2 * Vector3.down) - origin),
                new Ray(origin, (transform.position + collider1.bounds.size.z / 2 * Vector3.forward) - origin),
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
        return GeometryUtility.TestPlanesAABB(planes, collider1.bounds);
    }
}
