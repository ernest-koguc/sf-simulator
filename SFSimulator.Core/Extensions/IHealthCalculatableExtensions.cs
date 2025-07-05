namespace SFSimulator.Core;

public static class IHealthCalculatableExtensions
{
    public static long GetHealth(this IHealthCalculatable healthCalculatable)
    {
        var healthMultiplier = ClassConfigurationProvider.Get(healthCalculatable.Class).HealthMultiplier;
        var health = healthCalculatable.Constitution * (healthCalculatable.Level + 1D);

        health *= healthMultiplier;

        var portalBonus = 1 + (healthCalculatable.SoloPortal / 100);
        var runeBonus = 1 + (Math.Min(15, healthCalculatable.HealthRune) / 100D);

        health = Math.Ceiling(health * portalBonus);
        health = Math.Ceiling(health * runeBonus);
        if (healthCalculatable.HasEternityPotion)
        {
            health = Math.Ceiling(health * 1.25D);
        }

        return (long)health;
    }
}