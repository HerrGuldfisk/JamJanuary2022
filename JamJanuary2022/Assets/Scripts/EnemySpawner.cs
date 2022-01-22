using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
    public int xSize = MapGenerator.xSize;
    public int zSize = MapGenerator.zSize;

    int randomX;
    int randomZ;

    public GameObject enemyPrefab;

    private int maxEnemies = 10;

    private void Start()
    {
        for (int i = 0; i < maxEnemies; i++)
        {
            //StartCoroutine(SpawnEnemies());
            SpawnEnemy();
        }
    }

    private Vector3 GenerateSpawnPoint()
    {
        randomX = Random.Range(0, xSize);
        randomZ = Random.Range(0, zSize);

        Vector3 spawnPoint = Vector3.zero;

        Vector3 randomPoint = new Vector3(randomX, -10, randomZ);
        Debug.Log(randomPoint);
        RaycastHit hit;

        if (Physics.Raycast(randomPoint, new Vector3(randomPoint.x, 1, randomPoint.y), out hit, Mathf.Infinity))
        {
            randomPoint.y = hit.transform.position.y + 10;
            spawnPoint = randomPoint;
            Debug.Log(spawnPoint);
        }
        else
        {
            spawnPoint = new Vector3(xSize / 2, 10, zSize / 2);
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
        yield return new WaitForSeconds(0.5f);
    }
}
