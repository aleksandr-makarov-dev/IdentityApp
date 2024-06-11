using System.ComponentModel.DataAnnotations;
using IdentityApp.Core.Extensions;
using IdentityApp.Services;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace IdentityApp.Pages.Identity
{
    public class UserPasswordRecoveryConfirmModel : UserPageModel
    {
        private readonly UserManager<IdentityUser> _userManager;
        private readonly TokenUrlEncoderService _tokenUrlEncoderService;


        public UserPasswordRecoveryConfirmModel(
            UserManager<IdentityUser> userManager, 
            TokenUrlEncoderService tokenUrlEncoderService
            )
        {
            _userManager = userManager;
            _tokenUrlEncoderService = tokenUrlEncoderService;
        }

        [Required]
        [EmailAddress]
        [BindProperty] 
        public string Email { get; set; } = string.Empty;

        [Required] 
        [BindProperty] 
        public string Token { get; set; } = string.Empty;

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
                IdentityUser? foundUser = await _userManager.FindByEmailAsync(Email);

                string decodedToken = _tokenUrlEncoderService.DecodeToken(Token);

                IdentityResult resetPasswordResult =
                    await _userManager.ResetPasswordAsync(foundUser, decodedToken, Password);

                if (resetPasswordResult.Process(ModelState))
                {
                    TempData["message"] = "Password changed";
                    return RedirectToPage();
                }
            }

            return Page();
        }
    }
}
