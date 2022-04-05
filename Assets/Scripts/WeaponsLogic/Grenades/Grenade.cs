using System.Collections;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public abstract class Grenade : AmmoManagerGrenade
{
    [SerializeField, Range(1f, 10f)] protected float timeToExplosion;
    //[SerializeField, Min(1f)] protected float forceThrow;
    private void Awake()
    {
        body = GetComponent<Rigidbody>();
    }

    public void Throw(Vector3 direction)  // будет private
    {
        //Shot();
       
            body.isKinematic = false;
            body.useGravity = true;
            body.AddForce(direction);
            StartCoroutine(Throwing());
        
    }

    public void kek() => currentAmmo--; // rename?

    protected Rigidbody body;

    protected virtual IEnumerator Throwing()
    {
        yield return new WaitForSeconds(timeToExplosion);
        body.isKinematic = true;
        StartCoroutine(Explosion());
    }
    protected abstract IEnumerator Explosion();

    public override void UseWepon(Ray ray, int opponentID)
    {
        //Throw(ray.direction * forceThrow);
        Throw(ray.direction);
    }
}
