namespace SFSimulator.Core;
internal class PaladinFightContext : DelegatableFightableContext
{
    public PaladinStanceType Stance { get; set; } = PaladinStanceType.Initial;
    public required double InitialArmorReduction { get; set; }
    private ClassType opponentClass;
    private double CurrentArmorReduction => Stance switch
    {
        PaladinStanceType.Initial or PaladinStanceType.Defensive => Math.Min(0.45, InitialArmorReduction),
        PaladinStanceType.Offensive => Math.Min(0.20, InitialArmorReduction),
        _ => throw new NotSupportedException($"Unknown stance: {Stance}"),
    };

    private double DamageMultiplier => Stance switch
    {
        PaladinStanceType.Initial => 0.833,
        PaladinStanceType.Defensive => 0.568,
        PaladinStanceType.Offensive => 1.253,
        _ => throw new NotSupportedException($"Unknown stance: {Stance}"),
    };

    private int BlockChance => Stance switch
    {
        PaladinStanceType.Initial => 30,
        PaladinStanceType.Defensive => 50,
        PaladinStanceType.Offensive => 25,
        _ => throw new NotSupportedException($"Unknown stance: {Stance}"),
    };

    public PaladinFightContext(ClassType enemyClass)
    {
        opponentClass = enemyClass;
        if (enemyClass == ClassType.Mage)
        {
            AttackImplementation = MageAttackImpl;
            TakeAttackImplementation = NoBlockTakeAttackImpl;
        }
        else
        {
            AttackImplementation = AttackImpl;
            TakeAttackImplementation = TakeAttackImpl;
        }
        WillTakeAttackImplementation = WillTakeAttackImpl;
    }

    public override void ResetState()
    {
        Stance = PaladinStanceType.Initial;
        base.ResetState();
    }

    private bool AttackImpl(IAttackTakable target, ref int round)
    {
        round++;

        if (opponentClass != ClassType.Mage)
        {
            ChangeStance();
        }


        if (!target.WillTakeAttack())
            return false;

        var dmg = DungeonableDefaultImplementation.CalculateNormalHitDamage(MinimumDamage, MaximumDamage, round, CritChance, CritMultiplier, Random);
        if (opponentClass != ClassType.Mage)
        {
            dmg *= DamageMultiplier;
        }

        return target.TakeAttack(dmg);
    }

    private bool TakeAttackImpl(double damage)
    {
        var actualDamage = damage * (1 - CurrentArmorReduction);
        if (Random.Next(1, 101) <= BlockChance)
        {
            if (Stance == PaladinStanceType.Defensive)
            {
                var maxHeal = MaxHealth - Health;
                var health = actualDamage * 0.3;
                Health += Math.Min(maxHeal, (long)health);
            }

            return false;
        }

        Health -= (long)actualDamage;
        return Health <= 0;
    }

    private bool MageAttackImpl(IAttackTakable target, ref int round)
    {
        round++;
        var dmg = DungeonableDefaultImplementation.CalculateNormalHitDamage(MinimumDamage, MaximumDamage, round, CritChance, CritMultiplier, Random);
        return target.TakeAttack(dmg);
    }

    private bool NoBlockTakeAttackImpl(double damage)
    {
        Health -= (long)damage;
        return Health <= 0;
    }

    // Here we always take attack since we need to know how much would we heal in defensive stance, we might also try to adjust the abstraction?
    private bool WillTakeAttackImpl() => true;

    private void ChangeStance()
    {
        var shouldChangeStance = Random.Next(1, 101) <= 50;

        if (!shouldChangeStance)
            return;

        Stance = Stance switch
        {
            PaladinStanceType.Initial => PaladinStanceType.Defensive,
            PaladinStanceType.Defensive => PaladinStanceType.Offensive,
            PaladinStanceType.Offensive => PaladinStanceType.Initial,
            _ => throw new NotSupportedException($"Unknown stance: {Stance}"),
        };
    }
}
