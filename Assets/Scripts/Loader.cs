using UnityEngine;
using System.Collections;

public class Loader : MonoBehaviour {

    RectTransform rect;
	// Use this for initialization
	void Start () {
        rect = GetComponent<RectTransform>();

    }
	
	// Update is called once per frame
	void Update () {
        rect.Rotate(-3*Vector3.forward);

    }
}
