using IdentityApp.Data;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace IdentityApp.Pages {
    public class LandingModel : PageModel {

        public LandingModel(ApplicationDbContext ctx) => DbContext = ctx;

        public ApplicationDbContext DbContext { get; set; }

    }
}
