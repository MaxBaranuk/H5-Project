﻿using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using System.Text.RegularExpressions;

public class MainPanelUIManager : MonoBehaviour {

    public GameObject mainMenuPanel;
    public GameObject menuPanel;
    public GameObject agentPanel;
    public GameObject contactMePanel;
    public GameObject contactPanel;
    public Button contactMeButton;
    public Button contactButton;
    public GameObject[] callTypeButtons;
    int currCallType = 0;
    public Sprite selectedBut;
    public Sprite unselectedBut;
    public InputField nameInputField;
    public InputField emailInputField;
    public InputField phoneInputField;
    public Button callMeButton;
    public GameObject exitPopupPanel;
    public Button exitMenuButton;
    public Button agentButton;
    public GameObject infoPanel;
    public GameObject contactH5panel;
    public GameObject howToUsePanel;
    public GameObject aboutPanel;
    public Text versionTextView;
    

#if UNITY_IOS
    [DllImport("__Internal")]
    private static extern void sendWhatsappMessage();
#endif
    // Use this for initialization
    void Start ()
    {
        versionTextView.text = "Version: "+Application.version;

    }
	
	// Update is called once per frame
	void Update () {

    }

    public void OpenExitPopup() {
        exitPopupPanel.SetActive(true);
        exitMenuButton.interactable = false;
        agentButton.interactable = false;
    }

    public void CloseExitPopup() {
        exitPopupPanel.SetActive(false);
        exitMenuButton.interactable = true;
        agentButton.interactable = true;
    }

    public void ExitApp() {
        Application.Quit();
    }

    public void OpenAgentPanel()
    {
        menuPanel.SetActive(false);
        agentPanel.SetActive(true);
    }

    public void CloseAgentPanel()
    {
        agentPanel.SetActive(false);
        mainMenuPanel.SetActive(false);
    }
    public void OpenInfoPanel() {
        infoPanel.SetActive(true);
        menuPanel.SetActive(false);
    }

    public void CloseInfoPanel()
    {
        infoPanel.SetActive(false);
        menuPanel.SetActive(true);
    }

    public void OpenAboutPanel() {
        aboutPanel.SetActive(true);
        menuPanel.SetActive(false);
    }

    public void CloseAboutPanel()
    {
        aboutPanel.SetActive(false);
        menuPanel.SetActive(true);
    }

    public void OpenContactH5Panel()
    {
        contactH5panel.SetActive(true);
        infoPanel.SetActive(false);
    }

    public void CloseContactH5Panel()
    {
        contactH5panel.SetActive(false);
        infoPanel.SetActive(true);
    }

    public void OpenHowToUsePanel()
    {
        howToUsePanel.SetActive(true);
        infoPanel.SetActive(false);
    }

    public void CloseHowToUsePanel()
    {
        howToUsePanel.SetActive(false);
        infoPanel.SetActive(true);
    }

    public void SendContactH5Request()
    {

    }

    public void CloseMenu() {
        menuPanel.SetActive(false);
        mainMenuPanel.SetActive(false);
    }

    public void ContactMeOpen() {
        contactMePanel.SetActive(true);
        contactPanel.SetActive(false);
        contactMeButton.interactable = false;
        contactButton.interactable = true;
    }

    public void ContactOpen() {
        contactMePanel.SetActive(false);
        contactPanel.SetActive(true);
        contactButton.interactable = false;
        contactMeButton.interactable = true;
    }

    public void SetCallType(int type) {
        foreach (GameObject b in callTypeButtons) b.GetComponentInChildren<Image>().sprite = unselectedBut;
        callTypeButtons[type].GetComponentInChildren<Image>().sprite = selectedBut;
        currCallType = type;
        CheckValidData();
    }

    public void CallRequest() {

    }

    public void CheckValidData()
    {
        bool isValid = true;
        switch (currCallType) {

            case 1:
                if (phoneInputField.text.Length < 5) isValid = false;
                break;
            case 2:
                if(emailInputField.text.Length < 5 | !ValidateEmail(emailInputField.text)) isValid = false;
                break;

        }
        callMeButton.interactable = isValid;
    }

    public void MakeCall()
    {
        Application.OpenURL("tel://2468461321");
    }

    public void SendSMS()
    {
        Application.OpenURL("sms://5646451245;body=loremipsum");
    }

    public void SendMail()
    {
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

    public void MakeChat()
    {
#if UNITY_ANDROID
        AndroidJavaClass unityClass = new AndroidJavaClass("com.unity3d.player.UnityPlayer");
        AndroidJavaObject unityActivity = unityClass.GetStatic<AndroidJavaObject>("currentActivity");
        AndroidJavaClass customClass = new AndroidJavaClass("com.wear.locationservice.UnityLocationService");
        customClass.CallStatic("SendWhatsappMessage", unityActivity, "My Message");
#elif UNITY_IOS
        sendWhatsappMessage();
#endif
    }

    string MyEscapeURL(string url)
    {
        return WWW.EscapeURL(url).Replace("+", "%20");
    }

    bool ValidateEmail(string email)
    {
        Regex regex = new Regex(@"^([\w\.\-]+)@([\w\-]+)((\.(\w){2,3})+)$");
        Match match = regex.Match(email);
        if (match.Success)
            return true;
        else
            return false;
    }

}