using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using Photon.Pun;
using Photon.Realtime;
using System;

public class PlayerItemStats : MonoBehaviour
{
    [SerializeField] private TMP_Text playerName;
    [SerializeField] private Image iconCharacter;
    [SerializeField] private TMP_Text typeOfCharacter;
    [SerializeField] private CharactersIconStore charactersIcon;
    [SerializeField] private TMP_Text countKills;
    [SerializeField] private TMP_Text countDeaths;

    private Player player;

    public Player Player { get => player; }
    public Team Team => (Team)Enum.Parse(typeof(Team), (string)player.CustomProperties["Team"]);

    public void IniPlayerInStats(Player player)
    {
        playerName.text = player.NickName;
        this.player = player;
        CharacterType character = player.CustomProperties.GetEnumInProperties<CharacterType>("Character");
        typeOfCharacter.text = character.ToString();
        iconCharacter.sprite = charactersIcon[charactersIcon.GetIndexByEnum(character)].value;
        countKills.text = player.CustomProperties.GetIntInProperties("Kills").ToString();
        countDeaths.text = player.CustomProperties.GetIntInProperties("Deaths").ToString();
    }

    public void UpdateKills(int killsCount)
    {
        countKills.text = killsCount.ToString();
    }

    public void UpdateDeaths(int deathsCount)
    {
        countDeaths.text = deathsCount.ToString();
    }
}
