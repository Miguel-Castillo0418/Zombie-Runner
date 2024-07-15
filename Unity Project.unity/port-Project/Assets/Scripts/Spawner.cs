using UnityEngine;
using System.Collections;

public class Spawner : MonoBehaviour
{
    public GameObject enemyPrefab;
    public float spawnInterval = 5f;
    public float maxDistanceToPlayer = 20f;
    public float minDistanceToPlayer = 5f;
    public int maxEnemies; // Maximum number of enemies to spawn (-1 for infinite)

    private Transform player;
    private bool isActive = false;
    private float spawnTimer;
    private int spawnedEnemiesCount = 0;
    private bool canSpawn = true;

    void Start()
    {
        player = GameObject.FindWithTag("Player").transform;
        spawnTimer = spawnInterval;
    }

    public IEnumerator spawnEnemies()
    {
        if (!isActive) yield return null;

        // Stop checking if the maximum number of enemies has been reached
        if (maxEnemies != -1 && spawnedEnemiesCount >= maxEnemies)
        {
            Debug.Log("Maximum number of enemies reached.");
            canSpawn = false; // Prevent further spawning
            yield return null;
        }

        if (!canSpawn) yield return null;

        // Debug log to see if the update loop is still running
        Debug.Log("Update Loop Running");

        float distanceToPlayer = Vector3.Distance(transform.position, player.position);

        // Debug log to see the distance to player
        Debug.Log($"Distance to Player: {distanceToPlayer}");

        // Deactivate spawner if player is too close
        if (distanceToPlayer <= minDistanceToPlayer)
        {
            Debug.Log("Player too close to spawn.");
            yield return null;
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
        yield return null;
    }

    void SpawnEnemy()
    {
        Vector3 spawnPosition = transform.position + Random.insideUnitSphere; // * spawnRadius;
        spawnPosition.y = transform.position.y;
        Instantiate(enemyPrefab, spawnPosition, Quaternion.identity);
        spawnedEnemiesCount++;
        Debug.Log("Enemy spawned. Total spawned: " + spawnedEnemiesCount);

        if (maxEnemies != -1 && spawnedEnemiesCount >= maxEnemies)
        {
            Debug.Log("Reached maxEnemies in SpawnEnemy.");
            canSpawn = false; // Prevent further spawning
        }
    }

    public void SetActive(bool active)
    {
        isActive = active;
        if (!active)
        {
            spawnedEnemiesCount = 0;
            canSpawn = true;
        }
    }

}
