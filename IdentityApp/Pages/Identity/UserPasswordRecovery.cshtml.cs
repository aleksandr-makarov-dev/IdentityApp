using System.ComponentModel.DataAnnotations;
using IdentityApp.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace IdentityApp.Pages.Identity
{
    [AllowAnonymous]
    public class UserPasswordRecoveryModel : UserPageModel
    {
        private readonly UserManager<IdentityUser> _userManager;
        private readonly IdentityEmailService _emailService;

        public UserPasswordRecoveryModel(UserManager<IdentityUser> userManager, IdentityEmailService emailService)
        {
            _userManager = userManager;
            _emailService = emailService;
        }

        [Required]
        [EmailAddress]
        [BindProperty]
        public string Email { get; set; } = string.Empty;

        public async Task<IActionResult> OnPostAsync()
        {
            if (ModelState.IsValid)
            {
                IdentityUser? foundUser = await _userManager.FindByEmailAsync(Email);

                if (foundUser != null)
                {
                    await _emailService
                        .SendPasswordRecoveryEmailAsync(foundUser, "UserPasswordRecoveryConfirm");
                }

                TempData["message"] = "We sent you an email."
                                      + " Click the link it contains to choose a new password.";

                return RedirectToPage();
            }

            return Page();
        }
    }
}
