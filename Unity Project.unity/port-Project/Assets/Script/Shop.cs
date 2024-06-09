using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class Shop : MonoBehaviour
{

    [SerializeField] TMP_Text zombucksText;
    [SerializeField] TMP_Text HealthCostText;
    [SerializeField] TMP_Text SpeedCostText;
    [SerializeField] TMP_Text StrengthCostText;
    [SerializeField] TMP_Text RouletteCostText;
    [SerializeField] int zombuckAmount;
    [SerializeField] int healthCost;
    [SerializeField] int speedCost;
    [SerializeField] int strengthCost;
    [SerializeField] int rouletteCost;
    [SerializeField] PlayerController playerController;
    int Zombucks;
    // Start is called before the first frame update
    void Start()
    {
        HealthCostText.text = healthCost.ToString();
        SpeedCostText.text = speedCost.ToString();
        StrengthCostText.text = strengthCost.ToString();
        RouletteCostText.text = rouletteCost.ToString();
        updateZombucks(zombuckAmount);
        Zombucks = zombuckAmount;
    }

    // Update is called once per frame
    void Update()
    {
        updateZombucks(zombuckAmount);
    }
    public void updateZombucks(int amount)
    {
        Zombucks = amount;
        zombucksText.text = Zombucks.ToString("F0");
    }
    public void healthButton()
    {
        if (zombuckAmount - healthCost >= 0)
        {
            zombuckAmount -= healthCost;
            updateZombucks(zombuckAmount);
            playerController.IncreaseHealth();
        }
    }

    public void speedButton()
    {
        if (zombuckAmount - speedCost >= 0)
        {
            zombuckAmount -= speedCost;
            updateZombucks(zombuckAmount);
            playerController.IncreaseSpeed();
        }
    }

    public void strengthButton()
    {
        if (zombuckAmount - strengthCost >= 0)
        {
            zombuckAmount -= strengthCost;
            updateZombucks(zombuckAmount);
            playerController.IncreaseStrength();
        }
    }
    public void rouletteButton()
    {
        if(zombuckAmount - rouletteCost >= 0)
        {
            zombuckAmount -= rouletteCost;
        }
    }
}
