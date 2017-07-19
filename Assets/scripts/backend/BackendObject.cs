using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography;
using System.Text;
using UnityEngine;
using UnityEngine.Networking;

public class BackendObject : MonoBehaviour {

    // Use this for initialization

    private GameObject loadDataImage;
    private UnityWebRequest www;

    private string sessionKey;

    private void Awake()
    {
        loadDataImage = transform.Find("loadDataImage").gameObject;
        loadDataImage.SetActive(false);

        DontDestroyOnLoad(transform.gameObject);
        Backend.getInstance().setBackendObject(this);
    }

    public void ReceivedMemoryWarning(string nothing)
    {
        Resources.UnloadUnusedAssets();
    }

    public void setLockScreen(bool locked)
    {
        if(locked)
            loadDataImage.SetActive(true);
        else
            loadDataImage.SetActive(false);
    }

    public IEnumerator requestLogin(string email, string pass)
    {
        WWWForm form = new WWWForm();
        form.AddField("email", email);
        form.AddField("pass", pass);

        www = UnityWebRequest.Post("http://formigo.outdabox.in/public/api/user/login", form);
        yield return www.Send();

        if (www.downloadHandler.text.Contains("false"))
        {
            Backend.getInstance().requestLoginComplete(JsonUtility.FromJson<JsonError>(www.downloadHandler.text));
        }
        else
        {
            sessionKey = www.GetResponseHeader("SET-COOKIE");
            Backend.getInstance().requestLoginComplete(JsonUtility.FromJson<JsonLogin>(www.downloadHandler.text));
        }
    }

    public IEnumerator requestAntDataList()
    {
        www = UnityWebRequest.Get("http://formigo.outdabox.in/public/api/ants/data");
        www.SetRequestHeader("Cookie", sessionKey);
        yield return www.Send();
        Backend.getInstance().requestAntDataListComplete(JsonUtility.FromJson<JsonAntDataList>(www.downloadHandler.text));
    }

    public IEnumerator requestRanking()
    {
        www = UnityWebRequest.Get("http://formigo.outdabox.in/public/api/ranking");
        www.SetRequestHeader("Cookie", sessionKey);
        yield return www.Send();

        string editedJsonText = "{\"ranking\":" + www.downloadHandler.text + "}";
        Backend.getInstance().requestRankingComplete(JsonUtility.FromJson<JsonRanking>(editedJsonText));
    }

    public IEnumerator requestUserRanking(int userId)
    {
        www = UnityWebRequest.Get("http://formigo.outdabox.in/public/api/user/" + userId);
        www.SetRequestHeader("Cookie", sessionKey);
        yield return www.Send();
        Backend.getInstance().requestUserRankingComplete(JsonUtility.FromJson<JsonRankingUser>(www.downloadHandler.text));
    }

    public IEnumerator requestCreateUser(string name, string pass, string email, string facebookId)
    {
        WWWForm form = new WWWForm();
        
        form.AddField("name", name);
        form.AddField("password", pass);
        form.AddField("email", email);
        form.AddField("facebookId", facebookId);

        www = UnityWebRequest.Post("http://formigo.outdabox.in/public/api/user/create", form);
        yield return www.Send();

        if (www.downloadHandler.text.Contains("false"))
        {
            Backend.getInstance().requestCreateUserComplete(JsonUtility.FromJson<JsonError>(www.downloadHandler.text));
        }
        else
        {
            Backend.getInstance().requestCreateUserComplete(JsonUtility.FromJson<JsonLogin>(www.downloadHandler.text));
        }
    }

    public IEnumerator requestCaptureAnt(int userId, int antId)
    {
        WWWForm form = new WWWForm();

        form.AddField("user_id", userId);
        form.AddField("ant_id", antId);

        string hash = userId + "|OTB|" + antId;
        form.AddField("hash", MD5Hash(hash));

        www = UnityWebRequest.Post("http://formigo.outdabox.in/public/api/ant/capture", form);
        www.SetRequestHeader("Cookie", sessionKey);
        yield return www.Send();

        Backend.getInstance().requestCaptureAntComplete();
    }

    public IEnumerator requestAddScore(int userId, int score)
    {
        WWWForm form = new WWWForm();

        form.AddField("user_id", userId);
        form.AddField("score", score);

        string hash = userId + "|OTB|" + score;
        form.AddField("hash", MD5Hash(hash));

        www = UnityWebRequest.Post("http://formigo.outdabox.in/public/api/ranking", form);
        www.SetRequestHeader("Cookie", sessionKey);

        yield return www.Send();

        Backend.getInstance().requestAddScoreComplete();
    }

    public IEnumerator requestCurrentTime()
    {
        www = UnityWebRequest.Get("http://formigo.outdabox.in/public/api/current-time");
        www.SetRequestHeader("Cookie", sessionKey);
        yield return www.Send();
        Backend.getInstance().requestCurrentTimeComplete(JsonUtility.FromJson<JsonTime>(www.downloadHandler.text));
    }

    public IEnumerator requestAntTexture(int antId)
    {
        www = UnityWebRequest.Get("http://formigo.outdabox.in/public/api/ant/" + antId + "/texture");
        www.SetRequestHeader("Cookie", sessionKey);
        yield return www.Send();
        Backend.getInstance().requestAntTexturesComplete(antId, JsonUtility.FromJson<JsonAntTexture>(www.downloadHandler.text));
    }

    public static string MD5Hash(string text)
    {
        MD5 md5 = new MD5CryptoServiceProvider();

        //compute hash from the bytes of text
        md5.ComputeHash(ASCIIEncoding.ASCII.GetBytes(text));

        //get hash result after compute it
        byte[] result = md5.Hash;

        StringBuilder strBuilder = new StringBuilder();
        for (int i = 0; i < result.Length; i++)
        {
            //change it into 2 hexadecimal digits
            //for each byte
            strBuilder.Append(result[i].ToString("x2"));
        }

        //Debug.Log("RASH: " + strBuilder.ToString() );
        return strBuilder.ToString();
    }

}
