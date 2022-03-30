using System.Collections;
using UnityEngine;

public class FlashCanvasManager : MonoBehaviour
{
    [SerializeField] private Animator animator;

    private void Awake()
    {
        animator = GetComponent<Animator>();
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
