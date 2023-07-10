using UnityEngine;
using System.Collections;

public class EnemySpawner : MonoBehaviour
{

    public GameObject[] enemies;
    int enemyNo;
    public float maxPos = 2.5f;
    public float spawnInterval = 3f;
    public float currentSpawnTime = 0;

    public float bigCountdown = 120; // 120 seconds is 2 minutes
    public float currentBigTime = 0;

    void Start()
    {

    }

    void Update()
    {
        currentSpawnTime += Time.deltaTime;
        currentBigTime += Time.deltaTime;

        if (currentSpawnTime >= spawnInterval)
        {
            spawn();
            currentSpawnTime = 0;
        }

        if (currentBigTime >= bigCountdown)
        {
            spawnInterval -= .1f;
            currentBigTime = 0;
        }
    }

    void spawn()
    {
        Vector3 enemyPos = new Vector3(Random.Range(-2.8f, 2.8f), transform.position.y, transform.position.z);
        enemyNo = Random.Range(0, 27);
        Instantiate(enemies[enemyNo], enemyPos, transform.rotation);
    }
}

