using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IElementalDamage
{
    void takeFireDamage(float amount);
    void takePoisonDamage(float amount);
    void takeElectricDamage(float amount);
    void takeExplosiveDamage(float amount);
    IEnumerator applyDamageOverTime(float amount, float duration, GameObject VFX); //the total damage over time in seconds

}
