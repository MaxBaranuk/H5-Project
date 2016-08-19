using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;

public class Map : MonoBehaviour {

    Transform user;
    Transform mymap;
    Renderer maprender;
    int screenX;
    int screenY;
    Camera mycam;
//    Transform cam;
    string[] maptype;
    public bool gpsFix;
//    float initPointerSize;
    public bool ready;
//    bool fixPointer;
    public string status;
    int maxWait = 30;
    int initTime = 3;
    LocationInfo loc;
    public Vector3 iniRef;
    public float fixLat = 50.473528f; 
    public float fixLon = 30.509415f;
    float updateRate = 0.1f;
    Vector3 currentUserPos;
    Vector3 newUserPos;
//    float heading;
    float download;
    WWW www;
//    float altitude;
 //   float accuracy;
    bool mapping = false;
//    int multiplier;
    string url = "";
    public string key = "Paste your Appkey here";
    public int zoom = 17;
    public int index;
    double tempLat;
    double tempLon;
    public int maxZoom = 25;
    public int minZoom = 15;
    private Queue<float> headers;
    public float camDist = 5.0f;
    public int camAngle = 40;

    //    public Text info;

    void Awake(){
        //        Input.gyro.enabled = true;
//        cam = Camera.main.transform;
        mycam = Camera.main.GetComponent<Camera>();
        user = GameObject.FindGameObjectWithTag("Player").transform;
        mymap = transform;
        maprender = GetComponent<Renderer>();
        screenX = Screen.width;
        screenY = Screen.height;
        if (screenY > screenX) mycam.fieldOfView = 72.5f;
        else mycam.fieldOfView = 95 - (28 * (screenX * 1.0f / screenY * 1.0f));
        maptype = new string[] { "map", "sat", "hyb" };
        headers = new Queue<float>();
    }
        // Use this for initialization
        void Start () {
        StartCoroutine(Initialization());
    }

    IEnumerator Initialization()
    {
        //Setting variables values on Start
        gpsFix = false;
//        rect = new Rect(screenX / 10, screenY / 10, 8 * screenX / 10, 8 * screenY / 10);
        mymap.eulerAngles = new Vector3(0, 180, 0);
//        initPointerSize = user.localScale.x;
        user.position = new Vector3(0, user.position.y, 0);
        //Rotate the camera on Start to avoid showing unwanted scene elements during initialization  (e.g.GUITexts)
//        cam.eulerAngles = new Vector3(270, 0, 0);
        //The "ready" variable will be true when the map texture has been successfully loaded.
        ready = false;
//        fixPointer = false;

        //STARTING LOCATION SERVICES
        // First, check if user has location service enabled
        if (!Input.location.isEnabledByUser)
        {
            //This message prints to the Editor Console
            print("Please enable location services and restart the App");
            //You can use this "status" variable to show messages in your custom user interface (GUIText, etc.)
            status = "Please enable location services\n and restart the App";
            yield return new WaitForSeconds(4);
            Application.Quit();
        }

        // Start service before querying location
        Input.location.Start(3, 3);
        Input.compass.enabled = true;
        print("Initializing Location Services..");
        status = "Initializing Location Services..";

        // Wait until service initializes
        while (Input.location.status == LocationServiceStatus.Initializing && maxWait > 0)
        {
            yield return new WaitForSeconds(1);
            maxWait--;
        }

        // Service didn't initialize in 30 seconds
        if (maxWait < 1)
        {
            print("Unable to initialize location services.\nPlease check your location settings and restart the App");
            status = "Unable to initialize location services.\nPlease check your location settings\n and restart the App";
            yield return new WaitForSeconds(4);
            Application.Quit();
        }

        // Connection has failed
        if (Input.location.status == LocationServiceStatus.Failed)
        {
            print("Unable to determine your location.\nPlease check your location setting and restart this App");
            status = "Unable to determine your location.\nPlease check your location settings\n and restart this App";
            yield return new WaitForSeconds(4);
            Application.Quit();
        }

        // Access granted and location value could be retrieved
        else {
            print("GPS Fix established. Setting position..");
            status = "GPS Fix established!\n Setting position ...";
            
                yield return new WaitForSeconds(initTime);
                //Set position
                loc = Input.location.lastData;
                iniRef.x = ((loc.longitude * 20037508.34f / 180) / 100);
                iniRef.z = (float)System.Math.Log(Mathf.Tan((90f + loc.latitude) * Mathf.PI / 360f)) / (Mathf.PI / 180f);
                iniRef.z = ((iniRef.z * 20037508.34f / 180) / 100);
                iniRef.y = 0;
                fixLon = loc.longitude;
                fixLat = loc.latitude;
                //Successful GPS fix
                gpsFix = true;
                //Update Map for the current location
                StartCoroutine(MapPosition());
        }

        //Rescale map, set new camera height, and resize user pointer according to new zoom level
        StartCoroutine(ReScale());
        InvokeRepeating("MyPosition", 1, updateRate);
        InvokeRepeating("Orientate", 1, 0.05f);
        InvokeRepeating("AccuracyAltitude", 1, 2);
    }

