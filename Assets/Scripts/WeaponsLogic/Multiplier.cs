using UnityEngine;

public class Multiplier : MonoBehaviour
{
    [SerializeField, Min(0f)] private float damageMultiplier;
    [SerializeField] private ThirdPersonShooterController shooterController;

    public void DetectedDamage(float damage, int opponentID)
    {
        shooterController.ReducingLife(damage * damageMultiplier, opponentID);
    }
}
