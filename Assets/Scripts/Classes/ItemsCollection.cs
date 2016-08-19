using System.Collections.Generic;
using System.Xml.Serialization;
using System.IO;
using System.Xml;
using UnityEngine;

[XmlRoot("PlaceCollection")]
public class ItemsCollection {

    [XmlArray("Places")]
    [XmlArrayItem("Place")]
    public Item[] items;

    public static ItemsCollection Load(string path)
    {
        var serializer = new XmlSerializer(typeof(ItemsCollection));
        Stream reader = new MemoryStream((Resources.Load(path, typeof(TextAsset)) as TextAsset).bytes);
        StreamReader textReader = new StreamReader(reader);
        ItemsCollection lib = (ItemsCollection)serializer.Deserialize(textReader);
        reader.Dispose();
        return lib;
    }
}
