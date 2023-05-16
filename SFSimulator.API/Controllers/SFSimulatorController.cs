using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Primitives;
using Newtonsoft.Json;
using SFSimulator.API.Services;
using SFSimulator.Core;
using JsonSerializer = System.Text.Json.JsonSerializer;

namespace SFSimulator.API.Controllers
{

    [Route("api")]
    [ApiController]
    public class SFSimulatorController : ControllerBase
    {
        private readonly IGameService _gameService;
        private readonly IConfiguration _configuration;
        private readonly IMapper _mapper;

        public SFSimulatorController(IGameService gameService, IConfiguration configuration, IMapper mapper)
        {
            _gameService = gameService;
            _configuration = configuration;
            _mapper = mapper;
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
                var simulationOptions = _mapper.Map<SimulationOptionsDTO>(data);
                var jsonData = JsonConvert.SerializeObject(data);
                
                return PostMessageResult(jsonData);
            }
            catch
            {
                return PostMessageResult("{error:\"Could not fetch data\"}");
            }
        }

        private ActionResult PostMessageResult(string data)
        {
            var url = _configuration.GetValue<string>("Target");
            var script = $"<script>\r\nwindow.top.postMessage('{data}', '{url}');\r\n</script>\r\n";
            return Content(script, "text/html");
        }

        // Webhook link
        //https://beta.sftools.mar21.eu/request?scope=default+pets+items&origin=#{appname}&redirect=#{postUrl}
    }
}
