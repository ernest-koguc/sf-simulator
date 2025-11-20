namespace SFSimulator.Core;

public static class ClassConfigurationProvider
{
    private static Dictionary<ClassType, ClassConfiguration> ClassConfiguration { get; set; } = default!;

    static ClassConfigurationProvider()
    {
        InitClassConfiguration();
    }

    public static ClassConfiguration Get(ClassType classType)
    {
        if (!ClassConfiguration!.TryGetValue(classType, out var value))
            throw new ArgumentException($"Configuration for class {classType} is not supported");

        return value;
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
                    ItemBonusMultiplier = 1,
                    ArmorMultiplier = 1,
                    MaxArmorReduction = 0.5,
                    DamageMultiplier = 1,
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
                    ItemBonusMultiplier = 1,
                    ArmorMultiplier = 1,
                    MaxArmorReduction = 0.5,
                    DamageMultiplier = 1,
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
                    ItemBonusMultiplier = 1,
                    ArmorMultiplier = 1,
                    MaxArmorReduction = 0.1,
                    DamageMultiplier = 1,
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
                    ItemBonusMultiplier = 1,
                    ArmorMultiplier = 1,
                    MaxArmorReduction = 0.25,
                    DamageMultiplier = 1,
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
                    ItemBonusMultiplier = 1,
                    ArmorMultiplier = 1,
                    MaxArmorReduction = 0.25,
                    DamageMultiplier = 0.625,
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
                    ItemBonusMultiplier = 1.11,
                    ArmorMultiplier = 5,
                    MaxArmorReduction = 0.5,
                    DamageMultiplier = 1,
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
                    ItemBonusMultiplier = 1.1,
                    ArmorMultiplier = 0.5,
                    MaxArmorReduction = 0.25,
                    DamageMultiplier = 1.25,
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
                    ItemBonusMultiplier = 1,
                    ArmorMultiplier = 1,
                    MaxArmorReduction = 0.25,
                    DamageMultiplier = 1 / 3D,
                }
            },
            {
                ClassType.DemonHunter,
                new ClassConfiguration
                {
                    HealthMultiplier = 4,
                    WeaponMultiplier = 2.5,
                    WeaponGemMultiplier = 2,
                    WeaponAttributeMultiplier = 2,
                    ItemArmorMultiplier = 15,
                    MainAttribute = AttributeType.Dexterity,
                    ItemBonusMultiplier = 1,
                    ArmorMultiplier = 1,
                    MaxArmorReduction = 0.5,
                    DamageMultiplier = 1,
                }
            },
            {
                ClassType.Bard,
                new ClassConfiguration
                {
                    HealthMultiplier = 2,
                    WeaponMultiplier = 4.5,
                    WeaponGemMultiplier = 2,
                    WeaponAttributeMultiplier = 2,
                    ItemArmorMultiplier = 7.5,
                    MainAttribute = AttributeType.Intelligence,
                    ItemBonusMultiplier = 1,
                    ArmorMultiplier = 2,
                    MaxArmorReduction = 0.5,
                    DamageMultiplier = 1.125,
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
                    ItemBonusMultiplier = 1,
                    ArmorMultiplier = 2,
                    MaxArmorReduction = 0.2,
                    DamageMultiplier = 5D / 9D,
                }
            },
            {
                ClassType.Paladin,
                new ClassConfiguration
                {
                    HealthMultiplier = 6,
                    WeaponMultiplier = 2,
                    WeaponGemMultiplier = 2,
                    WeaponAttributeMultiplier = 1,
                    ItemArmorMultiplier = 15,
                    MainAttribute = AttributeType.Strength,
                    ItemBonusMultiplier = 1,
                    ArmorMultiplier = 1,
                    MaxArmorReduction = 0.45,
                    DamageMultiplier = 0.833D,
                }
            },
            {
                ClassType.PlagueDoctor,
                new ClassConfiguration
                {
                    HealthMultiplier = 4,
                    WeaponMultiplier = 2,
                    WeaponGemMultiplier = 2,
                    WeaponAttributeMultiplier = 1,
                    ItemArmorMultiplier = 3,
                    MainAttribute = AttributeType.Dexterity,
                    ItemBonusMultiplier = 1.11,
                    ArmorMultiplier = 2,
                    MaxArmorReduction = 0.20,
                    DamageMultiplier = 1.25D,
                }
            },
        };
    }
}