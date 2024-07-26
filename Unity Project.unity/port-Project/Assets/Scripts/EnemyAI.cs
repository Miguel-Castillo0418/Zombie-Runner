using System.Collections;
using System.Collections.Generic;
using System.Threading;
using Unity.VisualScripting;
using UnityEditor;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.Timeline;

public class EnemyAI : MonoBehaviour, IDamage, IKnockbackable
{
    [SerializeField] Rigidbody rb;
    [SerializeField] public NavMeshAgent agent;
    [SerializeField] Transform[] meleeAttack;
    int meleeAttackIndex;
    [SerializeField] new Collider collider;
    [SerializeField] public Animator anim;
    [SerializeField] public int meleeRange;
    [SerializeField] public float atkRate;
    [SerializeField] public float HP;
    public float maxHp;
    [SerializeField] public int lvl;
    [SerializeField] public int damage;
    [SerializeField] public int pointsRewarded;
    [SerializeField] private LayerMask enemyLayer;
    float HalfHpSpeed;
    [SerializeField] public GameObject onFire;
    [SerializeField] public GameObject poisoned;
    [SerializeField] public GameObject electrified;
    public WaveSpawner whereISpawned;
    public static bool isSound;
    //bool playerInRange;
    bool isDead=false;
    public GameObject explosion;
    // float range = 5;
    GameObject player;
    Vector3 playerDir;

    // Start is called before the first frame update
    void Start()
    {
        gameManager.instance.updateGameGoal(1);
        maxHp = HP;
        HalfHpSpeed = agent.speed * 3.5f;
        player = gameManager.instance.player;
        anim.SetFloat("Speed", 1);
        agent.speed = 0;

    }

    // Update is called once per frame
    void Update()
    {
        playerDir=(player.transform.position-transform.position).normalized;
        playerDir.y = 0f;
        if (playerDir != Vector3.zero) 
        {
            Quaternion lookRotation = Quaternion.LookRotation(playerDir);
            transform.rotation = lookRotation;
            agent.speed = 3.5f;
        }
        //if player is not in range enemy will follow if not they will stand still while attacking
            agent.SetDestination(gameManager.instance.player.transform.position);

        if (agent.remainingDistance <= agent.stoppingDistance)
        {
            agent.speed = 0;
            anim.SetBool("PlayerInRange", true);
        }
        else
        {
            anim.SetBool("PlayerInRange", false);

        }
        AudioManager.instance.playZombie();
    }

    //for IDamage
    public void takeDamage(float amount)
    {
        HP -= amount;
        if (HP / maxHp <= 0.5f && HP > 0)
        {
            anim.SetTrigger("HalfHp");
            agent.speed = HalfHpSpeed;
            anim.SetBool("HalfHp", true);
        }
        if (HP <= 0&&!isDead)
        {
            AudioManager.instance.stopSound();
            AudioManager.instance.zombDeath("Zdead");
            isDead= true;
            EnemyColliderToggle();
            anim.SetBool("IsDead", true);
            anim.SetTrigger("Die");
            agent.speed = 0;
            if (whereISpawned)
            {
                whereISpawned.updateEnemyNumber();
            }
            if (this.name == "enemy_1(Clone)" || this.name == "enemy_2(Clone)")
            {
                StartCoroutine(DeathAnimation());
            }
            else
            {
                Destroy(gameObject);
            }
            rewardZombucks();
            gameManager.instance.updateGameGoal(-1);
            gameManager.instance.deadEnemies += 1;
            gameManager.instance.coinsCollected += pointsRewarded;
        }
    }
    public void rewardZombucks()
    {
        gameManager.instance.addPoints(pointsRewarded);
    }

    public void Knockback(Collider other, int lvl, int damage)
    {
        int force = lvl * damage * 10;
        float knockbackDuration = 0.5f;
        float knockbackDistance = 3f;

        Vector3 targetPosition = transform.position + other.transform.forward * knockbackDistance;
        Vector3 knockbackDirection = (targetPosition - transform.position).normalized;
        StartCoroutine(ApplyKnockback(transform, targetPosition, knockbackDuration, force));

    }
    public IEnumerator ApplyKnockback(Transform playerTransform, Vector3 targetPosition, float duration, float force)
    {
        Vector3 initialPosition = playerTransform.position;
        float timer = 0f;

        while (timer < duration)
        {
            float progress = timer / duration;
            float currentSpeed = Mathf.Lerp(0, force, progress);

            playerTransform.position += (targetPosition - initialPosition).normalized * currentSpeed * Time.deltaTime;

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
        collider.enabled = false;
    }
    //for the explosion animation
    public void Explode()
    {
        //AudioManager.instance.explosionSound();
        Instantiate(explosion, transform.position + (Vector3.up * 2), Quaternion.identity);
        StartCoroutine(WaitForAnimationThenDestroy(explosion));
        //sources.Play();
    }

    private IEnumerator WaitForAnimationThenDestroy(GameObject explosion)
    {
        yield return new WaitForSeconds(5.0f);  // Adjust the time as needed
        Destroy(explosion);
    }
}
