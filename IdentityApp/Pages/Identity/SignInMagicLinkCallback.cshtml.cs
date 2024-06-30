using System.ComponentModel.DataAnnotations;
using System.Security.Claims;
using IdentityApp.Services;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace IdentityApp.Pages.Identity
{
    [AllowAnonymous]
    public class SignInMagicLinkCallbackModel : UserPageModel
    {
        private readonly UserManager<IdentityUser> _userManager;
        private readonly SignInManager<IdentityUser> _signInManager;
        private readonly TokenUrlEncoderService _tokenUrlEncoderService;

        public SignInMagicLinkCallbackModel(UserManager<IdentityUser> userManager, SignInManager<IdentityUser> signInManager, TokenUrlEncoderService tokenUrlEncoderService)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _tokenUrlEncoderService = tokenUrlEncoderService;
        }

        [Required]
        [EmailAddress]
        [BindProperty(SupportsGet = true)]
        public string Email { get; set; } = string.Empty;
        [Required]
        [BindProperty(SupportsGet = true)]
        public string Token { get; set; } = string.Empty;

        public async Task<IActionResult> OnGetAsync()
        {
            if (ModelState.IsValid)
            {
                IdentityUser? user = await _userManager.FindByEmailAsync(Email);

                if (user != null)
                {
                    string decodedToken = _tokenUrlEncoderService.DecodeToken(Token);

                    bool isValid = await _userManager.VerifyUserTokenAsync(user,"Default","passwordless-auth", decodedToken);

                    if (isValid)
                    {
                        await _userManager.UpdateSecurityStampAsync(user);

                        ClaimsIdentity? identity = (await _signInManager.CreateUserPrincipalAsync(user)).Identities.FirstOrDefault();

                        await HttpContext.SignInAsync(IdentityConstants.ApplicationScheme,new ClaimsPrincipal(identity));

                        return Redirect("/");
                    }
                }

                TempData["message"] = "Invalid user or token";
            }

            return Page();
        }
    }
}
