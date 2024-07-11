using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.AI;

public class EnemyAI : MonoBehaviour, IDamage//, IKnockbackable
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
    float maxHp;
    [SerializeField] public int lvl;
    [SerializeField] public int damage;
    [SerializeField] public int pointsRewarded;
    [SerializeField] private LayerMask enemyLayer;
    float HalfHpSpeed;
    float normSpeed;

    public WaveSpawner whereISpawned;
    public static bool isSound;
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
        AudioManager.instance.playZombie();
    }


    public void takeDamage(float amount)
    {
        HP -= amount;
        if (HP / maxHp <= 0.5f && HP > 0)
        {
            anim.SetTrigger("HalfHp");
            agent.speed = HalfHpSpeed;
            anim.SetBool("HalfHp", true);
        }
        if (HP <= 0)
        {
            AudioManager.instance.stopSound();
            AudioManager.instance.zombDeath("Zdead");
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
        }
    }
    public void takeFireDamage(float amount)
    {
        StartCoroutine(applyDamageOverTime(amount,5.0f));
    }
    public void takePoisonDamage(float amount) 
    {
        StartCoroutine(applyDamageOverTime(amount, 5.0f));
    }
    public void takeElectricDamage(float amount)
    {
        StartCoroutine(applyDamageOverTime(amount, 5.0f));
    }
    public void takeExplosiveDamage(float amount) 
    {
        StartCoroutine(applyDamageOverTime(amount, 5.0f));
    }
    IEnumerator applyDamageOverTime(float amount, float duration) //the total damage over time in seconds
    {
        float timer = 0f;
        float damagePerSec = amount / duration;
        while (timer < duration)
        {
            float damagePerFrame = damagePerSec * Time.deltaTime;
            takeDamage(damagePerFrame);
            timer += Time.deltaTime;
            yield return null;
        }
    }
    //private void OnTriggerEnter(Collider other)
    //{
    //    IDamage dmg = other.GetComponent<IDamage>();
    //    IKnockbackable _knock=other.GetComponent<IKnockbackable>();
    //    if (other.name == "Player")
    //    {
    //        int force = lvl * damage;
    //        float t = force * Time.deltaTime;
    //        Debug.Log(other.transform.name);
    //        dmg.takeDamage(damage);
    //        _knock.Knockback();

    //    }
    //}
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
    public void rewardZombucks()
    {
        gameManager.instance.addPoints(pointsRewarded);
    }

    public void Knockback()
    {
        int force = lvl * damage * 10; // Adjust this force value as needed
        float knockbackDuration = 0.5f; // Adjust the duration of knockback

        Vector3 knockbackDirection = (gameManager.instance.player.transform.position - transform.position).normalized;
        Vector3 targetPosition = gameManager.instance.player.transform.position + knockbackDirection * 3f; // Adjust the distance of knockback
        StartCoroutine(ApplyKnockback(gameManager.instance.player.transform, targetPosition, knockbackDuration));

    }
    public IEnumerator ApplyKnockback(Transform playerTransform, Vector3 targetPosition, float duration)
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
        collider.enabled = false;
    }


}
