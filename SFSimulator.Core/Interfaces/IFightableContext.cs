namespace SFSimulator.Core;

public interface IFightableContext : IAttackable, IAttackTakable
{
    void ResetState();
}
