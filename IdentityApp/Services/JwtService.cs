using System.Diagnostics;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using IdentityApp.Core.Configurations;
using IdentityApp.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;

namespace IdentityApp.Services
{
    public class JwtService
    {
        private readonly UserManager<IdentityUser> _userManager;
        private readonly JwtOptions _jwtOptions;
        private readonly AppOptions _appOptions;
        private readonly RefreshTokenOptions _refreshTokenOptions;

        public JwtService(IOptions<JwtOptions> jwtOptions, UserManager<IdentityUser> userManager, IOptions<AppOptions> appOptions, IOptions<RefreshTokenOptions> refreshTokenOptions)
        {
            _userManager = userManager;
            _refreshTokenOptions = refreshTokenOptions.Value;
            _appOptions = appOptions.Value;
            _jwtOptions = jwtOptions.Value;
        }

        public async Task<JwtAuthResult> GenerateTokens(IdentityUser user,ClaimsIdentity identity, DateTime date)
        {
            SecurityTokenDescriptor descriptor = new SecurityTokenDescriptor
            {
                Subject = identity,
                Expires = DateTime.Now.AddMinutes(_jwtOptions.Expires),
                SigningCredentials = new SigningCredentials(
                    new SymmetricSecurityKey(
                        Encoding.UTF8.GetBytes(_jwtOptions.KeySecret)
                    ), 
                    SecurityAlgorithms.HmacSha256Signature
                )
            };

            JwtSecurityTokenHandler handler = new JwtSecurityTokenHandler();
            SecurityToken token = new JwtSecurityTokenHandler().CreateToken(descriptor);

            string accessTokenString = handler.WriteToken(token);
            string refreshTokenString = await _userManager.GenerateUserTokenAsync(user, "Default", _refreshTokenOptions.Name);

            return new JwtAuthResult
            {
                AccessToken = accessTokenString,
                RefreshToken = new RefreshTokenModel
                {
                    TokenString = refreshTokenString,
                    ExpiresAt = date.AddMinutes(_refreshTokenOptions.Expires)
                }
            };
        }
    }
}
