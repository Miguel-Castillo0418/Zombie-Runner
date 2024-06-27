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
    [SerializeField] Transform[] meleeAttack;
    int meleeAttackIndex;
    [SerializeField] Collider collider;
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
    float normSpeed;

    public WaveSpawner whereISpawned;
    bool playerInRange;
    Vector3 playerDir;


    // Start is called before the first frame update
    void Start()
    {
        gameManager.instance.updateGameGoal(1);
        maxHp = HP;
        HalfHpSpeed = agent.speed * 3.5f;
    }

    // Update is called once per frame
    void Update()
    {
        if (agent.velocity.normalized.magnitude > 0)
            normSpeed = agent.velocity.normalized.magnitude;

        anim.SetFloat("Speed", normSpeed);
        agent.SetDestination(gameManager.instance.player.transform.position);
        if (agent.remainingDistance <= agent.stoppingDistance)
        {
            anim.SetBool("PlayerInRange", true);
            anim.SetFloat("Speed", normSpeed);
            StartCoroutine(MeleeAttack());
        }
        else
        {
            anim.SetBool("PlayerInRange", false);
        }
    }


    public void takeDamage(int amount)
    {
        HP -= amount;
        StartCoroutine(flashDamange());
        if (HP / (float)maxHp <= 0.5f&& HP>0)
        {
            anim.SetTrigger("HalfHp");
            agent.speed = HalfHpSpeed;
            anim.SetBool("HalfHp", true);
        }
        if (HP <= 0)
        {
            anim.SetBool("IsDead", true);
            anim.SetTrigger("Die");
            agent.speed = 0;
            if (whereISpawned)
            {
                whereISpawned.updateEnemyNumber();
            }
            EnemyColliderToggle();
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
        Collider[] hitplayer = Physics.OverlapSphere(meleeAttack[meleeAttackIndex].position, meleeRange, enemyLayer);

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

        yield return new WaitForSeconds(1.25f);
        Destroy(gameObject);

    }
    void EnemyColliderToggle()
    {
        collider.enabled = !collider;
    }
}
