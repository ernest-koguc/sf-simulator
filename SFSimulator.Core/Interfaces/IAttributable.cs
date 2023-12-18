namespace SFSimulator.Core;

public interface IAttributable
{
    public int UpgradeLevel { get; }
    public GemType GemType { get; }
    public int GemValue { get; }

    public int Strength { get; }
    public int Dexterity { get; }
    public int Intelligence { get; }
    public int Constitution { get; }
    public int Luck { get; }
}
