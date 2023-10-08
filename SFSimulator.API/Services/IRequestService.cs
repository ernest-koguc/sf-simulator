using SFSimulator.API.Requests;
using SFSimulator.Core;

namespace SFSimulator.API.Services
{
    public interface IRequestService
    {
        Task<SimulationResult> RunSimulationUntilDays(SimulateDaysRequest request);
        Task<SimulationResult> RunSimulationUntilLevel(SimulateUntilLevelRequest request);
        List<DungeonDTO> GetDungeons();
        DungeonSimulationResult SimulateDungeon(SimulateDungeonRequest request);
    }
}