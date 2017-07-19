using System.Collections;
using System.Collections.Generic;
using System.IO;
using Facebook.Unity;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class SharePhotoScreen : MonoBehaviour {

    public static byte[] currentPhotoBytes;
    public static string lastScreen;

    private string fileName;

    private Image flashImage;

    // Use this for initialization
    void Start () {
        flashImage = GameObject.Find("flash").GetComponent<Image>();

        Texture2D t2d = new Texture2D(720, 1280);
        t2d.LoadImage(currentPhotoBytes);

        GameObject.Find("photoArea").GetComponent<Image>().sprite = Sprite.Create(t2d, new Rect(new Vector2(0, 0), new Vector2(600, 900)), new Vector2(0, 0));
    }
	
	// Update is called once per frame
	void Update () {
        if (Input.GetKeyUp(KeyCode.Escape))
            clickInReturn();



        if (flashImage.color.a > 0)
        {
            Color c = new Color(255,255,255);
            c.a = flashImage.color.a - 0.05f;
            flashImage.color = c;
        }
            
    }

    public void clickInReturn()
    {
        SceneManager.LoadScene(lastScreen);
    }

    public void clickInFacebook()
    {

        if (Backend.getInstance().facebookPublishEnabled == true)
        {
            if (FB.IsLoggedIn)
                captureAndSharePhoto();
        }
        else
        {
            List<string> perms = new List<string>() { "publish_actions" };
            FB.LogInWithPublishPermissions(perms, AuthCallback);
        }

        print("ckickInFacebook");
    }

    private void AuthCallback(ILoginResult result)
    {
        if (FB.IsLoggedIn)
        {
            Backend.getInstance().facebookPublishEnabled = true;
            captureAndSharePhoto();
        }
    }

    public void clickInSave()
    {
        fileName = "ant_" + System.DateTime.Now.ToString("yyyy_MM_dd_HH_mm_ss") + ".png";
        string currentPath = Application.persistentDataPath + "/formigo/" + fileName;

        FileStream fs = File.Create(currentPath);

        BinaryWriter binary = new BinaryWriter(fs);
        binary.Write(currentPhotoBytes);
        fs.Close();
    }

    private void captureAndSharePhoto()
    {
        var wwwForm = new WWWForm();
        wwwForm.AddBinaryData("image", currentPhotoBytes, "photo.png");

        FB.API("me/photos", HttpMethod.POST, null, wwwForm);
        clickInReturn();
    }
}
