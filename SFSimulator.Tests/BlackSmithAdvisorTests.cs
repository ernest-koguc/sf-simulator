using Microsoft.VisualStudio.TestTools.UnitTesting;
using SFSimulator.Core;
using System.Collections.Generic;

namespace SFSimulator.Tests;


[TestClass]
public class BlackSmithAdvisorTests
{
    public IBlackSmithAdvisor BlackSmithAdvisor { get; set; } = DependencyProvider.GetRequiredService<IBlackSmithAdvisor>();

    [TestMethod]
    [DataRow(ItemAttributeType.Epic, 521, ClassType.BattleMage, 43, 9, 5, ItemType.Breastplate, 1126, 2253)]
    [DataRow(ItemAttributeType.EpicAllAttributes, 521, ClassType.BattleMage, 43, 9, 5, ItemType.Breastplate, 1126, 2253)]
    [DataRow(ItemAttributeType.Legendary, 521, ClassType.BattleMage, 43, 9, 5, ItemType.Breastplate, 1188, 2376)]
    [DataRow(ItemAttributeType.NormalOneStat, 521, ClassType.BattleMage, 43, 9, 5, ItemType.Breastplate, 3432, 19)]
    [DataRow(ItemAttributeType.NormalTwoStats, 521, ClassType.BattleMage, 43, 9, 5, ItemType.Breastplate, 1568, 180)]
    [DataRow(ItemAttributeType.Epic, 521, ClassType.BattleMage, 43, 9, 5, ItemType.Weapon, 1126, 2253)]
    [DataRow(ItemAttributeType.Epic, 521, ClassType.Mage, 43, 9, 5, ItemType.Weapon, 2252, 4506)]
    public void DismantleItem_gives_correct_splinters_and_metal_amount(ItemAttributeType itemAttributeType, int characterLevel,
        ClassType classType, int aura, int scrollsUnlocked, int itemQualityRuneValue, ItemType itemType, int expectedMetal, int expectedSplinters)
    {
        var builder = new EquipmentBuilder(itemAttributeType, characterLevel, classType, aura, scrollsUnlocked, itemQualityRuneValue, itemType)
            .WithAttributes();

        EquipmentItem item;
        if (itemType == ItemType.Weapon)
            item = builder.AsWeapon().Build();
        else
            item = builder.Build();

        var resources = BlackSmithAdvisor.DismantleItem(item);

        Assert.AreEqual(expectedMetal, resources.Metal);
        Assert.AreEqual(expectedSplinters, resources.Splinters);
    }

    [TestMethod]
    [DataRow(ItemAttributeType.Epic, 521, ClassType.Mage, 43, 9, 5)]
    [DataRow(ItemAttributeType.Epic, 521, ClassType.DemonHunter, 43, 9, 5)]
    [DataRow(ItemAttributeType.Epic, 521, ClassType.Scout, 43, 9, 5)]
    [DataRow(ItemAttributeType.Epic, 521, ClassType.Druid, 43, 9, 5)]
    [DataRow(ItemAttributeType.Epic, 521, ClassType.Necromancer, 43, 9, 5)]
    [DataRow(ItemAttributeType.Epic, 521, ClassType.Bard, 43, 9, 5)]
    public void DismantleItem_gives_double_splinters_and_metal_amount_for_nonwarrior_weapons(ItemAttributeType itemAttributeType, int characterLevel,
        ClassType classType, int aura, int scrollsUnlocked, int itemQualityRuneValue)
    {
        var item = new EquipmentBuilder(itemAttributeType, characterLevel, classType, aura, scrollsUnlocked, itemQualityRuneValue, ItemType.Amulet)
            .WithAttributes()
            .Build();

        var weapon = new EquipmentBuilder(itemAttributeType, characterLevel, classType, aura, scrollsUnlocked, itemQualityRuneValue, ItemType.Weapon)
            .WithAttributes()
            .AsWeapon()
            .Build();

        var itemResources = BlackSmithAdvisor.DismantleItem(item);
        var weaponResources = BlackSmithAdvisor.DismantleItem(weapon);

        Assert.AreEqual(weaponResources.Metal, itemResources.Metal * 2);
        Assert.AreEqual(weaponResources.Splinters, itemResources.Splinters * 2);
    }

