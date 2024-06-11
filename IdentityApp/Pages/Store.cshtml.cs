using IdentityApp.Data;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Authorization;

namespace IdentityApp.Pages {

    [Authorize]
    public class StoreModel : PageModel {
        public StoreModel(ApplicationDbContext ctx) => DbContext = ctx;

        public ApplicationDbContext DbContext { get; set; }
    }
}
