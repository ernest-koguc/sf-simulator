using Magic.IndexedDb;
using Microsoft.AspNetCore.Components;

namespace SFSimulator.Frontend.Pages.SavedResultsPage;

public partial class SavedResultsPage
{
    [Inject]
    private IMagicDbFactory DbFactory { get; set; } = default!;

    protected override async Task OnParametersSetAsync()
    {
        await base.OnParametersSetAsync();

        //var db = await DbFactory.GetDbManagerAsync(Constants.DatabaseName);
        //var records = await db.GetAll<SavedResultEntity>();
    }
}
