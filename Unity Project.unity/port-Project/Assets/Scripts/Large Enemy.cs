using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class LargeEnemy : MonoBehaviour, IDamage
{
    [SerializeField] Rigidbody rb;
    [SerializeField] Renderer model;
    [SerializeField] NavMeshAgent agent;
    [SerializeField] Transform meleeAttackPoint;
    //[SerializeField] int animTransSpeed;
    [SerializeField] int meleeRange;
    [SerializeField] float atkRate;
    [SerializeField] int HP;
    [SerializeField] int lvl;
    [SerializeField] int damage;
    [SerializeField] int pointsRewarded;
    //[SerializeField] int force;
    //[SerializeField] Animator anim;
    [SerializeField] private LayerMask enemyLayer;

    //------------NEW-------------
    [SerializeField] float chargeRadius = 10f;
    [SerializeField] int chargeDamage = 10;
    [SerializeField] float lastAttackTime = 0;
    [SerializeField] float chargeSpeed = 6f;
    [SerializeField] float normalSpeed = 3.5f;
    [SerializeField] float chargeDuration = 1f;
    [SerializeField] float chargeCooldown = 10f;
    [SerializeField] Animator anim;
    [SerializeField] new Collider collider;
    private bool isCharging = false;
    private bool canCharge = true;
    private bool attacking = false;


    //------------NEW-------------



    public WaveSpawner whereISpawned;
    bool playerInRange;
    Vector3 playerDir;


    // Start is called before the first frame update
    void Start()
    {
        gameManager.instance.updateGameGoal(1);
        agent.speed = normalSpeed;
    }

    // Update is called once per frame
    void Update()
    {
        agent.SetDestination(gameManager.instance.player.transform.position);
        //------------NEW-------------
        if (!isCharging || !attacking)
        {
            float distanceToPlayer = Vector3.Distance(transform.position, gameManager.instance.player.transform.position);

            if (distanceToPlayer <= meleeRange)
            {

                if (Time.time >= lastAttackTime + atkRate)
                {
                    //Debug.Log("attacking");
                    StartCoroutine(kick());
                    attacking = true;
                }
            }
            else if (distanceToPlayer <= chargeRadius && !(distanceToPlayer <= meleeRange) && canCharge)
            {
                // Charge towards the player

                Debug.Log("Startcharge");
                StartCoroutine(Charge());

            }

        }

        //------------NEW-------------
    }
    //------------NEW-------------
    IEnumerator Charge()
    {
        isCharging = true;
        canCharge = false;
        agent.isStopped = true;
        anim.SetBool("Charge", true);
        anim.SetFloat("Speed", 0f);

        Vector3 chargeDirection = (gameManager.instance.player.transform.position - transform.position).normalized;
        transform.rotation = Quaternion.LookRotation(chargeDirection);
        float chargeEndTime = Time.time + chargeDuration;

        while (Time.time < chargeEndTime)
        {

            transform.position += chargeDirection * chargeSpeed * Time.deltaTime;

            yield return null;
        }

        agent.isStopped = false;
        isCharging = false;
        anim.SetBool("Charge", false);
        Debug.Log("Endcharge");
        StartCoroutine(ChargeCooldown());
    }
    IEnumerator ChargeCooldown()
    {
        yield return new WaitForSeconds(chargeCooldown);
        canCharge = true;
    }
    private void OnCollisionEnter(Collision collision)
    {
        if (isCharging && !attacking)
        {
            if (collision.collider.CompareTag("Player"))
            {
                Debug.Log("chargeDMG");
                IDamage dmg = collision.collider.GetComponent<IDamage>();
                if (dmg != null)
                {
                    dmg.takeDamage(chargeDamage);
                    knockback();
                }
            }
        }

    }
    //------------NEW-------------


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
            rewardZombucks();
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
            attacking = false;
        }
    }
    IEnumerator kick()
    {
        anim.SetBool("Attacking", true);
        agent.isStopped = true;
        yield return new WaitForSeconds(1);
        agent.isStopped = false;
        anim.SetBool("Attacking", false);

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

}