using System.Collections;
using UnityEngine;

[RequireComponent(typeof(AudioSource), typeof(Rigidbody))]
public abstract class Grenade : MonoBehaviour
{
    [SerializeField] protected AudioClip audioClipExplosion;
    [SerializeField, Range(1f, 10f)] protected float timeToExplosion;

    protected Rigidbody body;

    private AudioSource audioSource;

    protected virtual IEnumerator Throwing()
    {
        yield return new WaitForSeconds(timeToExplosion);
        body.isKinematic = true;
        StartCoroutine(Explosion());
    }

    public void Throw(Vector3 direction)
    {
        body.AddForce(direction);
        StartCoroutine(Throwing());
    }

    protected abstract IEnumerator Explosion();

    private void Awake()
    {
        body = GetComponent<Rigidbody>();
        audioSource = GetComponent<AudioSource>();
    }
}
