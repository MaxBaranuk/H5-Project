using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;

public class ExitListener : MonoBehaviour {

    GameObject exitPopup;
    GameObject mainMenu;

	// Use this for initialization
	void Start () {
        mainMenu = GameObject.Find("Canvas").transform.FindChild("MainPanel").gameObject;
        exitPopup = mainMenu.transform.FindChild("ExitPopup").gameObject;
//        StartCoroutine(ServerManager.instanse.getObjectByTargetID(""));
    }
	
	// Update is called once per frame
	void Update () {
	    if (Input.GetKeyDown(KeyCode.Escape))
	    {
            mainMenu.SetActive(true);
            exitPopup.SetActive(true);
            
        }
	    
    }
}
