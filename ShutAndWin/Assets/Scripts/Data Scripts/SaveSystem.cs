﻿using UnityEngine;
using System.Runtime.Serialization.Formatters.Binary;
using System.IO;

public static class SaveSystem
{
    public static void Save<T>(T saveData)
    {
        BinaryFormatter bf = new BinaryFormatter();
        FileStream file = new FileStream(Application.persistentDataPath + "/savegame.dat", FileMode.Create);
        bf.Serialize(file, saveData);
        file.Close();
    }

    public static T Load<T>(bool isLoadingFromFile=true, string data="")
    {
        if (isLoadingFromFile)
        {
            if (File.Exists(Application.persistentDataPath + "/savegame.dat"))
            {
                BinaryFormatter bf = new BinaryFormatter();
                FileStream file = new FileStream(Application.persistentDataPath + "/savegame.dat", FileMode.Open);
                T loaded = (T)bf.Deserialize(file);
                file.Close();
                return loaded;
            }
            else
            {
                Debug.LogError("Save file not found!");
            }
        }
        else
        {
            if (data != "")
            {
                return JsonUtility.FromJson<T>(data);
            }
        }
       

        return default(T);
    }

 
}
