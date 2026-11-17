using Microsoft.VisualStudio.TestTools.UnitTesting;
using SFSimulator.Core;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SFSimulator.Tests;

[TestClass]
public class DungeonSimulatorTests
{
    private readonly IDungeonProvider DungeonProvider = DependencyProvider.Get<IDungeonProvider>();
    private readonly IDungeonSimulator DungeonSimulator = DependencyProvider.Get<IDungeonSimulator>();

    [DataRow(PetElementType.Shadow, 1, 1, 1, 1, 0, 0, 0, 0, 0.5)]
    [DataRow(PetElementType.Light, 5, 11, 66, 5, 2, 0, 0, 4, 0.1842)]
    [DataRow(PetElementType.Earth, 14, 20, 100, 14, 2, 2, 0, 8, 0.0083)]
    [DataRow(PetElementType.Fire, 15, 19, 100, 13, 1, 0, 1, 12, 0.0273)]
    [DataRow(PetElementType.Water, 14, 20, 100, 13, 2, 0, 0, 15, 0.0014)]
    [TestMethod]
    public async Task SimulatePetDungeon_yields_correct_win_ratio(PetElementType element, int position, int dungeonPosition, int level,
        int pack, int petsWithLevel100, int petsWithLevel150, int petsWithLevel200, int gladiatorLevel, double expectedWinRatio)
    {
        var petFightableFactory = DependencyProvider.Get<IPetFightableFactory>();

        var petsState = new PetsState();

        var selectedPet = petsState.Elements[element].First(p => p.Position == position);
        selectedPet.Level = level;
        selectedPet.IsObtained = true;

        foreach (var pet in petsState.Elements[element].Where(p => p.Position != position).Take(pack - 1))
        {
            pet.Level = 1;
            pet.IsObtained = true;
            pet.IsDefeated = true;
        }

        if (level >= 100 && level <= 149)
        {
            petsWithLevel100--;
        }

        foreach (var pet in petsState.AllPets.Where(p => p.Level == 1).Take(petsWithLevel100))
        {
            pet.Level = 100;
        }

        if (level >= 150 && level <= 199)
        {
            petsWithLevel150--;
        }

        foreach (var pet in petsState.AllPets.Where(p => p.Level == 1).Take(petsWithLevel150))
        {
            pet.Level = 150;
        }

        if (level == 200)
        {
            petsWithLevel200--;
        }

        foreach (var pet in petsState.AllPets.Where(p => p.Level == 1).Take(petsWithLevel200))
        {
            pet.Level = 200;
        }

        var dungeonPet = petFightableFactory.CreateDungeonPetFightable(element, dungeonPosition);
        var playerPet = petFightableFactory.CreatePetFightable(selectedPet, petsState, gladiatorLevel);

        var iterations = 1_000_000;
        var threads = 5D;
        var results = new ConcurrentBag<PetSimulationResult>();
        var tasks = new List<Task>();
        for (int i = 0; i < threads; i++)
        {
            var task = Task.Run(() =>
            {
                var result = DungeonSimulator.SimulatePetDungeon(dungeonPet, playerPet, 1,
                    new(iterations, iterations, false));
                results.Add(result);
            });

            tasks.Add(task);
        }

        await Task.WhenAll(tasks);

        var wonFights = results.Sum(r => r.WonFights);

        var winRatio = wonFights / (iterations * threads);

        Assert.AreEqual(expectedWinRatio, winRatio, 0.005, $"Expected win ratio: {expectedWinRatio} but actual was {winRatio}, won fights: {wonFights}");
    }

