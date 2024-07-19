using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class PipesScript : MonoBehaviour
{
    float[] rotations = { 0, 90, 180, 270 };
    public float[] correctRotation;
    [SerializeField] bool isCorrect = false;

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
                gameManager.instance.goodMove();
            }
        }
        else
        {
            if((int)transform.eulerAngles.z == correctRotation[0])
            {
                isCorrect = true;
                gameManager.instance.goodMove();
            }
        }
    }
    public void rotatePipe()
    {
        transform.Rotate(new Vector3(0, 0, 90));
        gameManager.instance.clickAud();

        if (corRotate > 1)
        {
            if (((int)transform.eulerAngles.z == correctRotation[0] || (int)transform.eulerAngles.z == correctRotation[1]) && isCorrect == false)
            {
                isCorrect = true;
                gameManager.instance.goodMove();
            }
            else if (isCorrect == true)
            {
                isCorrect = false;
                gameManager.instance.badMove();
            }
        }
        else
        {
            if ((int)transform.eulerAngles.z == correctRotation[0] && isCorrect == false)
            {
                isCorrect = true;
                gameManager.instance.goodMove();
            }
            else if (isCorrect == true)
            {
                isCorrect = false;
                gameManager.instance.badMove();
            }
        }
    }
}
