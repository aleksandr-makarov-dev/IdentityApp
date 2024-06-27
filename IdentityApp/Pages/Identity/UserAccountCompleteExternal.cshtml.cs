using System.Security.Claims;
using IdentityApp.Services;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace IdentityApp.Pages.Identity
{
    [AllowAnonymous]
    public class UserAccountCompleteExternalModel : UserPageModel
    {
        private readonly UserManager<IdentityUser?> _userManager;
        private readonly SignInManager<IdentityUser> _signInManager;
        private readonly TokenUrlEncoderService _tokenUrlEncoderService;

        public UserAccountCompleteExternalModel(UserManager<IdentityUser?> userManager, SignInManager<IdentityUser> signInManager, TokenUrlEncoderService tokenUrlEncoderService)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _tokenUrlEncoderService = tokenUrlEncoderService;
        }

        [BindProperty(SupportsGet = true)]
        public string Email { get; set; } = string.Empty;

        [BindProperty(SupportsGet = true)] 
        public string Token { get; set; } = string.Empty;

        public IdentityUser? IdentityUser { get; set; }

        public async Task<string> ExternalProvider() =>
            (await _userManager.GetLoginsAsync(IdentityUser))
            .FirstOrDefault()?.ProviderDisplayName;

        public async Task<IActionResult> OnPostAsync(string provider)
        {
            IdentityUser = await _userManager.FindByEmailAsync(Email);
            string decodedToken = _tokenUrlEncoderService.DecodeToken(Token);

            bool isTokenValid = await _userManager
                .VerifyUserTokenAsync(
                    IdentityUser,
                    _userManager.Options.Tokens.PasswordResetTokenProvider,
                    UserManager<IdentityUser>.ResetPasswordTokenPurpose,
                    decodedToken
                );

            if (!isTokenValid)
            {
                return Error("Invalid token");
            }

            string callbackUrl = Url.Page(
                "UserAccountCompleteExternal", 
                "Callback", 
                new { Email, Token });

            AuthenticationProperties props = _signInManager.ConfigureExternalAuthenticationProperties(
                provider, callbackUrl
                );

            return new ChallengeResult(provider, props);
        }

        public async Task<IActionResult> OnGetCallbackAsync()
        {
            ExternalLoginInfo? info = await _signInManager.GetExternalLoginInfoAsync();

            if (info is null)
            {
                return Error("Could not get external info");
            }

            string? email = info?.Principal.FindFirstValue(ClaimTypes.Email);

            if (string.IsNullOrEmpty(email))
            {
                return Error("External service has not provided an email address.");
            }

            if ((IdentityUser = await _userManager.FindByEmailAsync(email)) is null)
            {
                return Error("Your email address does not match.");
            }

            IdentityResult addLoginResult = await _userManager.AddLoginAsync(IdentityUser, info);

            if (!addLoginResult.Succeeded)
            {
                return Error("Cannot store external login.");
            }

            return RedirectToPage(new { id = IdentityUser.Id });
        }

        public async Task<IActionResult> OnGetAsync(string? id)
        {
            if (
                (string.IsNullOrEmpty(id) || 
                (IdentityUser = await _userManager.FindByIdAsync(id)) is null) &&
                !TempData.ContainsKey("errorMessage"))
            {
                return RedirectToPage("SignIn");
            }

            return Page();
        }

        private IActionResult Error(string err)
        {
            TempData["errorMessage"] = err;
            return RedirectToPage();
        }
    }
}
