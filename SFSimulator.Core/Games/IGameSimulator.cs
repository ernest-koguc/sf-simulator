namespace SFSimulator.Core
{
    public interface IGameSimulator
    {
        SimulationResult RunDays(int days, Character character, SimulationOptions characterPlaystylePreferences);
        SimulationResult RunLevels(int level, Character character, SimulationOptions characterPlaystylePreferences);
    }
}