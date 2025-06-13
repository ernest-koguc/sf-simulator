using Microsoft.AspNetCore.Components;
using Radzen;
using Radzen.Blazor;
using SFSimulator.Core;

namespace SFSimulator.Frontend.Pages.Simulator;

public partial class SavedSimulationOptionsDialog
{
    [Parameter]
    public SimulationContext Options { get; set; } = default!;
    public static DialogOptions DialogOptions { get; } = new()
    {
        Width = "780px",
        Height = "600px",
        CloseDialogOnEsc = true,
        CloseDialogOnOverlayClick = true,
        AutoFocusFirstElement = false,
        ContentCssClass = "h-full",
    };
    public static string Title { get; } = "More Options";
    [Inject]
    private DatabaseService DatabaseService { get; set; } = default!;
    [Inject]
    private DialogService DialogService { get; set; } = default!;
    private IEnumerable<SavedOptionsEntity> SavedOptions { get; set; } = default!;
    private FormModel Model { get; set; } = new();
    private bool IsSaved { get; set; }
    private bool IsLoading { get; set; }
    private int Count { get; set; }
    private RadzenDataList<SavedOptionsEntity> DataList { get; set; } = default!;

    private async Task SaveOptions(FormModel form)
    {
        var savedOptionsEntity = new SavedOptionsEntity
        {
            Name = form.Name,
            Options = Options,
        };
        await DatabaseService.SavedOptions.Put(savedOptionsEntity);
        IsSaved = true;
        await DataList.Reload();
    }

    private void LoadOptions(SavedOptionsEntity entity)
    {
        DialogService.Close(entity.Options);
    }

    private async Task OverwriteOptions(SavedOptionsEntity entity)
    {
        entity.Options = Options;
        await DatabaseService.SavedOptions.Put(entity);
    }

    private async Task RemoveOptions(SavedOptionsEntity entity)
    {
        await DatabaseService.SavedOptions.Remove(entity.Id);
        SavedOptions = SavedOptions.Where(e => e.Id != entity.Id).ToList();
    }

    private async Task LoadData(LoadDataArgs args)
    {
        IsLoading = true;
        var query = (await DatabaseService.SavedOptions.Get()).AsQueryable();

        query = query.OrderByDescending(e => e.LastModification);

        SavedOptions = query.ToList();
        Count = query.Count();

        IsLoading = false;
    }

    private class FormModel
    {
        public string Name { get; set; } = string.Empty;
    }
}
