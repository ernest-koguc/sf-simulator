using Microsoft.VisualStudio.TestTools.UnitTesting;
using SFSimulator.Core;
using System.Collections.Generic;
using System.Linq;

namespace SFSimulator.Tests;

[TestClass]
public class WitchServiceTests
{
    [TestMethod]
    public void GetAvailableItems_returns_all_items_in_a_cycle()
    {
        var service = DependencyProvider.Get<IWitchService>();

        var items = new List<ItemType>();

        for (var day = 1; day <= 9; day++)
        {
            items.AddRange(service.GetAvailableItems(day, false));
        }

        items = items.OrderBy(x => x).ToList();

        var expectedItems = new List<ItemType>
        {
            ItemType.Weapon, ItemType.Shield,
            ItemType.Headgear,
            ItemType.Breastplate,
            ItemType.Gloves,
            ItemType.Boots,
            ItemType.Amulet,
            ItemType.Belt,
            ItemType.Ring,
            ItemType.Trinket,
        }.OrderBy(x => x);

        Assert.IsTrue(items.SequenceEqual(expectedItems), "Witch rotation doesn't cover all items");
    }

    [TestMethod]
    public void GetAvailableItems_returns_no_duplicates()
    {
        var service = DependencyProvider.Get<IWitchService>();

        var items = new List<ItemType>();

        for (var day = 1; day <= 9; day++)
        {
            items.AddRange(service.GetAvailableItems(day, false));
        }

        Assert.IsTrue(items.Distinct().Count() == items.Count, "Witch rotation contains duplicates");
    }

    [TestMethod]
    public void GetAvailableItems_maintains_correct_sequence()
    {
        var service = DependencyProvider.Get<IWitchService>();

        var items = new List<ItemType>();

        for (var day = 1; day <= 81; day++)
        {
            items.AddRange(service.GetAvailableItems(day, false));
        }

        var expectedGroupCount = items.GroupBy(x => x).Count();
        var expectedItems = new List<ItemType>
        {
            ItemType.Weapon, ItemType.Shield,
            ItemType.Headgear,
            ItemType.Breastplate,
            ItemType.Gloves,
            ItemType.Boots,
            ItemType.Amulet,
            ItemType.Belt,
            ItemType.Ring,
            ItemType.Trinket,
        };

        foreach (var item in items.Distinct())
        {
            var group = items.GroupBy(x => x);
            Assert.AreEqual(expectedGroupCount, group.Count(), "Witch rotation is not maintained");
        }

        Assert.IsTrue(items.All(x => expectedItems.Contains(x)), "Witch rotation contains invalid item type");
    }

    [TestMethod]
    public void GetAvailableItems_returns_all_items_on_witch_event()
    {
        var service = DependencyProvider.Get<IWitchService>();
        var items = service.GetAvailableItems(4, true).OrderBy(x => x);

        var expectedItems = new List<ItemType>
        {
            ItemType.Weapon, ItemType.Shield,
            ItemType.Headgear,
            ItemType.Breastplate,
            ItemType.Gloves,
            ItemType.Boots,
            ItemType.Amulet,
            ItemType.Belt,
            ItemType.Ring,
            ItemType.Trinket,
        }.OrderBy(x => x);

        Assert.IsTrue(items.SequenceEqual(expectedItems), "Witch event does not provide all item types");
    }
}
