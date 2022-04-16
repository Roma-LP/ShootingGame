using System.Collections;
using UnityEngine;
using Photon.Pun;

public class GrenadeManager : AmmoManagerGrenade
{
    [SerializeField] private string grenadeName;
    [SerializeField] private Transform spawnPointer;
    [SerializeField, Min(1f)] protected float forceThrow;

    private bool isPlaying;
    public override void UseWepon(Ray ray, int opponentID)
    {
        if (isPlaying == false && CheckCountAmmo())
        {
            StartCoroutine(Grenade(ray, opponentID));
            //TestGrenade(ray);
        }
    }

    IEnumerator Grenade(Ray ray, int opponentID)
    {
        isPlaying = true;
        Shot();
        //ThrowingGrenade grenade = Instantiate(throwingGrenade, spawnPointer.position, Quaternion.identity);
        ThrowingGrenade grenade = PhotonNetwork.Instantiate(grenadeName, spawnPointer.position, Quaternion.identity).GetComponent<ThrowingGrenade>();
        grenade.RPC_Throw(ray.direction * forceThrow, opponentID);
        yield return new WaitForSeconds(1.3f);
        isPlaying = false;
    }

    private void OnEnable()
    {
        isPlaying = false;
    }
}


