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
    public void healthButton(int health)
    {
        if(zombuckAmount - healthCost >= 0) 
        {
            zombuckAmount -= healthCost;
            health *= 2;
            
        }
        
    }
    public void speedButton(int speed)
    {
        if(zombuckAmount - speedCost >= 0)
        {
            zombuckAmount -= speedCost;
                speed *= 2;
        }
        
    }
    public void strengthButton(int strength)
    {
        if(zombuckAmount - strengthCost >= 0)
        {
            zombuckAmount -= strengthCost;
                strength *= 2;
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
