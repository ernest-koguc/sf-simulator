namespace SFSimulator.Core;

internal interface IBeforeFightAttackable
{
    bool AttackBeforeFight(IAttackTakable target, ref int round);
}