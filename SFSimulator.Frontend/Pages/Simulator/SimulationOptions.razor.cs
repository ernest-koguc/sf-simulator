using FluentValidation;
using Microsoft.AspNetCore.Components;
using Radzen;
using SFSimulator.Core;
using SFSimulator.Frontend.Dialogs;
using SFSimulator.Frontend.Validation;
using System.Linq.Expressions;

namespace SFSimulator.Frontend.Pages.Simulator;

public partial class SimulationOptions
{
    [Parameter, EditorRequired]
    public EventCallback<SimulationContext> Simulate { get; set; }
    [Parameter(CaptureUnmatchedValues = true)]
    public Dictionary<string, object>? CapturedAttributes { get; set; }
    [Inject]
    private DialogService DialogService { get; set; } = default!;
    private SimulationContextValidator Validator { get; set; } = new();
    private SimulationContext Options { get; set; } = new();

    private async Task Submit()
    {
        await Simulate.InvokeAsync(Options);
    }

    private async Task OpenEndpoint()
    {
        var result = await DialogService.OpenAsync<EndpointDialog>(string.Empty, options: EndpointDialog.PreferredDialogOptions);
        if (result is Maria21DataDTO data)
        {
            Options = MappingUtils.MapToSimulationContext(Options, data);
        }
    }

    private async Task OpenMoreOptions()
    {
        var result = await DialogService.OpenAsync<SavedSimulationOptionsDialog>(SavedSimulationOptionsDialog.Title,
        parameters: new Dictionary<string, object>
        {
            [nameof(SavedSimulationOptionsDialog.Options)] = Options
        }, SavedSimulationOptionsDialog.DialogOptions);
        if (result is SimulationContext data)
        {
            Options = data;
            foreach (var companion in Options.Companions)
            {
                companion.Character = Options;
            }
        }
    }

    private ValidationResult ValidateField(Expression<Func<SimulationContext, object?>> fieldToValidate)
    {
        var result = Validator.Validate(Options, options =>
        {
            options.IncludeProperties(fieldToValidate);
        });
        var error = result.Errors.FirstOrDefault();
        if (error is not null)
        {
            return error.ErrorMessage;
        }

        return ValidationResult.Success;
    }
}
