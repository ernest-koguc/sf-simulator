using SFSimulator.API.Requests;
using SFSimulator.Core;

namespace SFSimulator.API.Services
{
    public interface IRequestService
    {
        Task<SimulationResult> RunSimulation(SimulateRequest request, SimulationType simulationType);
        List<DungeonDTO> GetDungeons();
        DungeonSimulationResult SimulateDungeon(SimulateDungeonRequest request);
    }
}
