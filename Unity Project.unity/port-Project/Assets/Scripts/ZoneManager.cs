using UnityEngine;

public class ZoneManager : MonoBehaviour
{
    public Spawner[] zoneOneSpawners;
    public Spawner[] zoneTwoSpawners;
    public Spawner[] zoneThreeSpawners;
    public Spawner[] zoneFourSpawners;

    [SerializeField] private Spawner[] activeZone;


    private void Awake()
    {
        DeactivateZone(zoneOneSpawners);
        DeactivateZone(zoneTwoSpawners);
        DeactivateZone(zoneThreeSpawners);
        DeactivateZone(zoneFourSpawners);

    }
    public void ActivateZone(Spawner[] newActiveZone)
    {
        if (activeZone != null)
        {
            DeactivateZone(activeZone);
        }

        activeZone = newActiveZone;
        foreach (var spawner in activeZone)
        {
            spawner.gameObject.SetActive(true);
        }
    }

    public void DeactivateZone(Spawner[] zone)
    {
        foreach (var spawner in zone)
        {
            spawner.gameObject.SetActive(false);
        }
    }
}
