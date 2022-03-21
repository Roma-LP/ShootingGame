using Photon.Pun;
using System;
using System.Collections;
using TMPro;
using UnityEngine;


public class ConnectToServer : MonoBehaviourPunCallbacks
{
    [SerializeField] private GameObject CmToLobby;
    [SerializeField] private TMP_InputField userNameInput;
    [SerializeField] private TMP_Text buttonText;

    private Color currentColor;
    private bool isCoroutineStart;

    private void Awake()
    {
        if (PlayerPrefs.HasKey("NickName"))
        {
            userNameInput.text = PlayerPrefs.GetString("NickName");
        }
    }
    public void OnClickConnect()
    {
       
        if (!String.IsNullOrEmpty(userNameInput.text))
        {
            PhotonNetwork.NickName = userNameInput.text;
            PlayerPrefs.SetString("NickName", userNameInput.text);
            CmToLobby.SetActive(true);
        }
        else
        {
            if(!isCoroutineStart)
                StartCoroutine(BlinkEmptyNickName());
        }
    }


    IEnumerator BlinkEmptyNickName()
    {
        isCoroutineStart = true;
        currentColor = userNameInput.image.color;
        userNameInput.image.color = Color.red;
        yield return new WaitForSeconds(0.3f);
        userNameInput.image.color = currentColor;
        yield return new WaitForSeconds(0.3f);
        userNameInput.image.color = Color.red;
        yield return new WaitForSeconds(0.3f);
        userNameInput.image.color = currentColor;
        isCoroutineStart=false;
    }
}
