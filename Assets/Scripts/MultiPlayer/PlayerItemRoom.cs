using Photon.Pun;
using Photon.Realtime;
using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class PlayerItemRoom : MonoBehaviour
{
    [SerializeField] private TMP_Text playerName;
    [SerializeField] private Image iconCharacter;
    [SerializeField] private TMP_Text typeOfCharacter;
    [SerializeField] private Toggle isReadyToggle;
    [SerializeField] private CharactersIconStore charactersIcon;

    private Player player;

    public Player Player { get => player; }
    public Team Team => player.CustomProperties.GetEnumInProperties<Team>("Team");

    private void Awake()
    {
        System.Random random = new System.Random();
    }

    public void OnTogglePressed(bool value)
    {
        var prop = player.CustomProperties;
        if (bool.Parse((string)prop["IsReady"]) != value)
        {
            prop["IsReady"] = value.ToString();
            player.SetCustomProperties(prop);
        }
    }

    public void IniPlayer(Player player)
    {
        this.playerName.text = player.NickName;
        this.player = player;
        isReadyToggle.interactable = player == PhotonNetwork.LocalPlayer;
        SetCharacter(player.CustomProperties.ContainsKey("Character") ? player.CustomProperties.GetEnumInProperties<CharacterType>("Character") : CharacterType.Soldier);
        isReadyToggle.isOn = player.CustomProperties.ContainsKey("IsReady") ? player.CustomProperties.GetBoolInProperties("IsReady") : false;
    }

    public void SetPlayerReady(bool isReady)
    {
        isReadyToggle.isOn = isReady;
    }

    public void SetCharacter(CharacterType character)
    {
        int index = charactersIcon.GetIndexByEnum(character);
        typeOfCharacter.text = character.ToString();
        iconCharacter.sprite = charactersIcon[index].value;
    }

}

public enum CharacterType
{
    Sniper = 0,
    Soldier = 1,
    Tank = 2
}
