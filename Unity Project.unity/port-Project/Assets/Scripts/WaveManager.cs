using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WaveManager : MonoBehaviour
{

    public static WaveManager instance;
    public WaveSpawner[] spawners;
    [SerializeField] int timeBetweenWaves;

    public int waveCurrent;


    // Start is called before the first frame update
    void Awake()
    {
        instance = this;  
        StartCoroutine(startWave());
    }

    public IEnumerator startWave()
    {
        waveCurrent++;

        if (waveCurrent <= spawners.Length)
        {
            yield return new WaitForSeconds(timeBetweenWaves);
            spawners[waveCurrent - 1].startWave();
        }
    }
}