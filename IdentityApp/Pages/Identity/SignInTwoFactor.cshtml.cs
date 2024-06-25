using System.ComponentModel.DataAnnotations;
using System.Text.RegularExpressions;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using SignInResult = Microsoft.AspNetCore.Identity.SignInResult;

namespace IdentityApp.Pages.Identity
{
    [AllowAnonymous]
    public class SignInTwoFactorModel : UserPageModel
    {
        private readonly UserManager<IdentityUser> _userManager;
        private readonly SignInManager<IdentityUser> _signInManager;

        public SignInTwoFactorModel(UserManager<IdentityUser> userManager, SignInManager<IdentityUser> signInManager)
        {
            _userManager = userManager;
            _signInManager = signInManager;
        }

        [BindProperty]
        public string? ReturnUrl { get; set; }

        [BindProperty]
        [Required]
        public string Token { get; set; } = string.Empty;

        [BindProperty] 
        public bool RememberMe { get; set; } = false;

        public async Task<IActionResult> OnPostAsync()
        {
            if (ModelState.IsValid)
            {
                IdentityUser? user = await _signInManager.GetTwoFactorAuthenticationUserAsync();

                if (user is not null)
                {
                    string token = Regex.Replace(Token, @"\s", "");

                    SignInResult signInResult =
                        await _signInManager.TwoFactorAuthenticatorSignInAsync(token, true, RememberMe);

                    if (!signInResult.Succeeded)
                    {
                        signInResult = await _signInManager.TwoFactorRecoveryCodeSignInAsync(token);
                    }

                    if (signInResult.Succeeded)
                    {
                        if (await _userManager.CountRecoveryCodesAsync(user) <= 3)
                        {
                            return RedirectToPage("SignInCodesWarning");
                        }

                        return Redirect(ReturnUrl ?? "/");
                    }
                }

                ModelState.AddModelError(string.Empty,"Invalid Token or Recovery Code");
            }

            return Page();
        }
    }
}
