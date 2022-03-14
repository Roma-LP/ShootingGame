using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Smoke : Grenade
{
    [SerializeField] private ParticleSystem smokeParticle;

    protected override IEnumerator Launch()
    {
        print(timeToExplosion);
        yield return new WaitForSeconds(timeToExplosion);
        //rigidbody.velocity = Vector3.zero;
        print("kek");
        smokeParticle.Play();
        //yield return new WaitForSeconds(smokeParticle);
        //Destroy(gameObject);
        print("lol");
    }
}
