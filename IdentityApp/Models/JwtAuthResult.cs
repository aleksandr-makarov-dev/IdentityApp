namespace IdentityApp.Models
{
    public class JwtAuthResult
    {
        public string AccessToken { get; set; }
        public RefreshTokenModel RefreshToken { get; set; }
    }
}
