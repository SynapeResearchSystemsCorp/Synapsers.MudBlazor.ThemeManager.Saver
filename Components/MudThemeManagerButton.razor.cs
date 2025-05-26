using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Web;

namespace MudBlazor.ThemeManager.Saver;

public partial class MudThemeManagerButton : ComponentBase
{
    [Parameter]
    public EventCallback<MouseEventArgs> OnClick { get; set; }
}