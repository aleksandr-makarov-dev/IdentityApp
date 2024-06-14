using System.ComponentModel.DataAnnotations;
using IdentityApp.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace IdentityApp.Pages.Identity
{
    [AllowAnonymous]
    public class SignUpResendModel : UserPageModel
    {
        private readonly UserManager<IdentityUser> _userManager;
        private readonly IdentityEmailService _emailService;

        public SignUpResendModel(IdentityEmailService emailService, UserManager<IdentityUser> userManager)
        {
            _emailService = emailService;
            _userManager = userManager;
        }

        [EmailAddress]
        [BindProperty(SupportsGet = true)]
        public string Email { get; set; }
        public async Task<IActionResult> OnPostAsync()
        {
            if (ModelState.IsValid)
            {
                IdentityUser? user = await _userManager.FindByEmailAsync(Email);
                if (user != null && !await _userManager.IsEmailConfirmedAsync(user))
                {
                    await _emailService.SendAccountConfirmEmailAsync(user,
                        "SignUpConfirm");
                }
                TempData["message"] = "Confirmation email sent. Check your inbox.";
                return RedirectToPage(new { Email });
            }
            return Page();
        }
    }
}
