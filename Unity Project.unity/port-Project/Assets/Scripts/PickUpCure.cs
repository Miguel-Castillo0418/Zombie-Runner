using System.Collections;
using System.Collections.Generic;
using UnityEditor.Rendering;
using UnityEngine;

public class PickUpCure : MonoBehaviour
{
    [SerializeField] public GameObject Cure;
    [SerializeField] public GameObject hint;
    GameObject newHint = null;
    private void Start()
    {
        enabled = false;
    }

    void Update()
    {
        //when the player picks up the item
        if (Input.GetKeyDown(KeyCode.E))
        {
            gameObject.SetActive(false);
            enabled = false;
            AudioManager.instance.playCollectibleGatheredSound();
            Destroy(newHint);
        }
    }
    void OnTriggerEnter(Collider other)
    {
        if (newHint == null && other.CompareTag("Player"))
        {
            newHint = Instantiate(hint);
                enabled = true;
            
        }
    }
}
