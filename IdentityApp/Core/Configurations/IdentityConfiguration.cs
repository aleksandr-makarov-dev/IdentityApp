using Microsoft.Extensions.Options;

namespace IdentityApp.Core.Configurations
{
    public class IdentityInitializeOptions
    {
        public string Role { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public string Password { get; set; } = string.Empty;
    }

    public class IdentityInitializeOptionsSetup : IConfigureOptions<IdentityInitializeOptions>
    {
        public const string SectionName = "Identity";
        private readonly IConfiguration _configuration;

        public IdentityInitializeOptionsSetup(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public void Configure(IdentityInitializeOptions options)
        {
            _configuration
                .GetSection(SectionName)
                .Bind(options);
        }
    }
}
