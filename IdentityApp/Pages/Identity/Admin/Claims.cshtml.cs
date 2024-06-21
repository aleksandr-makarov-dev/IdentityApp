using System.ComponentModel.DataAnnotations;
using System.Security.Claims;
using IdentityApp.Core.Extensions;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace IdentityApp.Pages.Identity.Admin
{
    public class ClaimsModel : AdminPageModel
    {
        private readonly UserManager<IdentityUser> _userManager;

        public ClaimsModel(UserManager<IdentityUser> userManager)
        {
            _userManager = userManager;
        }

        [BindProperty(SupportsGet = true)]
        public string Id { get; set; }

        public IEnumerable<Claim> Claims { get; set; } = Enumerable.Empty<Claim>();

        public async Task<IActionResult> OnGetAsync()
        {
            if (string.IsNullOrEmpty(Id))
            {
                return RedirectToPage(
                    "SelectUser",
                    new { Label = "Manage Claims", Callback = "Claims" }
                    );
            }

            IdentityUser? foundUser = await _userManager.FindByIdAsync(Id);
            Claims = await _userManager.GetClaimsAsync(foundUser);

            return Page();
        }

        public async Task<IActionResult> OnPostAsync(
            [Required] string task,
            [Required] string type,
            [Required] string value,
            string? oldValue
            )
        {
            IdentityUser? foundUser = await _userManager.FindByIdAsync(Id);
            Claims = await _userManager.GetClaimsAsync(foundUser);

            IdentityResult identityResult = IdentityResult.Success;

            if (ModelState.IsValid)
            {
                Claim claim = new Claim(type, value);
                switch (task)
                {
                    case "add":
                        identityResult = await _userManager.AddClaimAsync(foundUser, claim);
                        break;
                    case "change":
                        identityResult = await _userManager.ReplaceClaimAsync(
                            foundUser, 
                            new Claim(type, oldValue), 
                            claim
                            );
                        break;
                    case "delete":
                        identityResult = await _userManager.RemoveClaimAsync(foundUser, claim);
                        break;
                }

                if (identityResult.Process(ModelState))
                {
                    return RedirectToPage();
                }
            }

            return Page();
        }
    }
}
