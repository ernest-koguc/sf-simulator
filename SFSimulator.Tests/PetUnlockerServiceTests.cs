using Microsoft.VisualStudio.TestTools.UnitTesting;
using SFSimulator.Core;
using System;
using System.Linq;

namespace SFSimulator.Tests;

[TestClass]
public class PetUnlockerServiceTests
{
    [DataRow(PetElementType.Light, 5)]
    [DataRow(PetElementType.Earth, 9)]
    [DataRow(PetElementType.Fire, 8)]
    [TestMethod]
    public void UnlockPets_unlocks_pets_during_spring(PetElementType petElementType, int position)
    {
        var service = DependencyProvider.Get<IPetUnlockerService>();
        var simulationContext = new SimulationContext();

        foreach (var pet in simulationContext.Pets.AllPets)
        {
            pet.IsDefeated = true;
            pet.CanBeObtained = true;
            pet.IsObtained = false;
        }

        var springDate = new DateTime(DateTime.Now.Year + 1, 3, 1);
        var day = 365 - DateTime.Now.DayOfYear + springDate.DayOfYear;

        service.UnlockPets(day, simulationContext, []);

        var petToUnlock = simulationContext.Pets.Elements[petElementType].First(p => p.Position == position);

        Assert.IsTrue(petToUnlock.IsObtained, "Expected pet to be obtained but it is not");
    }

    [TestMethod]
    public void UnlockPets_unlocks_all_pets_when_conditions_are_met()
    {
        var service = DependencyProvider.Get<IPetUnlockerService>();
        var simulationContext = new SimulationContext();
        simulationContext.SoloPortal = 50;
        simulationContext.Aura = 20;
        simulationContext.GoldBonus.Tower = 100;
        simulationContext.GemMineLevel = 20;

        foreach (var pet in simulationContext.Pets.AllPets)
        {
            pet.IsDefeated = true;
            pet.CanBeObtained = true;
            pet.IsObtained = false;
        }

        foreach (var day in Enumerable.Range(1, 400))
        {
            service.UnlockPets(day, simulationContext, Enum.GetValues<EventType>().ToList());
        }
        foreach (var pet in simulationContext.Pets.AllPets)
        {
            Assert.IsTrue(pet.IsObtained, $"Expected pet {pet.ElementType} {pet.Position} to be obtained but it is not");
        }
    }
}
