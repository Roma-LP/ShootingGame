using ExitGames.Client.Photon;
using Photon.Pun;
using Photon.Realtime;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using System.Linq;
using System;

public enum Team { BlueTeam = 0, RedTeam = 1}

public class RoomManager : MonoBehaviourPunCallbacks
{
    //Players Pref in Room
    [SerializeField] private PlayerItemRoom playerItemRoomPref;
    [SerializeField] private TeamPanel[] teamPanels;
    [SerializeField] private GameObject VC_LobbyCamera;
    [SerializeField] private GameObject VC_RoomCamera;
    [SerializeField] private TMP_Text createdRoomName;
    [SerializeField] private TMP_Text hostName;
    [SerializeField] private TMP_Text maxCountPlayersMod;
    [SerializeField] private GameObject playButton;
    [SerializeField] private MapItemRoom mapItem;

    private List<PlayerItemRoom> playersItemsList = new List<PlayerItemRoom>();
    private System.Random random;

    private void Start()
    {
        random = new System.Random();
    }

    private bool HasUnselectedTeams()
    {
        return HasUnselectedTeams(out IEnumerable<Team> selectedTeams, out IEnumerable<Team> unselectedTeams);
    }

    private bool HasUnselectedTeams(out IEnumerable<Team> selectedTeams, out IEnumerable<Team> unselectedTeams)
    {
        List<Team> teams = new List<Team>(Enum.GetNames(typeof(Team)).Select(t => new { team = (Team)Enum.Parse(typeof(Team), t) }).Select(t => t.team));
        selectedTeams = PhotonNetwork.CurrentRoom.Players.Select(t => t.Value).Where(t => t.CustomProperties.ContainsKey("Team")).Select(t => t.CustomProperties.GetEnumInProperties<Team>("Team"));
        unselectedTeams = teams.Except(selectedTeams);
        return unselectedTeams.Count() != 0;
    }

    public override void OnJoinedRoom()
    {
        VC_RoomCamera.SetActive(true);
        VC_LobbyCamera.SetActive(false);
        createdRoomName.text = "Room Name: " + PhotonNetwork.CurrentRoom.Name;
        maxCountPlayersMod.text = "Max.Players: " + PhotonNetwork.CurrentRoom.MaxPlayers;
        var prop = PhotonNetwork.LocalPlayer.CustomProperties;
        prop.ResetPropertyValue("IsReady", false);
        prop.ResetPropertyValue("Character", CharacterType.Soldier);
        Team team;
        bool hasUnselectedTeams = HasUnselectedTeams(out IEnumerable<Team> selectedTeams, out IEnumerable<Team> unselectedTeams);
        if (hasUnselectedTeams)
        {
            team = unselectedTeams.First();
        }
        else
        {
            team = selectedTeams.GroupBy(t => t, (key, value) => new
            {
                team = key,
                TeamCount = value.Count()
            }).Aggregate((a, b) => a.TeamCount < b.TeamCount ? a : b).team;
        }
        prop.ResetPropertyValue("Team", team);
        PhotonNetwork.LocalPlayer.SetCustomProperties(prop);
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
            if (!player.Value.CustomProperties.ContainsKey("Team"))
                continue;
            Team team = player.Value.CustomProperties.GetEnumInProperties<Team>("Team");
            PlayerItemRoom newPlayerItem = Instantiate(playerItemRoomPref, teamPanels.First(t => t.Team == team).transform);
            newPlayerItem.IniPlayer(player.Value);
            playersItemsList.Add(newPlayerItem);
        }
        if (PhotonNetwork.IsMasterClient)
        {
            var prop = PhotonNetwork.CurrentRoom.CustomProperties;
            prop["Creator"] = PhotonNetwork.NickName;
            PhotonNetwork.CurrentRoom.SetCustomProperties(prop);
            mapItem.IsEnableToEdit = true;
        }
        else
            mapItem.IsEnableToEdit = false;
    }
    public override void OnPlayerEnteredRoom(Player newPlayer)
    {
        UpdatePlayersList();
    }

    public override void OnPlayerLeftRoom(Player otherPlayer)
    {
        UpdatePlayersList();
    }

    public void OnClickStartButton()
    {
        PhotonNetwork.CurrentRoom.IsVisible = false;
        PhotonNetwork.LoadLevel("Map_1");
    }

    public override void OnRoomPropertiesUpdate(Hashtable propertiesThatChanged)
    {
        base.OnRoomPropertiesUpdate(propertiesThatChanged);
        if (propertiesThatChanged.ContainsKey("MapName"))
        {
            MapType map = propertiesThatChanged.GetEnumInProperties<MapType>("MapName");
            mapItem.SetMap(map);
        }
        if (propertiesThatChanged.ContainsKey("Creator"))
        {
            hostName.text = "Host: " + (string)propertiesThatChanged["Creator"];
        }
    }

    public override void OnPlayerPropertiesUpdate(Player targetPlayer, Hashtable changedProps)
    {
        base.OnPlayerPropertiesUpdate(targetPlayer, changedProps);
        if (changedProps.ContainsKey("Team"))
        {
            UpdatePlayersList();
        }
        if (changedProps.ContainsKey("Character"))
        {
            var playerTab = playersItemsList.First(t => t.Player == targetPlayer);
            playerTab.SetCharacter(changedProps.GetEnumInProperties<CharacterType>("Character"));
        }
        if (changedProps.ContainsKey("IsReady"))
        {
            var players = PhotonNetwork.CurrentRoom.Players.Select(t => t.Value);
            var playerTab = playersItemsList.First(t => t.Player == targetPlayer);
            playerTab.SetPlayerReady(changedProps.GetBoolInProperties("IsReady"));
            playButton.SetActive(PhotonNetwork.IsMasterClient
                && !HasUnselectedTeams()
                && PhotonNetwork.CurrentRoom.Players.All(t => t.Value.CustomProperties.GetBoolInProperties("IsReady")));
        }
    }

    public void ChangeTeam(int team)
    {
        Team selectedTeam = (Team)team;
        int playersCount = PhotonNetwork.CurrentRoom.Players.Select(t => t.Value).Where(t => t.CustomProperties.ContainsKey("Team")).Where(t => t.CustomProperties.GetEnumInProperties<Team>("Team") == selectedTeam).Count();
        if (playersCount < PhotonNetwork.CurrentRoom.MaxPlayers / 2)
        {
            var prop = PhotonNetwork.LocalPlayer.CustomProperties;
            prop["Team"] = ((Team)team).ToString();
            prop["IsReady"] = false.ToString();
            PhotonNetwork.LocalPlayer.SetCustomProperties(prop);
        }
    }

    public void ChangeCharacter(int character)
    {
        var prop = PhotonNetwork.LocalPlayer.CustomProperties;
        prop["Character"] = ((CharacterType)character).ToString();
        PhotonNetwork.LocalPlayer.SetCustomProperties(prop);
    }
    public override void OnLeftRoom()
    {
        base.OnLeftRoom();
        PhotonNetwork.LocalPlayer.SetCustomProperties(new Hashtable());
    }
}
