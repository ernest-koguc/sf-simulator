using System.Text.Json;
using System.Text.Json.Serialization;

namespace SFSimulator.Core;

public class Maria21DataDTO
{
    public Toilet Toilet { get; set; } = null!;
    public string Name { get; set; } = null!;
    public string Prefix { get; set; } = null!;
    public ClassType Class { get; set; }
    public int XP { get; set; }
    public int Level { get; set; }
    public MountType Mount { get; set; }
    public int Book { get; set; }
    public int CalendarDay { get; set; }
    public int CalendarType { get; set; }
    public List<Potion> Potions { get; set; } = null!;

    public Inventory Inventory { get; set; } = null!;
    public Fortress Fortress { get; set; } = null!;
    public Underworld Underworld { get; set; } = null!;
    public SFToolsDungeonData Dungeons { get; set; } = null!;
    public Runes Runes { get; set; } = null!;
    public PlayerAttribute Strength { get; set; } = null!;
    public PlayerAttribute Dexterity { get; set; } = null!;
    public PlayerAttribute Intelligence { get; set; } = null!;
    public PlayerAttribute Constitution { get; set; } = null!;
    public PlayerAttribute Luck { get; set; } = null!;
    public int Armor { get; set; }
    public Slots Items { get; set; } = null!;
    public GroupData Group { get; set; } = null!;
    public SFCompanions? Companions { get; set; } = null!;
    public SFToolsPets Pets { get; set; } = null!;
    public SFToolsRunes? Idle { get; set; } = null!;
}

public class SFToolsRunes
{
    [JsonConverter(typeof(RunesJsonConverter))]
    public int Runes { get; set; }
}

public class Toilet
{
    public int Aura { get; set; }
    public int Fill { get; set; }
    public int Capacity { get; set; }
}

public class SFToolsPets
{
    public int ShadowFood { get; set; }
    public int LightFood { get; set; }
    public int EarthFood { get; set; }
    public int FireFood { get; set; }
    public int WaterFood { get; set; }
    public List<int> ShadowLevels { get; set; } = [];
    public List<int> LightLevels { get; set; } = [];
    public List<int> EarthLevels { get; set; } = [];
    public List<int> FireLevels { get; set; } = [];
    public List<int> WaterLevels { get; set; } = [];
    public int ShadowCount { get; set; }
    public int LightCount { get; set; }
    public int EarthCount { get; set; }
    public int FireCount { get; set; }
    public int WaterCount { get; set; }
    public int ShadowLevel { get; set; }
    public int LightLevel { get; set; }
    public int EarthLevel { get; set; }
    public int FireLevel { get; set; }
    public int WaterLevel { get; set; }
    public int Shadow { get; set; }
    public int Light { get; set; }
    public int Earth { get; set; }
    public int Fire { get; set; }
    public int Water { get; set; }
    public List<int> Dungeons { get; set; } = [];
}

public class SFCompanions
{
    public SFToolsCompanion Bert { get; set; } = null!;
    public SFToolsCompanion Kunigunde { get; set; } = null!;
    public SFToolsCompanion Mark { get; set; } = null!;
}

public class SFToolsCompanion
{
    public ClassType Class { get; set; }
    public int Armor { get; set; }
    public PlayerAttribute Strength { get; set; } = null!;
    public PlayerAttribute Dexterity { get; set; } = null!;
    public PlayerAttribute Intelligence { get; set; } = null!;
    public PlayerAttribute Constitution { get; set; } = null!;
    public PlayerAttribute Luck { get; set; } = null!;
    public Slots Items { get; set; } = null!;
}

public class Inventory
{
    public Slots Dummy { get; set; } = null!;
}
public class Potion
{
    public PotionType Type { get; set; }
    public int Size { get; set; }

    public static Potion Eternity => new Potion() { Size = 25, Type = PotionType.Eternity };
}
public class Slots
{
    public SFToolsItem Head { get; set; } = null!;
    public SFToolsItem Body { get; set; } = null!;
    public SFToolsItem Hand { get; set; } = null!;
    public SFToolsItem Feet { get; set; } = null!;
    public SFToolsItem Neck { get; set; } = null!;
    public SFToolsItem Belt { get; set; } = null!;
    public SFToolsItem Ring { get; set; } = null!;
    public SFToolsItem Misc { get; set; } = null!;
    public SFToolsItem Wpn1 { get; set; } = null!;
    public SFToolsItem Wpn2 { get; set; } = null!;
}

public class GroupData
{
    public InnerGroupData? Group { get; set; } = null!;
}

public class InnerGroupData
{
    public int Raid { get; set; }
    public int Hydra { get; set; }
    public int TotalInstructor { get; set; }
    public int TotalTreasure { get; set; }
}

public class SFToolsItem
{
    public int DamageMin { get; set; }
    public int DamageMax { get; set; }

    public int Armor { get; set; }

    public WitchScrollType Enchantment { get; set; }

    public RuneType RuneType { get; set; }
    public int RuneValue { get; set; }

    public bool HasSocket { get; set; }
    public bool HasGem { get; set; }
    public GemType GemType { get; set; }
    public int GemValue { get; set; }

    public int Upgrades { get; set; }
    public decimal UpgradeMultiplier { get; set; }

    public ItemType Type { get; set; }

    public List<int> AttributeTypes { get; set; } = null!;
    public List<int> Attributes { get; set; } = null!;
    public int Index { get; set; }
}


public class PlayerAttribute
{
    public int Total { get; set; }
    public int Base { get; set; }
    public int Bonus { get; set; }
}

public class SFToolsDungeonData
{
    public int Tower { get; set; }
    public int Group { get; set; }
    public int Player { get; set; }
    public int[] Normal { get; set; } = default!;
    public int[] Shadow { get; set; } = default!;
    public int Twister { get; set; }
    public int Youtube { get; set; }
}

public class Fortress
{
    public int Knights { get; set; }
    public int LaborerQuarters { get; set; }
    public int GemMine { get; set; }
    public int Academy { get; set; }
    public int Treasury { get; set; }
    public int Gladiator { get; set; }
}

public class Runes
{
    public int Gold { get; set; }
    public int XP { get; set; }
    public int Health { get; set; }
    public int ResistanceFire { get; set; }
    public int ResistanceCold { get; set; }
    public int ResistanceLightning { get; set; }
}

public class Underworld
{
    public int GoldPit { get; set; }
    public int TimeMachine { get; set; }
}

public class Witch
{
    public int Stage { get; set; }
    public int Items { get; set; }
    public int ItemsNext { get; set; }
    public int Item { get; set; }
}

public class RunesJsonConverter : JsonConverter<int>
{
    public override int Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
    {
        using var doc = JsonDocument.ParseValue(ref reader);
        var val = doc.RootElement.GetRawText();
        if (string.IsNullOrWhiteSpace(val))
            return 0;
        var vals = val.Split("e+");

        if (vals.Length > 1)
            return int.Parse(vals[1]);

        var longVal = long.Parse(vals[0]);
        return (int)Math.Floor(Math.Log10(longVal));
    }
    public override void Write(Utf8JsonWriter writer, int value, JsonSerializerOptions options)
    {
        writer.WriteStringValue("1E+" + value);
    }
}