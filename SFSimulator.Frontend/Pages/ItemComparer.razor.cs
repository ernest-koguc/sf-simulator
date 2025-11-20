using Microsoft.AspNetCore.Components;
using Radzen;
using SFSimulator.Core;
using SFSimulator.Frontend.Dialogs;
using SFSimulator.Frontend.Extensions;
using SpawnDev.BlazorJS.WebWorkers;
using static SFSimulator.Frontend.MappingUtils;

namespace SFSimulator.Frontend.Pages;

public partial class ItemComparer
{
    [Inject]
    private DialogService DialogService { get; set; } = default!;
    [Inject]
    private IDungeonProvider DungeonProvider { get; set; } = default!;
    [Inject]
    private WebWorkerService WebWorkerService { get; set; } = default!;
    [Inject]
    private NotificationService NotificationService { get; set; } = default!;
    private SimulationContext? Options { get; set; }
    private List<EquipmentItem> ChoosableItems { get; set; } = [];
    private static int Iterations { get; set; } = 10000;
    private bool IsSimulating { get; set; }

    private EquipmentItem? CompareTo { get; set; }
    private List<List<DungeonSimulationResult>>? ComparisonResults { get; set; }

    private async Task OpenEndpoint()
    {
        var result = await DialogService.OpenAsync<EndpointDialog>(string.Empty, options: EndpointDialog.PreferredDialogOptions);
        if (result is Maria21DataDTO data)
        {
            Options = MappingUtils.MapToSimulationContext(new(), data);
            ChoosableItems = MapItems(data.Class, [.. data.Inventory.Backpack, .. data.Inventory.Chest]);
        }
    }

    private async Task CompareItem()
    {
        if (CompareTo is null) return;

        DungeonProvider.InitDungeons();
        DungeonProvider.InitCharacterDungeonState(Options!);

        using var webWorker = await WebWorkerService.GetWebWorker();
        if (webWorker is null)
        {
            NotificationService.Error("ServiceWorker is not available in your browser, please try enabling it and try again.");
            return;
        }

        IsSimulating = true;
        ComparisonResults = null;
        StateHasChanged();
        var dungeonSimulator = webWorker.GetService<IDungeonSimulator>();

        try
        {

            List<List<DungeonSimulationResult>> dungeonWinRatios = [await SimulateOpenDungeons(Options!, dungeonSimulator)];

            var newContext = Options!.Copy();
            var itemsToSwap = Options!.Items.Where(i => i.ItemType == CompareTo.ItemType).ToList();
            foreach (var itemSwap in itemsToSwap)
            {
                var index = newContext.Items.IndexOf(itemSwap);
                if (index < 0)
                {
                    newContext.Items.Add(CompareTo);
                }
                else
                {
                    var prevItem = newContext.Items[index];
                    var newItem = CompareTo.Copy();
                    newItem.GemType = prevItem.GemType;
                    newItem.GemValue = prevItem.GemValue;
                    newItem.RuneType = prevItem.RuneType;
                    newItem.RuneValue = prevItem.RuneValue;
                    newItem.ScrollType = prevItem.ScrollType;
                    newItem.UpgradeLevel = prevItem.UpgradeLevel;
                    newContext.Items[index] = newItem;
                }

                DungeonProvider.InitDungeons();
                DungeonProvider.InitCharacterDungeonState(newContext);
                dungeonWinRatios.Add(await SimulateOpenDungeons(newContext, dungeonSimulator));
            }

            ComparisonResults = dungeonWinRatios;
        }
        catch (Exception ex)
        {
            SentrySdk.CaptureException(ex);
            await DialogService.OpenCrashReport("An error occured during simulation.", ex.Message + "\r\n" + (ex.StackTrace ?? ""));
        }

        IsSimulating = false;
    }

    private async Task<List<DungeonSimulationResult>> SimulateOpenDungeons(SimulationContext context, IDungeonSimulator dungeonSimulator)
    {
        var results = new List<DungeonSimulationResult>();
        var enemies = DungeonProvider.GetFightablesDungeonEnemies(context);
        foreach (var enemy in enemies)
        {
            var result = await dungeonSimulator.SimulateDungeonAsync(enemy, context, context.Companions, new DungeonSimulationOptions(Iterations, 0, false));
            results.Add(result);
        }

        return results;
    }
}