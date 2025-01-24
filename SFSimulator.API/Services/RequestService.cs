using AutoMapper;
using SFSimulator.API.Requests;
using SFSimulator.Core;

namespace SFSimulator.API.Services;

public class RequestService : IRequestService
{
    private readonly IGameSimulator _gameSimulator;
    private readonly IDungeonSimulator _dungeonSimulator;
    private readonly IDungeonProvider _dungeonProvider;
    private readonly IMapper _mapper;

    public RequestService(IGameSimulator gameSimulator, IDungeonSimulator dungeonSimulator, IDungeonProvider dungeonProvider, IMapper mapper)
    {
        _gameSimulator = gameSimulator;
        _dungeonSimulator = dungeonSimulator;
        _dungeonProvider = dungeonProvider;
        _mapper = mapper;
    }

    public SimulationResult RunSimulation(SimulateRequest request, SimulationFinishCondition simulationType)
    {
        var simulationOptions = _mapper.Map<SimulationContext>(request);

        foreach (var companion in simulationOptions.Companions)
        {
            companion.Character = simulationOptions;
            companion.Class = companion.Class == ClassType.Warrior ? ClassType.Bert : companion.Class;
        }

        var simulationResult = _gameSimulator.Run(simulationOptions, simulationType);

        return simulationResult;
    }
}