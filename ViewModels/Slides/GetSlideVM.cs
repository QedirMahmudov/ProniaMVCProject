using System.ComponentModel.DataAnnotations;

namespace ProniaMVCProject.ViewModels
{
    public class GetSlideVM
    {
        public int Id { get; set; }
        [MaxLength(25)]
        public string Title { get; set; }
        public int Order { get; set; }
        public string Image { get; set; }
        public DateTime CreatedAt { get; set; }
    }
}
