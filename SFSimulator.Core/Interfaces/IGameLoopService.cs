namespace SFSimulator.Core;

public interface IGameLoopService
{
    Task<SimulationResult?> Run(SimulationContext simulationContext, Action<SimulationProgress> progressCallback);
}