using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[System.Serializable]
public class PlayerData
{
    public float playerHp;
    public int playerSpeed;
    public float[] playerPos = new float[3];
    public float[] playerRot = new float[3];
    public int money;
    public SerializableCollectible[] collectibles=new SerializableCollectible[4];
    public SerializableGunModel gunModel;
    public PlayerData(PlayerController data ) 
    {
        playerHp = data.HP;
        playerSpeed = data.speed;
        playerPos[0]=data.transform.position.x;
        playerPos[1]=data.transform.position.y;
        playerPos[2]=data.transform.position.z;
        playerRot[0]=data.transform.rotation.x;
        playerRot[1]=data.transform.rotation.y;
        playerRot[2]=data.transform.rotation.z;
        money=data.money;
        CollectibleManager collectibleManager = CollectibleManager.instance;
        collectibles = new SerializableCollectible[collectibleManager.collectibleArr.Length];
        for (int i = 0; i < collectibleManager.collectibleArr.Length; i++)
        {
            collectibles[i] = new SerializableCollectible(collectibleManager.collectibleArr[i]);
        }
        gunModel = data.gunModel != null ? new SerializableGunModel(data.gunModel) : null;
    }
    public PlayerData()
    {
        playerHp = 100f; 
        playerSpeed = 8; 
        playerPos = new float[] { -1.5f, 3f, 71.98f }; 
        playerRot = new float[] { 0f, 180f, 0f };
        money = 0;
        collectibles = new SerializableCollectible[4];
        gunModel = null;
    }
    public static PlayerData CreateFromInstance()
    {
        if (PlayerController.instance != null)
        {
            return new PlayerData(PlayerController.instance);
        }
        else
        {
            Debug.LogError("PlayerController instance not found.");
            return new PlayerData(); // Return default if instance is not found
        }
    }
}
[System.Serializable]
public class SerializableCollectible
{
    public string name;
    public float[] position = new float[3];
    public float[] rotation = new float[3];
    public bool isActive;

    public SerializableCollectible(GameObject collectible)
    {
        name = collectible.name;
        position[0] = collectible.transform.position.x;
        position[1] = collectible.transform.position.y;
        position[2] = collectible.transform.position.z;
        rotation[0] = collectible.transform.rotation.eulerAngles.x;
        rotation[1] = collectible.transform.rotation.eulerAngles.y;
        rotation[2] = collectible.transform.rotation.eulerAngles.z;
        isActive = collectible.activeSelf;
    }
}
[System.Serializable]
public class SerializableGunModel
{
    public string tag;
    public float[] position = new float[3];
    public float[] rotation = new float[3];

    public SerializableGunModel(GameObject gunModel)
    {
        tag = gunModel.tag;
        position[0] = gunModel.transform.position.x;
        position[1] = gunModel.transform.position.y;
        position[2] = gunModel.transform.position.z;
        rotation[0] = gunModel.transform.rotation.eulerAngles.x;
        rotation[1] = gunModel.transform.rotation.eulerAngles.y;
        rotation[2] = gunModel.transform.rotation.eulerAngles.z;
    }
}