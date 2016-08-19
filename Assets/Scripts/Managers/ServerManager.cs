using UnityEngine;
using System.Collections.Generic;

public class ServerManager : MonoBehaviour {

    public static ServerManager instanse;
	// Use this for initialization
	void Start () {
        if (instanse == null) instanse = this;
    }
	
	// Update is called once per frame
	void Update () {
	
	}

    public List<Item> getObjectsList(Vector2 location) {
        List<Item> obj = new List<Item>();


        return obj;
    }

    public Agent getAgentInfo(string agentID)
    {
        Agent ag = new Agent();
        return ag;
    }

    public Item getObjectByTargetID(string targetID) {
        return new Item();
    }
}
