using Microsoft.VisualStudio.TestTools.UnitTesting;
using SFSimulator.Core;
using System;

namespace SFSimulator.Tests;

[TestClass]
public class PetPathProviderTests
{
    [TestMethod]
    public void GetPetFromPath_returns_no_pet_if_there_is_no_pet_available()
    {
        var service = DependencyProvider.Get<IPetPathProvider>();

        var petsState = new PetsState();

        foreach (var elementType in Enum.GetValues<PetElementType>())
        {
            var pet = service.GetPetFromPath(elementType, petsState);
            Assert.IsNull(pet, $"Expected no pet but got {pet!.ElementType} {pet!.Position}");
        }
    }
}
