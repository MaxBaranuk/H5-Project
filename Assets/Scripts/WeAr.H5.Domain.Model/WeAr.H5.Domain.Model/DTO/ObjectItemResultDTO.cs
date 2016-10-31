using System;
using System.Collections.Generic;
using System.Text;

// ReSharper disable once InconsistentNaming
namespace WeAr.H5.Domain.Model.DTO
{
    [Serializable]
    public class ObjectItemResultDTO
    {
        public int Id { get; set; }

        public int AgentId { get; set; }

        public string Name { get; set; }

        public string Description { get; set; }

        public float Latitude { get; set; }
        public float Longitude { get; set; }

        public int Price { get; set; }

        public byte Type { get; set; }
    }
}
