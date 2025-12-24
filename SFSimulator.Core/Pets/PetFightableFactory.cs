namespace SFSimulator.Core;

public class PetFightableFactory : IPetFightableFactory
{
    public PetFightable CreatePetFightable(Pet pet, PetsState petsState, int gladiatorLevel)
    {
        var classLevel = pet.ElementType switch
        {
            PetElementType.Shadow => Shadow[pet.Position - 1],
            PetElementType.Light => Light[pet.Position - 1],
            PetElementType.Earth => Earth[pet.Position - 1],
            PetElementType.Fire => Fire[pet.Position - 1],
            PetElementType.Water => Water[pet.Position - 1],
            _ => throw new ArgumentOutOfRangeException(nameof(pet.ElementType)),
        };

        var packBonus = petsState.Elements[pet.ElementType].Count(p => p.IsObtained) * 0.05M;
        var levelBonus = petsState.Elements[pet.ElementType].Where(p => p.Level >= 100).Sum(p => p.Level / 50) / 4 * 0.05M;

        return CreatePetFightable(pet.ElementType, pet.Level, classLevel.Class, pet.Position, 1 + packBonus + levelBonus, gladiatorLevel);
    }
    public PetFightable CreateDungeonPetFightable(PetElementType elementType, int position)
    {
        var classLevel = elementType switch
        {
            PetElementType.Shadow => Shadow[position - 1],
            PetElementType.Light => Light[position - 1],
            PetElementType.Earth => Earth[position - 1],
            PetElementType.Fire => Fire[position - 1],
            PetElementType.Water => Water[position - 1],
            _ => throw new ArgumentOutOfRangeException(nameof(elementType)),
        };

        return CreatePetFightable(elementType, classLevel.Level, classLevel.Class, position, 1 + position * 0.05M, 0);
    }

    private PetFightable CreatePetFightable(PetElementType elementType, int level, ClassType classType, int position, decimal elementalBonus, int gladiatorLevel)
    {
        var mainAndConStat = MainAttributePerLevel.ElementAt(position - 1) * (level + 1) * elementalBonus;
        var luckStat = LuckAttributePerLevel.ElementAt(position - 1) * (level + 1) * elementalBonus;
        var classConfiguration = ClassConfigurationProvider.Get(classType);
        var mainAttribute = classConfiguration.MainAttribute;
        decimal strength, dexterity, intelligence;
        switch (mainAttribute)
        {
            case AttributeType.Strength:
                strength = mainAndConStat;
                dexterity = mainAndConStat / 2;
                intelligence = mainAndConStat / 2;
                break;
            case AttributeType.Dexterity:
                strength = mainAndConStat / 2;
                dexterity = mainAndConStat;
                intelligence = mainAndConStat / 2;
                break;
            case AttributeType.Intelligence:
                strength = mainAndConStat / 2;
                dexterity = mainAndConStat / 2;
                intelligence = mainAndConStat;
                break;
            default:
                throw new InvalidOperationException("Invalid main attribute");
        }
        var secondaryAttribute = mainAndConStat / 2;
        var armorMultiplier = classType switch
        {
            ClassType.Mage => 1,
            ClassType.Scout => 2.5M,
            ClassType.Warrior => 5,
            _ => throw new InvalidOperationException("Invalid class type"),
        };
        var armor = level * 10 * armorMultiplier;

        var dmg = (int)((level + 1) * classConfiguration.WeaponMultiplier);

        var petFightable = new PetFightable
        {
            Class = classType,
            Level = level,
            Armor = (int)armor,
            Strength = (int)strength,
            Dexterity = (int)dexterity,
            Intelligence = (int)intelligence,
            Constitution = (int)mainAndConStat,
            Luck = (int)luckStat,
            CritMultiplier = 2 + (0.11 * gladiatorLevel),
            FirstWeapon = new() { MinDmg = dmg, MaxDmg = dmg, RuneType = RuneType.None, RuneValue = 0 },
            Position = position,
            ElementType = elementType,
        };

        return petFightable;
    }

    private static List<int> MainAttributePerLevel =
    [
        10,
        11,
        12,
        13,
        14,

        16,
        18,
        20,
        25,
        30,

        35,
        40,
        50,
        60,
        70,

        80,
        100,
        130,
        160,
        160
    ];

