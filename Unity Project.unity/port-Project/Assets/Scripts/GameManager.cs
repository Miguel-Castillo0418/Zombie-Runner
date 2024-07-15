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

    [SerializeField] GameObject loadingScreen;

    [SerializeField] GameObject gameComputer;
    [SerializeField] TMP_Text enemyCountText;
    [SerializeField] TMP_Text roundCountText;
    [SerializeField] TMP_Text pointsCountText;
    [SerializeField] TMP_Text ammoMagCountText;
    [SerializeField] TMP_Text ammoStockCountText;
    [SerializeField] GameObject testhintText;
    [SerializeField] GameObject hintobject;
    [SerializeField] GameObject shopObj;
    [SerializeField] GameObject shopText;
    [SerializeField] Wave tempRound;
    [SerializeField] WaveManager waveManager;
    [SerializeField] GameObject computer;
    [SerializeField] GameObject compText;
    [SerializeField] GameObject key;
    [SerializeField] Animator keyAnim;
    //[SerializeField] GameObject doorObj1;
    //[SerializeField] GameObject doorText;
    //[SerializeField] GameObject doorText2;
    [SerializeField] private float attackRate;
    [SerializeField] private float drainTime = 0.25f;
    [SerializeField] private Gradient PlayerHPBarGradient;
    [SerializeField] GameObject puzzleTxt;
    [SerializeField] public GameObject savesystemobj;
    public float hpTarget = 1f;
    public Coroutine drainHealthBar;

    public Image playerHPBar;


    public GameObject player;
    public PlayerController playerScript;
    private SaveSystem saveSystem;

    public bool isHint;
    public bool isPaused;
    private bool isWon = false;
    //bool doorPurchased;
    int enemycount;
    [SerializeField] public int points;
    int round;

    public GameObject PipeHolder;
    public GameObject[] Pipes;
    public int totalPipes = 0;
    private int correctPipes = 0;

    private int enemyCount;

    // Start is called before the first frame update
    void Awake()
    {
        //doorPurchased = false;
        instance = this;
        savesystemobj = new GameObject("savesystemobj");
        saveSystem = savesystemobj.AddComponent<SaveSystem>();
        pointsCountText.text = points.ToString("F0");
        player = GameObject.FindWithTag("Player");
        shopObj = GameObject.FindWithTag("ShopObj");
        shopText = GameObject.FindWithTag("ShopTxt");
        computer = GameObject.FindWithTag("Computer");
        compText = GameObject.FindWithTag("CompTxt");
        playerScript = player.GetComponent<PlayerController>();
        updateRound(1);
        CheckHealthBar();
        keyAnim = key.GetComponent<Animator>();
        totalPipes = PipeHolder.transform.childCount;
        Pipes = new GameObject[totalPipes];
        for (int i = 0; i < Pipes.Length; i++)
        {
            Pipes[i] = PipeHolder.transform.GetChild(i).gameObject;
        }
    }
    void Start()
    {
        //AudioManager.instance.playMusic("Song");
        points = saveSystem.LoadPoints();

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
        // showHints();
        updateAmmo();
        showShop();
        ComputerGame();
        // buyDoor();
        pointsCountText.text = points.ToString("F0");
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
        //    enemyCount += amount;
        //    enemyCountText.text = enemyCount.ToString("F0");

        //    if (enemyCount <= 0 && round <= tempRound.totalRounds)
        //    {
        //        Debug.Log("test");
        //        round++;
        //        Debug.Log(round);
        //        updateRound(round);
        //        if (round <= tempRound.totalRounds)
        //        {
        //            Debug.Log(round);
        //            // Start the next round or restart spawners
        //            waveManager.StartNextWave();
        //        }
        //        else
        //        {
        //            statePause();
        //            menuActive = menuWin;
        //            menuActive.SetActive(isPaused);
        //        }
        //    }
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

    public void loading()
    {
        Debug.Log("load");
        statePause();
        menuActive = loadingScreen;
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

            testhintText.SetActive(true);


        }
        else
        {

            testhintText.SetActive(false);

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

        if (shopDist < 2)

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

    //public void buyDoor()

    //{
    //  float doorDistance = Vector3.Distance(doorObj1.transform.position, gameManager.instance.player.transform.position);
    // if (doorDistance < 3)
    //{
    //if (!doorPurchased) {
    //  doorText.SetActive(true);
    // if (Input.GetButtonDown("Door"))
    // {

    // if (points >= 2000)
    // {
    //  points -= 2000;
    // pointsCountText.text = points.ToString("F0");
    //  doorPurchased = true;
    //  doorText.SetActive(false);
    //  WaveManager.instance.OnDoorPurchased();
    //      }
    //    }
    //  }
    //     else
    //    {
    //      doorText2.SetActive(true);
    //  }

    //   }
    //  else
    //   {
    //     doorText.SetActive(false);
    //     doorText2.SetActive(false);
    //  }
    //  }
    public IEnumerator DrainHealthBar()
    {
        float fill = playerHPBar.fillAmount;
        float timePassed = 0f;
        Debug.Log(hpTarget);
        while (timePassed < drainTime)
        {
            timePassed += Time.deltaTime;
            playerHPBar.fillAmount = Mathf.Lerp(fill, hpTarget, (timePassed / drainTime));
            yield return null;
        }
    }
    public void CheckHealthBar()
    {
        playerHPBar.color = PlayerHPBarGradient.Evaluate(hpTarget);
    }
    public void game()
    {
        statePause();
        menuActive = gameComputer;
        menuActive.SetActive(isPaused);
    }
    public void ComputerGame()
    {
        float computerDist = Vector3.Distance(computer.transform.position, gameManager.instance.player.transform.position);
        if (computerDist < 3.2)
        {
            compText.SetActive(true);
            if (Input.GetButtonDown("Shop"))
            {
                if (menuActive == null && !isWon)
                {
                    game();
                }
                else if (menuActive == gameComputer)
                {
                    stateUnpause();

                }
            }
        }
        else
        {
            compText.SetActive(false);
        }
    }

    public void goodMove()
    {
        correctPipes += 1;
        if (correctPipes == totalPipes)
        {
            puzzleTxt.SetActive(true);
            Instantiate(key, computer.transform);
            keyAnimation();
            isWon = true;
        }
    }
    public void badMove()
    {
        correctPipes -= 1;
    }
    IEnumerator keyAnimation()
    {
        yield return new WaitForSeconds(.4F);
        keyAnim.Play("KeyMove");
    }

    public void DisplayWinMenu()
    {
        statePause();
        menuActive = menuWin;
        menuActive.SetActive(isPaused);
    }
}