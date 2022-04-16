using System.Collections;
using UnityEngine;

public class HighExplosion : ThrowingGrenade
{
    [SerializeField, Min(1f)] private float damageExplosion;
    [SerializeField] private float radius;
    [SerializeField] private LayerMask layerMask;
    [SerializeField] private ParticleSystem HEParticlePrefab;

    protected override IEnumerator Explosion(int opponentID)
    {
        var heParticle = Instantiate(HEParticlePrefab, transform.position, Quaternion.identity);
        RaycastHit[] raycastHits = Physics.SphereCastAll(transform.position, radius, transform.position, 1f, layerMask);
        foreach (RaycastHit hit in raycastHits)
        {
            if (hit.collider.TryGetComponent(out HealthManager healthManager))
            {
                healthManager.RPC_ReducingLife(damageExplosion, opponentID);
            }
        }
        Destroy(gameObject);
        yield return HEParticlePrefab.main.duration;
        Destroy(heParticle);
    }
}