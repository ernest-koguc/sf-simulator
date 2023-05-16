using AutoMapper;
using SFSimulator.Core;

namespace SFSimulator.API.Services
{
    public class GameService : IGameService
    {
        private readonly IGameSimulator _gameSimulator;
        private readonly IMapper _mapper;

        public GameService(IGameSimulator gameSimulator, IMapper mapper)
        {
            _gameSimulator = gameSimulator;
            _mapper = mapper;
        }

        public SimulationResult RunSimulationUntilDays(SimulationOptionsDTO simulationOptions, int days)
        {
            _ = simulationOptions ?? throw new ArgumentNullException(nameof(simulationOptions));
            if (days == 0) throw new ArgumentOutOfRangeException(nameof(days));

            var character = _mapper.Map<Character>(simulationOptions);
            var preferences = _mapper.Map<SimulationOptions>(simulationOptions);

            var simulationResult = _gameSimulator.RunDays(days, character, preferences);

            return simulationResult;
        }
        public SimulationResult RunSimulationUntilLevel(SimulationOptionsDTO simulationOptions, int level)
        {
            _ = simulationOptions ?? throw new ArgumentNullException(nameof(simulationOptions));

            if (level == 0) throw new ArgumentOutOfRangeException(nameof(level));

            var character = _mapper.Map<Character>(simulationOptions);
            var preferences = _mapper.Map<SimulationOptions>(simulationOptions);

            var simulationResult = _gameSimulator.RunLevels(level, character, preferences);

            return simulationResult;
        }
    }
}
