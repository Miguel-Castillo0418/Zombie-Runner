using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WaveManager : MonoBehaviour
{
    public static WaveManager instance;

    [SerializeField] private List<Wave> waves;
    [SerializeField] private List<GameObject> spawnPoints;
    [SerializeField] private int totalRounds;
    [SerializeField] private List<EnemyMultiplier> enemyMultipliers;

    private int currentWaveIndex = 0;
    //private bool isSpawning = false;

    void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    void Start()
    {
        totalRounds = waves.Count;
    }

    public bool AllWavesCompleted()
    {
        return currentWaveIndex >= totalRounds;
    }

    public IEnumerator StartWave()
    {
        if (!AllWavesCompleted())
        {
            currentWaveIndex++;
            yield return StartCoroutine(SpawnWave());
        }
    }

    public void StartNextWave()
    {
        if (!AllWavesCompleted())
        {
            currentWaveIndex++;
            StartCoroutine(SpawnWave());
        }
    }

    private IEnumerator SpawnWave()
    {
       // isSpawning = true;
        Wave currentWave = waves[currentWaveIndex];

        for (int i = 0; i < currentWave.enemies.Length; i++)
        {
            int spawnPointIndex = Random.Range(0, spawnPoints.Count);
            GameObject spawnPoint = spawnPoints[spawnPointIndex];
            Instantiate(currentWave.enemies[i], spawnPoint.transform.position, spawnPoint.transform.rotation);
            yield return new WaitForSeconds(currentWave.spawnInterval);
        }

        //isSpawning = false;
    }

    public void OnDoorPurchased()
    {
        // Implement any logic needed when the door is purchased
    }

    [System.Serializable]
    public class Wave
    {
        public GameObject[] enemies;
        public float spawnInterval;
        public float nextWaveDelay;
        public int enemiesRemaining;
    }

    [System.Serializable]
    public class EnemyMultiplier
    {
        public GameObject enemyType;
        public int multiplier;
    }
}
