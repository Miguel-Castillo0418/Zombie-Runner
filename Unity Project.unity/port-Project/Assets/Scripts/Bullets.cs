using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    [SerializeField] Rigidbody rb;
    private float damage;
    [SerializeField] int speed;
    [SerializeField] float destroyTime;
    // Start is called before the first frame update
    void Start()
    {
        rb.velocity = transform.forward * speed;
        Destroy(gameObject, destroyTime);
    }

    public void SetDamage(float damage)
    {
        this.damage = damage;
    }

    // Update is called once per frame
    private void OnTriggerEnter(Collider other)
    {
        IDamage dmg = other.GetComponent<IDamage>();

        if (dmg != null)
        {
            dmg.takeDamage(damage);
            Destroy(gameObject);
        }
    }

}

