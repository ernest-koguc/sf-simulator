namespace SFSimulator.Core;

public interface IGameSimulator
{
    SimulationResult Run(SimulationOptions simulationOptions, SimulationFinishCondition simulationFinishCondition);
}