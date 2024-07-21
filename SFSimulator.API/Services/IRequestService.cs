using SFSimulator.API.Requests;
using SFSimulator.Core;

namespace SFSimulator.API.Services;

public interface IRequestService
{
    SimulationResult RunSimulation(SimulateRequest request, SimulationFinishCondition simulationType);
}