using System;
using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System.Collections.Generic;
using WeAr.H5.Domain.Model;
using WeAr.H5.Domain.Model.Enums;
using Text = UnityEngine.UI.Text;

public class MapUIManager : MonoBehaviour {

    public GameObject menuPanel;
    public GameObject mainMenuPanel;
    public GameObject ARButton;
    public GameObject filterPanel;
    public GameObject objectItemInfoPanel;
    public GameObject filterButton;
    public Toggle houseToggle;
    public Toggle appartToggle;
    public Toggle officeToggle;
    public Toggle stockToggle;
    public Toggle hangarToggle;
    HashSet<EObjectItemType> activeToggles;
    [HideInInspector]
    public Dictionary<ObjectItem, GameObject> itemsOnScene;
    public GameObject connectionInfoPanel;
    public GameObject GPSInfoPanel;
    public Sprite[] filterButtonImages;
    //    public GameObject connectionInfoPanel;

    //    public GameObject buildingInfoPanel;
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

    void Awake() {
        activeToggles = new HashSet<EObjectItemType>();
        itemsOnScene = new Dictionary<ObjectItem, GameObject>();
        activeToggles.Add(EObjectItemType.House);
        activeToggles.Add(EObjectItemType.Flat);
        activeToggles.Add(EObjectItemType.Area);
//        activeToggles.Add("Area");
        houseToggle.onValueChanged.AddListener((value) => {
            if (value) activeToggles.Add(EObjectItemType.House);
            else activeToggles.Remove(EObjectItemType.House);
//            UpdateItemsAfterTypeChange();
        });
        appartToggle.onValueChanged.AddListener((value) =>{
            if (value) activeToggles.Add(EObjectItemType.Flat);
            else activeToggles.Remove(EObjectItemType.Flat);
//            UpdateItemsAfterTypeChange();
         });
        
        officeToggle.onValueChanged.AddListener((value) => {
            if (value) activeToggles.Add(EObjectItemType.Area);
            else activeToggles.Remove(EObjectItemType.Area);
 //           UpdateItemsAfterTypeChange();
        });
        //stockToggle.onValueChanged.AddListener((value) => {
        //    if (value) activeToggles.Add("Stock");
        //    else activeToggles.Remove("Stock");
        //    UpdateItemsAfterTypeChange();
        //});
        //hangarToggle.onValueChanged.AddListener((value) => {
        //    if (value) activeToggles.Add("Hangar");
        //    else activeToggles.Remove("Hangar");
        //    UpdateItemsAfterTypeChange();
        //});
    }

    void Start () {
 //       InvokeRepeating("UpdateConnection", 0, 1);
    }
    
	// Update is called once per frame
	void Update () {    
        UserInput();
    }

    void UpdateConnection()
    {
        connectionInfoPanel.SetActive(!ServerManager.instanse.hasInternetConnection);
        bool disable = Input.location.lastData.longitude==0&&Input.location.lastData.latitude==0;
        GPSInfoPanel.SetActive(disable);
    }

    public void CloseInfoBuildingPanel() {
        objectItemInfoPanel.SetActive(false);
    }

    public void OpenMainMenu()
    {
        filterPanel.SetActive(false);
        StartCoroutine(OpenWithShadow(menuPanel));
    }
    public void OpenARMode() {
        Settings.nextScene = Settings.SceneTypes.AR;
        SceneManager.LoadScene("loadingScene");
    }

    public void FilterPanel()
    {
        if (filterPanel.activeInHierarchy)
        {
            filterPanel.SetActive(false);
            filterButton.GetComponent<UnityEngine.UI.Image>().sprite = filterButtonImages[0];
        }
        else
        {
            filterPanel.SetActive(true);
            filterButton.GetComponent<UnityEngine.UI.Image>().sprite = filterButtonImages[1];
        }
    }

    IEnumerator OpenWithShadow(GameObject panel)
    {
        mainMenuPanel.GetComponent<Animator>().SetTrigger("OpenMenu");
        yield return new WaitForSeconds(0.3f);
        panel.SetActive(true);
    }

    void UserInput()
    {
#if UNITY_EDITOR

#else
        //if (Input.GetTouch(0).phase == TouchPhase.Began)
        //{
        //    Ray ray = Camera.main.ScreenPointToRay(Input.GetTouch(0).position);
        //    RaycastHit hit;

        //    if (Physics.Raycast(ray, out hit, 10000) && !UnityEngine.EventSystems.EventSystem.current.IsPointerOverGameObject())
        //    {
        //        if (hit.transform.gameObject.tag == "Building")
        //        {
        //            itemInfoName.text = hit.transform.gameObject.name;
        //            buildingInfoPanel.SetActive(true);
        //        }
        //    }
        //}
#endif
    }

    //public void CreatePoint(ObjectItem it) {
        
    //    GameObject currItem = Instantiate(point);
    //    itemsOnScene.Add(it, currItem);
    //    SetGeolocation go = currItem.GetComponent<SetGeolocation>();
    //    go.lat = it.Latitude;
    //    go.lon = it.Longitude;
    //    currItem.name = it.Name;
    //    currItem.SetActive(true);
    //}

    //public void DestroyPoint(ObjectItem it) {
    //    itemsOnScene.Remove(it);
    //    GameObject currItem = GameObject.Find(it.Name);
    //    Destroy(currItem);
    //}

    public void ChangeNotificationStatus() {
        notificationsOn = !notificationsOn;
        Application.runInBackground = notificationsOn;
    }

    

    //void UpdateItemsAfterTypeChange() {

    //    Dictionary<ObjectItem, GameObject>.KeyCollection keyColl = itemsOnScene.Keys;
    //    IEnumerator<ObjectItem> ie = keyColl.GetEnumerator();
    //    while (ie.MoveNext()) {

    //        if (activeToggles.Contains(ie.Current.Type)) itemsOnScene[ie.Current].SetActive(true);
    //        else itemsOnScene[ie.Current].SetActive(false);
    //    }
    //}
}
