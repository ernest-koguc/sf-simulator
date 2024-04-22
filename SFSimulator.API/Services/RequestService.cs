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

    public SimulationResult RunSimulation(SimulateRequest request, SimulationType simulationType)
    {
        var character = _mapper.Map<Character>(request);
        var simulationOptions = _mapper.Map<SimulationOptions>(request);

        var simulationResult = _gameSimulator.Run(request.SimulateUntil, character, simulationOptions, simulationType);

        return simulationResult;
    }

    public List<DungeonDTO> GetDungeons()
    {
        var dungeons = _dungeonProvider.GetAllDungeons();
        var dto = _mapper.Map<List<DungeonDTO>>(dungeons);
        return dto;
    }

    public DungeonSimulationResult SimulateDungeon(SimulateDungeonRequest request)
    {
        var character = _mapper.Map<RawFightable>(request);
        foreach (var companion in character.Companions)
        {
            companion.Character = character;
        }

        var enemy = _dungeonProvider.GetDungeonEnemy(request.DungeonPosition, request.DungeonEnemyPosition);

        var result = _dungeonSimulator.SimulateDungeon(enemy, character, request.Iterations, request.WinTreshold);
        return result;
    }
}
