using System.ComponentModel.DataAnnotations;
using System.Security.Claims;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace IdentityApp.Pages.Identity
{
    [AllowAnonymous]
    public class SignUpExternalModel : UserPageModel
    {
        private readonly UserManager<IdentityUser> _userManager;
        private readonly SignInManager<IdentityUser> _signInManager;

        public SignUpExternalModel(UserManager<IdentityUser> userManager, SignInManager<IdentityUser> signInManager)
        {
            _userManager = userManager;
            _signInManager = signInManager;
        }

        public IdentityUser IdentityUser { get; private set; }

        public async Task<string?> ExternalProvider() =>
            (await _userManager.GetLoginsAsync(IdentityUser)).FirstOrDefault()?.ProviderDisplayName;

        public IActionResult OnPost([Required] string provider)
        {
            string callbackUrl = Url.Page("SignUpExternal", "Callback");

            AuthenticationProperties props =
                _signInManager.ConfigureExternalAuthenticationProperties(provider, callbackUrl);

            return new ChallengeResult(provider, props);
        }

        public async Task<IActionResult> OnGetCallbackAsync()
        {
            ExternalLoginInfo info = await _signInManager.GetExternalLoginInfoAsync();

            string? email = info?.Principal.FindFirst(ClaimTypes.Email)?.Value;

            if (string.IsNullOrEmpty(email))
            {
                return Error("External service has not provided an email address.");
            }

            if (await _userManager.FindByEmailAsync(email) is not null)
            {
                return Error("An account already exists with your email address");
            }

            IdentityUser userToCreate = new IdentityUser
            {
                UserName = email,
                Email = email,
                EmailConfirmed = true
            };

            IdentityResult createUserResult = await _userManager.CreateAsync(userToCreate);

            if (!createUserResult.Succeeded)
            {
                return Error("An account could not be created");
            }

            IdentityUser createdUser = await _userManager.FindByEmailAsync(email);

            IdentityResult addLoginResult = await _userManager.AddLoginAsync(createdUser, info);

            if (!addLoginResult.Succeeded)
            {
                return Error("Could not add login info to the user");
            }

            return RedirectToPage(new { id = createdUser.Id });
        }

        public async Task<IActionResult> OnGetAsync(string id)
        {
            if (string.IsNullOrEmpty(id))
            {
                return RedirectToPage("SignUp");
            }

            IdentityUser = await _userManager.FindByIdAsync(id);

            if (IdentityUser is null)
            {
                return RedirectToPage("SignUp");
            }

            return Page();
        }

        private IActionResult Error(string error)
        {
            TempData["errorMessage"] = error;

            return RedirectToPage();
        }
    }
}
