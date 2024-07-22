using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PickUpCure : MonoBehaviour
{
    [SerializeField] public GameObject Cure;
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.E))
        {
            gameObject.SetActive(false);
            enabled = false;
        }
    }
}
