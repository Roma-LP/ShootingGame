using Photon.Pun;
using Photon.Realtime;
using StarterAssets;
using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class GameManager : MonoBehaviourPunCallbacks
{
    [SerializeField, ArrayElementTitle("team")] private TeamPoint[] teamPoints;
    [SerializeField] private CharacterPrefabNameStore characterPathStore;
    [SerializeField] private ScorePanel scorePanelPrefab;

    private ScorePanel scorePanel;
    private StarterAssetsInputs starterAssetsInputs;

    private void Awake()
    {
        // не забыть сделать синхронизацию спавна
        var props = PhotonNetwork.LocalPlayer.CustomProperties;
        props.ResetPropertyValue("Kills", 0);
        props.ResetPropertyValue("Deaths", 0);
        PhotonNetwork.LocalPlayer.SetCustomProperties(props);
        Vector3 spawnPosition = teamPoints.First(t => t.team == PhotonNetwork.LocalPlayer.CustomProperties.GetEnumInProperties<Team>("Team")).GetPoint();
        string characterPrefabName = characterPathStore[characterPathStore.GetIndexByEnum(PhotonNetwork.LocalPlayer.CustomProperties.GetEnumInProperties<CharacterType>("Character"))].value;
        GameObject player = PhotonNetwork.Instantiate(characterPrefabName, spawnPosition, Quaternion.identity);
        starterAssetsInputs = player.GetComponent<StarterAssetsInputs>();
    }

    private void Update()
    {
        if (starterAssetsInputs.tab)
        {
            if (scorePanel == null)
                scorePanel = Instantiate(scorePanelPrefab);
        }
        else
        {
            if (scorePanel != null)
                Destroy(scorePanel.gameObject);
        }
    }

    [Serializable]
    public class TeamPoint
    {
        public Team team;
        public Transform[] spawnPoints;

        public Vector3 GetPoint()
        {
            // логика, чтобы игроки не спавнились в одном месте не забыть
            return spawnPoints[UnityEngine.Random.Range(0, spawnPoints.Length)].position;
        }
    }

    
}




