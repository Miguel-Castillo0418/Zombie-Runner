using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.AI;

public class EnemyAI : MonoBehaviour, IDamage
{
    [SerializeField] Rigidbody rb;
    [SerializeField] Renderer model;
    [SerializeField] NavMeshAgent agent;
    [SerializeField] Transform meleeAttackPoint;
    [SerializeField] Animator anim;
    [SerializeField] int meleeRange;
    [SerializeField] float atkRate;
    [SerializeField] int HP;
    int maxHp;
    [SerializeField] int lvl;
    [SerializeField] int damage;
    [SerializeField] int pointsRewarded;
    [SerializeField] private LayerMask enemyLayer;
    float HalfHpSpeed;


    public WaveSpawner whereISpawned;
    bool playerInRange;
    Vector3 playerDir;


    // Start is called before the first frame update
    void Start()
    {
        gameManager.instance.updateGameGoal(1);
        maxHp = HP;
        HalfHpSpeed = agent.speed*2;
    }

    // Update is called once per frame
    void Update()
    {
        agent.SetDestination(gameManager.instance.player.transform.position);
        anim.SetTrigger("PlayerInRange");
        if (agent.remainingDistance<=agent.stoppingDistance&& agent.remainingDistance>0)
        {
            
            anim.SetTrigger("Atk");
            StartCoroutine(MeleeAttack());
        }
    }


    public void takeDamage(int amount)
    {
        HP -= amount;
        StartCoroutine(flashDamange());

        if (HP / maxHp <= .5)
        {
            anim.SetTrigger("HalfHp");
            agent.speed = HalfHpSpeed;
        }


        if (HP <= 0)
        {
            agent.speed = 0;
            if (whereISpawned)
            {
                whereISpawned.updateEnemyNumber();
            }

            StartCoroutine(DeathAnimation());
            rewardZombucks();
            gameManager.instance.updateGameGoal(-1);



        }
    }

    IEnumerator flashDamange()
    {
        Color _color = model.material.color;
        model.material.color = Color.red;
        yield return new WaitForSeconds(0.1f);
        model.material.color = _color;
    }
    private void OnTriggerEnter(Collider other)
    {
        IDamage dmg = other.GetComponent<IDamage>();
        if (other.name == "Player")
        {
            int force = lvl * damage;
            float t = force * Time.deltaTime;
            Debug.Log(other.transform.name);
            dmg.takeDamage(damage);
            knockback();

        }
    }
    IEnumerator MeleeAttack()
    {
        //Stop the enemy
        //agent.speed = 0;
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
    void rewardZombucks()
    {
        gameManager.instance.addPoints(pointsRewarded);
    }

    void knockback()
    {
        int force = lvl * damage * 10; // Adjust this force value as needed
        float knockbackDuration = 0.5f; // Adjust the duration of knockback

        Vector3 knockbackDirection = (gameManager.instance.player.transform.position - transform.position).normalized;
        Vector3 targetPosition = gameManager.instance.player.transform.position + knockbackDirection * 3f; // Adjust the distance of knockback
        StartCoroutine(ApplyKnockback(gameManager.instance.player.transform, targetPosition, knockbackDuration));

    }
    IEnumerator ApplyKnockback(Transform playerTransform, Vector3 targetPosition, float duration)
    {
        Vector3 initialPosition = playerTransform.position;
        float timer = 0f;

        while (timer < duration)
        {
            float progress = timer / duration;
            playerTransform.position = Vector3.Lerp(initialPosition, targetPosition, progress);

            timer += Time.deltaTime;
            yield return null;
        }
    }
    IEnumerator DeathAnimation()
    {
        anim.SetTrigger("Dead");

        yield return new WaitForSeconds(1.5f);
        Destroy(gameObject);

    }
}
