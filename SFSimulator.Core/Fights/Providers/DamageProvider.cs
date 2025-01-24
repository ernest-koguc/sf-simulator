namespace SFSimulator.Core;

public class DamageProvider : IDamageProvider
{
    public (double Minimum, double Maximum) CalculateDamage<T, E>(IWeaponable? weapon, IFightable<T> attacker, IFightable<E> target, bool isSecondWeapon = false) where T : IWeaponable where E : IWeaponable
    {
        var damage = GetBaseDmg(weapon, attacker, isSecondWeapon);

        damage.Minimum *= 1 + (attacker.GuildPortal / 100);
        damage.Maximum *= 1 + (attacker.GuildPortal / 100);

        ProcAttributesBonus(attacker, target, ref damage);

        ProcRuneBonus(weapon, target, ref damage);

        ProcArmorDamageReduction(attacker, target, ref damage);

        ProcClassModifiers(attacker, ref damage);

        return (damage.Minimum, damage.Maximum);
    }

    private static void ProcClassModifiers<T>(IFightable<T> attacker, ref (double Minimum, double Maximum) damage) where T : IWeaponable
    {
        if (attacker.Class == ClassType.Berserker)
        {
            damage.Minimum *= 1.25D;
            damage.Maximum *= 1.25D;
        }
        if (attacker.Class == ClassType.Bard)
        {
            damage.Minimum *= 1.125D;
            damage.Maximum *= 1.125D;
        }
        if (attacker.Class == ClassType.Assassin)
        {
            damage.Minimum *= 0.625D;
            damage.Maximum *= 0.625D;
        }
        if (attacker.Class == ClassType.Druid)
        {
            damage.Minimum *= 1 / 3D;
            damage.Maximum *= 1 / 3D;
        }
        if (attacker.Class == ClassType.Necromancer)
        {
            damage.Minimum *= 0.56D;
            damage.Maximum *= 0.56D;
        }
    }

    private static void ProcArmorDamageReduction<T, E>(IFightable<T> attacker, IFightable<E> target, ref (double Minimum, double Maximum) damage) where T : IWeaponable where E : IWeaponable
    {
        if (attacker.Class == ClassType.Mage)
            return;

        var damageReduction = 0D;

        if (target.Armor > 0)
            damageReduction = (double)target.Armor / attacker.Level / 100D;

        switch (target.Class)
        {
            case ClassType.Mage:
                damageReduction = Math.Min(damageReduction, 0.1D);
                break;
            case ClassType.Bard:
                damageReduction *= 2;
                damageReduction = Math.Min(damageReduction, 0.5D);
                break;
            case ClassType.Warrior:
            case ClassType.DemonHunter:
            case ClassType.Bert:
                damageReduction = Math.Min(damageReduction, 0.5D);
                break;
            case ClassType.BattleMage:
                damageReduction *= 5;
                damageReduction = Math.Min(damageReduction, 0.5D);
                break;
            case ClassType.Scout:
            case ClassType.Assassin:
            case ClassType.Berserker:
                damageReduction = Math.Min(damageReduction, 0.25D);
                break;
            case ClassType.Druid:
                damageReduction = Math.Min(damageReduction, 0.2D);
                damageReduction *= 2;
                break;
            case ClassType.Necromancer:
                damageReduction *= 2;
                damageReduction = Math.Min(damageReduction, 0.2D);
                break;
            default:
                throw new ArgumentOutOfRangeException(nameof(target.Class));
        }

        damage.Minimum *= 1 - damageReduction;
        damage.Maximum *= 1 - damageReduction;
    }

    private static void ProcRuneBonus<T>(IWeaponable? weapon, IFightable<T> target, ref (double Minimum, double Maximum) damage) where T : IWeaponable
    {
        if (weapon == null || weapon.RuneType == RuneType.None)
            return;

        var enemyRuneResistance = weapon.RuneType switch
        {
            RuneType.LightningDamage => Math.Min(75, target.LightningResistance),
            RuneType.ColdDamage => Math.Min(75, target.ColdResistance),
            RuneType.FireDamage => Math.Min(75, target.FireResistance),
            _ => throw new ArgumentOutOfRangeException(nameof(weapon.RuneType)),
        };
        var runeBonus = weapon.RuneValue / 100D;
        runeBonus *= (100 - enemyRuneResistance) / 100D;
        runeBonus++;

        damage.Minimum *= runeBonus;
        damage.Maximum *= runeBonus;
    }

    private static void ProcAttributesBonus<T, E>(IFightable<T> attacker, IFightable<E> target, ref (double Minimum, double Maximum) damage) where T : IWeaponable where E : IWeaponable
    {
        var attribute = attacker.Class switch
        {
            ClassType.Mage or ClassType.Bard or ClassType.Druid or ClassType.Necromancer
                => Math.Max(attacker.Intelligence / 2D, attacker.Intelligence - target.Intelligence / 2D),
            ClassType.Scout or ClassType.Assassin or ClassType.DemonHunter
                => Math.Max(attacker.Dexterity / 2D, attacker.Dexterity - target.Dexterity / 2D),
            ClassType.Warrior or ClassType.BattleMage or ClassType.Berserker or ClassType.Bert
                => (double)Math.Max(attacker.Strength / 2D, attacker.Strength - target.Strength / 2D),

            _ => throw new ArgumentOutOfRangeException(nameof(attacker.Class)),
        };
        var attributeBonus = 1 + attribute / 10D;

        damage.Minimum *= attributeBonus;
        damage.Maximum *= attributeBonus;
    }

    private static (double Minimum, double Maximum) GetBaseDmg<T>(IWeaponable? weapon, IFightable<T> attacker, bool isSecondWeapon) where T : IWeaponable
    {
        var handDamage = GetHandDamage(attacker, isSecondWeapon);

        if (weapon is null || weapon.MinDmg < handDamage.Minimum)
        {
            return handDamage;
        }

        return (weapon.MinDmg, weapon.MaxDmg);
    }

    private static (double Minimum, double Maximum) GetHandDamage<T>(IFightable<T> attacker, bool isSecondWeapon) where T : IWeaponable
    {
        if (attacker.Level < 10)
            return (1, 2);

        var weaponMultiplier = ClassConfigurationProvider.GetClassConfiguration(attacker.Class).WeaponMultiplier;

        var damage = (attacker.Level - 9) * weaponMultiplier / 30 * 2;
        var min = Math.Ceiling(damage);
        var max = Math.Ceiling(damage * 2);

        if (isSecondWeapon)
        {
            min *= 7;
            max *= 7;
        }

        return (min, max);
    }

    public double CalculateFireBallDamage<T, E>(IFightable<T> main, IFightable<E> opponent) where T : IWeaponable where E : IWeaponable
    {
        var multiplier = opponent.Class switch
        {
            ClassType.Warrior or ClassType.Bert or ClassType.Druid or ClassType.BattleMage => 5,
            ClassType.Bard => 2,
            ClassType.Mage => 0,
            ClassType.Scout or ClassType.Assassin or ClassType.Berserker or ClassType.DemonHunter => 4,
            _ => throw new ArgumentOutOfRangeException(nameof(opponent.Class)),
        };
        var dmg = Math.Ceiling(multiplier * 0.05D * main.Health);
        return Math.Min(Math.Ceiling(opponent.Health / 3D), dmg);
    }
}