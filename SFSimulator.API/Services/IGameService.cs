using SFSimulator.Core;

namespace SFSimulator.API.Services
{
    public interface IGameService
    {
        SimulationResult RunSimulationUntilDays(SimulationOptionsDTO simulationOptions, int days);
        SimulationResult RunSimulationUntilLevel(SimulationOptionsDTO simulationOptions, int levels);
    }
}