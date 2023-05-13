using Microsoft.AspNetCore.Mvc;
using QuestSimulator.DTOs;
using QuestSimulator.Simulation;
using SFSimulatorAPI.Services;

namespace SFSimulatorAPI.Controllers
{

    [Route("api/[controller]")]
    [ApiController]
    public class GameController : ControllerBase
    {
        private readonly IGameService _gameService;
        public GameController(IGameService gameService)
        {
            _gameService = gameService;
        }
        [HttpPost("simulateUntilDays")]
        public ActionResult<SimulationResult> RunSimulationUntilDay([FromQuery] int? days, [FromBody] SimulationOptionsDTO simulationOptions)
        {
            if (days > 3000)
                days = 3000;

            var simulationResult = _gameService.RunSimulationUntilDays(simulationOptions, days ?? 1);

            return Ok(simulationResult);
        }
        [HttpPost("simulateUntilLevel")]
        public ActionResult<SimulationResult> RunSimulationUntilLevel([FromQuery] int? level, [FromBody] SimulationOptionsDTO simulationOptions)
        {
            if (level > 800)
                level = 800;

            var simulationResult = _gameService.RunSimulationUntilLevel(simulationOptions, level ?? 1);

            return Ok(simulationResult);
        }

    }
}
