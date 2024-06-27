using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Mvc;

namespace IdentityApp.Core.Extensions
{
    public static class CookieAuthEventsExtensions
    {
        public static void DisableRedirectionForApiClients(this
            CookieAuthenticationEvents events)
        {
            events.OnRedirectToLogin = ctx =>
                SelectiveRedirect(ctx, StatusCodes.Status401Unauthorized,"401 Unauthorized");
            events.OnRedirectToAccessDenied = ctx =>
                SelectiveRedirect(ctx, StatusCodes.Status403Forbidden, "403 Forbidden");
            events.OnRedirectToLogout = ctx =>
                SelectiveRedirect(ctx, StatusCodes.Status200OK);
            events.OnRedirectToReturnUrl = ctx =>
                SelectiveRedirect(ctx, StatusCodes.Status200OK);
        }
        private static Task SelectiveRedirect(
            RedirectContext<CookieAuthenticationOptions> context, int code,string? error = null)
        {
            if (IsApiRequest(context.Request))
            {
                context.Response.StatusCode = code;
                context.Response.Headers["Location"] = context.RedirectUri;
                if (!string.IsNullOrEmpty(error))
                {
                    context.Response.WriteAsJsonAsync(new ProblemDetails { Title = error });
                }
            }
            else
            {
                context.Response.Redirect(context.RedirectUri);
            }
            return Task.CompletedTask;
        }
        private static bool IsApiRequest(HttpRequest request)
        {
            return request.Path.StartsWithSegments("/api");
        }
    }
}
