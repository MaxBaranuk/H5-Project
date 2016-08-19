using UnityEngine;
using System.Collections;

public class MapNavC : MonoBehaviour {

public Transform user;                           //User(Player) transform
    public bool simGPS = false;                       //True when the GPS Emulator is enabled
    public float userSpeed = 5.0f;                   //User speed when using the GPS Emulator (keyboard input)
    public bool realSpeed = false;               //If true, the perceived player speed depends on zoom level(realistic behaviour)
    public float fixLat = 42.3627f;                      //Latitude
    public float fixLon = -71.05686f;                    //Longitude
    public float altitude;                           //Current GPS altitude
    public float heading;                            //Last compass sensor reading (Emulator disabled) or user's eulerAngles.y (Emulator enabled)
    public float accuracy;                           //GPS location accuracy (error)
    public int maxZoom = 18;                             //Maximum zoom level available. Set according to your maps provider
    public int minZoom = 1;                          //Minimum zoom level available
    public int zoom = 17;                                //Current zoom level
    public int multiplier;                           //1 for a size=640x640 tile, 2 for size=1280*1280 tile, etc.
    public string key = "Paste your Appkey here";       //AppKey (API key) code obtained from your maps provider (MapQuest, Google, etc.)
    public string[] maptype;                             //Array including available maptypes
    public int index;                                    //maptype array index. 
    public float camDist = 15.0f;                        //Camera distance(3D) or height(2D) to user
    public int camAngle = 40;                            //Camera angle from horizontal plane
    public int initTime = 3;                             //Hold time after a successful GPS fix in order to improve location accuracy
    public int maxWait = 30;                             //GPS fix timeout
    public bool buttons = true;                      //Enables GUI sample control buttons 
    public string dmsLat;                            //Latitude as degrees, minutes and seconds
    public string dmsLon;                            //Longitude as degrees, minutes and seconds
    public float updateRate = 0.1f;                  //User's position update rate
    public bool autoCenter = true;                   //Autocenter and refresh map
    public bool fixPointer = true;                   //Fix user's localScale whatever the zoom level is (2D mode only)
    public string status;                            //GPS and other status messages
    public bool gpsFix;                          //True after a successful GPS fix 
    public Vector3 iniRef;                           //First location data retrieved on Start	 
    public bool info;                                //Used by GPS-Status.js to enable/disable the GPS information window.
    public bool triDView = false;                    //2D/3D modes toggle
    public bool ready;							 //true when the map texture has been successfully loaded

float speed;
Transform cam;
Camera mycam;
float currentHeight;
LocationInfo loc;
Vector3 currentPosition;
Vector3 newUserPos; 
Vector3 currentUserPos;
float download;
WWW www;
string url = "";
double longitude;
double latitude;
Rect rect;
bool mapping = false;
int screenX;
int screenY;
Renderer maprender;
Transform mymap;
float initPointerSize;
double tempLat;
double tempLon;

    void Awake()
    {
        //Set the map's tag to GameController
        transform.tag = "GameController";

        //References to the Main Camera and Player. 
        //Please make sure your camera is tagged as "MainCamera" and your user visualization/character as "Player"
        cam = Camera.main.transform;
        mycam = Camera.main.GetComponent<Camera>();
        user = GameObject.FindGameObjectWithTag("Player").transform;

        //Store most used components and values into variables for faster access.
        mymap = transform;
        maprender = GetComponent<Renderer>();
        screenX = Screen.width;
        screenY = Screen.height;

        //Set the camera's field of view according to Screen size so map's visible area is maximized.
        if (screenY > screenX) mycam.fieldOfView = 72.5f;
        else  mycam.fieldOfView = 95 - (28 * (screenX * 1.0f / screenY * 1.0f));
        //Add possible values to maptype array. Change if using a maps provider other than MapQuest Open Static Maps.
        maptype = new string[] {"map", "sat", "hyb"};
    }
    // Use this for initialization
    void Start () {
        StartCoroutine(Initialization());
    }

