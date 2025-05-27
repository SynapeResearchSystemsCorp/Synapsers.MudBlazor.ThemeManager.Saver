using Microsoft.AspNetCore.Components;
using MudBlazor;
using MudBlazor.State;
using MudBlazor.Utilities;
using Synapsers.MudBlazor.ThemeManager.Saver.Extensions;

namespace Synapsers.MudBlazor.ThemeManager.Saver;

public partial class MudThemeManager : ComponentBaseWithState
{
    private static readonly PaletteLight DefaultPaletteLight = new();
    private static readonly PaletteDark DefaultPaletteDark = new();
    private readonly ParameterState<bool> _openState;
    private readonly ParameterState<bool> _isDarkModeState;

    private PaletteLight? _currentPaletteLight;
    private PaletteDark? _currentPaletteDark;
    private Palette _currentPalette;
    private MudTheme? _customTheme;

    public MudThemeManager()
    {
        using var registerScope = CreateRegisterScope();
        _openState = registerScope.RegisterParameter<bool>(nameof(Open))
            .WithParameter(() => Open)
            .WithEventCallback(() => OpenChanged);
        _isDarkModeState = registerScope.RegisterParameter<bool>(nameof(IsDarkMode))
            .WithParameter(() => IsDarkMode)
            .WithChangeHandler(OnIsDarkModeChanged);
        _currentPalette = GetPalette();
    }

    public string ThemePresets { get; set; } = "Not Implemented";

    [Parameter]
    public bool Open { get; set; }

    [Parameter]
    public EventCallback<bool> OpenChanged { get; set; }

    [Parameter]
    public ThemeManagerTheme? Theme { get; set; }

    [Parameter]
    public bool IsDarkMode { get; set; }

    [Parameter]
    public ColorPickerView ColorPickerView { get; set; } = ColorPickerView.Spectrum;

    [Parameter]
    public EventCallback<ThemeManagerTheme> ThemeChanged { get; set; }

    protected override void OnInitialized()
    {
        base.OnInitialized();
        InitializeThemeObjects();
    }

    protected override void OnParametersSet()
    {
        base.OnParametersSet();
        // Always re-initialize palette from Theme parameter to reflect latest JSON/theme changes
        InitializeThemeObjects();
    }

    private void InitializeThemeObjects()
    {
        if (Theme is null)
        {
            _currentPaletteLight = DefaultPaletteLight.DeepClone();
            _currentPaletteDark = DefaultPaletteDark.DeepClone();
            _customTheme = new MudTheme
            {
                PaletteLight = _currentPaletteLight!,
                PaletteDark = _currentPaletteDark!
            };
        }
        else
        {
            _customTheme = Theme.Theme.DeepClone();
            _currentPaletteLight = Theme.Theme.PaletteLight?.DeepClone() ?? DefaultPaletteLight.DeepClone();
            _currentPaletteDark = Theme.Theme.PaletteDark?.DeepClone() ?? DefaultPaletteDark.DeepClone();
        }
        _currentPalette = GetPalette();
    }    public Task UpdatePalette(ThemeUpdatedValue value)
    {
        ArgumentNullException.ThrowIfNull(value);

        if (Theme is null || _customTheme is null)
        {
            return Task.CompletedTask;
        }

        // Update both the theme palette and local palette objects
        Palette targetThemePalette;
        Palette targetLocalPalette;
        
        if (_isDarkModeState.Value)
        {
            targetThemePalette = _customTheme.PaletteDark;
            targetLocalPalette = _currentPaletteDark!;
        }
        else
        {
            targetThemePalette = _customTheme.PaletteLight;
            targetLocalPalette = _currentPaletteLight!;
        }

        // Update both palettes with the new color value
        UpdatePaletteColor(targetThemePalette, value);
        UpdatePaletteColor(targetLocalPalette, value);

        // Update the current palette reference
        _currentPalette = GetPalette();
        
        // Assign the updated theme back to the Theme object
        Theme.Theme = _customTheme;

        // Force state update to refresh the UI
        StateHasChanged();

        return UpdateThemeChangedAsync();
    }

