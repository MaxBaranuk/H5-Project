using UnityEngine;
using System.Collections;
using UnityEngine.UI;
public class MapArSceneManager : MonoBehaviour {

    public Text info;
    GameObject cam;
    GameObject box;
	// Use this for initialization
	void Start () {
        cam = GameObject.Find("ARCamera");
        box = GameObject.Find("Cube");
    }
	
	// Update is called once per frame
	void Update () {
        info.text = "Cam: " + cam.transform.position + "\n" + "Box: " + box.transform.position+"\nRot: "+cam.transform.rotation;
    }
}
