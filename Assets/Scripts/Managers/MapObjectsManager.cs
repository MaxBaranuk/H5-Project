using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;
using System.Runtime.InteropServices;

public class MapObjectsManager : MonoBehaviour {

    OnlineMaps map;
    OnlineMapsTileSetControl mapControl;
    public GameObject[] itemPrefabs;
    LocationManager locationManager;

    // Use this for initialization
    void Start () {
        locationManager = GetComponent<LocationManager>();
        mapControl = GameObject.Find("Map").GetComponent<OnlineMapsTileSetControl>();
        map = GameObject.Find("Map").GetComponent<OnlineMaps>();
        Settings.LoadMapObjects();
        DownloadNewItems(LoadAllObjectIDs());
        AddPlaces(MapObjectsCache.items);
    }

    private void AddPlaces(Dictionary<int, Item> items)
    {
        foreach (Item it in items.Values)
        {
            if (getDistanceFromLatLonInKm((float) map.latitude,(float) map.longitude, it.Lat, it.Lon) < 10000) {
                OnlineMapsMarker3D inst = mapControl.AddMarker3D(new Vector2(it.Lon, it.Lat), itemPrefabs[0]);
                inst.Init(mapControl.transform);
            }           
        }
    }

    // Update is called once per frame
    void Update () {
	
	}

    List<int> LoadAllObjectIDs() {
        List<int> ids = new List<int> {0,1,2,3,4,5,6,7,8,9,10,11,12,13,14,15,16,17,18,19,20};
        return ids;
    }

    void DownloadNewItems(List<int> items) {
        List<int> itemsToDownload = new List<int>();
        foreach (int i in items) {
            if (!MapObjectsCache.items.ContainsKey(i)) DownloadNewItem(i);
        }

        foreach (int i in MapObjectsCache.items.Keys) {
            if (!items.Contains(i)) MapObjectsCache.items.Remove(i);
        }
    }

    void DownloadNewItem(int id) {
        Item i = new Item();
        i.id = id;
        i.info = "text";
        i.Lat = UnityEngine.Random.Range(-180, 180);
        i.Lon = UnityEngine.Random.Range(-50, 70);
        i.Name = "Item - " + id;
        i.type = "Home";
        MapObjectsCache.items.Add(id, i);
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

    void OnDisable() {
        Settings.SaveMapObjects();
    }
}
