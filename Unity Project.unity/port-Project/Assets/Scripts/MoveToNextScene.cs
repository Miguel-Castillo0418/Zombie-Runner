using UnityEngine;
using UnityEngine.SceneManagement;

public class MoveToNextScene : MonoBehaviour
{
    public int nextSceneLoad;
    public MenuController gameLevelManager;
    public LoadingScreen loadingScreen;
    public GameObject nextLevel;
    public GameObject menuActive;
    public static MoveToNextScene instance;



    void Start()
    {
        nextSceneLoad = SceneManager.GetActiveScene().buildIndex + 1;
        gameLevelManager = FindObjectOfType<MenuController>();
        instance = this;
    }

    public void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Player")
        {
            if (SceneManager.GetActiveScene().buildIndex == 2)
            {
                gameManager.instance.DisplayWinMenu();
                Debug.Log("YOU WIN THE GAME");
            }
            else
            {
                nextlevel();
               // gameLevelManager.UnlockLevel(nextSceneLoad);
                PlayerPrefs.SetInt("levelAt", nextSceneLoad);
                PlayerPrefs.Save();
            }
        }
    }

    public void nextlevel()
    {
        gameManager.instance.statePause();
        menuActive = nextLevel;
        menuActive.SetActive(nextLevel);
       

    }
}
