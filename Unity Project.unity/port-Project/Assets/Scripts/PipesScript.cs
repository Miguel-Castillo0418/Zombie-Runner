using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class PipesScript : MonoBehaviour
{
    float[] rotations = { 0, 90, 180, 270 };
    public float[] correctRotation;
    [SerializeField] bool isCorrect = false;
    [SerializeField] Computer computer;

    int corRotate = 1;

    private void Start()
    {
        corRotate = correctRotation.Length;
        int randRot = Random.Range(0, rotations.Length);
        transform.eulerAngles = new Vector3(0, 0, rotations[randRot]);
        
        if(corRotate > 1)
        {
            if (((int)transform.eulerAngles.z == correctRotation[0] || (int)transform.eulerAngles.z == correctRotation[1]))
            {
                isCorrect = true;
                computer.goodMove();
            }
        }
        else
        {
            if((int)transform.eulerAngles.z == correctRotation[0])
            {
                isCorrect = true;
                computer.goodMove();
            }
        }
    }
    public void rotatePipe()
    {
        transform.Rotate(new Vector3(0, 0, 90));
        computer.clickAud();

        if (corRotate > 1)
        {
            if (((int)transform.eulerAngles.z == correctRotation[0] || (int)transform.eulerAngles.z == correctRotation[1]) && isCorrect == false)
            {
                isCorrect = true;
                computer.goodMove();
            }
            else if (isCorrect == true)
            {
                isCorrect = false;
                computer.badMove();
            }
        }
        else
        {
            if ((int)transform.eulerAngles.z == correctRotation[0] && isCorrect == false)
            {
                isCorrect = true;
                computer.goodMove();
            }
            else if (isCorrect == true)
            {
                isCorrect = false;
                computer.badMove();
            }
        }
    }
}
