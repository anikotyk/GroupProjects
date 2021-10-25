using UnityEngine;
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
        Debug.Log("Save Success!");
    }

    public static T Load<T>()
    {
        if (File.Exists(Application.persistentDataPath + "/savegame.dat"))
        {
            BinaryFormatter bf = new BinaryFormatter();
            FileStream file = new FileStream(Application.persistentDataPath + "/savegame.dat", FileMode.Open);

            Debug.Log("Load Success!");

            T loaded = (T)bf.Deserialize(file);
            file.Close();
            return loaded;
        }
        else
        {
            Debug.LogError("Save file not found!");
        }

        return default(T);
    }
}
