namespace WeAr.H5.Domain.Model
{
    public class Marker
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public int ContentId { get; set; }
        public byte[] Image { get; set; }
        public string ImageType { get; set; }
        //todo
        public string VuforiaId { get; set; }
        public virtual Content Content { get; set; }
    }
}
