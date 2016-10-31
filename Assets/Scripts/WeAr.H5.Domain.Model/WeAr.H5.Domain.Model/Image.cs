namespace WeAr.H5.Domain.Model
{
    public class Image : Entity
    {
        public int ContentId { get; set; }
        public byte[] Value { get; set; }
        public string ImageType { get; set; }
        public virtual Content Content { get; set; }
    }
}
