using System;
using System.Collections.Generic;
using System.Text;
using WeAr.H5.Domain.Model.Enums;

// ReSharper disable once InconsistentNaming
namespace WeAr.H5.Domain.Model.DTO
{
    public class ContentResultDTO
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public int ObjectItemId { get; set; }
        public EContentType ContentType { get; set; }

        //public Image Image { get; set; }
        //public AssetBundle Image { get; set; }
        //public Link Image { get; set; }
    }
}
