using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;


public class buttonFunctions : MonoBehaviour
{

    public void resume()
    {
        AudioManager.instance.clickSound("click");
        gameManager.instance.stateUnpause();
    }

    public void restart()
    {
        AudioManager.instance.clickSound("click");
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        gameManager.instance.stateUnpause();
    }

    public void mainMenu()
    {
        AudioManager.instance.clickSound("click");
        SceneManager.LoadScene("Level1");
    }
    public void creditsMenu()
    {
        SceneManager.LoadScene("Level1");
    }

    public void nextLevel()
    {
        gameManager.instance.loading();
        SceneManager.LoadScene(MoveToNextScene.instance.nextSceneLoad);
        gameManager.instance.stateUnpause();
    }

    public void quit()
    {
        AudioManager.instance.clickSound("click");
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#else
        Application.Quit();
#endif
    }
}