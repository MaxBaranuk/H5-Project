using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class UserPoint : MonoBehaviour
{
    private LocationInfo lastLocation;
    private LocationInfo currLocation;

    private GameObject arrrow;
    private OnlineMaps map;
    private OnlineMapsMarker3DInstance point;
    private Text info;
    // Use this for initialization
    void Start ()
    {
        Input.location.Start();
        map = GameObject.Find("Map").GetComponent<OnlineMaps>();
        info = GameObject.Find("Canvas").transform.FindChild("Info").GetComponent<Text>();
        point = GetComponentInParent<OnlineMapsMarker3DInstance>();
        arrrow = transform.FindChild("arrow").gameObject;
        lastLocation = Input.location.lastData;
       InvokeRepeating("LocationUpdate", 0, 0.1f);
    }
	
	// Update is called once per frame
	void Update ()
	{
        currLocation = Input.location.lastData;
        Vector2 newCoordinats = new Vector2(currLocation.longitude, currLocation.latitude);
        point.marker.LookToCoordinates(newCoordinats);
        point.marker.position = newCoordinats;
        float dist = getDistanceFromLatLonInKm(currLocation.latitude, currLocation.longitude, lastLocation.latitude, lastLocation.longitude);
        info.text = "" + currLocation.latitude + ", " + currLocation.longitude +
            "\n" + dist
            + "\n" + map.latitude + ", " + map.longitude;
        Vector3 lastPos = OnlineMapsTileSetControl.instance.GetWorldPosition(lastLocation.longitude, lastLocation.latitude);
        Vector3 currPos = OnlineMapsTileSetControl.instance.GetWorldPosition(currLocation.longitude, currLocation.latitude);
        Vector3 dir = currPos - lastPos;
        arrrow.SetActive(dist > 0.001);
        
        arrrow.transform.rotation = Quaternion.LookRotation(dir);
        lastLocation = currLocation;
       
    }

    void LocationUpdate()
    {
        
    }

    float getDistanceFromLatLonInKm(float lat1, float lon1, float lat2, float lon2)
    {
        var R = 6371; // Radius of the earth in km
        float dLat = Mathf.Deg2Rad * (lat2 - lat1);
        float dLon = Mathf.Deg2Rad * (lon2 - lon1);
        var a =
          Mathf.Sin(dLat / 2) * Mathf.Sin(dLat / 2) +
          Mathf.Cos(Mathf.Deg2Rad * (lat1)) * Mathf.Cos(Mathf.Deg2Rad * (lat2)) *
          Mathf.Sin(dLon / 2) * Mathf.Sin(dLon / 2)
          ;
        var c = 2 * Mathf.Atan2(Mathf.Sqrt(a), Mathf.Sqrt(1 - a));
        float d = R * c; // Distance in km
        return d;
    }
}
