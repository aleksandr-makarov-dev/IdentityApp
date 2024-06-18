using IdentityApp.Core.Extensions;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace IdentityApp.Pages.Identity
{
    public class UserDeleteModel : UserPageModel
    {
        private readonly UserManager<IdentityUser> _userManager;
        private readonly SignInManager<IdentityUser> _signInManager;
        public UserDeleteModel(UserManager<IdentityUser> userManager, SignInManager<IdentityUser> signInManager)
        {
            _userManager = userManager;
            _signInManager = signInManager;
        }

        public async Task<IActionResult> OnPostAsync()
        {
            IdentityUser? userToDelete = await _userManager.GetUserAsync(User);
            if (userToDelete is not null)
            {
                IdentityResult result = await _userManager.DeleteAsync(userToDelete);
                if (result.Process(ModelState))
                {
                    await _signInManager.SignOutAsync();
                    return Challenge();
                }
            }

            return Page();
        }
    }
}
