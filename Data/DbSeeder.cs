using ByteInoTaskManager.Models;
using Microsoft.AspNetCore.Identity;

namespace ByteInoTaskManager.Data
{
    public static class DbSeeder
    {
        public static async Task SeedAdminAsync(UserManager<ApplicationUser> userManager, RoleManager<IdentityRole> roleManager)
        {
          
            if (!await roleManager.RoleExistsAsync("Admin"))
                await roleManager.CreateAsync(new IdentityRole("Admin"));

            if (!await roleManager.RoleExistsAsync("User"))
                await roleManager.CreateAsync(new IdentityRole("User"));

          
            var adminEmail = "ad@gmail.com";
            var admin = await userManager.FindByEmailAsync(adminEmail);

            if (admin == null)
            {
                var newAdmin = new ApplicationUser
                {
                    UserName = "ad@gmail.com",
                    Email = adminEmail,
                    EmailConfirmed = true
                };

                var result = await userManager.CreateAsync(newAdmin, "admin@123");
                if (result.Succeeded)
                {
                    await userManager.AddToRoleAsync(newAdmin, "Admin");
                }
            }
        }
    }
}
