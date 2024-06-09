using System.Collections;
using System.Collections.Generic;
using UnityEditor.TestTools.CodeCoverage;
using UnityEngine;
using UnityEngine.AI;

public class EnemyAI : MonoBehaviour, IDamage
{
    [SerializeField] Rigidbody rb;
    [SerializeField] Renderer model;
    [SerializeField] NavMeshAgent agent;

    [SerializeField] int HP;
    [SerializeField] int lvl;
    [SerializeField] int damage;




    // Start is called before the first frame update
    void Start()
    {
       GameManager.instance.updateGameGoal(1);
    }

    // Update is called once per frame
    void Update()
    {
       agent.SetDestination(GameManager.instance.player.transform.position);

    }


    public void takeDamage(int amount)
    {
        HP -= amount;
        StartCoroutine(flashDamange());

        if (HP <= 0)
        {
            GameManager .instance.updateGameGoal(-1);
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
        IDamage dmg=other.GetComponent<IDamage>();
        if (dmg != null)
        {
            Debug.Log(other.transform.name);
            dmg.takeDamage(damage);
        }
    }
}
