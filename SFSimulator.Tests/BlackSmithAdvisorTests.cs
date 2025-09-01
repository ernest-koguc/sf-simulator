using Microsoft.VisualStudio.TestTools.UnitTesting;
using SFSimulator.Core;
using System.Collections.Generic;

namespace SFSimulator.Tests;


[TestClass]
public class BlackSmithAdvisorTests
{
    public IBlackSmithAdvisor BlackSmithAdvisor { get; set; } = DependencyProvider.Get<IBlackSmithAdvisor>();

    [TestMethod]
    [DataRow(ItemAttributeType.Epic, 521, ClassType.BattleMage, 43, 5, ItemType.Breastplate, 1936, 3872)]
    [DataRow(ItemAttributeType.EpicAllAttributes, 521, ClassType.BattleMage, 43, 5, ItemType.Breastplate, 1936, 3872)]
    [DataRow(ItemAttributeType.Legendary, 521, ClassType.BattleMage, 43, 5, ItemType.Breastplate, 2122, 4245)]
    [DataRow(ItemAttributeType.NormalOneStat, 521, ClassType.BattleMage, 43, 5, ItemType.Breastplate, 4179, 23)]
    [DataRow(ItemAttributeType.NormalTwoStats, 521, ClassType.BattleMage, 43, 5, ItemType.Breastplate, 2696, 311)]
    [DataRow(ItemAttributeType.Epic, 521, ClassType.BattleMage, 43, 5, ItemType.Weapon, 1936, 3872)]
    [DataRow(ItemAttributeType.Epic, 521, ClassType.Mage, 43, 5, ItemType.Weapon, 3872, 7744)]
    public void DismantleItem_gives_correct_splinters_and_metal_amount(ItemAttributeType itemAttributeType, int characterLevel,
        ClassType classType, int aura, int itemQualityRuneValue, ItemType itemType, int expectedMetal, int expectedSplinters)
    {
        var builder = new EquipmentBuilder(itemAttributeType, characterLevel, classType, aura, itemQualityRuneValue, itemType)
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
        var item = new EquipmentBuilder(itemAttributeType, characterLevel, classType, aura, itemQualityRuneValue, ItemType.Amulet)
            .WithAttributes()
            .Build();

        var weapon = new EquipmentBuilder(itemAttributeType, characterLevel, classType, aura, itemQualityRuneValue, ItemType.Weapon)
            .WithAttributes()
            .AsWeapon()
            .Build();

        var itemResources = BlackSmithAdvisor.DismantleItem(item);
        var weaponResources = BlackSmithAdvisor.DismantleItem(weapon);

        Assert.AreEqual(weaponResources.Metal, itemResources.Metal * 2);
        Assert.AreEqual(weaponResources.Splinters, itemResources.Splinters * 2);
    }

    [TestMethod]
    [DataRow(ItemAttributeType.Epic, 521, ClassType.BattleMage, 43, 5, ItemType.Breastplate, 20, 1067798, 1501933)]
    [DataRow(ItemAttributeType.EpicAllAttributes, 521, ClassType.BattleMage, 43, 5, ItemType.Breastplate, 10, 241461, 188823)]
    [DataRow(ItemAttributeType.EpicAllAttributes, 521, ClassType.BattleMage, 43, 5, ItemType.Weapon, 14, 495520, 522485)]
    [DataRow(ItemAttributeType.Legendary, 521, ClassType.Mage, 43, 5, ItemType.Weapon, 18, 1859800, 2416538)]
    [DataRow(ItemAttributeType.NormalOneStat, 400, ClassType.Berserker, 43, 5, ItemType.Ring, 8, 93114, 19318)]
    [DataRow(ItemAttributeType.NormalTwoStats, 200, ClassType.Warrior, 43, 5, ItemType.Trinket, 16, 99769, 78425)]
    public void DismantleItem_gives_proper_refund_costs_for_upgraded_items(ItemAttributeType itemAttributeType, int characterLevel,
        ClassType classType, int aura, int itemQualityRuneValue, ItemType itemType, int upgradeLevel, int expectedMetal, int expectedSplinters)
    {
        var builder = new EquipmentBuilder(itemAttributeType, characterLevel, classType, aura, itemQualityRuneValue, itemType)
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
            new EquipmentBuilder(ItemAttributeType.Epic, 521, ClassType.Scout, 43, 5, ItemType.Headgear)
                .WithAttributes()
                .Build(),
            new EquipmentBuilder(ItemAttributeType.Legendary, 300, ClassType.Warrior, 43, 5, ItemType.Breastplate)
                .WithAttributes()
                .Build(),
            new EquipmentBuilder(ItemAttributeType.NormalOneStat, 200, ClassType.DemonHunter, 43, 5, ItemType.Boots)
                .WithAttributes()
                .Build(),
            new EquipmentBuilder(ItemAttributeType.EpicAllAttributes, 400, ClassType.Necromancer, 43, 5, ItemType.Amulet)
                .WithAttributes()
                .Build(),
            new EquipmentBuilder(ItemAttributeType.NormalTwoStats, 100, ClassType.Mage, 43, 5, ItemType.Weapon)
                .WithAttributes()
                .AsWeapon()
                .Build()
        };

        var resources = BlackSmithAdvisor.UpgradeItems(items, new BlackSmithResources(int.MaxValue, int.MaxValue));

        Assert.AreEqual(2916814, resources.Metal);
        Assert.AreEqual(3691420, resources.Splinters);
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
            new EquipmentBuilder(ItemAttributeType.Epic, 521, ClassType.Scout, 43, 5, ItemType.Headgear)
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
            new EquipmentBuilder(ItemAttributeType.Epic, 521, ClassType.Scout, 43, 5, ItemType.Headgear)
                .WithAttributes()
                .Build(),
            new EquipmentBuilder(ItemAttributeType.Epic, 521, ClassType.Scout, 43, 5, ItemType.Headgear)
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