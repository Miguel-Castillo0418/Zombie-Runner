using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class enemy_invicible : EnemyAI
{
    // Start is called before the first frame update
    void Start()
    {
        agent.speed = 13;
    }

    // Update is called once per frame
    void Update()
    {
        agent.SetDestination(gameManager.instance.player.transform.position);

    }
    private void OnCollisionEnter(Collision collision)
    {
        if (collision.collider.CompareTag("Player"))
        {
            Debug.Log("chargeDMG");
            IDamage dmg = collision.collider.GetComponent<IDamage>();
            IKnockbackable _knock = collision.collider.GetComponent<IKnockbackable>();
            if (dmg != null)
            {
                dmg.takeDamage(100);
                _knock.Knockback();
            }
        }
    }
}