    // Update is called once per frame
    void Update () {
        currentUserPos.x = user.position.x;
        currentUserPos.x = Mathf.Lerp(user.position.x, newUserPos.x, 2.0f * Time.deltaTime);
        user.position = new Vector3(currentUserPos.x, user.position.y, user.position.z);

        currentUserPos.z = user.position.z;
        currentUserPos.z = Mathf.Lerp(user.position.z, newUserPos.z, 2.0f * Time.deltaTime);
        user.position = new Vector3(user.position.x, user.position.y, currentUserPos.z);

        float heading = 0;
        foreach (float f in headers) {
            heading += f;
        }
        heading /= headers.Count;

        if (System.Math.Abs(user.eulerAngles.y - heading) >= 5)
        {
            user.rotation = Quaternion.Slerp(user.transform.rotation, Quaternion.Euler(0, heading, 0), Time.time * 0.0005f);
        }
        //cam.parent = user;
        //if (ready)
        //    cam.LookAt(user);

        if (www != null) { download = www.progress; }
    }

    void Orientate(){
               float heading = Input.compass.trueHeading;
    //              float heading = Input.gyro.attitude.y* Mathf.Rad2Deg;

  //      info.text = "Compass: " + heading + "\nGyro: " + Input.gyro.attitude.y * Mathf.Rad2Deg;
        if (headers.Count > 5) headers.Dequeue();
        headers.Enqueue(heading); 
               
    }

    void MyPosition()
    {
        if (gpsFix)
        {
                loc = Input.location.lastData;
                newUserPos.x = ((loc.longitude * 20037508.34f / 180) / 100) - iniRef.x;
                newUserPos.z = Mathf.Log(Mathf.Tan((90 + loc.latitude) * Mathf.PI / 360)) / (Mathf.PI / 180);
                newUserPos.z = ((newUserPos.z * 20037508.34f / 180) / 100) - iniRef.z;
                fixLon = loc.longitude;
                fixLat = loc.latitude;
        }
    }

    void AccuracyAltitude()
    {
        if (gpsFix) {
 //           altitude = loc.altitude;
 //           accuracy = loc.horizontalAccuracy;
        }          
    }

    IEnumerator MapPosition()
    {

        //The mapping variable will only be true while the map is being updated
        mapping = true;

        //CHECK GPS STATUS AND RESTART IF NEEDED

        if (Input.location.status == LocationServiceStatus.Stopped || Input.location.status == LocationServiceStatus.Failed)
        {
            // Start service before querying location
            Input.location.Start(3, 3);

            // Wait until service initializes
            int maxWait = 20;
            while (Input.location.status == LocationServiceStatus.Initializing && maxWait > 0)
            {
                yield return new WaitForSeconds(1);
                maxWait--;
            }

            // Service didn't initialize in 20 seconds
            if (maxWait < 1)
            {
                print("Timed out");
                //use the status string variable to print messages to your own user interface (GUIText, etc.)
                status = "Timed out";
            }

            // Connection has failed
            if (Input.location.status == LocationServiceStatus.Failed)
            {
                print("Unable to determine device location");
                //use the status string variable to print messages to your own user interface (GUIText, etc.)
                status = "Unable to determine device location";
            }

        }

        //------------------------------------------------------------------	//

        www = null;
        //Get last available location data
        loc = Input.location.lastData;
        //Make player invisible while updating map
        user.gameObject.GetComponent<Renderer>().enabled = false;

        //GPS simulator enabled
       
            //Build a valid MapQuest OpenMaps tile request for the current location
 //           multiplier = 2;

            //ATENTTION: If you want to implement maps from a different tiles provider, modify the following url accordingly  to create a valid request
            url = "http://open.mapquestapi.com/staticmap/v4/getmap?key=" + key + "&size=1280,1280&zoom=" + zoom + "&type=" + maptype[index] + "&center=" + loc.latitude + "," + loc.longitude;

            //		url = "http://www.mapquestapi.com/directions/v2/route?key="+key+"&ambiguities=ignore&callback=handleRouteResponse&avoidTimedConditions=false&outFormat=json&routeType=fastest&enhancedNarrative=false&shapeFormat=raw&generalize=0&locale=en_US&unit=m&from="+loc.latitude+","+loc.longitude+"&to=York,PA";
            tempLat = loc.latitude;
            tempLon = loc.longitude;

        //Proceed with download if an Wireless internet connection is available 
        if (Application.internetReachability == NetworkReachability.ReachableViaLocalAreaNetwork)
        {
            StartCoroutine(Online());
        }
        //Proceed with download if a 3G/4G internet connection is available 
        else if (Application.internetReachability == NetworkReachability.ReachableViaCarrierDataNetwork)
        {
            StartCoroutine(Online());
        }
        //No internet connection is available. Switching to Offline mode.	 
        else {
            Offline();
        }
    }

