using System.Collections;
using System.Collections.Generic;
//using UnityEditor.TestTools.CodeCoverage;
using UnityEngine;
using UnityEngine.AI;

public class EnemyAI : MonoBehaviour, IDamage
{
    [SerializeField] Rigidbody rb;
    [SerializeField] Renderer model;
    [SerializeField] NavMeshAgent agent;
    [SerializeField] Transform meleeAttackPoint;

    [SerializeField] int HP;
    [SerializeField] int lvl;
    [SerializeField] int damage;
    [SerializeField] int force;
    [SerializeField] private float meleeRange;
    [SerializeField] private float atkRate;
    [SerializeField] private LayerMask enemyLayer;


    public WaveSpawner whereISpawned;


    // Start is called before the first frame update
    void Start()
    {
        gameManager.instance.updateGameGoal(1);
    }

    // Update is called once per frame
    void Update()
    {
        agent.SetDestination(gameManager.instance.player.transform.position);

    }


    public void takeDamage(int amount)
    {
        HP -= amount;
        StartCoroutine(flashDamange());

        if (HP <= 0)
        {
            gameManager.instance.updateGameGoal(-1);

            if (whereISpawned) 
            { 
                whereISpawned.updateEnemyNumber();
            }

            Destroy(gameObject);

        }
    }

    IEnumerator flashDamange()
    {
        model.material.color = Color.red;
        yield return new WaitForSeconds(0.1f);
        model.material.color = Color.white;
    }
    private void OnTriggerEnter(Collider other)
    {
        
        IDamage dmg = other.GetComponent<IDamage>();
        if (dmg != null)
        {

            force = lvl * damage;
            float t = force * Time.deltaTime;
            Debug.Log(other.transform.name);

            dmg.takeDamage(damage);
            other.transform.position=Vector3.Lerp(other.transform.position, other.transform.forward * force, t);
        }
    }
    IEnumerator MeleeAttack()
    {
        // Detect player in range
        Collider[] hitplayer = Physics.OverlapSphere(meleeAttackPoint.position, meleeRange, enemyLayer);

        // Apply damage to player
        foreach (Collider player in hitplayer)
        {
            IDamage damageable = player.GetComponent<IDamage>();
            if (damageable != null)
            {
                damageable.takeDamage(damage);
            }
        }

        yield return new WaitForSeconds(atkRate);
    }
}
