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
    // Start is called before the first frame update
    void Start()
    {
        HealthCostText.text = healthCost.ToString();
        SpeedCostText.text = speedCost.ToString();
        StrengthCostText.text = strengthCost.ToString();
        RouletteCostText.text = rouletteCost.ToString();
        Zombucks = gameManager.instance.points;
        healthbutton.enabled = false;
        updateZombucks();
    }

    // Update is called once per frame
    void Update()
    {
        Zombucks = gameManager.instance.points;
        if (playerController.shopHP < playerController.HPorig)
        {
            healthbutton.enabled = true;
        }
        updateZombucks();
    }
    public void updateZombucks()
    {
        zombucksText.text = Zombucks.ToString("F0");
    }
    public void healthButton()
    {
        if (playerController.HPorig == playerController.shopHP)
        {
            healthbutton.enabled = false;
        }
        if (Zombucks - healthCost >= 0)
        {
            
            gameManager.instance.points -= healthCost;
            AudioManager.instance.purchaseSound("Purchase Sound");
            updateZombucks();
            playerController.IncreaseHealth();
        }
    }

    public void speedButton()
    {
        
        if (Zombucks - speedCost >= 0)
        {
            gameManager.instance.points -= speedCost;
            AudioManager.instance.purchaseSound("Purchase Sound");
            updateZombucks();
            playerController.IncreaseSpeed();
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
    }
}
