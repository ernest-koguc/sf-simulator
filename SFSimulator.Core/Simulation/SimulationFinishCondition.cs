namespace SFSimulator.Core;
public class SimulationFinishCondition
{
    public SimulationFinishConditionType FinishWhen { get; set; }
    public int Until { get; set; } = 1;
    public int MaxUntil => FinishWhen switch
    {
        SimulationFinishConditionType.UntilDays => 10000,
        SimulationFinishConditionType.UntilLevel => CoreShared.MAX_LEVEL,
        SimulationFinishConditionType.UntilBaseStats => 10_000_000,
        _ => throw new ArgumentOutOfRangeException(nameof(FinishWhen), FinishWhen, null)
    };
}
