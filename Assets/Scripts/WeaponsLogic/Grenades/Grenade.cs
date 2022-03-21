using System.Collections;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public abstract class Grenade : AmmoManager
{
    [SerializeField, Range(1f, 10f)] protected float timeToExplosion;

    protected Rigidbody body;

    protected virtual IEnumerator Throwing()
    {
        yield return new WaitForSeconds(timeToExplosion);
        body.isKinematic = true;
        StartCoroutine(Explosion());
    }

    public void Throw(Vector3 direction)  // будет private
    {
        body.AddForce(direction);
        StartCoroutine(Throwing());
    }

    public override void UseWepon(Ray ray)
    {
        if (CheckCountAmmo())
            print("kek");
    }


    protected abstract IEnumerator Explosion();

    private void Awake()
    {
        body = GetComponent<Rigidbody>();
    }
}
