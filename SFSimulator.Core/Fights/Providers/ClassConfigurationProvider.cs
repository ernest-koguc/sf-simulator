namespace SFSimulator.Core;
public static class ClassConfigurationProvider
{
    private static Dictionary<ClassType, ClassConfiguration> ClassConfiguration { get; set; } = null!;

    public static ClassConfiguration GetClassConfiguration(ClassType classType)
    {
        if (ClassConfiguration == null)
            InitClassConfiguration();

        if (!ClassConfiguration!.ContainsKey(classType))
            throw new ArgumentException($"No configuration for class: {classType}");

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
                }
            },
            {
                ClassType.ShieldlessWarrior,
                new ClassConfiguration
                {
                    HealthMultiplier = 5,
                    WeaponMultiplier = 2,
                }
            },
            {
                ClassType.Mage,
                new ClassConfiguration
                {
                    HealthMultiplier = 2,
                    WeaponMultiplier = 4.5,
                }
            },
            {
                ClassType.Scout,
                new ClassConfiguration
                {
                    HealthMultiplier = 4,
                    WeaponMultiplier = 2.5,
                }
            },
            {
                ClassType.Assassin,
                new ClassConfiguration
                {
                    HealthMultiplier = 4,
                    WeaponMultiplier = 2,
                }
            },
            {
                ClassType.BattleMage,
                new ClassConfiguration
                {
                    HealthMultiplier = 5,
                    WeaponMultiplier = 2,
                }
            },
            {
                ClassType.Berserker,
                new ClassConfiguration
                {
                    HealthMultiplier = 4,
                    WeaponMultiplier = 2,
                }
            },
            {
                ClassType.Druid,
                new ClassConfiguration
                {
                    HealthMultiplier = 5,
                    WeaponMultiplier = 4.5,
                }
            },
            {
                ClassType.DemonHunter,
                new ClassConfiguration
                {
                    HealthMultiplier = 5,
                    WeaponMultiplier = 2.5,
                }
            },
            {
                ClassType.Bard,
                new ClassConfiguration
                {
                    HealthMultiplier = 3,
                    WeaponMultiplier = 4.5,
                }
            },

        };
    }

}
