using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using KeyCards;
public class PinPad : MonoBehaviour
{
    [SerializeField] TMP_Text code;
    [SerializeField] string answer;
    [SerializeField] private KeycardInventory _keyCards = null;
    [SerializeField] GameObject keyCard;
    public bool iscorrect = false;
    [SerializeField][Range(0, 1.0f)] float winVol; 

    private void Awake()
    {
        keyCard = GameObject.FindWithTag("keycardInv");
        _keyCards = keyCard.GetComponent<KeycardInventory>();
    }

    public void number(int num)
    {
        if (code.text != "Correct")
        {
            if (code.text == "Invalid")
            {
                code.text = "";
            }
            code.text += num.ToString();
            AudioManager.instance.Keypad();
        }
        else
        {
            return;
        }
    }
    public void enter()
    {
        if(code.text != "Correct")
        {
            if (code.text == answer)
            {
                code.text = "Correct";
                _keyCards.hasKeyCode = true;
                iscorrect = true;
            }
            else
            {
                code.text = "Invalid";
                AudioManager.instance.error();
            }
        }
        else
        {
            return;
        }
    }
    public void delete()
    {
        if (code.text != "Correct")
        {
            AudioManager.instance.Keypad();
            string result = code.text;
            code.text = result.Remove(result.Length - 1);
        }
        else
        {
            return; 
        }

    }
}
