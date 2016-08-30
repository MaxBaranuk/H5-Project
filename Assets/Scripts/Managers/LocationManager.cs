using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;
using com.youvisio;
using System.Threading;

public class LocationManager : MonoBehaviour {


    public bool hasAR = false;
    ItemsCollection colletion;
    LocationInfo lastLocation;
    public LocationInfo currLocation;
    MapUIManager mapManager;
    public HashSet<Item> nearItems;
    private BackgroundWorker _backgroundWorker;
    string message = "";
    int count = 0;
    bool hasMessageToSend = false;

    void Awake() {
        mapManager = GetComponent<MapUIManager>();
    }
    // Use this for initialization
    void Start () {
        colletion = ItemsCollection.Load("Places");
        currLocation = Input.location.lastData;
        lastLocation = Input.location.lastData;
        nearItems = new HashSet<Item>();
        StartBackgroundService();
        StartCoroutine(UpdateItems());
 //       InvokeRepeating("UpdateItems", 1, 1);
        if (_backgroundWorker != null) _backgroundWorker.CancelAsync();
       
 //       if(mapManager.notificationsOn) StartBackgroundService();

        //        InvokeRepeating("CheckForARObjects", 1, 1);
    }
	
	// Update is called once per frame
	void Update () {
        currLocation = Input.location.lastData;
//        if (_backgroundWorker != null) _backgroundWorker.Update();
        //if (hasMessageToSend) {
        //    SendNotification(message);
        //    hasMessageToSend = false;
        //}
    }

    public void StartBackgroundService()
    {
        _backgroundWorker = new BackgroundWorker();
        _backgroundWorker.DoWork += (o, a) =>
        {
            //           bool isFinish = false;
            while (Input.location.isEnabledByUser)
            {
                
                foreach (Item it in colletion.items)
                {
                    float dist = getDistanceFromLatLonInKm(Input.location.lastData.latitude, Input.location.lastData.longitude, it.Lat, it.Lon);
                    bool hasObj = false;
                    if (dist < 0.1f) hasObj = nearItems.Add(it);
                    // check for Ar objects
                    if (hasObj & mapManager.notificationsOn)
                    {
                        hasMessageToSend = true;
                        message = it.Name + " - " + count;
                        SendNotification(message);
                    }
                    if (dist > 0.15f) nearItems.Remove(it);

                    if (nearItems.Count > 0) hasAR = true;
                    else hasAR = false;

                }
                _backgroundWorker._thread.Join(500);
            }
        };
        _backgroundWorker.RunWorkerCompleted += (o, a) =>
        {
            // executed on main thread
            // you can use a.Result
        };

        _backgroundWorker.RunWorkerAsync("service");
    }

    IEnumerator UpdateItems()
    {
        while (Input.location.isEnabledByUser)
        {
            foreach (Item it in colletion.items)
            {
                float dist = getDistanceFromLatLonInKm(currLocation.latitude, currLocation.longitude, it.Lat, it.Lon);

                // draw items on map
                if (dist < 2 && !mapManager.itemsOnScene.Contains(it)) mapManager.CreatePoint(it);
                if (dist > 2 && mapManager.itemsOnScene.Contains(it)) mapManager.DestroyPoint(it);

                //// check for Ar objects
                //bool hasObj = false;
                //if (dist < 0.1f) hasObj = nearItems.Add(it);
                //// check for Ar objects
                //if (hasObj & mapManager.notificationsOn)
                //{
                //    hasMessageToSend = true;
                //    message = it.Name + " - " + count;
                //}
                //if (dist > 0.15f) nearItems.Remove(it);

                //if (nearItems.Count > 0) hasAR = true;
                //else hasAR = false;
            }
            yield return new WaitForSeconds(10);
        }
    }

    //void CheckForNotification() {
    //    foreach (Item it in colletion.items)
    //    {
    //        float dist = getDistanceFromLatLonInKm(currLocation.latitude, currLocation.longitude, it.Lat, it.Lon);

    //        // check for Ar objects
    //        if (nearItems.Add(it)&mapManager.notificationsOn & dist < 0.1f) SendNotification(it.Name);
    //        if (dist > 0.2f) nearItems.Remove(it);

    //        if (nearItems.Count > 0) hasAR = true;
    //        else hasAR = false;

    //    }
    //}

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
        NotificationServices.ScheduleLocalNotification(notif);
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
