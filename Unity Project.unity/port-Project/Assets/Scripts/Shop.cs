using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class Shop : MonoBehaviour
{

    [SerializeField] TMP_Text zombucksText;
    [SerializeField] TMP_Text HealthCostText;
    [SerializeField] TMP_Text SpeedCostText;
    [SerializeField] TMP_Text StrengthCostText;
    [SerializeField] TMP_Text RouletteCostText;
    [SerializeField] int healthCost;
    [SerializeField] int speedCost;
    [SerializeField] int strengthCost;
    [SerializeField] int rouletteCost;
    [SerializeField] PlayerController playerController;
    [SerializeField] Button healthbutton;
    int Zombucks;
    bool lowHealth;
    int speedCap;
    // Start is called before the first frame update

    private void Awake()
    {
        playerController = GameObject.FindWithTag("Player").GetComponent<PlayerController>();
    }
    void Start()
    {
        HealthCostText.text = healthCost.ToString();
        SpeedCostText.text = speedCost.ToString();
        StrengthCostText.text = strengthCost.ToString();
        RouletteCostText.text = rouletteCost.ToString();
        Zombucks = gameManager.instance.points;
        lowHealth = false;
        updateZombucks();
    }

    // Update is called once per frame
    void Update()
    {
        Zombucks = gameManager.instance.points;
        if (playerController.shopHP < playerController.HPorig)
        {
            lowHealth = true;
        }
        else if(playerController.shopHP == playerController.HPorig)
        {
            lowHealth = false;
        }
        updateZombucks();
    }
    public void updateZombucks()
    {
        zombucksText.text = Zombucks.ToString("F0");
    }
    public void healthButton()
    {
        if (Zombucks - healthCost >= 0 && lowHealth)
        {
            
            gameManager.instance.points -= healthCost;
            AudioManager.instance.purchaseSound("Purchase Sound");
            updateZombucks();
            playerController.IncreaseHealth();
        }
        else
        {
            AudioManager.instance.error();
        }
    }

    public void speedButton()
    {
        
        if (Zombucks - speedCost >= 0 && speedCap <= 4)
        {
            gameManager.instance.points -= speedCost;
            AudioManager.instance.purchaseSound("Purchase Sound");
            updateZombucks();
            playerController.IncreaseSpeed();
            speedCap++;
        }
        else 
        {
            AudioManager.instance.error();
        }
    }

    public void strengthButton()
    {
        if (Zombucks - strengthCost >= 0)
        {
            gameManager.instance.points -= strengthCost;
            AudioManager.instance.purchaseSound("Purchase Sound");
            updateZombucks();
            playerController.IncreaseStrength();
        }
        else
        {
            AudioManager.instance.error();
        }
    }
    public void rouletteButton()
    {
        if(Zombucks - rouletteCost >= 0)
        {
            gameManager.instance.points -= rouletteCost;
            AudioManager.instance.purchaseSound("Purchase Sound");
            updateZombucks();
            playerController.spinRoulette();
        }
        else
        {
            AudioManager.instance.error();
        }
    }
}
