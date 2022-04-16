using System.Collections;
using UnityEngine;

public class Smoke : ThrowingGrenade
{
    [SerializeField] private ParticleSystem smokeParticlePrefab;
    [SerializeField] private float minVelocity = 0.01f;
    [SerializeField] private float lifeSmoke;

    protected override IEnumerator Throwing(int opponentID)
    {
        yield return new WaitForSeconds(timeToExplosion);
        do
        {
            yield return new WaitForFixedUpdate();
        } while (body.velocity.magnitude > minVelocity);
        body.isKinematic = true;
        StartCoroutine(Explosion(opponentID));
    }

    protected override IEnumerator Explosion(int opponentID)
    {
        var smoke = Instantiate(smokeParticlePrefab, transform.position, Quaternion.identity);
        var smokePartickeSystem = smoke.main;
        smokePartickeSystem.duration = lifeSmoke * smokePartickeSystem.simulationSpeed - smokePartickeSystem.startLifetimeMultiplier;
        smoke.Play();
        yield return new WaitForSeconds(lifeSmoke);
        Destroy(smoke);
        Destroy(gameObject);
    }
}
