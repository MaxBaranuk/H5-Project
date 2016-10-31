using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;
using System.Runtime.InteropServices;
using WeAr.H5.Domain.Model;
using WeAr.H5.WebAPI.Client;
using UnityEngine.UI;

namespace UnityEngine.UI
{
    public class MapObjectsManager : MonoBehaviour {

    public GameObject[] itemPrefabs;

    OnlineMapsTileSetControl mapControl; 
    OnlineMapsMarker3D userPoint;
    GameObject objectItemInfoPanel;
    Text infoTitle;
    Text infoDescription;
    Image itemImage;
    public ObjectItem[] items;
    public List<OnlineMapsMarker3D> itemsOnScene;

        void OnEnable()
    {
        Input.location.Start();
    }

    // Use this for initialization
    void Start () {
        mapControl = GameObject.Find("Map").GetComponent<OnlineMapsTileSetControl>();

        objectItemInfoPanel =
            GameObject.Find("Canvas")
                .transform.FindChild("ObjectPanel").gameObject;
        infoTitle =
            objectItemInfoPanel.transform
                .FindChild("TitlePanel")
                .FindChild("Title")
                .GetComponent<Text>();

        infoDescription =
             objectItemInfoPanel.transform
                .FindChild("AboutPanel")
                .FindChild("ViewPort")
                .FindChild("Panel")
                .FindChild("Text")
                .GetComponent<Text>();

        itemImage =
              objectItemInfoPanel.transform
                .FindChild("GalleryPanel")
                .FindChild("GalleryPanel")
                .FindChild("Image")
                .GetComponent<Image>();
        items = DownloadItems();
        AddItemsToMap(items); 
        AddUserPoint();
    }

    //public GameObject AddPointToMap(GameObject prefab, float lat, float lon)
    //{
    //    OnlineMapsMarker3D inst = mapControl.AddMarker3D(new Vector2(lon, lat), prefab);
    //    inst.Init(mapControl.transform);
    //    return inst.instance;
    //}

    void AddUserPoint()
    {
        userPoint = mapControl.AddMarker3D(new Vector2(Input.location.lastData.longitude, Input.location.lastData.latitude), itemPrefabs[1]);
        userPoint.Init(mapControl.transform);
    }

    void Update () {
        userPoint.SetPosition(Input.location.lastData.longitude, Input.location.lastData.latitude);
    }

    ObjectItem [] DownloadItems()
    {
        ObjectItem[] items = WebApiClient.SendAndDeserialize<ObjectItem[]>(EMethod.GET,
                "http://wear-h5.azurewebsites.net/api/object-items");
        return items;
    }

    void AddItemsToMap(ObjectItem[] items)
    {
        foreach (ObjectItem it in items)
        {
            OnlineMapsMarker3D inst = mapControl.AddMarker3D(new Vector2(it.Longitude, it.Latitude), itemPrefabs[0]);
            inst.customData = it;
            inst.label = it.Name;

            Action<OnlineMapsMarkerBase> action = delegate (OnlineMapsMarkerBase marker)
            {
                ObjectItem item = (ObjectItem)marker.customData;
                infoTitle.text = item.Name;
                infoDescription.text = item.Description;
                objectItemInfoPanel.SetActive(true);


                ObjectItem itemDetail =  WebApiClient.SendAndDeserialize<ObjectItem>(EMethod.GET,
               "http://wear-h5.azurewebsites.net/api/object-items/photo/"+item.Id);
                Texture2D text = new Texture2D(1, 1, TextureFormat.ARGB32, false);
                text.LoadImage(itemDetail.Photo);

                Sprite sprite = Sprite.Create(text, new Rect(0, 0, text.width, text.height), new Vector2(.5f, .5f));
                itemImage.sprite = sprite;

            };
            inst.OnClick = action;
            inst.Init(mapControl.transform);
            itemsOnScene.Add(inst);
        }
    }

    void OnDisable() {
        Input.location.Stop();
    }

}
}
