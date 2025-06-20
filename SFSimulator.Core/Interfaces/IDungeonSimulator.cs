﻿namespace SFSimulator.Core;

public interface IDungeonSimulator
{
    DungeonSimulationResult SimulateDungeon<T, E>(DungeonEnemy dungeonEnemy, IFightable<T> character, IFightable<E>[] companions, int iterations, int winThreshold)
        where T : IWeaponable where E : IWeaponable;
    PetSimulationResult SimulatePetDungeon(PetFightable petDungeonEnemy, PetFightable playerPet, int simulationContextLevel, int iterations, int winThreshold);
}