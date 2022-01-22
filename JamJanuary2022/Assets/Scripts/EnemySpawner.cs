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
    private int currentEnemies = 0;
    float delay;
    [SerializeField] float startDelay = 4;
    [SerializeField] float delayPercShorteningPerSecond = 1f;

    private void Start()
    {
        StartCoroutine(SpawnEnemies());
        StartCoroutine(DecreaseSpawnDelay());
        delay = startDelay;
    }

    private Vector3 GenerateSpawnPoint()
    {
        randomX = Random.Range(0, xSize);
        randomZ = Random.Range(0, zSize);

        Vector3 spawnPoint = Vector3.zero;

        Vector3 randomPoint = new Vector3(randomX, 10000, randomZ);
        RaycastHit hit;

        if (Physics.Raycast(randomPoint, Vector3.down, out hit, Mathf.Infinity))
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
        while(currentEnemies < maxEnemies)
        {
            SpawnEnemy();
            yield return new WaitForSeconds(delay);
        }
    }

    IEnumerator DecreaseSpawnDelay(){
        while(true){
            delay -= delay*delayPercShorteningPerSecond/100;
            yield return new WaitForSeconds(1);
        }
    }
}
