namespace SFSimulator.Core;

public interface IGameSimulator
{
    Task<SimulationResult?> Run(SimulationContext simulationContext, Action<SimulationProgress> progressCallback);
}