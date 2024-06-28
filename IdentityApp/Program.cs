using System.Text;
using IdentityApp.Core.Configurations;
using IdentityApp.Core.Extensions;
using IdentityApp.Data;
using IdentityApp.Services;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;

var builder = WebApplication.CreateBuilder(args);

IConfiguration configuration = builder.Configuration;

// Add services to the container.
builder.Services.AddRazorPages();
builder.Services.AddControllers();
builder.Services.AddSwaggerGen();
builder.Services.AddCors();

//builder.Services.AddHttpsRedirection(opts => {
//    opts.HttpsPort = 44350;
//});

// Configure options

builder.Services.ConfigureOptions<IdentityInitializeOptionsSetup>();
builder.Services.ConfigureOptions<JwtOptionsSetup>();
builder.Services.ConfigureOptions<AppOptionsSetup>();
builder.Services.ConfigureOptions<RefreshOptionsSetup>();

// Add DbContext

builder.Services.AddDbContext<ApplicationDbContext>(opts => {
    opts.UseSqlite(
        builder.Configuration.GetConnectionStringOrThrow("SqliteConnection")
    );
});

builder.Services.AddScoped<IEmailSender, ConsoleEmailSender>();
builder.Services.AddScoped<JwtService>();

builder.Services
    .AddIdentity<IdentityUser, IdentityRole>(opts =>
    {
        opts.Password.RequiredLength = 6;
        opts.Password.RequireDigit = false;
        opts.Password.RequireLowercase = false;
        opts.Password.RequireUppercase = false;
        opts.Password.RequireNonAlphanumeric = false;
        opts.SignIn.RequireConfirmedAccount = true;
    })
    .AddEntityFrameworkStores<ApplicationDbContext>()
    .AddDefaultTokenProviders();

builder.Services.AddAuthentication()
    .AddGoogle((options) =>
    {
        GoogleOptions googleOptions = configuration
            .GetSection(GoogleOptionsSetup.SectionName)
            .GetOrThrow<GoogleOptions>();

        options.ClientId = googleOptions.ClientId;
        options.ClientSecret = googleOptions.ClientSecret;
    })
    .AddGitHub((options) =>
    {
        GithubOptions githubOptions = configuration
            .GetSection(GithubOptionsSetup.SectionName)
            .GetOrThrow<GithubOptions>();

        options.ClientId = githubOptions.ClientId;
        options.ClientSecret = githubOptions.ClientSecret;

        foreach (var scope in githubOptions.Scopes)
        {
            options.Scope.Add(scope);
        }
    })
    .AddJwtBearer(JwtBearerDefaults.AuthenticationScheme, (options) =>
    {
        JwtOptions jwtOptions = configuration
            .GetSection(JwtOptionsSetup.SectionName)
            .GetOrThrow<JwtOptions>();

        options.TokenValidationParameters.ValidateAudience = false;
        options.TokenValidationParameters.ValidateIssuer = false;
        options.TokenValidationParameters.IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtOptions.KeySecret));
        options.TokenValidationParameters.ClockSkew = TimeSpan.Zero;
    });

builder.Services.AddScoped<TokenUrlEncoderService>();
builder.Services.AddScoped<IdentityEmailService>();

builder.Services.ConfigureApplicationCookie((options) =>
{
    options.LoginPath = "/Identity/SignIn";
    options.LogoutPath = "/Identity/SignOut";
    options.AccessDeniedPath = "/Identity/Forbidden";

    options.Events.DisableRedirectionForApiClients();
});

builder.Services.Configure<SecurityStampValidatorOptions>((options) =>
{
    options.ValidationInterval = TimeSpan.FromMinutes(1);
});

builder.Services.AddCors(opts => {
    opts.AddDefaultPolicy(builder => {
        builder.WithOrigins("http://localhost:5173")
            .AllowAnyHeader()
            .AllowAnyMethod()
            .AllowCredentials();
    });
});

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

// seed database
app.SeedIdentityUsers();

//app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseCors();

app.UseAuthentication();
app.UseAuthorization();


app.MapRazorPages();
app.MapControllers();

app.UseSwagger();
app.UseSwaggerUI();

app.Run();
