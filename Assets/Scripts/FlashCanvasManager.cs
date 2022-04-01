using System.Collections;
using UnityEngine;

public class FlashCanvasManager : MonoBehaviour
{
    [SerializeField] private Animator animator;

    private void Start()
    {
        GameManager.Instance.OnFlashbagEffect += TriggerFlash;
    }

    private void OnDestroy()
    {
        GameManager.Instance.OnFlashbagEffect -= TriggerFlash;
    }

    private IEnumerator Flashing(float flashTime)
    {
        animator.SetTrigger("FlashOn");
        yield return new WaitForSeconds(flashTime);
        animator.SetTrigger("FlashOff");
    }

    public void TriggerFlash(float flashTime)
    {
        StartCoroutine(Flashing(flashTime));
    }
}
