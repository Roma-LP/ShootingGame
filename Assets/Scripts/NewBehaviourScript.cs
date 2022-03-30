using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NewBehaviourScript : MonoBehaviour
{
    int a = 5;
    private void Awake()
    {
        a = a++ + ++a;
        print(a);
    }
}
