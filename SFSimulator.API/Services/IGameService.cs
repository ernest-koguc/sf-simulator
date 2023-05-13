using QuestSimulator.DTOs;
using QuestSimulator.Simulation;

namespace SFSimulatorAPI.Services
{
    public interface IGameService
    {
        SimulationResult RunSimulationUntilDays(SimulationOptionsDTO simulationOptions, int days);
        SimulationResult RunSimulationUntilLevel(SimulationOptionsDTO simulationOptions, int levels);
    }
}