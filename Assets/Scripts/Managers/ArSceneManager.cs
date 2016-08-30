using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;

public class ArSceneManager : MonoBehaviour {

    public GameObject connectionInfoPanel;

    // Use this for initialization
    void Awake() {
//        StartCoroutine(CheckInternetConnection());
    }

    void Update() {
        connectionInfoPanel.SetActive(!ServerManager.instanse.hasInternetConnection);
    }

    public void OpenAgentScene() {
        SceneManager.LoadScene("contactScene");
    }
}
