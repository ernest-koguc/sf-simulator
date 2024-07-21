using System.ComponentModel;

namespace SFSimulator.Core;

public class PetsState
{
    public Pet[] Shadow { get; init; } = default!;
    public Pet[] Light { get; init; } = default!;
    public Pet[] Earth { get; init; } = default!;
    public Pet[] Fire { get; init; } = default!;
    public Pet[] Water { get; init; } = default!;

    public PetsState() { }

    public PetsState(Pets pets)
    {
        Shadow = pets.ShadowLevels
            .Select((level, index) =>
                    new Pet
                    {
                        Level = level,
                        Position = index + 1,
                        LockType = level > 0 ? PetUnlockType.Obtained : pets.Dungeons[0] >= index + 1 ? PetUnlockType.Unlocked : PetUnlockType.Locked,
                        ElementType = PetElementType.Shadow
                    })
            .ToArray();
        Light = pets.LightLevels
            .Select((level, index) =>
                    new Pet
                    {
                        Level = level,
                        Position = index + 1,
                        LockType = level > 0 ? PetUnlockType.Obtained : pets.Dungeons[1] >= index + 1 ? PetUnlockType.Unlocked : PetUnlockType.Locked,
                        ElementType = PetElementType.Light
                    })
            .ToArray();
        Earth = pets.EarthLevels
            .Select((level, index) =>
                    new Pet
                    {
                        Level = level,
                        Position = index + 1,
                        LockType = level > 0 ? PetUnlockType.Obtained : pets.Dungeons[2] >= index + 1 ? PetUnlockType.Unlocked : PetUnlockType.Locked,
                        ElementType = PetElementType.Earth
                    })
            .ToArray();
        Fire = pets.FireLevels
            .Select((level, index) =>
                    new Pet
                    {
                        Level = level,
                        Position = index + 1,
                        LockType = level > 0 ? PetUnlockType.Obtained : pets.Dungeons[3] >= index + 1 ? PetUnlockType.Unlocked : PetUnlockType.Locked,
                        ElementType = PetElementType.Fire
                    })
            .ToArray();
        Water = pets.WaterLevels
            .Select((level, index) =>
                    new Pet
                    {
                        Level = level,
                        Position = index + 1,
                        LockType = level > 0 ? PetUnlockType.Obtained : pets.Dungeons[4] >= index + 1 ? PetUnlockType.Unlocked : PetUnlockType.Locked,
                        ElementType = PetElementType.Water
                    })
            .ToArray();
    }

    public double GetPetBonusFor(AttributeType attributeType)
    {
        var elementToLookup = attributeType switch
        {
            AttributeType.Strength => Water,
            AttributeType.Dexterity => Light,
            AttributeType.Intelligence => Earth,
            AttributeType.Constitution => Shadow,
            AttributeType.Luck => Fire,
            _ => throw new InvalidEnumArgumentException(nameof(attributeType))
        };

        var bonusFromObtainedPets = elementToLookup.Count(p => p.LockType == PetUnlockType.Obtained);
        var bonusFromPetsLevel = elementToLookup.Where(p => p.LockType == PetUnlockType.Obtained && p.Level >= 100).Sum(p => p.Level / 50D * 0.25D);
        var totalBonus = 1 + ((bonusFromObtainedPets + Math.Floor(bonusFromPetsLevel)) / 100);

        return totalBonus;
    }
}