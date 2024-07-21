namespace SFSimulator.Core;

public class BardFightContext : DelegatableFightableContext
{
    private readonly (int duration, double dmgBonus)[] MelodiesEffects = new (int, double)[]
    {
        (1, 1.2),
        (1, 1.4),
        (2, 1.6)
    };

    public int MelodyLength { get; set; } = -1;
    public double MelodyDmgMultiplier { get; set; } = 1;
    private int MelodyAssignRound { get; set; }
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
            MelodyAssignRound = 1;
        }
    }

    public override void ResetState()
    {
        base.ResetState();
        MelodyLength = 0;
        MelodyDmgMultiplier = 1;
        MelodyAssignRound = 1;
    }

    private bool NoMelodiesAttackImpl(IAttackTakable target, ref int round)
    {
        round++;

        var dmg = DungeonableDefaultImplementation.CalculateNormalHitDamage(MinimumDamage, MaximumDamage, round, CritChance, CritMultiplier, Random);

        return target.TakeAttack(dmg);
    }

    private bool AttackImpl(IAttackTakable target, ref int round)
    {
        round++;

        if (MelodyAssignRound <= round)
            AssignMelodies(round);

        if (MelodyLength == 0)
            MelodyDmgMultiplier = 1;

        //Console.WriteLine($"Round: {round}, Melody length: {MelodyLength}, MelodyDmg: {MelodyDmgMultiplier}");
        MelodyLength--;

        if (!target.WillTakeAttack())
            return false;

        var dmg = DungeonableDefaultImplementation.CalculateNormalHitDamage(MinimumDamage, MaximumDamage, round, CritChance, CritMultiplier, Random) * MelodyDmgMultiplier;

        return target.TakeAttack(dmg);
    }

    private bool TakeAttackImpl(double damage)
    {
        Health -= (long)damage;
        return Health <= 0;
    }

    private bool WillTakeAttackImpl() => true;

    private void AssignMelodies(int round)
    {
        var rng = Random.NextDouble();
        var effectedRolled = 2;
        if (rng < 0.50)
            effectedRolled = 1;
        else if (rng < 0.75)
            effectedRolled = 0;

        var (duration, dmgBonus) = MelodiesEffects[effectedRolled];
        MelodyLength = duration;
        MelodyDmgMultiplier = dmgBonus;
        MelodyAssignRound = round + MelodiesEffects[2].duration * 2;
    }
}