    // Here we mainly care about opponents being properly generated. 
    // Sim logic should be checked with the arena fights test cases further below
    [TestMethod]
    // [DataRow(ClassType.Warrior, 3, 9, 0.3925)]
    // [DataRow(ClassType.Warrior, 4, 6, 0.4554)]
    // [DataRow(ClassType.Warrior, 5, 8, 0.0599)]
    // [DataRow(ClassType.Warrior, 25, 4, 0.0139)]
    // [DataRow(ClassType.Warrior, 25, 8, 0.0392)]
    // [DataRow(ClassType.Warrior, 24, 7, 0.2613)]
    // [DataRow(ClassType.Warrior, 24, 7, 0.2613)]
    // [DataRow(ClassType.Mage, 3, 9, 0.5717)]
    // [DataRow(ClassType.Mage, 4, 6, 0.7964)]
    // [DataRow(ClassType.Mage, 5, 8, 0.2204)]
    // [DataRow(ClassType.Mage, 25, 4, 0.0310)]
    // [DataRow(ClassType.Mage, 25, 8, 0.2365)]
    // [DataRow(ClassType.Mage, 24, 7, 0.1771)]
    // [DataRow(ClassType.Scout, 3, 9, 0.4673)]
    // [DataRow(ClassType.Scout, 4, 6, 0.7829)]
    // [DataRow(ClassType.Scout, 5, 8, 0.0767)]
    // [DataRow(ClassType.Scout, 25, 4, 0.1302)]
    // [DataRow(ClassType.Scout, 25, 8, 0.1609)]
    // [DataRow(ClassType.Scout, 24, 7, 0.2100)]
    // [DataRow(ClassType.Assassin, 3, 9, 0.4708)]
    // [DataRow(ClassType.Assassin, 4, 6, 0.8061)]
    // [DataRow(ClassType.Assassin, 5, 8, 0.0861)]
    // [DataRow(ClassType.Assassin, 25, 4, 0.1578)]
    // [DataRow(ClassType.Assassin, 25, 8, 0.2334)]
    // [DataRow(ClassType.Assassin, 24, 7, 0.2731)]
    // [DataRow(ClassType.BattleMage, 3, 9, 0.3695)]
    // [DataRow(ClassType.BattleMage, 4, 6, 0.4831)]
    // [DataRow(ClassType.BattleMage, 5, 8, 0.0724)]
    // [DataRow(ClassType.BattleMage, 25, 4, 0.0051)]
    // [DataRow(ClassType.BattleMage, 25, 8, 0.0010)]
    // [DataRow(ClassType.BattleMage, 24, 7, 0.1884)]
    // [DataRow(ClassType.Berserker, 3, 9, 0.6724)]
    // [DataRow(ClassType.Berserker, 4, 6, 0.6255)]
    // [DataRow(ClassType.Berserker, 5, 8, 0.0776)]
    // [DataRow(ClassType.Berserker, 25, 4, 0.0772)]
    // [DataRow(ClassType.Berserker, 25, 8, 0.2065)]
    // [DataRow(ClassType.Berserker, 24, 7, 0.3815)]
    [DataRow(ClassType.DemonHunter, 3, 9, 0.3628)]
    [DataRow(ClassType.DemonHunter, 4, 6, 0.6048)]
    [DataRow(ClassType.DemonHunter, 5, 8, 0.0767)]
    [DataRow(ClassType.DemonHunter, 25, 4, 0.1632)]
    [DataRow(ClassType.DemonHunter, 25, 8, 0.2662)]
    [DataRow(ClassType.DemonHunter, 24, 7, 0.2127)]
    // [DataRow(ClassType.Druid, 3, 9, 0.3396)]
    // [DataRow(ClassType.Druid, 4, 6, 0.4801)]
    // [DataRow(ClassType.Druid, 5, 8, 0.1235)]
    // [DataRow(ClassType.Druid, 25, 4, 0.0944)]
    // [DataRow(ClassType.Druid, 25, 8, 0.1538)]
    // [DataRow(ClassType.Druid, 24, 7, 0.2646)]
    // [DataRow(ClassType.Bard, 3, 9, 0.2435)]
    // [DataRow(ClassType.Bard, 4, 6, 0.4705)]
    // [DataRow(ClassType.Bard, 5, 8, 0.2344)]
    // [DataRow(ClassType.Bard, 25, 4, 0.0556)]
    // [DataRow(ClassType.Bard, 25, 8, 0.0512)]
    // [DataRow(ClassType.Bard, 24, 7, 0.1849)]
    // [DataRow(ClassType.Necromancer, 3, 9, 0.4997)]
    // [DataRow(ClassType.Necromancer, 4, 6, 0.6338)]
    // [DataRow(ClassType.Necromancer, 5, 8, 0.1461)]
    // [DataRow(ClassType.Necromancer, 25, 4, 0.0884)]
    // [DataRow(ClassType.Necromancer, 25, 8, 0.1619)]
    // [DataRow(ClassType.Necromancer, 24, 7, 0.2594)]
    // [DataRow(ClassType.Paladin, 3, 9, 0.6723)]
    // [DataRow(ClassType.Paladin, 4, 6, 0.6184)]
    // [DataRow(ClassType.Paladin, 5, 8, 0.0773)]
    // [DataRow(ClassType.Paladin, 25, 4, 0.0392)]
    // [DataRow(ClassType.Paladin, 25, 8, 0.1488)]
    // [DataRow(ClassType.Paladin, 24, 7, 0.3668)]
    // [DataRow(ClassType.PlagueDoctor, 3, 9, 0.4872)]
    // [DataRow(ClassType.PlagueDoctor, 4, 6, 0.7878)]
    // [DataRow(ClassType.PlagueDoctor, 5, 8, 0.0751)]
    // [DataRow(ClassType.PlagueDoctor, 25, 4, 0.1542)]
    // [DataRow(ClassType.PlagueDoctor, 25, 8, 0.1706)]
    // [DataRow(ClassType.PlagueDoctor, 24, 7, 0.2300)]
    public void SimulateDungeon_yields_correct_win_ratio_for_light_world_fights(ClassType @class, int dungeonPosition, int dungeonEnemyPosition, double expectedWinRatio)
    {
        DrHouse.IsDebugging = true;
        var dungeon = DungeonProvider.GetDungeonEnemy(dungeonPosition, dungeonEnemyPosition);
        var simulationContext = GetSimulationContextFor(@class, dungeonPosition, dungeonEnemyPosition);

        var iterations = 20_000_000;
        var result = DungeonSimulator.SimulateDungeon<EquipmentItem, EquipmentItem>(dungeon, simulationContext, [],
            new(iterations, iterations, false));

        var winRatio = (double)result.WonFights / iterations;
        DrHouse.Differential($"Class {@class}, actual WR: {winRatio}, expected {expectedWinRatio}");

        Assert.AreEqual(expectedWinRatio, winRatio, 0.001,
             $"Simulation for {@class} class for enemy {dungeonPosition}:{dungeonEnemyPosition} failed.\r\n" +
             $"Expected win ratio: {expectedWinRatio} but actual was {winRatio}, won fights: {result.WonFights}");
    }

