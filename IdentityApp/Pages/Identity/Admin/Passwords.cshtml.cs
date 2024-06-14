using System.ComponentModel.DataAnnotations;
using IdentityApp.Core.Extensions;
using IdentityApp.Services;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace IdentityApp.Pages.Identity.Admin
{
    public class PasswordsModel : AdminPageModel
    {
        private readonly IdentityEmailService _emailService;
        public UserManager<IdentityUser> UserManager { get; set; }
        public IdentityUser? IdentityUser { get; set; }

        public PasswordsModel(UserManager<IdentityUser> userManager, IdentityEmailService emailService)
        {
            UserManager = userManager;
            _emailService = emailService;
        }

        [BindProperty(SupportsGet = true)]
        public string? Id { get; set; }

        [Required]
        [BindProperty]
        public string Password { get; set; } = string.Empty;

        [Required]
        [Compare(nameof(Password))]
        [BindProperty]
        public string ConfirmPassword { get; set; } = string.Empty;

        public async Task<IActionResult> OnGetAsync()
        {
            if (string.IsNullOrEmpty(Id))
            {
                return RedirectToPage(
                    "SelectUser", 
                    new { Label = "Password", callback = "Passwords" }
                    );
            }

            IdentityUser = await UserManager.FindByIdAsync(Id);

            return Page();
        }

        public async Task<IActionResult> OnPostSetPasswordAsync()
        {
            if (ModelState.IsValid)
            {
                IdentityUser = await UserManager.FindByIdAsync(Id);

                if (await UserManager.HasPasswordAsync(IdentityUser))
                { 
                    await UserManager.RemovePasswordAsync(IdentityUser);
                }

                IdentityResult addPasswordResult = await UserManager.AddPasswordAsync(IdentityUser, Password);

                if (addPasswordResult.Process(ModelState))
                {
                    TempData["message"] = "Password changed";

                    return RedirectToPage();
                }
            }

            return Page();
        }

        public async Task<IActionResult> OnPostUserChangeAsync()
        {
            IdentityUser = await UserManager.FindByIdAsync(Id);

            await UserManager.RemovePasswordAsync(IdentityUser);
            await _emailService.SendPasswordRecoveryEmailAsync(IdentityUser, "/Identity/UserPasswordRecoveryConfirm");

            TempData["message"] = "Email sent to user";
            return RedirectToPage();
        }
    }
}
