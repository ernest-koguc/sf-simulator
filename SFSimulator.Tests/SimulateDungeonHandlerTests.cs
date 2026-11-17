using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using SFSimulator.Api;
using SFSimulator.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SFSimulator.Tests;

[TestClass]
public class SimulateDungeonHandlerTests
{
    [TestMethod]
    [DataRow(0, 1)]
    [DataRow(1, 2)]
    [DataRow(2, 3)]
    [DataRow(3, 4)]
    [DataRow(4, 5)]
    [DataRow(5, 6)]
    [DataRow(6, 7)]
    [DataRow(7, 8)]
    [DataRow(8, 9)]
    [DataRow(9, 10)]
    [DataRow(10, 11)]
    [DataRow(11, 12)]
    [DataRow(12, 15)]
    [DataRow(13, 19)]
    [DataRow(15, 17)]
    [DataRow(16, 18)]
    [DataRow(18, 24)]
    [DataRow(19, 27)]
    [DataRow(20, 21)]
    [DataRow(21, 13)]
    [DataRow(22, 14)]
    [DataRow(23, 16)]
    [DataRow(24, 20)]
    [DataRow(25, 28)]
    [DataRow(26, 22)]
    [DataRow(27, 23)]
    [DataRow(28, 25)]
    [DataRow(29, 26)]
    [DataRow(32, 29)]
    [DataRow(33, 30)]
    [DataRow(34, 31)]
    [DataRow(35, 32)]
    [DataRow(36, 33)]
    public void HandleSimulateDungeon_maps_game_slots_to_light_and_shadow_dungeons(int gameIndex, int dungeonPosition)
    {
        foreach (var shadow in new[] { false, true })
        {
            var progress = CreateLockedProgress();
            progress[gameIndex] = 3;
            var state = shadow ? new DungeonState([], progress, []) : new DungeonState(progress, [], []);

            var enemy = GetSimulatedEnemies(state).Single();

            Assert.AreEqual(dungeonPosition + (shadow ? 100 : 0), enemy.Dungeon.Position);
            Assert.AreEqual(4, enemy.Position);
        }
    }

    [TestMethod]
    [DataRow(false, 14, -1)]
    [DataRow(true, 14, -2)]
    [DataRow(true, 17, -3)]
    [DataRow(false, 31, -4)]
    public void HandleSimulateDungeon_maps_special_dungeon_slots(bool shadow, int gameIndex, int dungeonPosition)
    {
        var progress = CreateLockedProgress();
        progress[gameIndex] = 3;
        var state = shadow ? new DungeonState([], progress, []) : new DungeonState(progress, [], []);

        var enemy = GetSimulatedEnemies(state).Single();

        Assert.AreEqual(dungeonPosition, enemy.Dungeon.Position);
        Assert.AreEqual(4, enemy.Position);
    }

    [TestMethod]
    [DataRow(0, 201)]
    [DataRow(1, 202)]
    [DataRow(2, 203)]
    [DataRow(3, 204)]
    public void HandleSimulateDungeon_maps_class_dungeons_independently_of_world_array_lengths(int gameIndex, int dungeonPosition)
    {
        var progress = Enumerable.Repeat(-2, 5).ToArray();
        progress[gameIndex] = 3;

        var enemy = GetSimulatedEnemies(new DungeonState([-2], [-2, -2], progress)).Single();

        Assert.AreEqual(dungeonPosition, enemy.Dungeon.Position);
        Assert.AreEqual(4, enemy.Position);
    }

    [TestMethod]
    public void HandleSimulateDungeon_ignores_portal_unused_and_unsupported_dungeon_slots()
    {
        var light = Enumerable.Repeat(-2, 38).ToArray();
        var shadow = Enumerable.Repeat(-2, 38).ToArray();
        light[17] = light[30] = light[37] = 0;
        shadow[30] = shadow[31] = shadow[37] = 0;

        var enemies = GetSimulatedEnemies(new DungeonState(light, shadow, [-2, -2, -2, -2, 0]));

        Assert.AreEqual(0, enemies.Count);
    }

