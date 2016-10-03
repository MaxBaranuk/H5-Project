using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class FixScrollRect : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler, IScrollHandler
{
    public ScrollRect MainScroll;
//    public CapturePhotoScene sceneController;

    void Start() {
//       sceneController = GameObject.Find("SelfieManager").GetComponent<CapturePhotoScene>();
        MainScroll = GetComponentInParent<ScrollRect>();
//        sceneController.galaryPanel.GetComponent<ScrollRect>();
    }

    public void OnBeginDrag(PointerEventData eventData)
    {
        MainScroll.OnBeginDrag(eventData);
//        GetComponent<GlassGalaryItem>().isButtonClicked = false;
    }


    public void OnDrag(PointerEventData eventData)
    {
        MainScroll.OnDrag(eventData);
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        MainScroll.OnEndDrag(eventData);
//        GetComponent<GlassGalaryItem>().isButtonClicked = true;
    }


    public void OnScroll(PointerEventData data)
    {
        MainScroll.OnScroll(data);
    }


}
