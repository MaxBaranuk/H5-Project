using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using WeAr.H5.Domain.Model.DTO;
using WeAr.H5.Domain.Model.Enums;
using WeAr.H5.Domain.Model;

public class ARContentManager : MonoBehaviour {

    Dictionary<string, FullContentDTO> loadedContent;
    GameObject loadingPanel;
    GameObject currObj;
    GameObject currImage;
    GameObject errorDownloadingPanel;
         

    void OnEnable()
    {
        CloudRecoTrackableEventHandler.targetFind += GetContentByTargetName;
        CloudRecoTrackableEventHandler.targetLost += LoseTarget;
    }

    // Use this for initialization
    void Start () {
        loadedContent = new Dictionary<string, FullContentDTO>();
        loadingPanel = GameObject.Find("Canvas").transform.FindChild("LoadingPanel").gameObject;
        errorDownloadingPanel = GameObject.Find("Canvas").transform.FindChild("ErrorLoadingPanel").gameObject;
    }
	
	// Update is called once per frame
	void Update () {
	
	}

    public void GetContentByTargetName(string targetName) {
        StartCoroutine(GettingContent(targetName));
    }

    IEnumerator GettingContent(string targetName) {
        if (!loadedContent.ContainsKey(targetName)) {
            yield return StartCoroutine(DownloadContent(targetName));
        }
        if (ServerManager.Instanse().content == null) StartCoroutine(ShowErrorMessage());
        else StartCoroutine(ShowContent(targetName));

        //if (ServerManager.Instanse().hasInternetConnection)
        //    yield return StartCoroutine(DownloadContent(targetName));
        //if (ServerManager.Instanse().content == null) StartCoroutine(ShowErrorMessage());
        //else StartCoroutine(ShowContent(targetName));
    }


    IEnumerator DownloadContent(string targetName) {
        loadingPanel.SetActive(true);
        yield return StartCoroutine(ServerManager.Instanse().GetContentByTarget(targetName));
        loadedContent.Add(targetName, ServerManager.Instanse().content);
        loadingPanel.SetActive(false);
    }

    IEnumerator ShowContent(string targetName) {
        FullContentDTO content = loadedContent[targetName];
        switch (content.ContentType)
        {
            case EContentType.AssetBundle:

                WeAr.H5.Domain.Model.AssetBundle bundle = content.AssetBundle;
                AssetBundleCreateRequest assetBundleCreateRequest = UnityEngine.AssetBundle.LoadFromMemoryAsync(bundle.Value);
                yield return assetBundleCreateRequest;
                UnityEngine.AssetBundle assetBundle = assetBundleCreateRequest.assetBundle;
                //                assetBundle.name = "" + targetID;
                //                UnityEngine.AssetBundle assetBundle = UnityEngine.AssetBundle.CreateFromMemoryA(bundle.Value);

                currObj = Instantiate(assetBundle.LoadAllAssets<GameObject>()[0]);
                currObj.name = "3dModel";
                currObj.SetActive(true);
                Transform goTransform = currObj.GetComponent<Transform>();


                GameObject target = GameObject.Find("ImageTarget");
                goTransform.SetParent(target.transform);
                goTransform.localScale = Vector3.one * 0.01f;
                assetBundle.Unload(false);
                break;
            case EContentType.Image:
                currImage = GameObject.Find("ImageTarget").transform.FindChild("Image").gameObject;
                currImage.SetActive(true);

                Image im = content.Image;
                Texture2D tex = new Texture2D(2, 2);
                tex.LoadImage(im.Value);
                currImage.GetComponent<Renderer>().material.mainTexture = tex;

                break;
            case EContentType.Link:

                Link link = content.Link;
                Application.OpenURL(link.Value);
                break;
        }
    }

    IEnumerator ShowErrorMessage() {
        errorDownloadingPanel.SetActive(true);
        yield return new WaitForSecondsRealtime(2);
        errorDownloadingPanel.SetActive(false);
    }

    void LoseTarget() {
        StopAllCoroutines();
        loadingPanel.SetActive(false);
        if (currObj) currObj.SetActive(false);
        if (currImage) currImage.SetActive(false);
    }

    void OnDisable()
    {
        CloudRecoTrackableEventHandler.targetFind -= GetContentByTargetName;
        CloudRecoTrackableEventHandler.targetLost -= LoseTarget;
    }
}
