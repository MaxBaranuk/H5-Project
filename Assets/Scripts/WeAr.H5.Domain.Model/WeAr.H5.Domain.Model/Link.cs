namespace WeAr.H5.Domain.Model
{
    public class Link : Entity
    {
        public int ContentId { get; set; }
        public string Value { get; set; }   
        public virtual Content Content { get; set; }
    }
}
