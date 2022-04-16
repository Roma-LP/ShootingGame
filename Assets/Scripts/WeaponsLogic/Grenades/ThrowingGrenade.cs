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

    public void RPC_Throw(Vector3 direction, int opponentID)
    {
        grenadePV.RPC("Throw", RpcTarget.All, direction, opponentID);
    }


    [PunRPC]
    public void Throw(Vector3 direction, int opponentID) 
    {
        body.AddForce(direction);
        StartCoroutine(Throwing(opponentID));
    }

    protected virtual IEnumerator Throwing(int opponentID)
    {
        yield return new WaitForSeconds(timeToExplosion);
        body.isKinematic = true;
        StartCoroutine(Explosion(opponentID));
    }
    protected abstract IEnumerator Explosion(int opponentID);
}