namespace SFSimulator.Core;

internal class PaladinFightContext : DelegatableFightableContext
{
    public required double InitialArmorReduction { get; set; }

    private PaladinStanceType Stance { get; set; } = PaladinStanceType.Initial;
    private double CurrentArmorReduction => Stance switch
    {
        PaladinStanceType.Initial or PaladinStanceType.Defensive => 1,
        PaladinStanceType.Offensive => 1 / (1 - InitialArmorReduction) * (1 - Math.Min(0.20, InitialArmorReduction)),
        _ => throw new NotSupportedException($"Unknown stance: {Stance}"),
    };

    private double DamageMultiplier => Stance switch
    {
        PaladinStanceType.Initial => 1,
        PaladinStanceType.Defensive => (1 / 0.833) * 0.568,
        PaladinStanceType.Offensive => (1 / 0.833) * 1.253,
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

        ChangeStance();

        if (!target.WillTakeAttack())
        {
            return false;
        }

        var dmg = DungeonableDefaultImplementation.CalculateNormalHitDamage(MinimumDamage, MaximumDamage,
            round, CritChance, CritMultiplier, Random) * DamageMultiplier;

        return target.TakeAttack(dmg, ref round);
    }

    private bool TakeAttackImpl(double damage, ref int round)
    {
        var actualDamage = damage * CurrentArmorReduction;

        if (Stance == PaladinStanceType.Defensive && Random.Next(1, 101) <= BlockChance)
        {
            var health = actualDamage * 0.3;
            Health += Math.Min(Math.Max(0, MaxHealth - Health), (long)health);

            return false;
        }

        Health -= (long)actualDamage;
        return Health <= 0;
    }

    private bool MageAttackImpl(IAttackTakable target, ref int round)
    {
        round++;
        var dmg = DungeonableDefaultImplementation.CalculateNormalHitDamage(MinimumDamage, MaximumDamage, round, CritChance, CritMultiplier, Random);
        return target.TakeAttack(dmg, ref round);
    }

    private bool NoBlockTakeAttackImpl(double damage, ref int round)
    {
        Health -= (long)damage;
        return Health <= 0;
    }

    private bool WillTakeAttackImpl()
    {
        return Stance == PaladinStanceType.Defensive || Random.Next(1, 101) > BlockChance;
    }

    private void ChangeStance()
    {
        var shouldChangeStance = Random.Next(1, 101) <= 50;

        if (!shouldChangeStance)
        {
            return;
        }

        Stance = Stance switch
        {
            PaladinStanceType.Initial => PaladinStanceType.Defensive,
            PaladinStanceType.Defensive => PaladinStanceType.Offensive,
            PaladinStanceType.Offensive => PaladinStanceType.Initial,
            _ => throw new NotSupportedException($"Unknown stance: {Stance}"),
        };
    }
}