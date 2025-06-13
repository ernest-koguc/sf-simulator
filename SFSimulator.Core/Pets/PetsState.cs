using System.ComponentModel;

namespace SFSimulator.Core;

public class PetsState
{
    public List<Pet> AllPets => Elements.Values.SelectMany(p => p).ToList();
    public Dictionary<PetElementType, Pet[]> Elements { get; set; } = new();
    public Dictionary<PetElementType, double> Food { get; set; } = new()
    {
        {PetElementType.Shadow, 0},
        {PetElementType.Light, 0},
        {PetElementType.Earth, 0},
        {PetElementType.Fire, 0},
        {PetElementType.Water, 0}
    };

    public PetsState()
    {
        foreach (var element in Enum.GetValues<PetElementType>())
        {
            Elements.Add(element, Enumerable.Range(1, 20).Select(i => new Pet
            {
                Level = 0,
                Position = i,
                ElementType = element,
                IsDefeated = false,
                IsObtained = false,
                CanBeObtained = i <= 3
            }).ToArray());
        }
    }

    public PetsState(SFToolsPets pets)
    {
        Elements.Add(PetElementType.Shadow, pets.ShadowLevels
            .Select((level, index) =>
                    new Pet
                    {
                        Level = level,
                        Position = index + 1,
                        ElementType = PetElementType.Shadow,
                        IsDefeated = pets.Dungeons[0] >= index + 1,
                        IsObtained = level > 0,
                        CanBeObtained = index + 1 <= 3
                    })
            .ToArray());
        Elements.Add(PetElementType.Light, pets.LightLevels
            .Select((level, index) =>
                    new Pet
                    {
                        Level = level,
                        Position = index + 1,
                        ElementType = PetElementType.Light,
                        IsDefeated = pets.Dungeons[1] >= index + 1,
                        IsObtained = level > 0,
                        CanBeObtained = index + 1 <= 3
                    })
            .ToArray());
        Elements.Add(PetElementType.Earth, pets.EarthLevels
            .Select((level, index) =>
                    new Pet
                    {
                        Level = level,
                        Position = index + 1,
                        ElementType = PetElementType.Earth,
                        IsDefeated = pets.Dungeons[2] >= index + 1,
                        IsObtained = level > 0,
                        CanBeObtained = index + 1 <= 3
                    })
            .ToArray());
        Elements.Add(PetElementType.Fire, pets.FireLevels
            .Select((level, index) =>
                    new Pet
                    {
                        Level = level,
                        Position = index + 1,
                        ElementType = PetElementType.Fire,
                        IsDefeated = pets.Dungeons[3] >= index + 1,
                        IsObtained = level > 0,
                        CanBeObtained = index + 1 <= 3
                    })
            .ToArray());
        Elements.Add(PetElementType.Water, pets.WaterLevels
            .Select((level, index) =>
                    new Pet
                    {
                        Level = level,
                        Position = index + 1,
                        ElementType = PetElementType.Water,
                        IsDefeated = pets.Dungeons[4] >= index + 1,
                        IsObtained = level > 0,
                        CanBeObtained = index + 1 <= 3
                    })
            .ToArray());

        Food[PetElementType.Shadow] = pets.ShadowFood;
        Food[PetElementType.Light] = pets.LightFood;
        Food[PetElementType.Earth] = pets.EarthFood;
        Food[PetElementType.Fire] = pets.FireFood;
        Food[PetElementType.Water] = pets.WaterFood;
    }

    public double GetPetBonusFor(AttributeType attributeType)
    {
        var element = attributeType switch
        {
            AttributeType.Strength => PetElementType.Water,
            AttributeType.Dexterity => PetElementType.Light,
            AttributeType.Intelligence => PetElementType.Earth,
            AttributeType.Constitution => PetElementType.Shadow,
            AttributeType.Luck => PetElementType.Fire,
            _ => throw new InvalidEnumArgumentException(nameof(attributeType))
        };

        var bonusFromObtainedPets = Elements[element].Count(p => p.IsObtained);
        var bonusFromPetsLevel = Elements[element].Where(p => p.Level >= 100).Sum(p => p.Level / 50D * 0.25D);
        var totalBonus = 1 + ((bonusFromObtainedPets + Math.Floor(bonusFromPetsLevel)) / 100);

        return totalBonus;
    }
}