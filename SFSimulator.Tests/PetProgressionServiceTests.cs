using Microsoft.VisualStudio.TestTools.UnitTesting;
using SFSimulator.Core;
using System;
using System.Linq;

namespace SFSimulator.Tests;

[TestClass]
public class PetProgressionServiceTests
{
    [TestMethod]
    public void ProgressThrough_defeats_all_pets_over_period_of_time()
    {
        var service = DependencyProvider.Get<IPetProgressionService>();
        var simulationContext = new SimulationContext();
        simulationContext.Level = 75;
        simulationContext.GladiatorLevel = 15;

        var events = Enum.GetValues<EventType>().ToList();
        for (var i = 1; i < 1000; i++)
        {
            service.ProgressThrough(i, simulationContext, events, _ => { });
        }

        foreach (var pet in simulationContext.Pets.AllPets)
        {
            if (pet.IsObtained)
            {
                Assert.IsTrue(pet.Level >= 1, $"Expected obtained pet to have at least level 1 but got level {pet.Level} instead");
            }
            Assert.IsTrue(pet.IsDefeated, "Expected pet to be defeated");
        }
    }

    [TestMethod]
    public void ProgressThrough_doesnt_feed_pets_when_pets_are_not_unlocked()
    {
        var service = DependencyProvider.Get<IPetProgressionService>();
        var simulationContext = new SimulationContext();
        simulationContext.Level = 74;

        foreach (var element in Enum.GetValues<PetElementType>())
        {
            simulationContext.Pets.Food[element] = 1000;
        }

        service.ProgressThrough(1, simulationContext, [], _ => Assert.Fail("No pet dungeon should be beaten when pets aren't unlocked"));

        foreach (var pet in simulationContext.Pets.AllPets)
        {
            Assert.AreEqual(0, pet.Level, $"Expected pet to have 0 level but got {pet.Level}");
            Assert.IsFalse(pet.IsDefeated, "Expected pet to not be defeated");
            Assert.IsFalse(pet.IsObtained, "Expected pet to not be obtained");
        }
    }

    [TestMethod]
    public void ProgressThrough_feeds_the_last_pet_when_all_pets_are_defeated()
    {
        var service = DependencyProvider.Get<IPetProgressionService>();
        var simulationContext = new SimulationContext();
        simulationContext.Level = 100;

        foreach (var element in Enum.GetValues<PetElementType>())
        {
            simulationContext.Pets.Food[element] = 1000;
        }

        foreach (var pet in simulationContext.Pets.AllPets)
        {
            pet.IsDefeated = true;
            pet.IsObtained = true;
            pet.Level = 1;
        }

        service.ProgressThrough(1, simulationContext, [EventType.Pets], _ => { });

        foreach (var pet in simulationContext.Pets.AllPets.Where(p => p.Position == 20))
        {
            Assert.AreEqual(10, pet.Level);
        }
    }

    [DataRow(632, 1000, 2_000_000_000D)]
    [DataRow(632, 500, 1_000_000_000D)]
    [DataRow(632, 333, 666_000_000D)]
    [DataRow(631, 333, 0)]
    [DataRow(632, 0, 0)]
    [TestMethod]
    public void SellPetFood_gives_correct_amount_of_gold(int level, int foodAmount, double expectedGold)
    {
        var service = DependencyProvider.Get<IPetProgressionService>();
        var simulationContext = new SimulationContext();
        simulationContext.Level = level;

        foreach (var element in Enum.GetValues<PetElementType>())
        {
            simulationContext.Pets.Food[element] = (double)foodAmount / Enum.GetValues<PetElementType>().Count();
        }

        foreach (var pet in simulationContext.Pets.AllPets)
        {
            pet.IsDefeated = true;
            pet.IsObtained = true;
            pet.Level = 200;
        }

        var gold = service.SellPetFood(simulationContext.Pets, level);

        Assert.AreEqual((decimal)expectedGold, gold);
    }

    [DataRow(632, 1000, 0)]
    [DataRow(631, 1000, 1000)]
    [DataRow(632, 0, 0)]
    [TestMethod]
    public void SellPetFood_removes_food_if_it_was_sold(int level, int foodAmount, int expectedFood)
    {
        var service = DependencyProvider.Get<IPetProgressionService>();
        var simulationContext = new SimulationContext();
        simulationContext.Level = level;

        foreach (var element in Enum.GetValues<PetElementType>())
        {
            simulationContext.Pets.Food[element] = (double)foodAmount / Enum.GetValues<PetElementType>().Count();
        }

        foreach (var pet in simulationContext.Pets.AllPets)
        {
            pet.IsDefeated = true;
            pet.IsObtained = true;
            pet.Level = 200;
        }

        service.SellPetFood(simulationContext.Pets, level);
        var food = simulationContext.Pets.Food.Values.Sum();


        Assert.AreEqual((double)expectedFood, food);
    }
}
