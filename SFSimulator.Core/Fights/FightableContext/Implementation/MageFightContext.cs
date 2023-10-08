﻿namespace SFSimulator.Core;

public class MageFightContext : DelegatableFightableContext
{
    public MageFightContext()
    {
        AttackImplementation = AttackImpl;
        TakeAttackImplementation = TakeAttackImpl;
        WillTakeAttackImplementation = WillTakeAttackImpl;
    }
    private bool AttackImpl(IAttackTakable target, ref int round)
    {
        round++;
        var dmg = DungeonableDefaultImplementation.CalculateNormalHitDamage(MinimumDamage, MaximumDamage, round, CritChance, CritMultiplier, Random);

        return target.TakeAttack(dmg);
    }

    private bool TakeAttackImpl(double damage)
    {
        Health -= (long)damage;
        return Health <= 0;
    }

    private bool WillTakeAttackImpl() => true;
}
