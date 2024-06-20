using System.ComponentModel.DataAnnotations;
using IdentityApp.Core.Configurations;
using IdentityApp.Core.Extensions;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;

namespace IdentityApp.Pages.Identity.Admin
{
    public class RolesModel : PageModel
    {

        public RolesModel(
            RoleManager<IdentityRole> roleManager,
            UserManager<IdentityUser> userManager,
            IOptions<IdentityInitializeOptions> options
            )
        {
            RoleManager = roleManager;
            UserManager = userManager;
            SuperAdminRole= options.Value.Role;
        }
        public string SuperAdminRole { get; }
        public RoleManager<IdentityRole> RoleManager { get; }
        public UserManager<IdentityUser> UserManager { get; }

        [BindProperty(SupportsGet = true)]
        public string Id { get; set; }

        public IEnumerable<string> CurrentRoles { get; private set; } = new List<string>();
        public IEnumerable<string> AvailableRoles { get; private set; } = new List<string>();

        private async Task UpdateRolesAsync()
        {
            IdentityUser? foundUser = await UserManager.FindByIdAsync(Id);

            CurrentRoles = await UserManager.GetRolesAsync(foundUser);

            AvailableRoles = await RoleManager
                .Roles
                .Select(r => r.Name)
                .Where(r => !CurrentRoles.Contains(r))
                .ToListAsync();
        }

        public async Task<IActionResult> OnGetAsync()
        {
            if (string.IsNullOrEmpty(Id))
            {
                return RedirectToPage("SelectUser", new { Label = "Edit Roles", Callback = "Roles" });
            }

            await UpdateRolesAsync();

            return Page();
        }

        public async Task<IActionResult> OnPostAddToListAsync(string role)
        {
            IdentityRole roleToCreate = new IdentityRole(role);

            IdentityResult createRoleResult = await RoleManager.CreateAsync(roleToCreate);

            if (createRoleResult.Process(ModelState))
            {
                return RedirectToPage();
            }

            await UpdateRolesAsync();

            return Page();
        }

        public async Task<IActionResult> OnPostDeleteFromListAsync(string role)
        {
            IdentityRole? foundRole = await RoleManager.FindByNameAsync(role);

            IdentityResult deleteRoleResult = await RoleManager.DeleteAsync(foundRole);

            if (deleteRoleResult.Process(ModelState))
            {
                return RedirectToPage();
            }

            await UpdateRolesAsync();

            return Page();
        }

        public async Task<IActionResult> OnPostAddUserToRoleAsync([Required] string role)
        {
            if (ModelState.IsValid)
            {
                IdentityUser foundUser = await UserManager.FindByIdAsync(Id);

                if (!await UserManager.IsInRoleAsync(foundUser, role))
                {
                    IdentityResult addUserToRoleResult = await UserManager.AddToRoleAsync(foundUser, role);

                    if (addUserToRoleResult.Process(ModelState))
                    {
                        return RedirectToPage();
                    }
                }
            }

            await UpdateRolesAsync();

            return Page();
        }

        public async Task<IActionResult> OnPostRemoveUserFromRoleAsync(string role)
        {
            IdentityUser foundUser = await UserManager.FindByIdAsync(Id);

            if (await UserManager.IsInRoleAsync(foundUser, role))
            {
                IdentityResult removeFromRoleResult = await UserManager.RemoveFromRoleAsync(foundUser, role);

                if (removeFromRoleResult.Process(ModelState))
                {
                    return RedirectToPage();
                }
            }

            return Page();
        }
    }
}
