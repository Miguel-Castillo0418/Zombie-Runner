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
    //[SerializeField] GameObject testhintText;
    [SerializeField] GameObject hintobject;
    [SerializeField] GameObject shopObj;
    [SerializeField] GameObject shopText;
    public Image playerHPBar;

    public GameObject player;
    public PlayerController playerScript;

    public bool isHint;
    public bool isPaused;
    int enemycount;
    public int points;
    int round;

    // Start is called before the first frame update
    void Awake()
    {
        instance = this;
        points = 400;
        player = GameObject.FindWithTag("Player");
        playerScript = player.GetComponent<PlayerController>();
        updateRound(1);
        pointsCountText.text = points.ToString("F0");
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
<<<<<<< Updated upstream
       // showHints();
        updateAmmo();
        showShop();
        pointsCountText.text = points.ToString("F0");
=======
        //showHints();
        updateAmmo();
        showShop();
        pointsCountText.text = points.ToString();
>>>>>>> Stashed changes
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
            round++;
            if (round != 100)
            {
                updateRound(round);
                //startnext round or restart spawners
            }
            else
            {

                statePause();
                menuActive = menuWin;
                menuActive.SetActive(isPaused);
            }

        }
    }
    public void updateRound(int amount)
    {
        round = amount;
        roundCountText.text = round.ToString("F0");
    }
    public void youLose()
    {
        Debug.Log("Lose");
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
    public void showHints()
    {
        float hint = Vector3.Distance(hintobject.transform.position, gameManager.instance.player.transform.position);
        if (hint < 3)
        {
<<<<<<< Updated upstream
          //  testhintText.SetActive(true);
=======
            //testhintText.SetActive(true);
>>>>>>> Stashed changes

        }
        else
        {
<<<<<<< Updated upstream
          //  testhintText.SetActive(false);
=======
           // testhintText.SetActive(false);
>>>>>>> Stashed changes
        }
    }
    public void updateAmmo()
    {
        ammoMagCountText.text = playerScript.currentAmmo.ToString("F0");
        ammoStockCountText.text = playerScript.stockAmmo.ToString("F0");
    }

    public void shop()
    {
        statePause();
        menuActive = menuShop;
        menuActive.SetActive(isPaused);
    }
    public void showShop() 
    {
        float shopDist = Vector3.Distance(shopObj.transform.position, gameManager.instance.player.transform.position);
        if (shopDist < 3 )
        {
            shopText.SetActive(true);
            if (Input.GetButtonDown("Shop"))
            {
                shopText.SetActive(false);
                if (menuActive == null)
                {
                    
                    shop();
                    
                }
                else if (menuActive == menuShop)
                {
                    stateUnpause();
                    shopText.SetActive(true);
                }
            }
        }
        else
        {
            shopText.SetActive(false);
        }
    }
}
