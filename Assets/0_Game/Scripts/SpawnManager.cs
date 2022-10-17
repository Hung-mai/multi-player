using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnManager : MonoBehaviour
{
    public static SpawnManager ins;
    public List<Transform> spawnPoints;

    private void Awake()
    {
        ins = this;
    }

    public Transform GetSpawnPoint()
    {
        int random = Random.Range(0, spawnPoints.Count);

        Transform transformToGet = spawnPoints[random];
        spawnPoints.Remove(transformToGet);
        return transformToGet;
    }
}
