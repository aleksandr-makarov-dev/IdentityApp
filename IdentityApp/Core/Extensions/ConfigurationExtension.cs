namespace IdentityApp.Core.Extensions
{
    public static class ConfigurationExtension
    {
        public static string GetConnectionStringOrThrow(this IConfiguration configuration, string name)
        {
            return configuration.GetConnectionString(name) ??
                   throw new ArgumentNullException($"Connection string {name} is null");
        }
    }
}
