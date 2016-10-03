using System;
using System.Collections.Generic;
using System.Text;

// ReSharper disable once InconsistentNaming
namespace WeAr.H5.Domain.Model.DTO
{
    public class AgentResultDTO
    {
        public int Id { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Phone1 { get; set; }
        public string Phone2 { get; set; }
        public string Email { get; set; }
        public string Skype { get; set; }
        public string Address { get; set; }
        public string Description { get; set; }
    }
}
