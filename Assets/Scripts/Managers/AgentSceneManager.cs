using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;

public class AgentSceneManager : MonoBehaviour {

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

    }

    public void ExitToMenu() {
        SceneManager.LoadScene("main");
    }

    string MyEscapeURL(string url)
    {
        return WWW.EscapeURL(url).Replace("+", "%20");
    }
}
