using UnityEngine;
using System.Collections;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using System.Collections.Generic;

public static class Settings {

    public enum SceneTypes { AR,GEO}

    public static SceneTypes nextScene = SceneTypes.AR;

    public static void SaveMapObjects()
    {
        BinaryFormatter bf = new BinaryFormatter();
        //Application.persistentDataPath is a string, so if you wanted you can put that into debug.log if you want to know where save games are located
        FileStream file = File.Create(Application.persistentDataPath + "/savedObjects.dat"); //you can call it anything you want
        bf.Serialize(file, MapObjectsCache.items);
        file.Close();
    }

    public static void LoadMapObjects()
    {
        if (File.Exists(Application.persistentDataPath + "/savedObjects.dat"))
        {
            BinaryFormatter bf = new BinaryFormatter();
            FileStream file = File.Open(Application.persistentDataPath + "/savedObjects.dat", FileMode.Open);
            MapObjectsCache.items = (Dictionary<int, Item>)bf.Deserialize(file);
            file.Close();
        }
        else {
            MapObjectsCache.items = new Dictionary<int, Item>();
        }
    }
}
