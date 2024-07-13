using UnityEngine;

public class Spawner : MonoBehaviour
{
    public GameObject enemyPrefab; 
    public float spawnInterval = 5f; 
    //public float spawnRadius = 10f; 
    public float maxDistanceToPlayer = 20f; 
    public float minDistanceToPlayer = 5f; 
    public int maxEnemies; // Maximum number of enemies to spawn (-1 for infinite)

    private Transform player;
    private bool isActive = false;
    private float spawnTimer;
    private int spawnedEnemiesCount = 0;

    void Start()
    {
        player = GameObject.FindWithTag("Player").transform;
        spawnTimer = spawnInterval;
    }

    void Update()
    {
        if (!isActive) return;

        float distanceToPlayer = Vector3.Distance(transform.position, player.position);

        // Deactivate spawner if player is too close
        if (distanceToPlayer <= minDistanceToPlayer)
        {
            return;
        }

        // Adjust spawn rate based on distance to player
        float spawnChance = Mathf.Clamp01((distanceToPlayer - minDistanceToPlayer) / (maxDistanceToPlayer - minDistanceToPlayer));

        spawnTimer -= Time.deltaTime;
        if (spawnTimer <= 0 && Random.value < spawnChance)
        {
            if (maxEnemies == -1 || spawnedEnemiesCount < maxEnemies)
            {
                SpawnEnemy();
                spawnTimer = spawnInterval;
            }
        }
    }

    void SpawnEnemy()
    {
        Vector3 spawnPosition = transform.position + Random.insideUnitSphere; // * spawnRadius;
        spawnPosition.y = transform.position.y; 
        Instantiate(enemyPrefab, spawnPosition, Quaternion.identity);
        spawnedEnemiesCount++;
    }

    public void SetActive(bool active)
    {
        isActive = active;
        if (!active)
        {
            spawnedEnemiesCount = 0; 
        }
    }
}
