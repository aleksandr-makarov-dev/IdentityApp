using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using SignInResult = Microsoft.AspNetCore.Identity.SignInResult;

namespace IdentityApp.Pages.Identity
{
    [AllowAnonymous]
    public class SignInModel : UserPageModel
    {
        private readonly SignInManager<IdentityUser>  _signInManager;
        private readonly UserManager<IdentityUser> _userManager;

        public SignInModel(SignInManager<IdentityUser> signInManager, UserManager<IdentityUser> userManager)
        {
            _signInManager = signInManager;
            _userManager = userManager;
        }
        [Required]
        [EmailAddress]
        [BindProperty]
        public string Email { get; set; }

        [Required]
        [BindProperty]
        public string Password { get; set; }

        [BindProperty(SupportsGet = true)]
        public string? ReturnUrl { get; set; }

        public async Task<IActionResult> OnPostAsync()
        {
            if (ModelState.IsValid)
            {
                SignInResult signInResult = await _signInManager.PasswordSignInAsync(Email, Password, true, true);

                if (signInResult.Succeeded)
                {
                    return Redirect(ReturnUrl ?? "/");
                }

                if(signInResult.IsLockedOut)
                {
                    TempData["message"] = "Account is locked. Try again later";
                }else if (signInResult.IsNotAllowed)
                {
                    IdentityUser? user = await _userManager.FindByEmailAsync(Email);

                    if (user != null && !await _userManager.IsEmailConfirmedAsync(user))
                    {
                        return RedirectToPage("SignUpConfirm");
                    }

                    TempData["message"] = "Account is not confirmed (Not allowed)";
                }else if (signInResult.RequiresTwoFactor)
                {
                    return RedirectToPage("SignInTwoFactor", new { ReturnUrl });
                }
                else
                {
                    TempData["message"] = "Invalid credentials";
                }
            }

            return Page();
        }
    }
}
