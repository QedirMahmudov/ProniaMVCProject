using System.ComponentModel.DataAnnotations;

namespace ProniaMVCProject.Models
{
    public class Size : BaseEntity
    {
        [MaxLength(50)]
        public string Name { get; set; }
    }
}
