using Microsoft.AspNetCore.Identity;

namespace IdentityApp.Pages.Identity {

    public class IndexModel : UserPageModel {
        private readonly UserManager<IdentityUser> _userManager;

        public IndexModel(UserManager<IdentityUser> userManager)
        {
            _userManager = userManager;
        }

        public string Email { get; set; } = string.Empty;
        public string Phone { get; set; } = string.Empty;

        public async Task OnGetAsync()
        {
            IdentityUser? user = await _userManager.GetUserAsync(User);

            if (user is not null)
            {
                Email = user.Email ?? "(Empty)";
                Phone = user.PhoneNumber ?? "(Empty)";
            }
        }
    }
}
