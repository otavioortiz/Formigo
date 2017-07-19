using Facebook.Unity;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class RankingMenu : MonoBehaviour {

    public GameObject rankingPart;
    public Sprite[] positionPartSprites;

    // Use this for initialization
    void Start()
    {
        Backend.getInstance().setLockScreen(true);
        Backend.getInstance().requestRanking(requestRankingComplete);
    }

    private void requestRankingComplete()
    {
        Backend.getInstance().requestUserRanking(requestUserRankingComplete, DataManager.getInstance().userData.id);
    }

    private void requestUserRankingComplete()
    {
        showUsersRanking();
        Backend.getInstance().setLockScreen(false);
    }

    private void showUsersRanking()
    {
        GameObject playerRanking = Instantiate(rankingPart);
        playerRanking.transform.SetParent(GameObject.Find("PlayerRanking").transform, false);
        playerRanking.transform.Translate(new Vector3(6, -8, 0));
        
        playerRanking.transform.FindChild("tName").gameObject.GetComponent<Text>().text = Backend.getInstance().jsonRankingUser.name;
        playerRanking.transform.FindChild("tScore").gameObject.GetComponent<Text>().text = Backend.getInstance().jsonRankingUser.score + " pontos";
        playerRanking.transform.FindChild("tPosition").gameObject.GetComponent<Text>().text = Backend.getInstance().jsonRankingUser.position + "º";

        Text currentPname = playerRanking.transform.FindChild("tName").gameObject.GetComponent<Text>();
        Text currentPScore = playerRanking.transform.FindChild("tScore").gameObject.GetComponent<Text>();
        Text currentPPosition = playerRanking.transform.FindChild("tPosition").gameObject.GetComponent<Text>();

        currentPname.color = new Color32(121, 21, 24, 255);
        currentPScore.color = new Color32(163, 48, 49, 255);
        currentPPosition.color = new Color32(121, 21, 24, 255);

        playerRanking.transform.FindChild("rankingDiv").gameObject.SetActive(false);
        

        if (Backend.getInstance().jsonRankingUser.facebookId != "" && Backend.getInstance().jsonRankingUser.facebookId != null)
        {
            FB.API("https" + "://graph.facebook.com/" + Backend.getInstance().jsonRankingUser.facebookId + "/picture?type=large", HttpMethod.GET, delegate (IGraphResult result)
            {
                if(result.Texture.width > 50)
                    playerRanking.transform.FindChild("avatarMask").FindChild("avatarArea").gameObject.GetComponent<Image>().sprite = Sprite.Create(result.Texture, new Rect(0, 0, 200, 200), new Vector2(0f, 0f), 100);
            });
        }

        for (int i = 0; i < Backend.getInstance().jsonRankingUserList.Count; i++)
        {
            GameObject currentUserRanking = Instantiate(rankingPart);
            currentUserRanking.transform.SetParent(GameObject.Find("Content").transform, false);

            currentUserRanking.transform.Translate(new Vector3(-20, (-10) - i * 15, 0));
            //currentUserRanking.transform.Translate(new Vector3(5, (120) - i * 18, 0));

            currentUserRanking.transform.FindChild("tName").gameObject.GetComponent<Text>().text = Backend.getInstance().jsonRankingUserList[i].name;
            currentUserRanking.transform.FindChild("tScore").gameObject.GetComponent<Text>().text = Backend.getInstance().jsonRankingUserList[i].score + " pontos";
            currentUserRanking.transform.FindChild("tPosition").gameObject.GetComponent<Text>().text = Backend.getInstance().jsonRankingUserList[i].position + "º";


            Image currentPAImage = currentUserRanking.transform.FindChild("positionArea").gameObject.GetComponent<Image>();
            Text currentTname = currentUserRanking.transform.FindChild("tName").gameObject.GetComponent<Text>();
            Text currentTScore = currentUserRanking.transform.FindChild("tScore").gameObject.GetComponent<Text>();

            switch (i)
            {
                case 0: currentPAImage.sprite = positionPartSprites[0]; currentTScore.color = new Color32(199,150,12,255); currentTname.color = new Color32(211, 160, 13, 255); break;
                case 1: currentPAImage.sprite = positionPartSprites[1]; currentTScore.color = new Color32(108, 108, 108,255); currentTname.color = new Color32(131, 124, 107, 255); break;
                case 2: currentPAImage.sprite = positionPartSprites[2]; currentTScore.color = new Color32(160, 118, 56,255); currentTname.color = new Color32(172, 123, 53, 255); break;
            }

            if (Backend.getInstance().jsonRankingUserList[i].facebookId != "" && Backend.getInstance().jsonRankingUserList[i].facebookId != null)
            {
                FB.API("https" + "://graph.facebook.com/" + Backend.getInstance().jsonRankingUserList[i].facebookId + "/picture?type=large", HttpMethod.GET, delegate (IGraphResult result)
                {
                    if(result.Texture.width > 50)
                        currentUserRanking.transform.FindChild("avatarMask").FindChild("avatarArea").gameObject.GetComponent<Image>().sprite = Sprite.Create(result.Texture, new Rect(0, 0, 200, 200), new Vector2(0f, 0f), 100);
                });
            }
        }

        RectTransform rt = GameObject.Find("Content").GetComponent<RectTransform>();
        rt.SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, Backend.getInstance().jsonRankingUserList.Count * 170);// Backend.getInstance().jsonRankingUserList.Count * 200);
        //rt.position.y = -1174;

        GameObject.Find("ScrollView").GetComponent<ScrollRect>().verticalNormalizedPosition = 50;
    }

    // Update is called once per frame
    void Update () {
        if (Input.GetKeyUp(KeyCode.Escape))
            clickInReturn();
    }

    public void clickInReturn()
    {
        SceneManager.LoadScene("GameMenu");
    }
}
