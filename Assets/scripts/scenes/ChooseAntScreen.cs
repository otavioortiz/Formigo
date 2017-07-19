using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class ChooseAntScreen : MonoBehaviour
{

    public GameObject antAvatarImage;
    public Sprite noAntAvatar;

    private int numberOfTotalAnt1 = 0;
    private int numberOfTotalAnt2 = 0;
    private int numberOfTotalAnt3 = 0;

    private int numberOfCapturedAnt1 = 0;
    private int numberOfCapturedAnt2 = 0;
    private int numberOfCapturedAnt3 = 0;

    private Text tRarity1;
    private Text tRarity2;
    private Text tRarity3;

    private Text tNumbers1;
    private Text tNumbers2;
    private Text tNumbers3;

    // Use this for initialization
    void Start () {
        tRarity1 = GameObject.Find("bRarity1").transform.FindChild("tRarity").gameObject.GetComponent<Text>();
        tRarity2 = GameObject.Find("bRarity2").transform.FindChild("tRarity").gameObject.GetComponent<Text>();
        tRarity3 = GameObject.Find("bRarity3").transform.FindChild("tRarity").gameObject.GetComponent<Text>();

        tNumbers1 = GameObject.Find("bRarity1").transform.FindChild("tNumbers").gameObject.GetComponent<Text>();
        tNumbers2 = GameObject.Find("bRarity2").transform.FindChild("tNumbers").gameObject.GetComponent<Text>();
        tNumbers3 = GameObject.Find("bRarity3").transform.FindChild("tNumbers").gameObject.GetComponent<Text>();

        for (int i = 0; i < DataManager.getInstance().getAntList().Count; i++)
        {
            switch(DataManager.getInstance().getAntList()[i].rarity)
            {
                case 1: numberOfTotalAnt1++; break;
                case 2: numberOfTotalAnt2++; break;
                case 3: numberOfTotalAnt3++; break;
            }
        }

        for (int j = 0; j < DataManager.getInstance().userData.getAntIdList().Count; j++)
        {
            switch (DataManager.getInstance().getAntData(DataManager.getInstance().userData.getAntIdList()[j]).rarity)
            {
                case 1: numberOfCapturedAnt1++; break;
                case 2: numberOfCapturedAnt2++; break;
                case 3: numberOfCapturedAnt3++; break;
            }
        }

        tNumbers1.text = numberOfCapturedAnt1 + "/" + numberOfTotalAnt1;
        tNumbers2.text = numberOfCapturedAnt2 + "/" + numberOfTotalAnt2;
        tNumbers3.text = numberOfCapturedAnt3 + "/" + numberOfTotalAnt3;

        Text scoreText = GameObject.Find("tScore").GetComponent<Text>();
        scoreText.text = DataManager.getInstance().userData.score + "";

        clickInRarity1();
    }

    private void createAntButtons(int currentRarity)
    {
        GameObject[] antButtons = GameObject.FindGameObjectsWithTag("AntButton");
        foreach (GameObject antButton in antButtons)
            Destroy(antButton);

        int currentX = 0;
        int currentY = 0;

        List<int> currentAntIdList = new List<int>();
        //DataManager.getInstance().userData.getAntIdList().Count
        for (int j=0;j< DataManager.getInstance().getAntList().Count; j++)
        {
            if (DataManager.getInstance().getAntList()[j].rarity == currentRarity)
                currentAntIdList.Add(DataManager.getInstance().getAntList()[j].id);
        }
        
        for (int i = 0; i < currentAntIdList.Count; i++)
        {
            AntData antData = DataManager.getInstance().getAntData( currentAntIdList[i] );

            if (currentX > 50)
            {
                currentY += 1;
                currentX = 0;
            }

            GameObject currentAntAvatarImage = Instantiate(antAvatarImage);
            //currentAntAvatarImage.name = "ant_" + i;
            currentAntAvatarImage.transform.SetParent(GameObject.Find("Content").transform, false);
            currentAntAvatarImage.transform.Translate(new Vector3(3+currentX,-3-currentY * 14, 0));

            currentX += 14;

            Sprite currentAntAvatar;

            if (DataManager.getInstance().userData.verifyAntCaptured(antData.id))
            {
                currentAntAvatar = Sprite.Create(antData.avatar, new Rect(0, 0, 100, 100), new Vector2(0, 0));
                currentAntAvatarImage.transform.FindChild("Panel").gameObject.transform.FindChild("AvatarButton").gameObject.GetComponent<Button>().onClick.AddListener(() => antClick(antData.id));
            } 
            else
                currentAntAvatar = noAntAvatar;

            currentAntAvatarImage.transform.FindChild("Panel").gameObject.transform.FindChild("AvatarButton").gameObject.GetComponent<Button>().image.sprite = currentAntAvatar;
        }

        RectTransform rt = GameObject.Find("Content").GetComponent<RectTransform>();
        rt.SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, currentY * 190);
    }

    // Update is called once per frame
    void Update () {
        if (Input.GetKeyUp(KeyCode.Escape))
            clickInReturn();
    }

    void antClick(int antId)
    {
        print("-> " + antId);
        DataManager.getInstance().setCurrentShowedAntId(antId);
        SceneManager.LoadScene("AntDataScreen");
    }

    public void clickInReturn()
    {
        SceneManager.LoadScene("GameMenu");
    }

    public void clickInRarity1()
    {
        print("clickInRarity1");
        adjustColors(1);
        createAntButtons(1);
    }

    public void clickInRarity2()
    {
        print("clickInRarity2");
        adjustColors(2);
        createAntButtons(2);
    }

    public void clickInRarity3()
    {
        print("clickInRarity3");
        adjustColors(3);
        createAntButtons(3);
    }

    private void adjustColors(int rarity)
    {
        Color32 deactivatedColor = new Color32(122, 137, 136, 255);
        Color32 activatedRarityColor = new Color32(43, 66, 97, 255);
        Color32 activatedNumbersColor = new Color32(121, 21, 94, 255);

        tRarity1.color = deactivatedColor;
        tRarity2.color = deactivatedColor;
        tRarity3.color = deactivatedColor;

        tNumbers1.color = deactivatedColor;
        tNumbers2.color = deactivatedColor;
        tNumbers3.color = deactivatedColor;

        switch(rarity)
        {
            case 1: tRarity1.color = activatedRarityColor; tNumbers1.color = activatedNumbersColor; break;
            case 2: tRarity2.color = activatedRarityColor; tNumbers2.color = activatedNumbersColor; break;
            case 3: tRarity3.color = activatedRarityColor; tNumbers3.color = activatedNumbersColor; break;
        }
    }

    public void clickInConfig()
    {
        SceneManager.LoadScene("ConfigMenu");
    }
}
