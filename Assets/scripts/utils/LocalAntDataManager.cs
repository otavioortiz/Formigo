using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class LocalAntDataManager{

    private static LocalAntDataManager instance;

    public static LocalAntDataManager getInstance()
    {
        if (instance == null)
            instance = new LocalAntDataManager();

        return instance;
    }

    public void saveAntData(JsonAnt jsonAnt)
    {
        DirectoryInfo dir = new DirectoryInfo(Application.persistentDataPath + "/formigo/antData/");
        StreamWriter writer = new StreamWriter(dir.FullName + jsonAnt.id + ".json");
        writer.Write(JsonUtility.ToJson(jsonAnt));
        writer.Close();
    }

    public void saveAntTexture(int antId, JsonAntTexture jsonAntTexture)
    {
        DirectoryInfo dirTexture = new DirectoryInfo(Application.persistentDataPath + "/formigo/antTextures/");
        StreamWriter writerTexture = new StreamWriter(dirTexture.FullName + antId + ".json");
        writerTexture.Write(JsonUtility.ToJson(jsonAntTexture));
        writerTexture.Close();
    }

    public List<JsonAnt> getAntDataList()
    {
        List<JsonAnt> jsonAntList = new List<JsonAnt>();

        DirectoryInfo dir = new DirectoryInfo(Application.persistentDataPath + "/formigo/antData/");
        FileInfo[] antDataFiles = dir.GetFiles("*.json");

        foreach (FileInfo antDataFile in antDataFiles)
        {
            StreamReader reader = new StreamReader(antDataFile.FullName);
            string jsonAntData = reader.ReadToEnd();
            jsonAntList.Add(JsonUtility.FromJson<JsonAnt>(jsonAntData));
            reader.Close();
        }

        return jsonAntList;
    }

    public JsonAnt getAntData(int antId)
    {
        string path = Application.persistentDataPath + "/formigo/antData/" + antId + ".json";
        
        if( File.Exists(path) )
        {
            StreamReader reader = new StreamReader(path);
            string jsonAntData = reader.ReadToEnd();
            reader.Close();

            return JsonUtility.FromJson<JsonAnt>(jsonAntData);
        }

        return null;
    }

    public List<JsonAnt> getInnAppAntDataList()
    {
        List<JsonAnt> jsonAntList = new List<JsonAnt>();

        TextAsset[] jsonAntDataList = Resources.LoadAll<TextAsset>("antData/");
        for (int i = 0; i < jsonAntDataList.Length; i++)
        {
            jsonAntList.Add(JsonUtility.FromJson<JsonAnt>(jsonAntDataList[i].text));
            Resources.UnloadAsset(jsonAntDataList[i]);
        }
            
        return jsonAntList;
    }
    /*
    public List<JsonAntTexture> getInnAppAntTextureList()
    {
        List<JsonAntTexture> jsonAntTextureList = new List<JsonAntTexture>();

        TextAsset[] jsonAntDataList = Resources.LoadAll<TextAsset>("antTextures/");
        for (int i = 0; i < jsonAntDataList.Length; i++)
            jsonAntTextureList.Add(JsonUtility.FromJson<JsonAntTexture>(jsonAntDataList[i].text));

        Resources.UnloadUnusedAssets();
        return jsonAntTextureList;
    }
    */
    public JsonAntTexture getInnAppAntTexture(int antId)
    {
        TextAsset jsonAntTextureFile = Resources.Load<TextAsset>("antTextures/" + antId);
        JsonAntTexture jsonAntTexture = JsonUtility.FromJson<JsonAntTexture>(jsonAntTextureFile.text);
        Resources.UnloadAsset(jsonAntTextureFile);
        return jsonAntTexture;
    }
}
