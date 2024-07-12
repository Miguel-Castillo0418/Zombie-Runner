using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IKnockbackable
{
    void Knockback();
    IEnumerator ApplyKnockback(Transform objectTransform, Vector3 targetPosition, float duration);

}
