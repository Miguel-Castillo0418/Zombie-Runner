using System.Collections;
using System.Collections.Generic;
using System.IO;
using Unity.Burst.CompilerServices;
using UnityEngine;

public class Collectible : MonoBehaviour
{
    [SerializeField] public GameObject hint;
    GameObject newHint = null;
    CollectibleManager collectibleManager = null;
    bool hasObject = false;
    Transform item=null;

     // Start is called before the first frame update
    void Start()
    {
        collectibleManager = CollectibleManager.instance;
        GameObject collectibles = GameObject.Find("Collectibles");
        item = collectibles.transform.Find(name);

        //saveSystem = SaveSystem.instance;

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
            item.gameObject.SetActive(true);
            hasObject = true;
            enabled = false;
            //JsonSaveSystem.SavePlayer(PlayerController.instance);
            collectibleManager.SaveData();
            AudioManager.instance.playCollectibleGatheredSound();
            Destroy(newHint);
        }
    }
    private void OnTriggerEnter(Collider other)
    {
        //activates the hint so the player can see it
        if (newHint == null && other.CompareTag("Player"))
        {
            newHint = Instantiate(hint);
            //newHint.transform.SetParent(transform);
           // newHint.transform.localPosition = new Vector3(0f, 1f, 0f);
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
