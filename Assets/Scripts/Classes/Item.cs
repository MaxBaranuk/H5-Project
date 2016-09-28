using UnityEngine;
using System.Collections;
using System.Xml;
using System.Xml.Serialization;
using AssetBundles;
using System;

[System.Serializable]
public class Item {

    ///    public const string AssetBundlesOutputPath = "/AssetBundles/";
    //    public string assetBundleName;
    //    public string assetName;

    //    public enum ContentType {Link, Video, ThreeDObject, Image}
    public int id;
    public float Lat;
    public float Lon;
    public string type;
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
