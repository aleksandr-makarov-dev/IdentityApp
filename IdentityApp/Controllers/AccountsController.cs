using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using IdentityApp.Models.DTOs;
using Microsoft.AspNetCore.Authorization;
using SignInResult = Microsoft.AspNetCore.Identity.SignInResult;

namespace IdentityApp.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AccountsController : ControllerBase
    {
        private readonly SignInManager<IdentityUser> _signInManager;
        private readonly UserManager<IdentityUser> _userManager;

        public AccountsController(SignInManager<IdentityUser> signInManager, UserManager<IdentityUser> userManager)
        {
            _signInManager = signInManager;
            _userManager = userManager;
        }

        [HttpPost("sign-in/local")]
        public async Task<IActionResult> SignInLocal([FromBody] SignInLocalRequest request)
        {
            SignInResult signInResult = await _signInManager.PasswordSignInAsync(request.Email, request.Password, true, true);

            if (signInResult.Succeeded)
            {
                return Ok();
            }

            if (signInResult.IsLockedOut)
            {
                return Unauthorized(new ProblemDetails
                {
                    Title = "Account is locked. Try again later"
                });
            }

            if (signInResult.IsNotAllowed)
            {
                IdentityUser? user = await _userManager.FindByEmailAsync(request.Email);

                if (user != null && !await _userManager.IsEmailConfirmedAsync(user))
                {
                    return Unauthorized(new ProblemDetails
                    {
                        Title = "Account is not confirmed (Not allowed)"
                    });
                }
            }

            if (signInResult.RequiresTwoFactor)
            {
                return Unauthorized(new ProblemDetails
                {
                    Title = "Requires two factor"
                });
            }

            return Unauthorized(new ProblemDetails
            {
                Title = "Invalid credentials"
            });
        }

        [HttpGet("sign-in/external/providers")]
        public async Task<IActionResult> GetExternalProviders()
        {
            IEnumerable<AuthenticationScheme> providers = await _signInManager.GetExternalAuthenticationSchemesAsync();

            IEnumerable<ExternalProviderScheme> externalProviders =
                providers.Select(p => new ExternalProviderScheme(p.Name, p.DisplayName));

            return Ok(externalProviders);
        }

        [HttpGet("sign-in/external/{provider}")]
        public IActionResult External([FromRoute] string provider, [FromQuery] string? returnUrl)
        {
            AuthenticationProperties props =
                _signInManager.ConfigureExternalAuthenticationProperties(provider, $"/api/Accounts/external-callback?returnUrl={returnUrl}");

            return new ChallengeResult(provider, props);
        }

        [HttpGet("external-callback")]
        public async Task<IActionResult> ExternalCallback([FromQuery] string? returnUrl)
        {
            ExternalLoginInfo? info = await _signInManager.GetExternalLoginInfoAsync();

            if (info is null)
            {
                return Unauthorized("Failed to get external provider info");
            }

            SignInResult externalSignInResult = await _signInManager.ExternalLoginSignInAsync(info.LoginProvider, info.ProviderKey, true,true);

            if (externalSignInResult.Succeeded)
            {
                return RedirectPermanent(returnUrl);
            }

            if (externalSignInResult.IsLockedOut)
            {
                return Unauthorized("Account is locked out");
            }

            if (externalSignInResult.IsNotAllowed)
            {
                return Unauthorized("Sign in is not allowed");
            }

            return Unauthorized("Sign in failed");
        }
    }
}
