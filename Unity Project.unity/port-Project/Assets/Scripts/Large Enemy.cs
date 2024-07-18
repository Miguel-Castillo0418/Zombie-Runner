using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class LargeEnemy : EnemyAI
{
    [SerializeField] Transform meleeAttackPoint;
    //------------NEW-------------
    [SerializeField] EnemyAI enemyAI;
    [SerializeField] float chargeRadius = 10f;
    [SerializeField] int chargeDamage = 10;
    [SerializeField] float lastAttackTime = 0;
    [SerializeField] float chargeSpeed = 6f;
    [SerializeField] float normalSpeed = 3.5f;
    [SerializeField] float chargeDuration = 1f;
    [SerializeField] float chargeCooldown = 10f;
    [SerializeField] public NavMeshPath path;
    private bool isCharging = false;
    private bool canCharge = true;
    private bool attacking = false;


    //------------NEW-------------





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
                //StartCoroutine(Charge());
                agent.speed = chargeSpeed;
            }

        }

        //------------NEW-------------
    }
    //------------NEW-------------
    IEnumerator Charge()
    {
        Debug.Log("Startcharge");
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
                IKnockbackable _knock = collision.collider.GetComponent<IKnockbackable>();
                if (dmg != null)
                {
                    dmg.takeDamage(chargeDamage);
                    _knock.Knockback();
                }
            }
        }

    }
    //------------NEW-------------
    private void OnTriggerEnter(Collider other)
    {
        IDamage dmg = other.GetComponent<IDamage>();
        IKnockbackable _knock = other.GetComponent<IKnockbackable>();
        if (other.name == "Player")
        {
            int force = lvl * damage;
            float t = force * Time.deltaTime;
            Debug.Log(other.transform.name);
            dmg.takeDamage(damage);
            _knock.Knockback();
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

}
