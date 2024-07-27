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
        SceneManager.LoadScene("1-MainMenu");
    }
    public void creditsMenu()
    {
        gameManager.instance.loading();
        gameManager.instance.stateUnpause();
        SceneManager.LoadScene("6-EndCredits");

    }
    public void creditsMainMenu()
    {
        SceneManager.LoadScene("1-MainMenu");
        Cursor.visible = true;
        Cursor.lockState = CursorLockMode.Confined;
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