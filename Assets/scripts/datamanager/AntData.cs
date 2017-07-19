using System;
using System.Collections;
using UnityEngine;

public class AntData{

    public int id = 1;
    public int arGraphicId = 0;
    public string startTime;
    public string endTime;
    public string name = "Formiguita";
    public string description = "Lorem ipsum dolor sit amet, consectetur adipiscing elit, sed do eiusmod tempor incididunt ut";
    public int rarity = 3;
    public string type;
    private string textureData;

    public Vector2 mapPosition;

    public Texture2D avatar;
    public Texture2D texture;
    private byte[] b64_bytes;

    public AntData(int id, string name, int rarity)
    {
        this.id = id;
        this.name = name;
        this.rarity = rarity;

        //convertBase64InTexture();
    }

    public AntData(JsonAnt jsonAnt)
    {
        this.id = jsonAnt.id;
        this.arGraphicId = jsonAnt.ARGraphicId;
        this.startTime = jsonAnt.startTime;
        this.endTime = jsonAnt.endTime;
        this.name = jsonAnt.name;
        this.description = jsonAnt.description;
        this.rarity = jsonAnt.rarity;
        this.type = jsonAnt.type;
        //this.textureData = jsonAnt.textureData;
        //convertBase64InTexture();
    }

    public Texture2D getTexture(){
        //texture = new Texture2D(1, 1);
        //texture.LoadImage(b64_bytes);
        return AntTextureLoader.getInstance().getAntTexture(this.id);
    }
    /*
    private void convertBase64InTexture()
    {
        b64_bytes = Convert.FromBase64String(textureData.Split(',')[1]);
    } 
    */

}
