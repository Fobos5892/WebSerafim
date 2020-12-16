using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using SerafimeWeb.Models;
using System.Threading.Tasks;

namespace SerafimWeb.Models
{
    public class IndetitySeedData
    {
        private const string adminUser = "admin";
        private const string adminEmail = "admin";
        private const string adminPassword = "123456";
        public static async Task EnsurePopulated(IServiceScope serviceScope, UserManager<User> userManager, RoleManager<IdentityRole> rolesManager)
        {
            serviceScope.ServiceProvider.GetRequiredService<ApplicationContext>().Database.Migrate();

            if (await rolesManager.FindByNameAsync("admin") == null)
                await rolesManager.CreateAsync(new IdentityRole("admin"));

            if (await rolesManager.FindByNameAsync("operator") == null)
                await rolesManager.CreateAsync(new IdentityRole("operator"));

            if (await rolesManager.FindByNameAsync("user") == null)
                await rolesManager.CreateAsync(new IdentityRole("user"));


            if (await userManager.FindByNameAsync(adminUser) == null)
            {
                var admin = new User { Email = adminEmail, UserName = adminUser, FullName = "admin" };
                var result = await userManager.CreateAsync(admin, adminPassword);
                if (result.Succeeded) await userManager.AddToRoleAsync(admin, "admin");
            }
        }
    }
}
