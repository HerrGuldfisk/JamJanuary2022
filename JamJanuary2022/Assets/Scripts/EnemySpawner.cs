using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
    public int xSize = MapGenerator.xSize;
    public int zSize = MapGenerator.zSize;

    public GameObject enemyPrefab;

    private int maxEnemies = 40;

    private void Start()
    {
        for (int i = 0; i < maxEnemies; i++)
        {
            StartCoroutine(SpawnEnemies());
        }
    }

    private Vector3 GenerateSpawnPoint()
    {
        Vector3 spawnPoint = Vector3.zero;
        Vector3 randomPoint = new Vector3(Random.Range(0 + xSize/10, xSize - xSize / 10), -100, Random.Range(0 + zSize / 10, zSize - zSize / 10));
        RaycastHit hit;

        if (Physics.Raycast(randomPoint, new Vector3(0,0,1), out hit, Mathf.Infinity))
        {
            randomPoint.y = hit.transform.position.y + 1;
            spawnPoint = randomPoint;
        }

        return spawnPoint;
    }

    void SpawnEnemy()
    {
        Instantiate(enemyPrefab, GenerateSpawnPoint(), Quaternion.identity);
    }

    IEnumerator SpawnEnemies()
    {
        SpawnEnemy();
        yield return new WaitForSeconds(0.1f);
    }
}
