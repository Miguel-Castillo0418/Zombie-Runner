using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IDamage
{
    void takeDamage(float amount);
    void takeFireDamage(float amount);
    void takePoisonDamage(float amount);
    void takeElectricDamage(float amount);
    void takeExplosiveDamage(float amount);

}
