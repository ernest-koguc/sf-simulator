namespace SFSimulator.Core;

public interface IFightableContext : IAttackable, IAttackTakable
{
    double Health { get; set; }
    int Reaction { get; }
    void ResetState();

    AttackBeforeFightDelegate? AttackBeforeFightImplementation { get; }
    WillSkipRoundDelegate? WillSkipRoundImplementation { get; }

    delegate bool AttackDelegate(IAttackTakable target, ref int round);
    delegate bool TakeAttackDelegate(double damage, ref int round);
    delegate bool WillTakeAttackDelegate();
    delegate bool AttackBeforeFightDelegate(IAttackTakable target, ref int round);
    delegate bool WillSkipRoundDelegate(ref int round);
}