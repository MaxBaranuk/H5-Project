using System.Text;
using UnityEngine;

public class OnlineMapsGoogleGetPlace : OnlineMapsGoogleAPIQuery
{

    public override OnlineMapsQueryType type
    {
        get { return OnlineMapsQueryType.direction; }
    }

    private OnlineMapsGoogleGetPlace(float lat, float lon)
    {
        _status = OnlineMapsQueryStatus.downloading;
        StringBuilder url = new StringBuilder();
              url.AppendFormat("https://maps.googleapis.com/maps/api/geocode/xml?latlng="+ lat + ","+lon);
//        url.AppendFormat("https://maps.googleapis.com/maps/api/directions/xml?origin={0}&destination={1}&key=AIzaSyDlsy75sNjiK7i6sOucbRHuYPuYzwj4KrY", origin, destination);
        www = OnlineMapsUtils.GetWWW(url);
        OnlineMaps.instance.AddGoogleAPIQuery(this);
    }

    public static OnlineMapsGoogleGetPlace Find(float lat, float lon)
    {
        return new OnlineMapsGoogleGetPlace(lat, lon);
    }
}
