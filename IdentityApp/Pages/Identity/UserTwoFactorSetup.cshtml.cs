using System.ComponentModel.DataAnnotations;
using System.Text.RegularExpressions;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace IdentityApp.Pages.Identity
{
    public class UserTwoFactorSetupModel : UserPageModel
    {
        private readonly UserManager<IdentityUser> _userManager;
        private readonly SignInManager<IdentityUser> _signInManager;

        public UserTwoFactorSetupModel(UserManager<IdentityUser> userManager, SignInManager<IdentityUser> signInManager)
        {
            _userManager = userManager;
            _signInManager = signInManager;
        }

        public IdentityUser IdentityUser { get; private set; }
        public string AuthenticatorKey { get; private set; }
        public string QrCodeUrl { get; private set; }

        public async Task<IActionResult> OnGetAsync()
        {
            await LoadAuthenticatorKeysAsync();

            // if enabled redirect to two factor manage page

            if (await _userManager.GetTwoFactorEnabledAsync(IdentityUser))
            {
                return RedirectToPage("UserTwoFactorManage");
            }

            return Page();
        }

        public async Task<IActionResult> OnPostConfirmAsync([Required] string confirm)
        {
            await LoadAuthenticatorKeysAsync();

            if (ModelState.IsValid)
            {
                // clear input from spaces
                string token = Regex.Replace(confirm, @"\s", "");

                bool isCodeValid = await _userManager.VerifyTwoFactorTokenAsync(
                    IdentityUser,
                    _userManager.Options.Tokens.AuthenticatorTokenProvider,
                    token
                    );

                if (isCodeValid)
                {
                    TempData["RecoveryCodes"] =
                        await _userManager.GenerateNewTwoFactorRecoveryCodesAsync(IdentityUser, 10);

                    await _userManager.SetTwoFactorEnabledAsync(IdentityUser, true);
                    await _signInManager.RefreshSignInAsync(IdentityUser);

                    return RedirectToPage("UserRecoveryCodes");
                }
                else
                {
                    ModelState.AddModelError(string.Empty,"Confirmation code is invalid");
                }
            }

            return Page();
        }

        private async Task LoadAuthenticatorKeysAsync()
        {
            IdentityUser = await _userManager.GetUserAsync(User);

            AuthenticatorKey = await _userManager.GetAuthenticatorKeyAsync(IdentityUser);
            if (AuthenticatorKey == null)
            {
                await _userManager.ResetAuthenticatorKeyAsync(IdentityUser);
                AuthenticatorKey = await _userManager.GetAuthenticatorKeyAsync(IdentityUser);

                await _signInManager.RefreshSignInAsync(IdentityUser);
            }

            QrCodeUrl = $"otpauth://totp/IdentityApp:{IdentityUser.Email}?secret={AuthenticatorKey}";
        }
    }
}
