namespace SFSimulator.Core
{
    public interface IGameSimulator
    {
        Task<SimulationResult> Run(int until, Character character, SimulationOptions simulationOptions, SimulationType simulationType);
    }
}
