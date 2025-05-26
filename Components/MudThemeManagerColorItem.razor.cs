using Microsoft.AspNetCore.Components;
using MudBlazor.Utilities;

namespace MudBlazor.ThemeManager.Saver;

public partial class MudThemeManagerColorItem : ComponentBase
{
    private bool _isOpen;
    private bool _isColorModifiedByUser;

    [CascadingParameter]
    protected MudThemeManager ThemeManager { get; set; } = null!;

    [Parameter]
    public string? Name { get; set; }

    [Parameter]
    public string? ThemeColor { get; set; }

    [Parameter]
    public ThemePaletteColor ColorType { get; set; }

    [Parameter]
    public ColorPickerView ColorPickerView { get; set; } = ColorPickerView.Spectrum;

    [Parameter]
    public string Color { get; set; } = "";

    [Parameter]
    public EventCallback<string> ColorChanged { get; set; }

    protected override void OnInitialized()
    {
        if (string.IsNullOrEmpty(Color))
        {
            Color = ThemeColor ?? "";
        }
    }    protected override void OnParametersSet()
    {
        // Always update Color from ThemeColor when it changes, unless user has manually overridden it
        if (!_isColorModifiedByUser && !string.IsNullOrEmpty(ThemeColor))
        {
            Color = ThemeColor;
        }
        else if (string.IsNullOrEmpty(Color) && !string.IsNullOrEmpty(ThemeColor))
        {
            Color = ThemeColor;
        }
    }

    public void ToggleOpen()
    {
        _isOpen = !_isOpen;
    }    // Match the exact method signature used in the Razor file
    private async Task OnColorChanged(MudColor mudColor)
    {
        if (mudColor != null)
        {
            _isColorModifiedByUser = true;
            string colorValue = mudColor.ToString(MudColorOutputFormats.Hex);
            Color = colorValue;

            // First update our local color
            if (ColorChanged.HasDelegate)
            {
                await ColorChanged.InvokeAsync(colorValue).ConfigureAwait(false);
            }

            // Then update the theme palette with the new color
            await UpdateColor(colorValue).ConfigureAwait(false);
            
            // Reset the modification flag after the theme is updated so we can receive future theme updates
            _isColorModifiedByUser = false;
        }
    }

    public async Task UpdateColor(string value)
    {
        if (string.IsNullOrEmpty(value))
            return;
            
        var newPaletteColor = new ThemeUpdatedValue
        {
            ColorStringValue = value,
            ThemePaletteColor = ColorType
        };        // Update the palette with the new color
        await ThemeManager.UpdatePalette(newPaletteColor).ConfigureAwait(false);
    }
}