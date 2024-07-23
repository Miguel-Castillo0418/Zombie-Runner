using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SaveScene : MonoBehaviour
{
    [SerializeField] public string level1;
    [SerializeField] public string level2;
    // Start is called before the first frame update
    public void OnNewGameButton()
    {
        delete();
        SceneManager.LoadScene(level1);
    }

    public void OnContinueButton()
    {
        if (PlayerPrefs.GetInt("currentLevel", 0) == 0)
        {
            Debug.Log("lvl 1");
            SceneManager.LoadScene(level1);
        }
        if (PlayerPrefs.GetInt("currentLevel", 1) == 1)
        {
            Debug.Log("lvl 2");
            SceneManager.LoadScene(level2);
        }
    }
    public void delete()
    {
        PlayerPrefs.DeleteAll();
    }
}
