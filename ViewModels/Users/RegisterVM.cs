using System.ComponentModel.DataAnnotations;

namespace ProniaMVCProject.ViewModels
{
    public class RegisterVM
    {
        public string Name { get; set; }
        public string Surname { get; set; }
        [MaxLength(100)]
        public string UserName { get; set; }
        [MaxLength(100)]
        [DataType(DataType.EmailAddress)]
        public string Email { get; set; }
        [DataType(DataType.Password)]
        public string Password { get; set; }
        [DataType(DataType.Password)]
        [Compare(nameof(Password))]
        public string ConfirmPassword { get; set; }
    }
}
