namespace SFSimulator.Core
{
    public class SimulationResult
    {
        public string CharacterName { get; set; } = null!;
        public int Days { get; set; }
        public CharacterDTO CharacterPreviously { get; set; } = null!;
        public CharacterDTO CharacterAfter { get; set; } = null!;
        public List<SimulatedGains> SimulatedDays { get; set; } = null!;
        public SimulatedGains TotalGains { get; set; } = null!;
        public SimulatedGains AverageGains { get; set; } = null!;
    }
}
