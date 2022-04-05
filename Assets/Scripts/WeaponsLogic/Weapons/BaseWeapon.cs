using System.Collections;
using UnityEngine;

[RequireComponent(typeof(AudioSource))]
public abstract class BaseWeapon : MonoBehaviour , delete
{
    [SerializeField, Range(0.1f, 3f)] protected float rateOfFire = 0.8f;
    [SerializeField, Range(0f, 100f)] protected float ammoDamage = 30;
    [SerializeField] protected AudioClip audioClipExplosion;
    [SerializeField] protected Transform prefabHitPartical;
    [SerializeField] protected Transform prefabBulletHole;

    private bool isUsingWeapon;
    private AudioSource audioSource;

    private void Awake()
    {
        audioSource = GetComponent<AudioSource>();
    }
   
    public virtual void UseWepon(Ray raycast, int opponentID)
    {
        if (!isUsingWeapon)
        {
            StartCoroutine(UsingWeapon(raycast, opponentID));
            //audioSource.PlayOneShot(audioClipExplosion);  // пока нет звуков
        }
    } 

    protected virtual void Shot()
    {

    }

    private IEnumerator UsingWeapon(Ray raycast, int opponentID)
    {
        isUsingWeapon = true;
        if (Physics.Raycast(raycast, out RaycastHit raycastHit))
        {
            Debug.DrawRay(raycast.origin, raycast.direction, Color.blue);
            raycastHit.collider.gameObject.GetComponent<Multiplier>()?.DetectedDamage(ammoDamage, opponentID);
        }
        Instantiate(prefabHitPartical, raycastHit.point, Quaternion.identity);

        var hole = Instantiate(prefabBulletHole, raycastHit.point + raycastHit.normal * 0.001f, Quaternion.identity);
        hole.transform.position = raycastHit.point + raycastHit.normal * 0.01f;
        hole.transform.rotation = Quaternion.LookRotation(raycastHit.normal);
        hole.transform.Rotate(new Vector3(0, 0, 0));
        Shot();
        yield return new WaitForSeconds(rateOfFire);
        isUsingWeapon = false;
    }

    private void OnEnable()
    {
        isUsingWeapon = false;
    }
}
