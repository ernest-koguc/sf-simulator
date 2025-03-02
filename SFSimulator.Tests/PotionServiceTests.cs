using Microsoft.VisualStudio.TestTools.UnitTesting;
using SFSimulator.Core;
using System.Linq;

namespace SFSimulator.Tests;

[TestClass]
public class PotionServiceTests
{
    [DataRow(ClassType.Warrior, PotionType.Strength)]
    [DataRow(ClassType.Mage, PotionType.Intelligence)]
    [DataRow(ClassType.Scout, PotionType.Dexterity)]
    [DataRow(ClassType.Assassin, PotionType.Dexterity)]
    [DataRow(ClassType.BattleMage, PotionType.Strength)]
    [DataRow(ClassType.Berserker, PotionType.Strength)]
    [DataRow(ClassType.DemonHunter, PotionType.Dexterity)]
    [DataRow(ClassType.Druid, PotionType.Intelligence)]
    [DataRow(ClassType.Bard, PotionType.Intelligence)]
    [DataRow(ClassType.Necromancer, PotionType.Intelligence)]
    [DataRow(ClassType.Paladin, PotionType.Strength)]
    [TestMethod]
    public void GetPotions_gives_correct_main_attribute_potion(ClassType classType, PotionType expectedMainAttributePotion)
    {
        var potionService = DependencyProvider.Get<IPotionService>();

        var potions = potionService.GetPotions(classType);

        Assert.IsTrue(potions.Where(p => p.Type == expectedMainAttributePotion).Count() == 1, $"Potions list should contain only one {expectedMainAttributePotion}");
    }

    [TestMethod]
    public void GetPotions_gives_no_more_than_3_potions()
    {
        var potionService = DependencyProvider.Get<IPotionService>();

        var potions = potionService.GetPotions(ClassType.Mage);

        Assert.IsTrue(potions.Count == 3, "Potions list should contain only 3 potions");
    }

    [TestMethod]
    public void GetPotions_gives_constitution_and_eternity_potions()
    {
        var potionService = DependencyProvider.Get<IPotionService>();

        var potions = potionService.GetPotions(ClassType.Mage);

        Assert.IsTrue(potions.Where(p => p.Type == PotionType.Constitution).Count() == 1, $"Potions list should contain only one {PotionType.Constitution}");
        Assert.IsTrue(potions.Where(p => p.Type == PotionType.Eternity).Count() == 1, $"Potions list should contain only one {PotionType.Eternity}");
    }
}
