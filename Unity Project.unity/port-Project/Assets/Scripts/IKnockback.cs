using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IKnockbackable
{
    void Knockback(int lvl=0,int damage=0);
    IEnumerator ApplyKnockback(Transform objectTransform, Vector3 targetPosition, float duration);

}
