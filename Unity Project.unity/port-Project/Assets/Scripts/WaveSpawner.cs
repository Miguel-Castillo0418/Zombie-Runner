using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WaveSpawner : MonoBehaviour
{
    [SerializeField] GameObject objectToSpawn;
    [SerializeField] int numToSpawn;
    [SerializeField] int spawnTimer;
    [SerializeField] Transform[] spawnPos;


    int spawnCount;
    bool isSpawning;
    bool startSpawning;
    int numKilled; 



    // Update is called once per frame
    void Update()
    {
        if (startSpawning && !isSpawning && spawnCount < numToSpawn)
        {
            StartCoroutine(spawn());
        }
    }

   public void startWave()
    {
        startSpawning = true;
        gameManager.instance.updateGameGoal(numToSpawn);

    }

    IEnumerator spawn()
    {
        isSpawning = true;
        int arrayPos = Random.Range(0, spawnPos.Length);
        GameObject objectSpawned = Instantiate(objectToSpawn, spawnPos[arrayPos].position, spawnPos[arrayPos].rotation);

        if (objectSpawned.GetComponent<EnemyAI>())
        {
            objectSpawned.GetComponent<EnemyAI>().whereISpawned = this;
        }

        spawnCount++;
        yield return new WaitForSeconds(spawnTimer);
        isSpawning = false;
    }

    public void updateEnemyNumber()
    {
        numKilled++;

        if (numKilled >= numToSpawn) 
        {
            startSpawning = false;
            StartCoroutine(WaveManager.instance.StartWave());
        }
    }
}
