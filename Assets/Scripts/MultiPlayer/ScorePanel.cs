using UnityEngine;
using System.Linq;
using Photon.Pun;
using Photon.Realtime;
using ExitGames.Client.Photon;
using System.Collections.Generic;

public class ScorePanel : MonoBehaviourPunCallbacks
{
    [SerializeField] private TeamPanel[] teamPanels;
    [SerializeField] private PlayerItemStats playerItemPrefab;

    private List<PlayerItemStats> playersItemsStatsList = new List<PlayerItemStats>();

    private void Awake()
    {
        foreach (KeyValuePair<int, Player> player in PhotonNetwork.CurrentRoom.Players)
        {
            Team team = player.Value.CustomProperties.GetEnumInProperties<Team>("Team");
            PlayerItemStats newPlayerItem = Instantiate(playerItemPrefab, teamPanels.First(t => t.Team == team).transform);
            newPlayerItem.IniPlayerInStats(player.Value);
            playersItemsStatsList.Add(newPlayerItem);
        }
    }

    public override void OnPlayerPropertiesUpdate(Player targetPlayer, Hashtable changedProps)
    {
        base.OnPlayerPropertiesUpdate(targetPlayer, changedProps);
        var playerTab = playersItemsStatsList.First(t => t.Player == targetPlayer);
        if (changedProps.ContainsKey("Kills"))
        {
            playerTab.UpdateKills(changedProps.GetIntInProperties("Kills"));
        }
        if (changedProps.ContainsKey("Deaths"))
        {
            playerTab.UpdateDeaths(changedProps.GetIntInProperties("Deaths"));
        }
    }
}
