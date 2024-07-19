using System.Collections;
using System.Collections.Generic;
using System.IO;
using Unity.Burst.CompilerServices;
using UnityEngine;

public class Collectible : MonoBehaviour
{
    [SerializeField] public GameObject hint;
    [SerializeField] public GameObject itemImage;
    //[SerializeField] public GameObject saveSystemObj;
    [SerializeField] public gameManager gameManager;
    GameObject newHint = null;
    SaveSystem saveSystem = null;
    bool hasObject = false;
    
     // Start is called before the first frame update
    void Start()
    {
        saveSystem = SaveSystem.instance;

        //this will disable the object if it has already been collected
        if (hasObject)
        {
            Destroy(gameObject);
        }
        //this allows for update to be disabled by default so it is not called every frame
        enabled = false;
    }

    // Update is called once per frame
    void Update()
    {
        //this will rotate the hint to face the player
        if (newHint != null)
        {
            RotateToPlayer(newHint.transform);
        }
        //when the player picks up the item
        if (Input.GetKeyDown(KeyCode.E))
        {
            gameObject.SetActive(false);
            itemImage.SetActive(true);
            hasObject = true;
            enabled = false;
            saveSystem.saveCollectibles();
        }
    }
    private void OnTriggerEnter(Collider other)
    {
        //activates the hint so the player can see it
        if (newHint == null && other.CompareTag("Player"))
        {
            newHint = Instantiate(hint);
            newHint.transform.SetParent(transform);
            newHint.transform.localPosition = new Vector3(0.9f, 1.7f, 0.24f);
            //now update can be called
            enabled = true;
        }
    }
    private void OnTriggerExit(Collider other)
    {
        if (newHint != null)
        {
            Destroy(newHint);
            enabled = false;
        }
    }
    private void RotateToPlayer(Transform hintTransform)
    {
        Transform playerCamera = Camera.main.transform;
        Vector3 direction = playerCamera.position- hintTransform.position;
        Quaternion rotation = Quaternion.LookRotation(-direction);
        hintTransform.rotation = rotation;
    }
}
