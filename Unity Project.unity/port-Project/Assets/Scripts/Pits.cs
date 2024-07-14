using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pits : MonoBehaviour
{
    // Start is called before the first frame update
   
    private void OnTriggerEnter(Collider other)
    {
        IDamage dmg = GetComponent<IDamage>();
        dmg.takeDamage(500);
    }
}
