using UnityEngine;
using System.Collections;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using System.Collections.Generic;
using System.Text;
using Newtonsoft.Json;
using WeAr.H5.Domain.Model;

public static class Settings {

    public enum SceneTypes { AR,GEO}

    public static SceneTypes nextScene = SceneTypes.AR;

    public static void SaveMapObjects()
    {
        var encoding = new UTF8Encoding();
        FileStream file = File.Create(Application.persistentDataPath + "/savedObjects.dat"); //you can call it anything you want
        string data = JsonConvert.SerializeObject(MapObjectsCache.items);
        byte[] bytes = encoding.GetBytes(data);
        file.Write(bytes, 0, bytes.Length);
        file.Close();

    }

    public static void LoadMapObjects()
    {
        if (File.Exists(Application.persistentDataPath + "/savedObjects.dat"))
        {
//            BinaryFormatter bf = new BinaryFormatter();
            FileStream file = File.Open(Application.persistentDataPath + "/savedObjects.dat", FileMode.Open);
            StreamReader inp_stm = new StreamReader(file);
            string data = inp_stm.ReadToEnd();
            MapObjectsCache.items = JsonConvert.DeserializeObject<Dictionary<int, ObjectItem>>(data);
            
            inp_stm.Close();
            file.Close();
            //byte[] arr = new byte[file.Length];
            //file.Read(arr, 0, arr.Length);

            //MapObjectsCache.items = JsonConvert.DeserializeObject<Dictionary<int, ObjectItem>>(arr.ToString());
            //file.Close();
        }
        else {
            MapObjectsCache.items = new Dictionary<int, ObjectItem>();
        }
    }
}
