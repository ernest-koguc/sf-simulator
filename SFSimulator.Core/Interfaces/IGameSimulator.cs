namespace SFSimulator.Core
{
    public interface IGameSimulator
    {
        SimulationResult Run(int until, Character character, SimulationOptions simulationOptions, SimulationType simulationType);
    }
}
