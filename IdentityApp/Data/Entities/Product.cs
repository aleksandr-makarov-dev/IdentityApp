using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace IdentityApp.Data.Entities {

    public class Product {
        [Key]
        public long Id { get; set; }

        public string Name { get; set; } = string.Empty;
        
        [Column(TypeName = "decimal(8, 2)")]
        public decimal Price { get; set; }

        public string Category { get; set; } = string.Empty;
    }
}
