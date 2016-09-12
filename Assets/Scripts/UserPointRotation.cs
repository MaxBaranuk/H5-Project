using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;

public class UserPointRotation : MonoBehaviour {

    Queue<Vector3> rotationVectors;
    Vector3 speed;
//    public Text info;
    Vector3 lastPosition;
    Vector3 currPosition;
    Vector3 mainSpeed;
    GameObject threeDpoint;
    GameObject stayPoint;
    // Use this for initialization
    void Start () {
        rotationVectors = new Queue<Vector3>();
        mainSpeed = new Vector3();
        threeDpoint = transform.FindChild("3D_Pointer").gameObject;
        stayPoint = transform.FindChild("Stay_Point").gameObject;
        lastPosition = transform.position;
        StartCoroutine(CheckSpeed());
    }
	
	// Update is called once per frame
	void Update () {
        Vector3 newDir = Vector3.RotateTowards(transform.up, mainSpeed, 1, 0.0F);
        threeDpoint.transform.rotation = Quaternion.LookRotation(newDir);
        
        
    }

    IEnumerator CheckSpeed() {
        while (true) {
            currPosition = transform.position;
            speed = currPosition - lastPosition;
 //           info.text = "" + speed + " speed: " + speed.magnitude;
            
            lastPosition = currPosition;

            if (rotationVectors.Count > 5) rotationVectors.Dequeue();
            rotationVectors.Enqueue(speed);

            mainSpeed = Vector3.zero;
            IEnumerator<Vector3> ie = rotationVectors.GetEnumerator();
            while (ie.MoveNext()) {
                mainSpeed += ie.Current;
            }
            mainSpeed /= rotationVectors.Count;
 //           info.text += "\n" + mainSpeed + " main speed: " + mainSpeed.magnitude;
            if (mainSpeed.magnitude > 0.01f)
            {
                threeDpoint.SetActive(true);
                stayPoint.SetActive(false);
            }
            else {
                threeDpoint.SetActive(false);
                stayPoint.SetActive(true);
            }
            yield return new WaitForSeconds(0.2f);
        }
    }
}
