using IdentityApp.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace IdentityApp.Pages.Identity
{
    [AllowAnonymous]
    public class UserAccountCompleteExternalModel : UserPageModel
    {
        private readonly UserManager<IdentityUser> _userManager;
        private readonly SignInManager<IdentityUser> _signInManager;
        private readonly TokenUrlEncoderService _tokenUrlEncoderService;

        public UserAccountCompleteExternalModel(UserManager<IdentityUser> userManager, SignInManager<IdentityUser> signInManager, TokenUrlEncoderService tokenUrlEncoderService)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _tokenUrlEncoderService = tokenUrlEncoderService;
        }


    }
}
