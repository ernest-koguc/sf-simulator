namespace SFSimulator.Core
{
    public class ThirstSimulationOptions
    {
        public bool HasGoldScroll { get; set; } = false;
        public bool DrinkBeerOneByOne { get; set; } = false;
        public decimal GoldRuneBonus { get; set; } = 0;
        public MountType Mount { get; set; }
    }
}