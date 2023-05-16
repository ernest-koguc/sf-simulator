namespace SFSimulator.Core
{
    public class Character
    {
        public int Level { get; set; } = 400;
        public long Experience { get; set; } = 0;
        public int BaseStat { get; set; } = 0;
        public float Gold { get; set; } = 0;
        public int GoldPitLevel { get; set; } = 15;
        public int AcademyLevel { get; set; } = 20;
        public int HydraHeads { get; set; } = 0;
        public int GemMineLevel { get; set; } = 0;
        public int TreasuryLevel { get; set; } = 0;
        public MountType Mount { get; set; } = MountType.Griffin;
    }
}
