using UnityEngine;
using System.Collections;
using MiniJSON;
using System.Collections.Generic;
using WeAr.H5.Domain.Model;
using  WeAr.H5.WebAPI.Client;
using Text = UnityEngine.UI.Text;

public class TestRequest : MonoBehaviour
{

    public Text info;
//    string key = "5sdGejj8uo74NIbkSNPa9SfoeWioLq1K";
	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

    public void FacebookShare()
    {

    }

    public void TwitterShare()
    {

    }

    public void InstagramShare()
    {

    }

    public void Connect()
    {

        //ApiResponse resp = WebApiClient.SendWithResponse(EMethod.GET, "http://wear-h5.azurewebsites.net/api/object-items/1");
        //info.text = resp.Result;

        for(int i = 2; i<25; i++)
        {
            ObjectItem it = new ObjectItem();
            it.Longitude = Random.Range(-180, 180);
            it.Latitude = Random.Range(-85, 85);
            it.Name = "Item - " + i;
            it.AgentId = 1;
            it.Description = "description";
            it.Price = 100;
            it.Type = 1;
            ApiResponse resp = WebApiClient.SendWithResponse(EMethod.POST, "http://wear-h5.azurewebsites.net/api/object-items", it);
            info.text = resp.Result;
        }
        //WWW connect = new WWW("http://www.mapquestapi.com/directions/v2/route?key="+key+"&ambiguities=ignore&callback=handleRouteResponse&avoidTimedConditions=false&outFormat=json&routeType=fastest&enhancedNarrative=false&shapeFormat=raw&generalize=0&locale=en_US&unit=m&from=Lancaster,PA&to=York,PA");
        //StartCoroutine(WaitForRequest(connect));

    }

//    IEnumerator WaitForRequest(WWW www)
//    {
//        yield return www;

//        // check for errors
//        if (www.error == null)
//        {

////            JSONObject j = new JSONObject(www.text);
//            Debug.Log(www.text);
//            string trim = www.text.Substring(20, www.text.Length-22);

//            //            System.IO.File.WriteAllText("D:\\texr.txt", trim);

//            //            //Debug.Log(www.text);
//            //            JSONObject jo = new JSONObject(trim);
//            ////            accessData(jo);
//            //            JSONObject jo0 = jo.GetField("route");
//            //////            Debug.Log(jo0);
//            //            JSONObject jo1 = jo0.GetField("shape");

//            //            JSONObject arr = jo1["shapePoints"];
//            //            for(int i = 0; i < arr.list.Count; i+=2){
//            //                Debug.Log(arr[i] + ", " + arr[i + 1]);
//            //            }
//            //            Debug.Log(arr);

//            var dict = Json.Deserialize(trim) as Dictionary<string, object>;
//            Dictionary<string, object> route = (Dictionary<string, object>) dict["route"];
//            Debug.Log(route["shape"]);

//        }
//        else {
//            Debug.Log("WWW Error: " + www.error);
//        }
//    }

    //void accessData(JSONObject obj)
    //{
    //    switch (obj.type)
    //    {
    //        case JSONObject.Type.OBJECT:
    //            for (int i = 0; i < obj.list.Count; i++)
    //            {
    //                string key = (string)obj.keys[i];
    //                JSONObject j = (JSONObject)obj.list[i];
    //                Debug.Log(key);
    //                accessData(j);
    //            }
    //            break;
    //        case JSONObject.Type.ARRAY:
    //            foreach (JSONObject j in obj.list)
    //            {
    //                accessData(j);
    //            }
    //            break;
    //        case JSONObject.Type.STRING:
    //            Debug.Log(obj.str);
    //            break;
    //        case JSONObject.Type.NUMBER:
    //            Debug.Log(obj.n);
    //            break;
    //        case JSONObject.Type.BOOL:
    //            Debug.Log(obj.b);
    //            break;
    //        case JSONObject.Type.NULL:
    //            Debug.Log("NULL");
    //            break;

    //    }
    //}
}
