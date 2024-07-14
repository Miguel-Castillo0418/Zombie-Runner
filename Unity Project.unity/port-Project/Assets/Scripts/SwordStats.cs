using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu]

public class SwordStats : ScriptableObject
{
    public GameObject SwordModel;
    public string swordID;
    public Material[] swordMaterials;
    [Range(1, 100)] public int swordDMG;
    public ParticleSystem enemyHitEffect;
    public AudioClip swingSound;
    [Range(0, 1)] public float swingVol;
}
