//using UnityEngine;
//using System.IO;
//using System.Runtime.Serialization.Formatters.Binary;


//public static class JsonSaveSystem 
//{
//    public static void SavePlayer(PlayerController player)
//    {
//        BinaryFormatter formatter = new BinaryFormatter();
//        string path = Application.persistentDataPath + "/player.bin";
//        using (FileStream stream = new FileStream(path, FileMode.Create))
//        {
//            PlayerData data = PlayerData.CreateFromInstance();
//            formatter.Serialize(stream, data);
//            Debug.Log("Player data saved successfully.");
//            Debug.Log($"HP: {data.playerHp}, Speed: {data.playerSpeed}, Position: ({data.playerPos[0]}, {data.playerPos[1]}, {data.playerPos[2]}), Rotation: ({data.playerRot[0]}, {data.playerRot[1]}, {data.playerRot[2]}), Money: {data.money}, Gun Model Tag: {data.gunModel?.tag}");
//        }
//        }
//    public static PlayerData LoadPlayer()
//    {
//        string path = Application.persistentDataPath + "/player.bin";
//        if (File.Exists(path))
//        {
//            try
//            {
//                BinaryFormatter formatter = new BinaryFormatter();
//                using (FileStream stream = new FileStream(path, FileMode.Open))
//                {
//                    if (stream.Length > 0) // Check if the stream has data
//                    {
//                        PlayerData data = formatter.Deserialize(stream) as PlayerData;
//                        return data;
//                    }
//                    else
//                    {
//                        Debug.LogWarning("Save file is empty. Returning default PlayerData.");
//                        return GetDefaultPlayerData();
//                    }
//                }
//            }
//            catch (System.Exception ex)
//            {
//                Debug.LogWarning("Failed to load player data: " + ex.Message);
//                return GetDefaultPlayerData();
//            }
//        }
//        else
//        {
//            Debug.LogWarning("Save File Not Found In " + path);
//            return GetDefaultPlayerData();
//        }
//    }
//    private static PlayerData GetDefaultPlayerData()
//    {
//        // Create and return a default PlayerData object
//        PlayerData defaultData = new PlayerData();
//        // Initialize defaultData with default values
//        return defaultData;
//    }
//}
