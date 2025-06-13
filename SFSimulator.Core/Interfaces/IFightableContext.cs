namespace SFSimulator.Core;

public interface IFightableContext : IAttackable, IAttackTakable
{
    long Health { get; set; }
    int Reaction { get; }
    void ResetState();
}