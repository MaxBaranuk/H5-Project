using System;
using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System.Collections.Generic;
using System.Xml;
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
//    public Dictionary<ObjectItem, OnlineMapsMarker3D> itemsOnScene;
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
    private MapObjectsManager mapObjectsManager;
    public delegate void MapZoomEvent(float zoom);
    public static event MapZoomEvent ZoomEvent;


    // route
    //GameObject routePoint;
    //private List<GameObject> routePoints;
    //private List<GameObject> activePoints;
    //private LineRenderer lineRenderer;

    void Awake() {
        activeToggles = new HashSet<EObjectItemType>();
//        itemsOnScene = new Dictionary<ObjectItem, OnlineMapsMarker3D>();
        activeToggles.Add(EObjectItemType.House);
        activeToggles.Add(EObjectItemType.Flat);
        activeToggles.Add(EObjectItemType.Area);
//        activeToggles.Add("Area");
        houseToggle.onValueChanged.AddListener((value) => {
            if (value) activeToggles.Add(EObjectItemType.House);
            else activeToggles.Remove(EObjectItemType.House);
            Debug.Log("Value change");
            UpdateItemsAfterTypeChange();
        });
        appartToggle.onValueChanged.AddListener((value) =>{
            if (value) activeToggles.Add(EObjectItemType.Flat);
            else activeToggles.Remove(EObjectItemType.Flat);
            Debug.Log("Value change");
            UpdateItemsAfterTypeChange();
         });
        
        officeToggle.onValueChanged.AddListener((value) => {
            if (value) activeToggles.Add(EObjectItemType.Area);
            else activeToggles.Remove(EObjectItemType.Area);
            Debug.Log("Value change");
            UpdateItemsAfterTypeChange();
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

    void Start ()
    {
        mapObjectsManager = GetComponent<MapObjectsManager>();

        // route
        //routePoint = (GameObject) Resources.Load("EmptyMapPoint");
        //routePoints = new List<GameObject>();
        //activePoints = new List<GameObject>();
        //lineRenderer = gameObject.AddComponent<LineRenderer>();
        //lineRenderer.material = (Material)Resources.Load("LineMaterial");
        //lineRenderer.SetWidth(30F, 30F);

        InvokeRepeating("UpdateConnection", 0, 1);
    }
    
	// Update is called once per frame
	void Update () {    
        UserInput();

     //   activePoints.Clear();

     //   for (int i = 0; i < routePoints.Count; i++)
	    //{
     //       if(routePoints[i].activeInHierarchy&& routePoints[i].transform.position.x!=0)
     //                  activePoints.Add(routePoints[i]);
     //   }

     //   lineRenderer.SetVertexCount(activePoints.Count);

     //   for (int i = 0; i < activePoints.Count; i++)
	    //{
     //       lineRenderer.SetPosition(i, activePoints[i].transform.position + Vector3.up * 3);
     //   }

    }

    void UpdateConnection()
    {
        connectionInfoPanel.SetActive(!ServerManager.Instanse().hasInternetConnection);
        bool disable = Input.location.lastData.longitude==0&&Input.location.lastData.latitude==0;
        GPSInfoPanel.SetActive(disable);
    }

    //public void FindRoute()
    //{
    //    StartCoroutine(MakeRoad());
    //}

    //IEnumerator MakeRoad()
    //{
    //    Vector2 from = new Vector2(50.4465500f, 30.4369700f);
    //    Vector2 to = new Vector2(50.2516000f, 28.7213100f);
    //    string fromPlaceID = "";
    //    string toPlaceID = "";

    //    OnlineMapsGoogleAPIQuery serv = OnlineMapsGoogleGetPlace.Find(from.x, from.y);
        
    //    serv.OnComplete = (resp) =>
    //    {
    //        XmlDocument xml = new XmlDocument();
    //        xml.InnerXml = resp;
    //        XmlNodeList elemList = xml.GetElementsByTagName("result");
    //        if (elemList.Count > 0)
    //        {
    //            XmlNode node = elemList.Item(0);
    //            fromPlaceID = node["place_id"].InnerText;
    //        }
    //    };
    //    yield return new WaitUntil(() => serv.response != null);

    //    OnlineMapsGoogleAPIQuery serv2 = OnlineMapsGoogleGetPlace.Find(to.x, to.y);
    //    serv2.OnComplete = (resp2) =>
    //    {
    //        XmlDocument xml2 = new XmlDocument();
    //        xml2.InnerXml = resp2;
    //        XmlNodeList elemList2 = xml2.GetElementsByTagName("result");
    //        if (elemList2.Count > 0)
    //        {
    //            XmlNode node = elemList2.Item(0);
    //            toPlaceID = node["place_id"].InnerText;
    //        }
    //    };
    //    yield return new WaitUntil(() => serv2.response != null);

    //    OnlineMapsGoogleAPIQuery serv3 = OnlineMapsFindDirection.Find("place_id:" + fromPlaceID, "place_id:" + toPlaceID);
    //    serv3.OnComplete = (resp3) =>
    //    {
    //        XmlDocument xml3 = new XmlDocument();
    //        xml3.InnerXml = resp3;
    //        XmlNodeList elemList3 = xml3.GetElementsByTagName("route");
    //        foreach (XmlNode node in elemList3)
    //        {
    //            Vector2[] linePoints;
    //            XmlNode n = node["overview_polyline"];

    //            string points = n["points"].InnerText;
    //            linePoints = OnlineMapsFindDirection.DecodePolylinePoints(points).ToArray();

    //            for (int i = 1; i < linePoints.Length; i++)
    //            {
    //                Vector2 step = (linePoints[i] - linePoints[i - 1])/10;
    //                for (int j = 0; j < 10; j++)
    //                {
    //                     routePoints.Add(mapObjectsManager.AddPointToMap(routePoint, linePoints[i - 1].y+step.y*j, linePoints[i - 1].x + step.x*j));
    //                }
    //            }
    //        }

    //    };
    //}

    public void CloseInfoBuildingPanel() {
        objectItemInfoPanel.SetActive(false);
    }

    public void OpenMainMenu()
    {
        mainMenuPanel.SetActive(true);
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
        yield return new WaitForSeconds(0.2f);
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

    //public void ChangeNotificationStatus() {
    //    notificationsOn = !notificationsOn;
    //    Application.runInBackground = notificationsOn;
    //}



    void UpdateItemsAfterTypeChange()
    {
        foreach (OnlineMapsMarker3D i in mapObjectsManager.itemsOnScene)
        {
            ObjectItem oi = (ObjectItem) i.customData;
            EObjectItemType t = (EObjectItemType) oi.Type;
            if (activeToggles.Contains(t)) i.enabled = true;
            else i.enabled = false;

        }


        //Dictionary<ObjectItem, OnlineMapsMarker3D>.KeyCollection keyColl = itemsOnScene.Keys;
        //IEnumerator<ObjectItem> ie = keyColl.GetEnumerator();
        //while (ie.MoveNext())
        //{
        //    EObjectItemType t = (EObjectItemType) ie.Current.Type;
        //    if (activeToggles.Contains(t)) itemsOnScene[ie.Current].enabled = true;
        //    else itemsOnScene[ie.Current].enabled = false;
        //}
    }
}
