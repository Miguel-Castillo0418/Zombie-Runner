using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExplosiveBarrel : MonoBehaviour, IDamage
{
    public GameObject Barrel, Explosion;
    public float range;
    public int HP = 10;  // Add HP variable
    public int explosionDamage = 100; // Damage the barrel deals to enemies upon explosion

    private void Awake()
    {
        Barrel.SetActive(true);
        Explosion.SetActive(false);
    }

    public void Explode()
    {
        AudioManager.instance.explosionSound();
        Barrel.SetActive(false);
        Explosion.SetActive(true);

        Collider[] enemies = Physics.OverlapSphere(this.transform.position, range);
        foreach (Collider enemy in enemies)
        {
            if (enemy.GetComponent<EnemyAI>() != null)
            {
                enemy.GetComponent<EnemyAI>().takeFireDamage(explosionDamage); 
            }
        }

        //sources.Play();
    }

    public void takeDamage(float amount)
    {
        HP -= (int)amount;
        if (HP <= 0)
        {
            Explode();
        }
    }
    public void takeFireDamage(float amount) { }
    public void takePoisonDamage(float amount) { }
    public void takeElectricDamage(float amount) { }
    public void takeExplosiveDamage(float amount) { }
    public IEnumerator applyDamageOverTime(float amount, float duration, GameObject VFX) //the total damage over time in seconds
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
        Destroy(VFX);
    }
    private void OnDrawGizmos()
    {
        Gizmos.DrawWireSphere(transform.position, range);
    }

    
}
