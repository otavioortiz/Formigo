using Facebook.Unity;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class PhotoScreenSelfie : MonoBehaviour {

    private WebCamTexture webcamTexture;
    private AntData currentAntData;
    private Renderer mr;

    private string fileName;
    // Use this for initialization
    void Start () {
        GameObject.Find("AntModel").GetComponent<Animator>().SetTrigger("goAvatarPose");
        mr = GameObject.Find("modelBody").GetComponent<SkinnedMeshRenderer>();

        currentAntData = DataManager.getInstance().getAntData(DataManager.getInstance().getCurrentShowedAntId());
        mr.material.mainTexture = currentAntData.getTexture();

        //webcamTexture = new WebCamTexture();
        changeSelfieCam();
        //Renderer renderer = GameObject.Find("PlaneCam").GetComponent<Renderer>();
        //renderer.material.mainTexture = webcamTexture;

        GameObject.Find("UIPhoto").GetComponent<Image>().material.mainTexture = webcamTexture;

        webcamTexture.Play();
    }
	
	// Update is called once per frame
	void Update () {
        if (Input.GetKeyUp(KeyCode.Escape))
            clickInReturn();
    }

    public void clickInReturn()
    {
        webcamTexture.Stop();
        SceneManager.LoadScene("AntDataScreen");
    }

    public void clickInPhoto()
    {
        Camera cam = GameObject.Find("AntCam").GetComponent<Camera>();

        RenderTexture renderTexture = new RenderTexture(600, 900, 24);
        Texture2D sourceRender = new Texture2D(600, 900);
        RenderTexture.active = renderTexture;

        cam.targetTexture = renderTexture;
        cam.Render();
        sourceRender.ReadPixels(new Rect(0, 0, 600, 900), 0, 0, true);

        SharePhotoScreen.currentPhotoBytes = sourceRender.EncodeToPNG();
        SharePhotoScreen.lastScreen = "PhotoScreenSelfie";

        webcamTexture.Stop();

        SceneManager.LoadScene("SharePhotoSCreen");
    }

    private void changeSelfieCam()
    {
        WebCamDevice[] devices = WebCamTexture.devices;

        webcamTexture = new WebCamTexture();

        if(devices.Length > 1)
            webcamTexture.deviceName = devices[1].name;

        webcamTexture.Play();
    }
}
