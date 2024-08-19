using UnityEngine;

public class Spawner : MonoBehaviour
{
    public GameObject enemyPrefab;
    public float spawnInterval = 5f;
    public float maxDistanceToPlayer = 20f;
    public float minDistanceToPlayer = 5f;
    public int maxEnemies = -1; // Maximum number of enemies to spawn (-1 for infinite)

    private Transform player;
    private float spawnTimer;
    public int spawnedEnemiesCount = 0;
    public bool canSpawn = true;

    void Start()
    {
        player = GameObject.FindWithTag("Player").transform;
        spawnTimer = spawnInterval;
    }

    void Update()
    {
        //if (!isActive) return;

        // Stop checking if the maximum number of enemies has been reached
        if (maxEnemies != -1 && spawnedEnemiesCount >= maxEnemies)
        {
            Debug.Log("Maximum number of enemies reached.");
            canSpawn = false; // Prevent further spawning
            return;
        }

        if (!canSpawn) return;

        // Debug log to see if the update loop is still running
        Debug.Log("Update Loop Running");

        float distanceToPlayer = Vector3.Distance(transform.position, player.position);

        // Debug log to see the distance to player
        Debug.Log($"Distance to Player: {distanceToPlayer}");

        // Deactivate spawner if player is too close
        if (distanceToPlayer <= minDistanceToPlayer)
        {
            Debug.Log("Player too close to spawn.");
            return;
        }

        // Adjust spawn rate based on distance to player
        float spawnChance = Mathf.Clamp01((distanceToPlayer - minDistanceToPlayer) / (maxDistanceToPlayer - minDistanceToPlayer));

        spawnTimer -= Time.deltaTime;
        if (spawnTimer <= 0 && Random.value < spawnChance)
        {
            Debug.Log("Attempting to spawn enemy.");
            SpawnEnemy();
            spawnTimer = spawnInterval; // Reset timer after attempting to spawn
        }
    }

    void SpawnEnemy()
    {
        Vector3 spawnPosition = transform.position + Random.insideUnitSphere; // * spawnRadius;
        spawnPosition.y = transform.position.y; 
        Instantiate(enemyPrefab, spawnPosition, enemyPrefab.transform.rotation);
        ++spawnedEnemiesCount;
        Debug.Log("Enemy spawned. Total spawned: " + spawnedEnemiesCount);

        if (maxEnemies != -1 && spawnedEnemiesCount >= maxEnemies)
        {
            Debug.Log("Reached maxEnemies in SpawnEnemy.");
            canSpawn = false; // Prevent further spawning
        }
    }

    
}
