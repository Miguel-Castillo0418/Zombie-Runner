using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class DamageIndicator : MonoBehaviour
{
    [SerializeField] public Vector3 DamageLocation;
    [SerializeField] public Transform PlayerObject;
    [SerializeField] public Transform DamageImagePivot;
    public CanvasGroup DamageImageCanvas;
    public float fadeStartTime, fadeTime;
    float maxFadeTime;

    void Start()
    {
        maxFadeTime = fadeTime;
    }
    void Update()
    {
        if (fadeStartTime > 0)
        {
            fadeStartTime -= Time.deltaTime;
        }
        else
        {
            fadeTime -= Time.deltaTime;
            DamageImageCanvas.alpha = fadeTime / maxFadeTime;
            if(fadeTime <= 0)
            {
                Destroy(gameObject);
            }
        }
        DamageLocation.y = PlayerObject.position.y;
        Vector3 Direction = (DamageLocation - PlayerObject.position).normalized;
        float angle = (Vector3.SignedAngle(Direction, PlayerObject.forward, Vector3.up));
        DamageImagePivot.transform.localEulerAngles = new Vector3(0, 0, angle);
    }


}
