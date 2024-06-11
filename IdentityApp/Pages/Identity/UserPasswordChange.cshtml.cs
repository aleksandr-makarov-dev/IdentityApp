using System.ComponentModel.DataAnnotations;
using IdentityApp.Core.Extensions;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace IdentityApp.Pages.Identity
{
    public class UserPasswordChangeModel : UserPageModel
    {
        private readonly UserManager<IdentityUser> _userManager;

        public UserPasswordChangeModel(UserManager<IdentityUser> userManager)
        {
            _userManager = userManager;
        }

        [Required]
        [BindProperty]
        public string Current { get; set; } = string.Empty;

        [Required]
        [BindProperty]
        public string NewPassword { get; set; } = string.Empty;

        [Required]
        [Compare(nameof(NewPassword))]
        [BindProperty]
        public string ConfirmPassword { get; set; } = string.Empty;

        public async Task<IActionResult> OnPostAsync()
        {
            if (ModelState.IsValid)
            {
                IdentityUser? currentUser = await _userManager.GetUserAsync(User);

                IdentityResult passwordChangeResult =
                    await _userManager.ChangePasswordAsync(currentUser, Current, NewPassword);

                if (passwordChangeResult.Process(ModelState))
                {
                    TempData["message"] = "Password changed";
                    return RedirectToPage();
                }
            }

            return Page();
        }
    }
}
