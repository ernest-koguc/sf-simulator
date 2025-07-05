using System.ComponentModel;
using System.Text.Json.Serialization;

namespace SFSimulator.Core;

public class Companion : IFightable<EquipmentItem>, IHealthCalculatable, ITotalStatsCalculatable
{
    [JsonIgnore]
    public SimulationContext Character { get; set; } = null!;

    public int Level => Character.Level;
    public ClassType Class { get; set; }
    public int Armor => Items.Sum(i => i.Armor);

    public bool IsCompanion => true;

    public int BaseStrength => CompanionMappings.MapCompanionAttribute(AttributeType.Strength, Character, Class);
    public int BaseDexterity => CompanionMappings.MapCompanionAttribute(AttributeType.Dexterity, Character, Class);

    public int BaseIntelligence => CompanionMappings.MapCompanionAttribute(AttributeType.Intelligence, Character, Class);
    public int BaseConstitution => CompanionMappings.MapCompanionAttribute(AttributeType.Constitution, Character, Class);
    public int BaseLuck => CompanionMappings.MapCompanionAttribute(AttributeType.Luck, Character, Class);

    public int Strength => this.GetTotalStatsFor(AttributeType.Strength);
    public int Dexterity => this.GetTotalStatsFor(AttributeType.Dexterity);
    public int Intelligence => this.GetTotalStatsFor(AttributeType.Intelligence);
    public int Constitution => this.GetTotalStatsFor(AttributeType.Constitution);
    public int Luck => this.GetTotalStatsFor(AttributeType.Luck);

    public EquipmentItem? FirstWeapon => Items.FirstOrDefault(e => e.ItemType == ItemType.Weapon);
    public EquipmentItem? SecondWeapon => null;
    public List<EquipmentItem> Items { get; set; } = [];
    public List<Potion> Potions => Character.Potions;
    public PetsState Pets => Character.Pets;

    public int LightningResistance => Items.Where(i => i.RuneType is RuneType.LightningResistance or RuneType.TotalResistance).Sum(i => i.RuneValue);
    public int FireResistance => Items.Where(i => i.RuneType is RuneType.FireResistance or RuneType.TotalResistance).Sum(i => i.RuneValue);
    public int ColdResistance => Items.Where(i => i.RuneType is RuneType.ColdResistance or RuneType.TotalResistance).Sum(i => i.RuneValue);
    public int HealthRune => Items.Where(i => i.RuneType == RuneType.HealthBonus).Sum(i => i.RuneValue);

    public bool HasGlovesScroll => Items.Any(i => i.ScrollType == WitchScrollType.Reaction);
    public bool HasWeaponScroll => FirstWeapon?.HasEnchantment == true || SecondWeapon?.HasEnchantment == true;
    public bool HasEternityPotion => Potions.Any(p => p.Type == PotionType.Eternity);

    public double SoloPortal => Character.SoloPortal;
    public double GuildPortal => Character.GuildPortal;
    public double CritMultiplier => 2 + (0.11 * Character.GladiatorLevel) + (HasWeaponScroll ? 0.05 : 0);
    public int Reaction => Items.Any(i => i.ScrollType == WitchScrollType.Reaction) ? 1 : 0;
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