using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class gunPickup : MonoBehaviour
{

    [SerializeField] gunStats gun;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            gameManager.instance.playerScript.getGunStats(gun);
            Destroy(gameObject);
        }
    }
}
