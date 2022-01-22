using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
    public int xSize = MapGenerator.xSize;
    public int zSize = MapGenerator.zSize;

    public GameObject enemyPrefab;

    private void Start()
    {
        GenerateSpawnPoint();  
    }

    private Vector3 GenerateSpawnPoint()
    {
        Vector3 spawnPoint = new Vector3(Random.RandomRange(0, xSize), , Random.RandomRange(0, zSize));

        return spawnPoint;
    }

    void SpawnEnemy()
    {

    }
}
