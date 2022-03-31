using UnityEngine;

public class Multiplier : MonoBehaviour
{
    [SerializeField, Min(0f)] private float damageMultiplier;
    [SerializeField] private ThirdPersonShooterController shooterController;

    public void DetectedDamage(float damage)
    {
        shooterController.ReducingLife(damage * damageMultiplier);
    }
}
