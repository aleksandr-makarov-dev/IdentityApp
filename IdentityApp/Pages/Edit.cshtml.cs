using IdentityApp.Data;
using IdentityApp.Data.Entities;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Authorization;

namespace IdentityApp.Pages {

    [Authorize(Roles = "Admin")]
    public class EditModel : PageModel {

        public EditModel(ApplicationDbContext ctx) => DbContext = ctx;

        public ApplicationDbContext DbContext { get; set; }
        public Product Product { get; set; }

        public void OnGet(long id) {
            Product = DbContext.Find<Product>(id) ?? new Product();
        }

        public IActionResult OnPost([Bind(Prefix = "Product")] Product p) {
            DbContext.Update(p);
            DbContext.SaveChanges();
            return RedirectToPage("Admin");
        }
    }
}
