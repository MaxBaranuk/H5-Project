using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System.Collections.Generic;

public class MapManager : MonoBehaviour {

    LocationInfo lastLocation;
    LocationInfo currLocation;
    GameObject user;
    public GameObject ARButton;
    public GameObject buildingInfoPanel;
    MapNav map;
    public Text info;
    public Text loadInfo;
    public GameObject loadpanel;
    public GameObject uiPanel;
    ItemsCollection colletion;
    List<Item> itemsOnScene;
    public GameObject point;
    string text;
    public Text itemInfoName;

    // Use this for initialization
    void Start () {
        currLocation = Input.location.lastData;
        user = GameObject.Find("UserPoint");
        map = GameObject.Find("Map").GetComponent<MapNav>();
        InvokeRepeating("CheckForARObjects", 1, 1);
        lastLocation = Input.location.lastData;
        colletion = ItemsCollection.Load("Places");
        itemsOnScene = new List<Item>();
        InvokeRepeating("UpdateItems", 1, 1);
//        buildings = new List<GameObject>();
    }
    
	// Update is called once per frame
	void Update () {
        currLocation = Input.location.lastData; 
        GameObject[] items = GameObject.FindGameObjectsWithTag("Building");
        text = "user - " + user.transform.position + "\n";
        foreach (GameObject i in items)
        {
            text += i.name + " - " + i.transform.position+ "\n";
        }
        info.text = text;
        loadInfo.text = map.status;
        //if (Mathf.Abs(currLocation.latitude - lastLocation.latitude) > 0.001f |
        //      Mathf.Abs(currLocation.longitude - lastLocation.longitude) > 0.001f) ReloadMap();

        if (map.ready&&loadpanel.activeSelf)
        {
            uiPanel.SetActive(true);
            loadpanel.SetActive(false);
        }
        UserInput();
    }

    void CheckForARObjects() {
        bool hasAR = false;
        foreach (Item g in colletion.items)
        {

            if (getDistanceFromLatLonInKm(currLocation.latitude, currLocation.longitude, g.Lat, g.Lon) < 0.2f) hasAR = true;
        }
        if(hasAR) ARButton.SetActive(true);
        else ARButton.SetActive(false);
    }

    void ReloadMap() {
        map.ReloadMap();
        lastLocation = currLocation;
    }

   void MakeRoute() {

    }

    public void CloseInfoBuildingPanel() {
        buildingInfoPanel.SetActive(false);
    }

    public void OpenARMode() {
        SceneManager.LoadScene("ARMode");
    }

    void UserInput()
    {
        if (Input.GetTouch(0).phase == TouchPhase.Began)
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.GetTouch(0).position);
            RaycastHit hit;

            if (Physics.Raycast(ray, out hit, 10000) && !UnityEngine.EventSystems.EventSystem.current.IsPointerOverGameObject())
            {
                if (hit.transform.gameObject.tag == "Building")
                {
                    itemInfoName.text = hit.transform.gameObject.name;
                    buildingInfoPanel.SetActive(true);
                }
            }
        }
    }

    void UpdateItems() {
        foreach (Item it in colletion.items) {
            float dist = getDistanceFromLatLonInKm(currLocation.latitude, currLocation.longitude, it.Lat, it.Lon);

            if ( dist < 2 && !itemsOnScene.Contains(it)) {
                itemsOnScene.Add(it);
                GameObject currItem = Instantiate(point);
                SetGeolocation go = currItem.GetComponent<SetGeolocation>();
                go.lat = it.Lat;
                go.lon = it.Lon;
                currItem.name = it.Name;
                currItem.SetActive(true);               
            }
            if (dist > 2 && itemsOnScene.Contains(it)) {
                itemsOnScene.Remove(it);
                GameObject currItem = GameObject.Find(it.Name);
                Destroy(currItem);
            }
        }
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
