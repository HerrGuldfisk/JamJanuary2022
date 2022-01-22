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
        Vector3 spawnPoint = Vector3.zero;
        Vector3 randomPoint = new Vector3(Random.Range(0, xSize), -100, Random.Range(0, zSize));
        RaycastHit hit;

        if (Physics.Raycast(randomPoint, Vector3.up, out hit, Mathf.Infinity))
        {
            randomPoint.y = hit.transform.position.y + 1;
            spawnPoint = randomPoint;
        }

        return spawnPoint;
    }

    void SpawnEnemy()
    {

    }
}
