using UnityEngine;

public class ZoneTrigger : MonoBehaviour
{
    public ZoneManager zoneManager;
    public Spawner[] targetZoneSpawners;
    public Spawner[] previousZoneSpawners;

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            zoneManager.ActivateZone(targetZoneSpawners);
        }
    }

    void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player") && previousZoneSpawners != null)
        {
            zoneManager.ActivateZone(previousZoneSpawners);
        }
    }
}
