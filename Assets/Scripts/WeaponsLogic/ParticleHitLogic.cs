using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ParticleHitLogic : MonoBehaviour
{
    private void OnEnable()
    {
        StartCoroutine(WaitForDestroy());
    }

    IEnumerator WaitForDestroy()
    {
        yield return new WaitForSeconds(2f);
        Destroy(gameObject);
    }
}