    private static SimulationContext GetSimulationContextFor(ClassType @class, int dungeon, int enemy)
    {
        return (dungeon, enemy) switch
        {
            // Warrior dung
            (3, 9) => PrepareSimulationContext(@class, 64, 1766, 1359, 641, 225, 230, false,
            RuneType.None, 0, 0, 0, 0, 0, 0, 0, true, 0, false),
            // Scout dung
            (4, 6) => PrepareSimulationContext(@class, 64, 1766, 1359, 641, 225, 230, false,
            RuneType.None, 0, 0, 0, 0, 0, 0, 0, true, 0, false),
            // Mage dung
            (5, 8) => PrepareSimulationContext(@class, 64, 2500, 2000, 641, 225, 230, true,
            RuneType.None, 0, 0, 0, 0, 0, 0, 0, true, 0, true),
            // Assassin dung
            (25, 4) => PrepareSimulationContext(@class, 400, 32_000, 32_000, 10_000, 1000, 1000, true,
            RuneType.ColdDamage, 30, 0, 0, 0, 25, 25, 5, true, 10, true),
            // Battle Mage dung
            (25, 8) => PrepareSimulationContext(@class, 400, 32_000, 32_000, 10_000, 1000, 1000, true,
            RuneType.FireDamage, 60, 75, 75, 75, 50, 50, 15, true, 15, true),
            // Berserker dung
            (24, 7) => PrepareSimulationContext(@class, 400, 32_000, 32_000, 10_000, 1000, 1000, true,
            RuneType.LightningDamage, 20, 35, 35, 35, 40, 40, 15, true, 15, true),
            _ => throw new ArgumentOutOfRangeException($"Simulation context for dungeon {dungeon} and enemy {enemy} is not implemented")
        };
    }

