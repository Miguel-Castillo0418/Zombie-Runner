using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Rendering;
using UnityEngine.Rendering.PostProcessing;


public class DamageEffect : MonoBehaviour
{
    public static DamageEffect Instance;
    public float dmgIntensity = 0;
    PostProcessVolume dmgVolume;
    Vignette dmgVignette;


    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else if (Instance != this)
        {
            Destroy(gameObject);
        }
    }
    void Start()
    {
        dmgVolume = GetComponent<PostProcessVolume>();
        dmgVolume.profile.TryGetSettings<Vignette>(out dmgVignette);

        dmgVignette.enabled.Override(false);
    }
    void Update()
    {

    }
    public IEnumerator damageEffect()
    {
        dmgIntensity = 0.4f;
        dmgVignette.enabled.Override(true);
        dmgVignette.intensity.Override(0.4f);

        yield return new WaitForSeconds(0.4f);

        while (dmgIntensity > 0)
        {
            dmgIntensity -= 0.01f;
            if (dmgIntensity < 0)
            {
                dmgIntensity = 0;
            }
            dmgVignette.intensity.Override(dmgIntensity);
            yield return new WaitForSeconds(0.1f);
        }
        dmgVignette.enabled.Override(false);
        yield return null;
    }
    //public IEnumerator ShowBloodOverlay()
    //{
    //    dmgIntensity = 0.4f;
    //    dmgVignette.enabled.Override(true);
    //    dmgVignette.intensity.Override(0.4f);


    //    yield return new WaitUntil(() => PlayerController.instance.HP > 30);
    //    while (dmgIntensity > 0)
    //    {
    //        dmgIntensity -= 0.01f;
    //        if (dmgIntensity < 0)
    //        {
    //            dmgIntensity = 0;
    //        }
    //        dmgVignette.intensity.Override(dmgIntensity);
    //        yield return new WaitForSeconds(0.1f);
    //    }
    //    dmgVignette.enabled.Override(false);
    //    yield return null;
    //}

}
