namespace SFSimulator.Core
{
    public interface IGameSimulator
    {
        Task<SimulationResult> RunDays(int days, Character character, SimulationOptions simulationOptions);
        Task<SimulationResult> RunLevels(int level, Character character, SimulationOptions simulationOptions);
    }
}