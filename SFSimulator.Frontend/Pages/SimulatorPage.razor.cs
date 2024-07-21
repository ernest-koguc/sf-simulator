using AutoMapper;
using Microsoft.AspNetCore.Components;
using Radzen;
using SFSimulator.Core;
using SFSimulator.Frontend.Dialogs;

namespace SFSimulator.Frontend.Pages;

public partial class SimulatorPage
{
    [Parameter]
    public EventCallback<SimulationResult> OnSimulationFinished { get; set; }
    [Inject]
    private DialogService DialogService { get; set; } = default!;
    [Inject]
    private IMapper Mapper { get; set; } = default!;
    [Inject]
    private IGameSimulator GameSimulator { get; set; } = default!;
    private SimulationOptions SimulationOptions { get; set; } = new();
    private SimulationFinishCondition SimulationFinishCondition { get; set; } = new();
    private FormTab CurrentTab { get; set; }
    private SimulationResult? SimulationResult { get; set; }

    private void Simulate(SimulationOptions simulationOptions)
    {
        SimulationResult = GameSimulator.Run(SimulationOptions, SimulationFinishCondition);
    }
    private async Task OpenEndpoint()
    {
        var result = await DialogService.OpenAsync<EndpointDialog>(string.Empty, options: EndpointDialog.PreferredDialogOptions);
        if (result is Maria21DataDTO data)
        {
            SimulationOptions = Mapper.Map<SimulationOptions>(data);
        }
    }
}

internal enum FormTab
{
    Account,
    Bonuses,
    Playstyle,
    Dungeon,
}
