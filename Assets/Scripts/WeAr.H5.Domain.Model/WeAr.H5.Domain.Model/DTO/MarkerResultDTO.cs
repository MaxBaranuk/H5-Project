using System;
using System.Collections.Generic;
using System.Security.AccessControl;
using System.Text;

// ReSharper disable once InconsistentNaming
namespace WeAr.H5.Domain.Model.DTO
{
    public class MarkerResultDTO
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public int ContentId { get; set; }

        public string VuforiaId { get; set; }
    }
}
