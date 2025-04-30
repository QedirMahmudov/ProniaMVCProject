using ProniaMVCProject.Models;


namespace ProniaMVCProject.ViewModels
{
    public class DetailVM
    {
        public Product Product { get; set; }
        public List<Product> RelatedProducts { get; set; }
    }
}
