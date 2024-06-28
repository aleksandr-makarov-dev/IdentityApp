namespace IdentityApp.Models
{
    public class RefreshTokenModel
    {
        public string TokenString { get; set; }
        public DateTime ExpiresAt { get; set; }
    }
}
