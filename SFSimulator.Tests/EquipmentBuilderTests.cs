using Microsoft.VisualStudio.TestTools.UnitTesting;
using SFSimulator.Core;
using System;
using System.Linq;

namespace SFSimulator.Tests;

[TestClass]
public class EquipmentBuilderTests
{
    [TestMethod]
    // IQ <= 350
    [DataRow(ItemAttributeType.NormalOneStat, 200, ClassType.Warrior, 10, 0, ItemType.Boots, 426)] // IQ = 213
    [DataRow(ItemAttributeType.NormalTwoStats, 160, ClassType.Warrior, 0, 0, ItemType.Amulet, 163)] // IQ = 163
    [DataRow(ItemAttributeType.Epic, 200, ClassType.Scout, 84, 0, ItemType.Headgear, 344)] // IQ = 287
    [DataRow(ItemAttributeType.EpicAllAttributes, 200, ClassType.Mage, 17, 5, ItemType.Breastplate, 231)] // IQ = 231
    [DataRow(ItemAttributeType.Legendary, 100, ClassType.Assassin, 1, 0, ItemType.Gloves, 134)] // IQ = 104
    // IQ >= 350
    [DataRow(ItemAttributeType.NormalOneStat, 800, ClassType.Druid, 71, 4, ItemType.Belt, 2998)] // IQ = 909
    [DataRow(ItemAttributeType.NormalTwoStats, 500, ClassType.DemonHunter, 100, 0, ItemType.Ring, 1056)] // IQ = 603
    [DataRow(ItemAttributeType.Epic, 250, ClassType.Bard, 151, 0, ItemType.Trinket, 708)] // IQ = 404
    [DataRow(ItemAttributeType.EpicAllAttributes, 700, ClassType.Paladin, 29, 0, ItemType.Shield, 1420)] // IQ = 732
    [DataRow(ItemAttributeType.Legendary, 400, ClassType.Berserker, 75, 0, ItemType.Weapon, 971)] // IQ = 478
    [DataRow(ItemAttributeType.Legendary, 300, ClassType.Necromancer, 47, 0, ItemType.Trinket, 626)] // IQ = 350
    public void EquipmentBuilder_gives_correct_attributes(ItemAttributeType attrType, int characterLevel, ClassType classType, int aura,
            int itemQualityRune, ItemType itemType, int expectedStat)
    {
        var builder = new EquipmentBuilder(attrType, characterLevel, classType, aura, itemQualityRune, itemType);
        builder.WithAttributes();
        var item = builder.Build();

        var mainAttr = ClassConfigurationProvider.Get(classType).MainAttribute;
        Assert.AreEqual(expectedStat, item[mainAttr]);
    }

    [TestMethod]
    [DataRow(ItemAttributeType.Legendary, AttributeType.Strength, AttributeType.Constitution, AttributeType.Luck)]
    [DataRow(ItemAttributeType.Epic, AttributeType.Strength, AttributeType.Constitution, AttributeType.Luck)]
    [DataRow(ItemAttributeType.EpicAllAttributes, AttributeType.Strength, AttributeType.Dexterity, AttributeType.Intelligence, AttributeType.Constitution, AttributeType.Luck)]
    [DataRow(ItemAttributeType.NormalTwoStats, AttributeType.Strength, AttributeType.Constitution)]
    [DataRow(ItemAttributeType.NormalOneStat, AttributeType.Strength)]
    public void EquipmentBuilder_items_have_proper_attribute_types_assigned(ItemAttributeType attrType, params AttributeType[] expectedAttributes)
    {
        var builder = new EquipmentBuilder(attrType, 500, ClassType.Warrior, 20, 0, ItemType.Headgear);
        var item = builder.WithAttributes().Build();

        var attrWithoutValues = Enum.GetValues<AttributeType>().Except(expectedAttributes).Select(attr => item[attr]);
        var attrWithValues = expectedAttributes.Select(attr => item[attr]);

        Assert.IsTrue(attrWithValues.All(v => v > 0));
        Assert.IsTrue(attrWithoutValues.All(v => v == 0));
    }

