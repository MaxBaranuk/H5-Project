using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;

public class ExitListener : MonoBehaviour {

	// Use this for initialization
	void Start () {
//        StartCoroutine(ServerManager.instanse.getObjectByTargetID(""));
    }
	
	// Update is called once per frame
	void Update () {
        if (Input.GetKeyDown(KeyCode.Escape)) SceneManager.LoadScene("main");
    }
}
