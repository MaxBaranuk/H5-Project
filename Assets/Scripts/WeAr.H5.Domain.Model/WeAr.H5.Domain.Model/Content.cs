using WeAr.H5.Domain.Model.Enums;

namespace WeAr.H5.Domain.Model
{
    public class Content
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }

        public int ObjectItemId { get; set; }

        public int ViewsCount { get; set; }
    
        public EContentType ContentType { get; set; }

        //public virtual ObjectItem ObjectItem { get; set; }
    }
}
