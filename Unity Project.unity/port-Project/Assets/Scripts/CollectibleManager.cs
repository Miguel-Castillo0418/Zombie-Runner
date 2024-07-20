using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class CollectibleManager : MonoBehaviour
{
   
    [SerializeField] private GameObject[] collectibleArr;

    public static CollectibleManager instance;

    void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }

        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    void OnDestroy()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }

    void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        // Load collectibles when a scene is loaded
        LoadCollectibles();
    }

    public void SaveData()
    {
        SaveCollectibles();
        // Add other save methods as needed
    }

    public void LoadData()
    {
        LoadCollectibles();
        // Add other load methods as needed
    }

    private void SaveCollectibles()
    {
        for (int i = 0; i < collectibleArr.Length; i++)
        {
            string name = collectibleArr[i].name;
            int isActive = collectibleArr[i].activeSelf ? 1 : 0;
            PlayerPrefs.SetInt(name, isActive);
        }
        PlayerPrefs.Save();
    }

    private void LoadCollectibles()
    {
        for (int i = 0; i < collectibleArr.Length; i++)
        {
            Transform item = gameObject.transform.Find(collectibleArr[i].name);
            if (item.gameObject != null)
            {
                if (PlayerPrefs.GetInt(collectibleArr[i].name, 0) == 1)
                {
                    item.gameObject.SetActive(true);
                }
                else
                {
                    item.gameObject.SetActive(false);
                }
            }
            
        }
    }
}
