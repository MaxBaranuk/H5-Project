using UnityEngine;
using System.Collections;

public class UserPoint : MonoBehaviour {

    OnlineMapsMarker3DInstance instanse;
    LocationInfo currLocation;
    // Use this for initialization
    void Start () {
        instanse = GetComponent<OnlineMapsMarker3DInstance>();
        InvokeRepeating("LocationUpdater", 0, 0.5f);
    }
	
	// Update is called once per frame
	void Update () {
	
	}

    void LocationUpdater()
    {
        currLocation = Input.location.lastData;
        instanse.marker.SetPosition(currLocation.longitude, currLocation.latitude);
 //       instanse._longitude = currLocation.longitude;
    }
}
