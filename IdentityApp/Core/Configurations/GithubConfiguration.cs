using Microsoft.Extensions.Options;

namespace IdentityApp.Core.Configurations
{
    public class GithubOptions
    {
        public string ClientId { get; set; } = string.Empty;
        public string ClientSecret { get; set; } = string.Empty;
        public IEnumerable<string> Scopes { get; set; } = new List<string>();
    }

    public class GithubOptionsSetup: IConfigureOptions<GithubOptions>
    {
        public const string SectionName = "Github";
        private readonly IConfiguration _configuration;

        public GithubOptionsSetup(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public void Configure(GithubOptions options)
        {
            _configuration
                .GetSection(SectionName)
                .Bind(options);
        }
    }
}
