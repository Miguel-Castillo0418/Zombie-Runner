using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu]

public class gunStats : ScriptableObject
{
    public GameObject gunModel;
    [Range(1, 10)] public int shootDmg;
    [Range(15, 1000)] public int shootDist;
    [Range(0.1f, 3)] public float shootRate;
    [Range(5, 1000)] public int ammoCurr;
    [Range(5, 1000)] public int ammoMax;
    [Range(5, 1000)] public int magazineSize;
    public Material[] gunMaterials;
    public ParticleSystem hitEffect;
    public ParticleSystem enemyHitEffect;
    public AudioClip shootSound;
    [Range(0, 1)] public float shootVol;
}
