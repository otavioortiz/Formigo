using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class PhotoScreenFriend : MonoBehaviour {

    private WebCamTexture webcamTexture;
    private AntData currentAntData;
    private Renderer mr;

    
    // Use this for initialization
    void Start () {

        GameObject.Find("AntModel").GetComponent<Animator>().SetTrigger("goAvatarPose");
        mr = GameObject.Find("modelBody").GetComponent<SkinnedMeshRenderer>();

        currentAntData = DataManager.getInstance().getAntData( DataManager.getInstance().getCurrentShowedAntId() );
        mr.material.mainTexture = currentAntData.getTexture();

        webcamTexture = new WebCamTexture();
        //Renderer renderer = GameObject.Find("PlaneCam").GetComponent<Renderer>();
        //renderer.material.mainTexture = webcamTexture;

        GameObject.Find("UIPhoto").GetComponent<Image>().material.mainTexture = webcamTexture;

        webcamTexture.Play();
    }
	
	// Update is called once per frame
	void Update () {
        if (Input.GetKeyUp(KeyCode.Escape))
            ckickInReturn();
    }

    public void ckickInReturn(){
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
        SharePhotoScreen.lastScreen = "PhotoScreenFriend";

        webcamTexture.Stop();

        SceneManager.LoadScene("SharePhotoSCreen");
    }
}
