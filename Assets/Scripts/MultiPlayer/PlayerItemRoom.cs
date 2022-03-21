using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System;

public class PlayerItemRoom : MonoBehaviour
{
    [SerializeField] private TMP_Text playerName;
    [SerializeField] private Image iconCharacter;
    [SerializeField] private TMP_Text typeOfCharacter;
    [SerializeField] Sprite selectedSprite;
    [SerializeField] Sprite normalSprite;
    [SerializeField] Image image;

    private CharacterType characterType;

    private void Awake()
    {
        System.Random random = new System.Random();
        characterType = (CharacterType)random.Next(Enum.GetNames(typeof(CharacterType)).Length);
    }

    public void IniPlayer(string PlayerName)
    {
        this.playerName.text = PlayerName;
        //this.iconCharacter = ;
        this.typeOfCharacter.text = characterType.ToString();
    }

    public void OnClickChooseCharacter()
    {

    }

   
}

public enum CharacterType
{
    Tank,
    Solder,
    Sniper
}
