using Microsoft.AspNetCore.Identity;
using MudBlazor.Services;
using Synapsers.MudBlazor.ThemeManager.Saver.Extensions;
using Synapsers.MudBlazor.ThemeManager.Saver.Services;
using TestApp.Components;

var builder = WebApplication.CreateBuilder(args);

// Add MudBlazor services
builder.Services.AddMudServices();
 
// Add services to the container.
builder.Services.AddRazorComponents()
    .AddInteractiveServerComponents();

// Add theme persistence
builder.Services.AddThemePersistence();
// Register ThemeStateService as singleton to preserve state across navigation
builder.Services.AddSingleton<ThemeStateService>();

builder.Services.AddAuthentication(options =>
{
    options.DefaultScheme = IdentityConstants.ApplicationScheme;
    options.DefaultSignInScheme = IdentityConstants.ExternalScheme;
})
    .AddIdentityCookies();


var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error", createScopeForErrors: true);
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();


app.UseAntiforgery();

app.MapStaticAssets();
app.MapRazorComponents<App>()
    .AddInteractiveServerRenderMode();

// Initialize the theme when the application starts
using (var scope = app.Services.CreateScope())
{
    var themeStateService = scope.ServiceProvider.GetRequiredService<ThemeStateService>();
    if (!themeStateService.IsInitialized)
    {
        // Default to the system preference if possible, otherwise use light mode
        await themeStateService.InitializeAsync(false);
    }
}

app.Run();
