using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;

public class ExitListener : MonoBehaviour {

    public GameObject exitPopup;
    public GameObject mainMenu;

	// Use this for initialization
	void Start () {
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
