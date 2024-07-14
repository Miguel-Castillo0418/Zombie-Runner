using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Unity.Mathematics;
using UnityEngine;
using UnityEngine.UIElements;

public class SpawnSpecial : MonoBehaviour
{
    [SerializeField] public GameObject enemy;
    [SerializeField] public GameObject spawn;
    void Start()
    {

    }


    //melee dealing damage
    private void OnTriggerEnter(Collider other)
    {
        spawnEnemy(enemy, spawn.transform);
    }
    void spawnEnemy(GameObject gameObject,Transform position)
    {
        Instantiate(gameObject, position);
    }
}
