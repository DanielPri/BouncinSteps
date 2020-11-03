using UnityEngine;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using System;

public static class SaveSystem
{
    private static string path = Application.persistentDataPath + "/player.sav";
    public static void SavePlayer(int level)
    {
        
        BinaryFormatter formatter = new BinaryFormatter();
        using (FileStream stream = new FileStream(path, FileMode.Create))
        {
            PlayerData data = new PlayerData(level);
            formatter.Serialize(stream, data);
            stream.Close();
        }
    }

    public static PlayerData LoadPlayer()
    {
        if (File.Exists(path))
        {
            Debug.Log("file location: " + path);
            BinaryFormatter formatter = new BinaryFormatter();
            using (FileStream stream = new FileStream(path, FileMode.Open))
            {
                PlayerData data = (PlayerData)formatter.Deserialize(stream);

                stream.Close();
                return data;
            }
            
        } else
        {
            Debug.Log("Save file not found in: " + path);
            return null;
        }
    }
}
