using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Primitives;
using Newtonsoft.Json;
using SFSimulator.API.Requests;
using SFSimulator.API.Services;
using SFSimulator.Core;
using JsonSerializer = System.Text.Json.JsonSerializer;

namespace SFSimulator.API.Controllers
{

    [Route("api")]
    [ApiController]
    public class SFSimulatorController : ControllerBase
    {
        private readonly IRequestService _requestService;
        private readonly IConfiguration _configuration;
        private readonly IMapper _mapper;

        public SFSimulatorController(IRequestService gameService, IConfiguration configuration, IMapper mapper)
        {
            _requestService = gameService;
            _configuration = configuration;
            _mapper = mapper;
        }

        [HttpPost("simulateUntilDays")]
        public async Task<ActionResult<SimulationResult>> RunSimulationUntilDay([FromBody] SimulateRequest request)
        {
            var simulationResult = await _requestService.RunSimulation(request, SimulationType.UntilDays);
            return Ok(simulationResult);
        }

        [HttpPost("simulateUntilLevel")]
        public async Task<ActionResult<SimulationResult>> RunSimulationUntilLevel([FromBody] SimulateRequest request)
        {
            var simulationResult = await _requestService.RunSimulation(request, SimulationType.UntilLevel);

            return Ok(simulationResult);
        }

        [HttpPost("simulateUntilBaseStats")]
        public async Task<ActionResult<SimulationResult>> RunSimulationUntilBaseStats([FromBody] SimulateRequest request)
        {
            var simulationResult = await _requestService.RunSimulation(request, SimulationType.UntilBaseStats);

            return Ok(simulationResult);
        }

        [HttpPost("loadFromEndpoint")]
        public async Task<ActionResult> LoadFromEndpoint()
        {
            try
            {
                Request.Form.TryGetValue("data", out StringValues values);
                var json = values.ToString();
                var stream = new MemoryStream();
                using var writer = new StreamWriter(stream);
                await writer.WriteAsync(json);
                await writer.FlushAsync();
                stream.Position = 0;
                var data = await JsonSerializer.DeserializeAsync<Maria21DataDTO>(stream);
                var simulationOptions = _mapper.Map<EndpointDataDTO>(data);
                var jsonData = JsonConvert.SerializeObject(simulationOptions);
                return PostMessageResult(jsonData);
            }
            catch
            {
                return PostMessageResult("{error:\"Could not fetch data\"}");
            }
        }

        [HttpPost("loadDungeonFromEndpoint")]
        public async Task<ActionResult> LoadDungeonFromEndpoint()
        {
            try
            {
                Request.Form.TryGetValue("data", out StringValues values);
                var json = values.ToString();
                var stream = new MemoryStream();
                using var writer = new StreamWriter(stream);
                await writer.WriteAsync(json);
                await writer.FlushAsync();
                stream.Position = 0;
                var data = await JsonSerializer.DeserializeAsync<Maria21DataDTO>(stream);
                var simulationOptions = _mapper.Map<RawFightable>(data);
                var jsonData = JsonConvert.SerializeObject(simulationOptions);
                return PostMessageResult(jsonData);
            }
            catch
            {
                return PostMessageResult("{error:\"Could not fetch data\"}");
            }
        }

        [HttpGet("dungeons")]
        public ActionResult<List<DungeonDTO>> GetDungeons()
        {
            var dungeons = _requestService.GetDungeons();
            return dungeons;
        }

        [HttpPost("simulateDungeon")]
        public ActionResult<DungeonSimulationResult> SimulateDungeon([FromBody] SimulateDungeonRequest request)
        {
            var result = _requestService.SimulateDungeon(request);
            return result;
        }

        private ActionResult PostMessageResult(string data)
        {
            var url = _configuration.GetValue<string>("Target");
            var script = $"<script>window.top.postMessage({data}, '{url}');</script>";
            return Content(script, "text/html");
        }
    }
}
