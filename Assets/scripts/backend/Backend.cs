using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class Backend{

    public static bool updateAntDataActived = true;

    private BackendObject backandObject;
    private static Backend instance;

    private Action currentRequestComplete;

    public JsonError jsonError;
    public JsonPlayer jsonPlayer;
    public JsonAntDataList jsonAntDataList;

    public List<JsonRankingUser> jsonRankingUserList;
    public JsonRankingUser jsonRankingUser;
    public JsonTime jsonTime;

    public List<int> loadTextureAntIdList;

    public DateTime lastUpdated;
    public bool facebookPublishEnabled;

    /*
     * Não é permitido fazer mais de um request simultaneamente, só se pode fazer um novo resquest quando o anterior terminar.
     */

    public Backend()
    {
        
    }
        
    public static Backend getInstance(){
        if (instance == null)
            instance = new Backend();

        return instance;
    }

    public void setLockScreen(bool locked)
    {
        this.backandObject.setLockScreen(locked);
    }

    public void setBackendObject(BackendObject backandObject)
    {
        this.backandObject = backandObject;
    }

    public void requestCreateUser(Action requestComplete, string name, string pass, string email, string facebookId)
    {
        this.jsonError = null;
        backandObject.StartCoroutine(backandObject.requestCreateUser(name, pass, email, facebookId));
        currentRequestComplete = requestComplete;
    }

    public void requestCreateUserComplete(JsonLogin jsonLogin)
    {
        this.jsonPlayer = jsonLogin.user;
        currentRequestComplete();
    }

    public void requestCreateUserComplete(JsonError jsonError)
    {
        this.jsonError = jsonError;
        currentRequestComplete();
    }

    public void requestLogin(Action requestComplete, string email, string pass)
    {
        this.jsonError = null;
        backandObject.StartCoroutine( backandObject.requestLogin(email, pass) );
        currentRequestComplete = requestComplete;
    }

    public void requestLoginComplete(JsonLogin jsonLogin)
    {
        jsonPlayer = jsonLogin.user;
        currentRequestComplete();
    }

    public void requestLoginComplete(JsonError jsonError)
    {
        this.jsonError = jsonError;
        currentRequestComplete();
    }

    public void requestAntDataList(Action requestComplete)
    {
        backandObject.StartCoroutine("requestAntDataList");
        currentRequestComplete = requestComplete;
    }

    public void requestAntDataListComplete(JsonAntDataList jsonAntDataList)
    {
        this.jsonAntDataList = jsonAntDataList;
        currentRequestComplete();
    }

    public void requestRanking(Action requestComplete)
    {
        backandObject.StartCoroutine("requestRanking");
        currentRequestComplete = requestComplete;
    }

    public void requestRankingComplete(JsonRanking jsonRanking)
    {
        jsonRankingUserList = new List<JsonRankingUser>();

        for (int i = 0; i < jsonRanking.ranking.Length; i++)
            jsonRankingUserList.Add(jsonRanking.ranking[i]);

        currentRequestComplete();
    }

    public void requestUserRanking(Action requestComplete, int userId)
    {
        backandObject.StartCoroutine(backandObject.requestUserRanking(userId));
        currentRequestComplete = requestComplete;
    }

    public void requestUserRankingComplete(JsonRankingUser jsonRankingUser)
    {
        this.jsonRankingUser = jsonRankingUser;
        currentRequestComplete();
    }

    public void requestCaptureAnt(Action requestComplete, int userId, int antId)
    {
        backandObject.StartCoroutine(backandObject.requestCaptureAnt(userId, antId));
        currentRequestComplete = requestComplete;
    }

    public void requestCaptureAntComplete()
    {
        currentRequestComplete();
    }

    public void requestAddScore(Action requestComplete, int userId, int score)
    {
        backandObject.StartCoroutine(backandObject.requestAddScore(userId, score));
        currentRequestComplete = requestComplete;
    }

    public void requestAddScoreComplete()
    {
        currentRequestComplete();
    }

    public void requestCurrentTime(Action requestComplete)
    {
        backandObject.StartCoroutine("requestCurrentTime");
        currentRequestComplete = requestComplete;
    }

    public void requestAntTextures(Action requestComplete)
    {
        currentRequestComplete = requestComplete;

        if(loadTextureAntIdList.Count > 0)
        {
            backandObject.StartCoroutine(backandObject.requestAntTexture(loadTextureAntIdList[0]));
            loadTextureAntIdList.RemoveAt(0);
        }
        else
        {
            currentRequestComplete();
        }
            
    }

    public void requestAntTexturesComplete(int antId, JsonAntTexture jsonAntTexture)
    {
        LocalAntDataManager.getInstance().saveAntTexture(antId, jsonAntTexture);

        if(loadTextureAntIdList.Count > 0)
        {
            backandObject.StartCoroutine(backandObject.requestAntTexture(loadTextureAntIdList[0]));
            Debug.Log("Textura carregada: " + antId);
            loadTextureAntIdList.RemoveAt(0);
        }
        else
            currentRequestComplete();
    }

    public void requestCurrentTimeComplete(JsonTime jsonTime)
    {
        this.jsonTime = jsonTime;
        currentRequestComplete();
    }

    private void localSaveAntData(JsonAnt jsonAnt, JsonAntTexture jsonAntTexture)
    {
        DirectoryInfo dir = new DirectoryInfo(Application.persistentDataPath + "/formigo/antData/");
        StreamWriter writer = new StreamWriter(dir.FullName + jsonAnt.id + ".json");
        writer.Write(JsonUtility.ToJson(jsonAnt));
        writer.Close();

        DirectoryInfo dirTexture = new DirectoryInfo(Application.persistentDataPath + "/formigo/antTextures/");
        StreamWriter writerTexture = new StreamWriter(dirTexture.FullName + jsonAnt.id + ".json");
        writerTexture.Write(JsonUtility.ToJson(jsonAntTexture));
        writerTexture.Close();
    }
}
