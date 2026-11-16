namespace SFSimulator.Core;

public struct FightContext
{
    public ClassType Class;
    public ClassType OpponentClass;
    public int Reaction;
    public double MaxHealth;
    public double Health;
    public double MinimumDamage;
    public double MaximumDamage;
    public double CritChance;
    public double CritMultiplier;
    public int ReviveCount;
    public CombatRandom Random;

    private int BlockChance;
    private double SecondMinimumDamage;
    private double SecondMaximumDamage;
    private double FireBallDamage;
    private int ChainAttackCounter;
    private bool IsInBearForm;
    private DruidAttackMode NextDruidAttack;
    private double RageCritChance;
    private double SwoopChance;
    private double SwoopDamageMultiplier;
    private int MelodyLength;
    private int NextMelodyRound;
    private double MelodyDmgMultiplier;
    private double MelodyCritMultiplier;
    private double BaseDamageMultiplier;
    private NecromancerMinionType MinionType;
    private int MinionRounds;
    private int SkeletonRevivesCount;
    private double InitialArmorReduction;
    private PaladinStanceType Stance;
    private int PoisonRound;
    private double[]? PoisonDmgMultipliers;
    private double CurrentPoisonMultiplier;
    private int BloodPactStacks;
    private double DaggerDmgMultiplier;
    private double ScytheDmgMultiplier;

    public bool Attack(ref FightContext target, ref int round) => Class switch
    {
        ClassType.Mage => AttackWithoutAvoidance(ref target, ref round),
        ClassType.Assassin => Assassin.Attack(ref this, ref target, ref round),
        ClassType.Druid => Druid.Attack(ref this, ref target, ref round),
        ClassType.Bard => OpponentClass == ClassType.Mage
            ? AttackWithoutAvoidance(ref target, ref round)
            : Bard.Attack(ref this, ref target, ref round),
        ClassType.Necromancer => OpponentClass == ClassType.Mage
            ? AttackWithoutAvoidance(ref target, ref round)
            : Necromancer.Attack(ref this, ref target, ref round),
        ClassType.Paladin => OpponentClass == ClassType.Mage
            ? AttackWithoutAvoidance(ref target, ref round)
            : Paladin.Attack(ref this, ref target, ref round),
        ClassType.PlagueDoctor => OpponentClass == ClassType.Mage
            ? AttackWithoutAvoidance(ref target, ref round)
            : PlagueDoctor.Attack(ref this, ref target, ref round),
        ClassType.BloodWeaver => OpponentClass == ClassType.Mage
            ? AttackWithoutAvoidance(ref target, ref round)
            : BloodWeaver.Attack(ref this, ref target, ref round),
        _ => AttackNormally(ref target, ref round)
    };

    public bool WillTakeAttack() => Class switch
    {
        ClassType.Warrior or ClassType.Bert => Random.Next(1, 101) > BlockChance,
        ClassType.Scout or ClassType.Assassin => Random.Next(1, 101) > 50,
        ClassType.Druid => Druid.WillTakeAttack(ref this),
        ClassType.Necromancer => OpponentClass == ClassType.Mage
            || Necromancer.WillTakeAttack(ref this),
        ClassType.Paladin => Paladin.WillTakeAttack(ref this),
        ClassType.PlagueDoctor => PlagueDoctor.WillTakeAttack(ref this),
        ClassType.BloodWeaver => Random.Next(1, 101) > BlockChance,
        _ => true
    };

    public bool TakeAttack(double damage, ref int round)
    {
        switch (Class)
        {
            case ClassType.DemonHunter when OpponentClass != ClassType.Mage:
                return DemonHunter.TakeAttack(ref this, damage, ref round);
            case ClassType.Paladin:
                return OpponentClass == ClassType.Mage
                    ? Paladin.TakeAttackWithoutBlock(ref this, damage, ref round)
                    : Paladin.TakeAttack(ref this, damage, ref round);
            case ClassType.BloodWeaver when OpponentClass != ClassType.Mage:
                return BloodWeaver.TakeAttack(ref this, damage, ref round);
            default:
                Health -= damage;
                return Health <= 0;
        }
    }

