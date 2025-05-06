using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;

namespace ProniaMVCProject.Models
{
    [Index(nameof(Order), IsUnique = true)]
    public class Slide : BaseEntity
    {
        [MaxLength(25)]
        public string Title { get; set; }
        [MaxLength(50)]
        public string SubTitle { get; set; }
        [MaxLength(1000)]
        public string Description { get; set; }
        public string Image { get; set; }
        public int Order { get; set; }


    }
}
