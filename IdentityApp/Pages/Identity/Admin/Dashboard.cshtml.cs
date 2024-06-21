using IdentityApp.Core.Configurations;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using IdentityApp.Core.Extensions;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;

namespace IdentityApp.Pages.Identity.Admin {

    public class DashboardModel : AdminPageModel
    {
        private readonly IdentityInitializeOptions _options;
        public DashboardModel(UserManager<IdentityUser> userMgr, IOptions<IdentityInitializeOptions> options)
        {
            UserManager = userMgr;
            _options = options.Value;
        }

        public UserManager<IdentityUser> UserManager { get; set; }

        public int UsersCount { get; set; } = 0;
        public int UsersUnconfirmed { get; set; } = 0;
        public int UsersLockedout { get; set; } = 0;
        public int UsersTwoFactor { get; set; } = 0;

        private readonly string[] emails = {
            "alice@example.com", "bob@example.com", "charlie@example.com"
        };

        public async Task OnGetAsync()
        {

            var utcNow = DateTimeOffset.UtcNow;

            UsersCount = await UserManager.Users.CountAsync();
            UsersUnconfirmed = await UserManager.Users.CountAsync(u => !u.EmailConfirmed);

            UsersLockedout = await UserManager.Users
                .CountAsync(u => u.LockoutEnabled && u.LockoutEnd > utcNow);
        }

        public async Task<IActionResult> OnPostAsync() {
            foreach (IdentityUser existingUser in UserManager.Users.ToList()) {
                if (emails.Contains(existingUser.Email) ||
                    !await UserManager.IsInRoleAsync(existingUser, _options.Role))
                {
                    IdentityResult result = await UserManager.DeleteAsync(existingUser);
                    result.Process(ModelState);
                }
            }
            foreach (string email in emails) {
                IdentityUser userObject = new IdentityUser {
                    UserName = email,
                    Email = email,
                    EmailConfirmed = true
                };
                IdentityResult result = await UserManager.CreateAsync(userObject);

                if (result.Process(ModelState))
                {
                    result = await UserManager.AddPasswordAsync(userObject, "mysecret");
                    result.Process(ModelState);
                }
            }
            if (ModelState.IsValid) {
                return RedirectToPage();
            }
            return Page();
        }
    }
}
