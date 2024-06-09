using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Wave : MonoBehaviour
{

    [SerializeField] private float waveCountdown;
    [SerializeField] moreWaves[] waves;
    [SerializeField] private List<GameObject> spawnPoint;



    private int waveIndex = 0;
    private bool countdown;

    // Start is called before the first frame update
    void Start()
    {
        countdown = true;

        for (int i = 0; i < waves.Length; i++)
        {
            waves[i].enemiesRemaining = waves[i].enemies.Length;
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (waveIndex >= waves.Length)
        {
            gameManager.instance.updateGameGoal(1);
        }

        if (countdown == true)
        {
            waveCountdown -= Time.deltaTime;
        }

        if (waveCountdown <= 0)
        {
            countdown = false;

            waveCountdown = waves[waveIndex].waitingSpawnTime;
            StartCoroutine(spawnWave());
        }

        if (waves[waveIndex].enemiesRemaining == 0)
        {
            countdown = true;
            waveIndex++;
        }
    }

    private IEnumerator spawnWave()
    {
        if (waveIndex < waves.Length)
        {


            for (int i = 0; i < waves[waveIndex].enemies.Length; i++)
            {
                int rand = Random.Range(0, spawnPoint.Count);
                enemyAI enemy = Instantiate(waves[waveIndex].enemies[i], spawnPoint[rand].transform.position, spawnPoint[rand].transform.rotation);
                yield return new WaitForSeconds(waves[waveIndex].waitingSpawnTime);
            }
        }
    }


    [System.Serializable]
    public class moreWaves
    {
        public enemyAI[] enemies;
        public float waitingSpawnTime;
        public float waitingNextWave;

        [HideInInspector] public int enemiesRemaining;

    }
}
