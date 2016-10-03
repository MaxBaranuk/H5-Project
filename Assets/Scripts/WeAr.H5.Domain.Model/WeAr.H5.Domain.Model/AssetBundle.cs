namespace WeAr.H5.Domain.Model
{   
    public class AssetBundle
    {
        public int Id { get; set; }
        public int CoreUnitId { get; set; }
        public byte[] Value { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
    
        public virtual CoreUnit CoreUnit { get; set; }
    }
}
