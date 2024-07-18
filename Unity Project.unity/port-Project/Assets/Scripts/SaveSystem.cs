using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class SaveSystem : MonoBehaviour
{
    [SerializeField] GameObject[] collectibles;
    public SaveSystem instance;
    public void SaveHP(float hp)
    {
        PlayerPrefs.SetFloat("playerHP", hp);
        PlayerPrefs.Save();
        Debug.Log(hp);
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
    public void saveCollectibles() { 
    
    }
}