    private static void UpdatePaletteColor(Palette palette, ThemeUpdatedValue value)
    {
        switch (value.ThemePaletteColor)
        {
            case ThemePaletteColor.Primary:
                palette.Primary = value.ColorStringValue;
                break;
            case ThemePaletteColor.Secondary:
                palette.Secondary = value.ColorStringValue;
                break;
            case ThemePaletteColor.Tertiary:
                palette.Tertiary = value.ColorStringValue;
                break;
            case ThemePaletteColor.Info:
                palette.Info = value.ColorStringValue;
                break;
            case ThemePaletteColor.Success:
                palette.Success = value.ColorStringValue;
                break;
            case ThemePaletteColor.Warning:
                palette.Warning = value.ColorStringValue;
                break;
            case ThemePaletteColor.Error:
                palette.Error = value.ColorStringValue;
                break;
            case ThemePaletteColor.Dark:
                palette.Dark = value.ColorStringValue;
                break;
            case ThemePaletteColor.Surface:
                palette.Surface = value.ColorStringValue;
                break;
            case ThemePaletteColor.Background:
                palette.Background = value.ColorStringValue;
                break;
            case ThemePaletteColor.BackgroundGray:
                palette.BackgroundGray = value.ColorStringValue;
                break;
            case ThemePaletteColor.DrawerText:
                palette.DrawerText = value.ColorStringValue;
                break;
            case ThemePaletteColor.DrawerIcon:
                palette.DrawerIcon = value.ColorStringValue;
                break;
            case ThemePaletteColor.DrawerBackground:
                palette.DrawerBackground = value.ColorStringValue;
                break;
            case ThemePaletteColor.AppbarText:
                palette.AppbarText = value.ColorStringValue;
                break;
            case ThemePaletteColor.AppbarBackground:
                palette.AppbarBackground = value.ColorStringValue;
                break;
            case ThemePaletteColor.LinesDefault:
                palette.LinesDefault = value.ColorStringValue;
                break;
            case ThemePaletteColor.LinesInputs:
                palette.LinesInputs = value.ColorStringValue;
                break;
            case ThemePaletteColor.Divider:
                palette.Divider = value.ColorStringValue;
                break;
            case ThemePaletteColor.DividerLight:
                palette.DividerLight = value.ColorStringValue;
                break;
            case ThemePaletteColor.TextPrimary:
                palette.TextPrimary = value.ColorStringValue;
                break;
            case ThemePaletteColor.TextSecondary:
                palette.TextSecondary = value.ColorStringValue;
                break;
            case ThemePaletteColor.TextDisabled:
                palette.TextDisabled = value.ColorStringValue;
                break;
            case ThemePaletteColor.ActionDefault:
                palette.ActionDefault = value.ColorStringValue;
                break;
            case ThemePaletteColor.ActionDisabled:
                palette.ActionDisabled = value.ColorStringValue;
                break;
            case ThemePaletteColor.ActionDisabledBackground:
                palette.ActionDisabledBackground = value.ColorStringValue;
                break;
        }
    }

    private Task UpdateOpenValueAsync() => _openState.SetValueAsync(false);    private async Task UpdateThemeChangedAsync()
    {
        await ThemeChanged.InvokeAsync(Theme).ConfigureAwait(false);
        await InvokeAsync(StateHasChanged).ConfigureAwait(false);
    }

    private void OnIsDarkModeChanged(ParameterChangedEventArgs<bool> arg)
    {
        if (_customTheme is not null)
        {
            UpdateCustomTheme();
        }
    }

    private Task OnDrawerClipModeAsync(DrawerClipMode value)
    {
        if (Theme is null)
        {
            return Task.CompletedTask;
        }

        Theme.DrawerClipMode = value;

        return UpdateThemeChangedAsync();
    }

    private Task OnDefaultBorderRadiusAsync(int value)
    {
        if (Theme is null)
        {
            return Task.CompletedTask;
        }

        if (_customTheme is null)
        {
            return Task.CompletedTask;
        }

        Theme.DefaultBorderRadius = value;
        var newBorderRadius = _customTheme.LayoutProperties;

        newBorderRadius.DefaultBorderRadius = $"{value}px";

        _customTheme.LayoutProperties = newBorderRadius;
        Theme.Theme = _customTheme;

        return UpdateThemeChangedAsync();
    }

    private Task OnDefaultElevationAsync(int value)
    {
        if (Theme is null || _customTheme is null)
        {
            return Task.CompletedTask;
        }

        Theme.DefaultElevation = value;
        var newDefaultElevation = _customTheme.Shadows;

        string newElevation = newDefaultElevation.Elevation[value];
        newDefaultElevation.Elevation[1] = newElevation;

        _customTheme.Shadows.Elevation[1] = newElevation;
        Theme.Theme = _customTheme;

        return UpdateThemeChangedAsync();
    }

    private Task OnAppBarElevationAsync(int value)
    {
        if (Theme is null)
        {
            return Task.CompletedTask;
        }

        Theme.AppBarElevation = value;

        return UpdateThemeChangedAsync();
    }

    private Task OnDrawerElevationAsync(int value)
    {
        if (Theme is null)
        {
            return Task.CompletedTask;
        }

        Theme.DrawerElevation = value;

        return UpdateThemeChangedAsync();
    }