    private static SimulationContext PrepareSimulationContext(ClassType @class, int level, int main, int con, int luck,
        int side1, int side2, bool weaponScroll, RuneType weaponRune, int weaponRuneValue,
        int fireRes, int coldRes, int lightningRes, int soloPortal, int guildPortal, int runeHealth, bool lifePotion, int gladiator, bool gloveScroll)
    {
        var simulationContext = new SimulationContext();

        var mainAttribute = ClassConfigurationProvider.Get(@class).MainAttribute;
        var (str, dex, @int) = mainAttribute switch
        {
            AttributeType.Strength => (main, side1, side2),
            AttributeType.Dexterity => (side1, main, side2),
            AttributeType.Intelligence => (side1, side2, main),
            _ => throw new ArgumentOutOfRangeException($"{@class}'s main attribute is not supported")
        };
        simulationContext.Class = @class;
        simulationContext.Level = level;
        simulationContext.BaseStrength = str;
        simulationContext.BaseDexterity = dex;
        simulationContext.BaseIntelligence = @int;
        simulationContext.BaseConstitution = con;
        simulationContext.BaseLuck = luck;
        simulationContext.GladiatorLevel = gladiator;
        simulationContext.SoloPortal = soloPortal;
        simulationContext.GuildPortal = guildPortal;

        var equipmentBuilder = new EquipmentBuilder(ItemAttributeType.Epic, level, @class,
            0, 0, ItemType.Weapon);
        equipmentBuilder.WithRune(weaponRune, weaponRuneValue);
        if (weaponScroll)
        {
            equipmentBuilder.WithEnchantment();
        }
        var weapon = equipmentBuilder.AsWeapon().Build();

        simulationContext.Items =
        [
            weapon,
            new () { Armor = GetArmorFor(@class, level), ItemType = ItemType.Breastplate },
            new () { ItemType = ItemType.Gloves, ScrollType = gloveScroll ? WitchScrollType.Reaction : WitchScrollType.None },
            new () { ItemType = ItemType.Trinket, RuneType = RuneType.FireResistance, RuneValue = fireRes },
            new () { ItemType = ItemType.Amulet, RuneType = RuneType.ColdResistance, RuneValue = coldRes },
            new () { ItemType = ItemType.Ring, RuneType = RuneType.LightningResistance, RuneValue = lightningRes },
            new () { ItemType = ItemType.Headgear, RuneType = RuneType.HealthBonus, RuneValue = runeHealth },
        ];
        if (@class == ClassType.Assassin)
        {
            simulationContext.Items.Add(weapon);
        }
        if (lifePotion)
        {
            simulationContext.Potions = [Potion.Eternity];
        }

        return simulationContext;
    }

    private static int GetArmorFor(ClassType @class, int level)
    {
        var classConfig = ClassConfigurationProvider.Get(@class);
        var dmgReduction = classConfig.MaxArmorReduction * 0.8;

        return (int)Math.Floor(dmgReduction * 100 * level / classConfig.ArmorMultiplier);
    }

