namespace SFSimulator.Core
{
    public class Weapon
    {
        public int MinDmg { get; set; } = 0;
        public int MaxDmg { get; set; } = 0;
        public DamageRuneType DamageRuneType { get; set; } = DamageRuneType.None;
        public int RuneBonus { get; set; } = 0;
    }
}
