using UnityEngine;
using System.Collections;

public class CameraMovement : MonoBehaviour {

    LocationInfo loc;
    Vector3 newUserPos;
    Vector3 iniRef;
    // Use this for initialization
    void Start () {
        iniRef.x = ((loc.longitude * 20037508.34f / 180) / 100);
        iniRef.z = Mathf.Log(Mathf.Tan((90f + loc.latitude) * Mathf.PI / 360f)) / (Mathf.PI / 180f);
        iniRef.z = ((iniRef.z * 20037508.34f / 180) / 100);
        iniRef.y = 0.05f;
    }
	
	// Update is called once per frame
	void Update () {
        loc = Input.location.lastData;
        newUserPos.x = ((loc.longitude * 20037508.34f / 180) / 100) - iniRef.x;
        newUserPos.y = 0.05f;
        newUserPos.z = Mathf.Log(Mathf.Tan((90 + loc.latitude) * Mathf.PI / 360)) / (Mathf.PI / 180);
        newUserPos.z = ((newUserPos.z * 20037508.34f / 180) / 100) - iniRef.z;
        transform.position = newUserPos;
        transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.Euler(0, Input.compass.trueHeading, 0), Time.time * 0.0005f);
    }
}
