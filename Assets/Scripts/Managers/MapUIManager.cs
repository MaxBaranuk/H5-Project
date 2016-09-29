using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System.Collections.Generic;

public class MapUIManager : MonoBehaviour {

    public GameObject menuPanel;
    public GameObject mainMenuPanel;
    public GameObject ARButton;
    public GameObject filterPanel;
    public Button filterButton;
    public Toggle houseToggle;
    public Toggle appartToggle;
    public Toggle officeToggle;
    public Toggle stockToggle;
    public Toggle hangarToggle;
    HashSet<string> activeToggles;
    [HideInInspector]
    public Dictionary<Item, GameObject> itemsOnScene;
    public GameObject connectionInfoPanel;
    //    GameObject user;

    public GameObject buildingInfoPanel;
//    MapNav map;
    public Text info;
    public Text loadInfo;
    public GameObject loadpanel;
    public GameObject uiPanel;   
    public bool notificationsOn = false;
    public GameObject point;
    string text;
    public Text itemInfoName;
    
    public delegate void MapZoomEvent(float zoom);
    public static event MapZoomEvent ZoomEvent;
    

//    LocationManager locationManager;

    void Awake() {
        activeToggles = new HashSet<string>();
        itemsOnScene = new Dictionary<Item, GameObject>();
        activeToggles.Add("Appartment");
        activeToggles.Add("House");
        activeToggles.Add("Office");
        activeToggles.Add("Area");
        houseToggle.onValueChanged.AddListener((value) => {
            if (value) activeToggles.Add("House");
            else activeToggles.Remove("House");
            UpdateItemsAfterTypeChange();
        });
        appartToggle.onValueChanged.AddListener((value) =>{
            if (value) activeToggles.Add("Appartment");
            else activeToggles.Remove("Appartment");
            UpdateItemsAfterTypeChange();
         });
        
        officeToggle.onValueChanged.AddListener((value) => {
            if (value) activeToggles.Add("Office");
            else activeToggles.Remove("Office");
            UpdateItemsAfterTypeChange();
        });
        stockToggle.onValueChanged.AddListener((value) => {
            if (value) activeToggles.Add("Stock");
            else activeToggles.Remove("Stock");
            UpdateItemsAfterTypeChange();
        });
        hangarToggle.onValueChanged.AddListener((value) => {
            if (value) activeToggles.Add("Hangar");
            else activeToggles.Remove("Hangar");
            UpdateItemsAfterTypeChange();
        });
    }

    void Start () {      
//        map = GameObject.Find("Map").GetComponent<MapNav>();
    }
    
	// Update is called once per frame
	void Update () {
        connectionInfoPanel.SetActive(!ServerManager.instanse.hasInternetConnection);
//        if (locationManager.hasAR) ARButton.SetActive(true);
//        else ARButton.SetActive(false);
       
//        loadInfo.text = map.status;
        //if (map.ready&&loadpanel.activeSelf)
        //{
        //    uiPanel.SetActive(true);
        //    loadpanel.SetActive(false);
        //}

//        if(map.mapping) info.text = "Updating map " + System.Math.Round(map.download * 100) + " %";
//        else info.text = "" + locationManager.colletion.items.Length;

        UserInput();
    }

    public void CloseInfoBuildingPanel() {
        buildingInfoPanel.SetActive(false);
    }

    public void OpenMainMenu() {
        mainMenuPanel.SetActive(true);
        menuPanel.SetActive(true);
        
    }
    public void OpenARMode() {
        SceneManager.LoadScene("ARMode");
    }

    public void OpenFilterPanel()
    {
        filterButton.interactable = false;
        filterPanel.SetActive(true);
    }

    public void CloseFilterPanel() {
        filterButton.interactable = true;
        filterPanel.SetActive(false);
    }

    void UserInput()
    {
#if UNITY_EDITOR

#else
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
#endif
    }

    //public void RescaleItems() {
    //    ZoomEvent(map.zoom);
    //    //GameObject[] items = GameObject.FindGameObjectsWithTag("Building");
    //    //foreach (GameObject g in items) {
    //    //    g.GetComponent<ItemScale>().Rescale(map.zoom);
    //    //}
    //}


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

    

    void UpdateItemsAfterTypeChange() {

        Dictionary<Item, GameObject>.KeyCollection keyColl = itemsOnScene.Keys;
        IEnumerator<Item> ie = keyColl.GetEnumerator();
        while (ie.MoveNext()) {

            if (activeToggles.Contains(ie.Current.type)) itemsOnScene[ie.Current].SetActive(true);
            else itemsOnScene[ie.Current].SetActive(false);
        }
    }
}
