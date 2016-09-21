using UnityEngine;
using System.Collections;
using System.IO;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System;
using System.Runtime.InteropServices;

public class InfoSceneManager : MonoBehaviour {


    public GameObject variantsPanel;
    public GameObject mainButton;
    public Text info;
    bool isServiceRun;
    int counter;
    AndroidJavaClass unityClass;
    AndroidJavaObject unityActivity;
    AndroidJavaClass customClass;

#if UNITY_IOS
 //   [DllImport("__Internal")]
//    private static extern void applicationDidEnterBackground();
 //   private static extern void _makeToast();
#endif
    // Use this for initialization
    void Start() {
        unityClass = new AndroidJavaClass("com.unity3d.player.UnityPlayer");
        unityActivity = unityClass.GetStatic<AndroidJavaObject>("currentActivity");
        customClass = new AndroidJavaClass("com.wear.locationservice.UnityLocationService");
        //       Application.runInBackground = true;
    }

    // Update is called once per frame
    void Update() {
        if (Input.GetKeyDown(KeyCode.Escape)) {
            if (variantsPanel.activeInHierarchy) {
                mainButton.SetActive(true);
                variantsPanel.SetActive(false);
            }
            else SceneManager.LoadScene("main");
        }
    }

    public void StartDownload() {
        if (!variantsPanel.activeInHierarchy)
            variantsPanel.SetActive(true);
        mainButton.SetActive(false);
    }

    public void DownloadToDevice() {
        StartCoroutine(Downloading());

    }

    public void SendToMail() {
        mainButton.SetActive(true);
        variantsPanel.SetActive(false);
    }

    public void LinkToDropbox() {
        Application.OpenURL("https://drive.google.com/open?id=0B7Z-psnMTZbnckJCbEtuTmEtQVU");
        mainButton.SetActive(true);
        variantsPanel.SetActive(false);
    }

    IEnumerator Downloading() {
        string fileName = "senna.jpg";
        string url = "http://192.168.1.105/images/" + fileName;
        Texture2D texture = new Texture2D(1, 1);
        WWW www = new WWW(url);

        while (!www.isDone) {
            info.text = "Loading: " + (int)(www.progress * 100) + "%";
            yield return new WaitForEndOfFrame();
        }
        if (www.error == null)
        {
            string path = Application.persistentDataPath;
#if (UNITY_ANDROID) && (!UNITY_EDITOR)
        try
        {
            using (AndroidJavaClass unity = new AndroidJavaClass("com.unity3d.player.UnityPlayer"))
            {
                using (AndroidJavaObject activity = unity.GetStatic<AndroidJavaObject>("currentActivity"))
                {
                    using (AndroidJavaClass storage = new AndroidJavaClass("com.wear.h5lib.Storage"))
                    {
                            path = storage.CallStatic<string>("GetInternalStoragePath", activity);
//                        info.text = ""
//                                +storage.CallStatic<string>("GetExternalStoragePath", activity)+"\n"
//                                      + root + "\n"
//;
//                        path = root;
//                            path = storage.CallStatic<string>("GetExternalStoragePath", activity);
                        }
                }
            }
        }
        catch (Exception e)
        {
            info.text = ""+e.Message+"\n";
        }
#endif
            path += "/" + fileName;
            info.text = "Loaded: " + fileName + " (" + www.bytesDownloaded / 1024 + " kB)";
            try
            {
                File.WriteAllBytes(path, www.bytes);

            }
            catch (Exception e) {
                info.text += "" + e.Message;
            }
        }
        else {
            info.text = www.error;
        }
        yield return new WaitForSeconds(1);
        info.text = "";
        //        info.text += "\n"+Application.persistentDataPath;
        //        www.LoadImageIntoTexture(texture);
        //        byte[] dataToSave = texture.EncodeToPNG();

        mainButton.SetActive(true);
        variantsPanel.SetActive(false);
    }

    public void StartService() {
        if (isServiceRun)
        {
            isServiceRun = false;
            StopAndroidService();
        }
        else {
            isServiceRun = true;
            StartAndroidService();
        }
    }

    void StartAndroidService()
    {
#if UNITY_ANDROID
        customClass.CallStatic("StartCheckerService", unityActivity);
#elif UNITY_IOS
        //    applicationDidEnterBackground();
        _makeToast();
#endif

}

    void StopAndroidService() {
    //AndroidJavaClass unityClass = new AndroidJavaClass("com.unity3d.player.UnityPlayer");
    //AndroidJavaObject unityActivity = unityClass.GetStatic<AndroidJavaObject>("currentActivity");
    //AndroidJavaClass customClass = new AndroidJavaClass("com.wear.locationservice.UnityLocationService");
#if UNITY_ANDROID
        customClass.CallStatic("StopCheckerService");
#elif UNITY_IOS

#endif
}
}
