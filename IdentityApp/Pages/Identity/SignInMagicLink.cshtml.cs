using System.ComponentModel.DataAnnotations;
using IdentityApp.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace IdentityApp.Pages.Identity
{
    [AllowAnonymous]
    public class SignInMagicLinkModel : UserPageModel
    {
        private readonly UserManager<IdentityUser> _userManager;
        private readonly IdentityEmailService _emailService;

        public SignInMagicLinkModel(UserManager<IdentityUser> userManager, IdentityEmailService emailService)
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
                IdentityUser? user = await _userManager.FindByEmailAsync(Email);

                if (user != null)
                {
                    await _emailService.SendMagicLinkEmailAsync(user, "SignInMagicLinkCallback");
                }

                TempData["message"] =
                    "If user is registered they will receive email with link. Click the link it contains to sign in.";
            }

            return Page();
        }
    }
}
