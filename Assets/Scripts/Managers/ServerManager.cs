using UnityEngine;
using System.Collections.Generic;
using System.Collections;
using AssetBundles;
using System;
using UnityEngine.UI;
using WeAr.H5.Domain.Model;
using WeAr.H5.Domain.Model.Enums;
using WeAr.H5.WebAPI.Client;

public class ServerManager : MonoBehaviour {

    static ServerManager instanse;
    public const string AssetBundlesOutputPath = "/AssetBundles/";
    public string assetBundleName = "objects";
    public string assetName = "customTarget";
    GameObject currObj;
    public string status;
    public bool hasInternetConnection = true;
//    GameObject target;

    AndroidJavaClass unityClass;
    AndroidJavaObject unityActivity;
    AndroidJavaClass customClass;

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

#if UNITY_ANDROID
        unityClass = new AndroidJavaClass("com.unity3d.player.UnityPlayer");
        unityActivity = unityClass.GetStatic<AndroidJavaObject>("currentActivity");
        customClass = new AndroidJavaClass("com.wear.locationservice.UnityLocationService");
#endif

    }

    public static ServerManager Instanse() {

        if (instanse == null) {
            GameObject g = Instantiate(Resources.Load<GameObject>("ServerManager"));
            instanse = g.GetComponent<ServerManager>();
        }
        return instanse;
    }

    void Update () {
	
	}


    public void OperateService(bool isStart)
    {
        if (isStart) StartAndroidService();
        else StopAndroidService();
    }

    void StartAndroidService()
    {
#if UNITY_ANDROID
        customClass.CallStatic("StartCheckerService", unityActivity);
#elif UNITY_IOS
        //    applicationDidEnterBackground();
//        _makeToast();
#endif

    }

    void StopAndroidService()
    {
        //AndroidJavaClass unityClass = new AndroidJavaClass("com.unity3d.player.UnityPlayer");
        //AndroidJavaObject unityActivity = unityClass.GetStatic<AndroidJavaObject>("currentActivity");
        //AndroidJavaClass customClass = new AndroidJavaClass("com.wear.locationservice.UnityLocationService");
#if UNITY_ANDROID
        customClass.CallStatic("StopCheckerService", unityActivity);
#elif UNITY_IOS

#endif
    }

    public Agent getAgentInfo(string agentID)
    {
        Agent ag = new Agent();
        return ag;
    }

    //public IEnumerator getObjectByTargetID(string targetID) {

    //    assetName = targetID;
    //    yield return StartCoroutine(LoadAsset());
    //}

    IEnumerator CheckInternetConnection()
    {
        while (true)
        {
            if (Application.internetReachability == NetworkReachability.NotReachable) hasInternetConnection = false;
            else hasInternetConnection = true;
//            {
//                Debug.Log("Error. Check internet connection!");
//            }
//            //           WWW www = new WWW("http://192.168.1.105/dashboard");
//            WWW www = new WWW("http://www.this-page-intentionally-left-blank.org/");
            
//            float waitTime = 2;
//            while (!www.isDone && waitTime > 0)
//            {
//                yield return new WaitForSeconds(0.1f);
//                waitTime-=0.1f;
//            }
            
//            if (!www.isDone | www.error != null)
//            {
//                hasInternetConnection = false;
//            }
//            else {
//                hasInternetConnection = true;
//            }
////            connectionInfoPanel.SetActive(!hasInternetConnection);
            yield return new WaitForSeconds(.2f);
        }
    }

