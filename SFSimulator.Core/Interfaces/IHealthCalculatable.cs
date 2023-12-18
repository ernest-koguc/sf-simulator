namespace SFSimulator.Core;

public interface IHealthCalculatable
{
    public ClassType Class { get; }
    public int Level { get; }
    public int Constitution { get; }
    public double SoloPortal { get; }
    public bool HasEternityPotion { get; }
    public int HealthRune { get; }
}
