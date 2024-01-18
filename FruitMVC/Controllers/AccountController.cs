using FruitMVC.DAL;
using FruitMVC.Helpers;
using FruitMVC.Models;
using FruitMVC.ViewModels;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace FruitMVC.Controllers
{
    public class AccountController : Controller
    {
        private readonly RoleManager<IdentityRole> roleManager;
        private readonly UserManager<AppUser> userManager;
        private readonly SignInManager<AppUser> signInManager;


        public AccountController(RoleManager<IdentityRole> roleManager, UserManager<AppUser> userManager, SignInManager<AppUser> signInManager)
        {
            this.roleManager = roleManager;
            this.userManager = userManager;
            this.signInManager = signInManager;
        }
        public IActionResult Register()
        {
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> Register(RegisterVM registerVM)
        {
            if (!ModelState.IsValid)
            {
                return View();
            }

            AppUser appUser = new AppUser()
            {
                Name = registerVM.Name,
                Surname = registerVM.Surname,
                Email = registerVM.Email,
                UserName = registerVM.UserName
            };
            var result = await userManager.CreateAsync(appUser, registerVM.Password);
            if (!result.Succeeded)
            {
                foreach (var item in result.Errors)
                {
                    ModelState.AddModelError("", item.Description);
                }
            }
            await userManager.AddToRoleAsync(appUser,UserRole.Admin.ToString());
            return RedirectToAction(nameof(Login));
        }

        public IActionResult Login()
        {
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> Login(LoginVM loginVM)
        {
            var user = await userManager.FindByEmailAsync(loginVM.UserOrEmail);

            if (user == null)
            {
                 user = await userManager.FindByNameAsync(loginVM.UserOrEmail);
                if (user == null) throw new Exception("User or Email isn't correct");

            }

            var result = await signInManager.CheckPasswordSignInAsync(user, loginVM.Password,false);

            if (!result.Succeeded) throw new Exception("UserName/Emaiil or Password");

            await signInManager.SignInAsync(user, false);

            return RedirectToAction("Index","Home");
        }

        public async Task<IActionResult> CreateRole()
        {
            foreach(var item in Enum.GetValues(typeof(UserRole)))
            {
                if (await roleManager.FindByNameAsync (item.ToString())== null)
                {
                    await roleManager.CreateAsync(new IdentityRole()
                    {
                        Name = item.ToString()
                    });

                }
            }
            return RedirectToAction("Index", "Home");

        } 

        public IActionResult LogOut()
        {
            signInManager.SignOutAsync();
            return RedirectToAction("Index","Home");
        }
    }
}
