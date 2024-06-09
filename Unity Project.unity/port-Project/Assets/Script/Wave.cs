using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Wave : MonoBehaviour
{

    [SerializeField] private float countdown;
    [SerializeField] moreWaves[] waves;
    [SerializeField] private GameObject spawnPoint;


    private int waveIndex = 0;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
        countdown -= Time.deltaTime;

        if (countdown <= 0)
        {
            SpawnWave();
        }
    }

    private void SpawnWave()
    {
        for (int i = 0; i < waves[waveIndex].enemies.Length; i++)
        {
            Instantiate(waves[waveIndex].enemies[i], spawnPoint.transform);
        }
    }
}

[System.Serializable]

public class moreWaves
{
    public enemyAI[] enemies;
}
