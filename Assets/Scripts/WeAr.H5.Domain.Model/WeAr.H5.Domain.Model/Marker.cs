namespace WeAr.H5.Domain.Model
{
    public class Marker
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public int ObjectItemId { get; set; }
        public byte[] Image { get; set; }

        public virtual ObjectItem ObjectItem { get; set; }
    }
}
