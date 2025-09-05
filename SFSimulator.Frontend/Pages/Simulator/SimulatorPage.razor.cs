using Microsoft.AspNetCore.Components;
using Radzen;
using SFSimulator.Core;
using SFSimulator.Frontend.Extensions;
using SpawnDev.BlazorJS.WebWorkers;
using System.Diagnostics;

namespace SFSimulator.Frontend.Pages.Simulator;

public partial class SimulatorPage
{
    [Inject]
    private WebWorkerService WebWorkerService { get; set; } = default!;
    [Inject]
    private NotificationService NotificationService { get; set; } = default!;
    [Inject]
    private DialogService DialogService { get; set; } = default!;
    [Inject]
    private Stopwatch Stopwatch { get; set; } = default!;
    private SimulationResult? SimulationResult { get; set; }
    private bool IsSimulating { get; set; } = false;
    private int ProgressValue { get; set; }
    private string ProgressText { get; set; } = string.Empty;

    private async Task Simulate(SimulationContext context)
    {
        if (IsSimulating)
        {
            return;
        }

        SimulationResult = null;

        using var webWorker = await WebWorkerService.GetWebWorker();
        if (webWorker is null)
        {
            NotificationService.Error("ServiceWorker is not available in your browser, please try enabling it and try again.");
            return;
        }

        IsSimulating = true;
        ProgressText = "Current day: 1";
        StateHasChanged();

        try
        {
            var service = webWorker.GetService<IGameLoopService>();
            Stopwatch.Restart();
            string? error = null;
            SimulationResult = await service.Run(context, simulationProgress =>
            {
                ProgressValue = simulationProgress.Progress;
                ProgressText = $"Current day: {simulationProgress.CurrentDay}";
                StateHasChanged();
            }, ex =>
            {
                error = ex;
            });

            if (error is not null)
                throw new Exception(error);

            Stopwatch.Stop();
            NotificationService.Info($"Simulation finished, took {Stopwatch.ElapsedMilliseconds}ms");
        }
        catch (Exception ex)
        {
            Stopwatch.Stop();
            SentrySdk.CaptureException(ex);
            await DialogService.OpenCrashReport("An error occured during simulation.", ex.Message + "\r\n" + (ex.StackTrace ?? ""));
        }

        IsSimulating = false;
        ProgressValue = 0;
        ProgressText = string.Empty;
        StateHasChanged();
    }
}