    [TestMethod]
    [DataRow(ItemAttributeType.Epic, 521, ClassType.BattleMage, 43, 9, 5, ItemType.Breastplate, 20, 621682, 874358)]
    [DataRow(ItemAttributeType.EpicAllAttributes, 521, ClassType.BattleMage, 43, 9, 5, ItemType.Breastplate, 10, 140501, 109849)]
    [DataRow(ItemAttributeType.EpicAllAttributes, 521, ClassType.BattleMage, 43, 9, 5, ItemType.Weapon, 14, 288427, 304094)]
    [DataRow(ItemAttributeType.Legendary, 521, ClassType.Mage, 43, 9, 5, ItemType.Weapon, 18, 1041622, 1353482)]
    [DataRow(ItemAttributeType.NormalOneStat, 400, ClassType.Berserker, 43, 9, 5, ItemType.Ring, 8, 91633, 19015)]
    [DataRow(ItemAttributeType.NormalTwoStats, 200, ClassType.Warrior, 43, 9, 5, ItemType.Trinket, 16, 137949, 108413)]
    public void DismantleItem_gives_proper_refund_costs_for_upgraded_items(ItemAttributeType itemAttributeType, int characterLevel,
        ClassType classType, int aura, int scrollsUnlocked, int itemQualityRuneValue, ItemType itemType, int upgradeLevel, int expectedMetal, int expectedSplinters)
    {
        var builder = new EquipmentBuilder(itemAttributeType, characterLevel, classType, aura, scrollsUnlocked, itemQualityRuneValue, itemType)
            .WithAttributes();

        EquipmentItem item;
        if (itemType == ItemType.Weapon)
            item = builder.AsWeapon().Build();
        else
            item = builder.Build();

        item.UpgradeLevel = upgradeLevel;

        var resources = BlackSmithAdvisor.DismantleItem(item);

        Assert.AreEqual(expectedMetal, resources.Metal);
        Assert.AreEqual(expectedSplinters, resources.Splinters);
    }

    [TestMethod]
    public void UpgradeItems_returns_correct_resources_costs()
    {
        var items = new List<EquipmentItem>
        {
            new EquipmentBuilder(ItemAttributeType.Epic, 521, ClassType.Scout, 43, 9, 5, ItemType.Headgear)
                .WithAttributes()
                .Build(),
            new EquipmentBuilder(ItemAttributeType.Legendary, 300, ClassType.Warrior, 43, 9, 5, ItemType.Breastplate)
                .WithAttributes()
                .Build(),
            new EquipmentBuilder(ItemAttributeType.NormalOneStat, 200, ClassType.DemonHunter, 43, 9, 5, ItemType.Boots)
                .WithAttributes()
                .Build(),
            new EquipmentBuilder(ItemAttributeType.EpicAllAttributes, 400, ClassType.Necromancer, 43, 9, 5, ItemType.Amulet)
                .WithAttributes()
                .Build(),
            new EquipmentBuilder(ItemAttributeType.NormalTwoStats, 100, ClassType.Mage, 43, 9, 5, ItemType.Weapon)
                .WithAttributes()
                .AsWeapon()
                .Build()
        };

        var resources = BlackSmithAdvisor.UpgradeItems(items, new BlackSmithResources(int.MaxValue, int.MaxValue));

        Assert.AreEqual(2352682, resources.Metal);
        Assert.AreEqual(2749160, resources.Splinters);
    }

    [TestMethod]
    public void UpgradeItems_returns_zero_resources_costs_for_empty_items_list()
    {
        var resources = BlackSmithAdvisor.UpgradeItems(new List<EquipmentItem>(), new BlackSmithResources(int.MaxValue, int.MaxValue));

        Assert.AreEqual(0, resources.Metal);
        Assert.AreEqual(0, resources.Splinters);
    }

    [TestMethod]
    public void UpgradeItems_returns_zero_resources_costs_if_there_are_zero_resources_available()
    {
        var items = new List<EquipmentItem>
        {
            new EquipmentBuilder(ItemAttributeType.Epic, 521, ClassType.Scout, 43, 9, 5, ItemType.Headgear)
                .WithAttributes()
                .Build()
        };
        var resources = BlackSmithAdvisor.UpgradeItems(items, new BlackSmithResources(0, 0));

        Assert.AreEqual(0, resources.Metal);
        Assert.AreEqual(0, resources.Splinters);
    }

    [TestMethod]
    public void UpgradeItems_doesnt_upgrade_items_that_are_fully_upgraded()
    {
        var items = new List<EquipmentItem>
        {
            new EquipmentBuilder(ItemAttributeType.Epic, 521, ClassType.Scout, 43, 9, 5, ItemType.Headgear)
                .WithAttributes()
                .Build(),
            new EquipmentBuilder(ItemAttributeType.Epic, 521, ClassType.Scout, 43, 9, 5, ItemType.Headgear)
                .WithAttributes()
                .Build(),
        };

        foreach (var item in items)
        {
            item.UpgradeLevel = 20;
        }
        var resources = BlackSmithAdvisor.UpgradeItems(items, new BlackSmithResources(int.MaxValue, int.MaxValue));

        Assert.AreEqual(0, resources.Metal);
        Assert.AreEqual(0, resources.Splinters);
    }
}