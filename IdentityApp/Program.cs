using IdentityApp.Core.Configurations;
using IdentityApp.Core.Extensions;
using IdentityApp.Data;
using IdentityApp.Services;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddRazorPages();

builder.Services.AddHttpsRedirection(opts => {
    opts.HttpsPort = 44350;
});

// Configure options

builder.Services.ConfigureOptions<IdentityInitializeOptionsSetup>();

builder.Services.AddDbContext<ApplicationDbContext>(opts => {
    opts.UseSqlite(
        builder.Configuration.GetConnectionStringOrThrow("SqliteConnection")
    );
});

builder.Services.AddScoped<IEmailSender, ConsoleEmailSender>();

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

//builder.Services.AddAuthentication()
//    .AddFacebook(opts => {
//        opts.AppId = Configuration["Facebook:AppId"];
//        opts.AppSecret = Configuration["Facebook:AppSecret"];
//    })
//    .AddGoogle(opts => {
//        opts.ClientId = Configuration["Google:ClientId"];
//        opts.ClientSecret = Configuration["Google:ClientSecret"];
//    })
//    .AddTwitter(opts => {
//      opts.ConsumerKey = Configuration["Twitter:ApiKey"];
//        opts.ConsumerSecret = Configuration["Twitter:ApiSecret"];
//    });

builder.Services.AddScoped<TokenUrlEncoderService>();
builder.Services.AddScoped<IdentityEmailService>();

builder.Services.ConfigureApplicationCookie((options) =>
{
    options.LoginPath = "/Identity/SignIn";
    options.LogoutPath = "/Identity/SignOut";
    options.AccessDeniedPath = "/Identity/Forbidden";
});

builder.Services.Configure<SecurityStampValidatorOptions>((options) =>
{
    options.ValidationInterval = TimeSpan.FromMinutes(1);
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

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthorization();

app.MapRazorPages();

app.Run();
