using System;
using System.Collections;
using System.Collections.Generic;
using System.Xml.Serialization;
using Unity.VisualScripting;
using UnityEngine;

public class DamageIndicator : MonoBehaviour
{
    private const float lifeTimer = 8.0f;
    private float timer = lifeTimer;


    private CanvasGroup canvasGroup_m = null;
    private RectTransform rect_m = null;
    public Transform Target { get; private set; } = null;
    private Transform player = null;
    private IEnumerator CountDown_m = null;
    private Action unRegister = null;
    private Quaternion TargetRot = Quaternion.identity;
    private Vector3 TargetPos = Vector3.zero;
    private Vector3 dir = Vector3.zero;

    public CanvasGroup CanvasGroup
    {
        get
        {
            if (canvasGroup_m == null)
            {
                canvasGroup_m = GetComponent<CanvasGroup>();
                if (canvasGroup_m == null)
                {
                    canvasGroup_m = gameObject.AddComponent<CanvasGroup>();
                }
            }
            return canvasGroup_m;
        }
    }
    public RectTransform Rect
    {
        get
        {
            if (rect_m == null)
            {
                rect_m = GetComponent<RectTransform>();
                if (rect_m == null)
                {
                    rect_m = gameObject.AddComponent<RectTransform>();
                }
            }
            return rect_m;
        }
    }

    public void Register(Transform target, Transform player, Action unRegister)
    {
        this.Target = target;
        this.player = player;
        this.unRegister = unRegister;
        StartCoroutine(RotateIndicator());
        Timer();
    }

    IEnumerator RotateIndicator()
    {
        while (enabled)
        {
            if (Target)
            {
                TargetPos = Target.position;
                TargetRot = Target.transform.rotation;

            }
            dir = player.transform.position - TargetPos;
            TargetRot = Quaternion.LookRotation(TargetPos - player.position);
            TargetRot.z = -TargetRot.y;
            TargetRot.x = 0;
            TargetRot.y = 0;
            Vector3 dir2 = new Vector3(0, 0, player.transform.eulerAngles.y);
            Rect.localRotation = TargetRot * Quaternion.Euler(dir2);
            yield return null;

        }
    }
    private void Timer()
    {
        if (CountDown_m != null)
        {
            StopCoroutine(CountDown_m);
        }
        CountDown_m = Countdown();
        StartCoroutine(CountDown_m);
    }
    IEnumerator Countdown()
    {
        while (CanvasGroup.alpha < 1.0f)
        {
            CanvasGroup.alpha += 4 * Time.deltaTime;
            yield return null;
        }
        while (timer > 0)
        {
            timer--;
            yield return new WaitForSeconds(1);
        }
        while (CanvasGroup.alpha > 0.0f)
        {
            CanvasGroup.alpha -= 2 * Time.deltaTime;
            yield return null;
        }
        unRegister();
        Destroy(gameObject);
    }
    public void Restart()
    {
        timer = lifeTimer;
        Timer();
    }
}
