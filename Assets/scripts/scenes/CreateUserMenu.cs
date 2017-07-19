using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class CreateUserMenu : MonoBehaviour {

    private string currentEmail;
    private string currentPassword;
    private string errorMessage;
    private Text tErrorMessage;

    // Use this for initialization
    void Start () {
        tErrorMessage = GameObject.Find("tErrorMessage").GetComponent<Text>();
    }
	
	// Update is called once per frame
	void Update () {
        if (Input.GetKeyUp(KeyCode.Escape))
            clickInReturn();
    }

    public void clickInReturn(){
        SceneManager.LoadScene("StartMenu");
    }

    public void clickInRegister(){

        tErrorMessage.text = "";

        InputField inputName = GameObject.Find("inputName").GetComponent<InputField>();
        InputField inputPass = GameObject.Find("inputPass").GetComponent<InputField>();
        InputField inputEmail = GameObject.Find("inputEmail").GetComponent<InputField>();

        currentEmail = inputEmail.text;
        currentPassword = inputPass.text;

        print("Name: " + inputName.text);
        print("Pass: " + inputPass.text);
        print("Email: " + inputEmail.text);

        Backend.getInstance().setLockScreen(true);
        Backend.getInstance().requestCreateUser(createUserComplete, inputName.text, inputPass.text, inputEmail.text, "");

    }

    private void createUserComplete()
    {
        Backend.getInstance().requestLogin(loginComplete, currentEmail, currentPassword);
    }

    private void loginComplete()
    {
        if (Backend.getInstance().jsonError == null)
        {
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
        print("clickInFacebook");
    }

    public void clickInGoLogin()
    {
        print("clickInGoLogin");
        SceneManager.LoadScene("LoginMenu");
    }
}
