using Microsoft.AspNetCore.Components;
using Radzen;
using SFSimulator.Core;

namespace SFSimulator.Frontend.Pages.SimulatorPage;

public partial class SimulationResultView
{
    [Parameter, EditorRequired]
    public required SimulationResult Result { get; set; }
    [Inject]
    public NotificationService NotificationService { get; set; } = default!;
    private int ChosenDay { get; set; }
    private List<ChartRecord> TotalExperienceGains { get; set; } = default!;
    private List<ChartRecord> AverageExperienceGains { get; set; } = default!;
    private List<ChartRecord> ChosenDayExperienceGains => Result
        .SimulatedDays[ChosenDay - 1]
        .ExperienceGain
        .Select((kvp) => new ChartRecord(kvp.Key.GetDisplayName(), kvp.Value))
        .ToList();
    private List<ChartRecord> TotalBaseStatGains { get; set; } = default!;
    private List<ChartRecord> AverageBaseStatGains { get; set; } = default!;
    private List<ChartRecord> ChosenDayBaseStatGains => Result
        .SimulatedDays[ChosenDay - 1]
        .BaseStatGain
        .Select((kvp) => new ChartRecord(kvp.Key.GetDisplayName(), kvp.Value))
        .ToList();

    private async Task SaveResult()
    {
        //var db = await DbFactory.GetDbManager(Constants.DatabaseName);
        //await db.Add(new SavedResultEntity
        //{
        //    Saved = DateTime.Now,
        //    Result = Result.Copy()
        //});
        NotificationService.Error("Saving result not implemented yet.");
    }

    protected override void OnParametersSet()
    {
        TotalExperienceGains = Result.SimulatedDays.Aggregate(new Dictionary<GainSource, long>(), (acc, day) =>
        {
            foreach (var key in day.ExperienceGain.Keys)
            {
                acc.TryAdd(key, 0);
                acc[key] += day.ExperienceGain[key];
            }
            return acc;
        }).Select((keyValuePair) => new ChartRecord(keyValuePair.Key.GetDisplayName(), keyValuePair.Value)).ToList();

        TotalBaseStatGains = Result.SimulatedDays.Aggregate(new Dictionary<GainSource, decimal>(), (acc, day) =>
        {
            foreach (var key in day.BaseStatGain.Keys)
            {
                acc.TryAdd(key, 0);
                acc[key] += day.BaseStatGain[key];
            }
            return acc;
        }).Select((keyValuePair) => new ChartRecord(keyValuePair.Key.GetDisplayName(), keyValuePair.Value)).ToList();

        AverageExperienceGains = TotalExperienceGains.Select(t => new ChartRecord(t.Source, t.Value / Result.Days)).ToList();

        AverageBaseStatGains = TotalBaseStatGains.Select(t => new ChartRecord(t.Source, t.Value / Result.Days)).ToList();

        base.OnParametersSet();
    }
}
