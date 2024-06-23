using System.Security.Claims;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace IdentityApp.Pages.Identity.Admin
{
    public class ViewClaimsPrincipalModel : AdminPageModel
    {
        private readonly UserManager<IdentityUser> _userManager;
        private readonly SignInManager<IdentityUser> _signInManager;

        public ViewClaimsPrincipalModel(UserManager<IdentityUser> userManager, SignInManager<IdentityUser> signInManager)
        {
            _userManager = userManager;
            _signInManager = signInManager;
        }

        [BindProperty(SupportsGet = true)] 
        public string Id { get; set; } = string.Empty;

        [BindProperty(SupportsGet = true)] 
        public string Callback { get; set; } = string.Empty;
        public ClaimsPrincipal Principal { get; private set; }

        public async Task<IActionResult> OnGetAsync()
        {
            if (string.IsNullOrEmpty(Id))
            {
                return RedirectToPage("Selectuser",
                    new
                    {
                        Label = "View ClaimsPrincipal",
                        Callback = "ClaimsPrincipal"
                    });
            }

            IdentityUser? foundUser = await _userManager.FindByIdAsync(Id);

            Principal = await _signInManager.CreateUserPrincipalAsync(foundUser);

            return Page();
        }
    }
}
