using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Smoke : Grenade
{
    [SerializeField] private ParticleSystem smokePrefab;
    [SerializeField] private float minVelocity = 0.01f;
    [SerializeField] private float lifeSmoke;

    protected override IEnumerator Throwing()
    {
        yield return new WaitForSeconds(timeToExplosion);
        do
        {
            yield return new WaitForFixedUpdate();
        } while (body.velocity.magnitude > minVelocity);
        body.isKinematic = true;
        StartCoroutine(Explosion());
    }

    protected override IEnumerator Explosion()
    {
        var smoke = Instantiate(smokePrefab, transform.position, Quaternion.identity);
        var smokePartickeSystem = smoke.main;
        smokePartickeSystem.duration = lifeSmoke * smokePartickeSystem.simulationSpeed - smokePartickeSystem.startLifetimeMultiplier;
        smoke.Play();
        yield return new WaitForSeconds(lifeSmoke);
        Destroy(gameObject);
    }
}
