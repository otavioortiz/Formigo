using Facebook.Unity;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class ConfigMenu : MonoBehaviour {

	// Use this for initialization
	void Start () {
        Text userNameField = GameObject.Find("userName").transform.FindChild("Text").gameObject.GetComponent<Text>();
        userNameField.text = DataManager.getInstance().userData.userName;

        Text userEmailField = GameObject.Find("userEmail").transform.FindChild("Text").gameObject.GetComponent<Text>();
        userEmailField.text = DataManager.getInstance().userData.userEmail;
    }

    // Update is called once per frame
    void Update () {
        if (Input.GetKeyUp(KeyCode.Escape))
            SceneManager.LoadScene("GameMenu");
    }

    public void clickInClose()
    {
        SceneManager.LoadScene("GameMenu");
    }

    public void clickInChangePhoto()
    {
        print("clickInChangePhoto");
    }

    public void clickInLogout()
    {
        print("clickInLogout");

        if (DataManager.getInstance().userData.facebookId != "")
        {
            Backend.getInstance().facebookPublishEnabled = false;
            FB.LogOut();
        }

        PlayerPrefs.SetString("UserName", "");
        PlayerPrefs.SetString("UserPass", "");
        PlayerPrefs.SetString("UserFacebookMail", "");
        PlayerPrefs.SetString("UserFacebookId", "");

        SceneManager.LoadScene("LoginMenu");
    }

    public void clickInSound()
    {
        if (AudioListener.volume != 0 ) 
            AudioListener.volume = 0;
        else
            AudioListener.volume = 100;
    }
}
