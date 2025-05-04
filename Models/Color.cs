using System.ComponentModel.DataAnnotations;

namespace ProniaMVCProject.Models
{
    public class Color : BaseEntity
    {
        [MaxLength(50)]
        [MinLength(2, ErrorMessage = "Minimum 2 herf olmalidir!")]
        public string Name { get; set; }
    }
}
