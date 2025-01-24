using AutoMapper;
using FluentValidation;
using Microsoft.AspNetCore.Components;
using Radzen;
using SFSimulator.Core;
using SFSimulator.Frontend.Dialogs;
using SFSimulator.Frontend.Validation;
using System.Linq.Expressions;

namespace SFSimulator.Frontend.Pages.SimulatorPage;

public partial class SimulationOptions
{
    [Parameter]
    public SimulationContext Options { get; set; } = default!;
    [Parameter]
    public EventCallback<SimulationContext> OptionsChanged { get; set; }
    [Parameter(CaptureUnmatchedValues = true)]
    public Dictionary<string, object>? CapturedAttributes { get; set; }
    [Inject]
    private DialogService DialogService { get; set; } = default!;
    [Inject]
    private IMapper Mapper { get; set; } = default!;
    private SimulationContextValidator Validator { get; set; } = new();

    private Task Submit()
    {
        return OptionsChanged.InvokeAsync(Options);
    }

    private async Task OpenEndpoint()
    {
        var result = await DialogService.OpenAsync<EndpointDialog>(string.Empty, options: EndpointDialog.PreferredDialogOptions);
        if (result is Maria21DataDTO data)
            Options = Mapper.Map<SimulationContext>(data);
    }

    private ValidationResult ValidateField(Expression<Func<SimulationContext, object>> fieldToValidate)
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

