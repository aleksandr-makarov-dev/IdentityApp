using IdentityApp.Core.Extensions;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace IdentityApp.Pages.Identity
{
    public class UserTwoFactorManageModel : UserPageModel
    {
        private readonly UserManager<IdentityUser> _userManager;
        private readonly SignInManager<IdentityUser> _signInManager;

        public UserTwoFactorManageModel(UserManager<IdentityUser> userManager, SignInManager<IdentityUser> signInManager)
        {
            _userManager = userManager;
            _signInManager = signInManager;
        }

        public IdentityUser IdentityUser { get; private set; }
        public async Task<bool> IsTwoFactorEnabled() => await _userManager.GetTwoFactorEnabledAsync(IdentityUser);

        public async Task OnGetAsync()
        {
            IdentityUser = await _userManager.GetUserAsync(User);
        }

        public async Task<IActionResult> OnPostDisableAsync()
        {
            IdentityUser = await _userManager.GetUserAsync(User);

            IdentityResult setTwoFactorResult = await _userManager.SetTwoFactorEnabledAsync(IdentityUser, false);

            if (setTwoFactorResult.Process(ModelState))
            {
                await _signInManager.SignOutAsync();

                return RedirectToPage("Index", new { });
            }

            return Page();
        }

        public async Task<IActionResult> OnPostGenerateCodesAsync()
        {
            IdentityUser = await _userManager.GetUserAsync(User);

            TempData["RecoveryCodes"] = await _userManager.GenerateNewTwoFactorRecoveryCodesAsync(IdentityUser, 10);

            return RedirectToPage("UserRecoveryCodes");
        }
    }
}
