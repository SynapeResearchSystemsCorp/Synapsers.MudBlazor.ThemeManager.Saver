# TestApp: Synapsers.MudBlazor.ThemeManager.Saver Integration Example

This project demonstrates how to integrate and use the `Synapsers.MudBlazor.ThemeManager.Saver` library in a Blazor Server application with MudBlazor. The app provides a modern UI for theme configuration and persistence, allowing users to customize and save their preferred theme settings.

## Features
- **MudBlazor Integration**: Uses MudBlazor components for a rich UI experience.
- **Theme Manager**: Integrates `Synapsers.MudBlazor.ThemeManager.Saver` for runtime theme customization and persistence.
- **State Persistence**: Theme state is preserved across navigation and reloads.
- **Authentication**: Basic authentication setup using ASP.NET Core Identity.

## Key Implementation Details

### 1. Service Registration (`Program.cs`)
- **MudBlazor Services**: Registered via `builder.Services.AddMudServices()`.
- **Theme Persistence**: Registered with `builder.Services.AddThemePersistence()`.
- **Theme State Service**: Registered as a singleton to maintain theme state across the app.
- **Authentication**: Configured with Identity cookies.
- **Theme Initialization**: On app startup, the theme is initialized to the user's system preference or defaults to light mode.

### 2. Layout and Theme Management (`MainLayout.razor`)
- **MudThemeProvider**: Binds the current theme and dark mode state from `ThemeStateService`.
- **Theme Manager UI**: Includes `MudThemeManagerButton` and `MudThemeManager` components for opening the theme configuration panel and applying changes.
- **State Handling**: Listens for theme changes and navigation events to ensure the UI updates and color pickers refresh correctly.
- **Dark Mode Toggle**: App bar button toggles dark mode using the theme state service.

### 3. App Shell (`App.razor`)
- **Head and Body Setup**: Loads required fonts, stylesheets, and scripts for MudBlazor and the theme manager.
- **Routing**: Uses `<Routes />` for navigation.

### 4. Example Page (`Home.razor`)
- **Component Showcase**: Demonstrates various MudBlazor components (buttons, tables, alerts, badges, chips, avatars) to visualize theme changes in real time.
- **Instructions**: Guides users to open the theme configuration panel via the settings button.

## How Theme Persistence Works
- The `ThemeStateService` manages the current theme and dark mode state.
- The theme is initialized on application startup and can be updated via the UI.
- Changes are persisted and reflected across the app, even after navigation or reloads.

## How to Use
1. **Run the App**: Start the application. The default theme will match the system preference or use light mode.
2. **Open Theme Manager**: Click the settings button (bottom right) to open the theme configuration panel.
3. **Customize Theme**: Adjust colors, dark mode, and other settings. Changes are applied instantly.
4. **Persistence**: Your theme preferences are saved and restored on subsequent visits.

## File Overview
- `Program.cs`: Configures services, authentication, and theme initialization.
- `Components/Layout/MainLayout.razor`: Main layout with theme manager integration and state handling.
- `Components/App.razor`: App shell, head, and routing setup.
- `Components/Pages/Home.razor`: Example page showcasing MudBlazor components.
- `Components/_Imports.razor`: Common usings for components.

