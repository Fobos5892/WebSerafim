using System;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using SerafimeWeb.Models;
using SerafimeWeb.ViewModels;

namespace SerafimeWeb.Controllers
{
    public class EditController : Controller
    {
        private readonly UserManager<User> _userManager;
        private readonly SignInManager<User> _signInManager;
        private readonly RoleManager<IdentityRole> _roleManager;

        public EditController(UserManager<User> userManager, SignInManager<User> signInManager, RoleManager<IdentityRole> roleManager)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _roleManager = roleManager;
        }

        [Authorize(Roles = "admin")]
        public async Task<IActionResult> Edit(string id)
        {
            var user = await _userManager.FindByIdAsync(id);
            if (user == null) return NotFound();

            var model = new EditViewModel
            {
                Id = user.Id,
                UserName = user.UserName,
                Email = user.Email,
                FullName = user.FullName,
                Birthdate = user.Birthdate,
                PhoneNumber = user.PhoneNumber,
                Address = user.Address
                
            };
            return View(model);
        }
        [HttpPost]
        [Authorize(Roles = "admin")]
        public async Task<IActionResult> Edit(EditViewModel model, string roleString)
        {
            if (ModelState.IsValid)
            {
                var user = await _userManager.FindByIdAsync(model.Id);

                if (user != null)
                {
                    if ((!String.IsNullOrEmpty(model.NewPassword)) && (!String.IsNullOrEmpty(model.NewPasswordConfirm)))
                    {
                        var passwordValidator = HttpContext.RequestServices.GetService(typeof(IPasswordValidator<User>)) as IPasswordValidator<User>;
                        var passwordHasher = HttpContext.RequestServices.GetService(typeof(IPasswordHasher<User>)) as IPasswordHasher<User>;

                        IdentityResult passResult = await passwordValidator.ValidateAsync(_userManager, user, model.NewPassword);
                        if (passResult.Succeeded)
                        {
                            user.PasswordHash = passwordHasher.HashPassword(user, model.NewPassword);

                        }
                        else
                        {
                            ModelState.AddModelError("", "Пароль введен некорректно, проверьте правильность введенных данных");
                            return View(model);
                        }
                    }
                }

                user.UserName = model.UserName;
                user.FullName = model.FullName;
                user.Email = model.Email;
                user.EmailConfirmed = true;
                user.PhoneNumber = model.PhoneNumber;
                user.PhoneNumberConfirmed = true;
                user.Birthdate = model.Birthdate;
                user.Address = model.Address;
                user.Id = model.Id;

                var result = await _userManager.UpdateAsync(user);

                if (result.Succeeded)
                {
                    
                    if (!string.IsNullOrEmpty(roleString))
                    {
                        var oldRole = await _userManager.GetRolesAsync(user);
                        await _userManager.RemoveFromRolesAsync(user, oldRole);
                        if (roleString == "0")
                        {
                            await _userManager.AddToRoleAsync(user, "admin");
                        }
                        else
                        {
                            await _userManager.AddToRoleAsync(user, "user");

                        }

                        var oldClaims = await _userManager.GetClaimsAsync(user);

                        await _userManager.RemoveClaimsAsync(user, oldClaims);
                        await _userManager.AddClaimAsync(user, new Claim(ClaimTypes.Email, user.Email));
                    }

                    ViewBag.Notification = "Данные пользователя обновлены!";
                    return View(model);
                }

                foreach (var error in result.Errors) ModelState.AddModelError(string.Empty, error.Description);

            }

            return View(model);
        }
      
    }
}