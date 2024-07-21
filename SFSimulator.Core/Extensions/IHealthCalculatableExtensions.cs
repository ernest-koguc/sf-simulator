namespace SFSimulator.Core;

public static class IHealthCalculatableExtensions
{
    public static long GetHealth(this IHealthCalculatable healthCalculatable)
    {
        var healthMultiplier = ClassConfigurationProvider.GetClassConfiguration(healthCalculatable.Class).HealthMultiplier;

        var health = healthCalculatable.Constitution * (healthCalculatable.Level + 1D);
        health *= healthMultiplier;
        var portalBonus = 1 + healthCalculatable.SoloPortal;
        var runeBonus = 1 + (Math.Min(15, healthCalculatable.HealthRune) / 100D);
        health = health * portalBonus * runeBonus;

        if (healthCalculatable.HasEternityPotion)
            health *= 1.25D;

        return (long)health;
    }
}