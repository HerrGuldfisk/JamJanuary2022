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
        Vector3 randomPoint = new Vector3(Random.Range(1, xSize-1), -10, Random.Range(1, zSize-1));
        Debug.Log(randomPoint);
        RaycastHit hit;

        if (Physics.Raycast(randomPoint, new Vector3(0, 0, 1), out hit, Mathf.Infinity))
        {
            randomPoint.y = hit.transform.position.y + 2;
            spawnPoint = randomPoint;
        }
        else
        {
            spawnPoint = new Vector3(xSize / 2, 1, zSize / 2);
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
