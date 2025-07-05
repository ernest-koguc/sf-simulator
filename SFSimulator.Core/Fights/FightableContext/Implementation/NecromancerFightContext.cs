namespace SFSimulator.Core;
public class NecromancerFightContext : DelegatableFightableContext
{
    public required double BaseDamageMultiplier { get; init; }
    private NecromancerMinionType MinionType { get; set; } = NecromancerMinionType.None;
    private int MinionRounds { get; set; } = 0;
    private int SkeletonRevivesCount { get; set; } = 0;
    public NecromancerFightContext(ClassType opponentClass)
    {
        if (opponentClass != ClassType.Mage)
        {
            AttackImplementation = AttackImpl;
            WillTakeAttackImplementation = WillTakeAttackImpl;
        }
        else
        {
            AttackImplementation = MageAttackImpl;
            WillTakeAttackImplementation = NoBlockWillTakeAttackImpl;
        }

        TakeAttackImplementation = TakeAttackImpl;
    }

    public override void ResetState()
    {
        MinionRounds = 0;
        SkeletonRevivesCount = 0;
        MinionType = NecromancerMinionType.None;
        base.ResetState();
    }

    private bool MageAttackImpl(IAttackTakable target, ref int round)
    {
        round++;

        var dmg = DungeonableDefaultImplementation.CalculateNormalHitDamage(MinimumDamage, MaximumDamage, round, CritChance, CritMultiplier, Random);

        return target.TakeAttack(dmg, ref round);
    }

    private bool NoBlockWillTakeAttackImpl()
    {
        return true;
    }

    private bool AttackImpl(IAttackTakable target, ref int round)
    {
        round++;

        if (MinionType == NecromancerMinionType.None && Random.Next(1, 101) <= 50)
        {
            SummonMinion();
            return AttackWithMinion(target, ref round);
        }

        if (target.WillTakeAttack())
        {
            var dmg = DungeonableDefaultImplementation.CalculateNormalHitDamage(MinimumDamage, MaximumDamage,
                round, CritChance, CritMultiplier, Random);
            if (target.TakeAttack(dmg, ref round))
            {
                return true;
            }
        }

        return AttackWithMinion(target, ref round);
    }

    private bool TakeAttackImpl(double damage, ref int round)
    {
        Health -= (long)damage;
        return Health <= 0;
    }

    private bool WillTakeAttackImpl()
    {
        if (MinionType != NecromancerMinionType.Golem)
        {
            return true;
        }

        return Random.Next(1, 101) > 25;
    }

    private bool AttackWithMinion(IAttackTakable target, ref int round)
    {
        if (MinionType == NecromancerMinionType.None)
        {
            return false;
        }

        round++;

        MinionRounds--;
        var currentMinion = MinionType;
        // TODO: this is currently not working as in game but we are comparing to SFTOOLS
        if (MinionRounds == 0 && MinionType == NecromancerMinionType.Skeleton && SkeletonRevivesCount < 2
            )//&& Random.Next(1, 101) > 50)
        {
            MinionRounds = 1;
            SkeletonRevivesCount++;
        }
        else if (MinionRounds == 0)
        {
            MinionType = NecromancerMinionType.None;
            SkeletonRevivesCount = 0;
        }

        if (!target.WillTakeAttack())
        {
            return false;
        }

        var critChance = currentMinion == NecromancerMinionType.Hound ? Math.Min(0.6, CritChance + 0.1) : CritChance;
        var critMultiplier = currentMinion == NecromancerMinionType.Hound ? 2.5 * CritMultiplier / 2 : CritMultiplier;
        var dmg = DungeonableDefaultImplementation.CalculateNormalHitDamage(MinimumDamage, MaximumDamage,
            round, critChance, critMultiplier, Random);
        dmg *= GetMinionDmgMultiplier(currentMinion);

        return target.TakeAttack(dmg, ref round);
    }

    private void SummonMinion()
    {
        var minionTypeChance = Random.Next(1, 4);

        MinionType = minionTypeChance switch
        {
            1 => NecromancerMinionType.Skeleton,
            2 => NecromancerMinionType.Hound,
            3 => NecromancerMinionType.Golem,
            _ => throw new ArgumentOutOfRangeException(nameof(minionTypeChance), "Invalid minion type chance generated.")
        };

        MinionRounds = MinionType switch
        {
            NecromancerMinionType.Skeleton => 3,
            NecromancerMinionType.Hound => 2,
            NecromancerMinionType.Golem => 4,
            _ => throw new ArgumentOutOfRangeException(nameof(MinionType), "Invalid minion type.")
        };
    }

    private double GetMinionDmgMultiplier(NecromancerMinionType minionType)
    {
        return minionType switch
        {
            NecromancerMinionType.Skeleton => (BaseDamageMultiplier + 0.25) / BaseDamageMultiplier,
            NecromancerMinionType.Hound => (BaseDamageMultiplier + 1) / BaseDamageMultiplier,
            NecromancerMinionType.Golem => 1,
            _ => throw new ArgumentOutOfRangeException(nameof(minionType), "Invalid minion type.")
        };
    }
}
