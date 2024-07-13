using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Unity.Mathematics;
using UnityEngine;

public class EnemyDefault : MonoBehaviour, IElementalDamage
{
   public EnemyAI _aiScript;
    void Start()
    {
        _aiScript = GetComponent<EnemyAI>();

    }
    //melee dealing damage
    private void OnTriggerEnter(Collider other)
    {
        IDamage dmg = other.GetComponent<IDamage>();
        IKnockbackable _knock = other.GetComponent<IKnockbackable>();
        if (other.name == "Player")
        {
                Debug.Log(other.transform.name);
                dmg.takeDamage(_aiScript.damage);
                _knock.Knockback(_aiScript.lvl, _aiScript.damage);
        }
    }
    //for IElementalDamage
    public void takeFireDamage(float amount)
    {
        GameObject fireVFX = Instantiate(_aiScript.onFire, transform.position, Quaternion.identity);
        fireVFX.transform.parent = transform;
        StartCoroutine(applyDamageOverTime(amount, 5.0f, fireVFX));
    }
    public void takePoisonDamage(float amount)
    {
        Vector3 newPosition = Vector3.zero + Vector3.up * 1.5f;
        GameObject poisonVFX = Instantiate(_aiScript.poisoned, newPosition, Quaternion.identity);
        poisonVFX.transform.parent = transform;
        poisonVFX.transform.localPosition = newPosition;
        StartCoroutine(applyDamageOverTime(amount, 5.0f, poisonVFX));
    }
    public void takeElectricDamage(float amount)
    {
        Vector3 newPosition = Vector3.zero + Vector3.forward * 1.3f + Vector3.down * 0.002f;
        GameObject ElecVFX = Instantiate(_aiScript.electrified, transform.position, Quaternion.identity);
        ElecVFX.transform.parent = transform.Find("Z_Body");
        ElecVFX.transform.localRotation = Quaternion.identity;
        ElecVFX.transform.localPosition = newPosition;
        Vector3 newScale = Vector3.one * 0.4f;
        ElecVFX.transform.localScale = newScale;
        StartCoroutine(applyDamageOverTime(amount, 5.0f, ElecVFX));
    }
    public void takeExplosiveDamage(float amount)
    {
        GameObject fireVFX = Instantiate(_aiScript.onFire, transform.position, Quaternion.identity);
        fireVFX.transform.parent = transform;
        StartCoroutine(applyDamageOverTime(amount, 5.0f, fireVFX));
    }
    public IEnumerator applyDamageOverTime(float amount, float duration, GameObject VFX) //the total damage over time in seconds
    {
        float timer = 0f;
        float damagePerSec = amount / duration;

        while (timer < duration)
        {
            float damagePerFrame = damagePerSec * Time.deltaTime;
            _aiScript.takeDamage(damagePerFrame);
            timer += Time.deltaTime;
            yield return null;
        }
        Destroy(VFX);
    }
}