    [TestMethod]
    [DataRow(-2)]
    [DataRow(-1)]
    [DataRow(1000)]
    public void HandleSimulateDungeon_skips_locked_unopened_and_completed_dungeons(int progress)
    {
        var enemies = GetSimulatedEnemies(new DungeonState(
            Enumerable.Repeat(progress, 37).ToArray(),
            Enumerable.Repeat(progress, 37).ToArray(),
            Enumerable.Repeat(progress, 5).ToArray()));

        Assert.AreEqual(0, enemies.Count);
    }

    [TestMethod]
    public void HandleSimulateDungeon_handles_empty_and_partial_progress_arrays()
    {
        Assert.AreEqual(0, GetSimulatedEnemies(new DungeonState([], [], [])).Count);

        var enemies = GetSimulatedEnemies(new DungeonState([0], [-2, 2], [0]));

        CollectionAssert.AreEqual(new[] { 1, 102, 201 }, enemies.Select(enemy => enemy.Dungeon.Position).ToArray());
        CollectionAssert.AreEqual(new[] { 1, 3, 1 }, enemies.Select(enemy => enemy.Position).ToArray());
    }

    [TestMethod]
    public void HandleSimulateDungeon_keeps_all_worlds_and_special_dungeons_distinct()
    {
        var enemies = GetSimulatedEnemies(new DungeonState(new int[37], new int[37], new int[5]));

        Assert.AreEqual(74, enemies.Count);
        Assert.AreEqual(74, enemies.Select(enemy => enemy.Dungeon.Position).Distinct().Count());
        Assert.IsTrue(enemies.All(enemy => enemy.Position == 1));
    }

    private static int[] CreateLockedProgress() => Enumerable.Repeat(-2, 37).ToArray();

    private static List<DungeonEnemy> GetSimulatedEnemies(DungeonState state)
    {
        var simulator = new RecordingDungeonSimulator();
        var handler = new SimulateDungeonHandler(simulator, new DungeonProvider(), DependencyProvider.Get<IGameFormulasService>());
        var player = new Player(ClassType.Mage, 100, 100, 100, 100, 100, 100, 1000, 0,
            null, null, 0, 2, 0, 0, 0, 0);

        var result = handler.HandleSimulateDungeonAsync(new SimulateDungeonRequest(1, 1, state, player, []));

        Assert.IsInstanceOfType<Ok<List<SimulateDungeonResponse>>>(result.Result);
        var responses = ((Ok<List<SimulateDungeonResponse>>)result.Result).Value!;
        Assert.AreEqual(simulator.Enemies.Count, responses.Count);
        for (var index = 0; index < responses.Count; index++)
        {
            Assert.AreEqual(simulator.Enemies[index].Dungeon.Name, responses[index].DungeonMetadata.DungeonName);
            Assert.AreEqual(simulator.Enemies[index].Position, responses[index].DungeonMetadata.Position);
        }

        return simulator.Enemies;
    }

    private sealed class RecordingDungeonSimulator : IDungeonSimulator
    {
        public List<DungeonEnemy> Enemies { get; } = [];

        public DungeonSimulationResult SimulateDungeon<T, E>(DungeonEnemy dungeonEnemy, IFightable<T> character,
            IFightable<E>[] companions, DungeonSimulationOptions options) where T : IWeaponable where E : IWeaponable
        {
            Enemies.Add(dungeonEnemy);
            return DungeonSimulationResult.FailedResult(0, dungeonEnemy);
        }

        public Task<DungeonSimulationResult> SimulateDungeonAsync<T, E>(DungeonEnemy dungeonEnemy, IFightable<T> character,
            IFightable<E>[] companions, DungeonSimulationOptions options) where T : IWeaponable where E : IWeaponable
            => throw new NotSupportedException();

        public PetSimulationResult SimulatePetDungeon(PetFightable petDungeonEnemy, PetFightable playerPet,
            int simulationContextLevel, DungeonSimulationOptions options) => throw new NotSupportedException();

        public FightSimulationResult SimulateArenaFight<T, E>(IFightable<T> left, IFightable<E> right,
            DungeonSimulationOptions options) where T : IWeaponable where E : IWeaponable => throw new NotSupportedException();
    }
}