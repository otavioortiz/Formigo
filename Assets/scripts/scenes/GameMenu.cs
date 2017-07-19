using System;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameMenu : MonoBehaviour {

    private AntData currentAntData;
    private Renderer mr;
    private List<int> loadAntIdList;

    //Variáveis referentes aos avatares das formigas
    private Camera antAvatarCam;
    private RenderTexture antAvatarRenderTexture;
    private Texture2D avatarAntoSourceRender;
    private Rect avatarAntPhotoRect;
    private FileStream antAvatarFileStream;
    private BinaryWriter antAvatarBinaryWriter;
    private byte[] antAvatarFileData;
    private Texture2D currentAvatarTexture;
    private List<int> loadAntAvatarList;
    //

    // Use this for initialization
    void Start () {
        Text scoreText = GameObject.Find("tScore").GetComponent<Text>();
        scoreText.text = DataManager.getInstance().userData.score + "";
    }
	
    public void clickInCapture(){
        SceneManager.LoadScene("CaptureScreen");
    }

    public void clickInAnts(){
        SceneManager.LoadScene("ChooseAntScreen");
    }

    public void clickInSearchAnts(){
        SceneManager.LoadScene("FindAntsScreen");
    }

    public void clickInRanking(){
        SceneManager.LoadScene("RankingMenu");
    }

    public void clickInConfig()
    {
        SceneManager.LoadScene("ConfigMenu");
    }

    //Sistema que captura avatares das formigas

    private void Awake()
    {
        loadAntIdList = new List<int>();

        GameObject.Find("AntModel").GetComponent<Animator>().SetTrigger("goParada");
        mr = GameObject.Find("modelBody").GetComponent<SkinnedMeshRenderer>();

        if (Backend.updateAntDataActived)
        {
            if (Backend.getInstance().lastUpdated.Hour != DateTime.Now.Hour || Backend.getInstance().lastUpdated.Year == 1)
            {
                Backend.getInstance().facebookPublishEnabled = false;
                Backend.getInstance().setLockScreen(true);
                Backend.getInstance().lastUpdated = DateTime.Now;
                Backend.getInstance().requestAntDataList(antListRequested);
            }
            else
            {
                //requestAntTexturesComplete();
                loadComplete();
            }
        }
        else
        {
            Backend.getInstance().facebookPublishEnabled = false;
            Backend.getInstance().setLockScreen(true);
            Backend.getInstance().lastUpdated = DateTime.Now;
            requestAntTexturesComplete();
        }
    }

    //Verifica atualização das formigas, salva localmente e adiciona no dataManager
    private void antListRequested()
    {
        Backend.getInstance().loadTextureAntIdList = new List<int>();

        foreach (JsonAnt jsonAnt in Backend.getInstance().jsonAntDataList.antList)
        {
            JsonAnt currentLocalAnt = LocalAntDataManager.getInstance().getAntData(jsonAnt.id);
            if (currentLocalAnt == null)
            {
                LocalAntDataManager.getInstance().saveAntData(jsonAnt);
                Backend.getInstance().loadTextureAntIdList.Add(jsonAnt.id);
            }
            else
            {
                JsonAnt antUpdated = AntDataComparator.getUpdatedAnt(currentLocalAnt, jsonAnt);
                if(antUpdated.updated)
                {
                    LocalAntDataManager.getInstance().saveAntData(antUpdated);
                    Backend.getInstance().loadTextureAntIdList.Add(jsonAnt.id);
                }   
            }
        }

        DataManager.getInstance().cleanAntData();
        foreach (JsonAnt jsonAnt in Backend.getInstance().jsonAntDataList.antList)
            DataManager.getInstance().getAntList().Add(new AntData(jsonAnt));

        Backend.getInstance().requestAntTextures(requestAntTexturesComplete);
    }

    private void requestAntTexturesComplete()
    {
        startPhotos();
    }

    //BACKEND-END

    // Update is called once per frame
    void Update()
    {

    }

    private void loadComplete()
    {
        Backend.getInstance().setLockScreen(false);
        print("LOAD COMPLETE");
    }

    private void startPhotos()
    {
        loadAntAvatarList = new List<int>();
        antAvatarCam = GameObject.Find("AntCam").GetComponent<Camera>();
        antAvatarRenderTexture = new RenderTexture(100, 100, 24);
        avatarAntoSourceRender = new Texture2D(100, 100);
        avatarAntPhotoRect = new Rect(0.0f, 0.0f, 100, 100);

        for (int i = 0; i < DataManager.getInstance().getAntList().Count; i++)
            loadAntAvatarList.Add(DataManager.getInstance().getAntList()[i].id);

        nextAntAvatar();
    }

    private void nextAntAvatar()
    {
        if(loadAntAvatarList.Count > 0)
        {
            createOrLoadAntAvatar(loadAntAvatarList[0]);
            loadAntAvatarList.RemoveAt(0);
            Resources.UnloadUnusedAssets();
            InvokeRepeating("nextAntAvatar", 0.1f, 0);
        }
        else
        {
            antAvatarCam = null;
            antAvatarRenderTexture = null;
            avatarAntoSourceRender = null;
            antAvatarFileStream = null;
            antAvatarBinaryWriter = null;
            antAvatarFileData = null;
            currentAvatarTexture = null;
            Resources.UnloadUnusedAssets();

            loadComplete();
        }
    }

    private void createOrLoadAntAvatar(int antId)
    {
        currentAntData = DataManager.getInstance().getAntData(antId);
        mr.material.mainTexture = currentAntData.getTexture();

        string currentPath = Application.persistentDataPath + "/formigo/" + currentAntData.id + ".png";

        if(!File.Exists(currentPath))
        {
            RenderTexture.active = antAvatarRenderTexture;

            antAvatarCam.targetTexture = antAvatarRenderTexture;
            antAvatarCam.Render();
            avatarAntoSourceRender.ReadPixels(avatarAntPhotoRect, 0, 0);

            antAvatarFileStream = File.Create(currentPath);

            antAvatarBinaryWriter = new BinaryWriter(antAvatarFileStream);
            antAvatarBinaryWriter.Write(avatarAntoSourceRender.EncodeToPNG());
            antAvatarFileStream.Close();
        }

        antAvatarFileData = File.ReadAllBytes(currentPath);
        antAvatarFileData.Initialize();

        currentAvatarTexture = new Texture2D(1, 1);
        currentAvatarTexture.LoadImage(antAvatarFileData);

        currentAntData.avatar = currentAvatarTexture;
    }
}
