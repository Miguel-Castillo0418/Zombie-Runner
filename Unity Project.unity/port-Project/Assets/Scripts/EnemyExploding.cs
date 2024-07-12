using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Unity.Mathematics;
using UnityEngine;

public class EnemyExploding : MonoBehaviour
{
   public EnemyAI _aiScript;
    bool gonExplode;
    void Start()
    {
        _aiScript = GetComponent<EnemyAI>();

    }

    public void Update() 
    {
            if (_aiScript.HP / _aiScript.maxHp < 0.5f)
            {
                gonExplode = true;
                GameObject fireVFX = Instantiate(_aiScript.onFire, transform.position, Quaternion.identity);
                fireVFX.transform.parent = transform;
                enabled = false;
            }
    }
    private void OnTriggerEnter(Collider other)
    {
        IDamage dmg = other.GetComponent<IDamage>();
        IKnockbackable _knock = other.GetComponent<IKnockbackable>();
        if (other.name == "Player")
        {
            if (!gonExplode)
            {
                Debug.Log(other.transform.name);
                dmg.takeDamage(_aiScript.damage);
                _knock.Knockback(_aiScript.lvl, _aiScript.damage);
            }
            else
            {
                dmg.takeExplosiveDamage(_aiScript.damage*10);
                _knock.Knockback(_aiScript.lvl, _aiScript.damage);
                _aiScript.Explode();
                _aiScript.takeDamage(_aiScript.maxHp);
            }
        }
    }
}
