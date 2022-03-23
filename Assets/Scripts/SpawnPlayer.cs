using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class SpawnPlayer : MonoBehaviour
{
    [SerializeField] private GameObject playerPrefab;
    public float minX, maxX, minZ, maxZ;

    private void Start()
    {
        Vector3 randomPosition = new Vector3(Random.Range(minX, maxX), 0, Random.Range(minZ, maxZ));
        Quaternion spawnRotation = Quaternion.identity;
        Instantiate(playerPrefab, randomPosition, spawnRotation);
    }
}
