using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SwordPickup : MonoBehaviour
{
    [SerializeField] SwordStats sword;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {

            gameManager.instance.playerScript.toggleSword();
            gameManager.instance.playerScript.getSwordStats(sword);
            Destroy(gameObject);
        }
    }
}
