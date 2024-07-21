using Microsoft.AspNetCore.Components;
using SFSimulator.Core;

namespace SFSimulator.Frontend.Components;

public partial class SimulationResultView
{
    [Parameter]
    public SimulationResult? Result { get; set; }
    private List<Gain>? TotalExperienceGains => Result?.TotalGains.ExperienceGain.Select(x => new Gain(x.Key.ToLabel(), x.Value)).ToList();
    private List<Gain>? TotalBaseStatsGains => Result?.TotalGains.BaseStatGain.Select(x => new Gain(x.Key.ToLabel(), x.Value)).ToList();

    private record Gain(string Source, decimal Value);
    private string FormatString(object value)
    {
        var type = value.GetType();
        if (value is not double)
            return value?.ToString() ?? string.Empty;

        var val = Convert.ToDouble(value);

        if (val > 1_000_000_000)
            return $"{val / 1_000_000_000:N2}B";
        if (val > 1_000_000)
            return $"{val / 1_000_000:N2}M";
        if (val > 1_000)
            return $"{val / 1_000:N2}K";

        return val.ToString("N2");
    }
}

public static class GainSourceExtensions
{
    public static string ToLabel(this GainSource gain) => gain.ToString().Replace("_", " ").ToUpper();
}
