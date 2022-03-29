using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public abstract class ThrowingGrenade : MonoBehaviour
{
    [SerializeField, Range(1f, 10f)] protected float timeToExplosion;

    protected Rigidbody body;

    private void Awake()
    {
        body = GetComponent<Rigidbody>();
    }

    public void Throw(Vector3 direction)  // будет private
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
