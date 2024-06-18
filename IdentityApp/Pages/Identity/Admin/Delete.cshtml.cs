using IdentityApp.Core.Extensions;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace IdentityApp.Pages.Identity.Admin
{
    public class DeleteModel : AdminPageModel
    {
        private readonly UserManager<IdentityUser> _userManager;

        public DeleteModel(UserManager<IdentityUser> userManager)
        {
            _userManager = userManager;
        }

        public IdentityUser? IdentityUser { get; set; }

        [BindProperty(SupportsGet = true)]
        public string Id { get; set; }

        public async Task<IActionResult> OnGetAsync()
        {
            if (string.IsNullOrEmpty(Id))
            {
                return RedirectToPage("SelectUser", new { Label = "Delete", Callback = "Delete" });
            }

            IdentityUser = await _userManager.FindByIdAsync(Id);

            if (IdentityUser is null)
            {
                return RedirectToPage("SelectUser", new { Label = "Delete", Callback = "Delete" });
            }

            return Page();
        }

        public async Task<IActionResult> OnPostAsync()
        {
            IdentityUser = await _userManager.FindByIdAsync(Id);
            IdentityResult deleteUserResult = await _userManager.DeleteAsync(IdentityUser);

            if (deleteUserResult.Process(ModelState))
            {
                return RedirectToPage("Dashboard");
            }

            return Page();
        }
    }
}
