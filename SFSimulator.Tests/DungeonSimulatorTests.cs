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

    [TestMethod]
    [DataRow(ClassType.Warrior, 3, 9, 0.4149)]
    [DataRow(ClassType.Warrior, 4, 6, 0.4697)]
    [DataRow(ClassType.Warrior, 5, 8, 0.0672)]
    [DataRow(ClassType.Warrior, 25, 4, 0.0142)]
    [DataRow(ClassType.Warrior, 25, 8, 0.0399)]
    [DataRow(ClassType.Warrior, 24, 7, 0.2634)]
    [DataRow(ClassType.Mage, 3, 9, 0.5914)]
    [DataRow(ClassType.Mage, 4, 6, 0.8084)]
    [DataRow(ClassType.Mage, 5, 8, 0.2353)]
    [DataRow(ClassType.Mage, 25, 4, 0.0316)]
    [DataRow(ClassType.Mage, 25, 8, 0.2384)]
    [DataRow(ClassType.Mage, 24, 7, 0.1786)]
    [DataRow(ClassType.Scout, 3, 9, 0.4794)]
    [DataRow(ClassType.Scout, 4, 6, 0.7900)]
    [DataRow(ClassType.Scout, 5, 8, 0.0834)]
    [DataRow(ClassType.Scout, 25, 4, 0.1311)]
    [DataRow(ClassType.Scout, 25, 8, 0.1619)]
    [DataRow(ClassType.Scout, 24, 7, 0.2112)]
    [DataRow(ClassType.Assassin, 3, 9, 0.4857)]
    [DataRow(ClassType.Assassin, 4, 6, 0.8156)]
    [DataRow(ClassType.Assassin, 5, 8, 0.0953)]
    [DataRow(ClassType.Assassin, 25, 4, 0.1584)]
    [DataRow(ClassType.Assassin, 25, 8, 0.2340)]
    [DataRow(ClassType.Assassin, 24, 7, 0.2739)]
    [DataRow(ClassType.BattleMage, 3, 9, 0.3956)]
    [DataRow(ClassType.BattleMage, 4, 6, 0.4974)]
    [DataRow(ClassType.BattleMage, 5, 8, 0.0807)]
    [DataRow(ClassType.BattleMage, 25, 4, 0.0052)]
    [DataRow(ClassType.BattleMage, 25, 8, 0.0011)]
    [DataRow(ClassType.BattleMage, 24, 7, 0.1904)]
    [DataRow(ClassType.Berserker, 3, 9, 0.6881)]
    [DataRow(ClassType.Berserker, 4, 6, 0.6359)]
    [DataRow(ClassType.Berserker, 5, 8, 0.0860)]
    [DataRow(ClassType.Berserker, 25, 4, 0.0778)]
    [DataRow(ClassType.Berserker, 25, 8, 0.2085)]
    [DataRow(ClassType.Berserker, 24, 7, 0.3833)]
    [DataRow(ClassType.DemonHunter, 3, 9, 0.3686)]
    [DataRow(ClassType.DemonHunter, 4, 6, 0.6130)]
    [DataRow(ClassType.DemonHunter, 5, 8, 0.0834)]
    [DataRow(ClassType.DemonHunter, 25, 4, 0.1641)]
    [DataRow(ClassType.DemonHunter, 25, 8, 0.2669)]
    [DataRow(ClassType.DemonHunter, 24, 7, 0.2135)]
    [DataRow(ClassType.Druid, 3, 9, 0.3507)]
    [DataRow(ClassType.Druid, 4, 6, 0.4887)]
    [DataRow(ClassType.Druid, 5, 8, 0.1364)]
    [DataRow(ClassType.Druid, 25, 4, 0.0952)]
    [DataRow(ClassType.Druid, 25, 8, 0.1550)]
    [DataRow(ClassType.Druid, 24, 7, 0.2658)]
    [DataRow(ClassType.Bard, 3, 9, 0.2570)]
    [DataRow(ClassType.Bard, 4, 6, 0.4800)]
    [DataRow(ClassType.Bard, 5, 8, 0.2498)]
    [DataRow(ClassType.Bard, 25, 4, 0.0562)]
    [DataRow(ClassType.Bard, 25, 8, 0.0518)]
    [DataRow(ClassType.Bard, 24, 7, 0.1863)]
    [DataRow(ClassType.Necromancer, 3, 9, 0.5143)]
    [DataRow(ClassType.Necromancer, 4, 6, 0.6431)]
    [DataRow(ClassType.Necromancer, 5, 8, 0.1594)]
    [DataRow(ClassType.Necromancer, 25, 4, 0.0891)]
    [DataRow(ClassType.Necromancer, 25, 8, 0.1633)]
    [DataRow(ClassType.Necromancer, 24, 7, 0.2601)]
    [DataRow(ClassType.Paladin, 4, 6, 0.6304)]
    [DataRow(ClassType.Paladin, 3, 9, 0.6904)]
    [DataRow(ClassType.Paladin, 5, 8, 0.0857)]
    [DataRow(ClassType.Paladin, 25, 4, 0.0398)]
    [DataRow(ClassType.Paladin, 25, 8, 0.1505)]
    [DataRow(ClassType.Paladin, 24, 7, 0.3690)]

    // NOT TESTED
    // DH vs DRUID <===== this one might be tricky
    // DH vs BARD
    // DH VS NECRO <===== this one might be tricky
    // DH VS PALA 
    // DRUID VS BARD
    // DRUID VS NECRO
    // DRUID VS PALA
    // BARD VS NECRO
    // BARD VS PALA
    // NECRO VS PALA
    public void SimulateDungeon_yields_correct_win_ratio_for_light_world_fights(ClassType @class, int dungeonPosition, int dungeonEnemyPosition, double expectedWinRatio)
    {
        DrHouse.IsDebugging = true;
        var dungeon = DungeonProvider.GetDungeonEnemy(dungeonPosition, dungeonEnemyPosition);
        var simulationContext = GetSimulationContextFor(@class, dungeonPosition, dungeonEnemyPosition);

        var iterations = 2_000_000;
        var result = DungeonSimulator.SimulateDungeon<EquipmentItem, EquipmentItem>(dungeon, simulationContext, [],
            new(iterations, iterations, false));

        var winRatio = (double)result.WonFights / iterations;
        DrHouse.Differential($"Class {@class}, actual WR: {winRatio}, expected {expectedWinRatio}");

        Assert.AreEqual(expectedWinRatio, winRatio, 0.005,
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
            // Mage dung
            RuneType.None, 0, 0, 0, 0, 0, 0, 0, true, 0, false),
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
}
