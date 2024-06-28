using Microsoft.Extensions.Options;

namespace IdentityApp.Core.Configurations
{
    public class RefreshTokenOptions
    {
        public int Expires { get; set; }
        public string Name { get; set; }
    }

    public class RefreshOptionsSetup : IConfigureOptions<RefreshTokenOptions>
    {
        public const string SectionName = "RefreshToken";
        private readonly IConfiguration _configuration;

        public RefreshOptionsSetup(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public void Configure(RefreshTokenOptions options)
        {
            _configuration
                .GetSection(SectionName)
                .Bind(options);
        }
    }
}