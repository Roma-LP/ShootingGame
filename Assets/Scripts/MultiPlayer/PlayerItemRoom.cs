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
    [SerializeField] private Sprite selectedSprite;
    [SerializeField] private Sprite normalSprite;
    [SerializeField] private Image image;
    [SerializeField] private Toggle isReadyToggle;
    [SerializeField] private CharactersIconStore charactersIcon;

    private Player player;

    public Player Player { get => player; }
    public Team Team => (Team)Enum.Parse(typeof(Team), (string)player.CustomProperties["Team"]);

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

    public void IniPlayer(string PlayerName, Player player)
    {
        this.playerName.text = PlayerName;
        //this.iconCharacter = ;
        this.player = player;
        isReadyToggle.interactable = player == PhotonNetwork.LocalPlayer;
        SetCharacter(player.CustomProperties.ContainsKey("Character") ? (CharacterType)Enum.Parse(typeof(CharacterType), (string)player.CustomProperties["Character"]) : CharacterType.Soldier);
        isReadyToggle.isOn = player.CustomProperties.ContainsKey("IsReady") ? bool.Parse((string)player.CustomProperties["IsReady"]) : false;
    }

    public void SetPlayerReady(bool isReady)
    {
        isReadyToggle.isOn = isReady;
    }

    public void SetCharacter(CharacterType character)
    {
        int index = charactersIcon.GetIndexByEnum(character);
        typeOfCharacter.text = character.ToString();
        iconCharacter.sprite = charactersIcon[index].iconSprite;
    }

    public void OnClickChooseCharacter()
    {

    }
}

public enum CharacterType
{
    Sniper = 0,
    Soldier = 1,
    Tank = 2
}
