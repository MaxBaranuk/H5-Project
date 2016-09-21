using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LoadingPanel : MonoBehaviour {

    public GameObject[] animatedImages;
    // Use this for initialization
    void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    void OnEnable() {
        StartCoroutine(LoadingAnimation());
    }

    void OnDisable() {
        for (int i = 0; i < animatedImages.Length; i++)
        {
            animatedImages[i].SetActive(false);
            RectTransform rt = animatedImages[i].GetComponent<RectTransform>();
            rt.localPosition = new Vector2(-405, rt.localPosition.y);
        }
    }

    IEnumerator LoadingAnimation()
    {
        for (int i = 0; i < animatedImages.Length; i++)
        {
            animatedImages[i].SetActive(true);
            yield return new WaitForSeconds(0.3225f);
        }
    }
}
