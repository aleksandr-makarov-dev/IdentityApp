using System.Text;
using Microsoft.AspNetCore.WebUtilities;

namespace IdentityApp.Services
{
    public class TokenUrlEncoderService
    {
        public virtual string EncodeToken(string token)
        {
            return WebEncoders.Base64UrlEncode(Encoding.UTF8.GetBytes(token));
        }

        public virtual string DecodeToken(string encodedToken)
        {
            return Encoding.UTF8.GetString(WebEncoders.Base64UrlDecode(encodedToken));
        }
    }
}
