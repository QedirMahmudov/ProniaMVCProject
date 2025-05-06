using System.ComponentModel.DataAnnotations;

namespace ProniaMVCProject.ViewModels
{
    public class CreateSlideVM
    {
        [MaxLength(25)]
        public string Title { get; set; }
        public string SubTitle { get; set; }
        public string Description { get; set; }
        public int Order { get; set; }
        public IFormFile Photo { get; set; }
    }
}
