namespace SFSimulator.Core
{
    public class Maria21DataDTO
    {
        public string Name { get; set; } = null!;
        public string Prefix { get; set; } = null!;
        public int Class { get; set; }
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
        public PlayerModelDungeonData Dungeons { get; set; } = null!;
        public Runes Runes { get; set; } = null!;
        public PlayerAttribute Strength { get; set; } = null!;
        public PlayerAttribute Dexterity { get; set; } = null!;
        public PlayerAttribute Intelligence { get; set; } = null!;
        public PlayerAttribute Constitution { get; set; } = null!;
        public PlayerAttribute Luck { get; set; } = null!;
        public int Armor { get; set; }
        public Slots Items { get; set; } = null!;
        public GroupData Group { get; set; } = null!;
    }
    public class Inventory
    {
        public Slots Dummy { get; set; } = null!;
    }
    public class Potion
    {
        public int Type { get; set; }
    }
    public class Slots
    {
        public PlayerItem Head { get; set; } = null!;
        public PlayerItem Body { get; set; } = null!;
        public PlayerItem Hand { get; set; } = null!;
        public PlayerItem Feet { get; set; } = null!;
        public PlayerItem Neck { get; set; } = null!;
        public PlayerItem Belt { get; set; } = null!;
        public PlayerItem Ring { get; set; } = null!;
        public PlayerItem Misc { get; set; } = null!;
        public WeaponDto Wpn1 { get; set; } = null!;
        public WeaponDto Wpn2 { get; set; } = null!;
    }
    public class WeaponDto
    {
        public int DamageMin { get; set; }
        public int DamageMax { get; set; }
        public bool HasEnchantment { get; set; }
        public int RuneType { get; set; }
        public int RuneValue { get; set; }

    }
    public class GroupData
    {
        public InnerGroupData Group { get; set; } = null!;
    }

    public class InnerGroupData
    {
        public int Raid { get; set; }
        public int Hydra { get; set; }
        public int TotalInstructor { get; set; }
        public int TotalTreasure { get; set; }

    }
    public class PlayerItem
    {
        public bool HasEnchantment { get; set; }
        public bool HasRune { get; set; }
        public int RuneType { get; set; }
        public int RuneValue { get; set; }
    }
    public class PlayerAttribute
    {
        public int Base { get; set; }
        public int Bonus { get; set; }
    }

    public class PlayerModelDungeonData
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
}
