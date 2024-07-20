using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
public class SaveSystem : MonoBehaviour
{
    public static SaveSystem instance;
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
    }
    //Put all methods needed to be saved in the method below to only need to call one method
    //this method stays at the top
    public void saveData()
    {

    }
    //Put all methods needed to be loaded in the method below to only need to call one method
    //this method stays at the top

    public void loadData()
    {
    }
    public void SaveHP(float hp)
    {
        PlayerPrefs.SetFloat("playerHP", hp);
        PlayerPrefs.Save();
        Debug.Log("Saved HP:" + hp);
    }
    public float LoadHP()
    {
        if (PlayerPrefs.HasKey("playerHP"))
        {
            return PlayerPrefs.GetFloat("playerHP"); ;
        }
        else
        {
            return PlayerController.instance.HP;
        }
    }
    public void SavePoints(int coins)
    {
        PlayerPrefs.SetInt("playerPoints", coins);
        PlayerPrefs.Save();
        Debug.Log(coins);
    }
    public int LoadPoints()
    {
        if (PlayerPrefs.HasKey("playerPoints"))
        {
            return PlayerPrefs.GetInt("playerPoints"); ;
        }
        else
        {
            return gameManager.instance.points;
        }
    }
    public void delete()
    {
        PlayerPrefs.DeleteAll();
    }

}

