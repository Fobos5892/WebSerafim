using System;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using SerafimeWeb.Models;

namespace SerafimeWeb.Controllers
{
    public class HomeController : Controller
    {
        private readonly UserManager<User> _userManager;
        private readonly SignInManager<User> _signInManager;
        private readonly RoleManager<IdentityRole> _roleManager;

        public HomeController(UserManager<User> userManager, SignInManager<User> signInManager, RoleManager<IdentityRole> roleManager)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _roleManager = roleManager;
        }

        //[Authorize(Roles = "admin, user")]
        public async Task<IActionResult> Index()
        {
            //AddUser();
            var currentUser = await _userManager.GetUserAsync(User);
            return View(currentUser);
        }

        public IActionResult OnAlarm()
        {
            //            _imageService.Alarm =true;

            return RedirectToAction("Index");
        }

        public async void AddUser()
        {
            var AdminRole = new IdentityRole { Name = "admin", NormalizedName = "ADMIN" };
            var UserRole = new IdentityRole { Name = "user", NormalizedName = "USER" };

            // добавляем роли в бд
            await _roleManager.CreateAsync(AdminRole);
            await _roleManager.CreateAsync(UserRole);

            await _roleManager.AddClaimAsync(AdminRole, new Claim(ClaimTypes.Name, AdminRole.Name));
            await _roleManager.AddClaimAsync(UserRole, new Claim(ClaimTypes.Name, UserRole.Name));

            User userAdmin = new User
            {
                UserName = "Roman_cezar@mail.ru",
                FullName = "Roman_cezar@mail.ru",
                Email = "Roman_cezar@mail.ru",
                Birthdate = Convert.ToDateTime("23.07.1992"),
                Address = "г.Пенза, ул.Калинина д.104А кв.3",
                PhoneNumber = "+79023540178",
                PhoneNumberConfirmed = true,
                EmailConfirmed = true
            };
            string password = "C@e$ar0O792!";
            await _userManager.CreateAsync(userAdmin, password);

            await _userManager.AddToRoleAsync(userAdmin, AdminRole.Name);
            await _userManager.AddClaimAsync(userAdmin, new Claim(ClaimTypes.Email, userAdmin.Email));

            User test_user = new User
            {
                FullName = "Fobos5892@gmail.com",
                UserName = "user",
                Email = "Fobos5892@gmail.com",
                Birthdate = Convert.ToDateTime("23.07.1992"),
                Address = "г.Пенза, ул.Калинина д.104А кв.3",
                PhoneNumber = "+79023540178",
                PhoneNumberConfirmed = true,
            };
            password = "C@e$ar0O792!";

            await _userManager.CreateAsync(test_user, password);
            await _userManager.AddToRoleAsync(test_user, UserRole.Name);
            await _userManager.AddClaimAsync(test_user, new Claim(ClaimTypes.Email, test_user.Email));
            
        }
    }
}
