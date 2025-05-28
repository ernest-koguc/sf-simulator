using Microsoft.AspNetCore.Components;
using Radzen;
using Radzen.Blazor;
using System.Linq.Dynamic.Core;

namespace SFSimulator.Frontend.Pages.SavedResults;

public partial class SavedResultsPage
{
    [Inject]
    private DatabaseService DatabaseService { get; set; } = default!;
    private bool IsLoading { get; set; } = false;
    private int Count { get; set; } = 0;
    RadzenDataGrid<SavedResultEntity> Grid { get; set; } = default!;
    private List<SavedResultEntity> SavedResults { get; set; } = default!;

    private async Task LoadData(LoadDataArgs args)
    {
        IsLoading = true;
        var query = (await DatabaseService.SavedResults.Get()).AsQueryable();
        if (!string.IsNullOrEmpty(args.Filter))
        {
            query = query.Where(args.Filter);
        }
        if (!string.IsNullOrEmpty(args.OrderBy))
        {
            query = query.OrderBy(args.OrderBy);
        }

        Count = query.Count();
        SavedResults = query.ToList();

        IsLoading = false;
    }

    private async Task ConfirmEdit(SavedResultEntity entity)
    {
        await Grid.UpdateRow(entity);
    }

    private async Task Save(SavedResultEntity entity)
    {
        await DatabaseService.SavedResults.Put(entity);
    }

    private async Task Edit(SavedResultEntity entity)
    {
        await Grid.EditRow(entity);
    }

    private async Task CancelEdit(SavedResultEntity entity)
    {
        var originalEntity = await DatabaseService.SavedResults.Get(entity.Id);
        entity.Name = originalEntity.Name;
        Grid.CancelEditRow(entity);
    }


    private async Task Remove(SavedResultEntity entity)
    {
        await DatabaseService.SavedResults.Remove(entity.Id);
        await Grid.Reload();
    }
}
