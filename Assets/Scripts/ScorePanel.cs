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
        if (changedProps.ContainsKey("Kills"))
        {
            var playerTab = playersItemsStatsList.First(t => t.Player == targetPlayer);
            playerTab.UpdateKills(changedProps.GetIntInProperties("Kills"));
        }
        if (changedProps.ContainsKey("Deaths"))
        {
            var playerTab = playersItemsStatsList.First(t => t.Player == targetPlayer);
            playerTab.UpdateDeaths(changedProps.GetIntInProperties("Deaths"));
        }
    }
}
