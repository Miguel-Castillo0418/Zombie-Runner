using System.Collections;
using System.Collections.Generic;

//using UnityEditor.TestTools.CodeCoverage;
using UnityEngine;
using UnityEngine.AI;

public class Temp_EnemyAI : MonoBehaviour, IDamage
{
    [SerializeField] Rigidbody rb;
    [SerializeField] Renderer model;
    [SerializeField] NavMeshAgent agent;

    [SerializeField] int HP;
    [SerializeField] int lvl;
    [SerializeField] int damage;
    //[SerializeField] int force;



    // Start is called before the first frame update
    void Start()
    {
        TempgameManager.instance.updateGameGoal(1);
    }

    // Update is called once per frame
    void Update()
    {
       
    }


    public void takeDamage(int amount)
    {
        HP -= amount;
        StartCoroutine(flashDamange());

        if (HP <= 0)
        {
            TempgameManager.instance.updateGameGoal(-1);
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
            
            Debug.Log(other.transform.name);
            knockBack(other);
        }
    }
    private void knockBack(Collider other)
    {
        float force = lvl * damage;
        float t = force * Time.deltaTime;
        agent.SetDestination(TempgameManager.instance.player.transform.position);
        Vector3 start = other.transform.position;
        Vector3 end = start + model.transform.forward;

        Vector3 newPos = Vector3.Lerp(start, end, force);
        other.transform.position = newPos;
    }
}
