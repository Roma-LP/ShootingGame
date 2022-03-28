using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using System;
using System.Linq;

public class GameManager : MonoBehaviour
{
    [SerializeField, ArrayElementTitle("team")] private TeamPoint[] teamPoints;
    [SerializeField] private CharacterPrefabNameStore characterPathStore;



    private void Awake()
    {
        // не забыть сделать синхронизацию спавна
        Vector3 spawnPosition = teamPoints.First(t => t.team == PhotonNetwork.LocalPlayer.CustomProperties.GetEnumInProperties<Team>("Team")).GetPoint();
        string characterPrefabName = characterPathStore[characterPathStore.GetIndexByEnum(PhotonNetwork.LocalPlayer.CustomProperties.GetEnumInProperties<CharacterType>("Character"))].value;
        GameObject player = PhotonNetwork.Instantiate(characterPrefabName, spawnPosition, Quaternion.identity);
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