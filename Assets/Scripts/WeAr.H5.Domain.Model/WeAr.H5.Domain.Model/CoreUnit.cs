using WeAr.H5.Domain.Model.Enums;

namespace WeAr.H5.Domain.Model
{
    public class CoreUnit
    {
        public int Id { get; set; }
        public string Name { get; set; }

        public int ObjectItemId { get; set; }

        public int ViewsCount { get; set; }
    
        public virtual EUnitType UnitType { get; set; }
        public virtual ObjectItem ObjectItem { get; set; }
    }
}
