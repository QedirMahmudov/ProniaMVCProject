using System.ComponentModel.DataAnnotations;

namespace ProniaMVCProject.Models
{
    public class Category : BaseEntity
    {
        [MaxLength(25, ErrorMessage = "Melikmemmed nagli danismirsan ad qoyursan.")]
        public string Name { get; set; }

        //reletional property

        #region Niye List<Product>?
        //? isaresinin sebebi, CategoryControllerde Create actionunun post methodunda, ModelState.IsValid ile yoxlama apararken Tek yoxlama apracaqimiz Properti Name Olduqu
        //Ucundur
        #endregion
        public List<Product>? Products { get; set; }
    }
}
