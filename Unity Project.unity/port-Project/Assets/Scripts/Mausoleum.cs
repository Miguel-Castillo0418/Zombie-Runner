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
    [SerializeField] gameManager gameManager;
    // Start is called before the first frame update
    void Start()
    {  
        enabled = false;

       // gameObject.GetComponent<Animator>().Play(); ;
    }

    // Update is called once per frame
    void Update()
    {
        if (gameManager.instance.deadEnemies >= 2)
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
