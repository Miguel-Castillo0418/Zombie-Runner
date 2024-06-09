using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;

    //[SerializeField] GameObject menuPause;
    //[SerializeField] GameObject menuActive;
    //[SerializeField] GameObject menuWin;
    //[SerializeField] GameObject menuLose;

    ////Enemy Counter
    //[SerializeField] TMP_Text enemyCountText;

    ////Player Health Bar
    //public Image playerHPBar;

    public GameObject player;
    public PlayerController playerScript;

    public bool isPaused;
    int enemyCount;

    // Start is called before the first frame update
    void Awake()
    {
        instance = this;
        player = GameObject.FindWithTag("Player");
        playerScript = player.GetComponent<PlayerController>();
    }

    // Update is called once per frame
    void Update()
    {
    //    if (Input.GetButtonDown("Cancel"))
    //    {

    //        if (menuActive == null)
    //        {
    //            statePaused();
    //            menuActive = menuPause;
    //            menuActive.SetActive(isPaused);
    //        }

    //        else if (menuActive == menuPause)
    //        {
    //            stateUnpause();
    //        }
       // }
    }

    public void statePaused()
    {
        //isPaused = !isPaused;
        Time.timeScale = 0;
        Cursor.visible = true;
        Cursor.lockState = CursorLockMode.Confined;
    }

    public void stateUnpause()
    {
        isPaused = !isPaused;
        Time.timeScale = 1;
        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;
        //menuActive.SetActive(isPaused);
        //menuActive = null;
    }

    public void updateGameGoal(int amount)
    {
        enemyCount += amount;
        //enemyCountText.text = enemyCount.ToString("F0");

        if(enemyCount <= 0)
        {
            statePaused();
            //menuActive = menuWin;
            //menuActive.SetActive(isPaused);
        }
    }

    public void youLose()
    {
        statePaused();
        //menuActive = menuLose;
        //menuActive.SetActive(isPaused); 
    }
}
