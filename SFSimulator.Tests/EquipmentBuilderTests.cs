using Microsoft.VisualStudio.TestTools.UnitTesting;
using SFSimulator.Core;
using System;
using System.Collections.Generic;
using System.Linq;

namespace SFSimulator.Tests;

[TestClass]
public class EquipmentBuilderTests
{
    [TestMethod]
    [DataRow(ItemAttributeType.NormalOneStat, 513, ClassType.Warrior, 41, 9, 5, 100, 1000, ItemType.Headgear, 1290)]
    [DataRow(ItemAttributeType.NormalTwoStats, 513, ClassType.Warrior, 41, 9, 5, 100, 1000, ItemType.Headgear, 645)]
    [DataRow(ItemAttributeType.Epic, 513, ClassType.Warrior, 41, 9, 5, 100, 1000, ItemType.Headgear, 774)]
    [DataRow(ItemAttributeType.Legendary, 513, ClassType.Warrior, 41, 9, 5, 100, 1000, ItemType.Headgear, 810)]
    [DataRow(ItemAttributeType.NormalOneStat, 148, ClassType.Mage, 5, 9, 0, 10, 114, ItemType.Headgear, 334)]
    public void EquipmentBuilder_gives_correct_attributes(ItemAttributeType attrType, int characterLevel, ClassType classType, int aura,
            int scrollsUnlocked, int itemQualityRune, int gemMineLevel, int knights, ItemType itemType, int expectedStat)
    {
        var builder = new EquipmentBuilder(attrType, characterLevel, classType, aura, scrollsUnlocked, itemQualityRune, itemType);
        builder.WithAttributes();
        var item = builder.Build();

        var mainAttr = ClassConfigurationProvider.GetClassConfiguration(classType).MainAttribute;
        Assert.AreEqual(expectedStat, item[mainAttr]);
    }

    [TestMethod]
    [DataRow(ItemAttributeType.Legendary)]
    [DataRow(ItemAttributeType.Epic)]
    public void EquipmentBuilder_epic_and_legendary_items_have_3_equal_attributes(ItemAttributeType attrType)
    {
        foreach (var classType in Enum.GetValues<ClassType>())
        {
            var builder = new EquipmentBuilder(attrType, 500, classType, 20, 0, 0, ItemType.Headgear);
            builder.WithAttributes();
            var item = builder.Build();

            var mainAttr = ClassConfigurationProvider.GetClassConfiguration(classType).MainAttribute;

            var attrWithNoValue = Enum.GetValues<AttributeType>().Except([mainAttr, AttributeType.Constitution, AttributeType.Luck]).Select(attr => item[attr]);
            var attrWithValues = new List<int> { item[mainAttr], item.Constitution, item.Luck };

            // Assert that main + con + luck are all equal
            Assert.AreEqual(item[mainAttr], item.Constitution, item.Luck);
            // Assert that main + con + luck are greater than 0
            Assert.IsTrue(attrWithValues.All(v => v > 0));
            // Assert that 2 secondaries are equal to 0
            Assert.IsTrue(attrWithNoValue.All(v => v == 0));
        }
    }

    [TestMethod]
    [DataRow(ItemAttributeType.Legendary, AttributeType.Strength, AttributeType.Constitution, AttributeType.Luck)]
    [DataRow(ItemAttributeType.Epic, AttributeType.Strength, AttributeType.Constitution, AttributeType.Luck)]
    [DataRow(ItemAttributeType.NormalTwoStats, AttributeType.Strength, AttributeType.Constitution)]
    [DataRow(ItemAttributeType.NormalOneStat, AttributeType.Strength)]
    public void EquipmentBuilder_items_have_proper_attribute_types_assigned(ItemAttributeType attrType, params AttributeType[] expectedAttributes)
    {
        var builder = new EquipmentBuilder(attrType, 500, ClassType.Warrior, 20, 0, 0, ItemType.Headgear);
        var item = builder.WithAttributes().Build();

        var attrWithoutValues = Enum.GetValues<AttributeType>().Except(expectedAttributes).Select(attr => item[attr]);
        var attrWithValues = expectedAttributes.Select(attr => item[attr]);

        Assert.IsTrue(attrWithValues.All(v => v > 0));
        Assert.IsTrue(attrWithoutValues.All(v => v == 0));
    }

    [TestMethod]
    [DataRow(ItemType.Headgear, 1815)]
    [DataRow(ItemType.Breastplate, 1815)]
    [DataRow(ItemType.Gloves, 1815)]
    [DataRow(ItemType.Boots, 1815)]
    [DataRow(ItemType.Weapon, 0)]
    [DataRow(ItemType.Shield, 0)]
    [DataRow(ItemType.Amulet, 0)]
    [DataRow(ItemType.Belt, 1815)]
    [DataRow(ItemType.Ring, 0)]
    [DataRow(ItemType.Trinket, 0)]
    public void EquipmentBuilder_provides_armor_for_proper_item_type(ItemType itemType, int expectedArmor)
    {
        var builder = new EquipmentBuilder(ItemAttributeType.Epic, 513, ClassType.BattleMage, 41, 9, 5, itemType);
        var item = builder.WithArmor().Build();

        Assert.AreEqual(expectedArmor, item.Armor);
    }

