namespace ProniaMVCProject.Models
{
    public class Product : BaseEntity
    {




        public string Name { get; set; }
        public string Description { get; set; }
        public string SKU { get; set; }
        public decimal Price { get; set; }

        //reletional properties
        public int CategoryId { get; set; }
        public Category Category { get; set; }
        public List<ProductImage> ProductImage { get; set; }
    }
}
