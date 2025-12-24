namespace SFSimulator.Core;

public class BardFightContext : FightContextBase
{
    private readonly (int duration, double dmgBonus)[] MelodiesEffects =
    [
        (3, 1.2),
        (3, 1.4),
        (4, 1.6)
    ];

    public int MelodyLength { get; set; } = -1;
    public int NextMelodyRound { get; set; } = 0;
    public double MelodyDmgMultiplier { get; set; } = 1;

    public BardFightContext(ClassType enemyClass)
    {
        TakeAttackImplementation = TakeAttackImpl;
        WillTakeAttackImplementation = WillTakeAttackImpl;

        if (enemyClass == ClassType.Mage)
            AttackImplementation = NoMelodiesAttackImpl;
        else
        {
            AttackImplementation = AttackImpl;
        }
    }

    public override void ResetState()
    {
        base.ResetState();
        MelodyLength = 0;
        MelodyDmgMultiplier = 1;
        NextMelodyRound = 0;
    }

    private bool NoMelodiesAttackImpl(IAttackTakable target, ref int round)
    {
        round++;

        var dmg = CalculateNormalHitDamage(MinimumDamage, MaximumDamage,
            round, CritChance, CritMultiplier, Random);

        return target.TakeAttackImplementation(dmg, ref round);
    }

    private bool AttackImpl(IAttackTakable target, ref int round)
    {
        round++;

        if (MelodyLength == 0)
        {
            MelodyDmgMultiplier = 1;
        }
        if (MelodyLength <= 0 && NextMelodyRound <= 0)
        {
            AssignMelodies();
        }
        MelodyLength--;
        NextMelodyRound--;

        if (!target.WillTakeAttackImplementation())
            return false;

        var dmg = CalculateNormalHitDamage(MinimumDamage, MaximumDamage,
            round, CritChance, CritMultiplier, Random) * MelodyDmgMultiplier;

        return target.TakeAttackImplementation(dmg, ref round);
    }

    private bool TakeAttackImpl(double damage, ref int round)
    {
        Health -= damage;
        return Health <= 0;
    }

    private bool WillTakeAttackImpl() => true;

    private void AssignMelodies()
    {
        var rng = Random.NextDouble();
        var melody = 2;
        if (rng < 0.50)
            melody = 1;
        else if (rng < 0.75)
            melody = 0;

        (MelodyLength, MelodyDmgMultiplier) = MelodiesEffects[melody];
        NextMelodyRound = 4;
    }
}