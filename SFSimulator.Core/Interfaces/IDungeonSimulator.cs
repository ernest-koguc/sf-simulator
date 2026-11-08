namespace SFSimulator.Core;

public interface IDungeonSimulator
{
    DungeonSimulationResult SimulateDungeon<T, E>(DungeonEnemy dungeonEnemy, IFightable<T> character, IFightable<E>[] companions,
        DungeonSimulationOptions options) where T : IWeaponable where E : IWeaponable;
    PetSimulationResult SimulatePetDungeon(PetFightable petDungeonEnemy, PetFightable playerPet, int simulationContextLevel,
        DungeonSimulationOptions options);
    Task<DungeonSimulationResult> SimulateDungeonAsync<T, E>(DungeonEnemy dungeonEnemy, 
            IFightable<T> character, IFightable<E>[] companions, DungeonSimulationOptions options) 
        where T : IWeaponable 
        where E : IWeaponable;

    FightSimulationResult SimulateArenaFight<T, E>(IFightable<T> left, IFightable<E> right, DungeonSimulationOptions options)
        where T : IWeaponable
        where E : IWeaponable;
}