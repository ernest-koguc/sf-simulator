namespace SFSimulator.Core;

public class SimulationResult
{
    public required int Days { get; set; }
    public required ContextSnapshot BeforeSimulation { get; set; }
    public required ContextSnapshot AfterSimulation { get; set; }
    public required List<SimulatedGains> SimulatedDays { get; set; }
    public required List<SimulationAchievement> Achievements { get; set; }
}

public record ContextSnapshot(int BaseStat, int Level, long Experience);