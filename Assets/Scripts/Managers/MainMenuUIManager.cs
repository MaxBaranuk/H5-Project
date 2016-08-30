﻿using UnityEngine;
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

    //IEnumerator CheckInternetConnection()
    //{
    //    while (true)
    //    {
    //        WWW www = new WWW("http://192.168.1.105/dashboard");

    //        float waitTime = 2;
    //        while (!www.isDone && waitTime > 0)
    //        {
    //            yield return new WaitForSeconds(1);
    //            waitTime--;
    //        }
    //        bool hasInternetConnection = false;
    //        if (!www.isDone | www.error != null)
    //        {
    //            hasInternetConnection = false;
    //        }
    //        else {
    //            hasInternetConnection = true;
    //        }
    //        connectionInfoPanel.SetActive(!hasInternetConnection);
    //        yield return new WaitForSeconds(1);
    //    }
    //}

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
    }
}
