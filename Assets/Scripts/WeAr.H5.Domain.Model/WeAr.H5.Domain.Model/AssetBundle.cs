namespace WeAr.H5.Domain.Model
{   
    public class AssetBundle : Entity
    {
        public int ContentId { get; set; }
        public byte[] Value { get; set; }
        public virtual Content Content { get; set; }
    }
}