    private Task OnFontFamilyAsync(string value)
    {
        if (Theme is null || _customTheme is null)
        {
            return Task.CompletedTask;
        }

        Theme.FontFamily = value;
        var newTypography = _customTheme.Typography;

        newTypography.Body1.FontFamily = new[] { value, "Helvetica", "Arial", "sans-serif" };
        newTypography.Body2.FontFamily = new[] { value, "Helvetica", "Arial", "sans-serif" };
        newTypography.Button.FontFamily = new[] { value, "Helvetica", "Arial", "sans-serif" };
        newTypography.Caption.FontFamily = new[] { value, "Helvetica", "Arial", "sans-serif" };
        newTypography.Default.FontFamily = new[] { value, "Helvetica", "Arial", "sans-serif" };
        newTypography.H1.FontFamily = new[] { value, "Helvetica", "Arial", "sans-serif" };
        newTypography.H2.FontFamily = new[] { value, "Helvetica", "Arial", "sans-serif" };
        newTypography.H3.FontFamily = new[] { value, "Helvetica", "Arial", "sans-serif" };
        newTypography.H4.FontFamily = new[] { value, "Helvetica", "Arial", "sans-serif" };
        newTypography.H5.FontFamily = new[] { value, "Helvetica", "Arial", "sans-serif" };
        newTypography.H6.FontFamily = new[] { value, "Helvetica", "Arial", "sans-serif" };
        newTypography.Overline.FontFamily = new[] { value, "Helvetica", "Arial", "sans-serif" };
        newTypography.Subtitle1.FontFamily = new[] { value, "Helvetica", "Arial", "sans-serif" };
        newTypography.Subtitle2.FontFamily = new[] { value, "Helvetica", "Arial", "sans-serif" };

        _customTheme.Typography = newTypography;
        Theme.Theme = _customTheme;

        return UpdateThemeChangedAsync();
    }

    private void UpdateCustomTheme()
    {
        if (_customTheme is null)
        {
            return;
        }

        if (_currentPaletteLight is not null)
        {
            _customTheme.PaletteLight = _currentPaletteLight;
        }

        if (_currentPaletteDark is not null)
        {
            _customTheme.PaletteDark = _currentPaletteDark;
        }

        _currentPalette = GetPalette();
    }

    private Palette GetPalette() => _isDarkModeState.Value
        ? _currentPaletteDark ?? DefaultPaletteDark
        : _currentPaletteLight ?? DefaultPaletteLight;

    /// <summary>
    /// Gets the current palette color value for the specified color type
    /// </summary>
    /// <param name="colorType">The color type to retrieve</param>
    /// <returns>The color value as a string, or null if not found</returns>
    public string? GetPaletteColorValue(ThemePaletteColor colorType)
    {
        if (_customTheme is null)
        {
            return null;
        }

        var palette = GetPalette();
        
        return colorType switch
        {
            ThemePaletteColor.Primary => palette.Primary?.ToString(),
            ThemePaletteColor.Secondary => palette.Secondary?.ToString(),
            ThemePaletteColor.Tertiary => palette.Tertiary?.ToString(),
            ThemePaletteColor.Info => palette.Info?.ToString(),
            ThemePaletteColor.Success => palette.Success?.ToString(),
            ThemePaletteColor.Warning => palette.Warning?.ToString(),
            ThemePaletteColor.Error => palette.Error?.ToString(),
            ThemePaletteColor.Dark => palette.Dark?.ToString(),
            ThemePaletteColor.Surface => palette.Surface?.ToString(),
            ThemePaletteColor.Background => palette.Background?.ToString(),
            ThemePaletteColor.BackgroundGray => palette.BackgroundGray?.ToString(),
            ThemePaletteColor.DrawerText => palette.DrawerText?.ToString(),
            ThemePaletteColor.DrawerIcon => palette.DrawerIcon?.ToString(),
            ThemePaletteColor.DrawerBackground => palette.DrawerBackground?.ToString(),
            ThemePaletteColor.AppbarText => palette.AppbarText?.ToString(),
            ThemePaletteColor.AppbarBackground => palette.AppbarBackground?.ToString(),
            ThemePaletteColor.LinesDefault => palette.LinesDefault?.ToString(),
            ThemePaletteColor.LinesInputs => palette.LinesInputs?.ToString(),
            ThemePaletteColor.Divider => palette.Divider?.ToString(),
            ThemePaletteColor.DividerLight => palette.DividerLight?.ToString(),
            ThemePaletteColor.TextPrimary => palette.TextPrimary?.ToString(),
            ThemePaletteColor.TextSecondary => palette.TextSecondary?.ToString(),
            ThemePaletteColor.TextDisabled => palette.TextDisabled?.ToString(),
            ThemePaletteColor.ActionDefault => palette.ActionDefault?.ToString(),
            ThemePaletteColor.ActionDisabled => palette.ActionDisabled?.ToString(),
            ThemePaletteColor.ActionDisabledBackground => palette.ActionDisabledBackground?.ToString(),
            _ => null
        };
    }

    /// <summary>
    /// Refreshes the state of all color pickers. Call this after navigating back to the page.
    /// </summary>
    public void RefreshColorPickers()
    {
        // Re-initialize the theme objects to ensure they have the latest data
        InitializeThemeObjects();
        
        // Refresh the UI
        InvokeAsync(StateHasChanged);
    }
}