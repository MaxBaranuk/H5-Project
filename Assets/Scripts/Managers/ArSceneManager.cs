using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;
using System.IO;
using UnityEngine.UI;

public class ArSceneManager : MonoBehaviour
{

    public GameObject connectionInfoPanel;
    public GameObject mainMenuPanel;
    public GameObject menuPanel;
    public Button photoButton;
    public Button mapButton;
    public Button menuButton;
    bool _isProcessing;

    // Use this for initialization
    void Update()
    {
        connectionInfoPanel.SetActive(!ServerManager.instanse.hasInternetConnection);
    }

    public void ToMap()
    {
        Settings.nextScene = Settings.SceneTypes.GEO;
        SceneManager.LoadScene("loadingScene");
    }

    public void ScreenShot()
    {
        if (!_isProcessing)
        {
            photoButton.GetComponent<Image>().enabled = false;
            mapButton.GetComponent<Image>().enabled = false;
            menuButton.GetComponent<Image>().enabled = false;
            StartCoroutine(ShareScreenshot());
        }
    }

    public void OpenMenuPanel()
    {
        mainMenuPanel.SetActive(true);
        menuPanel.SetActive(true);
    }

    public IEnumerator ShareScreenshot()
    {
        _isProcessing = true;
        yield return new WaitForEndOfFrame();

        Texture2D screenTexture = new Texture2D(Screen.width, Screen.height, TextureFormat.RGB24, true);
        screenTexture.ReadPixels(new Rect(0, 0, Screen.width, Screen.height), 0, 0);
        screenTexture.Apply();

        byte[] dataToSave = screenTexture.EncodeToPNG();
        string date = System.DateTime.Now.ToString("yyyy-MM-dd-HH-mm-ss");
        Debug.Log(date);
        string path = Application.persistentDataPath + "/wear" + date + ".png";
        //      string path = Application.persistentDataPath + "/wear.png";
        File.WriteAllBytes(path, dataToSave);

#if UNITY_ANDROID

            AndroidJavaClass intentClass = new AndroidJavaClass("android.content.Intent");

            AndroidJavaObject intentObject = new AndroidJavaObject("android.content.Intent");

            intentObject.Call<AndroidJavaObject>("setAction", intentClass.GetStatic<string>("ACTION_SEND"));

            AndroidJavaClass uriClass = new AndroidJavaClass("android.net.Uri");

            AndroidJavaObject uriObject = uriClass.CallStatic<AndroidJavaObject>("parse", "file://" + path);

            intentObject.Call<AndroidJavaObject>("putExtra", intentClass.GetStatic<string>("EXTRA_STREAM"), uriObject);

            intentObject.Call<AndroidJavaObject>("setType", "image/*");

            AndroidJavaClass unity = new AndroidJavaClass("com.unity3d.player.UnityPlayer");

            AndroidJavaObject currentActivity = unity.GetStatic<AndroidJavaObject>("currentActivity");

            currentActivity.Call("startActivity", intentObject);

#elif UNITY_IOS
            CallSocialShareAdvanced("", "", "", path);
#else
		Debug.Log("No sharing set up for this platform.");
#endif
        photoButton.GetComponent<Image>().enabled = true;
        mapButton.GetComponent<Image>().enabled = true;
        menuButton.GetComponent<Image>().enabled = true;
        _isProcessing = false;
    }

#if UNITY_IOS
    public struct ConfigStruct
    {
        public string title;
        public string message;
    }

    [DllImport("__Internal")]
    private static extern void showAlertMessage(ref ConfigStruct conf);

    public struct SocialSharingStruct
    {
        public string text;
        public string url;
        public string image;
        public string subject;
    }

    [DllImport("__Internal")]
    private static extern void showSocialSharing(ref SocialSharingStruct conf);

    public static void CallSocialShare(string title, string message)
    {
        ConfigStruct conf = new ConfigStruct();
        conf.title = title;
        conf.message = message;
        showAlertMessage(ref conf);
    }

    public static void CallSocialShareAdvanced(string defaultTxt, string subject, string url, string img)
    {
        SocialSharingStruct conf = new SocialSharingStruct();
        conf.text = defaultTxt;
        conf.url = url;
        conf.image = img;
        conf.subject = subject;

        showSocialSharing(ref conf);
    }
#endif
}