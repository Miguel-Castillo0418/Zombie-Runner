using UnityEngine;
using UnityEngine.SceneManagement;

public class MoveToNextScene : MonoBehaviour
{
    public int nextSceneLoad;
    public MenuController gameLevelManager;
    public LoadingScreen loadingScreen;

    void Start()
    {
        nextSceneLoad = SceneManager.GetActiveScene().buildIndex + 1;
        gameLevelManager = FindObjectOfType<MenuController>();
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
                //loadingScreen.loadScene(2);
                SceneManager.LoadScene(nextSceneLoad);
                gameLevelManager.UnlockLevel(nextSceneLoad);
                PlayerPrefs.SetInt("levelAt", nextSceneLoad);
                PlayerPrefs.Save();
            }
        }
    }
}
