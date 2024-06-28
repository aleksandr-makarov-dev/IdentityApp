using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using IdentityApp.Core.Configurations;
using IdentityApp.Models;
using IdentityApp.Models.DTOs;
using IdentityApp.Services;
using Microsoft.Extensions.Options;
using SignInResult = Microsoft.AspNetCore.Identity.SignInResult;
using System.Security.Principal;

namespace IdentityApp.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AccountsController : ControllerBase
    {
        private readonly SignInManager<IdentityUser> _signInManager;
        private readonly UserManager<IdentityUser> _userManager;
        private readonly JwtService _jwtService;
        private readonly AppOptions _appOptions;
        private readonly RefreshTokenOptions _refreshTokenOptions;

        public AccountsController(SignInManager<IdentityUser> signInManager, UserManager<IdentityUser> userManager, JwtService jwtService, IOptions<AppOptions> appOptions, IOptions<RefreshTokenOptions> refreshTokenOptions)
        {
            _signInManager = signInManager;
            _userManager = userManager;
            _jwtService = jwtService;
            _refreshTokenOptions = refreshTokenOptions.Value;
            _appOptions = appOptions.Value;
        }

        [HttpPost("sign-in/local")]
        public async Task<IActionResult> SignInLocal([FromBody] SignInLocalRequest request)
        {
            IdentityUser? user = await _userManager.FindByEmailAsync(request.Email);

            if (user == null)
            {
                return Unauthorized("User not found");
            }

            SignInResult checkPasswordResult = await _signInManager.CheckPasswordSignInAsync(user,request.Password,true);

            if (checkPasswordResult.Succeeded)
            {

                ClaimsIdentity? identity = (await _signInManager.CreateUserPrincipalAsync(user)).Identities.FirstOrDefault();

                if (identity == null)
                {
                    return Unauthorized("Could not get user identity");
                }

                JwtAuthResult jwtAuthResult = await _jwtService.GenerateTokens(user,identity, DateTime.Now);

                await _userManager.SetAuthenticationTokenAsync(
                    user, _appOptions.AppName,
                    _refreshTokenOptions.Name,
                    jwtAuthResult.RefreshToken.TokenString
                );

                await HttpContext.SignInAsync(IdentityConstants.ApplicationScheme,new ClaimsPrincipal(identity));

                return Ok(new { token = jwtAuthResult.AccessToken });
            }

            if (checkPasswordResult.IsNotAllowed)
            {
                if (!await _userManager.IsEmailConfirmedAsync(user))
                {
                    return Unauthorized(new ProblemDetails { Title = "Account is not confirmed (Not allowed)" });
                }
            }

            if (checkPasswordResult.RequiresTwoFactor)
            {
                return Unauthorized(new ProblemDetails { Title = "Requires two factor" });
            }

            return Unauthorized(new ProblemDetails { Title = "Invalid credentials" });
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
            string query = string.IsNullOrEmpty(returnUrl) ? string.Empty : $"?returnUrl={returnUrl}";

            AuthenticationProperties props =
                _signInManager.ConfigureExternalAuthenticationProperties(provider, $"/api/Accounts/external-callback{query}");

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
                return string.IsNullOrEmpty(returnUrl) ? Ok() : RedirectPermanent(returnUrl);
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

        [HttpPost("refresh-token")]
        public async Task<IActionResult> RefreshToken()
        {
            IdentityUser user = await _userManager.FindByIdAsync(User.FindFirstValue(ClaimTypes.NameIdentifier));


            ClaimsIdentity? identity = (await _signInManager.CreateUserPrincipalAsync(user)).Identities.FirstOrDefault();

            JwtAuthResult jwtAuthResult = await _jwtService.GenerateTokens(user, identity, DateTime.Now);

            await _userManager.SetAuthenticationTokenAsync(
                user, _appOptions.AppName,
                _refreshTokenOptions.Name,
                jwtAuthResult.RefreshToken.TokenString
            );

            await HttpContext.SignInAsync(IdentityConstants.ApplicationScheme, new ClaimsPrincipal(identity));

            return Ok(new { token = jwtAuthResult.AccessToken });
        }

        [HttpDelete("sign-out")]
        public async Task<IActionResult> SignOut()
        {
            await _signInManager.SignOutAsync();

            return NoContent();
        }
    }
}
