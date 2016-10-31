using System;

namespace WeAr.H5.Domain.Model
{
    [Serializable]
    public class ObjectItem
    {
        public int Id { get; set; }

        public int AgentId { get; set; }

        public string Name { get; set; }

        public string Description { get; set; }

        public byte[] Photo { get; set; }
        public string ImageType { get; set; }

        public float Latitude { get; set; }
        public float Longitude { get; set; }

        public int Price { get; set; }

        public byte Type { get; set; }

        public virtual Agent Agent { get; set; }
    }
}
