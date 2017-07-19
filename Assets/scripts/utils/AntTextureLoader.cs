
using System;
using System.IO;
using UnityEngine;

public class AntTextureLoader{

    private static AntTextureLoader instance;

    private DirectoryInfo dir;
    private FileInfo antDataFile;

    private StreamReader reader;
    private string jsonTextureData;

    public AntTextureLoader()
    {
        dir = new DirectoryInfo(Application.persistentDataPath + "/formigo/antTextures/");
    }

    public static AntTextureLoader getInstance()
    {
        if (instance == null)
            instance = new AntTextureLoader();

        return instance;
    }

    public Texture2D getAntTexture(int antId)
    {
        Texture2D texture = new Texture2D(1, 1);
        string textureData = getJsonAntTexture(antId).textureData;
        byte[] b64Bytes = Convert.FromBase64String(textureData.Split(',')[1]);
        texture.LoadImage(b64Bytes);
        texture.Compress(false);

        reader = null;
        jsonTextureData = null;
        Resources.UnloadUnusedAssets();

        return texture;
    }

    private JsonAntTexture getJsonAntTexture(int antId)
    {
        reader = new StreamReader(dir.FullName + antId + ".json");
        jsonTextureData = reader.ReadToEnd();
        reader.Close();

        return JsonUtility.FromJson<JsonAntTexture>(jsonTextureData);
    }
}
