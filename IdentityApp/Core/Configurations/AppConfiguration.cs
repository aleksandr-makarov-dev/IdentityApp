using Microsoft.Extensions.Options;

namespace IdentityApp.Core.Configurations
{
    public class AppOptions
    {
        public string AppName { get; set; }
    }

    public class AppOptionsSetup : IConfigureOptions<AppOptions>
    {
        public const string SectionName = "App";
        private readonly IConfiguration _configuration;

        public AppOptionsSetup(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public void Configure(AppOptions options)
        {
            _configuration
                .GetSection(SectionName)
                .Bind(options);
        }
    }
}
