using Microsoft.AspNetCore.Mvc.ModelBinding;
using System.Linq;
using Microsoft.AspNetCore.Identity;

namespace IdentityApp.Core.Extensions
{
    public static class IdentityExtensions
    {

        public static bool Process(this IdentityResult result,
                ModelStateDictionary modelState)
        {
            foreach (IdentityError err in result.Errors)
            {
                modelState.AddModelError(string.Empty, err.Description);
            }
            return result.Succeeded;
        }
    }
}
