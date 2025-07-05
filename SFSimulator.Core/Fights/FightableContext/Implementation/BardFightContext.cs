namespace SFSimulator.Core;

public class BardFightContext : DelegatableFightableContext
{
    private readonly (int duration, double dmgBonus)[] MelodiesEffects =
    [
        (1, 1.2),
        (1, 1.4),
        (2, 1.6)
    ];

    public int MelodyLength { get; set; } = -1;
    public int NextMelodyRound { get; set; } = 0;
    public double MelodyDmgMultiplier { get; set; } = 1;

    public BardFightContext(ClassType enemyClass, int melodyLengthBonus)
    {
        TakeAttackImplementation = TakeAttackImpl;
        WillTakeAttackImplementation = WillTakeAttackImpl;

        if (enemyClass == ClassType.Mage)
            AttackImplementation = NoMelodiesAttackImpl;
        else
        {
            AttackImplementation = AttackImpl;

            for (var i = 0; i < MelodiesEffects.Length; i++)
            {
                MelodiesEffects[i].duration += melodyLengthBonus;
            }
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

        var dmg = DungeonableDefaultImplementation.CalculateNormalHitDamage(MinimumDamage, MaximumDamage,
            round, CritChance, CritMultiplier, Random);

        return target.TakeAttack(dmg, ref round);
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

        if (!target.WillTakeAttack())
            return false;

        var dmg = DungeonableDefaultImplementation.CalculateNormalHitDamage(MinimumDamage, MaximumDamage,
            round, CritChance, CritMultiplier, Random) * MelodyDmgMultiplier;

        return target.TakeAttack(dmg, ref round);
    }

    private bool TakeAttackImpl(double damage, ref int round)
    {
        Health -= (long)damage;
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