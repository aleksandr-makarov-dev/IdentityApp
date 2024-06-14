using System.ComponentModel.DataAnnotations;
using IdentityApp.Core.Extensions;
using IdentityApp.Services;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace IdentityApp.Pages.Identity.Admin
{
    public class CreateModel : AdminPageModel
    {
        private readonly UserManager<IdentityUser> _userManager;
        private readonly IdentityEmailService _emailService;

        public CreateModel(UserManager<IdentityUser> userManager, IdentityEmailService emailService)
        {
            _userManager = userManager;
            _emailService = emailService;
        }

        [Required]
        [EmailAddress]
        [BindProperty]
        public string Email { get; set; }

        public async Task<IActionResult> OnPostAsync()
        {
            if (ModelState.IsValid)
            {
                IdentityUser userToCreate = new IdentityUser
                {
                    Email = Email,
                    UserName = Email,
                    EmailConfirmed = true
                };

                IdentityResult createUserResult = await _userManager.CreateAsync(userToCreate);

                if(createUserResult.Process(ModelState))
                {
                    await _emailService.SendPasswordRecoveryEmailAsync(
                        userToCreate,
                        "/Identity/UserAccountComplete"
                        );

                    TempData["message"] = "Account Created";
                    return RedirectToPage();
                }
            }

            return Page();
        }
    }
}
