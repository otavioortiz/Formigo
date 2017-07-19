using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class FindAntScreen : MonoBehaviour {

    public string pageToOpen = "https://www.google.com/maps/d/viewer?mid=1t9i5QgltOR3sFNmeonJyXiY0rDQ&ll=-31.761085899999955%2C-52.33830929999999&z=17";
    public GameObject antMapData;

    // Use this for initialization
    /*
    private Sprite tex;

    private void test()
    {
        print("---> " + tex.name);
    }
    */
    void Start () {

        AntData currentAntData;
        //tex = Resources.Load<Sprite>("ant1");
        //InvokeRepeating("test", 5, 0);

        List<int> currentAntIdList = new List<int>();
        bool currentAntCaptured; ;

        for(int j=0;j<DataManager.getInstance().getAntList().Count;j++)
        {
            currentAntCaptured = false;
            for (int l = 0; l < DataManager.getInstance().userData.getAntIdList().Count; l++)
            {
                if (DataManager.getInstance().userData.verifyAntCaptured(DataManager.getInstance().getAntList()[j].id))
                {
                    currentAntCaptured = true;
                    break;
                }
            }
                
            if (!currentAntCaptured)
                currentAntIdList.Add(DataManager.getInstance().getAntList()[j].id);
        }

        for (int i=0;i< currentAntIdList.Count; i++)
        {
            currentAntData = DataManager.getInstance().getAntData( currentAntIdList[i] );

            GameObject currentAntMapData = Instantiate(antMapData);
            currentAntMapData.transform.SetParent(GameObject.Find("Content").transform, false);

            currentAntMapData.transform.Translate(new Vector3(3,-i * 16,0));

            currentAntMapData.transform.FindChild("tAntName").gameObject.GetComponent<Text>().text = currentAntData.name;
            currentAntMapData.transform.FindChild("tAntText").gameObject.GetComponent<Text>().text = currentAntData.description;

            Sprite currentAntAvatar = Sprite.Create(currentAntData.avatar, new Rect(0,0,100,100), new Vector2(0,0));

            currentAntMapData.transform.FindChild("Panel").gameObject.transform.FindChild("tAntImage").gameObject.GetComponent<Image>().sprite = currentAntAvatar;
        }

        RectTransform rt = GameObject.Find("Content").GetComponent<RectTransform>();

        rt.SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, currentAntIdList.Count * 182);
    }
	
	// Update is called once per frame
	void Update () {
        if (Input.GetKeyUp(KeyCode.Escape))
            clickInReturn();
    }

    public void clickInCapture(){
        SceneManager.LoadScene("CaptureScreen");
    }

    public void clickInReturn(){
        SceneManager.LoadScene("GameMenu");
    }

    public void clickInMap()
    {
        InAppBrowser.DisplayOptions options = new InAppBrowser.DisplayOptions();
        options.displayURLAsPageTitle = false;
        options.pageTitle = "Mapa";
        options.backButtonText = "Voltar";
        InAppBrowser.OpenURL(pageToOpen, options);
    }

    public void OnClearCacheClicked()
    {
        InAppBrowser.ClearCache();
    }
}
