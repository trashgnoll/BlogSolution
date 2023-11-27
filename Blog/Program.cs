using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Blog.Data;
using Blog.Areas.Identity.Data;
using Blog.Authorization;
using Microsoft.AspNetCore.Authorization;
using Serilog;
using Microsoft.CodeAnalysis.Elfie.Model;
using Microsoft.EntityFrameworkCore.Storage;

var configuration = new ConfigurationBuilder()
  .AddJsonFile("appsettings.json")
  .Build();

Log.Logger = new LoggerConfiguration()
    .ReadFrom.Configuration(configuration)
    .CreateLogger();

Log.Information("App start");

var builder = WebApplication.CreateBuilder(args);
var connectionString = builder.Configuration.GetConnectionString("BlogContextConnection") ?? throw new InvalidOperationException("Connection string 'BlogContextConnection' not found.");
Log.Information("Using ConnectionString {connectionString}", connectionString);

builder.Services.AddDbContext<BlogContext>(options =>
    options.UseSqlServer(connectionString));

Log.Information("Adding DefaultIdentoty using hardcoded params");

builder.Services.AddDefaultIdentity<BlogUser>(options =>
{
    options.SignIn.RequireConfirmedAccount = false;
    options.Lockout.AllowedForNewUsers = false;
    options.Password.RequiredLength = 1;
    options.Password.RequireDigit = false;
    options.Password.RequireNonAlphanumeric = false;
    options.Password.RequireUppercase = false;
    options.Password.RequireLowercase = false;

}).AddEntityFrameworkStores<BlogContext>();

builder.Services.AddRazorPages();

builder.Services.AddAuthorization(options =>
{
    options.AddPolicy("CanManageUsers", policyBuider => policyBuider.AddRequirements(new AllowedManagementRequirement()));
    options.AddPolicy("Editor", policyBuilder => policyBuilder.RequireClaim("IsEditor"));
});

builder.Services.AddSingleton<IAuthorizationHandler, UserAdminHandler>();

var fullLogging = builder.Configuration.GetValue("FullLogging", false);
if (fullLogging)
    builder.Host.UseSerilog();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();
app.UseAuthentication();;

app.UseAuthorization();

app.MapRazorPages();

app.Run();