//    IEnumerator LoadAsset()
//    {
//        yield return StartCoroutine(Initialize());
//        // Load asset.
//        yield return StartCoroutine(InstantiateGameObjectAsync(assetBundleName, assetName));
////        status += "finish\n";
//    }

    protected IEnumerator Initialize()
    {
        DontDestroyOnLoad(gameObject);

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


    public void CheckTargetID(string targetID)
    {
        if(hasInternetConnection)
            StartCoroutine(GetContentByTarget(targetID));
    }

    IEnumerator GetContentByTarget(string targetID)
    {
        UnityEngine.UI.Text info = GameObject.Find("Canvas").transform.FindChild("info").GetComponent<UnityEngine.UI.Text>();
        info.text = "loading";
        yield return new WaitForSeconds(0.5f);
        Marker marker = WebApiClient.SendAndDeserialize<Marker>(EMethod.GET,
                "http://wear-h5.azurewebsites.net/api/markers/vuforia/"+ targetID);


        yield return new WaitForSeconds(0.5f);
        info.text = "loading 2";
        Content content = WebApiClient.SendAndDeserialize<Content>(EMethod.GET,
                "http://wear-h5.azurewebsites.net/api/contents/" + marker.ContentId);

        yield return new WaitForSeconds(0.5f);
        info.text = "loading 3";

        switch (content.ContentType)
        {
            case EContentType.AssetBundle:
                WeAr.H5.Domain.Model.AssetBundle bundle = WebApiClient.SendAndDeserialize<WeAr.H5.Domain.Model.AssetBundle>(EMethod.GET,
                "http://wear-h5.azurewebsites.net/api/contents/asset-bundle/" + content.Id);
                AssetBundleCreateRequest assetBundleCreateRequest = UnityEngine.AssetBundle.LoadFromMemoryAsync(bundle.Value);
                yield return assetBundleCreateRequest;
                UnityEngine.AssetBundle assetBundle = assetBundleCreateRequest.assetBundle;
//                UnityEngine.AssetBundle assetBundle = UnityEngine.AssetBundle.CreateFromMemoryA(bundle.Value);

                GameObject go = Instantiate(assetBundle.LoadAllAssets<GameObject>()[0]);
                go.name = "3dModel";
                go.SetActive(true);
                Transform goTransform = go.GetComponent<Transform>();


                GameObject target = GameObject.Find("ImageTarget");
                goTransform.SetParent(target.transform);
                goTransform.localScale = Vector3.one * 0.01f;
                break;
            case EContentType.Image:
                GameObject image = GameObject.Find("ImageTarget").transform.FindChild("Image").gameObject;
                image.SetActive(true);
                WeAr.H5.Domain.Model.Image im = WebApiClient.SendAndDeserialize<WeAr.H5.Domain.Model.Image>(EMethod.GET,
                "http://wear-h5.azurewebsites.net/api/contents/image/" + content.Id);
                Texture2D tex = new Texture2D(2, 2);
                tex.LoadImage(im.Value);
                image.GetComponent<Renderer>().material.mainTexture = tex;

                break;
            case EContentType.Link:
                Link link = WebApiClient.SendAndDeserialize<Link>(EMethod.GET,
                "http://wear-h5.azurewebsites.net/api/contents/link/" + content.Id);
                Application.OpenURL(link.Value);
                break;
        }

        info.text = "finish loading";
        //WWW www = new WWW("" + nameOfTarget);

        //loadingBar.SetActive(true);
        //Image loading = loadingBar.GetComponentsInChildren<Image>()[1];
        //while (!www.isDone)
        //{
        //    loading.fillAmount = www.progress;
        //    yield return new WaitForFixedUpdate();
        //}
        //loadingBar.SetActive(false);

        //string contentType = www.responseHeaders["Content-Type"];
        //if (contentType.Contains("image"))
        //{
        //    testImage.gameObject.SetActive(true);
        //    testImage.material.mainTexture = www.texture;
        //    float coef = www.texture.width > www.texture.height ? www.texture.width : www.texture.height;
        //    testImage.transform.localScale = new Vector3(www.texture.width / coef, www.texture.height / coef, 1);
        //}
        //else if (contentType.Contains("video"))
        //{
        //    string format = "";
        //    if (contentType.Contains("x-matroska"))
        //        format = "mkv";
        //    else if (contentType.Contains("mp4"))
        //        format = "mp4";
        //    string path = Path.Combine(Application.persistentDataPath, "testVideo." + format);
        //    FileStream ff = new FileStream(path, FileMode.Create);
        //    ff.Write(www.bytes, 0, www.bytesDownloaded);
        //    ff.Close();

        //    GameObject go = Instantiate(Resources.Load<GameObject>("VideoPlay"));
        //    go.name = "VideoPlay";
        //    go.transform.SetParent(imgTarget);
        //    go.transform.localPosition = Vector3.zero;
        //    go.transform.localScale = new Vector3(.05f, .05f, .028f);
        //    _testVideo = go;
        //    _testVideo.GetComponent<VideoPlaybackBehaviour>().m_path = path;
        //    _testVideo.gameObject.SetActive(true);
        //}
        //else if (contentType.Contains("audio"))
        //{
        //    if (contentType.Contains("wav"))
        //        audioPlayer.clip = www.GetAudioClip(false, false, AudioType.WAV);
        //    else if (contentType.Contains("mpeg") || contentType.Contains("mp3"))
        //        audioPlayer.clip = www.GetAudioClip(false, false, AudioType.MPEG);
        //    audioPlayer.Play();
        //    particles.Play();
        //}
        //else if (contentType.Contains("text"))
        //{
        //    JsonClass myObject = JsonUtility.FromJson<JsonClass>(www.text);
        //    _userPhone = myObject.phone;
        //    _userSMSPhone = myObject.smsphone;
        //    _userEmail = myObject.email;
        //    _userLink = myObject.link;
        //    TextParsing();
        //}
        //else if (contentType.Contains("octet-stream"))
        //{
        //    _assetBundle = www.assetBundle;
        //    GameObject go = Instantiate(www.assetBundle.LoadAllAssets<GameObject>()[0]);
        //    go.name = "3dModel";
        //    Transform goTransform = go.GetComponent<Transform>();
        //    goTransform.SetParent(imgTarget);
        //    goTransform.localScale = Vector3.one * 4;
        //    goTransform.localPosition = new Vector3(0, .1f, 0);
        //    goTransform.localEulerAngles = Vector3.zero;
        //}
//        yield return null;
    }






//    protected IEnumerator InstantiateGameObjectAsync(string assetBundleName, string assetName)
//    {
//        // This is simply to get the elapsed time for this phase of AssetLoading.
//        float startTime = Time.realtimeSinceStartup;
////        status += "time " + startTime+"\n";
//        // Load asset from assetBundle.
//        AssetBundleLoadAssetOperation request = AssetBundleManager.LoadAssetAsync(assetBundleName, assetName, typeof(GameObject));
//        if (request == null)
//        {
//            status += "request null\n";
//            yield break;
//        }
//        else {
//            status += "request ok\n";
//        }   
//        yield return StartCoroutine(request);

//        // Get the asset.
//        GameObject prefab = request.GetAsset<GameObject>();
//        if (prefab != null) {
//            currObj = Instantiate(prefab);
//            GameObject target = GameObject.Find("ImageTarget");
//            target.GetComponent<CloudRecoTrackableEventHandler>().currObject = currObj;
//            currObj.transform.parent = target.transform;
//            status += "obj created\n";
//        }       
//        else status += "prefab null\n";
//        // Calculate and display the elapsed time.
//        float elapsedTime = Time.realtimeSinceStartup - startTime;
//        status += ""+ assetName + (prefab == null ? " was not" : " was") + " loaded successfully in " + elapsedTime + " seconds\n";
//        Debug.Log(assetName + (prefab == null ? " was not" : " was") + " loaded successfully in " + elapsedTime + " seconds");
//        //status += "" + currObj.transform.position+"\n";
//        //status += "" + currObj.transform.localScale + "\n";
//    }
}

