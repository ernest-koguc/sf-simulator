using System.ComponentModel;
using System.Text.Json.Serialization;

namespace SFSimulator.Core;

public class Companion : IFightable<EquipmentItem>, IHealthCalculatable, ITotalStatsCalculatable
{
    [JsonIgnore]
    public SimulationContext Character { get; set; } = null!;

    [JsonIgnore]
    public int Level => Character.Level;
    public ClassType Class { get; set; }
    [JsonIgnore]
    public int Armor => Items.Sum(i => i.Armor);
    [JsonIgnore]
    public bool IsCompanion => true;

    [JsonIgnore]
    public int BaseStrength => CompanionMappings.MapCompanionAttribute(AttributeType.Strength, Character, Class);
    [JsonIgnore]
    public int BaseDexterity => CompanionMappings.MapCompanionAttribute(AttributeType.Dexterity, Character, Class);
    [JsonIgnore]
    public int BaseIntelligence => CompanionMappings.MapCompanionAttribute(AttributeType.Intelligence, Character, Class);
    [JsonIgnore]
    public int BaseConstitution => CompanionMappings.MapCompanionAttribute(AttributeType.Constitution, Character, Class);
    [JsonIgnore]
    public int BaseLuck => CompanionMappings.MapCompanionAttribute(AttributeType.Luck, Character, Class);

    [JsonIgnore]
    public int Strength => this.GetTotalStatsFor(AttributeType.Strength);
    [JsonIgnore]
    public int Dexterity => this.GetTotalStatsFor(AttributeType.Dexterity);
    [JsonIgnore]
    public int Intelligence => this.GetTotalStatsFor(AttributeType.Intelligence);
    [JsonIgnore]
    public int Constitution => this.GetTotalStatsFor(AttributeType.Constitution);
    [JsonIgnore]
    public int Luck => this.GetTotalStatsFor(AttributeType.Luck);

    [JsonIgnore]
    public EquipmentItem? FirstWeapon => Items.FirstOrDefault(e => e.ItemType == ItemType.Weapon);
    [JsonIgnore]
    public EquipmentItem? SecondWeapon => null;
    public List<EquipmentItem> Items { get; set; } = [];
    [JsonIgnore]
    public List<Potion> Potions => Character.Potions;
    [JsonIgnore]
    public PetsState Pets => Character.Pets;

    [JsonIgnore]
    public int LightningResistance => Items.Where(i => i.RuneType is RuneType.LightningResistance or RuneType.TotalResistance).Sum(i => i.RuneValue);
    [JsonIgnore]
    public int FireResistance => Items.Where(i => i.RuneType is RuneType.FireResistance or RuneType.TotalResistance).Sum(i => i.RuneValue);
    [JsonIgnore]
    public int ColdResistance => Items.Where(i => i.RuneType is RuneType.ColdResistance or RuneType.TotalResistance).Sum(i => i.RuneValue);
    [JsonIgnore]
    public int HealthRune => Items.Where(i => i.RuneType == RuneType.HealthBonus).Sum(i => i.RuneValue);

    [JsonIgnore]
    public bool HasGlovesScroll => Items.Any(i => i.ScrollType == WitchScrollType.Reaction);
    [JsonIgnore]
    public bool HasWeaponScroll => FirstWeapon?.HasEnchantment == true || SecondWeapon?.HasEnchantment == true;
    [JsonIgnore]
    public bool HasEternityPotion => Potions.Any(p => p.Type == PotionType.Eternity);

    [JsonIgnore]
    public double SoloPortal => Character.SoloPortal;
    [JsonIgnore]
    public double GuildPortal => Character.GuildPortal;
    [JsonIgnore]
    public double CritMultiplier => 2 + (0.11 * Character.GladiatorLevel) + (HasWeaponScroll ? 0.05 : 0);
    [JsonIgnore]
    public int Reaction => Items.Any(i => i.ScrollType == WitchScrollType.Reaction) ? 1 : 0;
    [JsonIgnore]
    public long Health => this.GetHealth();
}

public static class CompanionMappings
{
    public static int MapCompanionAttribute(AttributeType attributeType, SimulationContext simulationContext, ClassType companionClass)
    {
        var characterMainAttribute = ClassConfigurationProvider.Get(simulationContext.Class).MainAttribute;

        var companionMainAttribute = ClassConfigurationProvider.Get(companionClass).MainAttribute;

        return attributeType == companionMainAttribute
            ? simulationContext.GetBaseAttributesOf(characterMainAttribute)
            : attributeType != characterMainAttribute
            ? simulationContext.GetBaseAttributesOf(attributeType)
            : simulationContext.GetBaseAttributesOf(companionMainAttribute);
    }

    private static int GetBaseAttributesOf(this SimulationContext simulationContext, AttributeType attributeType)
    {
        return attributeType switch
        {
            AttributeType.Strength => simulationContext.BaseStrength,
            AttributeType.Dexterity => simulationContext.BaseDexterity,
            AttributeType.Intelligence => simulationContext.BaseIntelligence,
            AttributeType.Constitution => simulationContext.BaseConstitution,
            AttributeType.Luck => simulationContext.BaseLuck,
            _ => throw new InvalidEnumArgumentException(nameof(attributeType))
        };
    }
}