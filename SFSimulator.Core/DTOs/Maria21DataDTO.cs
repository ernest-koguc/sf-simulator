namespace SFSimulator.Core;

public class Maria21DataDTO
{
    public string Name { get; set; } = null!;
    public string Prefix { get; set; } = null!;
    public ClassType Class { get; set; }
    public int XP { get; set; }
    public int Level { get; set; }
    public int Mount { get; set; }
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
    public Companions Companions { get; set; } = null!;
}

public class Companions
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
    public SFToolsWeapon Wpn1 { get; set; } = null!;
    public SFToolsWeapon Wpn2 { get; set; } = null!;
}

public class SFToolsWeapon
{
    public int DamageMin { get; set; }
    public int DamageMax { get; set; }

    public WitchScrollType Enchantment { get; set; }

    public RuneType RuneType { get; set; }
    public int RuneValue { get; set; }

    public bool HasSocket { get; set; }
    public GemType GemType { get; set; }
    public int GemValue { get; set; }

    public int Upgrades { get; set; }

    public ItemAttribute Strength { get; set; } = null!;
    public ItemAttribute Dexterity { get; set; } = null!;
    public ItemAttribute Intelligence { get; set; } = null!;
    public ItemAttribute Constitution { get; set; } = null!;
    public ItemAttribute Luck { get; set; } = null!;
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
    public int Armor { get; set; }
    public WitchScrollType Enchantment { get; set; }
    public bool HasSocket { get; set; }
    public bool HasGem { get; set; }
    public RuneType RuneType { get; set; }
    public int RuneValue { get; set; }
    public GemType GemType { get; set; }
    public int GemValue { get; set; }
    public int Upgrades { get; set; }
    public ItemAttribute Strength { get; set; } = null!;
    public ItemAttribute Dexterity { get; set; } = null!;
    public ItemAttribute Intelligence { get; set; } = null!;
    public ItemAttribute Constitution { get; set; } = null!;
    public ItemAttribute Luck { get; set; } = null!;
}

public class ItemAttribute
{
    public int Value { get; set; }
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
}

public class Fortress
{
    // TODO WORKER IN FORTRESS LOGIC
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
