using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class corotin : MonoBehaviour
{
    [SerializeField, Min(0f)] private float rateOfFire=5f;
    private bool isUsingWeapon;

    public virtual void UseWepon()
    {
        if (!isUsingWeapon)
        {
            StartCoroutine(UsingWeapon());
            //audioSource.PlayOneShot(audioClipExplosion);  // пока нет звуков
        }
    }

    private IEnumerator UsingWeapon()
    {
        isUsingWeapon = true;
        print("DO");
        yield return new WaitForSeconds(rateOfFire);
        print("POSLE");
        isUsingWeapon = false;
    }
}
