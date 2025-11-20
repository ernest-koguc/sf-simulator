namespace SFSimulator.Core;

public class DungeonEnemy : IFightable<RawWeapon>
{
    public int Position { get; set; }
    public bool IsDefeated { get; set; }
    public string Name => DungeonEnemyName.GetName(Dungeon.Name, Position);
    public ClassType Class { get; set; }
    public int Level { get; set; }
    public int Strength { get; set; }
    public int Dexterity { get; set; }
    public int Intelligence { get; set; }
    public int Constitution { get; set; }
    public int Luck { get; set; }
    public long Health { get; set; }
    public int Armor { get; set; }
    public RawWeapon? FirstWeapon { get; set; }
    public RawWeapon? SecondWeapon { get; set; }
    public int Reaction { get; set; }
    public double CritMultiplier { get; set; } = 2;
    public int LightningResistance { get; set; }
    public int FireResistance { get; set; }
    public int ColdResistance { get; set; }
    public int HealthRune { get; set; }
    public double SoloPortal { get; } = 0;
    public double GuildPortal { get; } = 0;
    public Dungeon Dungeon { get; set; } = null!;

    public DungeonEnemy(int position,
        ClassType @class,
        int level,
        int strength,
        int dexterity,
        int intelligence,
        int constitution,
        int luck,
        long health,
        int minWeaponDmg,
        int maxWeaponDmg,
        DungeonEnemyRuneBonuses? dungeonRuneBonuses = null,
        int? armor = null,
        double? armorMultiplier = null)
    {
        Position = position;
        Class = @class;
        Level = level;
        Strength = strength;
        Dexterity = dexterity;
        Intelligence = intelligence;
        Constitution = constitution;
        Luck = luck;
        Health = health;

        dungeonRuneBonuses ??= new DungeonEnemyRuneBonuses();
        ColdResistance = dungeonRuneBonuses.ColdResistance;
        FireResistance = dungeonRuneBonuses.FireResistance;
        LightningResistance = dungeonRuneBonuses.LightningResistance;
        HealthRune = 0;

        FirstWeapon = new RawWeapon
        {
            MinDmg = minWeaponDmg,
            MaxDmg = maxWeaponDmg,
            RuneType = dungeonRuneBonuses.DamageRuneType,
            RuneValue = dungeonRuneBonuses.DamageBonus
        };
        if (Class == ClassType.Assassin)
            SecondWeapon = new RawWeapon
            {
                MinDmg = minWeaponDmg,
                MaxDmg = maxWeaponDmg,
                RuneType = dungeonRuneBonuses.DamageRuneType,
                RuneValue = dungeonRuneBonuses.DamageBonus
            };

        armorMultiplier ??= 1;
        if (armor != null)
        {
            Armor = (int)(armor.Value * armorMultiplier.Value);
            return;
        }

        var classConfiguration = ClassConfigurationProvider.Get(@class);
        Armor = (int)(Level * classConfiguration.MaxArmorReduction * 100 / classConfiguration.ArmorMultiplier * armorMultiplier);
    }
}