    public bool AttackBeforeFight(ref FightContext target, ref int round)
    {
        if (Class != ClassType.BattleMage)
            return false;

        round++;
        return target.TakeAttack(FireBallDamage, ref round);
    }

    public bool WillSkipRound(ref int round) => Class == ClassType.Berserker
        && Berserker.WillSkipRound(ref this, ref round);

    public void ResetState()
    {
        Health = MaxHealth;
        ReviveCount = 0;

        switch (Class)
        {
            case ClassType.Berserker:
                ChainAttackCounter = 0;
                break;
            case ClassType.Druid:
                SwoopChance = 0.15;
                IsInBearForm = false;
                NextDruidAttack = OpponentClass == ClassType.Mage ? DruidAttackMode.Mage : DruidAttackMode.Normal;
                break;
            case ClassType.Bard:
                MelodyLength = 0;
                MelodyDmgMultiplier = 1;
                NextMelodyRound = 0;
                break;
            case ClassType.Necromancer:
                MinionRounds = 0;
                SkeletonRevivesCount = 0;
                MinionType = NecromancerMinionType.None;
                break;
            case ClassType.Paladin:
                Stance = PaladinStanceType.Initial;
                break;
            case ClassType.PlagueDoctor:
                PoisonRound = 0;
                CurrentPoisonMultiplier = 1;
                break;
            case ClassType.BloodWeaver:
                BlockChance = 0;
                BloodPactStacks = 0;
                break;
        }
    }

    internal void InitializeClassState<T, E>(IFightable<T> main, IFightable<E> opponent,
        IDamageProvider damageProvider, ICritChanceProvider critChanceProvider)
        where T : IWeaponable where E : IWeaponable
    {
        switch (main.Class)
        {
            case ClassType.Warrior:
                BlockChance = 25;
                break;
            case ClassType.Mage:
            case ClassType.Scout:
            case ClassType.Bert:
            case ClassType.Berserker:
            case ClassType.DemonHunter:
                break;
            case ClassType.Assassin:
                (SecondMinimumDamage, SecondMaximumDamage) =
                    damageProvider.CalculateDamage(main.SecondWeapon, main, opponent, true);
                break;
            case ClassType.BattleMage:
                FireBallDamage = damageProvider.CalculateFireBallDamage(main, opponent);
                break;
            case ClassType.Druid:
                RageCritChance = critChanceProvider.CalculateCritChance(main, opponent, 0.75D, 0.1);
                SwoopDamageMultiplier = damageProvider.CalculateSwoopMultiplier(main, opponent);
                SwoopChance = 0.15;
                NextDruidAttack = opponent.Class == ClassType.Mage ? DruidAttackMode.Mage : DruidAttackMode.Normal;
                break;
            case ClassType.Bard:
                MelodyLength = -1;
                MelodyDmgMultiplier = 1;
                MelodyCritMultiplier = 1;
                break;
            case ClassType.Necromancer:
                BaseDamageMultiplier = damageProvider.CalculateDamageMultiplier(main, opponent);
                break;
            case ClassType.Paladin:
                InitialArmorReduction = damageProvider.CalculateDamageReduction(opponent, main);
                break;
            case ClassType.PlagueDoctor:
                BaseDamageMultiplier = damageProvider.CalculateDamageMultiplier(main, opponent);
                PoisonDmgMultipliers = PlagueDoctor.GetPoisonMultipliers(BaseDamageMultiplier);
                CurrentPoisonMultiplier = 1;
                break;
            case ClassType.BloodWeaver:
                DaggerDmgMultiplier = damageProvider.CalculateDaggerMultiplier(main, opponent);
                ScytheDmgMultiplier = damageProvider.CalculateScytheMultiplier(main, opponent);
                break;
            default:
                throw new ArgumentOutOfRangeException(nameof(main), $"{main.Class} is not supported in FightableContext");
        }
    }

    private bool AttackNormally(ref FightContext target, ref int round)
    {
        round++;
        if (!target.WillTakeAttack())
            return false;

        var damage = CalculateNormalHitDamage(MinimumDamage, MaximumDamage, round, CritChance, CritMultiplier, Random);
        return target.TakeAttack(damage, ref round);
    }

