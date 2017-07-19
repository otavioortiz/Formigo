using Facebook.Unity;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class LoginMenu : MonoBehaviour {

    private string currentFacebookEmail;
    private string currentFacebookName;
    private string currentFacebookId;
    private Text tErrorMessage;

    private string errorMessage;

    private InputField inputName;
    private InputField inputPass;

    // Use this for initialization
    void Start () {
        tErrorMessage = GameObject.Find("tErrorMessage").GetComponent<Text>();

        inputName = GameObject.Find("inputName").GetComponent<InputField>();
        inputPass = GameObject.Find("inputPass").GetComponent<InputField>();

        inputName.text = PlayerPrefs.GetString("UserName", "");
        inputPass.text = PlayerPrefs.GetString("UserPass", "");

        if(PlayerPrefs.GetString("UserFacebookMail", "") != "")
        {
            Backend.getInstance().setLockScreen(true);
            Backend.getInstance().requestLogin(loginFacebookComplete, PlayerPrefs.GetString("UserFacebookMail", ""), PlayerPrefs.GetString("UserFacebookId", ""));
        }
        else if(PlayerPrefs.GetString("UserName", "") != "")
        {
            Backend.getInstance().setLockScreen(true);
            Backend.getInstance().requestLogin(loginComplete, PlayerPrefs.GetString("UserName", ""), PlayerPrefs.GetString("UserPass", ""));
        }

    }
	
	// Update is called once per frame
	void Update () {
		
	}

    public void clickInReturn()
    {
        SceneManager.LoadScene("StartMenu");
    }

    public void clickInConfirm()
    {
        tErrorMessage.text = "";
        
        Backend.getInstance().setLockScreen(true);
        Backend.getInstance().requestLogin(loginComplete, inputName.text, inputPass.text);
        //Backend.getInstance().requestLogin(loginComplete, "jsinoti@gmail.com", "123456");
        
        /*
        var wwwForm = new WWWForm();
        wwwForm.AddBinaryData("image", DataManager.getInstance().getAntData(1).avatar.EncodeToPNG(), "antData.png");
        wwwForm.AddField("message", "Teste Fenadoce");

        FB.API("me/photos", HttpMethod.POST, null, wwwForm);
        */
    }

    private void loginComplete()
    {
        if (Backend.getInstance().jsonError == null)
        {
            PlayerPrefs.SetString("UserName", inputName.text);
            PlayerPrefs.SetString("UserPass", inputPass.text);

            DataManager.getInstance().setUserData(new UserData(Backend.getInstance().jsonPlayer));
            SceneManager.LoadScene("GameMenu");
        }
        else
        {
            errorMessage = Backend.getInstance().jsonError.message;
            tErrorMessage.text = errorMessage;
        }

        Backend.getInstance().setLockScreen(false);
    }

    public void clickInFacebook()
    {
        Backend.getInstance().setLockScreen(true);
        List<string> perms = new List<string>() { "public_profile", "email", "user_friends" };
        FB.LogInWithReadPermissions(perms, AuthCallback);
    }

    public void clickInRegister()
    {
        print("clickInRegister");
        SceneManager.LoadScene("CreateUserMenu");
    }

    private void AuthCallback(ILoginResult result)
    {
        //GameObject.Find("testText").GetComponent<Text>().text = "";
        if (FB.IsLoggedIn)
        {
            var aToken = Facebook.Unity.AccessToken.CurrentAccessToken;

            FB.API("me?fields=name,email", HttpMethod.GET, delegate (IGraphResult FBResult)
            {
            currentFacebookEmail = (String)FBResult.ResultDictionary["email"];
            currentFacebookName = (String)FBResult.ResultDictionary["name"];
            currentFacebookId = (String)FBResult.ResultDictionary["id"];

            Backend.getInstance().requestLogin(loginFacebookComplete, currentFacebookEmail, currentFacebookId);
            });

        }
        else
        {
            Debug.Log("User cancelled login");
        }
    }

    private void loginFacebookComplete()
    {
        if( Backend.getInstance().jsonError == null )
        {
            PlayerPrefs.SetString("UserFacebookMail", currentFacebookEmail);
            PlayerPrefs.SetString("UserFacebookId", currentFacebookId);
            loginComplete();
        }
        else
        {
            Backend.getInstance().requestCreateUser(facebookCreateUserComplete, currentFacebookName, currentFacebookId, currentFacebookEmail, currentFacebookId);
            //cria usuario
        }
    }

    private void facebookCreateUserComplete()
    {
        if (Backend.getInstance().jsonError == null)
        {
            Backend.getInstance().requestLogin(loginComplete, currentFacebookEmail, currentFacebookId);
        }
        else
        {
            Backend.getInstance().setLockScreen(false);
            Debug.Log("CREATE USER ERROR!");
        }
    }
}
