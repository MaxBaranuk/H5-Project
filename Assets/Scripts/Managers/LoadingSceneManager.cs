using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;
using System.Collections.Generic;
using UnityEngine.UI;

public class LoadingSceneManager : MonoBehaviour {

    AsyncOperation async;
    public Transform panel;
    public GameObject [] animatedImages;
    public Text info;
    void Awake() {

    }
	// Use this for initialization
	void Start () {
 //       images =  GameObject.FindGameObjectsWithTag("AnimatedPoint");
        StartCoroutine(LoadingAnimation());

        switch (Settings.nextScene)
        {
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

    IEnumerator LoadingAnimation() {

        for (int i = 0; i < animatedImages.Length; i++) {
            animatedImages[i].SetActive(true);
            yield return new WaitForSeconds(0.3225f);
        }
        //for (int i = 0; i < images.Length; i++)
        //{
        //    images[i].SetActive(true);
        //    yield return new WaitForSeconds(1);
        //}
    }

    private IEnumerator LoadALevel(string levelName)
    {
        async = SceneManager.LoadSceneAsync(levelName);
        yield return async;
    }
    
}
