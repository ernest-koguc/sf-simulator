﻿namespace SFSimulator.Core;

public class DruidFightContext : DelegatableFightableContext
{
    private int EvadeChance { get; set; } = 35;
    private bool IsInBearForm { get; set; } = false;
    public double RageCritChance { get; set; }
    private double RageCritMultiplierBonus { get; set; } = 4D;
    private double SwoopChance { get; set; } = 0.15;
    private double SwoopDamageModifier { get; set; } = (1 / 3D + 0.8D) / (1 / 3D);
    private ClassType OpponentClass { get; set; }

    public DruidFightContext(ClassType opponentClass)
    {
        OpponentClass = opponentClass;
        if (opponentClass != ClassType.Mage)
        {
            AttackImplementation = AttackImpl;
        }
        else
        {
            AttackImplementation = AttackMageImpl;
        }
        TakeAttackImplementation = TakeAttackImpl;
        WillTakeAttackImplementation = WillTakeAttackImpl;
    }

    public override void ResetState()
    {
        base.ResetState();
        SwoopChance = 0.15;
        IsInBearForm = false;
        if (OpponentClass != ClassType.Mage)
        {
            AttackImplementation = AttackImpl;
        }
        else
        {
            AttackImplementation = AttackMageImpl;
        }
    }

    private bool AttackInBearForm(IAttackTakable target, ref int round)
    {
        IsInBearForm = true;
        AttackImplementation = AttackImpl;
        round++;

        if (!target.WillTakeAttack())
        {
            return false;
        }

        var critMultiplier = (2 + RageCritMultiplierBonus) * CritMultiplier / 2;
        var dmg = DungeonableDefaultImplementation.CalculateNormalHitDamage(MinimumDamage, MaximumDamage,
            round, RageCritChance, critMultiplier, Random);

        return target.TakeAttack(dmg, ref round);
    }

    private bool AttackImpl(IAttackTakable target, ref int round)
    {
        IsInBearForm = false;
        var willSwoop = Random.NextDouble() < SwoopChance;

        if (willSwoop)
        {
            round++;

            SwoopChance = Math.Min(0.5, SwoopChance + 0.05);
            if (target.WillTakeAttack())
            {
                var swoopDmg = DungeonableDefaultImplementation.CalculateNormalHitDamage(MinimumDamage, MaximumDamage,
                    round, CritChance, CritMultiplier, Random) * SwoopDamageModifier;

                if (target.TakeAttack(swoopDmg, ref round))
                {
                    return true;
                }
            }
        }

        round++;

        if (!target.WillTakeAttack())
        {
            return false;
        }

        var dmg = DungeonableDefaultImplementation.CalculateNormalHitDamage(MinimumDamage, MaximumDamage,
            round, CritChance, CritMultiplier, Random);

        return target.TakeAttack(dmg, ref round);
    }

    private bool AttackMageImpl(IAttackTakable target, ref int round)
    {
        round++;
        var dmg = DungeonableDefaultImplementation.CalculateNormalHitDamage(MinimumDamage, MaximumDamage,
            round, CritChance, CritMultiplier, Random);

        return target.TakeAttack(dmg, ref round);
    }

    private bool TakeAttackImpl(double damage, ref int round)
    {
        Health -= (long)damage;
        return Health <= 0;
    }

    private bool WillTakeAttackImpl()
    {
        if (!IsInBearForm && Random.Next(1, 101) <= EvadeChance)
        {
            AttackImplementation = AttackInBearForm;
            return false;
        }

        return true;
    }
}