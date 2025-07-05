﻿namespace SFSimulator.Core;

public class DamageProvider : IDamageProvider
{
    public (double Minimum, double Maximum) CalculateDamage<T, E>(IWeaponable? weapon, IFightable<T> attacker, IFightable<E> target, bool isSecondWeapon = false)
        where T : IWeaponable where E : IWeaponable
    {
        var damage = GetBaseDmg(weapon, attacker, isSecondWeapon);

        damage.Minimum *= 1 + (attacker.GuildPortal / 100);
        damage.Maximum *= 1 + (attacker.GuildPortal / 100);

        ProcAttributesBonus(attacker, target, ref damage);

        ProcRuneBonus(weapon, target, ref damage);

        ProcArmorDamageReduction(attacker, target, ref damage);

        ProcClassModifiers(attacker, target, ref damage);

        return (damage.Minimum, damage.Maximum);
    }

    private void ProcClassModifiers<T, E>(IFightable<T> attacker, IFightable<E> target, ref (double Minimum, double Maximum) damage)
        where T : IWeaponable where E : IWeaponable
    {
        var dmgMultiplier = CalculateDamageMultiplier(attacker, target);
        damage.Minimum *= dmgMultiplier;
        damage.Maximum *= dmgMultiplier;
    }

    private void ProcArmorDamageReduction<T, E>(IFightable<T> attacker, IFightable<E> target, ref (double Minimum, double Maximum) damage) where T : IWeaponable where E : IWeaponable
    {
        var dmgReduction = CalculateDamageReduction(attacker, target);

        damage.Minimum *= 1 - dmgReduction;
        damage.Maximum *= 1 - dmgReduction;
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
            ClassType.Warrior or ClassType.BattleMage or ClassType.Berserker or ClassType.Paladin or ClassType.Bert
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

        if (weapon is null || (weapon.MinDmg < handDamage.Minimum && weapon.MaxDmg < handDamage.Maximum))
        {
            return handDamage;
        }

        return (weapon.MinDmg, weapon.MaxDmg);
    }

    private static (double Minimum, double Maximum) GetHandDamage<T>(IFightable<T> attacker, bool isSecondWeapon) where T : IWeaponable
    {
        if (attacker.Level <= 10)
            return (1, 2);

        var multiplier = 0.7;
        if (attacker.Class == ClassType.Assassin)
        {
            multiplier = isSecondWeapon ? 1.25 : 0.875;
        }

        var weaponMultiplier = ClassConfigurationProvider.Get(attacker.Class).WeaponMultiplier;

        var damage = multiplier * (attacker.Level - 9) * weaponMultiplier;
        var min = Math.Max(1, Math.Ceiling(damage * 2 / 3));
        var max = Math.Max(2, Math.Round(damage * 4 / 3));

        return (min, max);
    }

    public double CalculateFireBallDamage<T, E>(IFightable<T> main, IFightable<E> opponent) where T : IWeaponable where E : IWeaponable
    {
        var multiplier = opponent.Class switch
        {
            ClassType.Paladin => 6,
            ClassType.Warrior or ClassType.Bert or ClassType.Druid or ClassType.BattleMage => 5,
            ClassType.Bard => 2,
            ClassType.Mage => 0,
            ClassType.Scout or ClassType.Assassin or ClassType.Berserker or ClassType.DemonHunter or ClassType.Necromancer => 4,
            _ => throw new ArgumentOutOfRangeException(nameof(opponent.Class)),
        };
        var dmg = Math.Ceiling(multiplier * 0.05D * main.Health);
        return Math.Min(Math.Ceiling(opponent.Health / 3D), dmg);
    }

    public double CalculateDamageReduction<T, E>(IFightable<T> attacker, IFightable<E> target)
        where T : IWeaponable where E : IWeaponable
    {
        // Mage negates enemy armor
        if (attacker.Class == ClassType.Mage)
            return 0;

        if (target.Armor <= 0)
            return 0;

        var classConfig = ClassConfigurationProvider.Get(target.Class);
        var maxDmgReduction = classConfig.MaxArmorReduction;
        var damageReduction = 0D;

        damageReduction = classConfig.ArmorMultiplier * target.Armor / attacker.Level / 100D;

        return damageReduction = Math.Min(damageReduction, maxDmgReduction);
    }

    public double CalculateDamageMultiplier<T, E>(IFightable<T> attacker, IFightable<E> target)
        where T : IWeaponable where E : IWeaponable
        => (attacker.Class, target.Class) switch
        {
            (ClassType.Mage, ClassType.Paladin) => 1.5D,
            (ClassType.Assassin, _) => 0.625D,
            (ClassType.Berserker, _) => 1.25D,
            (ClassType.Druid, ClassType.Mage) => 1D / 3D * 4D / 3D,
            (ClassType.Druid, ClassType.DemonHunter) => 1D / 3D * 1.15D,
            (ClassType.Druid, _) => 1D / 3D,
            (ClassType.Bard, _) => 1.125D,
            (ClassType.Necromancer, ClassType.DemonHunter) => ((5D / 9D) + 0.1D),
            (ClassType.Necromancer, _) => 5D / 9D,
            (ClassType.Paladin, ClassType.Mage) => 0.833D * 1.5D,
            (ClassType.Paladin, _) => 0.833D,
            (_, _) => 1D,
        };
}