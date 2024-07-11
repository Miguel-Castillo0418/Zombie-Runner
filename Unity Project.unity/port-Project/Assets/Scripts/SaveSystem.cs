using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class SaveSystem : MonoBehaviour
{
    public static SaveSystem instance;
    public void SaveData(float hp)
    {
        PlayerPrefs.SetFloat("playerHP", hp);
        PlayerPrefs.Save();
        Debug.Log(hp);
    }
    public float LoadData()
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
    public void delete()
    {
        PlayerPrefs.DeleteAll();
    }
}
