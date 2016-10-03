namespace WeAr.H5.Domain.Model
{
    public class Link
    {
        public int Id { get; set; }
        public int CoreUnitId { get; set; }
        public string Value { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
    
        public virtual CoreUnit CoreUnit { get; set; }
    }
}