    [TestMethod]
    [DataRow(ItemType.Headgear, true)]
    [DataRow(ItemType.Breastplate, true)]
    [DataRow(ItemType.Gloves, true)]
    [DataRow(ItemType.Boots, true)]
    [DataRow(ItemType.Weapon, false)]
    [DataRow(ItemType.Shield, false)]
    [DataRow(ItemType.Amulet, false)]
    [DataRow(ItemType.Belt, true)]
    [DataRow(ItemType.Ring, false)]
    [DataRow(ItemType.Trinket, false)]
    public void EquipmentBuilder_provides_armor_for_proper_item_type(ItemType itemType, bool shouldHaveArmor)
    {
        var builder = new EquipmentBuilder(ItemAttributeType.Epic, 513, ClassType.BattleMage, 41, 5, itemType);
        var item = builder.WithArmor().Build();

        Assert.AreEqual(shouldHaveArmor, item.Armor > 0);
    }

    [TestMethod]
    [DataRow(ItemAttributeType.NormalOneStat, 800, ClassType.Warrior, 30, 5, ItemType.Headgear, 13125)] // IQ = 875
    [DataRow(ItemAttributeType.NormalTwoStats, 400, ClassType.Mage, 40, 0, ItemType.Breastplate, 1329)] // IQ = 443
    [DataRow(ItemAttributeType.Epic, 700, ClassType.Scout, 59, 0, ItemType.Gloves, 5715)] // IQ = 762
    [DataRow(ItemAttributeType.EpicAllAttributes, 700, ClassType.Assassin, 53, 0, ItemType.Boots, 5670)] // IQ = 756
    [DataRow(ItemAttributeType.Epic, 500, ClassType.BattleMage, 72, 0, ItemType.Belt, 1725)] // IQ = 575
    [DataRow(ItemAttributeType.NormalTwoStats, 600, ClassType.Berserker, 47, 0, ItemType.Headgear, 9750)] // IQ = 650
    [DataRow(ItemAttributeType.Legendary, 800, ClassType.DemonHunter, 37, 0, ItemType.Breastplate, 12902)] // IQ = 840
    [DataRow(ItemAttributeType.Legendary, 500, ClassType.Druid, 0, 0, ItemType.Gloves, 3863)] // IQ = 503
    [DataRow(ItemAttributeType.Legendary, 400, ClassType.Bard, 47, 0, ItemType.Boots, 3456)] // IQ = 450
    [DataRow(ItemAttributeType.Epic, 400, ClassType.Necromancer, 7, 0, ItemType.Belt, 1230)] // IQ = 410
    [DataRow(ItemAttributeType.NormalOneStat, 397, ClassType.Paladin, 0, 0, ItemType.Headgear, 6000)] // IQ = 400
    public void EquipmentBuilder_provides_proper_armor_values(ItemAttributeType attrType, int characterLevel, ClassType classType, int aura,
            int itemQualityRune, ItemType itemType, int expectedArmor)
    {
        var builder = new EquipmentBuilder(attrType, characterLevel, classType, aura, itemQualityRune, itemType);
        builder.WithArmor();
        var item = builder.Build();

        Assert.AreEqual(expectedArmor, item.Armor);
    }

    [TestMethod]
    [DataRow(513, ClassType.Mage, 28, 1000, GemType.Legendary, 1301)]
    [DataRow(148, ClassType.Mage, 10, 114, GemType.Intelligence, 177)]
    [DataRow(56, ClassType.Assassin, 1, 365, GemType.Constitution, 144)]
    public void EquipmentBuilder_gives_proper_gem_values(int characterLevel, ClassType classType,
            int gemMineLevel, int knights, GemType gemType, int expectedGemValue)
    {
        var builder = new EquipmentBuilder(ItemAttributeType.EpicAllAttributes, characterLevel, classType, 0, 0, ItemType.Boots);
        var item = builder.WithGem(gemType, gemMineLevel, knights).Build();

        Assert.AreEqual(expectedGemValue, item.GemValue);
        Assert.AreEqual(gemType, item.GemType);
    }

