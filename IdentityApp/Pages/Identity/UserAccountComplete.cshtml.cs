using System.ComponentModel.DataAnnotations;
using IdentityApp.Core.Extensions;
using IdentityApp.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace IdentityApp.Pages.Identity
{
    [AllowAnonymous]
    public class UserAccountCompleteModel : UserPageModel
    {
        private readonly UserManager<IdentityUser> _userManager;
        private readonly TokenUrlEncoderService _tokenUrlEncoderService;

        public UserAccountCompleteModel(UserManager<IdentityUser> userManager, TokenUrlEncoderService tokenUrlEncoderService)
        {
            _userManager = userManager;
            _tokenUrlEncoderService = tokenUrlEncoderService;
        }

        [BindProperty(SupportsGet = true)] 
        public string Token { get; set; } = string.Empty;

        [EmailAddress]
        [BindProperty(SupportsGet = true)]
        public string Email { get; set; } = string.Empty;

        [Required]
        [BindProperty]
        public string Password { get; set; } = string.Empty;

        [Required]
        [Compare(nameof(Password))]
        [BindProperty]
        public string ConfirmPassword { get; set; } = string.Empty;

        public async Task<IActionResult> OnPostAsync()
        {
            if (ModelState.IsValid)
            {
                IdentityUser? user = await _userManager.FindByEmailAsync(Email);
                string decodedToken = _tokenUrlEncoderService.DecodeToken(Token);

                IdentityResult resetPasswordResult =
                    await _userManager.ResetPasswordAsync(user, decodedToken, Password);

                if (resetPasswordResult.Process(ModelState))
                {
                    return RedirectToPage("SignIn", new { });
                }
            }

            return Page();
        }
    }
}
