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



    public WaveSpawner whereISpawned;
    bool playerInRange;
    Vector3 playerDir;


    // Start is called before the first frame update
    void Start()
    {
        gameManager.instance.updateGameGoal(1);
    }

    // Update is called once per frame
    void Update()
    {
      //  if (playerInRange)
      //  {
            agent.SetDestination(gameManager.instance.player.transform.position);
      //  }
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
            rewardZombucks(); 
        }
    }

    IEnumerator flashDamange()
    {
        Color _color=model.material.color;
        model.material.color = Color.red;
        yield return new WaitForSeconds(0.1f);
        model.material.color = _color;
    }
    private void OnTriggerEnter(Collider other)
    {
       // if(other.CompareTag("Player"))
           // playerInRange= true;
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
   // public void OnTriggerExit(Collider other)
    //{
     //   if(other.CompareTag("Player"))
     //       playerInRange= false;
   // }
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

        // Ensure the player reaches the exact target position
        playerTransform.position = targetPosition;
        Rigidbody playerRigidbody = gameManager.instance.player.GetComponent<Rigidbody>();
        playerRigidbody.angularVelocity = Vector3.zero;
    }
}