    [TestMethod]
    [DataRow(ClassType.Warrior, ClassType.Mage, 0.5000)]
    [DataRow(ClassType.Warrior, ClassType.Scout, 0.4889)]
    [DataRow(ClassType.Warrior, ClassType.Assassin, 0.4821)]
    [DataRow(ClassType.Warrior, ClassType.BattleMage, 0.5034)]
    [DataRow(ClassType.Warrior, ClassType.Berserker, 0.4909)]
    [DataRow(ClassType.Warrior, ClassType.DemonHunter, 0.4946)]
    [DataRow(ClassType.Warrior, ClassType.Druid, 0.5224)]
    [DataRow(ClassType.Warrior, ClassType.Bard, 0.3900)]
    [DataRow(ClassType.Warrior, ClassType.Necromancer, 0.5093)]
    [DataRow(ClassType.Warrior, ClassType.Paladin, 0.4836)]
    [DataRow(ClassType.Warrior, ClassType.PlagueDoctor, 0.4920)]
    [DataRow(ClassType.Warrior, ClassType.BloodWeaver, 0.4916)]
    [DataRow(ClassType.Mage, ClassType.Scout, 0.5000)]
    [DataRow(ClassType.Mage, ClassType.Assassin, 0.4895)]
    [DataRow(ClassType.Mage, ClassType.BattleMage, 0.5000)]
    [DataRow(ClassType.Mage, ClassType.Berserker, 0.5000)]
    [DataRow(ClassType.Mage, ClassType.DemonHunter, 0.5000)]
    [DataRow(ClassType.Mage, ClassType.Druid, 0.5000)]
    [DataRow(ClassType.Mage, ClassType.Bard, 0.4876)]
    [DataRow(ClassType.Mage, ClassType.Necromancer, 0.5000)]
    [DataRow(ClassType.Mage, ClassType.Paladin, 0.5000)]
    [DataRow(ClassType.Mage, ClassType.PlagueDoctor, 0.5000)]
    [DataRow(ClassType.Mage, ClassType.BloodWeaver, 0.5000)]
    [DataRow(ClassType.Scout, ClassType.Assassin, 0.4910)]
    [DataRow(ClassType.Scout, ClassType.BattleMage, 0.5183)]
    [DataRow(ClassType.Scout, ClassType.Berserker, 0.5003)]
    [DataRow(ClassType.Scout, ClassType.DemonHunter, 0.4933)]
    [DataRow(ClassType.Scout, ClassType.Druid, 0.5418)]
    [DataRow(ClassType.Scout, ClassType.Bard, 0.4569)]
    [DataRow(ClassType.Scout, ClassType.Necromancer, 0.5344)]
    [DataRow(ClassType.Scout, ClassType.Paladin, 0.5006)]
    [DataRow(ClassType.Scout, ClassType.PlagueDoctor, 0.5082)]
    [DataRow(ClassType.Scout, ClassType.BloodWeaver, 0.5135)]
    [DataRow(ClassType.Assassin, ClassType.BattleMage, 0.5240)]
    [DataRow(ClassType.Assassin, ClassType.Berserker, 0.5125)]
    [DataRow(ClassType.Assassin, ClassType.DemonHunter, 0.4992)]
    [DataRow(ClassType.Assassin, ClassType.Druid, 0.5253)]
    [DataRow(ClassType.Assassin, ClassType.Bard, 0.4784)]
    [DataRow(ClassType.Assassin, ClassType.Necromancer, 0.5433)]
    [DataRow(ClassType.Assassin, ClassType.Paladin, 0.5017)]
    [DataRow(ClassType.Assassin, ClassType.PlagueDoctor, 0.5095)]
    [DataRow(ClassType.Assassin, ClassType.BloodWeaver, 0.5039)]
    [DataRow(ClassType.BattleMage, ClassType.Berserker, 0.4849)]
    [DataRow(ClassType.BattleMage, ClassType.DemonHunter, 0.4933)]
    [DataRow(ClassType.BattleMage, ClassType.Druid, 0.5150)]
    [DataRow(ClassType.BattleMage, ClassType.Bard, 0.3813)]
    [DataRow(ClassType.BattleMage, ClassType.Necromancer, 0.5124)]
    [DataRow(ClassType.BattleMage, ClassType.Paladin, 0.4754)]
    [DataRow(ClassType.BattleMage, ClassType.PlagueDoctor, 0.4890)]
    [DataRow(ClassType.BattleMage, ClassType.BloodWeaver, 0.5109)]
    [DataRow(ClassType.Berserker, ClassType.DemonHunter, 0.4846)]
    [DataRow(ClassType.Berserker, ClassType.Druid, 0.5073)]
    [DataRow(ClassType.Berserker, ClassType.Bard, 0.4250)]
    [DataRow(ClassType.Berserker, ClassType.Necromancer, 0.5101)]
    [DataRow(ClassType.Berserker, ClassType.Paladin, 0.4786)]
    [DataRow(ClassType.Berserker, ClassType.PlagueDoctor, 0.4691)]
    [DataRow(ClassType.Berserker, ClassType.BloodWeaver, 0.4748)]
    [DataRow(ClassType.DemonHunter, ClassType.Druid, 0.4751)]
    [DataRow(ClassType.DemonHunter, ClassType.Bard, 0.4721)]
    [DataRow(ClassType.DemonHunter, ClassType.Necromancer, 0.4505)]
    [DataRow(ClassType.DemonHunter, ClassType.Paladin, 0.5050)]
    [DataRow(ClassType.DemonHunter, ClassType.PlagueDoctor, 0.4721)]
    [DataRow(ClassType.DemonHunter, ClassType.BloodWeaver, 0.5172)]
    [DataRow(ClassType.Druid, ClassType.Bard, 0.3854)]
    [DataRow(ClassType.Druid, ClassType.Necromancer, 0.4865)]
    [DataRow(ClassType.Druid, ClassType.Paladin, 0.4694)]
    [DataRow(ClassType.Druid, ClassType.PlagueDoctor, 0.4911)]
    [DataRow(ClassType.Druid, ClassType.BloodWeaver, 0.4395)]
    [DataRow(ClassType.Bard, ClassType.Necromancer, 0.5854)]
    [DataRow(ClassType.Bard, ClassType.Paladin, 0.5778)]
    [DataRow(ClassType.Bard, ClassType.PlagueDoctor, 0.5399)]
    [DataRow(ClassType.Bard, ClassType.BloodWeaver, 0.5107)]
    [DataRow(ClassType.Necromancer, ClassType.Paladin, 0.4764)]
    [DataRow(ClassType.Necromancer, ClassType.PlagueDoctor, 0.4647)]
    [DataRow(ClassType.Necromancer, ClassType.BloodWeaver, 0.4810)]
    [DataRow(ClassType.Paladin, ClassType.PlagueDoctor, 0.5008)]
    [DataRow(ClassType.Paladin, ClassType.BloodWeaver, 0.5020)]
    [DataRow(ClassType.PlagueDoctor, ClassType.BloodWeaver, 0.5125)]
    public void SimulateArenaFight_Returns_Expected_WinRatio(ClassType @class,
        ClassType opponentClass, double expectedWinRatio)
    {
        var left = InitSimulationContext(@class);
        var right = InitSimulationContext(opponentClass);

        var options = new DungeonSimulationOptions(10_000_000, 2_000_000, false);
        
        var result = DungeonSimulator.SimulateArenaFight(left, right, options);

        var actualWinRatio = (double)result.WonFights / options.Iterations;
        Assert.AreEqual(expectedWinRatio, actualWinRatio, 0.001,
            $"Match up: {@class} vs {opponentClass} had unexpected delta of {Math.Abs(actualWinRatio - expectedWinRatio)}");
    }

