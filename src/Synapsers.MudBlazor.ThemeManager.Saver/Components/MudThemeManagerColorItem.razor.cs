using Microsoft.AspNetCore.Components;
using MudBlazor;
using MudBlazor.Utilities; // Keep this if MudColor uses it, though often not needed directly by user

namespace Synapsers.MudBlazor.ThemeManager.Saver;

public partial class MudThemeManagerColorItem : ComponentBase
{
    private bool _isOpen;
    private bool _isColorModifiedByUser;

    // This will hold the MudColor object for the picker's internal binding
    private MudColor _internalPickerColorForBinding;

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
    public string Color { get; set; } = ""; // This is your string representation

    [Parameter]
    public EventCallback<string> ColorChanged { get; set; }

    protected override void OnInitialized()
    {
        // Initialize Color string if it's empty
        if (string.IsNullOrEmpty(Color))
        {
            Color = ThemeColor ?? "";
        }
        // Initialize the _internalPickerColorForBinding from the Color string
        UpdateInternalPickerColorFromString(Color);
    }

    protected override void OnParametersSet()
    {
        // If ThemeColor (an external source) changes and user hasn't modified it, update.
        if (!_isColorModifiedByUser && !string.IsNullOrEmpty(ThemeColor) && ThemeColor != Color)
        {
            Color = ThemeColor;
            UpdateInternalPickerColorFromString(Color);
        }
        // If Color is somehow reset externally and ThemeColor is available
        else if (string.IsNullOrEmpty(Color) && !string.IsNullOrEmpty(ThemeColor))
        {
            Color = ThemeColor;
            UpdateInternalPickerColorFromString(Color);
        }
        // Fallback to persisted/default theme color from ThemeManager if Color is still not set
        else if (ThemeManager != null && string.IsNullOrEmpty(Color))
        {
            string? paletteColor = ThemeManager.GetPaletteColorValue(ColorType);
            if (!string.IsNullOrEmpty(paletteColor))
            {
                Color = paletteColor;
            }
            else if (!string.IsNullOrEmpty(ThemeColor)) // Fallback to initial ThemeColor if paletteColor is also null
            {
                Color = ThemeColor;
            }
            UpdateInternalPickerColorFromString(Color);
        }
        
        // The line below was correctly commented out from previous advice
        // _isOpen = false; 
    }
    private void UpdateInternalPickerColorFromString(string? colorStr)
    {
        if (!string.IsNullOrEmpty(colorStr))
        {
            try
            {
                _internalPickerColorForBinding = new MudColor(colorStr);
            }
            catch (ArgumentException) // Handle cases where the string is not a valid color
            {
                // Use string literal for Grey's default color
                _internalPickerColorForBinding = new MudColor("#9E9E9E"); // Default Grey
                                                                          // Optionally log this error
            }
        }
        else
        {
            // Use string literal for Transparent
            _internalPickerColorForBinding = new MudColor("transparent");
        }
    }

    public void ToggleOpen()
    {
        _isOpen = !_isOpen;
        // No StateHasChanged needed here if MudPopover's Open=@_isOpen handles it
    }

    // Called continuously by MudColorPicker's ValueChanged as the user drags
    private void OnPickerValueChanged(MudColor newMudColor)
    {
        _internalPickerColorForBinding = newMudColor;
        Color = newMudColor.ToString(MudColorOutputFormats.Hex); // Update the string version for the swatch display
        // DO NOT do heavy updates here. Only update what's needed for live preview within this component.
        // We need StateHasChanged if the swatch div relies on the 'Color' property directly and isn't updated by the picker binding.
        InvokeAsync(StateHasChanged); // To update the color swatch div
    }

    // Called when the user releases the mouse button after picking/dragging
    private async Task OnPickerSelectionCommitted()
    {
        if (_internalPickerColorForBinding == null) return;

        // This is where we commit the change and notify the parent
        _isColorModifiedByUser = true;
        string finalColorValue = _internalPickerColorForBinding.ToString(MudColorOutputFormats.Hex);
        Color = finalColorValue; // Ensure string Color is definitely the final committed value

        if (ColorChanged.HasDelegate)
        {
            await ColorChanged.InvokeAsync(finalColorValue).ConfigureAwait(false);
        }

        await UpdateColor(finalColorValue).ConfigureAwait(false); // This calls ThemeManager.UpdatePalette

        // StateHasChanged might not be strictly necessary here if the parent's re-render updates this component,
        // but can be kept if specific visual updates in this component depend on the committed state.
        // await InvokeAsync(StateHasChanged);

        _isColorModifiedByUser = false;
    }

    // Your existing UpdateColor method
    public async Task UpdateColor(string value)
    {
        if (string.IsNullOrEmpty(value))
            return;

        var newPaletteColor = new ThemeUpdatedValue
        {
            ColorStringValue = value,
            ThemePaletteColor = ColorType
        };
        await ThemeManager.UpdatePalette(newPaletteColor).ConfigureAwait(false);
    }
}