using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using SFSimulator.API.Requests;
using SFSimulator.API.Services;
using SFSimulator.API.Validation.Validators;
using SFSimulator.Core;
using JsonSerializer = System.Text.Json.JsonSerializer;

namespace SFSimulator.API.Controllers;

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
    public ActionResult<SimulationResult> RunSimulationUntilDay([FromBody] SimulateRequest request)
    {
        var simulationResult = _requestService.RunSimulation(request, SimulationFinishCondition.UntilDays);
        return Ok(simulationResult);
    }

    [HttpPost("simulateUntilLevel")]
    public ActionResult<SimulationResult> RunSimulationUntilLevel([FromBody] SimulateRequest request)
    {
        var validation = new SimulateRequestValidator(null);
        validation.
        var simulationResult = _requestService.RunSimulation(request, SimulationFinishCondition.UntilLevel);

        return Ok(simulationResult);
    }

    [HttpPost("simulateUntilBaseStats")]
    public ActionResult<SimulationResult> RunSimulationUntilBaseStats([FromBody] SimulateRequest request)
    {
        var simulationResult = _requestService.RunSimulation(request, SimulationFinishCondition.UntilBaseStats);

        return Ok(simulationResult);
    }

    [HttpPost("loadFromEndpoint")]
    public async Task<ActionResult> LoadFromEndpoint()
    {
        try
        {
            var values = Request.Form["data"];
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
            return PostMessageResult(/*lang=json*/ "{error:\"Could not fetch data\"}");
        }
    }

    private ContentResult PostMessageResult(string data)
    {
        data = data.Remove(data.Length - 1) + ",\"id\":\"sfsim\"}";
        var url = _configuration.GetValue<string>("Target");
        var script = $"<script>window.top.postMessage({data}, '{url}');</script>";
        return Content(script, "text/html");
    }
}