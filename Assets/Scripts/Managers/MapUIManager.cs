using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System.Collections.Generic;

public class MapUIManager : MonoBehaviour {

   
    GameObject user;
    public GameObject ARButton;
    public GameObject buildingInfoPanel;
    MapNav map;
    public Text info;
    public Text loadInfo;
    public GameObject loadpanel;
    public GameObject uiPanel;
    [HideInInspector]
    public List<Item> itemsOnScene;
    public bool notificationsOn = false;
    public GameObject point;
    string text;
    public Text itemInfoName;
    public GameObject connectionInfoPanel;

    LocationManager locationManager;

    void Awake() {
        locationManager = GetComponent<LocationManager>();
//        StartCoroutine(CheckInternetConnection());
    }
    // Use this for initialization
    void Start () {      
        user = GameObject.Find("UserPoint");
        map = GameObject.Find("Map").GetComponent<MapNav>();
        itemsOnScene = new List<Item>();
    }
    
	// Update is called once per frame
	void Update () {
        connectionInfoPanel.SetActive(!ServerManager.instanse.hasInternetConnection);
        if (locationManager.hasAR) ARButton.SetActive(true);
        else ARButton.SetActive(false);
       
        loadInfo.text = map.status;
        info.text = "" + locationManager.nearItems.Count;
        if (map.ready&&loadpanel.activeSelf)
        {
            uiPanel.SetActive(true);
            loadpanel.SetActive(false);
        }

        if(map.mapping) info.text = "Updating map " + System.Math.Round(map.download * 100) + " %";
        else info.text = "" + locationManager.nearItems.Count;
        UserInput();
    }

    //IEnumerator CheckInternetConnection()
    //{
    //    while (true)
    //    {
    //        WWW www = new WWW("http://192.168.1.105/dashboard");

    //        float waitTime = 2;
    //        while (!www.isDone && waitTime > 0)
    //        {
    //            yield return new WaitForSeconds(1);
    //            waitTime--;
    //        }
    //        bool hasInternetConnection = false;
    //        if (!www.isDone | www.error != null)
    //        {
    //            hasInternetConnection = false;
    //        }
    //        else {
    //            hasInternetConnection = true;
    //        }
    //        connectionInfoPanel.SetActive(!hasInternetConnection);
    //        yield return new WaitForSeconds(1);
    //    }
    //}

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

    public void RescaleItems() {
        GameObject[] items = GameObject.FindGameObjectsWithTag("Building");
        foreach (GameObject g in items) {
            g.GetComponent<ItemScale>().Rescale(map.zoom);
        }
    }


    public void CreatePoint(Item it) {
        itemsOnScene.Add(it);
        GameObject currItem = Instantiate(point);
        SetGeolocation go = currItem.GetComponent<SetGeolocation>();
        go.lat = it.Lat;
        go.lon = it.Lon;
        currItem.name = it.Name;
        currItem.SetActive(true);
    }

    public void DestroyPoint(Item it) {
        itemsOnScene.Remove(it);
        GameObject currItem = GameObject.Find(it.Name);
        Destroy(currItem);
    }

    public void ChangeNotificationStatus() {
        notificationsOn = !notificationsOn;
        Application.runInBackground = notificationsOn;
//        locationManager.StartBackgroundService();
    }
}
