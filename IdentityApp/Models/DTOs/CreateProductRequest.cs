using System.ComponentModel.DataAnnotations;

namespace IdentityApp.Models.DTOs
{
    public record CreateProductRequest(
        [Required] string Name,
        [Required] decimal Price,
        [Required] string Category
        );
}
