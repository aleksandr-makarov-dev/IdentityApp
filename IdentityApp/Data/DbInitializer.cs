using IdentityApp.Core.Configurations;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Options;

namespace IdentityApp.Data
{
    public static class DbInitializer
    {
        public static void SeedIdentityUsers(this WebApplication app)
        {
            SeedIdentityUsersAsync(app).Wait();
        }
        private static async Task SeedIdentityUsersAsync(WebApplication app)
        {
            using (var scope = app.Services.CreateScope())
            {
                IdentityInitializeOptions options = scope
                    .ServiceProvider
                    .GetRequiredService<IOptions<IdentityInitializeOptions>>().Value;

                UserManager<IdentityUser> userManager = scope
                    .ServiceProvider
                    .GetRequiredService<UserManager<IdentityUser>>();
                RoleManager<IdentityRole> roleManager = scope
                    .ServiceProvider
                    .GetRequiredService<RoleManager<IdentityRole>>();


                string role = options.Role;
                string email = options.Email;
                string password = options.Password;

                if (!await roleManager.RoleExistsAsync(role))
                {
                    await roleManager.CreateAsync(new IdentityRole(role));
                }

                IdentityUser? foundUser = await userManager.FindByEmailAsync(email);

                if (foundUser is null)
                {
                    IdentityUser defaultUser = new IdentityUser
                    {
                        Email = email,
                        UserName = email,
                        EmailConfirmed = true
                    };

                    await userManager.CreateAsync(defaultUser, password);

                    foundUser = await userManager.FindByEmailAsync(email);
                }

                if (!await userManager.IsInRoleAsync(foundUser, role))
                {
                    await userManager.AddToRoleAsync(foundUser, role);
                }
            }
        }
    }
}
