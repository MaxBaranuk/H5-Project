using System;
using System.Collections.Generic;
using System.Text;
using WeAr.H5.Domain.Model.Enums;

namespace WeAr.H5.Domain.Model.DTO
{
    public class FullContentDTO
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public string VuforiaId { get; set; }
        public EContentType ContentType { get; set; }

        public Image Image { get; set; }
        public Link Link { get; set; }
        public AssetBundle AssetBundle { get; set; }
    }
}
