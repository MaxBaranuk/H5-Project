using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class AnimatedPoint : MonoBehaviour {

    RectTransform rect;
//    LoadingSceneManager manager;
    // Use this for initialization
    void Start () {
//        manager = GameObject.Find("SceneManager").GetComponent<LoadingSceneManager>();
        rect = GetComponent<RectTransform>();
        StartCoroutine(Move());
	}
	
	// Update is called once per frame
	//void Update () {
		
	//}

    IEnumerator Move() {
        float step;
        float time = 0;
        while (true)
        {            
            step = Mathf.Abs(rect.localPosition.x / 15);
            if (step < 1f) step = 1f;
            rect.localPosition = new Vector2(rect.localPosition.x + step, rect.localPosition.y);
            yield return new WaitForSeconds(0.01f);
            time += 0.01f;
            if (rect.localPosition.x > 400) {
                rect.localPosition = new Vector2(-405, rect.localPosition.y);
//                manager.info.text = "" + time;
                time = 0;
            } 
        }
    }


}
