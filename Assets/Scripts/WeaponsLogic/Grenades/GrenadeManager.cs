using System.Collections;
using UnityEngine;
using Photon.Pun;

public class GrenadeManager : AmmoManagerGrenade
{
    [SerializeField] private ThrowingGrenade throwingGrenade;
    [SerializeField] private string grenadeName;
    [SerializeField] private Transform spawnPointer;
    [SerializeField, Min(1f)] protected float forceThrow;

    private bool isPlaying;
    public override void UseWepon(Ray ray, int opponentID)
    {
        if (isPlaying == false && CheckCountAmmo())
        {
            StartCoroutine(Grenade(ray));
            //TestGrenade(ray);
        }
    }

    IEnumerator Grenade(Ray ray)
    {
        isPlaying = true;
        Shot();
        //ThrowingGrenade grenade = Instantiate(throwingGrenade, spawnPointer.position, Quaternion.identity);
        ThrowingGrenade grenade = PhotonNetwork.Instantiate(grenadeName, spawnPointer.position, Quaternion.identity).GetComponent<ThrowingGrenade>();
        grenade.PhotonThrow(ray.direction * forceThrow);
        yield return new WaitForSeconds(1.3f);
        isPlaying = false;
    }

    private void OnEnable()
    {
        isPlaying = false;
    }
}


