using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
    private int xSize = MapGenerator.xSize;
    private int zSize = MapGenerator.zSize;

    int randomX;
    int randomZ;

    public GameObject enemyPrefab;

    private int maxEnemies = 10;
    private int currentEnemies = 0;
    float delay;
    [SerializeField] float startDelay = 4;
    [SerializeField] float delayPercShorteningPerSecond = 1f;
    [SerializeField] float minSpawnRangeFromPlayer = 15f;

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

        //CHECK AGAINST PLAYER POS
        Transform playerTrans = GameObject.FindGameObjectWithTag("Player").transform;
        Vector2 playerPos2D = new Vector2(playerTrans.position.x, playerTrans.position.z);
        Vector2 spawnPos2D = new Vector2(randomX, randomZ);
        while (Vector2.Distance(playerPos2D, spawnPos2D) < minSpawnRangeFromPlayer){
            randomX = Random.Range(0, xSize);
            randomZ = Random.Range(0, zSize);
            spawnPos2D = new Vector2(randomX, randomZ);
        }

        //GENERATE SPAWNPOINT
        Vector3 spawnPoint = Vector3.zero;

        Vector3 randomPoint = new Vector3(randomX, 10000, randomZ);
        RaycastHit hit;

        if (Physics.Raycast(randomPoint, Vector3.down, out hit, Mathf.Infinity))
        {
            randomPoint.y = hit.transform.position.y + 10;
            spawnPoint = randomPoint;
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
