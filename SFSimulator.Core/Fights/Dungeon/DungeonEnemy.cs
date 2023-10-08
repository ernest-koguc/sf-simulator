namespace SFSimulator.Core
{
    public class DungeonEnemy : IFightable
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
        public Weapon? FirstWeapon { get; set; }
        public Weapon? SecondWeapon { get; set; }
        public int Reaction { get; set; } = 0;
        public double CritMultiplier { get; set; } = 2;
        public ResistanceRuneBonuses RuneResistance { get; set; }
        public double SoloPortal { get; set; } = 0;
        public double GuildPortal { get; set; } = 0;
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
            RuneResistance = new ResistanceRuneBonuses
            {
                ColdResistance = dungeonRuneBonuses.ColdResistance,
                FireResistance = dungeonRuneBonuses.FireResistance,
                LightningResistance = dungeonRuneBonuses.ColdResistance,
                HealthRune = 0
            };

            FirstWeapon = new Weapon { MinDmg = minWeaponDmg, MaxDmg = maxWeaponDmg, DamageRuneType = dungeonRuneBonuses.DamageRuneType, RuneBonus = dungeonRuneBonuses.DamageBonus };
            if (Class == ClassType.Assassin)
                SecondWeapon = new Weapon { MinDmg = minWeaponDmg, MaxDmg = maxWeaponDmg, DamageRuneType = dungeonRuneBonuses.DamageRuneType, RuneBonus = dungeonRuneBonuses.DamageBonus };

            armorMultiplier ??= 1;
            if (armor != null)
            {
                Armor = (int)(armor.Value * armorMultiplier.Value);
                return;
            }

            var maxReduction = Class switch
            {
                ClassType.Mage => 10,
                ClassType.Warrior or ClassType.Bard or ClassType.DemonHunter or ClassType.ShieldlessWarrior or ClassType.BattleMage => 50,
                ClassType.Scout or ClassType.Assassin or ClassType.Berserker => 25,
                ClassType.Druid => 40,
                _ => throw new ArgumentOutOfRangeException(nameof(Class)),
            };
            Armor = (int)(Level * maxReduction * armorMultiplier);
        }
    }
}
