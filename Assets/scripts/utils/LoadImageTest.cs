using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LoadImageTest : MonoBehaviour {

    public Texture2D tex;
    private string base64Image;

	// Use this for initialization
	void Start () {

        byte[] bytes = tex.EncodeToPNG();

        base64Image = System.Convert.ToBase64String(bytes);

    }

    // Update is called once per frame
    void Update () {
		
	}

    public string getTextureData()
    {

        return base64Image;
    }
}
