using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class AntDataScreen : MonoBehaviour {

    private AntData currentAntData;
    private Renderer mr;

    private GameObject tDescription;
    private GameObject panelAntText;
    private GameObject buttonBalloon;

    private GameObject antModel;

    public Sprite bMessage;
    public Sprite bMessageClose;

    // Use this for initialization
    void Start () {

        antModel = GameObject.Find("AntModel");
        GameObject.Find("AntModel").GetComponent<Animator>().SetTrigger("goLoop");

        tDescription = GameObject.Find("tDescription");
        panelAntText = GameObject.Find("PanelAntText");
        panelAntText.SetActive(false);

        buttonBalloon = GameObject.Find("buttonBalloon");
        buttonBalloon.SetActive(false);

        mr = GameObject.Find("modelBody").GetComponent<SkinnedMeshRenderer>();

        currentAntData = DataManager.getInstance().getAntData( DataManager.getInstance().getCurrentShowedAntId() );
        mr.material.mainTexture = currentAntData.getTexture();

        string adjustedName = currentAntData.name.Insert(7, " \n");

        GameObject.Find("tName").GetComponent<Text>().text = adjustedName;

        tDescription.GetComponent<Text>().text = currentAntData.description;

        string rarityText = "";

        if (currentAntData.rarity == 1)
            rarityText = "Comum";
        else if (currentAntData.rarity == 2)
            rarityText = "Incomum";
        else if (currentAntData.rarity == 3)
            rarityText = "Rara";

        GameObject.Find("tRarity").GetComponent<Text>().text = rarityText;
    }
	
	// Update is called once per frame
	void Update () {
        if (Input.GetKeyUp(KeyCode.Escape))
            clickInReturn();
    }

    public void clickInSelfie(){
        SceneManager.LoadScene("PhotoScreenSelfie");
    }

    public void clickInTip(){
        if(panelAntText.activeSelf )
        {
            antModel.SetActive(true);
            GameObject.Find("AntModel").GetComponent<Animator>().SetTrigger("goLoop");
            panelAntText.SetActive(false);
            GameObject.Find("bTip").GetComponent<Image>().sprite = bMessage;
        }
        else
        {
            antModel.SetActive(false);
            panelAntText.SetActive(true);
            GameObject.Find("bTip").GetComponent<Image>().sprite = bMessageClose;
        }
    }

    public void clickInPhoto()
    {
        if (buttonBalloon.activeSelf)
            buttonBalloon.SetActive(false);
        else
            buttonBalloon.SetActive(true);
    }

    public void clickInFriend(){
        SceneManager.LoadScene("PhotoScreenFriend");
    }

    public void clickInReturn(){
        SceneManager.LoadScene("ChooseAntScreen");
    }
}
