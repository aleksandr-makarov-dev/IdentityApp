using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace IdentityApp.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AccountsController : ControllerBase
    {
        private readonly SignInManager<IdentityUser> _signInManager;

        public AccountsController(SignInManager<IdentityUser> signInManager)
        {
            _signInManager = signInManager;
        }

        [HttpGet("external/{provider}")]
        public IActionResult External([FromRoute] string provider)
        {
            AuthenticationProperties props =
                _signInManager.ConfigureExternalAuthenticationProperties(provider, "/api/Accounts/external-callback");

            return new ChallengeResult(provider, props);
        }

        [HttpGet("external-callback")]
        public async Task<IActionResult> ExternalCallback()
        {
            ExternalLoginInfo info = await _signInManager.GetExternalLoginInfoAsync();

            return Ok(new
            {
                email = info.Principal.FindFirst(ClaimTypes.Email).Value,
                name = info.Principal.FindFirst(ClaimTypes.Name).Value
            });
        }
    }
}
