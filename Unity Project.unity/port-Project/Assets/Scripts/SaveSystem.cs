using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using TMPro;
using UnityEngine;



public class SaveSystem : MonoBehaviour
{
    [SerializeField] GameObject[] collectibleArr;
    List<bool> collectibleStates = new List<bool>();
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
        saveCollectibles();

    }
    //Put all methods needed to be loaded in the method below to only need to call one method
    //this method stays at the top

    public void loadData()
    {
        loadCollectibles();
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
    public void saveCollectibles()
    {

        for (int i = 0; i < collectibleArr.Length; i++)
        {
            string name = collectibleArr[i].name;
            int isActive = collectibleArr[i].activeSelf ? 1 : 0;
            PlayerPrefs.SetInt(name, isActive);
        }
        PlayerPrefs.Save();
    }

    public void loadCollectibles()
    {
        for (int i = 0; i < collectibleArr.Length; i++)
        {
            string name = collectibleArr[i].name;

            if (PlayerPrefs.GetInt(name, 0) == 1)
            {
                collectibleArr[i].SetActive(true);
            }
            else
            {
                collectibleArr[i].SetActive(false);
            }
        }
    }
}

