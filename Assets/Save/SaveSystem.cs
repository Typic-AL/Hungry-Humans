using UnityEngine;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;

public static class SaveSystem
{
    public static void Save(gm gm)
    {
        BinaryFormatter formatter = new BinaryFormatter();
        string path = Application.persistentDataPath + "/gameData.hungryhungryhuman";
        FileStream stream = new FileStream(path, FileMode.Create);

        SaveData data = new SaveData(gm);

        formatter.Serialize(stream, data);
        stream.Close();
        Debug.Log("Saved");
    }

    public static SaveData Load()
    {
        string path = Application.persistentDataPath + "/gameData.hungryhungryhuman";
        if(File.Exists(path))
        {
            BinaryFormatter formatter = new BinaryFormatter();
            FileStream stream = new FileStream(path, FileMode.Open);

            SaveData data = formatter.Deserialize(stream) as SaveData;
            stream.Close();

            Debug.Log("Loaded");

            gm.i.saveFileFound = true;

            return data;
        }
        else
        {
            Debug.Log("save file not found in" + path);
            gm.i.saveFileFound = false;
            return null;
        }
    }
}