    [TestMethod]
    [DataRow(ItemAttributeType.NormalOneStat, 513, ClassType.BattleMage, 41, 9, 5, 100, 1000, ItemType.Headgear, 1815)]
    [DataRow(ItemAttributeType.NormalOneStat, 513, ClassType.Mage, 41, 9, 5, 100, 1000, ItemType.Headgear, 1815)]
    [DataRow(ItemAttributeType.NormalOneStat, 513, ClassType.Necromancer, 41, 9, 5, 100, 1000, ItemType.Headgear, 1815)]
    [DataRow(ItemAttributeType.NormalTwoStats, 513, ClassType.Warrior, 41, 9, 5, 100, 1000, ItemType.Headgear, 9075)]
    [DataRow(ItemAttributeType.NormalTwoStats, 513, ClassType.Berserker, 41, 9, 5, 100, 1000, ItemType.Headgear, 9075)]
    [DataRow(ItemAttributeType.NormalTwoStats, 513, ClassType.DemonHunter, 41, 9, 5, 100, 1000, ItemType.Headgear, 9075)]
    [DataRow(ItemAttributeType.Legendary, 513, ClassType.Scout, 41, 9, 5, 100, 1000, ItemType.Headgear, 4612)]
    [DataRow(ItemAttributeType.Legendary, 513, ClassType.Assassin, 41, 9, 5, 100, 1000, ItemType.Headgear, 4612)]
    [DataRow(ItemAttributeType.Legendary, 513, ClassType.Druid, 41, 9, 5, 100, 1000, ItemType.Breastplate, 4612)]
    [DataRow(ItemAttributeType.Legendary, 513, ClassType.Bard, 41, 9, 5, 100, 1000, ItemType.Breastplate, 4612)]
    [DataRow(ItemAttributeType.NormalOneStat, 148, ClassType.Mage, 5, 9, 0, 10, 114, ItemType.Headgear, 489)]
    public void EquipmentBuilder_provides_proper_armor_values(ItemAttributeType attrType, int characterLevel, ClassType classType, int aura,
            int scrollsUnlocked, int itemQualityRune, int gemMineLevel, int knights, ItemType itemType, int expectedArmor)
    {
        var builder = new EquipmentBuilder(attrType, characterLevel, classType, aura, scrollsUnlocked, itemQualityRune, itemType);
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
        var builder = new EquipmentBuilder(ItemAttributeType.EpicAllAttributes, characterLevel, classType, 0, 0, 0, ItemType.Boots);
        var item = builder.WithGem(gemType, gemMineLevel, knights).Build();

        Assert.AreEqual(expectedGemValue, item.GemValue);
        Assert.AreEqual(gemType, item.GemType);
    }

    [TestMethod]
    public void EquipmentBuilder_shield_cant_have_gem()
    {
        var builder = new EquipmentBuilder(ItemAttributeType.NormalOneStat, 500, ClassType.Warrior, 0, 0, 0, ItemType.Shield);
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
        var builder = new EquipmentBuilder(ItemAttributeType.NormalOneStat, 500, ClassType.Warrior, 0, 0, 0, itemType);
        var item = builder.WithEnchantment().Build();

        Assert.AreEqual(expectedScrollType, item.ScrollType);
    }

    [TestMethod]
    [DataRow(ItemAttributeType.NormalOneStat, 513, ClassType.BattleMage, 41, 9, 5, 1310)]
    [DataRow(ItemAttributeType.Epic, 513, ClassType.Warrior, 41, 9, 5, 1310)]
    [DataRow(ItemAttributeType.EpicAllAttributes, 513, ClassType.Assassin, 41, 9, 5, 1310)]
    [DataRow(ItemAttributeType.Legendary, 513, ClassType.Berserker, 41, 9, 5, 1330)]
    [DataRow(ItemAttributeType.NormalOneStat, 148, ClassType.Mage, 5, 9, 0, 751)]
    [DataRow(ItemAttributeType.Epic, 148, ClassType.Necromancer, 5, 9, 0, 751)]
    [DataRow(ItemAttributeType.Epic, 148, ClassType.Druid, 5, 9, 0, 751)]
    [DataRow(ItemAttributeType.Epic, 148, ClassType.Bard, 5, 9, 0, 751)]
    [DataRow(ItemAttributeType.NormalTwoStats, 227, ClassType.DemonHunter, 7, 9, 0, 630)]
    [DataRow(ItemAttributeType.NormalTwoStats, 227, ClassType.Scout, 7, 9, 0, 630)]
    public void EquipmentBuilder_weapon_has_proper_damage_values(ItemAttributeType attrType, int characterLevel, ClassType classType, int aura,
            int scrollsUnlocked, int itemQualityRune, int expectedAvgDmg)
    {
        var builder = new EquipmentBuilder(attrType, characterLevel, classType, aura, scrollsUnlocked, itemQualityRune, ItemType.Weapon);
        var item = builder.AsWeapon(1.5D).Build();

        var avgDmg = (item.MaxDmg - item.MinDmg) / 2 + item.MinDmg;

        Assert.AreEqual(expectedAvgDmg, avgDmg);
    }
}