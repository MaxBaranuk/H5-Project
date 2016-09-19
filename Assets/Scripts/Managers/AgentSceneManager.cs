using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using System.Text.RegularExpressions;
using System.Net.Mail;
using System;

public class AgentSceneManager : MonoBehaviour {

    
    public GameObject contactMeForm;
    public InputField nameInputField;
    public InputField emailInputField;
    public InputField phoneInputField;
    public Button callMeButton;
    // Use this for initialization
    void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

    public void MakeCall() {
        Application.OpenURL("tel://2468461321");
    }

    public void SendSMS() {
        Application.OpenURL("sms://5646451245;body=loremipsum");
    }

    public void SendMail() {
        //email Id to send the mail to
        string email = "info.wearstudio@wear-studio.com";
        //subject of the mail
        string subject = MyEscapeURL("Support");
        //body of the mail which consists of Device Model and its Operating System
        string body = MyEscapeURL("Please Enter your message here\n\n\n\n" +
         "________" +
         "\n\nSend from\n\n" +
         "Model: " + SystemInfo.deviceModel + "\n\n" +
            "OS: " + SystemInfo.operatingSystem + "\n\n");

        //Open the Default Mail App
        Application.OpenURL("mailto:" + email + "?subject=" + subject + "&body=" + body);
    }

    public void MakeChat() {
        AndroidJavaClass unityClass = new AndroidJavaClass("com.unity3d.player.UnityPlayer");
        AndroidJavaObject unityActivity = unityClass.GetStatic<AndroidJavaObject>("currentActivity");
        AndroidJavaClass customClass = new AndroidJavaClass("com.wear.locationservice.UnityLocationService");
        customClass.CallStatic("SendWhatsappMessage", unityActivity, "My Message");
    }

    public void ContactMe() {
        contactMeForm.SetActive(true);
    }

    public void ExitToMenu() {
        SceneManager.LoadScene("main");
    }

    string MyEscapeURL(string url)
    {
        return WWW.EscapeURL(url).Replace("+", "%20");
    }

    public void CheckValidData() {

        bool isValid = phoneInputField.text.Length > 5 | (emailInputField.text.Length > 5 & ValidateEmail(emailInputField.text));
        callMeButton.interactable = isValid;
    }

    public void SendContactRequest() {
        contactMeForm.SetActive(false);
    }

    public void CloseContactMeForm() {
        contactMeForm.SetActive(false);
    }


    bool ValidateEmail(string email) {
        Regex regex = new Regex(@"^([\w\.\-]+)@([\w\-]+)((\.(\w){2,3})+)$");
        Match match = regex.Match(email);
        if (match.Success)
            return true;
        else
            return false;
    }
}
