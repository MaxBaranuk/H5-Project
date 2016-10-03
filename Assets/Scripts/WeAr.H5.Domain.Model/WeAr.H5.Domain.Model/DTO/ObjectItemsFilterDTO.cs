using System;
using System.Collections.Generic;
using System.Text;

// ReSharper disable once InconsistentNaming
namespace WeAr.H5.Domain.Model.DTO
{
    public class ObjectItemsFilterDTO
    {
        public ObjectItemsFilterDTO()
        {            
        }

        public LocationFilter Location { get; set; }

        public int[] AgentIds { get; set; }

        public PriceFilter Price { get; set; }

        public short[] Types { get; set; }

    }

    public class LocationFilter
    {
        public float? Latitude { get; set; }
        public float? Longitude { get; set; }
        public float? Radius { get; set; }
    }

    public class PriceFilter
    {
        public int? PriceFrom { get; set; }
        public int? PriceTo { get; set; }
    }
}
