﻿using UnityEngine;
using System.Collections.Generic;
using System.Collections;
using AssetBundles;
using System;
using UnityEngine.UI;

public class ServerManager : MonoBehaviour {

    public static ServerManager instanse;
    public const string AssetBundlesOutputPath = "/AssetBundles/";
    public string assetBundleName = "objects";
    public string assetName = "customTarget";
    GameObject currObj;
    public string status;
    public bool hasInternetConnection = true;
//    string key = "5sdGejj8uo74NIbkSNPa9SfoeWioLq1K";
    //public int zoom = 17;
//    LocationInfo loc;
//    public Texture2D currMap;

    void Awake() {
        StartCoroutine(CheckInternetConnection());
    }

    void Start () {
        if (instanse == null) {
            instanse = this;
            DontDestroyOnLoad(instanse);
            Input.location.Start(3, 3);
//            loc = Input.location.lastData;
        } 
    }

	void Update () {
	
	}

//    public void UpdateMapImage() {
//        StartCoroutine(GetMapImage());
//    }

//    IEnumerator GetMapImage() {
//        while (true) {
//            string url = "http://open.mapquestapi.com/staticmap/v4/getmap?key=" + key + "&size=1280,1280&zoom=" + zoom + "&type=map&center=" + loc.latitude + "," + loc.longitude;
//            WWW www = new WWW(url);
//            // Wait for download to complete
//            float download = (www.progress);

//            yield return new WaitUntil(() => !www.isDone);
           
//            //Show download progress and apply texture
//            if (www.error == null)
//            {
//                //use the status string variable to print messages to your own user interface (GUIText, etc.)
////                status = "Updating map 100 %\nMap Ready!";
//                yield return new WaitForSeconds(0.5f);
//                currMap = null;
//                Texture2D tmp;
//                tmp = new Texture2D(1280, 1280, TextureFormat.RGB24, false);
//                currMap = tmp;
//                www.LoadImageIntoTexture(tmp);
//            }
//            //Download Error. Switching to offline mode
//            else {
//                //use the status string variable to print messages to your own user interface (GUIText, etc.)
//                status = "Map Error:" + www.error;
//                yield return new WaitForSeconds(1);
//                currMap = Resources.Load("offline") as Texture2D;
//            }
//            yield return new WaitForSeconds(30);
//        }
//    }

    //public List<Item> getObjectsList(Vector2 location) {
    //    List<Item> obj = new List<Item>();     
    //    return obj;
    //}

    public Agent getAgentInfo(string agentID)
    {
        Agent ag = new Agent();
        return ag;
    }

    public IEnumerator getObjectByTargetID(string targetID) {

        assetName = targetID;
        yield return StartCoroutine(LoadAsset());
    }

    IEnumerator CheckInternetConnection()
    {
        while (true)
        {
 //           WWW www = new WWW("http://192.168.1.105/dashboard");
            WWW www = new WWW("http://www.this-page-intentionally-left-blank.org/");
            
            float waitTime = 2;
            while (!www.isDone && waitTime > 0)
            {
                yield return new WaitForSeconds(0.1f);
                waitTime-=0.1f;
            }
            
            if (!www.isDone | www.error != null)
            {
                hasInternetConnection = false;
            }
            else {
                hasInternetConnection = true;
            }
//            connectionInfoPanel.SetActive(!hasInternetConnection);
            yield return new WaitForSeconds(1);
        }
    }

    IEnumerator LoadAsset()
    {
        yield return StartCoroutine(Initialize());
        // Load asset.
        yield return StartCoroutine(InstantiateGameObjectAsync(assetBundleName, assetName));
//        status += "finish\n";
    }

    protected IEnumerator Initialize()
    {
        // Don't destroy this gameObject as we depend on it to run the loading script.
        DontDestroyOnLoad(gameObject);

        // With this code, when in-editor or using a development builds: Always use the AssetBundle Server
        // (This is very dependent on the production workflow of the project. 
        // 	Another approach would be to make this configurable in the standalone player.)
#if DEVELOPMENT_BUILD || UNITY_EDITOR
//        AssetBundleManager.SetDevelopmentAssetBundleServer();
        AssetBundleManager.SetSourceAssetBundleURL("http://192.168.1.105/assets/");
#else
		// Use the following code if AssetBundles are embedded in the project for example via StreamingAssets folder etc:
		AssetBundleManager.SetSourceAssetBundleURL(Application.dataPath + "/");
		// Or customize the URL based on your deployment or configuration
		AssetBundleManager.SetSourceAssetBundleURL("http://192.168.1.105/assets/");
#endif

        // Initialize AssetBundleManifest which loads the AssetBundleManifest object.
        var request = AssetBundleManager.Initialize();
        if (request != null) {
            yield return StartCoroutine(request);
            status += "request ok\n";
        }
        else {
            status += "request null\n";
        }
    }

    protected IEnumerator InstantiateGameObjectAsync(string assetBundleName, string assetName)
    {
        // This is simply to get the elapsed time for this phase of AssetLoading.
        float startTime = Time.realtimeSinceStartup;
//        status += "time " + startTime+"\n";
        // Load asset from assetBundle.
        AssetBundleLoadAssetOperation request = AssetBundleManager.LoadAssetAsync(assetBundleName, assetName, typeof(GameObject));
        if (request == null)
        {
            status += "request null\n";
            yield break;
        }
        else {
            status += "request ok\n";
        }   
        yield return StartCoroutine(request);

        // Get the asset.
        GameObject prefab = request.GetAsset<GameObject>();
        if (prefab != null) {
            currObj = Instantiate(prefab);
            GameObject target = GameObject.Find("ImageTarget");
            target.GetComponent<CloudRecoTrackableEventHandler>().currObject = currObj;
            currObj.transform.parent = target.transform;
            status += "obj created\n";
        }       
        else status += "prefab null\n";
        // Calculate and display the elapsed time.
        float elapsedTime = Time.realtimeSinceStartup - startTime;
        status += ""+ assetName + (prefab == null ? " was not" : " was") + " loaded successfully in " + elapsedTime + " seconds\n";
        Debug.Log(assetName + (prefab == null ? " was not" : " was") + " loaded successfully in " + elapsedTime + " seconds");
        //status += "" + currObj.transform.position+"\n";
        //status += "" + currObj.transform.localScale + "\n";
    }
}

