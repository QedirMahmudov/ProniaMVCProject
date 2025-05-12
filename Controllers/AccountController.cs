using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using ProniaMVCProject.Models;
using ProniaMVCProject.ViewModels;

namespace ProniaMVCProject.Controllers
{
    public class AccountController : Controller
    {
        private readonly UserManager<AppUser> _userManager;
        private readonly SignInManager<AppUser> _signInManager;

        public AccountController(UserManager<AppUser> userManager, SignInManager<AppUser> signInManager)
        {
            _userManager = userManager;
            _signInManager = signInManager;
        }
        public IActionResult Register()
        {
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> Register(RegisterVM registerVM)
        {
            if (!ModelState.IsValid) return View();

            AppUser user = new AppUser
            {
                Name = registerVM.Name,
                Surname = registerVM.Surname,
                UserName = registerVM.UserName,
                Email = registerVM.Email
            };

            //Password hashlayir, ve usera menimsedir.
            IdentityResult result = await _userManager.CreateAsync(user, registerVM.Password);
            //verdiyimiz sertlerin odendiyini yoxlayir.
            if (!result.Succeeded)
            {
                foreach (var error in result.Errors)
                {
                    //addmodelerrorun keyi bos string gonderirik deye, errorlarin hamsini div summary icinde yazdiracaq. (Createde elave olunan dive)
                    ModelState.AddModelError(string.Empty, error.Description);
                }
                return View();
            }
            //registerden sonra istifadecini avtomatik login eletdiririk deye, IsPersistent ona gore false olur.
            //Yeniki istifadeci oz rizasi olmadan passwordu yaddasda saxlamamaliyiq deye false veririk. (Login zamani,Remember me qutusu eyni Mentiq)
            await _signInManager.SignInAsync(user, isPersistent: false);



            return RedirectToAction(nameof(HomeController.Index), "Home");
        }
    }
}