    [TestMethod]
    public void EquipmentBuilder_shield_cant_have_gem()
    {
        var builder = new EquipmentBuilder(ItemAttributeType.NormalOneStat, 500, ClassType.Warrior, 0, 0, ItemType.Shield);
        var item = builder.WithGem(GemType.Legendary, 100, 1000).Build();

        Assert.IsFalse(item.HasSocket);
        Assert.AreEqual(0, item.GemValue);
        Assert.AreEqual(GemType.None, item.GemType);
    }

    [TestMethod]
    [DataRow(ItemType.Headgear, WitchScrollType.QuestExperience)]
    [DataRow(ItemType.Breastplate, WitchScrollType.QuestMushroom)]
    [DataRow(ItemType.Gloves, WitchScrollType.Reaction)]
    [DataRow(ItemType.Boots, WitchScrollType.QuestSpeed)]
    [DataRow(ItemType.Amulet, WitchScrollType.QuestItems)]
    [DataRow(ItemType.Belt, WitchScrollType.Beer)]
    [DataRow(ItemType.Ring, WitchScrollType.QuestGold)]
    [DataRow(ItemType.Trinket, WitchScrollType.ArenaGold)]
    [DataRow(ItemType.Weapon, WitchScrollType.Crit)]
    [DataRow(ItemType.Shield, WitchScrollType.None)]
    public void EquipmentBuilder_item_have_proper_enchantment(ItemType itemType, WitchScrollType expectedScrollType)
    {
        var builder = new EquipmentBuilder(ItemAttributeType.NormalOneStat, 500, ClassType.Warrior, 0, 0, itemType);
        var item = builder.WithEnchantment().Build();

        Assert.AreEqual(expectedScrollType, item.ScrollType);
    }

    [TestMethod]
    [DataRow(ItemAttributeType.Epic, 1000, ClassType.Warrior, 24, 0, 2027)] // IQ = 1027
    [DataRow(ItemAttributeType.NormalOneStat, 800, ClassType.Mage, 17, 5, 3874)] // IQ = 861
    [DataRow(ItemAttributeType.NormalTwoStats, 900, ClassType.Scout, 13, 0, 2290)] // IQ = 916
    [DataRow(ItemAttributeType.EpicAllAttributes, 800, ClassType.Assassin, 14, 0, 1634)] // IQ = 817
    [DataRow(ItemAttributeType.NormalOneStat, 700, ClassType.BattleMage, 50, 0, 1506)] // IQ = 753
    [DataRow(ItemAttributeType.Legendary, 700, ClassType.Berserker, 8, 0, 1456)] // IQ = 711
    [DataRow(ItemAttributeType.NormalTwoStats, 597, ClassType.DemonHunter, 0, 0, 1500)] // IQ = 600
    [DataRow(ItemAttributeType.Epic, 500, ClassType.Druid, 51, 0, 2493)] // IQ = 554
    [DataRow(ItemAttributeType.Epic, 500, ClassType.Bard, 0, 0, 2264)] // IQ = 503
    [DataRow(ItemAttributeType.Legendary, 397, ClassType.Necromancer, 0, 0, 1843)] // IQ = 400
    [DataRow(ItemAttributeType.Epic, 350, ClassType.Paladin, 1, 0, 708)] // IQ = 354
    public void EquipmentBuilder_weapon_has_proper_damage_values(ItemAttributeType attrType, int characterLevel, ClassType classType, int aura,
            int itemQualityRune, int expectedAvgDmg)
    {
        var builder = new EquipmentBuilder(attrType, characterLevel, classType, aura, itemQualityRune, ItemType.Weapon);
        var item = builder.AsWeapon(1.5D).Build();

        var avgDmg = (item.MaxDmg - item.MinDmg) / 2 + item.MinDmg;

        Assert.AreEqual(expectedAvgDmg, avgDmg);
    }
}