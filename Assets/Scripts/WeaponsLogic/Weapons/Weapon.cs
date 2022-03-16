using System.Collections;
using UnityEngine;

[RequireComponent(typeof(AudioSource))]
public abstract class Weapon : MonoBehaviour
{
    [SerializeField] protected AudioClip audioClipExplosion;
    [SerializeField] protected Transform prefabHitPartical;
    [SerializeField] protected Transform prefabBulletHole;
    [SerializeField, Range(0.1f, 3f)] protected float rateOfFire = 0.8f;
    [SerializeField] protected uint ammoMagazine = 30;
    [SerializeField] protected float ammoDamage = 30;

    protected AudioSource audioSource;

    private bool isShootingWork;
    private uint currentAmmo;
    //protected abstract IEnumerator Explosion();

    private void Awake()
    {
        audioSource = GetComponent<AudioSource>();
        currentAmmo = ammoMagazine;
    }

    protected virtual IEnumerator Shooting(RaycastHit raycastHit)
    {
        isShootingWork = true;
        Instantiate(prefabHitPartical, raycastHit.point, Quaternion.identity);

        var hole = Instantiate(prefabBulletHole, raycastHit.point + raycastHit.normal * 0.001f, Quaternion.identity);
        hole.transform.position = raycastHit.point + raycastHit.normal * 0.01f;
        hole.transform.rotation = Quaternion.LookRotation(raycastHit.normal);
        hole.transform.Rotate(new Vector3(0, 0, 0));
        yield return new WaitForSeconds(rateOfFire);
        isShootingWork = false;
    }

    public void Shoot(RaycastHit raycastHit)
    {
        if (!isShootingWork && CheckcountPantrons())
            StartCoroutine(Shooting(raycastHit));
    }

    private bool CheckcountPantrons()
    {
        if (currentAmmo == 0)
            return false;
        else
        {
            currentAmmo--;
            return true;
        }
    }
}
