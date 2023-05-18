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
        public Fortress Fortress { get; set; } = null!;
        public Underworld Underworld { get; set; } = null!;
        public Dungeons Dungeons { get; set; } = null!;
        public Runes Runes { get; set; } = null!;
        public Strength Strength { get; set; } = null!;
        public Dexterity Dexterity { get; set; } = null!;
        public Intelligence Intelligence { get; set; } = null!;
        public Constitution Constitution { get; set; } = null!;
        public Items Items { get; set; } = null!;
        public GroupData Group { get; set; } = null!;
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
    public class Items
    {
        public Head Head { get; set; } = null!;
        public Neck Neck { get; set; } = null!;
        public Ring Ring { get; set; } = null!;
    }

    public class Head
    {
        public int Enchantment { get; set; }
    }
    public class Neck
    {
        public int Enchantment { get; set; }
    }
    public class Ring
    {
        public int Enchantment { get; set; }
    }
    public class Constitution
    {
        public int Type { get; set; }
        public int Value { get; set; }
        public int Base { get; set; }
    }

    public class Dexterity
    {
        public int Type { get; set; }
        public int Value { get; set; }
        public int Base { get; set; }
    }

    public class Dungeons
    {
        public int Raid { get; set; }
        public int Tower { get; set; }
    }

    public class Fortress
    {
        // TODO WORKER IN FORTRESS LOGIC
        public int LaborerQuarters { get; set; }
        public int GemMine { get; set; }
        public int Academy { get; set; }
        public int Treasury { get; set; }
    }

    public class Intelligence
    {
        public int Type { get; set; }
        public int Value { get; set; }
        public int Base { get; set; }
    }


    public class Runes
    {
        public int Gold { get; set; }
        public int XP { get; set; }
    }
    public class Strength
    {
        public int Type { get; set; }
        public int Value { get; set; }
        public int Base { get; set; }
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
