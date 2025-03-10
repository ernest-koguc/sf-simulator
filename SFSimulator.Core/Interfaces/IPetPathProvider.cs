namespace SFSimulator.Core;

public interface IPetPathProvider
{
    Pet? GetPetFromPath(PetElementType elementType, PetsState petsState);
}