    private bool AttackWithoutAvoidance(ref FightContext target, ref int round)
    {
        round++;
        var damage = CalculateNormalHitDamage(MinimumDamage, MaximumDamage, round, CritChance, CritMultiplier, Random);
        return target.TakeAttack(damage, ref round);
    }

    private static double CalculateNormalHitDamage(double minDmg, double maxDmg, int round,
        double critChance, double critMultiplier, CombatRandom random)
    {
        random.NextDoublePair(out var damageRoll, out var critRoll);
        var baseDamage = damageRoll * (1 + maxDmg - minDmg) + minDmg;
        var damage = baseDamage * (1 + (round - 1) * (1.0 / 6.0));
        if (critRoll < critChance)
            damage *= critMultiplier;

        return damage;
    }

    private enum DruidAttackMode
    {
        Normal,
        Bear,
        Mage
    }

    private static class Assassin
    {
        public static bool Attack(ref FightContext context, ref FightContext target, ref int round)
        {
            round++;

            if (target.WillTakeAttack())
            {
                var firstWeaponDamage = FightContext.CalculateNormalHitDamage(context.MinimumDamage, context.MaximumDamage,
                    round, context.CritChance, context.CritMultiplier, context.Random);
                if (target.TakeAttack(firstWeaponDamage, ref round))
                {
                    return true;
                }
            }

            round++;

            if (!target.WillTakeAttack())
            {
                return false;
            }

            var secondWeaponDamage = FightContext.CalculateNormalHitDamage(context.SecondMinimumDamage, context.SecondMaximumDamage,
                round, context.CritChance, context.CritMultiplier, context.Random);

            return target.TakeAttack(secondWeaponDamage, ref round);
        }
    }

    private static class Bard
    {
        private static readonly (int duration, double dmgBonus)[] MelodiesEffects =
        [
            (3, 1.2),
            (3, 1.4),
            (4, 1.6),
        ];

        public static bool Attack(ref FightContext context, ref FightContext target, ref int round)
        {
            round++;

            if (context.MelodyLength == 0)
            {
                context.MelodyDmgMultiplier = 1;
                context.MelodyCritMultiplier = context.CritMultiplier;
            }
            if (context.MelodyLength <= 0 && context.NextMelodyRound <= 0)
            {
                AssignMelodies(ref context);
            }
            context.MelodyLength--;
            context.NextMelodyRound--;

            if (!target.WillTakeAttack())
                return false;

            var dmg = FightContext.CalculateNormalHitDamage(context.MinimumDamage, context.MaximumDamage,
                round, context.CritChance, context.MelodyCritMultiplier, context.Random) * context.MelodyDmgMultiplier;

            return target.TakeAttack(dmg, ref round);
        }

        private static void AssignMelodies(ref FightContext context)
        {
            var rng = context.Random.NextDouble();
            var melody = 2;
            if (rng < 0.50)
                melody = 1;
            else if (rng < 0.75)
                melody = 0;

            (context.MelodyLength, context.MelodyDmgMultiplier) = MelodiesEffects[melody];
            context.NextMelodyRound = 4;
            // BUG: this is current Bard bug, there should be no crit multiplier changes
            context.MelodyCritMultiplier = (context.CritMultiplier + context.MelodyDmgMultiplier) / context.MelodyDmgMultiplier;
        }
    }

    private static class Berserker
    {
        public static bool WillSkipRound(ref FightContext context, ref int round)
        {
            if (context.OpponentClass == ClassType.Mage)
            {
                return false;
            }

            if (context.ChainAttackCounter == 14)
            {
                context.ChainAttackCounter = 0;
            }
            else if (context.Random.Next(1, 101) > 50)
            {
                round++;
                context.ChainAttackCounter++;
                return true;
            }
            else
            {
                context.ChainAttackCounter = 0;
            }

            return false;
        }
    }

