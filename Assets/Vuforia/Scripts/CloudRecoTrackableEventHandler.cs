/*===============================================================================
Copyright (c) 2015 PTC Inc. All Rights Reserved.
 
Copyright (c) 2010-2015 Qualcomm Connected Experiences, Inc. All Rights Reserved.
 
Vuforia is a trademark of PTC Inc., registered in the United States and other 
countries.
===============================================================================*/
using UnityEngine;
using Vuforia;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System;
/// <summary>
/// A custom handler that implements the ITrackableEventHandler interface.
/// </summary>
public class CloudRecoTrackableEventHandler : MonoBehaviour, ITrackableEventHandler
{
    public delegate void TargetFindAction(string targetName);
    public delegate void TargetLostAction();
    public static event TargetFindAction targetFind;
    public static event TargetLostAction targetLost;
    public GameObject currObject;
    public Text info;
//    public GameObject agentButton;
    public GameObject mainMenu;
    private bool currMenuStateIsActive;
    #region PRIVATE_MEMBERS
    private TrackableBehaviour mTrackableBehaviour;
    private ObjectTracker objectTracker;
    //public static Action targetFind;
    //public static Action targetLost;
    #endregion // PRIVATE_MEMBERS


    #region MONOBEHAVIOUR_METHODS
    void Start()
    {
        mTrackableBehaviour = GetComponent<TrackableBehaviour>();
        if (mTrackableBehaviour)
        {
            mTrackableBehaviour.RegisterTrackableEventHandler(this);
        }
        objectTracker = TrackerManager.Instance.GetTracker<ObjectTracker>();
    }

    void Update() {
        if (currMenuStateIsActive != mainMenu.activeInHierarchy)
        {
            currMenuStateIsActive = mainMenu.activeInHierarchy;
            if (currMenuStateIsActive) objectTracker.TargetFinder.Stop();
            else objectTracker.TargetFinder.StartRecognition();
        }
//        info.text = ServerManager.instanse.status;
    }
    #endregion //MONOBEHAVIOUR_METHODS


    #region PUBLIC_METHODS
    /// <summary>
    /// Implementation of the ITrackableEventHandler function called when the
    /// tracking state changes.
    /// </summary>
    public void OnTrackableStateChanged(
                                    TrackableBehaviour.Status previousStatus,
                                    TrackableBehaviour.Status newStatus)
    {
        if (newStatus == TrackableBehaviour.Status.DETECTED ||
            newStatus == TrackableBehaviour.Status.TRACKED ||
            newStatus == TrackableBehaviour.Status.EXTENDED_TRACKED)
        {
            OnTrackingFound();
        }
        else if (previousStatus == TrackableBehaviour.Status.UNKNOWN &&
                 newStatus == TrackableBehaviour.Status.NOT_FOUND)
        {
            // Ignore this specific combo
            return;
        }
        else
        {
            OnTrackingLost();
        }
    }
    #endregion //PUBLIC_METHODS


    #region PRIVATE_METHODS

    private void OnTrackingFound()
    {
        string id = ""+mTrackableBehaviour.Trackable.Name;

        info.text = "loading: "+id;

        
        //        if (id == "ananas_group")
        //        {
        //            StartCoroutine(ServerManager.instanse.getObjectByTargetID(id));
        ////            agentButton.SetActive(true);
        //            ServerManager.instanse.status += "";
        //        }
        //        if (id == "customTarget") {
        //            SceneManager.LoadScene("WebView");
        //        }

        Renderer[] rendererComponents = GetComponentsInChildren<Renderer>(true);
        Collider[] colliderComponents = GetComponentsInChildren<Collider>(true);

        // Enable rendering:
        foreach (Renderer component in rendererComponents)
        {
            component.enabled = true;
        }

        // Enable colliders:
        foreach (Collider component in colliderComponents)
        {
            component.enabled = true;
        }

        // Stop finder since we have now a result, finder will be restarted again when we lose track of the result
        
        if (objectTracker != null)
        {
            objectTracker.TargetFinder.Stop();
        }

        targetFind(id);
        

        Debug.Log("Trackable " + mTrackableBehaviour.TrackableName + " found");
    }

    private void OnTrackingLost()
    {

        if (objectTracker != null)
        {
            objectTracker.TargetFinder.ClearTrackables(false);
            objectTracker.TargetFinder.StartRecognition();
        }

        //        g.SetActive(false);
//        info.text = "clear";
        ServerManager.Instanse().status = "";
        Destroy(currObject);
        transform.FindChild("Image").gameObject.SetActive(false);
//        agentButton.SetActive(false);
        Renderer[] rendererComponents = GetComponentsInChildren<Renderer>(true);
        Collider[] colliderComponents = GetComponentsInChildren<Collider>(true);

        // Disable rendering:
        foreach (Renderer component in rendererComponents)
        {
            component.enabled = false;
        }

        // Disable colliders:
        foreach (Collider component in colliderComponents)
        {
            component.enabled = false;
        }

        // Start finder again if we lost the current trackable

        targetLost();

 //       ServerManager.Instanse().LoseTarget();
        Debug.Log("Trackable " + mTrackableBehaviour.TrackableName + " lost");
    }


    #endregion //PRIVATE_METHODS
}
