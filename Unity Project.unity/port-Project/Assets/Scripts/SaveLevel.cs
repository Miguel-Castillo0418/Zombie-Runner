using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SaveLevel : MonoBehaviour
{
    public bool Level1, Level2;
    void Start()
    {
        if (Level1 == true)
        {
            PlayerPrefs.SetInt("currentLevel", 0);
            PlayerPrefs.Save();
            Debug.Log("lvl 1 set");
        }
        if (Level2 == true)
        {
            PlayerPrefs.SetInt("currentLevel", 1);
            PlayerPrefs.Save();
            Debug.Log("lvl 2 set");
        }
    }
}
