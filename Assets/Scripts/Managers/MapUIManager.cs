using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System.Collections.Generic;

public class MapUIManager : MonoBehaviour {

   
//    GameObject user;
    public GameObject ARButton;
    public GameObject buildingInfoPanel;
    MapNav map;
    public Text info;
    public Text loadInfo;
    public GameObject loadpanel;
    public GameObject uiPanel;
    [HideInInspector]
//    public List<Item> itemsOnScene;
    public Dictionary<Item, GameObject> itemsOnScene;
    public bool notificationsOn = false;
    public GameObject point;
    string text;
    public Text itemInfoName;
    public GameObject connectionInfoPanel;
    public delegate void MapZoomEvent(float zoom);
    public static event MapZoomEvent ZoomEvent;
    public GameObject checkboxPanel;
    public GameObject checkboxButton;
    public Toggle appartToggle;
    public Toggle houseToggle;
    public Toggle officeToggle;
    public Toggle areaToggle;
    HashSet<string> activeToggles;

    LocationManager locationManager;

    void Awake() {
        locationManager = GetComponent<LocationManager>();
        activeToggles = new HashSet<string>();
        itemsOnScene = new Dictionary<Item, GameObject>();
        activeToggles.Add("Appartment");
        activeToggles.Add("House");
        activeToggles.Add("Office");
        activeToggles.Add("Area");
        appartToggle.onValueChanged.AddListener((value) =>{
            if (value) activeToggles.Add("Appartment");
            else activeToggles.Remove("Appartment");
            UpdateItemsAfterTypeChange();
         });
        houseToggle.onValueChanged.AddListener((value) => {
            if (value) activeToggles.Add("House");
            else activeToggles.Remove("House");
            UpdateItemsAfterTypeChange();
        });
        officeToggle.onValueChanged.AddListener((value) => {
            if (value) activeToggles.Add("Office");
            else activeToggles.Remove("Office");
            UpdateItemsAfterTypeChange();
        });
        areaToggle.onValueChanged.AddListener((value) => {
            if (value) activeToggles.Add("Area");
            else activeToggles.Remove("Area");
            UpdateItemsAfterTypeChange();
        });
        //        StartCoroutine(CheckInternetConnection());
    }
    // Use this for initialization
    void Start () {      
 //       user = GameObject.Find("UserPoint");
        map = GameObject.Find("Map").GetComponent<MapNav>();

    }
    
	// Update is called once per frame
	void Update () {
        connectionInfoPanel.SetActive(!ServerManager.instanse.hasInternetConnection);
        if (locationManager.hasAR) ARButton.SetActive(true);
        else ARButton.SetActive(false);
       
        loadInfo.text = map.status;
//        info.text = "" + locationManager.nearItems.Count;
        if (map.ready&&loadpanel.activeSelf)
        {
            uiPanel.SetActive(true);
            loadpanel.SetActive(false);
        }

        if(map.mapping) info.text = "Updating map " + System.Math.Round(map.download * 100) + " %";
        else info.text = "" + locationManager.colletion.items.Length;

        UserInput();
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

    public void RescaleItems() {
        ZoomEvent(map.zoom);
        //GameObject[] items = GameObject.FindGameObjectsWithTag("Building");
        //foreach (GameObject g in items) {
        //    g.GetComponent<ItemScale>().Rescale(map.zoom);
        //}
    }


    public void CreatePoint(Item it) {
        
        GameObject currItem = Instantiate(point);
        itemsOnScene.Add(it, currItem);
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

    public void CheckboxButtonclick() {
        if (checkboxPanel.activeInHierarchy) checkboxPanel.SetActive(false);
        else checkboxPanel.SetActive(true);
//        checkboxPanel.SetActive(!checkboxPanel.activeInHierarchy);
    }

    void UpdateItemsAfterTypeChange() {

        Dictionary<Item, GameObject>.KeyCollection keyColl = itemsOnScene.Keys;
        IEnumerator<Item> ie = keyColl.GetEnumerator();
        while (ie.MoveNext()) {

            if (activeToggles.Contains(ie.Current.type)) itemsOnScene[ie.Current].SetActive(true);
            else itemsOnScene[ie.Current].SetActive(false);
        }
    }
}
