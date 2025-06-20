using Microsoft.AspNetCore.Components;
using SFSimulator.Core;

namespace SFSimulator.Frontend.Pages.Simulator;

public partial class SimulationResultView
{
    [Parameter, EditorRequired]
    public required SimulationResult Result { get; set; }
    private List<ChartRecord> TotalExperienceGains { get; set; } = default!;
    private List<ChartRecord> TotalBaseStatGains { get; set; } = default!;
    //private int ChosenDay { get; set; }
    //private List<ChartRecord> AverageExperienceGains { get; set; } = default!;
    //private List<ChartRecord> ChosenDayExperienceGains => Result
    //    .SimulatedDays[ChosenDay - 1]
    //    .ExperienceGain
    //    .OrderBy(x => x.Key)
    //    .Select((kvp) => new ChartRecord(kvp.Key.GetDisplayName(), kvp.Value))
    //    .ToList();
    //private List<ChartRecord> AverageBaseStatGains { get; set; } = default!;
    //private List<ChartRecord> ChosenDayBaseStatGains => Result
    //    .SimulatedDays[ChosenDay - 1]
    //    .BaseStatGain
    //    .Select((kvp) => new ChartRecord(kvp.Key.GetDisplayName(), kvp.Value))
    //    .ToList();

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
        }).OrderBy(x => x.Key)
            .Select((keyValuePair) => new ChartRecord(keyValuePair.Key.GetDisplayName(), keyValuePair.Value)).ToList();

        TotalBaseStatGains = Result.SimulatedDays.Aggregate(new Dictionary<GainSource, decimal>(), (acc, day) =>
        {
            foreach (var key in day.BaseStatGain.Keys)
            {
                acc.TryAdd(key, 0);
                acc[key] += day.BaseStatGain[key];
            }
            return acc;
        }).OrderBy(x => x.Key)
            .Select((keyValuePair) => new ChartRecord(keyValuePair.Key.GetDisplayName(), keyValuePair.Value)).ToList();

        //AverageExperienceGains = TotalExperienceGains.Select(t => new ChartRecord(t.Source, t.Value / Result.Days)).ToList();

        //AverageBaseStatGains = TotalBaseStatGains.Select(t => new ChartRecord(t.Source, t.Value / Result.Days)).ToList();

        base.OnParametersSet();
    }
}
