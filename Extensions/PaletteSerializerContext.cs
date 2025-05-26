using System.Text.Json.Serialization;

namespace MudBlazor.ThemeManager.Saver.Extensions;

[JsonSerializable(typeof(Palette))]
[JsonSerializable(typeof(PaletteDark))]
[JsonSerializable(typeof(PaletteLight))]
internal sealed partial class PaletteSerializerContext : JsonSerializerContext;