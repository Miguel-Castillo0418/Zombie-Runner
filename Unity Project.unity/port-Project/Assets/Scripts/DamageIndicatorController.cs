using System;
using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography.X509Certificates;
using UnityEngine;

public class DamageIndicatorController : MonoBehaviour
{
    public static DamageIndicatorController Instance;
    [Header("References")]
    [SerializeField] private DamageIndicator indicatorprefab = null;
    [SerializeField] private RectTransform holder = null;
    [SerializeField] private new Camera playercamera = null;
    [SerializeField] private Transform player = null;

    private Dictionary<Transform, DamageIndicator> Indicators = new Dictionary<Transform, DamageIndicator>();

    #region Delegates
    public static Action<Transform> CreateIndicator = delegate { };
    public static Func<Transform, bool> CheckIfObjectInSight = null;

    #endregion

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }
    private void OnEnable()
    {
        CreateIndicator += Create;
        CheckIfObjectInSight += Insight;
    }
    private void OnDisable()
    {
        CreateIndicator -= Create;
        CheckIfObjectInSight -= Insight;
    }
    private void Create(Transform target)
    {
        if (Indicators.ContainsKey(target))
        {
            Indicators[target].Restart();
            return;
        }
        DamageIndicator newIndicator = Instantiate(indicatorprefab, holder);
        newIndicator.Register(target, player, new Action(() => { Indicators.Remove(target); }));

        Indicators.Add(target, newIndicator);
    }
    private bool Insight(Transform t)
    {
        Vector3 point = playercamera.WorldToViewportPoint(t.position);
        return point.z > 0 && point.x > 0 && point.x < 1 && point.y > 0 && point.y < 1;
    }
}