    IEnumerator Initialization() {
        //Setting variables values on Start
        gpsFix = false;
        rect = new Rect(screenX / 10, screenY / 10, 8 * screenX / 10, 8 * screenY / 10);
        mymap.eulerAngles = new Vector3(0, 180, 0);
        initPointerSize = user.localScale.x;
        user.position = new Vector3(0, user.position.y, 0);
        //Rotate the camera on Start to avoid showing unwanted scene elements during initialization  (e.g.GUITexts)
        cam.eulerAngles = new Vector3(270, 0, 0);
        //The "ready" variable will be true when the map texture has been successfully loaded.
        ready = false;

        if (triDView)
            //Disable fixed size pointer on 3d view mode
            fixPointer = false;

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
            if (!simGPS)
            {
                //Wait in order to find enough satellites and increase GPS accuracy
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
            else {
                //Simulate initialization time
                yield return new WaitForSeconds(initTime);
                //Set Position
                iniRef.x = ((fixLon * 20037508.34f / 180) / 100);
                iniRef.z = Mathf.Log((float)Mathf.Tan((90 + fixLat) * Mathf.PI / 360)) / (Mathf.PI / 180);
                iniRef.z = ((iniRef.z * 20037508.34f / 180) / 100);
                iniRef.y = 0;
                //Simulated successful GPS fix
                gpsFix = true;
                //Update Map for the current location
               StartCoroutine(MapPosition());
            }
        }
        //Rescale map, set new camera height, and resize user pointer according to new zoom level
        StartCoroutine(ReScale());
        InvokeRepeating("MyPosition", 1, updateRate);
//        InvokeRepeating("Orientate", 1, 0.05f);
        InvokeRepeating("AccuracyAltitude", 1, 2);
        InvokeRepeating("Check", 1, 0.2f);
    }
	// Update is called once per frame
	void Update () {
        //User pointer speed
        if (realSpeed)
        {
            speed = userSpeed * 0.05f;
        }
        else {
            speed = speed = userSpeed * 10000 / (Mathf.Pow(2, zoom) * 1.0f);
        }

        //3D-2D View Camera Toggle 
        if (triDView)
        {
            cam.parent = user;
            if (ready)
                cam.LookAt(user);
        }
        else {
            cam.parent = null;
        }


        if (ready)
        {
            if (!simGPS)
            {
                //Smoothly move pointer to updated position and update rotation once the map has been successfully downloaded
                currentUserPos.x = user.position.x;
                currentUserPos.x = Mathf.Lerp(user.position.x, newUserPos.x, 2.0f * Time.deltaTime);
                user.position = new Vector3(currentUserPos.x, user.position.y, user.position.z);

                currentUserPos.z = user.position.z;
                currentUserPos.z = Mathf.Lerp(user.position.z, newUserPos.z, 2.0f * Time.deltaTime);
                user.position = new Vector3(user.position.x, user.position.y, currentUserPos.z);

                if (System.Math.Abs(user.eulerAngles.y - heading) >= 5)
                {
                    user.rotation = Quaternion.Slerp(user.transform.rotation, Quaternion.Euler(0, heading, 0), Time.time * 0.0005f);
                }
            }

            else {
                //When GPS Emulator is enabled, user position is controlled by keyboard input.
                if (mapping == false)
                {
                    //Use keyboard input to move the player
                    if (Input.GetKey("up") || Input.GetKey("w"))
                    {
                        user.transform.Translate(Vector3.forward * speed * Time.deltaTime);
                    }
                    if (Input.GetKey("down") || Input.GetKey("s"))
                    {
                        user.transform.Translate(-Vector3.forward * speed * Time.deltaTime);
                    }
                    //rotate pointer when pressing Left and Right arrow keys
                    user.Rotate(Vector3.up, Input.GetAxis("Horizontal") * 80 * Time.deltaTime);
                }
            }
        }

        if (mapping)
        {
            //get download progress while images are still downloading
            if (www != null) { download = www.progress; }
        }
    }

    void MyPosition()
    {
        if (gpsFix)
        {
            if (!simGPS)
            {
                loc = Input.location.lastData;
                newUserPos.x = ((loc.longitude * 20037508.34f / 180) / 100) - iniRef.x;
                newUserPos.z = Mathf.Log(Mathf.Tan((90 + loc.latitude) * Mathf.PI / 360)) / (Mathf.PI / 180);
                newUserPos.z = ((newUserPos.z * 20037508.34f / 180) / 100) - iniRef.z;
                fixLon = loc.longitude;
                fixLat = loc.latitude;
            }
            else {
                newUserPos.x = ((fixLon * 20037508.34f / 180) / 100) - iniRef.x;
                newUserPos.z = Mathf.Log(Mathf.Tan((90 + fixLat) * Mathf.PI / 360)) / (Mathf.PI / 180);
                newUserPos.z = ((newUserPos.z * 20037508.34f / 180) / 100) - iniRef.z;
                fixLon = (18000 * (user.position.x + iniRef.x)) / 20037508.34f;
                fixLat = ((360 / Mathf.PI) * Mathf.Atan(Mathf.Exp(0.00001567855943f * (user.position.z + iniRef.z)))) - 90;
            }
            dmsLat = convertdmsLat(fixLat);
            dmsLon = convertdmsLon(fixLon);
        }
    }

