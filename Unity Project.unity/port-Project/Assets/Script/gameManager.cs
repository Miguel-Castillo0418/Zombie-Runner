using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class gameManager : MonoBehaviour
{
    public static gameManager instance;
    [SerializeField] GameObject menuActive;
    [SerializeField] GameObject menuPause;
    [SerializeField] GameObject menuWin;
    [SerializeField] GameObject menuLose;
    [SerializeField] GameObject menuShop;
    [SerializeField] TMP_Text enemyCountText;
    [SerializeField] TMP_Text roundCountText;
    [SerializeField] TMP_Text pointsCountText;
    [SerializeField] TMP_Text ammoMagCountText;
    [SerializeField] TMP_Text ammoStockCountText;
    public Image playerHPBar;

    public GameObject player;
    public PlayerController playerScript;

    public bool isPaused;
    int enemycount;
    int points;

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
        if (Input.GetButtonDown("Cancel"))
        {
            if (menuActive == null)
            {
                statePause();
                menuActive = menuPause;
                menuActive.SetActive(isPaused);
            }
            else if (menuActive == menuPause)
            {
                stateUnpause();
            }
        }
        if(Input.GetButtonDown("Shop"))
            {
            if(menuActive == null)
            {
                shop();
            }
            else if(menuActive == menuShop)
            {
                stateUnpause();
            }
        }
    }

    public void statePause()
    {
        isPaused = !isPaused;
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
        menuActive.SetActive(isPaused);
        menuActive = null;

    }
    public void updateGameGoal(int amount)
    {
        enemycount += amount;
        enemyCountText.text = enemycount.ToString("F0");
        if (enemycount <= 0)
        {
            statePause();
            menuActive = menuWin;
            menuActive.SetActive(isPaused);

        }
    }
    public void youLose()
    {
        statePause();
        menuActive = menuLose;
        menuActive.SetActive(isPaused);

    }
    public void addPoints(int amount)
    {
        points += amount;
        pointsCountText.text = points.ToString("F0");
    }
    public void removePoints(int amount)
    {
        points -= amount;
        pointsCountText.text = points.ToString("F0");
    }
    public void shop()
    {
        statePause();
        menuActive = menuShop;
        menuActive.SetActive(isPaused);
    }
}
