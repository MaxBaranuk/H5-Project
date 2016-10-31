using System.Collections.Generic;
using System.Xml.Serialization;
using System.IO;
using System.Xml;
using UnityEngine;
using System;

[XmlRoot("PlaceCollection")]
public class ItemsCollection {

    [XmlArray("Places")]
    [XmlArrayItem("Place")]
    public Item[] items;

    public static ItemsCollection Load(string path)
    {
        ItemsCollection lib;
        try
        {
            var serializer = new XmlSerializer(typeof(ItemsCollection));
            Stream reader = new MemoryStream((Resources.Load(path, typeof(TextAsset)) as TextAsset).bytes);
            StreamReader textReader = new StreamReader(reader);
            lib = (ItemsCollection)serializer.Deserialize(textReader);
            reader.Dispose();
        }
        catch (Exception ) {
            lib = new ItemsCollection();
            lib.items = new Item[0];
        }
        return lib;
    }
}
