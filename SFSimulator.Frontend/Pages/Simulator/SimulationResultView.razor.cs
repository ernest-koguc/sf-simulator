using Microsoft.AspNetCore.Components;
using SFSimulator.Core;

namespace SFSimulator.Frontend.Pages.Simulator;

public partial class SimulationResultView
{
    [Parameter, EditorRequired]
    public required SimulationResult Result { get; set; }
    private List<ChartRecord> TotalExperienceGains { get; set; } = default!;
    private List<ChartRecord> TotalBaseStatGains { get; set; } = default!;

    private List<ChartRecord> LevelProgress { get; set; } = default!;
    private List<ChartRecord> BaseStatProgress { get; set; } = default!;

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

        LevelProgress = Result.SimulatedDays.Select(d => new ChartRecord($"Day - {d.DayIndex}", d.Level))
            .ToList();
        BaseStatProgress = Result.SimulatedDays.Select(d => new ChartRecord($"Day - {d.DayIndex}", d.BaseStat))
            .ToList();

        base.OnParametersSet();
    }
}
