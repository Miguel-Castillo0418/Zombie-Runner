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
    [SerializeField] int healthCost;
    [SerializeField] int speedCost;
    [SerializeField] int strengthCost;
    [SerializeField] int rouletteCost;
    int Zombucks;
    int health =2;
    int speed = 3;
    int strength = 4;
    // Start is called before the first frame update
    void Start()
    {
        HealthCostText.text = healthCost.ToString();
        SpeedCostText.text = speedCost.ToString();
        StrengthCostText.text = strengthCost.ToString();
        RouletteCostText.text = rouletteCost.ToString();
        Zombucks = gameManager.instance.points;
        updateZombucks();
    }

    // Update is called once per frame
    void Update()
    {
        Zombucks = gameManager.instance.points;
        updateZombucks();
    }
    public void updateZombucks()
    {
        zombucksText.text = Zombucks.ToString("F0");
    }
    public void healthButton()
    {
        if(Zombucks - healthCost >= 0) 
        {
            Zombucks -= healthCost;
            health *= 2;
            
        }
        
    }
    public void speedButton()
    {
        if(Zombucks - speedCost >= 0)
        {
            Zombucks -= speedCost;
                speed *= 2;
        }
        
    }
    public void strengthButton()
    {
        if(Zombucks - strengthCost >= 0)
        {
            Zombucks -= strengthCost;
                strength *= 2;
        }
        
    }
    public void rouletteButton()
    {
        if(Zombucks - rouletteCost >= 0)
        {
            Zombucks -= rouletteCost;
        }
    }
}
