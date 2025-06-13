namespace SFSimulator.Core;
public record SimulationProgress(int CurrentValue, int CurrentDay, int SimulateUntil)
{
    public int Progress => (int)((double)CurrentValue / SimulateUntil * 100d);
}
