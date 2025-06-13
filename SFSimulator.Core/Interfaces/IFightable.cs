namespace SFSimulator.Core
{
    public interface IFightable<T> where T : IWeaponable
    {
        public ClassType Class { get; }
        public int Level { get; }
        public int Strength { get; }
        public int Dexterity { get; }
        public int Intelligence { get; }
        public int Constitution { get; }
        public int Luck { get; }
        public long Health { get; }
        public int Armor { get; }
        public T? FirstWeapon { get; }
        public T? SecondWeapon { get; }
        public int Reaction { get; }
        public double CritMultiplier { get; }
        public int LightningResistance { get; }
        public int FireResistance { get; }
        public int ColdResistance { get; }
        public int HealthRune { get; }
        public double SoloPortal { get; }
        public double GuildPortal { get; }
    }
}