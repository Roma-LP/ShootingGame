using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GrenadeSpawner : MonoBehaviour
{
    [SerializeField] private GrenadeType grenadeType;

}

public enum GrenadeType
{
    Smoke, Flash, HE
}