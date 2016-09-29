using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;

public class ArSceneManager : MonoBehaviour {

    public GameObject connectionInfoPanel;
    public GameObject mainMenuPanel;
    public GameObject menuPanel;

    // Use this for initialization

    public void ToMap() {
        SceneManager.LoadScene("Map");
    }

    public void ScreenShot() {

    }

    public void OpenMenuPanel()
    {
        mainMenuPanel.SetActive(true);
        menuPanel.SetActive(true);
    }
}
