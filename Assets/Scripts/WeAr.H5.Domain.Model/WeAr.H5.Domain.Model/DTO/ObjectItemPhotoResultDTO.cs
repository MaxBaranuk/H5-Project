using System;
using System.Collections.Generic;
using System.Text;

// ReSharper disable once InconsistentNaming
namespace WeAr.H5.Domain.Model.DTO
{
    [Serializable]
    public class ObjectItemPhotoResultDTO
    {
        public int Id { get; set; }

        public byte[] Photo { get; set; }

        public string ImageType { get; set; }
    }
}
