using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class ChangeCountPlayersMod : MonoBehaviour
{
    [SerializeField] TMP_Text CountPlayersMod;
    [SerializeField] public int CountPlayers = 4;

    private void Awake()
    {
        CountPlayersMod.text = CountPlayers.ToString();
    }

    public void AddCountOfPlayers()
    {
        if (CountPlayers != 8)
        {
            CountPlayers += 2;
            CountPlayersMod.text = CountPlayers.ToString();
        }
    }

    public void ReduceCountOfPlayers()
    {
        if (CountPlayers != 2)
        {
            CountPlayers -= 2;
            CountPlayersMod.text = CountPlayers.ToString();
        }
    }
}
