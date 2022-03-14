using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(AudioSource), typeof(Rigidbody))]
public abstract class Grenade : MonoBehaviour
{
    [SerializeField] protected AudioClip audioClipExplosion;
    [SerializeField, Range(1f, 10f)] protected float timeToExplosion;

    private AudioSource audioSource;

    abstract protected IEnumerator Launch();

    private void Awake()
    {
        audioSource = GetComponent<AudioSource>();  
    }

    private void OnEnable()
    {
        print("qwerty");
        StartCoroutine(Launch());
    }
}
