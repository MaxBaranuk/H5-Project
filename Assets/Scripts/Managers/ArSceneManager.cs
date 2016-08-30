using UnityEngine;
using System.Collections;
using UnityEngine.UI;
public class ArSceneManager : MonoBehaviour {

    public GameObject connectionInfoPanel;

    // Use this for initialization
    void Awake() {
//        StartCoroutine(CheckInternetConnection());
    }

    void Update() {
        connectionInfoPanel.SetActive(!ServerManager.instanse.hasInternetConnection);
    }

    //IEnumerator CheckInternetConnection()
    //{
    //    while (true)
    //    {
    //        WWW www = new WWW("http://192.168.1.105/dashboard");

    //        float waitTime = 2;
    //        while (!www.isDone && waitTime > 0)
    //        {
    //            yield return new WaitForSeconds(1);
    //            waitTime--;
    //        }
    //        bool hasInternetConnection = false;
    //        if (!www.isDone | www.error != null)
    //        {
    //            hasInternetConnection = false;
    //        }
    //        else {
    //            hasInternetConnection = true;
    //        }
    //        connectionInfoPanel.SetActive(!hasInternetConnection);
    //        yield return new WaitForSeconds(1);
    //    }
    //}
}
