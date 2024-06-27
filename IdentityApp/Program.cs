using IdentityApp.Core.Configurations;
using IdentityApp.Core.Extensions;
using IdentityApp.Data;
using IdentityApp.Services;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

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
builder.Services.ConfigureOptions<GoogleOptionsSetup>();

// Add DbContext

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

builder.Services.AddAuthentication()
    .AddGoogle((options) =>
    {
        options.ClientId = builder.Configuration["Google:ClientId"];
        options.ClientSecret = builder.Configuration["Google:ClientSecret"];
    })
    .AddGitHub((options) =>
    {
        options.ClientId = builder.Configuration["Github:ClientId"];
        options.ClientSecret = builder.Configuration["Github:ClientSecret"];
        options.Scope.Add("user:email");
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
