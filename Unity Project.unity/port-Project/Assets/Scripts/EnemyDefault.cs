using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Unity.Mathematics;
using UnityEngine;

public class EnemyDefault : MonoBehaviour
{
   public EnemyAI _aiScript;
    void Start()
    {
        _aiScript = GetComponent<EnemyAI>();

    }

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
    
}
