namespace SFSimulator.Core;

public enum RuneType
{
    None = 0,
    GoldBonus = 1,
    EpicChance = 2,
    ItemQuality = 3,
    ExperienceBonus = 4,
    HealthBonus = 5,
    FireResistance = 6,
    ColdResistance = 7,
    LightningResistance = 8,
    TotalResistance = 9,
    FireDamage = 10,
    ColdDamage = 11,
    LightningDamage = 12
}

public static class RuneTypeExtensions
{
    public static bool IsWeaponRune(this RuneType rune)
        => rune is not RuneType.None or RuneType.LightningDamage or RuneType.FireDamage or RuneType.ColdDamage;
}