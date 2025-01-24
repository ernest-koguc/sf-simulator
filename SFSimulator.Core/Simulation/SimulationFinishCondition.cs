namespace SFSimulator.Core;
public class SimulationFinishCondition
{
    public SimulationFinishConditionType FinishWhen { get; set; }
    public int Until { get; set; } = 1;
}
