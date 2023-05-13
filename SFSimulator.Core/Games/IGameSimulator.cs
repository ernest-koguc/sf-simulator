using QuestSimulator.Characters;
using QuestSimulator.Simulation;

namespace QuestSimulator.Games
{
    public interface IGameSimulator
    {
        SimulationResult RunDays(int days, Character character, SimulationOptions characterPlaystylePreferences);
        SimulationResult RunLevels(int level, Character character, SimulationOptions characterPlaystylePreferences);
    }
}