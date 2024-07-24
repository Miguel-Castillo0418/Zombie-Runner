using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Computer : MonoBehaviour
{

    [SerializeField] GameObject computer;
    [SerializeField] GameObject compText;
    [SerializeField] GameObject puzzleTxt;
    [SerializeField] GameObject gameComputer;
    [SerializeField] GameObject key;
    [SerializeField] Animator keyAnim;
    [SerializeField] AudioSource pipeWin;
    [SerializeField] public AudioClip winSound;
    [SerializeField] AudioClip pipeClick;
    public GameObject PipeHolder;
    public GameObject[] Pipes;
    public int totalPipes = 0;
    private int correctPipes = 0;
    private bool isWon = false;
    private void Awake()
    {
        keyAnim = key.GetComponent<Animator>();

        totalPipes = PipeHolder.transform.childCount;
        Pipes = new GameObject[totalPipes];
        for (int i = 0; i < Pipes.Length; i++)
        {
            Pipes[i] = PipeHolder.transform.GetChild(i).gameObject;
        }

    }
    
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        ComputerGame();
    }
    public void ComputerGame()
    {
            //Find distance
            float computerDist = Vector3.Distance(computer.transform.position, gameManager.instance.player.transform.position);
            if (computerDist < 3.2)
            {
                //Set text to true
                compText.SetActive(true);
                if (Input.GetButtonDown("Shop"))
                {
                    if (gameManager.instance.menuActive == null && !isWon)
                    {
                      game();
                    }
                    else if (gameManager.instance.menuActive == gameComputer)
                    {
                        gameManager.instance.stateUnpause();

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
            pipeWin.PlayOneShot(winSound);
            Instantiate(key, computer.transform);
            keyAnimation();
            isWon = true;
        }
    }
    public void badMove()
    {
        correctPipes -= 1;
    }
    public void game()
    {
        gameManager.instance.statePause();
        gameManager.instance.menuActive = gameComputer;
        gameManager.instance.menuActive.SetActive(true);
    }
    IEnumerator keyAnimation()
    {
        yield return new WaitForSeconds(.4F);
        keyAnim.Play("KeyMove");
    }
        public void clickAud()
    {
        pipeWin.PlayOneShot(pipeClick, 0.5f);
    }
}
