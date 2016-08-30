using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;

public class LoadingSceneManager : MonoBehaviour {

	// Use this for initialization
	void Start () {
        StartCoroutine(LoadALevel("main"));
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
