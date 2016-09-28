using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;
using System.Collections.Generic;
using UnityEngine.UI;

public class LoadingSceneManager : MonoBehaviour {

    AsyncOperation async;
    public Text info;

	// Use this for initialization
	void Start () {
        switch (Settings.nextScene)
        {
            case Settings.SceneTypes.AR:
                StartCoroutine(LoadALevel("ARMode"));
                break;
            case Settings.SceneTypes.GEO:
                StartCoroutine(LoadALevel("Map"));
                break;
        }
    }
	
	// Update is called once per frame
	//void Update () {
	
	//}

    //IEnumerator LoadingAnimation() {
    //    for (int i = 0; i < animatedImages.Length; i++) {
    //        animatedImages[i].SetActive(true);
    //        yield return new WaitForSeconds(0.3225f);
    //    }
    //}

    private IEnumerator LoadALevel(string levelName)
    {
        async = SceneManager.LoadSceneAsync(levelName);
        yield return async;
    }
    
}
