using System.Text.Json.Serialization;

namespace MudBlazor.ThemeManager.Saver.Extensions;

[JsonSerializable(typeof(MudTheme))]
[JsonSerializable(typeof(Shadow))]
[JsonSerializable(typeof(LayoutProperties))]
[JsonSerializable(typeof(ZIndex))]
[JsonSerializable(typeof(PseudoCss))]
internal sealed partial class ThemeSerializerContext : JsonSerializerContext;