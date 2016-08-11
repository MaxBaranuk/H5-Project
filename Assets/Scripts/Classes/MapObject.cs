using UnityEngine;
using System.Collections;

public struct MapObject {

    public enum ContentType {Link, Video, ThreeDObject, Image}
    public Vector2 location { get; set; }
	public float price { get; set; }
    public ContentType contenttype { get; set; }
    public string info { get; set; }
    public string contentLink { get; set; }
    public string agentID { get; set; }
    public string targetID { get; set; }


}
