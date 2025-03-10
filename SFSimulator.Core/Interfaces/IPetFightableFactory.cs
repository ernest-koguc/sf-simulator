namespace SFSimulator.Core;
public interface IPetFightableFactory
{
    PetFightable CreatePetFightable(Pet pet, PetsState petsState, int gladiatorLevel);
    PetFightable CreateDungeonPetFightable(PetElementType elementType, int position);
}
