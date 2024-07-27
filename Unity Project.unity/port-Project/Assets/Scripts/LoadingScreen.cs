using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class LoadingScreen : MonoBehaviour
{
    public static LoadingScreen instance;
    [SerializeField] GameObject loadingScreen;
    [SerializeField] Image loadFill;

    public void loadScene(int sceneID)
    {
        StartCoroutine(loadSceneAsync(sceneID));

    }
    IEnumerator loadSceneAsync(int sceneID)
    {
        AsyncOperation operation = SceneManager.LoadSceneAsync(sceneID);
       
        loadingScreen.SetActive(true);
        while(!operation.isDone)
        {
            float progressBar = Mathf.Clamp01(operation.progress / 0.9f);
            loadFill.fillAmount = progressBar;
            yield return null;
        }
        
    }
}
