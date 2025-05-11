using ProniaMVCProject.Models;
using System.ComponentModel.DataAnnotations;

namespace ProniaMVCProject.ViewModels
{
    public class UpdateProductVM
    {
        public string Name { get; set; }
        public string Description { get; set; }
        public string SKU { get; set; }
        [Required]
        public decimal? Price { get; set; }

        //reletional properties
        [Required]
        public int? CategoryId { get; set; }
        public string PrimaryImage { get; set; }
        public IFormFile? MainPhoto { get; set; }

        public List<Category>? Categories { get; set; }
    }
}
