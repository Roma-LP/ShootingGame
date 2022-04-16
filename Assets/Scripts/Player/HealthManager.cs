using UnityEngine;
using Photon.Pun;
using System.Linq;

public abstract class HealthManager : MonoBehaviourPun
{
    [SerializeField, Min(1f)] private float healthPoint = 100;
    [SerializeField] private PhotonView PV;
    [SerializeField] private HealthBarManager healthBarManager;

    private void Awake()
    {
        healthBarManager.SetMaxHP(HP);
    }

    public float HP
    {
        private set
        {
            healthPoint = value;
            if (healthPoint < 0)
            {
                healthPoint = 0;
            }
        }
        get
        {
            return healthPoint;
        }
    }

    public void RPC_ReducingLife(float damage, int opponentID)
    {
        PV.RPC("ReducingLife", RpcTarget.All, damage, opponentID);
    }

    [PunRPC]
    public void ReducingLife(float damage, int opponentID)
    {
        if (!PV.IsMine) return;

        HP = HP - damage;
        if (HP <= 0f)
        {
            Dead();
            var props = PhotonNetwork.PlayerList.First(x => x.ActorNumber == opponentID).CustomProperties;
            int countKilss = props.GetIntInProperties("Kills");
            props.ResetPropertyValue("Kills", ++countKilss);
        }
        healthBarManager.SetCurrentHP(HP);
    }

    private void Dead()
    {
        var props = PhotonNetwork.LocalPlayer.CustomProperties;
        int countDeaths = props.GetIntInProperties("Deaths");
        props.ResetPropertyValue("Deaths", ++countDeaths);
        PhotonNetwork.Destroy(gameObject);
        GameManager.Instance.Spawn();
    }
}