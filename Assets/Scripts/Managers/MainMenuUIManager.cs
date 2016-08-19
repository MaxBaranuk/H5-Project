using UnityEngine;
using System.Collections;
using System;
using UnityEngine.SceneManagement;

public class MainMenuUIManager : MonoBehaviour {

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
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

    }

    public void OpenInfoMode() {

    }
}
