using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ProniaMVCProject.Models;
using ProniaMVCProject.Utilities.Enums;
using ProniaMVCProject.ViewModels;

namespace ProniaMVCProject.Controllers
{
    public class AccountController : Controller
    {
        private readonly UserManager<AppUser> _userManager;
        private readonly SignInManager<AppUser> _signInManager;
        private readonly RoleManager<IdentityRole> _roleManager;

        public AccountController(
            UserManager<AppUser> userManager,
            SignInManager<AppUser> signInManager,
            RoleManager<IdentityRole> roleManager
            )
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _roleManager = roleManager;
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

            await _userManager.AddToRoleAsync(user, UserRole.Member.ToString());
            await _signInManager.SignInAsync(user, isPersistent: false);



            return RedirectToAction(nameof(HomeController.Index), "Home");
        }


        public IActionResult Login()
        {
            return View();
        }
        [HttpPost]

        public async Task<IActionResult> Login(LoginVM loginVM, string? returnUrl)
        {
            if (!ModelState.IsValid) return View();

            AppUser user = await _userManager.Users.FirstOrDefaultAsync(u => u.UserName == loginVM.UserNameOrEmail || u.Email == loginVM.UserNameOrEmail);
            if (user == null)
            {
                ModelState.AddModelError(string.Empty, "Username,Email or password is incorrect");
                return View();
            }

            var result = await _signInManager.PasswordSignInAsync(user, loginVM.Password, loginVM.IsPersistent, true);



            if (result.IsLockedOut)
            {
                ModelState.AddModelError(string.Empty, "Account Blocked! Wait 10 minutes!");
                return View();
            }

            if (!result.Succeeded)
            {
                ModelState.AddModelError(string.Empty, "Username,Email or password is incorrect");
                return View();

            }

            if (returnUrl is null)
            {
                return RedirectToAction(nameof(HomeController.Index), "Home");
            }


            return Redirect(returnUrl);





            //AppUser user = await _userManager.FindByNameAsync(loginVM.UserNameOrEmail);

            //if (user == null)
            //{
            //    user = await _userManager.FindByEmailAsync(loginVM.UserNameOrEmail);
            //    if (user is null)
            //    {

            //    }
            //}

        }
        public async Task<IActionResult> Logout()
        {
            await _signInManager.SignOutAsync();
            return RedirectToAction(nameof(HomeController.Index), "Home");


        }


        public async Task<IActionResult> CreateRoles()
        {
            foreach (UserRole role in Enum.GetValues(typeof(UserRole)))
            {
                if (!await _roleManager.RoleExistsAsync(role.ToString()))
                {
                    await _roleManager.CreateAsync(new IdentityRole
                    {
                        Name = role.ToString()
                    });
                }
            }
            return RedirectToAction(nameof(HomeController.Index), "Home");
        }
    }
}
