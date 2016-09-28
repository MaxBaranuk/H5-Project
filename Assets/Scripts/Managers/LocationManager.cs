using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;
using System.Threading;

public class LocationManager : MonoBehaviour {


    public bool hasAR = false;
    public ItemsCollection colletion;
//    LocationInfo lastLocation;
    public LocationInfo currLocation;
    MapUIManager mapManager;
    public HashSet<Item> nearItems;
    string message = "";
    int count = 0;
    bool hasMessageToSend = false;
    OnlineMaps map;
    OnlineMapsTileSetControl mapControl;
    public GameObject point;

    void Awake() {
        mapManager = GetComponent<MapUIManager>();
    }
    // Use this for initialization
    void Start () {
        currLocation = Input.location.lastData;
        //colletion = ItemsCollection.Load("Places");
        map = GameObject.Find("Map").GetComponent<OnlineMaps>();
        //mapControl = GameObject.Find("Map").GetComponent<OnlineMapsTileSetControl>();
        //AddPlaces(colletion);
        //nearItems = new HashSet<Item>();
    }

    void Update() {
        currLocation = Input.location.lastData;
 //       map.latitude = currLocation.latitude;
 //       map.longitude = currLocation.longitude;
    }
    //void LocationUpdater() {
    //    currLocation = Input.location.lastData;
    //    map.latitude = currLocation.latitude;
    //    map.longitude = currLocation.longitude;
    //}

    void AddPlaces(ItemsCollection places) {
        foreach (Item it in colletion.items) {

//            Instantiate(point);
//            point.GetComponent<OnlineMapsMarker3DInstance>().marker.SetPosition(it.Lon, it.Lat);

            OnlineMapsMarker3D inst = mapControl.AddMarker3D(new Vector2(it.Lon, it.Lat), point);
            inst.Init(mapControl.transform);
            //inst.transform.localPosition = new Vector3(transform.localPosition.x,
            //                                            45, transform.localPosition.z);
        }
    }

    IEnumerator UpdateItems()
    {
        while (Input.location.isEnabledByUser)
        {
            foreach (Item it in colletion.items)
            {
                float dist = getDistanceFromLatLonInKm(currLocation.latitude, currLocation.longitude, it.Lat, it.Lon);

                // draw items on map
                if (dist < 2 && !mapManager.itemsOnScene.ContainsKey(it)) mapManager.CreatePoint(it);
                if (dist > 2 && mapManager.itemsOnScene.ContainsKey(it)) mapManager.DestroyPoint(it);

            }
            yield return new WaitForSeconds(10);
        }
    }

    void SendNotification(string name) {
#if UNITY_ANDROID
        AndroidLocalNotification.SendNotification(0, TimeSpan.Zero, "Notification", name);
        count++;
#elif UNITY_IOS
        var notif = new UnityEngine.iOS.LocalNotification();
		notif.fireDate = DateTime.Now.AddSeconds(0);
        notif.alertAction = "Notification";
        notif.alertBody = "Hello!";
        notif.alertLaunchImage = "";
       UnityEngine.iOS.NotificationServices.ScheduleLocalNotification(notif);
#endif
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
