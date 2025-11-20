namespace SFSimulator.Core;

internal class PlagueDoctorFightContext : DelegatableFightableContext
{
    private double BaseDamageMultiplier { get; init; }
    private int PoisonRound { get; set; } = 0;
    private double[] PoisonDmgMultipliers { get; init; }
    private List<(double, double)> PoisonDmgRanges => PoisonDmgMultipliers.Select(c => (MinimumDamage * c, MaximumDamage * c)).ToList();

    private int EvadeChance => PoisonRound switch
    {
        3 => 65,
        2 => 50,
        1 => 35,
        _ => 20
    };

    public PlagueDoctorFightContext(ClassType enemyClass, double baseDamageMultiplier)
    {
        BaseDamageMultiplier = baseDamageMultiplier;
        PoisonDmgMultipliers = GetPoisonMultipliers();

        if (enemyClass == ClassType.Mage)
        {
            AttackImplementation = MageAttackImpl;
        }
        else
        {
            AttackImplementation = AttackImpl;
        }

        TakeAttackImplementation = TakeAttackImpl;
        WillTakeAttackImplementation = WillTakeAttackImpl;
    }

    public override void ResetState()
    {
        PoisonRound = 0;
        base.ResetState();
    }

    private bool AttackImpl(IAttackTakable target, ref int round)
    {
        if (PoisonRound <= 0 && Random.Next(1, 101) > 50)
        {
            round++;
            if (!target.WillTakeAttack())
            {
                return false;
            }

            PoisonRound = 3;

            var poisonMultiplier = PoisonDmgMultipliers[2];
            var tinctureThrowDmg = DungeonableDefaultImplementation.CalculateNormalHitDamage(MinimumDamage * poisonMultiplier, 
            MaximumDamage * poisonMultiplier, round, CritChance, CritMultiplier, Random);

            return target.TakeAttack(tinctureThrowDmg, ref round);
        }

        if (PoisonRound > 0)
        {
            round++;
            PoisonRound--;
            var poisonMultiplier = PoisonDmgMultipliers[PoisonRound];
            var poisonDmg = DungeonableDefaultImplementation.CalculateNormalHitDamage(MinimumDamage * poisonMultiplier, 
            MaximumDamage * poisonMultiplier, round, CritChance, CritMultiplier, Random);

            if (target.TakeAttack(poisonDmg, ref round))
            {
                return true;
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

    private bool TakeAttackImpl(double damage, ref int round)
    {
        Health -= (long)damage;
        return Health <= 0;
    }

    private bool MageAttackImpl(IAttackTakable target, ref int round)
    {
        round++;
        var dmg = DungeonableDefaultImplementation.CalculateNormalHitDamage(MinimumDamage, MaximumDamage,
            round, CritChance, CritMultiplier, Random);
        return target.TakeAttack(dmg, ref round);
    }

    private bool WillTakeAttackImpl()
    {
        return Random.Next(1, 101) > EvadeChance;
    }

    private double[] GetPoisonMultipliers()
    {
        var dmgMultiplier = ClassConfigurationProvider.Get(ClassType.PlagueDoctor).DamageMultiplier;
        var classSpecificDmgMultiplier = BaseDamageMultiplier / dmgMultiplier;

        return
        [
            (BaseDamageMultiplier - 0.9 * classSpecificDmgMultiplier) / BaseDamageMultiplier,
            (BaseDamageMultiplier - 0.55 * classSpecificDmgMultiplier) / BaseDamageMultiplier,
            (BaseDamageMultiplier - 0.2 * classSpecificDmgMultiplier) / BaseDamageMultiplier,
        ];
    }
}