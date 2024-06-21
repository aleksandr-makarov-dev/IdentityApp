using Microsoft.AspNetCore.Authorization;

namespace IdentityApp.Pages.Identity.Admin {

    [Authorize(Roles = "SuperAdmin")]
    public class AdminPageModel : UserPageModel {

        // no methods or properties required
    }
}
