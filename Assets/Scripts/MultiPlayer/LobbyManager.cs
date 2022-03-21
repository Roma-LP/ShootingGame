using ExitGames.Client.Photon;
using Photon.Pun;
using Photon.Realtime;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using System.Linq;

public class LobbyManager : MonoBehaviourPunCallbacks
{
    [SerializeField] private TMP_InputField inputRoomName;
    [SerializeField] private TMP_Text CountPlayersMod;
    [SerializeField] private GameObject VC_LobbyCamera;
    [SerializeField] private GameObject VC_RoomCamera;
    [SerializeField] private TMP_Text createdRoomName;
    [SerializeField] private TMP_Text hostName;
    [SerializeField] private TMP_Text maxCountPlayersMod;

    //Room Pref in Lobby
    [SerializeField] private RoomInfoTab roomItemPrefab;
    [SerializeField] private Transform roomItemsContent;
    [SerializeField] private MapItemRoom mapItemRoom;
    [SerializeField] private MapIconsStore iconsStore;
    private RoomInfoTab lastTabSelected;
    private List<RoomInfoTab> roomItemsList = new List<RoomInfoTab>();
    private float timeBetweenUpdates = 1.5f;
    float nextUpdateTime;

    private void Start()
    {
        PhotonNetwork.JoinLobby();
    }
    public override void OnJoinedLobby()
    {
        base.OnJoinedLobby();
        print("Joined to lobby");
    }
    public void OnRoomInfoTabSelected(RoomInfoTab roomInfoTab)
    {
        if (lastTabSelected != roomInfoTab)
        {
            lastTabSelected?.DeSelect();
            lastTabSelected = roomInfoTab;
        }
    }
    public void OnClickCreate()
    {
        if (inputRoomName.text != string.Empty)
        {
            Hashtable customProperties = new Hashtable();
            customProperties.Add("Creator", PhotonNetwork.NickName);
            customProperties.Add("MapName", iconsStore.MapIcons[0].mapName);

            //set custom properties for display in room list.
            string[] customPropertiesForLobby = new string[2];
            customPropertiesForLobby[0] = "Creator";
            customPropertiesForLobby[1] = "MapName";

            RoomOptions options = new RoomOptions()
            {
                IsVisible = true,
                IsOpen = true,
                MaxPlayers = byte.Parse(CountPlayersMod.text),
                CustomRoomProperties = customProperties,
                CustomRoomPropertiesForLobby = customPropertiesForLobby,
            };
            PhotonNetwork.CreateRoom(inputRoomName.text, options);
        }
    }
    public override void OnCreatedRoom()
    {
        base.OnCreatedRoom();
        print("Room created");
    }
    public override void OnRoomPropertiesUpdate(Hashtable propertiesThatChanged)
    {
        base.OnRoomPropertiesUpdate(propertiesThatChanged);
        var roomInfo = roomItemsList.FirstOrDefault(t => t.MapName == PhotonNetwork.CurrentRoom.Name);
        if (roomInfo != null)
            roomInfo.ChangeMap((string)propertiesThatChanged["MapName"]);
    }

    public override void OnRoomListUpdate(List<RoomInfo> roomList)
    {
        if (Time.time >= nextUpdateTime)
        {
            foreach (RoomInfoTab item in roomItemsList)
            {
                Destroy(item.gameObject);
            }
            roomItemsList.Clear();

            foreach (var room in roomList)
            {

                RoomInfoTab newRoom = Instantiate(roomItemPrefab, roomItemsContent);
                newRoom.Ini(this, room.Name, (string)room.CustomProperties["Creator"], (string)room.CustomProperties["MapName"], PhotonNetwork.CountOfPlayersInRooms, room.MaxPlayers);
                roomItemsList.Add(newRoom);
            }
            nextUpdateTime = Time.time + timeBetweenUpdates;
        }
    }
    public void OnClickJoinRoom()
    {
        PhotonNetwork.JoinRoom(lastTabSelected.Name);
    }
    public void QuitGame()
    {
        Application.Quit();
    }
}