    void Orientate()
    {
        if (!simGPS && gpsFix)
        {
            heading = Input.compass.trueHeading;
        }
        else {
            heading = user.eulerAngles.y;
        }
    }

    void AccuracyAltitude()
    {
        if (gpsFix)
            altitude = loc.altitude;
        accuracy = loc.horizontalAccuracy;
    }

    void Check()
    {
        if (autoCenter && triDView == false)
        {
            if (ready == true && mapping == false && gpsFix)
            {
                if (rect.Contains(Vector2.Scale(mycam.WorldToViewportPoint(user.position), new Vector2(screenX, screenY))))
                {
                    //DoNothing
                }
                else {
                    StartCoroutine(MapPosition());
                }
            }
        }
    }

    void OnTriggerExit(Collider other)
    {
        if (other.tag == "Player" && autoCenter && triDView)
        {
            StartCoroutine(MapPosition());
           StartCoroutine(ReScale());
        }
    }

    //Update Map with the corresponding map images for the current location
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
            int maxWait  = 20;
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
        if (simGPS)
        {

            //Build a valid MapQuest OpenMaps tile request for the current location
            multiplier = 2; //Since default tile size is 1280x1280 (640*multiplier). Modify as needed.

            //ATENTTION: If you want to implement maps from a different tiles provider, modify the following url accordingly to create a valid request
            url = "http://open.mapquestapi.com/staticmap/v4/getmap?key=" + key + "&size=1280,1280&zoom=" + zoom + "&type=" + maptype[index] + "&center=" + fixLat + "," + fixLon;
            tempLat = fixLat;
            tempLon = fixLon;

        }

        //GPS simulator disabled
        else {
            //Build a valid MapQuest OpenMaps tile request for the current location
            multiplier = 2;

            //ATENTTION: If you want to implement maps from a different tiles provider, modify the following url accordingly  to create a valid request
            url = "http://open.mapquestapi.com/staticmap/v4/getmap?key=" + key + "&size=1280,1280&zoom=" + zoom + "&type=" + maptype[index] + "&center=" + loc.latitude + "," + loc.longitude;

            //		url = "http://www.mapquestapi.com/directions/v2/route?key="+key+"&ambiguities=ignore&callback=handleRouteResponse&avoidTimedConditions=false&outFormat=json&routeType=fastest&enhancedNarrative=false&shapeFormat=raw&generalize=0&locale=en_US&unit=m&from="+loc.latitude+","+loc.longitude+"&to=York,PA";
            tempLat = loc.latitude;
            tempLon = loc.longitude;
        }

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

    void ReSet()
    {
        transform.position = new Vector3((((float)tempLon * 20037508.34f / 180) / 100) - iniRef.x, transform.position.y, Mathf.Log(Mathf.Tan((90 + (float) tempLat) * Mathf.PI / 360)) / (Mathf.PI / 180));
        transform.position = new Vector3(transform.position.x, transform.position.y, ((transform.position.z * 20037508.34f / 180) / 100) - iniRef.z);
        cam.position = new Vector3((((float)tempLon * 20037508.34f / 180) / 100) - iniRef.x, cam.position.y, Mathf.Log(Mathf.Tan((90 + (float)tempLat) * Mathf.PI / 360)) / (Mathf.PI / 180));
        cam.position = new Vector3(cam.position.x, cam.position.y, ((cam.position.z * 20037508.34f / 180) / 100) - iniRef.z);
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
            yield return new WaitForSeconds (0.5f);
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
        mymap.localScale = new Vector3(multiplier * 100532.244f / (Mathf.Pow(2, zoom)), mymap.localScale.y, transform.localScale.x);

        if (fixPointer)
        {
            user.localScale = new Vector3(initPointerSize * 65536 / (Mathf.Pow(2, zoom)), user.localScale.y, user.localScale.x);
        }

        //3D View 
        if (triDView)
        {
            fixPointer = false;
            cam.localPosition = new Vector3(cam.localPosition.x, 65536 * camDist * Mathf.Sin(camAngle * Mathf.PI / 180) / Mathf.Pow(2, zoom), -(65536 * camDist * Mathf.Cos(camAngle * Mathf.PI / 180)) / Mathf.Pow(2, zoom));
        }
        //2D View 
        else {
            cam.localEulerAngles = new Vector3(90, 0, 0);
            cam.position = new Vector3(cam.position.x, (65536 * camDist) / (Mathf.Pow(2, zoom)), user.position.z);
            //Correct the camera's near and far clipping distances according to its new height.
            //Introduced to avoid the player and plane not being rendered under some circunstances.
            mycam.nearClipPlane = cam.position.y / 10;
            mycam.farClipPlane = cam.position.y + 1;
            //Small correction to the user's height according to zoom level to avoid similar camera issues.
            user.position = new Vector3(user.position.x, 10 * Mathf.Exp(-zoom) + 0.01f, user.position.z);
        }
    }

