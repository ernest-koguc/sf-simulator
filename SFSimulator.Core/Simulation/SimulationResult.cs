namespace SFSimulator.Core
{
    public class SimulationResult
    {
        public required int Days { get; set; }
        public required int Level { get; set; }
        public required long Experience { get; set; }
        public required int BaseStat { get; set; }
        public required List<SimulatedGains> SimulatedDays { get; set; }
        public required SimulatedGains TotalGains { get; set; } = null!;
        public required SimulatedGains AverageGains { get; set; } = null!;
    }
}