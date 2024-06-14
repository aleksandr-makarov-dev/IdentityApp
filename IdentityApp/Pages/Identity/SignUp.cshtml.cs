using System.ComponentModel.DataAnnotations;
using IdentityApp.Core.Extensions;
using IdentityApp.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace IdentityApp.Pages.Identity
{
    [AllowAnonymous]
    public class SignUpModel : UserPageModel
    {
        private readonly UserManager<IdentityUser> _userManager;
        private readonly IdentityEmailService _identityEmailService;

        public SignUpModel(UserManager<IdentityUser> userManager, IdentityEmailService identityEmailService)
        {
            _userManager = userManager;
            _identityEmailService = identityEmailService;
        }

        [Required]
        [EmailAddress]
        [BindProperty]
        public string Email { get; set; }

        [Required]
        [BindProperty]
        public string Password { get; set; }

        public async Task<IActionResult> OnPostAsync()
        {
            if (ModelState.IsValid)
            {
                IdentityUser? foundUser = await _userManager.FindByEmailAsync(Email);

                if (foundUser != null && !await _userManager.IsEmailConfirmedAsync(foundUser))
                {
                    return RedirectToPage("SignUpConfirm");
                }

                IdentityUser userToCreate = new IdentityUser
                {
                    Email = Email,
                    UserName = Email
                };

                IdentityResult createUserResult = await _userManager.CreateAsync(userToCreate, Password);

                if (createUserResult.Process(ModelState))
                {
                    await _identityEmailService
                        .SendAccountConfirmEmailAsync(
                            userToCreate, 
                            "SignUpConfirm"
                            );

                    return RedirectToPage("SignUpConfirm");
                }
            }

            return Page();
        }
    }
}