    public void ReloadMap()
    {
       StartCoroutine(MapPosition());
    }

    //Translate decimal latitude to Degrees Minutes and Seconds
    string convertdmsLat(float lat){
	var latAbs = Mathf.Abs(Mathf.Round(lat * 1000000));
    string result;
    result = (Mathf.Floor(latAbs / 1000000) + "В° "
    		 + Mathf.Floor(((latAbs/1000000) - Mathf.Floor(latAbs/1000000)) * 60)  + "\' "
    	     + (Mathf.Floor(((((latAbs/1000000) - Mathf.Floor(latAbs/1000000)) * 60) - Mathf.Floor(((latAbs/1000000) - Mathf.Floor(latAbs/1000000)) * 60)) * 100000) *60/100000 ).ToString("F2") + "\" ")+ ((lat > 0) ? "N" : "S");
	return result;
}
    //Translate decimal longitude to Degrees Minutes and Seconds  
    string convertdmsLon(float lon){
	var lonAbs = Mathf.Abs(Mathf.Round(lon * 1000000));
string result; 
    result = (Mathf.Floor(lonAbs / 1000000) + "В° " 
      		 + Mathf.Floor(((lonAbs/1000000) - Mathf.Floor(lonAbs/1000000)) * 60)  + "\' " 
      		 + (Mathf.Floor(((((lonAbs/1000000) - Mathf.Floor(lonAbs/1000000)) * 60) - Mathf.Floor(((lonAbs/1000000) - Mathf.Floor(lonAbs/1000000)) * 60)) * 100000) *60/100000 ).ToString("F2") + "\" " + ((lon > 0) ? "E" : "W") );
	return result;
}

    void OnGUI()
    {

        if (ready && !mapping && buttons)
        {
            GUI.BeginGroup(new Rect(0, screenY - screenY / 12, screenX, screenY / 12));

            GUI.Box(new Rect(0, 0, screenX, screenY / 12), "");
            //Map type toggle button
            if (GUI.Button(new Rect(0, 0, screenX / 5, screenY / 12), maptype[index]))
            {
                if (mapping == false)
                {
                    if (index < maptype.Length - 1)
                        index = index + 1;
                    else
                        index = 0;
                    StartCoroutine(MapPosition());
                    StartCoroutine(ReScale());
                }
            }
            //Zoom Out button
            if (GUI.Button(new Rect(screenX / 5, 0, screenX / 5, screenY / 12), "zoom -"))
            {
                if (zoom > minZoom)
                {
                    zoom = zoom - 1;
                    StartCoroutine(MapPosition());
                    StartCoroutine(ReScale());
                }
            }
            //Zoom In button
            if (GUI.Button(new Rect(2 * screenX / 5, 0, screenX / 5, screenY / 12), "zoom +"))
            {
                if (zoom < maxZoom)
                {
                    zoom = zoom + 1;
                    StartCoroutine(MapPosition());
                    StartCoroutine(ReScale());
                }
            }
            //Update map and center user position 
            if (GUI.Button(new Rect(3 * screenX / 5, 0, screenX / 5, screenY / 12), "refresh"))
            {
                ;
                MapPosition();
                ReScale();
            }
            //Show GPS Status info. Please make sure the GPS-Status.js script is attached and enabled in the map object.
            if (GUI.Button(new Rect(4 * screenX / 5, 0, screenX / 5, screenY / 12), "info"))
            {
                if (info)
                    info = false;
                else
                    info = true;
            }
            GUI.EndGroup();
        }
    }
}