    //ONLINE MAP DOWNLOAD
    IEnumerator Online()
    {

        // Start a download of the given URL
        www = new WWW(url);
        // Wait for download to complete
        download = (www.progress);
        while (!www.isDone)
        {
            print("Updating map " + System.Math.Round(download * 100) + " %");
            //use the status string variable to print messages to your own user interface (GUIText, etc.)
            status = "Updating map " + System.Math.Round(download * 100) + " %";
        }
        //Show download progress and apply texture
        if (www.error == null)
        {
            print("Updating map 100 %");
            print("Map Ready!");
            //use the status string variable to print messages to your own user interface (GUIText, etc.)
            status = "Updating map 100 %\nMap Ready!";
            yield return new WaitForSeconds(0.5f);
            maprender.material.mainTexture = null;
            Texture2D tmp;
            tmp = new Texture2D(1280, 1280, TextureFormat.RGB24, false);
            maprender.material.mainTexture = tmp;
            www.LoadImageIntoTexture(tmp);
        }
        //Download Error. Switching to offline mode
        else {
            print("Map Error:" + www.error);
            //use the status string variable to print messages to your own user interface (GUIText, etc.)
            status = "Map Error:" + www.error;
            yield return new WaitForSeconds(1);
            maprender.material.mainTexture = null;
            Offline();
        }
        maprender.enabled = true;
        ReSet();
        user.gameObject.GetComponent<Renderer>().enabled = true;
        ready = true;
        mapping = false;
    }

    void Offline()
    {
        maprender.material.mainTexture = Resources.Load("offline") as Texture2D;
        maprender.enabled = true;
        ReSet();
        ready = true;
        mapping = false;
        user.gameObject.GetComponent<Renderer>().enabled = true;
    }

    IEnumerator ReScale()
    {
        while (mapping)
        {
            yield return null;
        }
 //       mymap.localScale = new Vector3(multiplier * 100532.244f / (Mathf.Pow(2, zoom)), mymap.localScale.y, transform.localScale.x);
 //       user.localScale = new Vector3(initPointerSize * 65536 / (Mathf.Pow(2, zoom)), user.localScale.y, user.localScale.x);
        //if (fixPointer)
        //{
        //    user.localScale = new Vector3(initPointerSize * 65536 / (Mathf.Pow(2, zoom)), user.localScale.y, user.localScale.x);
        //}

        //3D View 
//        cam.localPosition = new Vector3(cam.localPosition.x, 65536 * camDist * Mathf.Sin(camAngle * Mathf.PI / 180) / Mathf.Pow(2, zoom), -(65536 * camDist * Mathf.Cos(camAngle * Mathf.PI / 180)) / Mathf.Pow(2, zoom));
 //       fixPointer = false;
    }

    void ReSet()
    {
        transform.position = new Vector3((((float)tempLon * 20037508.34f / 180) / 100) - iniRef.x, transform.position.y, Mathf.Log(Mathf.Tan((90 + (float)tempLat) * Mathf.PI / 360)) / (Mathf.PI / 180));
        transform.position = new Vector3(transform.position.x, transform.position.y, ((transform.position.z * 20037508.34f / 180) / 100) - iniRef.z);
    }


    public void ReloadMap() {
        StartCoroutine(MapPosition());
//        StartCoroutine(ReScale());
    }

    public void ZoomPlus() {
        if (zoom < maxZoom)
        {
            zoom = zoom + 1;
            StartCoroutine(MapPosition());
//            StartCoroutine(ReScale());
        }       
    }

    public void ZoomMinus() {
        if (zoom > minZoom)
        {
            zoom = zoom - 1;
            StartCoroutine(MapPosition());
 //           StartCoroutine(ReScale());
        }
    }
    //void OnGUI() {
    //    //Zoom Out button
    //    if (GUI.Button(new Rect(screenX / 5, 0, screenX / 5, screenY / 12), "zoom -"))
    //    {
    //        if (zoom > minZoom)
    //        {
    //            zoom = zoom - 1;
    //            MapPosition();
    //            ReScale();
    //        }
    //    }
    //    //Zoom In button
    //    if (GUI.Button(new Rect(2 * screenX / 5, 0, screenX / 5, screenY / 12), "zoom +"))
    //    {
    //        if (zoom < maxZoom)
    //        {
    //            zoom = zoom + 1;
    //            MapPosition();
    //            ReScale();
    //        }
    //    }
    //}
}
