using Microsoft.Extensions.Options;

namespace IdentityApp.Core.Configurations
{
    public class GoogleOptions
    {
        public string ClientId { get; set; } = string.Empty;
        public string ClientSecret { get; set; } = string.Empty;
    }

    public class GoogleOptionsSetup: IConfigureOptions<GoogleOptions>
    {
        private const string SectionName = "Google";
        private readonly IConfiguration _configuration;

        public GoogleOptionsSetup(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public void Configure(GoogleOptions options)
        {
            _configuration
                .GetSection(SectionName)
                .Bind(options);
        }
    }
}
