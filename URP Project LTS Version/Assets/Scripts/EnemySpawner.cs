using UnityEngine;
using System.Collections;
using System;

public class EnemySpawner : MonoBehaviour
{

    public GameObject theEnemy;
    public int xPos1;
    public int zPos1;
    public int xPos2;
    public int zPos2;
    public int xPos3;
    public int zPos3;
    public int xPos4;
    public int zPos4;
    public int enemyCount;

    public float maxPos = 2.5f;
    public float spawnInterval = 3f;
    public float currentSpawnTime = 0;

    public float bigCountdown = 120;
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
            StartCoroutine(EnemyDrop());
            currentSpawnTime = 0;
        }

        if (currentBigTime >= bigCountdown && spawnInterval > 1.5f)
        {
            spawnInterval -= .1f;
            currentBigTime = 0;
        }
    }

    IEnumerator EnemyDrop()
    {
        while( enemyCount < 10)
        {
            xPos1 = UnityEngine.Random.Range(-42, 42);
            zPos1 = UnityEngine.Random.Range(-40, -42);
            xPos2 = UnityEngine.Random.Range(-40, -42);
            zPos2 = UnityEngine.Random.Range(-42, 42);
            xPos3 = UnityEngine.Random.Range(-42, 42);
            zPos3 = UnityEngine.Random.Range(40, 42);
            xPos4 = UnityEngine.Random.Range(40, 42);
            zPos4 = UnityEngine.Random.Range(-42, 42);
            Instantiate(theEnemy, new Vector3(xPos1, 0, zPos1), Quaternion.identity);
            Instantiate(theEnemy, new Vector3(xPos2, 0, zPos2), Quaternion.identity);
            Instantiate(theEnemy, new Vector3(xPos3, 0, zPos3), Quaternion.identity);
            Instantiate(theEnemy, new Vector3(xPos4, 0, zPos4), Quaternion.identity);
            yield return new WaitForSeconds(3f);
            enemyCount += 1;
        }
    }
}

