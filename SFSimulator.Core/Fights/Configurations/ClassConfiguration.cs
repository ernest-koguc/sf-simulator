namespace SFSimulator.Core;

public sealed class ClassConfiguration
{
    /// <summary>
    /// Property <c>WeaponMultiplier</c> specifies a multiplier for weapon base damage calculation.
    /// </summary>
    public required double WeaponMultiplier { get; init; }
    ///
    /// <summary>
    /// Property <c>WeaponGemMultiplier</c> specifices a gem multiplier used to calculate gem value for weapon.
    /// </summary>
    public required int WeaponGemMultiplier { get; init; }

    /// <summary>
    /// Property <c>WeaponAttributeMultiplier</c> specifies weapon attribute multiplier used for calculating weapon attributes.
    /// </summary>
    public required int WeaponAttributeMultiplier { get; init; }

    /// <summary>
    /// Property <c>HealthMultiplier</c> specifies a multiplier for total health calculation.
    /// </summary>
    public required double HealthMultiplier { get; init; }

    /// <summary>
    /// Property <c>ItemArmorMultiplier</c> specifies a multiplier used for calculating item's base armor.
    /// </summary>
    public required double ItemArmorMultiplier { get; init; }

    /// <summary>
    /// Property <c>MainAttribute</c> specifies class' main attribute, e.g. Strength, Dexterity.
    /// </summary>
    public required AttributeType MainAttribute { get; init; }

    /// <summary>
    /// Property <c>ItemBonusMultiplier</c> specifies a bonus multiplier for all items.
    /// </summary>
    public required double ItemBonusMultiplier { get; init; }

}