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
    [SerializeField] PlayerController playerController;
    int Zombucks;
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
        if (Zombucks - healthCost >= 0)
        {
            gameManager.instance.points -= healthCost;
            updateZombucks();
            playerController.IncreaseHealth();
        }
    }

    public void speedButton()
    {
        if (Zombucks - speedCost >= 0)
        {
            gameManager.instance.points -= speedCost;
            updateZombucks();
            playerController.IncreaseSpeed();
        }
    }

    public void strengthButton()
    {
        if (Zombucks - strengthCost >= 0)
        {
            gameManager.instance.points -= strengthCost;
            updateZombucks();
            playerController.IncreaseStrength();
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
