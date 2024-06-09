using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Wave : MonoBehaviour
{

    [SerializeField] private float countdown;
    [SerializeField] moreWaves[] waves;

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

    }
}

[System.Serializable]

public class moreWaves
{
    public EnemyAI[] enemies;
}
