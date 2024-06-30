using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI.Services;

namespace IdentityApp.Services
{
    public class IdentityEmailService
    {
        private readonly IEmailSender _emailSender;
        private readonly UserManager<IdentityUser> _userManager;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly LinkGenerator _linkGenerator;
        private readonly TokenUrlEncoderService _tokenUrlEncoderService;

        public IdentityEmailService(
            TokenUrlEncoderService tokenUrlEncoderService,
            LinkGenerator linkGenerator,
            IHttpContextAccessor httpContextAccessor,
            UserManager<IdentityUser> userManager,
            IEmailSender emailSender
            )
        {
            _tokenUrlEncoderService = tokenUrlEncoderService;
            _linkGenerator = linkGenerator;
            _httpContextAccessor = httpContextAccessor;
            _userManager = userManager;
            _emailSender = emailSender;
        }

        private string GetUrl(string emailAddress, string token, string page)
        {
            string safeToken = _tokenUrlEncoderService.EncodeToken(token);

            return _linkGenerator
                .GetUriByPage(
                    _httpContextAccessor.HttpContext,
                    page,
                    null,
                    new { email = emailAddress, token = safeToken }
                );
        }

        public async Task SendPasswordRecoveryEmailAsync(IdentityUser user, string confirmationPage)
        {
            string token = await _userManager.GeneratePasswordResetTokenAsync(user);
            string url = GetUrl(user.Email, token, confirmationPage);

            await _emailSender.SendEmailAsync(user.Email, "Set Your Password",
                $"Please set your password by <a href=\"{url}\">clicking here </a>");
        }

        public async Task SendAccountConfirmEmailAsync(IdentityUser user, string confirmationPage)
        {
            string token = await _userManager.GenerateEmailConfirmationTokenAsync(user);
            string url = GetUrl(user.Email, token, confirmationPage);

            await _emailSender.SendEmailAsync(
                user.Email, 
                "Complete you account setup",
                $"Please set up your account by <a href=\"{url}\">clicking here</a>."
                );
        }

        public async Task SendMagicLinkEmailAsync(IdentityUser user, string confirmationPage)
        {
            string token = await _userManager.GenerateUserTokenAsync(user, "Default", "passwordless-auth");

            string url = GetUrl(user.Email, token, confirmationPage);

            await _emailSender.SendEmailAsync(
                user.Email,
                "Sign in to account",
                $"Click the link to sing in to your account <a href=\"{url}\">clicking here</a>."
            );
        }
    }
}
