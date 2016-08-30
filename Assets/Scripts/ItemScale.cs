using UnityEngine;
using System.Collections;

public class ItemScale : MonoBehaviour {

    float initPointerSize;
    // Use this for initialization
    void Start () {
        initPointerSize = transform.localScale.x;
    }
	
    public void Rescale(float zoom) {
        float scale = initPointerSize * 65536 / (Mathf.Pow(2, zoom));
        transform.localScale = new Vector3(scale, scale, scale);
    }
}
