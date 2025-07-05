using Microsoft.VisualStudio.TestTools.UnitTesting;
using SFSimulator.Core;
using System;
using System.Linq;

namespace SFSimulator.Tests;

[TestClass]
public class PetFightableFactoryTests
{
    [DataRow(1, 1)]
    [DataRow(5, 14)]
    [DataRow(10, 34)]
    [DataRow(15, 54)]
    [DataRow(20, 75)]
    [TestMethod]
    public void CreateDungeonPetFightable_assigns_correct_level(int position, int expectedLevel)
    {
        var service = DependencyProvider.Get<IPetFightableFactory>();

        foreach (var elementType in Enum.GetValues<PetElementType>())
        {
            var petFightable = service.CreateDungeonPetFightable(elementType, position);
            Assert.AreEqual(expectedLevel, petFightable.Level, $"Expected level {expectedLevel} for a pet in {elementType} with a position {position}");
        }
    }

    [DataRow(1, 21, 10, 15)]
    [DataRow(5, 262, 131, 196)]
    [DataRow(10, 1_575, 787, 1_181)]
    [DataRow(15, 6_737, 3_368, 5_053)]
    [DataRow(20, 24_320, 12_160, 18_240)]
    [TestMethod]
    public void CreateDungeonPetFightable_assigns_correct_stats(int position, int expectedMainStat, int expectedSecondaryStat, int expectedLuck)
    {
        var service = DependencyProvider.Get<IPetFightableFactory>();

        foreach (var elementType in Enum.GetValues<PetElementType>())
        {
            var petFightable = service.CreateDungeonPetFightable(elementType, position);
            var mainAttribute = ClassConfigurationProvider.Get(petFightable.Class).MainAttribute;
            int main, secondaryFirst, secondarySecond;
            (main, secondaryFirst, secondarySecond) = mainAttribute switch
            {
                AttributeType.Strength => (petFightable.Strength, petFightable.Intelligence, petFightable.Dexterity),
                AttributeType.Intelligence => (petFightable.Intelligence, petFightable.Strength, petFightable.Dexterity),
                _ => (petFightable.Dexterity, petFightable.Strength, petFightable.Intelligence)
            };

            Assert.AreEqual(expectedMainStat, main, $"Expected {expectedMainStat} main stat but got {main}");
            Assert.AreEqual(expectedSecondaryStat, secondaryFirst, $"Expcted {expectedSecondaryStat} secondary stat but got {secondaryFirst}");
            Assert.AreEqual(expectedSecondaryStat, secondarySecond, $"Expcted {expectedSecondaryStat} secondary stat but got {secondarySecond}");
            Assert.AreEqual(expectedMainStat, petFightable.Constitution, $"Expected {expectedMainStat} con stat but got {petFightable.Constitution}");
            Assert.AreEqual(expectedLuck, petFightable.Luck, $"Expected {expectedLuck} luck stat but got {petFightable.Luck}");
        }
    }

    [DataRow(1, PetElementType.Light, 50, 4)]
    [DataRow(5, PetElementType.Shadow, 140, 67)]
    [DataRow(10, PetElementType.Earth, 1_700, 70)]
    [DataRow(15, PetElementType.Fire, 1_350, 137)]
    [DataRow(20, PetElementType.Water, 1_875, 190)]
    [TestMethod]
    public void CreateDungeonPetFightable_assigns_correct_damage_and_armor(int position, PetElementType elementType, int armor, int dmg)
    {
        var service = DependencyProvider.Get<IPetFightableFactory>();

        var petFightable = service.CreateDungeonPetFightable(elementType, position);

        Assert.AreEqual(armor, petFightable.Armor, $"Expected {armor} armor but got {petFightable.Armor}");
        Assert.AreEqual(dmg, petFightable.FirstWeapon!.MaxDmg, $"Expected {dmg} damage but got {petFightable.FirstWeapon.MaxDmg}");
        Assert.AreEqual(dmg, petFightable.FirstWeapon!.MinDmg, $"Expected {dmg} damage but got {petFightable.FirstWeapon.MinDmg}");
    }

    [DataRow(1, 50, PetElementType.Light, 5, 2, 663, 331, 497)]
    [DataRow(5, 75, PetElementType.Shadow, 10, 3, 1_649, 824, 1_236)]
    [DataRow(10, 66, PetElementType.Earth, 12, 8, 3_618, 1_809, 2_713)]
    [DataRow(15, 21, PetElementType.Fire, 9, 5, 2_387, 1_193, 1_790)]
    [DataRow(20, 100, PetElementType.Fire, 20, 20, 40_400, 20_200, 30_300)]
    [TestMethod]
    public void CreatePetFightable_calculates_correct_pack_bonus(int position, int level, PetElementType elementType,
        int pack, int petsWithLevel100, int expectedMainStat, int expectedSecondaryStat, int expectedLuck)
    {
        var service = DependencyProvider.Get<IPetFightableFactory>();
        var pets = new PetsState();
        var elementPets = pets.Elements[elementType];
        var pet = elementPets.First(p => p.Position == position);
        pet.IsObtained = true;
        pet.Level = level;

        foreach (var petInPack in elementPets.Where(p => p.Position != position).Take(pack - 1))
        {
            petInPack.IsObtained = true;
        }

        if (level == 100)
        {
            petsWithLevel100--;
        }

        foreach (var petWithLevel100 in elementPets.Where(p => p.IsObtained && p.Position != position).Take(petsWithLevel100))
        {
            petWithLevel100.Level = 100;
        }

        var petFightable = service.CreatePetFightable(pet, pets, 0);

        var mainAttribute = ClassConfigurationProvider.Get(petFightable.Class).MainAttribute;
        int main, secondaryFirst, secondarySecond;
        (main, secondaryFirst, secondarySecond) = mainAttribute switch
        {
            AttributeType.Strength => (petFightable.Strength, petFightable.Intelligence, petFightable.Dexterity),
            AttributeType.Intelligence => (petFightable.Intelligence, petFightable.Strength, petFightable.Dexterity),
            _ => (petFightable.Dexterity, petFightable.Strength, petFightable.Intelligence)
        };

        Assert.AreEqual(expectedMainStat, main, $"Expected {expectedMainStat} main stat but got {main}");
        Assert.AreEqual(expectedSecondaryStat, secondaryFirst, $"Expcted {expectedSecondaryStat} secondary stat but got {secondaryFirst}");
        Assert.AreEqual(expectedSecondaryStat, secondarySecond, $"Expcted {expectedSecondaryStat} secondary stat but got {secondarySecond}");
        Assert.AreEqual(expectedMainStat, petFightable.Constitution, $"Expected {expectedMainStat} con stat but got {petFightable.Constitution}");
        Assert.AreEqual(expectedLuck, petFightable.Luck, $"Expected {expectedLuck} luck stat but got {petFightable.Luck}");
    }
}
