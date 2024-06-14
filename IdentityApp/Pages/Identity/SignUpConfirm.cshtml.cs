using IdentityApp.Core.Extensions;
using IdentityApp.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace IdentityApp.Pages.Identity
{
    [AllowAnonymous]
    public class SignUpConfirmModel : UserPageModel
    {
        private readonly UserManager<IdentityUser> _userManager;
        private readonly TokenUrlEncoderService _tokenUrlEncoderService;

        public SignUpConfirmModel(UserManager<IdentityUser> userManager, TokenUrlEncoderService tokenUrlEncoderService)
        {
            _userManager = userManager;
            _tokenUrlEncoderService = tokenUrlEncoderService;
        }

        [BindProperty(SupportsGet = true)]
        public string Email { get; set; } = string.Empty;

        [BindProperty(SupportsGet = true)] 
        public string Token { get; set; } = string.Empty;

        public bool ShowConfirmedMessage { get; set; }

        public async Task<IActionResult> OnGetAsync()
        {
            if (!string.IsNullOrEmpty(Email) && !string.IsNullOrEmpty(Token))
            {
                IdentityUser? foundUser = await _userManager.FindByEmailAsync(Email);

                if (foundUser != null)
                {
                    string decodedToken = _tokenUrlEncoderService.DecodeToken(Token);

                    IdentityResult confirmEmailResult = await _userManager
                        .ConfirmEmailAsync(foundUser, decodedToken);

                    if (confirmEmailResult.Process(ModelState))
                    {
                        ShowConfirmedMessage = true;
                    }
                }
            }

            return Page();
        }
    }
}
