using UnityEngine;

public class SpawnIndicator : MonoBehaviour
{
    public static SpawnIndicator Instance;

    private void Start()
    {
        Invoke("Register", Random.Range(0, 4));
    }
    public void Register()
    {
        if (DamageIndicatorController.Instance != null && !DamageIndicatorController.CheckIfObjectInSight(this.transform))
        {
            DamageIndicatorController.CreateIndicator(this.transform);
        }
    }
}
