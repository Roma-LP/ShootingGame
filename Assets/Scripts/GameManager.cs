using Photon.Pun;
using Photon.Realtime;
using StarterAssets;
using System;
using System.Linq;
using UnityEngine;

public class GameManager : MonoBehaviourPunCallbacks
{
    [SerializeField, ArrayElementTitle("team")] private TeamPoint[] teamPoints;
    [SerializeField] private CharacterPrefabNameStore characterPathStore;
    [SerializeField] private ScorePanel scorePanelPrefab;

    private ScorePanel scorePanel;
    private StarterAssetsInputs starterAssetsInputs;
    private static GameManager instance;
    private string characterPrefabName;
    private Vector3 spawnPosition;

    public event Action<float> OnFlashbagEffect;

    private void Awake()
    {
        instance = this;
        // не забыть сделать синхронизацию спавна
        var props = PhotonNetwork.LocalPlayer.CustomProperties;
        props.ResetPropertyValue("Kills", 0);
        props.ResetPropertyValue("Deaths", 0);
        PhotonNetwork.LocalPlayer.SetCustomProperties(props);
        spawnPosition = teamPoints.First(t => t.team == PhotonNetwork.LocalPlayer.CustomProperties.GetEnumInProperties<Team>("Team")).GetPoint();
        characterPrefabName = characterPathStore[characterPathStore.GetIndexByEnum(PhotonNetwork.LocalPlayer.CustomProperties.GetEnumInProperties<CharacterType>("Character"))].value;
        Spawn();
    }

    public static GameManager Instance => instance;

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

    public void Spawn()
    {
        GameObject player = PhotonNetwork.Instantiate(characterPrefabName, spawnPosition, Quaternion.identity);
        starterAssetsInputs = player.GetComponent<StarterAssetsInputs>();
    }
    public void ShowFlashBagEffect(float flashTime)
    {
        OnFlashbagEffect?.Invoke(flashTime);
    }
}