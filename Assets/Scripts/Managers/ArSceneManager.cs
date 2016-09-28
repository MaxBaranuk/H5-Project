using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;

public class ArSceneManager : MonoBehaviour {

    public GameObject connectionInfoPanel;
    public GameObject menuPanel;
    public GameObject agentPanel;
    // Use this for initialization

    public void ToMap() {
        SceneManager.LoadScene("Map");
    }

    public void OpenAgentScene() {
        SceneManager.LoadScene("contactScene");
    }

    public void OpenMenuPanel() {
        menuPanel.SetActive(true);
    }

    public void OpenAgentPanel() {
        menuPanel.SetActive(false);
        agentPanel.SetActive(true);
    }
}