    private static class BloodWeaver
    {
        public static bool Attack(ref FightContext context, ref FightContext target, ref int round)
        {
            round++;

            var dmgMultiplier = 1D;
            if (context.ReviveCount > 0 && context.Random.Next(1, 101) > 50)
            {
                dmgMultiplier = context.ScytheDmgMultiplier;
            }
            else if (context.ReviveCount == 0 && context.Random.Next(1, 101) <= 30)
            {
                if (target.WillTakeAttack())
                {
                    var normalHitDmg = FightContext.CalculateNormalHitDamage(context.MinimumDamage, context.MaximumDamage, round, context.CritChance, context.CritMultiplier, context.Random);
                    if (target.TakeAttack(normalHitDmg, ref round))
                        return true;
                }

                round++;

                context.TakeAttack(context.Health * 0.1D, ref round);
                dmgMultiplier = context.DaggerDmgMultiplier;
                context.BloodPactStacks = Math.Min(3, context.BloodPactStacks + 1);
            }

            if (!target.WillTakeAttack())
            {
                return false;
            }

            var dmg = FightContext.CalculateNormalHitDamage(context.MinimumDamage, context.MaximumDamage, round, context.CritChance, context.CritMultiplier, context.Random) * dmgMultiplier;

            return target.TakeAttack(dmg, ref round);
        }

        public static bool TakeAttack(ref FightContext context, double damage, ref int round)
        {
            context.Health -= damage;
            if (context.Health <= 0 && context.ReviveCount == 0)
            {
                context.ReviveCount++;
                context.BlockChance = 30;
                context.Health = context.MaxHealth * (0.4D + (context.BloodPactStacks * 0.1D));
                round++;
            }

            return context.Health <= 0;
        }
    }

    private static class DemonHunter
    {
        public static bool TakeAttack(ref FightContext context, double damage, ref int round)
        {
            context.Health -= damage;

            if (context.Health <= 0)
            {
                Revive(ref context, ref round);
            }

            return context.Health <= 0;
        }

        private static void Revive(ref FightContext context, ref int round)
        {
            var currentReviveChance = 0.44 - context.ReviveCount * 0.11;
            if (currentReviveChance <= 0 || context.Random.NextDouble() >= currentReviveChance) {
                return;
            }

            context.Health = context.MaxHealth * (0.9 - context.ReviveCount * 0.1);

            round++;
            context.ReviveCount++;
        }
    }

    private static class Druid
    {
        public static bool Attack(ref FightContext context, ref FightContext target, ref int round) => context.NextDruidAttack switch
        {
            DruidAttackMode.Bear => AttackInBearForm(ref context, ref target, ref round),
            DruidAttackMode.Mage => context.AttackWithoutAvoidance(ref target, ref round),
            _ => AttackNormally(ref context, ref target, ref round)
        };

        private static bool AttackInBearForm(ref FightContext context, ref FightContext target, ref int round)
        {
            context.IsInBearForm = true;
            context.NextDruidAttack = DruidAttackMode.Normal;
            round++;

            if (!target.WillTakeAttack())
            {
                return false;
            }

            var critMultiplier = (2 + 4D) * context.CritMultiplier / 2;
            var dmg = FightContext.CalculateNormalHitDamage(context.MinimumDamage, context.MaximumDamage,
                round, context.RageCritChance, critMultiplier, context.Random);

            return target.TakeAttack(dmg, ref round);
        }

