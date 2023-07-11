using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;

public static class SaveManager
{
    public static void Save(object data, string fileName)
    {
        string m_jsonString = JsonUtility.ToJson(data, true);
        File.WriteAllText(Application.persistentDataPath + "/"+fileName+".json", m_jsonString);
    }

    public static Data Load<Data>(string fileName) where Data : new()
    {
        Data data = new Data();

        if (GetIfFileExists(fileName))
        {
            //string raw = File.ReadAllText(Application.persistentDataPath + "/save.json");
            string raw = File.ReadAllText(Application.persistentDataPath + "/"+fileName+".json");
            JsonUtility.FromJsonOverwrite(raw, data);
        }

        return data;
    }

    public static bool GetIfFileExists(string fileName)
    {
        //if (File.Exists(Application.persistentDataPath + "/save.json")) return true;
        if (File.Exists(Application.persistentDataPath + "/"+fileName+".json")) return true;
        return false;
    }
}
