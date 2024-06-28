using Microsoft.Extensions.Options;

namespace IdentityApp.Core.Configurations
{
    public class JwtOptions
    {
        public int Expires { get; set; }
        public string KeySecret { get; set; } = string.Empty;
    }

    public class JwtOptionsSetup:IConfigureOptions<JwtOptions>
    {
        public const string SectionName = "JsonWebToken";
        private readonly IConfiguration _configuration;

        public JwtOptionsSetup(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public void Configure(JwtOptions options)
        {
            _configuration
                .GetSection(SectionName)
                .Bind(options);
        }
    }
}
