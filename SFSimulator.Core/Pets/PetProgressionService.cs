namespace SFSimulator.Core;
public class PetProgressionService(IPetFightableFactory petFightableFactory, IPetPathProvider petPathProvider,
    IPetUnlockerService petUnlockerService, IDungeonSimulator dungeonSimulator) : IPetProgressionService
{
    private const int PetUnlockLevel = 75;
    private bool PetsUnlocked { get; set; }


    public void ProgressThrough(int currentDay, SimulationContext simulationContext, List<EventType> events, Action<PetSimulationResult> onWonFight)
    {
        if (simulationContext.Level < PetUnlockLevel)
            return;

        if (!PetsUnlocked)
        {
            PetsUnlocked = true;
            foreach (var pet in simulationContext.Pets.AllPets.Where(p => p.Position <= 3))
            {
                pet.CanBeObtained = true;
            }
        }

        petUnlockerService.UnlockPets(currentDay, simulationContext, events);
        var isPetEvent = events.Contains(EventType.Pets);
        DoPetArenaFights(currentDay, simulationContext.Pets, isPetEvent);
        FeedPets(currentDay, simulationContext.Pets, isPetEvent);

        var openPetDungeons = new List<Pet>();
        foreach (var element in simulationContext.Pets.Elements)
        {
            var petDungeon = element.Value.FirstOrDefault(p => !p.IsDefeated);
            if (petDungeon is not null)
            {
                openPetDungeons.Add(petDungeon);
            }
        }

        DoPetDungeonsFights(currentDay, simulationContext, openPetDungeons, isPetEvent, onWonFight);
        petUnlockerService.UnlockPets(currentDay, simulationContext, events);
        FeedPets(currentDay, simulationContext.Pets, isPetEvent);
    }

    private void FeedPets(int currentDay, PetsState pets, bool isPetEvent)
    {
        var feedingLimit = isPetEvent ? 9 : 3;
        var petLevelLimit = pets.AllPets.All(p => p.IsDefeated) ? 200 : 100;

        foreach (var elementType in Enum.GetValues<PetElementType>())
        {
            var canFeed = pets.Food[elementType] >= 1;
            while (canFeed)
            {
                Pet? petToFeed;
                if (pets.Elements[elementType].All(p => p.IsDefeated))
                {
                    petToFeed = pets.Elements[elementType].LastOrDefault(p => p.IsObtained && p.Level < petLevelLimit);
                }
                else
                {
                    petToFeed = petPathProvider.GetPetFromPath(elementType, pets);
                }

                if (petToFeed is null || !petToFeed.IsObtained ||
                    (petToFeed.LastDayWhenFed == currentDay && petToFeed.LastFedAmount == feedingLimit) || petToFeed.Level == petLevelLimit)
                {
                    break;
                }

                if (petToFeed.LastDayWhenFed != currentDay)
                {
                    petToFeed.LastDayWhenFed = currentDay;
                    petToFeed.LastFedAmount = 0;
                }

                var missingLevels = petLevelLimit - petToFeed.Level;
                var food = Math.Min(missingLevels, Math.Min(feedingLimit - petToFeed.LastFedAmount, Math.Floor(pets.Food[elementType])));
                pets.Food[elementType] -= food;
                petToFeed.Level += (int)food;
                petToFeed.LastFedAmount = (int)food;

                canFeed = pets.Food[elementType] >= 1;
            }
        }
    }

    public decimal SellPetFood(PetsState pets, int characterLevel)
    {
        if (characterLevel < 632 || pets.AllPets.Any(p => p.Level < 200))
        {
            return 0;
        }

        var totalFood = pets.Food.Values.Sum();

        foreach (var key in pets.Food.Keys)
        {
            pets.Food[key] = 0;
        }

        return (decimal)totalFood * 2_000_000M;
    }

    public void GivePetFood(PetsState pets, double amount, PetElementType? elementType = null)
    {
        if (elementType is not null)
        {
            pets.Food[elementType.Value] += amount;
            return;
        }

        foreach (var key in pets.Food.Keys)
        {
            pets.Food[key] += amount / pets.Food.Keys.Count;
        }
    }

    private void DoPetDungeonsFights(int currentDay, SimulationContext simulationContext, List<Pet> dungeons, bool isPetEvent, Action<PetSimulationResult> onWonFight)
    {
        List<Pet> nextDungeons = [];

        foreach (var dungeonPet in dungeons)
        {
            var playerPet = petPathProvider.GetPetFromPath(dungeonPet.ElementType, simulationContext.Pets);
            if (playerPet is null || !playerPet.IsObtained)
            {
                continue;
            }

            var petDungeonFightable = petFightableFactory.CreateDungeonPetFightable(dungeonPet.ElementType, dungeonPet.Position);
            var playerPetFightable = petFightableFactory.CreatePetFightable(playerPet, simulationContext.Pets, simulationContext.GladiatorLevel);
            var result = dungeonSimulator
                .SimulatePetDungeon(petDungeonFightable, playerPetFightable, simulationContext.Level, 1000, 7);

            if (result.Succeeded)
            {
                onWonFight(result);
                dungeonPet.IsDefeated = true;
                dungeonPet.CanBeObtained = true;
                simulationContext.Pets.Food[dungeonPet.ElementType] += isPetEvent ? 3 : 1;

                var nextDungeonPet = simulationContext.Pets.Elements[dungeonPet.ElementType].FirstOrDefault(p => p.Position == dungeonPet.Position + 1);
                if (nextDungeonPet is not null)
                {
                    nextDungeons.Add(nextDungeonPet);
                }
            }
        }

        FeedPets(currentDay, simulationContext.Pets, isPetEvent);

        if (nextDungeons.Any())
        {
            DoPetDungeonsFights(currentDay, simulationContext, nextDungeons, isPetEvent, onWonFight);
        }
    }

    private void DoPetArenaFights(int currentDay, PetsState pets, bool isPetEvent)
    {
        var petType = GetCurrentArenaElement(currentDay);
        var food = isPetEvent ? 7.5 : 2.5;
        pets.Food[petType] += 2.5;
    }

    private PetElementType GetCurrentArenaElement(int currentDay)
        => (PetElementType)(currentDay % 5);
}