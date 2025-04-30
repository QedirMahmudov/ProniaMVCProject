namespace ProniaMVCProject.Models
{
    public class Category : BaseEntity
    {
        public string Name { get; set; }

        //reletional property
        public List<Product> Products { get; set; }
    }
}
