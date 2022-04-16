using UnityEngine;

public class Multiplier : MonoBehaviour
{
    [SerializeField, Min(0f)] private float damageMultiplier;
    [SerializeField] private HealthManager healthManager;

    public void DetectedDamage(float damage, int opponentID)
    {
        healthManager.RPC_ReducingLife(damage * damageMultiplier, opponentID);
    }
}
