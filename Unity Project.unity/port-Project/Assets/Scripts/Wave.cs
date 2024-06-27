using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Wave : MonoBehaviour
{
    static public Wave instance;
    [SerializeField] private float initialWaveCountdown;
    [SerializeField] private List<EnemyType> enemyTypes;
    [SerializeField] private List<GameObject> spawnPoints;
    [SerializeField] public int totalRounds;
    [SerializeField] private float spawnRateMultiplier;

    public int currentWaveIndex = 0;
    private float waveCountdown;
    private bool isCountingDown = true;
    //new code
    private string playerName;
    //private HighScoreTable highScoreTable;

    private List<EnemyAI> activeEnemies = new List<EnemyAI>();

    void Start()
    {
        waveCountdown = initialWaveCountdown;
        PrepareEnemyTypes();
        //new code
        playerName = PlayerPrefs.GetString("playerName", "Unknown");
        //highScoreTable = FindObjectOfType<HighScoreTable>();
    }

    void Update()
    {
        if (currentWaveIndex == totalRounds)
        {
            Debug.Log("test5");
            gameManager.instance.updateGameGoal(0);
            return;
        }

        if (isCountingDown)
        {
            waveCountdown -= Time.deltaTime;
            if (waveCountdown <= 0)
            {
                isCountingDown = false;
                waveCountdown = enemyTypes[currentWaveIndex].waitingSpawnTime;
                StartCoroutine(SpawnWave());
            }
        }

        CheckIfAllEnemiesDefeated();
    }

    private void PrepareEnemyTypes()
    {
        foreach (var enemyType in enemyTypes)
        {
            enemyType.currentSpawnCount = enemyType.initialSpawnCount;
        }
    }

    private IEnumerator SpawnWave()
    {
        if (currentWaveIndex < totalRounds)
        {
            for (int i = 0; i < enemyTypes.Count; i++)
            {
                for (int j = 0; j < enemyTypes[i].currentSpawnCount; j++)
                {
                    int spawnPointIndex = Random.Range(0, spawnPoints.Count);
                    EnemyAI spawnedEnemy = Instantiate(enemyTypes[i].enemyPrefab, spawnPoints[spawnPointIndex].transform.position, spawnPoints[spawnPointIndex].transform.rotation);
                    activeEnemies.Add(spawnedEnemy);
                    yield return new WaitForSeconds(enemyTypes[i].spawnDelay);
                }

                // Multiply spawn count for the next round
                enemyTypes[i].currentSpawnCount = Mathf.RoundToInt(enemyTypes[i].currentSpawnCount * spawnRateMultiplier);
            }
        }
    }

    private void CheckIfAllEnemiesDefeated()
    {
        activeEnemies.RemoveAll(enemy => enemy == null);
        if (activeEnemies.Count == 0 && !isCountingDown)
        {
            StartNextWave();
        }
    }

    private void StartNextWave()
    {
        isCountingDown = true;
        currentWaveIndex++;
        if (currentWaveIndex < totalRounds)
        {
            waveCountdown = initialWaveCountdown;
        }
    }

    [System.Serializable]
    public class EnemyType
    {
        public EnemyAI enemyPrefab;
        public int initialSpawnCount;
        public float spawnDelay;
        public float waitingSpawnTime;
        [HideInInspector] public int currentSpawnCount;
    }
}