    private static SimulationContext InitSimulationContext(ClassType @class)
    {

        var config = ClassConfigurationProvider.Get(@class);
        var (str, dex, @int) = config.MainAttribute switch
        {
            AttributeType.Strength => (100_000, 4_000, 4_000),
            AttributeType.Dexterity => (4_000, 100_000, 4_000),
            AttributeType.Intelligence => (4_000, 4_000, 100_000),
            _ => throw new ArgumentException($"{@class} has unsupported main attribute of {config.MainAttribute}")
        };

        var simulationContext = new SimulationContext
        {
            Level = 500,
            Class = @class,
            GladiatorLevel = 0,
            SoloPortal = 50,
            GuildPortal = 50,
            Potions = [Potion.Eternity],
            BaseStrength = str,
            BaseDexterity = dex,
            BaseIntelligence = @int,
            BaseConstitution = 100_000,
            BaseLuck = 30_000,
            Items = [
                new()
                {
                    RuneType = RuneType.FireResistance,
                    RuneValue = 75,
                    ItemType = ItemType.Headgear
                },
                new()
                {
                    RuneType = RuneType.LightningResistance,
                    RuneValue = 75,
                    ItemType = ItemType.Breastplate
                },
                new()
                {
                    RuneType = RuneType.ColdResistance,
                    RuneValue = 75,
                    ItemType = ItemType.Gloves,
                    ScrollType = WitchScrollType.Reaction
                }
            ]
        };


        var armor = (int)(config.MaxArmorReduction * 100 * simulationContext.Level / config.ArmorMultiplier * 2);
        simulationContext.Items.Add(new()
        {
            Armor = armor,
            RuneType = RuneType.HealthBonus,
            RuneValue = 15,
            ItemType = ItemType.Boots,
        });
        var weapon = new EquipmentItem()
        {
            RuneType = RuneType.FireDamage,
            RuneValue = 60,
            ItemType = ItemType.Weapon,
            ScrollType = WitchScrollType.Crit,
            MinDmg = (int)(700 / 2d * config.WeaponMultiplier),
            MaxDmg = (int)(2100 / 2d * config.WeaponMultiplier)
        };

        simulationContext.Items.Add(weapon);
        if (@class == ClassType.Assassin)
        {
            simulationContext.Items.Add(weapon);
        }

        return simulationContext;
    }
}