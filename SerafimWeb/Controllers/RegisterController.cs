using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using SerafimeWeb.Models;
using SerafimeWeb.ViewModels;

namespace SerafimeWeb.Controllers
{
    public class RegisterController : Controller
    {
        private readonly UserManager<User> _userManager;
        private readonly SignInManager<User> _signInManager;

        public RegisterController(UserManager<User> userManager, SignInManager<User> signInManager)
        {
            _userManager = userManager;
            _signInManager = signInManager;
        }
        [HttpGet]
        [Authorize(Roles = "admin")]
        public IActionResult Register()
        {
            return View();
        }
        [HttpPost]
        [Authorize(Roles = "admin")]
        public async Task<IActionResult> Register(RegisterViewModel model, string roleString)
        {
            if (ModelState.IsValid)
            {
                User user = new User
                {
                    UserName = model.UserName,
                    FullName = model.FullName,
                    Email = model.Email,
                    Birthdate = model.Birthdate,
                    Address = model.Address,
                    PhoneNumber = model.PhoneNumber,
                    PhoneNumberConfirmed = true,
                    EmailConfirmed = true
                };

                // добавляем пользователя
                var result = await _userManager.CreateAsync(user, model.Password);

                if (result.Succeeded)
                {
                    if (!string.IsNullOrEmpty(roleString))
                    {
                        if(roleString == "0")
                        {
                            await _userManager.AddToRoleAsync(user, "admin");
                        }
                        else
                        {
                            await _userManager.AddToRoleAsync(user, "user");
                            
                        }
                        await _userManager.AddClaimAsync(user, new Claim(ClaimTypes.Email, user.Email));
                    }

                    return RedirectToAction("Index", "Home");
                }
                else
                {
                    foreach (var error in result.Errors)
                    {
                        ModelState.AddModelError(string.Empty, error.Description);
                    }
                }
            }
            return View(model);
        }
    }
}