using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class WaveSpawner : MonoBehaviour
{
    [System.Serializable]
    public class Wave
    {
        public Enemy[] enemies;
        public int count;
        public float timeBetweenSpawns;
    }

    public Wave[] waves;
    public Transform[] spawnPoints;
    public float timeBetweenWaves;

    private Wave currentWave;
    private int currentWaveIndex;
    private Transform player;

    private bool finishedWave;

    private void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player").transform;
        StartCoroutine(NextWave(currentWaveIndex));
    }

    IEnumerator NextWave(int index)
    {
        yield return new WaitForSeconds(timeBetweenWaves);
        StartCoroutine(SpawnWave(index));
    }

    IEnumerator SpawnWave(int index)
    {
        currentWave = waves[index];

        for (int i = 0; i < currentWave.count; i++)
        {
            if(player == null) yield break;

            Enemy randomEnemy = currentWave.enemies[Random.Range(0, currentWave.enemies.Length)];
            Transform randomSpot = spawnPoints[Random.Range(0, spawnPoints.Length)];
            Instantiate(randomEnemy, randomSpot.position, randomSpot.rotation);

            if (i == currentWave.count - 1)
            {
                finishedWave = true;
            }
            else
            {
                finishedWave = false;
            }

            yield return new WaitForSeconds(currentWave.timeBetweenSpawns);
        }
    }

    private void Update()
    {
        if (finishedWave && GameObject.FindGameObjectsWithTag("Enemy").Length == 0)
        {
            finishedWave = false;
            if (currentWaveIndex + 1 < waves.Length)
            {
                currentWaveIndex++;
                StartCoroutine(NextWave(currentWaveIndex));
            }
            else
            {
                Debug.Log("Game finished.");
            }
        }
    }
}
