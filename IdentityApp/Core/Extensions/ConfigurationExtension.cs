namespace IdentityApp.Core.Extensions
{
    public static class ConfigurationExtension
    {
        public static string GetConnectionStringOrThrow(this IConfiguration configuration, string name)
        {
            return configuration.GetConnectionString(name) ??
                   throw new ArgumentNullException($"Connection string {name} is null");
        }

        public static T GetOrThrow<T>(this IConfigurationSection section)
        {
            return section.Get<T>() ?? throw new ArgumentNullException(nameof(T));
        }
    }
}
