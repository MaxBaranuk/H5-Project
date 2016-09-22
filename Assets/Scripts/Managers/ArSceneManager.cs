using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;

public class ArSceneManager : MonoBehaviour {

    public GameObject connectionInfoPanel;

    // Use this for initialization

    public void Exit() {
        SceneManager.LoadScene("main");
    }

    public void OpenAgentScene() {
        SceneManager.LoadScene("contactScene");
    }
}
