using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class RoomInfoTab : MonoBehaviour
{
    [SerializeField] Sprite selectedSprite;
    [SerializeField] Sprite normalSprite;
    [SerializeField] Image image;
    [SerializeField] TMP_Text roomName;
    [SerializeField] TMP_Text hostName;
    [SerializeField] TMP_Text countPlayersInRoom;
    [SerializeField] TMP_Text mapName;

    private LobbyManager lobbyManager;

    public string Name { get; protected set; }
    public string HostName { get; protected set; }
    public int CurrentCountPlayersInRoom { get; protected set; }
    public int MaxCountPlayersInRoom { get; protected set; }
    public string MapName { get; protected set; }

    private void Start()
    {
        lobbyManager = FindObjectOfType<LobbyManager>();
    }

    public void Ini(LobbyManager lobby, string roomName, string hostPlayerName, string mapName, int currentPlayersInRoom, int maxCountPlayers)
    {
        lobbyManager = lobby;

        this.roomName.text = roomName;
        Name = roomName;

        this.hostName.text = hostPlayerName;
        HostName = hostPlayerName;

        CurrentCountPlayersInRoom = 1;
        this.countPlayersInRoom.text = currentPlayersInRoom + "/" + maxCountPlayers;
        MaxCountPlayersInRoom = maxCountPlayers;

        this.CurrentCountPlayersInRoom = currentPlayersInRoom;
        this.MaxCountPlayersInRoom = maxCountPlayers;

        this.mapName.text = mapName;
        MapName = this.mapName.text;
    }

    public void ChangeMap(string mapNameString)
    {
        mapName.text = mapNameString;
    }

    public void Select()
    {
        image.sprite = selectedSprite;
        lobbyManager.OnRoomInfoTabSelected(this);
    }

    public void DeSelect()
    {
        image.sprite = normalSprite;
    }
}
