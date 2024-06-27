using System.ComponentModel.DataAnnotations;

namespace IdentityApp.Models.DTOs
{
    public record SignInLocalRequest(
        [Required][EmailAddress] string Email,
        [Required][MinLength(6)] string Password
    );
}