        private static bool AttackNormally(ref FightContext context, ref FightContext target, ref int round)
        {
            context.IsInBearForm = false;

            if (context.Random.NextDouble() < context.SwoopChance)
            {
                round++;

                context.SwoopChance = Math.Min(0.5, context.SwoopChance + 0.05);
                if (target.WillTakeAttack())
                {
                    var isCrit = context.Random.NextDouble() < context.CritChance;

                    // BUG: SwoopMultiplier is bugged in game and calculates as follows:
                    var swoopMultiplier = isCrit ? (context.CritMultiplier + context.SwoopDamageMultiplier - 1) / context.CritMultiplier : context.SwoopDamageMultiplier;
                    var swoopDmg = FightContext.CalculateNormalHitDamage(context.MinimumDamage, context.MaximumDamage,
                        round, isCrit ? 1 : -1, context.CritMultiplier, context.Random) * swoopMultiplier;

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

            var dmg = FightContext.CalculateNormalHitDamage(context.MinimumDamage, context.MaximumDamage,
                round, context.CritChance, context.CritMultiplier, context.Random);

            return target.TakeAttack(dmg, ref round);
        }

        public static bool WillTakeAttack(ref FightContext context)
        {
            if (!context.IsInBearForm && context.Random.Next(1, 101) <= 35)
            {
                context.NextDruidAttack = DruidAttackMode.Bear;
                return false;
            }

            return true;
        }
    }

    private static class Necromancer
    {
        public static bool Attack(ref FightContext context, ref FightContext target, ref int round)
        {
            round++;

            if (context.MinionType == NecromancerMinionType.None && context.Random.Next(1, 101) <= 50)
            {
                SummonMinion(ref context);
                return AttackWithMinion(ref context, ref target, ref round);
            }

            if (target.WillTakeAttack())
            {
                var dmg = FightContext.CalculateNormalHitDamage(context.MinimumDamage, context.MaximumDamage,
                    round, context.CritChance, context.CritMultiplier, context.Random);
                if (target.TakeAttack(dmg, ref round))
                {
                    return true;
                }
            }

            return AttackWithMinion(ref context, ref target, ref round);
        }

        public static bool WillTakeAttack(ref FightContext context)
        {
            if (context.MinionType != NecromancerMinionType.Golem)
            {
                return true;
            }

            return context.Random.Next(1, 101) > 25;
        }

        private static bool AttackWithMinion(ref FightContext context, ref FightContext target, ref int round)
        {
            if (context.MinionType == NecromancerMinionType.None)
            {
                return false;
            }

            round++;

            context.MinionRounds--;
            var currentMinion = context.MinionType;
            // NOTE: Currently skeleton can revive only once per fight but this is a bug
            if (context.MinionRounds == 0 && context.MinionType == NecromancerMinionType.Skeleton &&
                context.SkeletonRevivesCount < 1 && context.Random.Next(1, 101) > 50)
            {
                context.MinionRounds = 1;
                context.SkeletonRevivesCount++;
            }
            else if (context.MinionRounds == 0)
            {
                context.MinionType = NecromancerMinionType.None;
                context.SkeletonRevivesCount = 0;
            }

            if (!target.WillTakeAttack())
            {
                return false;
            }

            var critChance = currentMinion == NecromancerMinionType.Hound ? Math.Min(0.6, context.CritChance + 0.1) : context.CritChance;
            var critMultiplier = currentMinion == NecromancerMinionType.Hound ? 2.5 * context.CritMultiplier / 2 : context.CritMultiplier;
            var dmg = FightContext.CalculateNormalHitDamage(context.MinimumDamage, context.MaximumDamage,
                round, critChance, critMultiplier, context.Random);
            dmg *= GetMinionDmgMultiplier(ref context, currentMinion);

            return target.TakeAttack(dmg, ref round);
        }

        private static void SummonMinion(ref FightContext context)
        {
            var minionTypeChance = context.Random.Next(1, 4);

            context.MinionType = minionTypeChance switch
            {
                1 => NecromancerMinionType.Skeleton,
                2 => NecromancerMinionType.Hound,
                3 => NecromancerMinionType.Golem,
                _ => throw new ArgumentOutOfRangeException(nameof(minionTypeChance), "Invalid minion type chance generated.")
            };

            context.MinionRounds = context.MinionType switch
            {
                NecromancerMinionType.Skeleton => 3,
                NecromancerMinionType.Hound => 2,
                NecromancerMinionType.Golem => 4,
                _ => throw new ArgumentOutOfRangeException(nameof(context.MinionType), "Invalid minion type.")
            };
        }

        private static double GetMinionDmgMultiplier(ref FightContext context, NecromancerMinionType minionType)
        {
            return minionType switch
            {
                NecromancerMinionType.Skeleton => (context.BaseDamageMultiplier + 0.25) / context.BaseDamageMultiplier,
                NecromancerMinionType.Hound => (context.BaseDamageMultiplier + 1) / context.BaseDamageMultiplier,
                NecromancerMinionType.Golem => 1,
                _ => throw new ArgumentOutOfRangeException(nameof(minionType), "Invalid minion type.")
            };
        }
    }

    private static class Paladin
    {
        public static bool Attack(ref FightContext context, ref FightContext target, ref int round)
        {
            round++;

            ChangeStance(ref context);

            if (!target.WillTakeAttack())
            {
                return false;
            }

            var dmg = FightContext.CalculateNormalHitDamage(context.MinimumDamage, context.MaximumDamage,
                round, context.CritChance, context.CritMultiplier, context.Random) * GetDamageMultiplier(ref context);

            return target.TakeAttack(dmg, ref round);
        }

        public static bool TakeAttack(ref FightContext context, double damage, ref int round)
        {
            var actualDamage = damage * GetArmorReduction(ref context);

            if (context.Stance == PaladinStanceType.Defensive && context.Random.Next(1, 101) <= GetBlockChance(ref context))
            {
                var health = actualDamage * 0.3;
                context.Health += Math.Min(Math.Max(0, context.MaxHealth - context.Health), health);

                return false;
            }

            context.Health -= actualDamage;
            return context.Health <= 0;
        }

        public static bool TakeAttackWithoutBlock(ref FightContext context, double damage, ref int round)
        {
            var actualDamage = damage * GetArmorReduction(ref context);
            context.Health -= actualDamage;
            return context.Health <= 0;
        }

        public static bool WillTakeAttack(ref FightContext context)
        {
            return context.Stance == PaladinStanceType.Defensive || context.Random.Next(1, 101) > GetBlockChance(ref context);
        }

        private static void ChangeStance(ref FightContext context)
        {
            var shouldChangeStance = context.Random.Next(1, 101) <= 50;

            if (!shouldChangeStance)
            {
                return;
            }

            context.Stance = context.Stance switch
            {
                PaladinStanceType.Initial => PaladinStanceType.Defensive,
                PaladinStanceType.Defensive => PaladinStanceType.Offensive,
                PaladinStanceType.Offensive => PaladinStanceType.Initial,
                _ => throw new NotSupportedException($"Unknown stance: {context.Stance}"),
            };
        }

        private static double GetArmorReduction(ref FightContext context) => context.Stance switch
        {
            PaladinStanceType.Initial or PaladinStanceType.Defensive => 1,
            PaladinStanceType.Offensive => 1 / (1 - context.InitialArmorReduction) * (1 - Math.Min(0.20, context.InitialArmorReduction)),
            _ => throw new NotSupportedException($"Unknown stance: {context.Stance}"),
        };

        private static double GetDamageMultiplier(ref FightContext context) => context.Stance switch
        {
            PaladinStanceType.Initial => 1,
            PaladinStanceType.Defensive => (1 / 0.833) * 0.568,
            PaladinStanceType.Offensive => (1 / 0.833) * 1.253,
            _ => throw new NotSupportedException($"Unknown stance: {context.Stance}"),
        };

        private static int GetBlockChance(ref FightContext context) => context.Stance switch
        {
            PaladinStanceType.Initial => 30,
            PaladinStanceType.Defensive => 50,
            PaladinStanceType.Offensive => 25,
            _ => throw new NotSupportedException($"Unknown stance: {context.Stance}"),
        };
    }

    private static class PlagueDoctor
    {
        public static bool Attack(ref FightContext context, ref FightContext target, ref int round)
        {
            if (context.PoisonRound <= 0 && context.Random.Next(1, 101) > 50)
            {
                round++;
                context.CurrentPoisonMultiplier = context.PoisonDmgMultipliers![2];
                if (!target.WillTakeAttack())
                {
                    return false;
                }

                context.PoisonRound = 3;

                var tinctureThrowDmg = FightContext.CalculateNormalHitDamage(context.MinimumDamage * context.CurrentPoisonMultiplier,
                    context.MaximumDamage * context.CurrentPoisonMultiplier, round, context.CritChance, context.CritMultiplier, context.Random);

                // NOTE: dirty trick to make Paladin vs Plague Doctor work.
                // In general I think this is okay to keep right now since refactoring is expensive and
                // usually each new class has some unique random shit never seen before that will break it again
                if (target.Class == ClassType.Paladin)
                {
                    var hpBeforeAttack = target.Health;
                    var result = target.TakeAttack(tinctureThrowDmg, ref round);
                    context.PoisonRound = hpBeforeAttack > target.Health ? 3 : 0;
                    return result;
                }
                return target.TakeAttack(tinctureThrowDmg, ref round);
            }

            if (context.PoisonRound > 0)
            {
                var result = PoisonAttack(ref context, ref target, ref round);
                if (context.PoisonRound == 0)
                    context.CurrentPoisonMultiplier = 1;

                return result;
            }
            else
            {
                round++;
                if (!target.WillTakeAttack())
                {
                    return false;
                }

                // BUG: We use context.CurrentPoisonMultiplier here becuase PD has a bug - if the Tincture Throw is blocked or enemy revived after the poison hit
                // PD stays on the last poison/tincture multiplier
                var dmg = FightContext.CalculateNormalHitDamage(context.MinimumDamage * context.CurrentPoisonMultiplier, context.MaximumDamage * context.CurrentPoisonMultiplier,
                    round, context.CritChance, context.CritMultiplier, context.Random);

                return target.TakeAttack(dmg, ref round);
            }
        }

        private static bool PoisonAttack(ref FightContext context, ref FightContext target, ref int round)
        {
            round++;
            context.PoisonRound--;
            context.CurrentPoisonMultiplier = context.PoisonDmgMultipliers![context.PoisonRound];
            var poisonDmg = FightContext.CalculateNormalHitDamage(context.MinimumDamage * context.CurrentPoisonMultiplier,
            context.MaximumDamage * context.CurrentPoisonMultiplier, round, context.CritChance, context.CritMultiplier, context.Random);

            // NOTE: dirty trick to make Paladin vs Plague Doctor work
            if (target.Class == ClassType.Paladin)
            {
                context.CurrentPoisonMultiplier = 1;
                if (Paladin.TakeAttackWithoutBlock(ref target, poisonDmg, ref round))
                {
                    return true;
                }
            }
            // BUG: follow in-game revive bug that makes PD do poison dmg on normal hit after revive
            else
            {
                var revivesBefore = target.ReviveCount;
                var result = target.TakeAttack(poisonDmg, ref round);
                if (revivesBefore == target.ReviveCount)
                {
                    context.CurrentPoisonMultiplier = 1;
                }

                if (result) return true;
            }

            round++;
            if (!target.WillTakeAttack())
            {
                return false;
            }

            // BUG: We use context.CurrentPoisonMultiplier here becuase PD has a bug - if the Tincture Throw is blocked or enemy revived after the poison hit
            // PD stays on the last poison/tincture multiplier
            var dmg = FightContext.CalculateNormalHitDamage(context.MinimumDamage * context.CurrentPoisonMultiplier, context.MaximumDamage * context.CurrentPoisonMultiplier,
                round, context.CritChance, context.CritMultiplier, context.Random);

            return target.TakeAttack(dmg, ref round);
        }

        public static bool WillTakeAttack(ref FightContext context)
        {
            var evadeChance = context.PoisonRound switch
            {
                3 => 65,
                2 => 50,
                1 => 35,
                _ => 20
            };
            return context.Random.Next(1, 101) > evadeChance;
        }

        public static double[] GetPoisonMultipliers(double baseDamageMultiplier)
        {
            var dmgMultiplier = ClassConfigurationProvider.Get(ClassType.PlagueDoctor).DamageMultiplier;
            var classSpecificDmgMultiplier = baseDamageMultiplier / dmgMultiplier;

            return
            [
                ((baseDamageMultiplier / classSpecificDmgMultiplier - 0.9) * classSpecificDmgMultiplier) /
                baseDamageMultiplier,
                ((baseDamageMultiplier / classSpecificDmgMultiplier - 0.55) * classSpecificDmgMultiplier) /
                baseDamageMultiplier,
                ((baseDamageMultiplier / classSpecificDmgMultiplier - 0.2) * classSpecificDmgMultiplier) /
                baseDamageMultiplier
            ];
        }
    }
}