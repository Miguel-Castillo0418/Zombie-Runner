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
            StartCoroutine(knockBack(other.transform,(float)lvl*damage));
        }
    }
    IEnumerator knockBack(Transform target, float force)
    {
        force = lvl * damage;
        float t = force * Time.deltaTime;
        agent.SetDestination(TempgameManager.instance.player.transform.position);
        Vector3 start = target.position;
        Vector3 end = start + model.transform.forward*force;
        float startTime = 0f;
        float duration = 1.0f;
        while (startTime < duration) { 
        target.position = Vector3.Lerp(start, end, t);
            startTime += Time.deltaTime;

        }
        yield return new WaitForSeconds(t);
        target.position = end;
    }
}
