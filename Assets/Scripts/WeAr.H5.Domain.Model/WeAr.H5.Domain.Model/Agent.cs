namespace WeAr.H5.Domain.Model
{   
    public class Agent
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

        public byte[] Photo { get; set; }
    }
}
