using System.Text.Json.Serialization;
using MudBlazor;

namespace Synapsers.MudBlazor.ThemeManager.Saver.Extensions;

[JsonSerializable(typeof(Palette))]
[JsonSerializable(typeof(PaletteDark))]
[JsonSerializable(typeof(PaletteLight))]
internal sealed partial class PaletteSerializerContext : JsonSerializerContext;