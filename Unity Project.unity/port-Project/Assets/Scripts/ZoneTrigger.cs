using KeyCards;
using Unity.VisualScripting;
using UnityEngine;

public class ZoneTrigger : MonoBehaviour
{
    public ZoneManager zoneManager;
    public Spawner[] targetZoneSpawners;
    public Spawner[] previousZoneSpawners;
    public KeycardInventory inventory;
    public GameObject keyCard;

    void Start()
    {
        keyCard = GameObject.FindWithTag("keycardInv");
        inventory = keyCard.GetComponent<KeycardInventory>();
    }

    void OnTriggerEnter(Collider other)
    {
        if (name == "Zone2Trigger" && inventory.hasKeyCard)
        {
            zoneManager.ActivateZone(targetZoneSpawners);
        }
        else if (other.CompareTag("Player") && name != "Zone2Trigger")
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
