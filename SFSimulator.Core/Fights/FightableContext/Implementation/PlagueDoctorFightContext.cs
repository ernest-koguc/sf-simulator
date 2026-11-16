namespace SFSimulator.Core;

// TODO: Demon Hunter vs Plague Doctor revive bug
internal class PlagueDoctorFightContext : FightContextBase
{
    private double BaseDamageMultiplier { get; init; }
    private int PoisonRound { get; set; } = 0;
    private double[] PoisonDmgMultipliers { get; init; }
    private double CurrentPoisonMultiplier { get; set; } = 1;
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
        CurrentPoisonMultiplier = 1;
        base.ResetState();
    }

    private bool AttackImpl(IAttackTakable target, ref int round)
    {
        if (PoisonRound <= 0 && Random.Next(1, 101) > 50)
        {
            round++;
            CurrentPoisonMultiplier = PoisonDmgMultipliers[2];
            if (!target.WillTakeAttackImplementation())
            {
                return false;
            }

            PoisonRound = 3;

            var tinctureThrowDmg = CalculateNormalHitDamage(MinimumDamage * CurrentPoisonMultiplier,
                MaximumDamage * CurrentPoisonMultiplier, round, CritChance, CritMultiplier, Random);

            // NOTE: dirty trick to make Paladin vs Plague Doctor work.
            // In general I think this is okay to keep right now since refactoring is expensive and
            // usually each new class has some unique random shit never seen before that will break it again
            if (target is PaladinFightContext paladinTarget)
            {
                var hpBeforeAttack = paladinTarget.Health;
                var result = target.TakeAttackImplementation(tinctureThrowDmg, ref round);
                PoisonRound = hpBeforeAttack > paladinTarget.Health ? 3 : 0;
                return result;
            }
            return target.TakeAttackImplementation(tinctureThrowDmg, ref round);
        }

        if (PoisonRound > 0)
        {
            var result = PoisonAttack(target, ref round);
            if (PoisonRound == 0)
                CurrentPoisonMultiplier = 1;

            return result;
        }
        else 
        {
            round++;
            if (!target.WillTakeAttackImplementation())
            {
                return false;
            }

            // BUG: We use CurrentPoisonMultiplier here becuase PD has a bug - if the Tincture Throw is blocked or enemy revived after the poison hit
            // PD stays on the last poison/tincture multiplier
            var dmg = CalculateNormalHitDamage(MinimumDamage * CurrentPoisonMultiplier, MaximumDamage * CurrentPoisonMultiplier,
                round, CritChance, CritMultiplier, Random);

            return target.TakeAttackImplementation(dmg, ref round);
        }
    }

    private bool PoisonAttack(IAttackTakable target, ref int round)
    {
        round++;
        PoisonRound--;
        CurrentPoisonMultiplier = PoisonDmgMultipliers[PoisonRound];
        var poisonDmg = CalculateNormalHitDamage(MinimumDamage * CurrentPoisonMultiplier,
        MaximumDamage * CurrentPoisonMultiplier, round, CritChance, CritMultiplier, Random);

        // NOTE: dirty trick to make Paladin vs Plague Doctor work
        if (target is PaladinFightContext paladinTarget)
        {
            CurrentPoisonMultiplier = 1;
            if (paladinTarget.NoBlockTakeAttackImpl(poisonDmg, ref round))
            {
                return true;
            }
        }
        // BUG: follow in-game revive bug that makes PD do poison dmg on normal hit after revive
        else
        {
            var targetContext = target as FightContextBase;
            var revivesBefore = targetContext!.ReviveCount;
            var result = target.TakeAttackImplementation(poisonDmg, ref round);
            if (revivesBefore == targetContext.ReviveCount)
            {
                CurrentPoisonMultiplier = 1;
            }

            if (result) return true;
        }

        round++;
        if (!target.WillTakeAttackImplementation())
        {
            return false;
        }

        // BUG: We use CurrentPoisonMultiplier here becuase PD has a bug - if the Tincture Throw is blocked or enemy revived after the poison hit
        // PD stays on the last poison/tincture multiplier
        var dmg = CalculateNormalHitDamage(MinimumDamage * CurrentPoisonMultiplier, MaximumDamage * CurrentPoisonMultiplier,
            round, CritChance, CritMultiplier, Random);

        return target.TakeAttackImplementation(dmg, ref round);
    }

    private bool TakeAttackImpl(double damage, ref int round)
    {
        Health -= damage;
        return Health <= 0;
    }

    private bool MageAttackImpl(IAttackTakable target, ref int round)
    {
        round++;
        var dmg = CalculateNormalHitDamage(MinimumDamage, MaximumDamage,
            round, CritChance, CritMultiplier, Random);
        return target.TakeAttackImplementation(dmg, ref round);
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
            ((BaseDamageMultiplier / classSpecificDmgMultiplier - 0.9) * classSpecificDmgMultiplier) /
            BaseDamageMultiplier,
            ((BaseDamageMultiplier / classSpecificDmgMultiplier - 0.55) * classSpecificDmgMultiplier) /
            BaseDamageMultiplier,
            ((BaseDamageMultiplier / classSpecificDmgMultiplier - 0.2) * classSpecificDmgMultiplier) /
            BaseDamageMultiplier
        ];
    }
}