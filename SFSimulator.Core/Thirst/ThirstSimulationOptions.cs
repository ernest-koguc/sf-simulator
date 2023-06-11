namespace SFSimulator.Core
{
    public class ThirstSimulationOptions
    {
        public bool HasGoldScroll { get; set; } = false;
        public bool DrinkBeerOneByOne { get; set; } = false;
        public float GoldRuneBonus { get; set; } = 0f;
        public MountType Mount { get; set; }
    }
}
