using Microsoft.VisualStudio.TestTools.UnitTesting;
using SFSimulator.Core;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SFSimulator.Tests;

[TestClass]
public class DungeonSimulatorTests
{
    [DataRow(PetElementType.Shadow, 1, 1, 1, 1, 0, 0, 0, 0, 0.5)]
    [DataRow(PetElementType.Light, 5, 11, 66, 5, 2, 0, 0, 4, 0.1842)]
    [DataRow(PetElementType.Earth, 14, 20, 100, 14, 2, 2, 0, 8, 0.0083)]
    [DataRow(PetElementType.Fire, 15, 19, 100, 13, 1, 0, 1, 12, 0.0273)]
    [DataRow(PetElementType.Water, 14, 20, 100, 13, 2, 0, 0, 15, 0.0014)]
    [TestMethod]
    public async Task SimulatePetDungeon_yields_correct_win_ratio(PetElementType element, int position, int dungeonPosition, int level,
        int pack, int petsWithLevel100, int petsWithLevel150, int petsWithLevel200, int gladiatorLevel, double expectedWinRatio)
    {
        var dungeonSimulator = DependencyProvider.Get<IDungeonSimulator>();
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
                var result = dungeonSimulator.SimulatePetDungeon(dungeonPet, playerPet, 1, iterations, iterations);
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
    [DataRow(4, 6, 64, 1766, 225, 230, 1359, 641, 2236, 94, 102, true, 0.3365)]
    [DataRow(3, 9, 64, 1766, 225, 230, 1359, 641, 2236, 94, 102, true, 0.2459)]
    [DataRow(5, 8, 64, 2500, 225, 230, 2000, 641, 2236, 100, 200, true, 0.1025)]
    public void SimulateDungeon_yields_correct_win_ratio(int dungeonPosition, int dungeonEnemyPosition, int level, int strength, int dexterity,
        int intelligence, int constitution, int luck, int armor, int minDmg, int maxDmg, bool hasEternity, double expectedWinRatio)
    {
        var dungeonSimulator = DependencyProvider.Get<IDungeonSimulator>();
        var simulationContext = new SimulationContext();
        simulationContext.Class = ClassType.Paladin;
        simulationContext.Level = level;
        simulationContext.BaseStrength = strength;
        simulationContext.BaseDexterity = dexterity;
        simulationContext.BaseIntelligence = intelligence;
        simulationContext.BaseConstitution = constitution;
        simulationContext.BaseLuck = luck;
        simulationContext.Items =
        [
            new () { ItemType = ItemType.Weapon, MinDmg = minDmg, MaxDmg = maxDmg},
            new () { Armor = armor, ItemType = ItemType.Breastplate}
        ];
        if (hasEternity)
        {
            simulationContext.Potions = [Potion.Eternity];
        }

        var dungeonProvider = DependencyProvider.Get<IDungeonProvider>();
        var dungeon = dungeonProvider.GetDungeonEnemy(dungeonPosition, dungeonEnemyPosition);

        var iterations = 2_000_000;
        var result = dungeonSimulator.SimulateDungeon<EquipmentItem, EquipmentItem>(dungeon, simulationContext, [],
            iterations, iterations);
        var winRatio = (double)result.WonFights / iterations;

        Assert.AreEqual(expectedWinRatio, winRatio, 0.005,
             $"Expected win ratio: {expectedWinRatio} but actual was {winRatio}, won fights: {result.WonFights}");
    }
}
