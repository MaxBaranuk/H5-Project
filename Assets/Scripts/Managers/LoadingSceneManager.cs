using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;

public class LoadingSceneManager : MonoBehaviour {

    void Awake() {

    }
	// Use this for initialization
	void Start () {
        switch (Settings.nextScene) {
            case Settings.SceneTypes.AR:
                StartCoroutine(LoadALevel("ARMode"));
                break;
            case Settings.SceneTypes.GEO:
                StartCoroutine(LoadALevel("MapMode3D"));
                break;

        }
       
    }
	
	// Update is called once per frame
	void Update () {
	
	}

    private IEnumerator LoadALevel(string levelName)
    {
        SceneManager.LoadSceneAsync(levelName);
        yield return null;
    }
}
