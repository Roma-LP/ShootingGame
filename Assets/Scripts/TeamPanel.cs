using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[DisallowMultipleComponent]
public class TeamPanel : MonoBehaviour
{
    [SerializeField] Team team;

    public Team Team { get => team; }
}
