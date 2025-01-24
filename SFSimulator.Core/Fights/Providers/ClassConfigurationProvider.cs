namespace SFSimulator.Core;

public static class ClassConfigurationProvider
{
    private static Dictionary<ClassType, ClassConfiguration> ClassConfiguration { get; set; } = default!;

    static ClassConfigurationProvider()
    {
        InitClassConfiguration();
    }

    public static ClassConfiguration GetClassConfiguration(ClassType classType)
    {
        if (!ClassConfiguration!.ContainsKey(classType))
            throw new ArgumentException($"Configuration for class {classType} is not supported");

        return ClassConfiguration[classType];
    }
    private static void InitClassConfiguration()
    {
        ClassConfiguration = new()
        {
            {
                ClassType.Warrior,
                new ClassConfiguration
                {
                    HealthMultiplier = 5,
                    WeaponMultiplier = 2,
                    WeaponGemMultiplier = 1,
                    WeaponAttributeMultiplier = 1,
                    ItemArmorMultiplier = 15,
                    MainAttribute = AttributeType.Strength,
                    ItemBonusMultiplier = 1
                }
            },
            {
                ClassType.Bert,
                new ClassConfiguration
                {
                    HealthMultiplier = 6.1,
                    WeaponMultiplier = 2,
                    WeaponGemMultiplier = 1,
                    WeaponAttributeMultiplier = 1,
                    ItemArmorMultiplier = 15,
                    MainAttribute = AttributeType.Strength,
                    ItemBonusMultiplier = 1
                }
            },
            {
                ClassType.Mage,
                new ClassConfiguration
                {
                    HealthMultiplier = 2,
                    WeaponMultiplier = 4.5,
                    WeaponGemMultiplier = 2,
                    WeaponAttributeMultiplier = 2,
                    ItemArmorMultiplier = 3,
                    MainAttribute = AttributeType.Intelligence,
                    ItemBonusMultiplier = 1
                }
            },
            {
                ClassType.Scout,
                new ClassConfiguration
                {
                    HealthMultiplier = 4,
                    WeaponMultiplier = 2.5,
                    WeaponGemMultiplier = 2,
                    WeaponAttributeMultiplier = 2,
                    ItemArmorMultiplier = 7.5,
                    MainAttribute = AttributeType.Dexterity,
                    ItemBonusMultiplier = 1
                }
            },
            {
                ClassType.Assassin,
                new ClassConfiguration
                {
                    HealthMultiplier = 4,
                    WeaponMultiplier = 2,
                    WeaponGemMultiplier = 1,
                    WeaponAttributeMultiplier = 1,
                    ItemArmorMultiplier = 7.5,
                    MainAttribute = AttributeType.Dexterity,
                    ItemBonusMultiplier = 1
                }
            },
            {
                ClassType.BattleMage,
                new ClassConfiguration
                {
                    HealthMultiplier = 5,
                    WeaponMultiplier = 2,
                    WeaponGemMultiplier = 2,
                    WeaponAttributeMultiplier = 1,
                    ItemArmorMultiplier = 3,
                    MainAttribute = AttributeType.Strength,
                    ItemBonusMultiplier = 1.11
                }
            },
            {
                ClassType.Berserker,
                new ClassConfiguration
                {
                    HealthMultiplier = 4,
                    WeaponMultiplier = 2,
                    WeaponGemMultiplier = 1,
                    WeaponAttributeMultiplier = 1,
                    ItemArmorMultiplier = 15,
                    MainAttribute = AttributeType.Strength,
                    ItemBonusMultiplier = 1.1
                }
            },
            {
                ClassType.Druid,
                new ClassConfiguration
                {
                    HealthMultiplier = 5,
                    WeaponMultiplier = 4.5,
                    WeaponGemMultiplier = 2,
                    WeaponAttributeMultiplier = 2,
                    ItemArmorMultiplier = 7.5,
                    MainAttribute = AttributeType.Intelligence,
                    ItemBonusMultiplier = 1
                }
            },
            {
                ClassType.DemonHunter,
                new ClassConfiguration
                {
                    HealthMultiplier = 5,
                    WeaponMultiplier = 2.5,
                    WeaponGemMultiplier = 2,
                    WeaponAttributeMultiplier = 2,
                    ItemArmorMultiplier = 15,
                    MainAttribute = AttributeType.Dexterity,
                    ItemBonusMultiplier = 1
                }
            },
            {
                ClassType.Bard,
                new ClassConfiguration
                {
                    HealthMultiplier = 3,
                    WeaponMultiplier = 4.5,
                    WeaponGemMultiplier = 2,
                    WeaponAttributeMultiplier = 2,
                    ItemArmorMultiplier = 7.5,
                    MainAttribute = AttributeType.Intelligence,
                    ItemBonusMultiplier = 1
                }
            },
            {
                ClassType.Necromancer,
                new ClassConfiguration
                {
                    HealthMultiplier = 4,
                    WeaponMultiplier = 4.5,
                    WeaponGemMultiplier = 2,
                    WeaponAttributeMultiplier = 2,
                    ItemArmorMultiplier = 3,
                    MainAttribute = AttributeType.Intelligence,
                    ItemBonusMultiplier = 1
                }
            },
        };
    }
}