using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class Shop : MonoBehaviour
{

    [SerializeField] TMP_Text zombucksText;
    [SerializeField] int zombuckAmount;
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
        zombuckAmount -= healthCost;
        health *= 2;
    }
    public void speedButton()
    {
        zombuckAmount -= speedCost;
        speed *= 2;
    }
    public void strengthButton()
    {
        zombuckAmount -= strengthCost;
        strength *= 2;
    }
    public void rouletteButton()
    {
        zombuckAmount -= rouletteCost;
    }
}
