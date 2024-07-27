using UnityEngine;

public class TogglePrefab : MonoBehaviour
{
    public GameObject prefab;

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (prefab != null)
            {
                prefab.SetActive(!prefab.activeSelf);
            }
        }
    }
}
