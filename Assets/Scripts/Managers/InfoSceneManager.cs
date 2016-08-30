using UnityEngine;
using System.Collections;
using UnityEngine.UI;
public class InfoSceneManager : MonoBehaviour {

    public Text info;
	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

    public void DownloadImage() {
        info.text = "" + Application.persistentDataPath;
    }
}