## Dependencies
- [MudBlazor](https://mudblazor.com/)
- [Synapsers.MudBlazor.ThemeManager.Saver](https://www.nuget.org/packages/Synapsers.MudBlazor.ThemeManager.Saver)

## Credits
- Theme manager and persistence by [Synapsers.MudBlazor.ThemeManager.Saver](https://github.com/Synapsers/MudBlazor.ThemeManager.Saver)
- UI components by [MudBlazor](https://mudblazor.com/)

## Code Examples

Below are code excerpts from the main files in this project to help you understand the integration and usage of Synapsers.MudBlazor.ThemeManager.Saver. Each snippet includes comments explaining the purpose of each section.

### Program.cs
```csharp
using Microsoft.AspNetCore.Identity;
using MudBlazor.Services;
using Synapsers.MudBlazor.ThemeManager.Saver.Extensions;
using Synapsers.MudBlazor.ThemeManager.Saver.Services;
using TestApp.Components;

var builder = WebApplication.CreateBuilder(args);

// Register MudBlazor services for UI components
builder.Services.AddMudServices();
 
// Register Razor components and enable interactive server-side rendering
builder.Services.AddRazorComponents()
    .AddInteractiveServerComponents();

// Register theme persistence services for saving/loading theme settings
builder.Services.AddThemePersistence();
// Register ThemeStateService as singleton to preserve theme state across navigation
builder.Services.AddSingleton<ThemeStateService>();

// Configure authentication using ASP.NET Core Identity cookies
builder.Services.AddAuthentication(options =>
{
    options.DefaultScheme = IdentityConstants.ApplicationScheme;
    options.DefaultSignInScheme = IdentityConstants.ExternalScheme;
})
    .AddIdentityCookies();

var app = builder.Build();

// Configure the HTTP request pipeline
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error", createScopeForErrors: true);
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseAntiforgery();

// Map static assets and Razor components
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
```

### Components/Layout/MainLayout.razor
```razor
@using Synapsers.MudBlazor.ThemeManager.Saver.Services
@using Synapsers.MudBlazor.ThemeManager.Saver
@using MudBlazor
@inherits LayoutComponentBase
@inject ThemeStateService ThemeStateService
@inject NavigationManager NavigationManager

<!-- Provides the current theme and dark mode state to all MudBlazor components -->
<MudThemeProvider @ref="@_mudThemeProvider" Theme="ThemeStateService.ThemeManager.Theme" @bind-IsDarkMode="ThemeStateService.IsDarkMode" />
<MudPopoverProvider />
<MudDialogProvider />
<MudSnackbarProvider />
<MudLayout>
    <!-- App bar with menu and dark mode toggle -->
    <MudAppBar Elevation="ThemeStateService.ThemeManager.AppBarElevation">
        <MudIconButton Icon="@Icons.Material.Filled.Menu" Color="Color.Inherit" Edge="Edge.Start" OnClick="@((e) => DrawerToggle())" />
        <MudText Typo="Typo.h6" Class="ml-3">Theme Configuration</MudText>
        <MudSpacer />
        <!-- Button toggles dark/light mode -->
        <MudIconButton Icon="@(ThemeStateService.IsDarkMode ? Icons.Material.Filled.LightMode : Icons.Material.Filled.DarkMode)" 
                       Color="Color.Inherit" OnClick="@(async (e) => await DarkModeToggleAsync())" Edge="Edge.End" />
    </MudAppBar>
    <!-- Side navigation drawer -->
    <MudDrawer @bind-Open="_drawerOpen" ClipMode="ThemeStateService.ThemeManager.DrawerClipMode" Elevation="ThemeStateService.ThemeManager.DrawerElevation">
        <NavMenu />
    </MudDrawer>
    <!-- Main content area -->
    <MudMainContent>
        <MudContainer MaxWidth="MaxWidth.False" Class="mt-16 px-16">
            @Body
        </MudContainer>
    </MudMainContent>
    <!-- Button to open the theme manager panel -->
    <MudThemeManagerButton OnClick="@((e) => OpenThemeManager(true))" />
    <!-- Theme manager panel for customizing and saving the theme -->
    <MudThemeManager @ref="_themeManager" @key="_themeKey" Open="_themeManagerOpen" OpenChanged="OpenThemeManager" Theme="ThemeStateService.ThemeManager" ThemeChanged="UpdateThemeAsync" IsDarkMode="ThemeStateService.IsDarkMode" />
</MudLayout>
```

### Components/App.razor
```razor
<!DOCTYPE html>
<html lang="en">
<head>
    <meta charset="utf-8" /> <!-- Set character encoding -->
    <meta name="viewport" content="width=device-width, initial-scale=1.0" /> <!-- Responsive scaling -->
    <base href="/" /> <!-- Base URL for the app -->
    <!-- Google Fonts for consistent typography -->
    <link href="https://fonts.googleapis.com/css2?family=Montserrat:wght@400;700&family=Open+Sans&display=swap" rel="stylesheet" />
    <link href="https://fonts.googleapis.com/css2?family=Ubuntu:wght@300;400;500;700&display=swap" rel="stylesheet">
    <link href="https://fonts.googleapis.com/css2?family=Montserrat:wght@400;500;600;700&display=swap" rel="stylesheet">
    <link href="https://fonts.googleapis.com/css?family=Roboto:300,400,500,700&display=swap" rel="stylesheet" />
    <!-- ThemeManager and MudBlazor stylesheets -->
    <link href=@Assets["_content/Synapsers.MudBlazor.ThemeManager.Saver/MudBlazorThemeManager.css"] rel="stylesheet" />
    <link href=@Assets["_content/MudBlazor/MudBlazor.min.css"] rel="stylesheet" />
    <ImportMap /> <!-- Import map for JS modules -->
    <link rel="icon" type="image/ico" href="favicon.ico" /> <!-- Favicon -->
    <HeadOutlet @rendermode="InteractiveServer" /> <!-- Blazor head outlet -->
</head>
<body>
    <Routes @rendermode="InteractiveServer" /> <!-- Main routing outlet -->
    <script src="_framework/blazor.web.js"></script> <!-- Blazor runtime -->
    <script src=@Assets["_content/MudBlazor/MudBlazor.min.js"]></script> <!-- MudBlazor JS -->
    <script src=@Assets["_content/Extensions.MudBlazor.StaticInput/NavigationObserver.js"]></script> <!-- Navigation observer for extensions -->
</body>
</html>
```

### Components/Pages/Home.razor
```razor
@page "/" 

<PageTitle>Theme Configuration</PageTitle>

<!-- Page header -->
<MudText Typo="Typo.h3" GutterBottom="true">Theme Configuration</MudText>
<!-- Instructions for the user -->
<MudText Typo="Typo.body1" Class="mb-6">
    Configure and customize your application's theme using the Theme Manager. Click the settings button on the right side of the screen to open the theme configuration panel.
</MudText>

<!-- Grid layout to showcase MudBlazor components -->
<MudGrid Spacing="6">
    <!-- Button variants -->
    <MudItem xs="12" sm="6" md="4">
        <MudPaper Class="pa-4 d-flex justify-center flex-wrap" Height="100%">
            <MudButton Variant="Variant.Filled" Class="ma-4">Default</MudButton> <!-- Default button -->
            <MudButton Variant="Variant.Filled" Color="Color.Primary" Class="ma-4">Primary</MudButton> <!-- Primary button -->
            <MudButton Variant="Variant.Filled" Color="Color.Secondary" Class="ma-4">Secondary</MudButton> <!-- Secondary button -->
            <MudButton Variant="Variant.Filled" Disabled="true" Class="ma-4">Disabled</MudButton> <!-- Disabled button -->
        </MudPaper>
    </MudItem>
    <MudItem xs="12" sm="6" md="4">
        <MudPaper Class="pa-4 d-flex justify-center flex-wrap" Height="100%">
            <MudTextField Label="Primary Color" Variant="Variant.Filled" Class="ma-4" />
            <MudTextField Label="Secondary Color" Variant="Variant.Filled" Class="ma-4" />
            <MudTextField Label="Error Color" Variant="Variant.Filled" Class="ma-4" />
            <MudTextField Label="Warning Color" Variant="Variant.Filled" Class="ma-4" />
        </MudPaper>
    </MudItem>
    <MudItem xs="12" sm="6" md="4">
        <MudPaper Class="pa-4 d-flex justify-center flex-wrap" Height="100%">
            <MudSwitch Label="Dark Mode" @bind-Checked="ThemeStateService.IsDarkMode" Class="ma-4" />
            <MudButton Variant="Variant.Filled" Color="Color.Success" Class="ma-4" OnClick="@(async () => await SaveThemeAsync())">Save Theme</MudButton>
        </MudPaper>
    </MudItem>
</MudGrid>

<MudAlert Severity="Severity.Info" Class="mt-4">
    Theme changes are applied instantly. Use the buttons above to customize your theme colors and settings.
</MudAlert>
```

### Components/_Imports.razor
```razor
@using System.Net.Http           // Enables HTTP requests
@using System.Net.Http.Json      // Enables JSON HTTP requests
@using Microsoft.AspNetCore.Components.Forms // For forms
@using Microsoft.AspNetCore.Components.Routing // For routing
@using Microsoft.AspNetCore.Components.Web // For web events
@using static Microsoft.AspNetCore.Components.Web.RenderMode // For render modes
@using Microsoft.AspNetCore.Components.Web.Virtualization // For virtualization
@using Microsoft.JSInterop // For JS interop
@using MudBlazor                 // MudBlazor components
@using MudBlazor.Services        // MudBlazor services
@using Synapsers.MudBlazor.ThemeManager.Saver // Theme manager
@using TestApp // App namespace
@using TestApp.Components // Components namespace
```

### Components/Routes.razor
```razor
<Router AppAssembly="typeof(Program).Assembly">
    <Found Context="routeData">
        <!-- Renders the matched page with the default layout -->
        <RouteView RouteData="routeData" DefaultLayout="typeof(Layout.MainLayout)" />
        <!-- Focuses on the first h1 after navigation for accessibility -->
        <FocusOnNavigate RouteData="routeData" Selector="h1" />
    </Found>
</Router>
```

---
This project serves as a reference for integrating advanced theme management and persistence in Blazor Server applications using MudBlazor.
