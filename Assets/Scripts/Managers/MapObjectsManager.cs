﻿using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;
using System.Runtime.InteropServices;
using WeAr.H5.Domain.Model;
using WeAr.H5.WebAPI.Client;

public class MapObjectsManager : MonoBehaviour {

    OnlineMapsTileSetControl mapControl;
    public GameObject[] itemPrefabs;
    public GameObject userPointPrefab;
    private OnlineMapsMarker3D userPoint;

    // Use this for initialization
    void Start () {
        mapControl = GameObject.Find("Map").GetComponent<OnlineMapsTileSetControl>();
        Settings.LoadMapObjects();
        DownloadNewItems(LoadAllObjectIDs());
        AddPlaces(MapObjectsCache.items);
        AddUserPoint();
    }

    private void AddPlaces(Dictionary<int, ObjectItem> items)
    {
        foreach (ObjectItem it in items.Values)
        {
                OnlineMapsMarker3D inst = mapControl.AddMarker3D(new Vector2(it.Longitude, it.Latitude), itemPrefabs[0]);
                inst.label = it.Name;
                inst.Init(mapControl.transform);     
        }
    }

    void AddUserPoint()
    {
        userPoint = mapControl.AddMarker3D(new Vector2(Input.location.lastData.longitude, Input.location.lastData.latitude), userPointPrefab);
        userPoint.Init(mapControl.transform);
    }

    // Update is called once per frame
    void Update () {
//        userPoint.SetPosition(Input.location.lastData.longitude, Input.location.lastData.latitude);

    }

    List<int> LoadAllObjectIDs() {
        List<int> ids = new List<int>();
        for (int i = 1; i < 25; i++) {
            ids.Add(i);
        }
        return ids;
    }

    void DownloadNewItems(List<int> items) {
        foreach (int i in items) {
            if (!MapObjectsCache.items.ContainsKey(i)) DownloadNewItem(i);
        }

        foreach (int i in MapObjectsCache.items.Keys) {
            if (!items.Contains(i)) MapObjectsCache.items.Remove(i);
        }
    }

    void DownloadNewItem(int id)
    {
        try
        {
            ObjectItem i = WebApiClient.SendAndDeserialize<ObjectItem>(EMethod.GET,
                "http://wear-h5.azurewebsites.net/api/object-items/" + id);
            MapObjectsCache.items.Add(id, i);
        }
        catch (Exception)
        {
            Debug.Log("Fail to load item " + id);
        }
        ;
    }

    void OnDisable() {
        Settings.SaveMapObjects();
    }
}
