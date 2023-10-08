namespace SFSimulator.Core
{
    public interface IFightable
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
    }
}