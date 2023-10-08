namespace SFSimulator.Core;

public static class DungeonableDefaultImplementation
{
    public static double CalculateNormalHitDamage(double minDmg, double maxDmg, int round, double critChance, double critMultiplier, Random random)
    {
        var dmg = CalculateNormalHitNoCritDamage(minDmg, maxDmg, round, random);

        var isCrit = random.NextDouble() < critChance;
        if (isCrit)
            dmg *= critMultiplier;

        return dmg;
    }
    public static double CalculateNormalHitNoCritDamage(double minDmg, double maxDmg, int round, Random random)
    {
        var baseDamage = random.NextDouble() * (1 + maxDmg - minDmg) + minDmg;
        var dmg = baseDamage;
        var enrage = 1 + (round - 1) / 6D;
        dmg *= enrage;

        return dmg;
    }
}
