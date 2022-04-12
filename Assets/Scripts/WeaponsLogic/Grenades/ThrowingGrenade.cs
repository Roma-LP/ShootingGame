using System.Collections;
using UnityEngine;
using Photon.Pun;

[RequireComponent(typeof(Rigidbody), typeof(CapsuleCollider))]
public abstract class ThrowingGrenade : MonoBehaviourPun
{
    [SerializeField, Range(1f, 10f)] protected float timeToExplosion;

    private PhotonView grenadePV;

    protected Rigidbody body;

    protected virtual void Awake()
    {
        body = GetComponent<Rigidbody>();
        grenadePV = GetComponent<PhotonView>();
    }

    public void PhotonThrow(Vector3 direction)
    {
        grenadePV.RPC("Throw", RpcTarget.All, direction);
    }


    [PunRPC]
    public void Throw(Vector3 direction) 
    {
        //body.isKinematic = false;
       // body.useGravity = true;
        body.AddForce(direction);
        StartCoroutine(Throwing());
    }


    protected virtual IEnumerator Throwing()
    {
        yield return new WaitForSeconds(timeToExplosion);
        body.isKinematic = true;
        StartCoroutine(Explosion());
    }
    protected abstract IEnumerator Explosion();
}
