using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Mausoleum : MonoBehaviour
{
    [SerializeField] Animator animEnter;
    [SerializeField] Animator animEnter2;
    [SerializeField] Animator animExit;
    [SerializeField] Animator animExit2;
    [SerializeField] Spawner spawner;
    [SerializeField] Spawner spawner2;
    [SerializeField] Spawner spawner3;
    [SerializeField] gameManager gameManager;
    [SerializeField] ObjectiveComplete objective;
    public int enemyGoal;
    
    // Start is called before the first frame update
    void Start()
    {
            enemyGoal = spawner.maxEnemies + spawner2.maxEnemies + spawner3.maxEnemies;
            enabled = false;

            // gameObject.GetComponent<Animator>().Play(); ;

    }

    // Update is called once per frame
    void Update()
    {
        if (gameManager.instance.deadEnemies >= enemyGoal)
        {
            spawner.canSpawn = false;
            animExit.SetTrigger("EnemiesDead");
            animExit2.SetTrigger("EnemiesDead");
        }
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player")&&gameObject.name== "playerEnter")
        {

            animEnter.SetTrigger("PlayerEnter");
            animEnter2.SetTrigger("PlayerEnter");
            enabled = true;
        }
    }
}
