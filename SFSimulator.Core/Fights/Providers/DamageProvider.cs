namespace SFSimulator.Core;

public class DamageProvider : IDamageProvider
{
    public (double Minimum, double Maximum) CalculateDamage(Weapon? weapon, IFightable attacker, IFightable target, bool isSecondWeapon = false)
    {
        var damage = GetBaseDmg(weapon, attacker, isSecondWeapon);

        damage.Minimum *= 1 + attacker.GuildPortal;
        damage.Maximum *= 1 + attacker.GuildPortal;

        ProcAttributesBonus(attacker, target, ref damage);

        ProcRuneBonus(weapon, target, ref damage);

        ProcArmorDamageReduction(attacker, target, ref damage);

        ProcClassModifiers(attacker, ref damage);

        return (damage.Minimum, damage.Maximum);
    }

    private static void ProcClassModifiers(IFightable attacker, ref (double Minimum, double Maximum) damage)
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
    }

    private static void ProcArmorDamageReduction(IFightable attacker, IFightable target, ref (double Minimum, double Maximum) damage)
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
            case ClassType.ShieldlessWarrior:
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
            default:
                throw new ArgumentOutOfRangeException(nameof(target.Class));
        }

        damage.Minimum *= 1 - damageReduction;
        damage.Maximum *= 1 - damageReduction;
    }

    private static void ProcRuneBonus(Weapon? weapon, IFightable target, ref (double Minimum, double Maximum) damage)
    {
        if (weapon == null || weapon.DamageRuneType == DamageRuneType.None)
            return;

        var runeBonus = 1 + weapon.RuneBonus / 100D;
        var enemyRuneResistance = weapon.DamageRuneType switch
        {
            DamageRuneType.Lightning => Math.Min(75, target.RuneResistance.LightningResistance),
            DamageRuneType.Cold => Math.Min(75, target.RuneResistance.ColdResistance),
            DamageRuneType.Fire => Math.Min(75, target.RuneResistance.FireResistance),
            _ => throw new ArgumentOutOfRangeException(nameof(weapon.DamageRuneType)),
        };
        runeBonus *= 1 - enemyRuneResistance / 100D;

        damage.Minimum *= runeBonus;
        damage.Maximum *= runeBonus;
    }

    private static void ProcAttributesBonus(IFightable attacker, IFightable target, ref (double Minimum, double Maximum) damage)
    {
        var attribute = attacker.Class switch
        {
            ClassType.Mage or ClassType.Bard or ClassType.Druid
                => Math.Max(attacker.Intelligence / 2, attacker.Intelligence - target.Intelligence / 2),
            ClassType.Scout or ClassType.Assassin or ClassType.DemonHunter
                => Math.Max(attacker.Dexterity / 2, attacker.Dexterity - target.Dexterity / 2),
            ClassType.Warrior or ClassType.BattleMage or ClassType.Berserker or ClassType.ShieldlessWarrior
                => (double)Math.Max(attacker.Strength / 2, attacker.Strength - target.Strength / 2),

            _ => throw new ArgumentOutOfRangeException(nameof(attacker.Class)),
        };
        var attributeBonus = 1 + attribute / 10;

        damage.Minimum *= attributeBonus;
        damage.Maximum *= attributeBonus;
    }

    private static (double Minimum, double Maximum) GetBaseDmg(Weapon? weapon, IFightable attacker, bool isSecondWeapon)
    {
        var handDamage = GetHandDamage(attacker, isSecondWeapon);

        if (weapon is null || weapon.MinDmg < handDamage.Minimum)
        {
            return handDamage;
        }

        return (weapon.MinDmg, weapon.MaxDmg);
    }

    private static (double Minimum, double Maximum) GetHandDamage(IFightable attacker, bool isSecondWeapon)
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

    public double CalculateFireBallDamage(IFightable main, IFightable opponent)
    {
        var multiplier = opponent.Class switch
        {
            ClassType.Warrior or ClassType.ShieldlessWarrior or ClassType.Druid or ClassType.BattleMage => 5,
            ClassType.Bard => 2,
            ClassType.Mage => 0,
            ClassType.Scout or ClassType.Assassin or ClassType.Berserker or ClassType.DemonHunter => 4,
            _ => throw new ArgumentOutOfRangeException(nameof(opponent.Class)),
        };
        var dmg = Math.Ceiling(multiplier * 0.05D * main.Health);
        return Math.Min(Math.Ceiling(opponent.Health / 3D), dmg);
    }
}
