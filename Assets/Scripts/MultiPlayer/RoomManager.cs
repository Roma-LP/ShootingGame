using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;
using ExitGames.Client.Photon;
using TMPro;

public class RoomManager : MonoBehaviourPunCallbacks
{
    //Players Pref in Room
    [SerializeField] private PlayerItemRoom playerItemRoomPref;
    [SerializeField] private Transform playerInBlueTeam;
    [SerializeField] private Transform playerInRedTeam;
    [SerializeField] private GameObject VC_LobbyCamera;
    [SerializeField] private GameObject VC_RoomCamera;
    [SerializeField] private TMP_Text createdRoomName;
    [SerializeField] private TMP_Text hostName;
    [SerializeField] private TMP_Text maxCountPlayersMod;
    [SerializeField] private GameObject playButton;

    private List<PlayerItemRoom> playersItemsList = new List<PlayerItemRoom>();
    private System.Random random;

    private void Start()
    {
        random = new System.Random();
    }

    public override void OnJoinedRoom()
    {
        VC_RoomCamera.SetActive(true);
        VC_LobbyCamera.SetActive(false);
        createdRoomName.text = "Room Name: " + PhotonNetwork.CurrentRoom.Name;
        maxCountPlayersMod.text = "Max.Players: " + PhotonNetwork.CurrentRoom.MaxPlayers;
        if (PhotonNetwork.IsMasterClient)
        {
            hostName.text = "Host: " + PhotonNetwork.NickName;
        }
        UpdatePlayersList();
    }
    public void OnClickLeaveRoom()
    {
        PhotonNetwork.LeaveRoom();
        VC_LobbyCamera.SetActive(true);
        VC_RoomCamera.SetActive(false);
    }

    public override void OnConnectedToMaster()
    {
        PhotonNetwork.JoinLobby();
    }

    public void UpdatePlayersList()
    {
        foreach (PlayerItemRoom item in playersItemsList)
        {
            Destroy(item.gameObject);
        }
        playersItemsList.Clear();

        if (PhotonNetwork.CurrentRoom == null)
        {
            return;
        }

        foreach (KeyValuePair<int, Player> player in PhotonNetwork.CurrentRoom.Players)
        {
            PlayerItemRoom newPlayerItem = Instantiate(playerItemRoomPref, random.Next(2) == 0 ? playerInBlueTeam : playerInRedTeam);
            newPlayerItem.IniPlayer(player.Value.NickName);
            playersItemsList.Add(newPlayerItem);

            if (PhotonNetwork.IsMasterClient)
            {
                hostName.text = "Host: " + PhotonNetwork.NickName;
            }

        }
    }
    public override void OnPlayerEnteredRoom(Player newPlayer)
    {
        UpdatePlayersList();
    }

    public override void OnPlayerLeftRoom(Player otherPlayer)
    {
        UpdatePlayersList();
    }

    private void Update()
    {
        if (PhotonNetwork.IsMasterClient && PhotonNetwork.CurrentRoom.PlayerCount >= 2)
        {
            playButton.SetActive(true);
        }
        else
        {
            playButton.SetActive(false);
        }
    }

    public void OnClickStartButton()
    {
        PhotonNetwork.LoadLevel("Map_1");
    }
}
