using UnityEngine;
using System.Collections;
using System.Xml;
using System.Xml.Serialization;

public class Item {

//    public enum ContentType {Link, Video, ThreeDObject, Image}
    public float Lat;
    public float Lon;
    public string info;
    //public float price { get; set; }
    //public ContentType contenttype { get; set; }
    //public string info { get; set; }
    //public string contentLink { get; set; }
    //public string agentID { get; set; }
    //public string targetID { get; set; }
    [XmlAttribute("name")]
    public string Name;

    public void ShowObject()
    {

    }
}
