using Facebook.Unity;
using System;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.SceneManagement;


public class LoadDataScreen : MonoBehaviour {

    private int timer = 0;

    private void Awake()
    {
        createFolders();
        loadAntData();
        initializeFacebook();
    }

    private void initializeFacebook()
    {
        if (!FB.IsInitialized)
            FB.Init(InitCallback, OnHideUnity);
        else
            FB.ActivateApp();
    }

    private void createFolders()
    {
        if (!Directory.Exists(Application.persistentDataPath + "/formigo/"))
            Directory.CreateDirectory(Application.persistentDataPath + "/formigo/");

        if (!Directory.Exists(Application.persistentDataPath + "/formigo/antData/"))
            Directory.CreateDirectory(Application.persistentDataPath + "/formigo/antData/");

        if (!Directory.Exists(Application.persistentDataPath + "/formigo/antTextures/"))
            Directory.CreateDirectory(Application.persistentDataPath + "/formigo/antTextures/");
    }

    private void loadAntData()
    {
        List<JsonAnt> inAppAntList = LocalAntDataManager.getInstance().getInnAppAntDataList();

        foreach(JsonAnt currentInnAppAnt in inAppAntList)
        {
            JsonAnt currentLocalAnt = LocalAntDataManager.getInstance().getAntData(currentInnAppAnt.id);
            if (currentLocalAnt == null )
            {
                LocalAntDataManager.getInstance().saveAntData(currentInnAppAnt);
                LocalAntDataManager.getInstance().saveAntTexture(currentInnAppAnt.id, LocalAntDataManager.getInstance().getInnAppAntTexture(currentInnAppAnt.id));
            }
            else
            {
                JsonAnt antUpdated = AntDataComparator.getUpdatedAnt(currentLocalAnt, currentInnAppAnt);
                if(antUpdated.updated)
                {
                    LocalAntDataManager.getInstance().saveAntData(antUpdated);
                    LocalAntDataManager.getInstance().saveAntTexture(antUpdated.id, LocalAntDataManager.getInstance().getInnAppAntTexture(antUpdated.id));
                }
            }
        }
    }

    private void InitCallback()
    {
        if (FB.IsInitialized)
        {
            FB.ActivateApp();
            timer = 1;
        }
        else
            Debug.Log("Failed to Initialize the Facebook SDK");
    }

    private void OnHideUnity(bool isGameShown)
    {
        if (!isGameShown)
            Time.timeScale = 0;
        else
            Time.timeScale = 1;
    }

    // Update is called once per frame
    void Update () {
        if(timer > 0)
        {
            timer++;
            if(timer == 100)
                SceneManager.LoadScene("LoginMenu");
        }
    }
}