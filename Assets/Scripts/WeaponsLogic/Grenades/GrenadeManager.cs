using System.Collections;
using UnityEngine;

public class GrenadeManager : AmmoManagerGrenade
{
    [SerializeField] private ThrowingGrenade throwingGrenade;
    [SerializeField] private Transform spawnPointer;
    [SerializeField, Min(1f)] protected float forceThrow;

    private bool isPlaying;
    public override void UseWepon(Ray ray)
    {
        if (isPlaying == false && CheckCountAmmo())
            StartCoroutine(Grenade(ray));
    }

    IEnumerator Grenade(Ray ray)
    {
        isPlaying = true;
        Shot();
        ThrowingGrenade grenade = Instantiate(throwingGrenade, spawnPointer.position, Quaternion.identity);
        grenade.Throw(ray.direction * forceThrow);
        yield return new WaitForSeconds(1.3f);
        isPlaying = false;
    }

    private void OnEnable()
    {
        isPlaying = false;
    }
}
