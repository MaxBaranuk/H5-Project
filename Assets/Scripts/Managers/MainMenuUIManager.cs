using UnityEngine;
using System.Collections;
using System;
using UnityEngine.SceneManagement;

public class MainMenuUIManager : MonoBehaviour {

    public GameObject connectionInfoPanel;
    void Awake() {
//        StartCoroutine(CheckInternetConnection());
    }

	void Start () {
#if UNITY_IOS
        UnityEngine.iOS.NotificationServices.RegisterForNotifications(
                UnityEngine.iOS.NotificationType.Alert |
                UnityEngine.iOS.NotificationType.Badge |
                UnityEngine.iOS.NotificationType.Sound);
#endif
    }

    void Update () {
        connectionInfoPanel.SetActive(!ServerManager.instanse.hasInternetConnection);
        ExitListener();
	}

    private void ExitListener() {
        if (Input.GetKeyDown(KeyCode.Escape)) Application.Quit();
    }

    public void OpenARMode() {
        SceneManager.LoadScene("ARMode");
    }

    public void OpenGEOMode() {
        SceneManager.LoadScene("MapMode3D");
    }

    public void OpenContactMode() {
        SceneManager.LoadScene("contactScene");
    }

    public void OpenInfoMode()
    {
        SceneManager.LoadScene("infoScene");
    }
}
