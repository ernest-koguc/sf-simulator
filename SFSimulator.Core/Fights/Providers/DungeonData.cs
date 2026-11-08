using System.Text.Json.Serialization;

namespace SFSimulator.Core;

internal sealed class DungeonData
{
    [JsonPropertyName("id")]
    public required int Id { get; init; }

    [JsonPropertyName("armor_multiplier")]
    public double? ArmorMultiplier { get; init; }

    [JsonPropertyName("floors")]
    public required Dictionary<int, DungeonFloorData> Floors { get; init; }
}

internal sealed class DungeonFloorData
{
    [JsonPropertyName("pos")]
    public required int Position { get; init; }

    [JsonPropertyName("class")]
    public ClassType? Class { get; init; }

    [JsonPropertyName("level")]
    public required int Level { get; init; }

    [JsonPropertyName("str")]
    public int Strength { get; init; }

    [JsonPropertyName("dex")]
    public int Dexterity { get; init; }

    [JsonPropertyName("int")]
    public int Intelligence { get; init; }

    [JsonPropertyName("con")]
    public int Constitution { get; init; }

    [JsonPropertyName("lck")]
    public int Luck { get; init; }

    [JsonPropertyName("health")]
    public long Health { get; init; }

    [JsonPropertyName("min")]
    public int MinWeaponDamage { get; init; }

    [JsonPropertyName("max")]
    public int MaxWeaponDamage { get; init; }

    [JsonPropertyName("armor")]
    public int? Armor { get; init; }

    [JsonPropertyName("runes")]
    public DungeonRuneData? Runes { get; init; }
}

internal sealed class DungeonRuneData
{
    [JsonPropertyName("type")]
    public required int Type { get; init; }

    [JsonPropertyName("res")]
    public required int[] Resistances { get; init; }

    [JsonPropertyName("damage")]
    public required int Damage { get; init; }
}

[JsonSourceGenerationOptions(GenerationMode = JsonSourceGenerationMode.Metadata)]
[JsonSerializable(typeof(Dictionary<int, DungeonData>))]
internal partial class DungeonDataJsonContext : JsonSerializerContext;