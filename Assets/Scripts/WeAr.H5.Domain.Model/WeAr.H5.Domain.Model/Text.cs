namespace WeAr.H5.Domain.Model
{
    public class Text
    {
        public int Id { get; set; }
        public int CoreUnitId { get; set; }
        public string Value { get; set; }
    
        public virtual CoreUnit CoreUnit { get; set; }
    }
}
