using System.Collections;
using UnityEngine;

[RequireComponent(typeof(Rigidbody), typeof(CapsuleCollider))]
public abstract class ThrowingGrenade : MonoBehaviour
{
    [SerializeField, Range(1f, 10f)] protected float timeToExplosion;

    protected Rigidbody body;

    protected virtual void Awake()
    {
        body = GetComponent<Rigidbody>();
    }

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
