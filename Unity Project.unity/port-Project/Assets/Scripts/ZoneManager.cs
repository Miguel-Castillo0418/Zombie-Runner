using UnityEngine;

public class ZoneManager : MonoBehaviour
{
    public Spawner[] zoneOneSpawners;
    public Spawner[] zoneTwoSpawners;
    public Spawner[] zoneThreeSpawners;
    public Spawner[] zoneFourSpawners;

    [SerializeField] private Spawner[] activeZone;

    

    public void ActivateZone(Spawner[] newActiveZone)
    {
        if (activeZone != null)
        {
            DeactivateZone(activeZone);
        }

        activeZone = newActiveZone;
        foreach (var spawner in activeZone)
        {
            spawner.SetActive(true);
        }
    }

    public void DeactivateZone(Spawner[] zone)
    {
        foreach (var spawner in zone)
        {
            spawner.SetActive(false);
        }
    }
}