    private static List<decimal> LuckAttributePerLevel =
    [
        7.5M,
        8.5M,
        9.0M,
        9.5M,
        10.5M,

        12.0M,
        13.5M,
        15.0M,
        19.0M,
        22.5M,

        26.0M,
        30.0M,
        37.5M,
        45.0M,
        52.5M,

        60.0M,
        75M,
        97.5M,
        120M,
        120
    ];

    #region Pet Dungeons
    private static (ClassType Class, int Level)[] Shadow =
    [
        (ClassType.Scout, 1),
        (ClassType.Warrior, 3),
        (ClassType.Warrior, 6),
        (ClassType.Mage, 10),
        (ClassType.Mage, 14),
        (ClassType.Mage, 18),
        (ClassType.Scout, 22),
        (ClassType.Scout, 26),
        (ClassType.Scout, 30),
        (ClassType.Warrior, 34),
        (ClassType.Mage, 38),
        (ClassType.Scout, 42),
        (ClassType.Scout, 46),
        (ClassType.Scout, 50),
        (ClassType.Warrior, 54),
        (ClassType.Warrior, 58),
        (ClassType.Mage, 62),
        (ClassType.Warrior, 66),
        (ClassType.Warrior, 70),
        (ClassType.Scout, 75),
    ];

    private static (ClassType Class, int Level)[] Light =
    [
        (ClassType.Warrior, 1),
        (ClassType.Warrior, 3),
        (ClassType.Mage, 6),
        (ClassType.Mage, 10),
        (ClassType.Scout, 14),
        (ClassType.Scout, 18),
        (ClassType.Mage, 22),
        (ClassType.Warrior, 26),
        (ClassType.Warrior, 30),
        (ClassType.Mage, 34),
        (ClassType.Mage, 38),
        (ClassType.Scout, 42),
        (ClassType.Scout, 46),
        (ClassType.Mage, 50),
        (ClassType.Mage, 54),
        (ClassType.Warrior, 58),
        (ClassType.Warrior, 62),
        (ClassType.Warrior, 66),
        (ClassType.Mage, 70),
        (ClassType.Scout, 75),
    ];

    private static (ClassType Class, int Level)[] Earth =
    [
        (ClassType.Warrior, 1),
        (ClassType.Warrior, 3),
        (ClassType.Scout, 6),
        (ClassType.Scout, 10),
        (ClassType.Warrior, 14),
        (ClassType.Scout, 18),
        (ClassType.Mage, 22),
        (ClassType.Mage, 26),
        (ClassType.Warrior, 30),
        (ClassType.Warrior, 34),
        (ClassType.Scout, 38),
        (ClassType.Warrior, 42),
        (ClassType.Scout, 46),
        (ClassType.Scout, 50),
        (ClassType.Mage, 54),
        (ClassType.Mage, 58),
        (ClassType.Mage, 62),
        (ClassType.Warrior, 66),
        (ClassType.Warrior, 70),
        (ClassType.Warrior, 75),
    ];

    private static (ClassType Class, int Level)[] Fire =
    [
        (ClassType.Scout, 1),
        (ClassType.Scout, 3),
        (ClassType.Warrior, 6),
        (ClassType.Mage, 10),
        (ClassType.Mage, 14),
        (ClassType.Scout, 18),
        (ClassType.Scout, 22),
        (ClassType.Mage, 26),
        (ClassType.Warrior, 30),
        (ClassType.Mage, 34),
        (ClassType.Mage, 38),
        (ClassType.Scout, 42),
        (ClassType.Scout, 46),
        (ClassType.Scout, 50),
        (ClassType.Scout, 54),
        (ClassType.Scout, 58),
        (ClassType.Mage, 62),
        (ClassType.Warrior, 66),
        (ClassType.Mage, 70),
        (ClassType.Warrior, 75),
    ];

    private static (ClassType Class, int Level)[] Water =
    [
        (ClassType.Mage, 1),
        (ClassType.Warrior, 3),
        (ClassType.Warrior, 6),
        (ClassType.Warrior, 10),
        (ClassType.Warrior, 14),
        (ClassType.Scout, 18),
        (ClassType.Warrior, 22),
        (ClassType.Scout, 26),
        (ClassType.Scout, 30),
        (ClassType.Warrior, 34),
        (ClassType.Mage, 38),
        (ClassType.Mage, 42),
        (ClassType.Mage, 46),
        (ClassType.Warrior, 50),
        (ClassType.Mage, 54),
        (ClassType.Mage, 58),
        (ClassType.Warrior, 62),
        (ClassType.Mage, 66),
        (ClassType.Warrior, 70),
        (ClassType.Scout, 75),
    ];
    #endregion
}