using Microsoft.AspNetCore.Components;
using Radzen;
using SFSimulator.Core;
using SpawnDev.BlazorJS.WebWorkers;

namespace SFSimulator.Frontend.Pages.SimulatorPage;

public partial class SimulatorPage
{
    [Inject]
    private WebWorkerService WebWorkerService { get; set; } = default!;
    [Inject]
    private DialogService DialogService { get; set; } = default!;
    private SimulationResult? SimulationResult { get; set; }
    private SimulationContext Options { get; set; } = new();
    private SimulationContext? ContextAfterSimulation { get; set; }
    private bool IsSimulating { get; set; } = false;
    private int ProgressValue { get; set; }
    private string ProgressText { get; set; } = string.Empty;

    private async Task Simulate(SimulationContext context)
    {
        if (IsSimulating)
        {
            return;
        }

        Options = context;
        var copy = Options.Copy();
        ContextAfterSimulation = copy;
        var webWorker = await WebWorkerService.GetWebWorker();
        if (webWorker is not null)
        {
            IsSimulating = true;
            ProgressText = "Current day: 1";

            StateHasChanged();
            var service = webWorker.GetService<IGameSimulator>();
            SimulationResult = await service.Run(copy, simulationProgress =>
            {
                ProgressValue = simulationProgress.Progress;
                ProgressText = $"Current day: {simulationProgress.CurrentDay}";
                StateHasChanged();
            });
            IsSimulating = false;
            ProgressValue = 0;
            ProgressText = string.Empty;
        }
    }
}
