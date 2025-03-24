using Microsoft.AspNetCore.Components;
using Radzen;
using Radzen.Blazor;
using System.Linq.Dynamic.Core;

namespace SFSimulator.Frontend.Pages.SavedResultsPage;

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
        var query = (await DatabaseService.GetSavedResults()).AsQueryable();
        if (!string.IsNullOrEmpty(args.Filter))
        {
            query = query.Where(args.Filter);
        }
        if (!string.IsNullOrEmpty(args.OrderBy))
        {
            query = query.OrderBy(args.OrderBy);
        }

        SavedResults = query.ToList();
        Count = SavedResults.Count;

        IsLoading = false;
    }
}
