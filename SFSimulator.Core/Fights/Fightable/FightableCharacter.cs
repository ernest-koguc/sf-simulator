namespace SFSimulator.Core
{
    public class FightableCharacter : IFightable
    {
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
        public int Reaction { get; set; }
        public double CritMultiplier { get; set; }
        public ResistanceRuneBonuses RuneResistance { get; set; }
        public double SoloPortal { get; set; }
        public double GuildPortal { get; set; }

        public FightableCharacter(Character character)
        {
            Class = character.Class;
            Level = character.Level;
            Strength = character.Strength;
            Dexterity = character.Dexterity;
            Intelligence = character.Intelligence;
            Constitution = character.Constitution;
            Luck = character.Luck;
            Armor = character.Armor;
            Reaction = character.HasGlovesScroll ? 1 : 0;
            FirstWeapon = character.FirstWeapon;
            SecondWeapon = character.SecondWeapon;

            CritMultiplier = 2D + 0.11D * character.GladiatorLevel;

            if (character.HasWeaponScroll)
                CritMultiplier += 0.05D;

            RuneResistance = character.RuneBonuses;
            SoloPortal = character.SoloPortal / 100D;
            GuildPortal = character.GuildPortal / 100D;
            InitHealth(character.HasEternityPotion);
        }

        private void InitHealth(bool hasEternityPotion)
        {
            var healthMultiplier = ClassConfigurationProvider.GetClassConfiguration(Class).HealthMultiplier;

            var health = Constitution * (Level + 1L);
            health *= healthMultiplier;
            var portalBonus = 1 + SoloPortal;
            var runeBonus = 1 + RuneResistance.HealthRune / 100D;
            health = (long)(health * portalBonus * runeBonus);

            if (hasEternityPotion)
                health = (long)(health * 1.25D);

            Health = health;
        }
    }
}