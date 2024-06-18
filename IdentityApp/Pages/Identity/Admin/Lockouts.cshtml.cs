using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;

namespace IdentityApp.Pages.Identity.Admin
{
    public class LockoutsModel : AdminPageModel
    {
        private readonly UserManager<IdentityUser> _userManager;

        public LockoutsModel(UserManager<IdentityUser> userManager)
        {
            _userManager = userManager;
        }

        public IEnumerable<IdentityUser> LockedOutUsers { get; set; }
        public IEnumerable<IdentityUser> OtherUsers { get; set; }

        public async Task<TimeSpan> TimeLeft(IdentityUser user)
        {
            DateTimeOffset? lockoutEndDate = await _userManager.GetLockoutEndDateAsync(user);

            return lockoutEndDate.GetValueOrDefault().Subtract(DateTimeOffset.Now);
        }

        public async Task OnGetAsync()
        {
            var utcNow = DateTimeOffset.UtcNow;

            LockedOutUsers =
                await _userManager
                    .Users
                    .Where(u => u.LockoutEnd != null && u.LockoutEnd> utcNow)
                    .OrderBy(u=>u.Email)
                    .ToListAsync();

            OtherUsers = await _userManager
                .Users.Where(u => u.LockoutEnd == null || u.LockoutEnd <= utcNow)
                .OrderBy(u => u.Email)
                .ToListAsync();
        }

        public async Task<IActionResult> OnPostLockAsync(string id)
        {
            IdentityUser? foundUser = await _userManager.FindByIdAsync(id);

            if (foundUser is not null)
            {
                await _userManager.SetLockoutEnabledAsync(foundUser, true);
                await _userManager.SetLockoutEndDateAsync(foundUser, DateTimeOffset.Now.AddDays(5));
                await _userManager.UpdateSecurityStampAsync(foundUser);
            }

            return RedirectToPage();
        }

        public async Task<IActionResult> OnPostUnlockAsync(string id)
        {
            IdentityUser? foundUser = await _userManager.FindByIdAsync(id);

            if (foundUser is not null)
            {
                await _userManager.SetLockoutEndDateAsync(foundUser,null);
            }

            return RedirectToPage();
        }
    }
}
