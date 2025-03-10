using System.Collections.Frozen;

namespace SFSimulator.Core;
public class PetUnlockerService : IPetUnlockerService
{
    public void UnlockPets(int currentDay, SimulationContext simulationContext, List<EventType> events)
    {
        var currentDateTime = DateTime.Now.AddDays(currentDay);

        var unlockablePets = simulationContext.Pets.AllPets
            .Where(p => p.CanBeObtained && !p.IsObtained)
            .ToList();

        foreach (var pet in unlockablePets)
        {
            if (UnlockResolvers[(pet.ElementType, pet.Position)](currentDateTime, events, simulationContext))
            {
                pet.IsObtained = true;
                pet.Level = 1;
            }
        }
    }

    private FrozenDictionary<(PetElementType elementType, int Position), UnlockResolver> UnlockResolvers { get; set; } =
        new Dictionary<(PetElementType ElementType, int Position), UnlockResolver>()
        {
            { (PetElementType.Shadow, 1), (_, _, _) => true },
            { (PetElementType.Shadow, 2), (_, _, _) => true },
            { (PetElementType.Shadow, 3), (_, _, _) => true },
            { (PetElementType.Shadow, 4), (_, _, _) => true },
            { (PetElementType.Shadow, 5), (d, _, _) => UnlockOnMonday(d) },
            { (PetElementType.Shadow, 6), (_, _, _) => true },
            { (PetElementType.Shadow, 7), (d, _, _) => UnlockDuringSummer(d) },
            { (PetElementType.Shadow, 8), (d, _, _) => UnlockDuringWinter(d) },
            { (PetElementType.Shadow, 9), (d, _, _) => UnlockOnFriday(d) },
            { (PetElementType.Shadow, 10), (d, _, _) => UnlockDuringAutumn(d) },
            { (PetElementType.Shadow, 11), (d, _, _) => UnlockOnMonday(d) },
            { (PetElementType.Shadow, 12), (d, _, _) => UnlockOnTuesday(d) },
            { (PetElementType.Shadow, 13), (d, _, _) => UnlockOnWednesday(d) },
            { (PetElementType.Shadow, 14), (d, _, _) => UnlockOnThursday(d) },
            { (PetElementType.Shadow, 15), (_, e, _) => UnlockDuringMushroomEvent(e) },
            { (PetElementType.Shadow, 16), (_, _, _) => true },
            { (PetElementType.Shadow, 17), (_, e, _) => UnlockDuringExperienceEvent(e) },
            { (PetElementType.Shadow, 18), (d, _, _) => UnlockOnFriday13th(d) },
            { (PetElementType.Shadow, 19), (_, _, _) => true },
            { (PetElementType.Shadow, 20), (_, _, _) => true },

            { (PetElementType.Light, 1), (_, _, _) => true },
            { (PetElementType.Light, 2), (_, _, _) => true },
            { (PetElementType.Light, 3), (_, _, _) => true },
            { (PetElementType.Light, 4), (_, _, _) => true },
            { (PetElementType.Light, 5), (d, _, _) => UnlockDuringSpring(d) },
            { (PetElementType.Light, 6), (d, _, _) => UnlockOnSaturday(d) },
            { (PetElementType.Light, 7), (d, _, _) => UnlockDuringSummer(d) },
            { (PetElementType.Light, 8), (d, _, _) => UnlockDuringAutumn(d) },
            { (PetElementType.Light, 9), (d, _, _) => UnlockOnMonday(d) },
            { (PetElementType.Light, 10), (d, _, _) => UnlockOnTuesday(d) },
            { (PetElementType.Light, 11), (d, _, _) => UnlockOnSunday(d) },
            { (PetElementType.Light, 12), (d, _, _) => UnlockOnWednesday(d) },
            { (PetElementType.Light, 13), (d, _, _) => UnlockOnThursday(d) },
            { (PetElementType.Light, 14), (d, _, _) => UnlockOnFriday(d) },
            { (PetElementType.Light, 15), (_, _, _) => true },
            { (PetElementType.Light, 16), (d, _, _) => UnlockDuringDecember(d) },
            { (PetElementType.Light, 17), (d, _, _) => UnlockDuringAprilFools(d) },
            { (PetElementType.Light, 18), (_, e, _) => UnlockDuringShoppingSpreeExtravaganzEvent(e) },
            { (PetElementType.Light, 19), (_, _, s) => UnlockWhenTowerFinished(s) },
            { (PetElementType.Light, 20), (_, _, _) => true },

            { (PetElementType.Earth, 1), (_, _, _) => true },
            { (PetElementType.Earth, 2), (_, _, _) => true },
            { (PetElementType.Earth, 3), (_, _, _) => true },
            { (PetElementType.Earth, 4), (_, _, _) => true },
            { (PetElementType.Earth, 5), (_, _, _) => true },
            { (PetElementType.Earth, 6), (d, _, _) => UnlockDuringSummer(d) },
            { (PetElementType.Earth, 7), (d, _, _) => UnlockDuringAutumn(d) },
            { (PetElementType.Earth, 8), (d, _, _) => UnlockDuringWinter(d) },
            { (PetElementType.Earth, 9), (d, _, _) => UnlockDuringSpring(d) },
            { (PetElementType.Earth, 10), (d, _, _) => UnlockOnSunday(d) },
            { (PetElementType.Earth, 11), (d, _, _) => UnlockOnMonday(d) },
            { (PetElementType.Earth, 12), (d, _, _) => UnlockOnTuesday(d) },
            { (PetElementType.Earth, 13), (d, _, _) => UnlockOnWednesday(d) },
            { (PetElementType.Earth, 14), (d, _, _) => UnlockOnThursday(d) },
            { (PetElementType.Earth, 15), (d, _, _) => UnlockDuringEaster(d) },
            { (PetElementType.Earth, 16), (d, _, _) => UnlockDuringPentecost(d) },
            { (PetElementType.Earth, 17), (_, e, _) => UnlockDuringGoldEvent(e) },
            { (PetElementType.Earth, 18), (_, _, _) => true },
            { (PetElementType.Earth, 19), (_, _, s) => UnlockWhenGemMine20Reached(s) },
            { (PetElementType.Earth, 20), (_, _, _) => true },

            { (PetElementType.Fire, 1), (_, _, _) => true },
            { (PetElementType.Fire, 2), (_, _, _) => true },
            { (PetElementType.Fire, 3), (_, _, _) => true },
            { (PetElementType.Fire, 4), (_, _, _) => true },
            { (PetElementType.Fire, 5), (d, _, _) => UnlockOnTuesday(d) },
            { (PetElementType.Fire, 6), (d, _, _) => UnlockDuringAutumn(d) },
            { (PetElementType.Fire, 7), (_, _, _) => true },
            { (PetElementType.Fire, 8), (d, _, _) => UnlockDuringSpring(d) },
            { (PetElementType.Fire, 9), (d, _, _) => UnlockDuringWinter(d) },
            { (PetElementType.Fire, 10), (d, _, _) => UnlockOnWednesday(d) },
            { (PetElementType.Fire, 11), (d, _, _) => UnlockOnThursday(d) },
            { (PetElementType.Fire, 12), (d, _, _) => UnlockDuringSummer(d) },
            { (PetElementType.Fire, 13), (d, _, _) => UnlockOnFriday(d) },
            { (PetElementType.Fire, 14), (d, _, _) => UnlockOnSaturday(d) },
            { (PetElementType.Fire, 15), (_, _, _) => true },
            { (PetElementType.Fire, 16), (_, e, _) => UnlockDuringToiletEvent(e) },
            { (PetElementType.Fire, 17), (d, _, _) => UnlockDuringValentine(d) },
            { (PetElementType.Fire, 18), (d, _, _) => UnlockDuringNewYear(d) },
            { (PetElementType.Fire, 19), (_, _, s) => UnlockWhenSoloPortalFinsihed(s) },
            { (PetElementType.Fire, 20), (_, _, _) => true },

            { (PetElementType.Water, 1), (_, _, _) => true },
            { (PetElementType.Water, 2), (_, _, _) => true },
            { (PetElementType.Water, 3), (_, _, _) => true },
            { (PetElementType.Water, 4), (_, _, _) => true },
            { (PetElementType.Water, 5), (d, _, _) => UnlockOnMonday(d) },
            { (PetElementType.Water, 6), (d, _, _) => UnlockOnTuesday(d) },
            { (PetElementType.Water, 7), (d, _, _) => UnlockOnWednesday(d) },
            { (PetElementType.Water, 8), (d, _, _) => UnlockDuringWinter(d) },
            { (PetElementType.Water, 9), (d, _, _) => UnlockOnThursday(d) },
            { (PetElementType.Water, 10), (d, _, _) => UnlockOnFriday(d) },
            { (PetElementType.Water, 11), (d, _, _) => UnlockOnSaturday(d) },
            { (PetElementType.Water, 12), (d, _, _) => UnlockDuringAutumn(d) },
            { (PetElementType.Water, 13), (d, _, _) => UnlockDuringSummer(d) },
            { (PetElementType.Water, 14), (d, _, _) => UnlockOnSunday(d) },
            { (PetElementType.Water, 15), (_, _, _) => true },
            { (PetElementType.Water, 16), (_, _, _) => true },
            { (PetElementType.Water, 17), (d, _, _) => UnlockDuringSfBirthday(d) },
            { (PetElementType.Water, 18), (_, _, s) => UnlockWhenAura20Reached(s) },
            { (PetElementType.Water, 19), (_, _, _) => true },
            { (PetElementType.Water, 20), (_, _, _) => true },

        }.ToFrozenDictionary();

    private delegate bool UnlockResolver(DateTime dateTime, List<EventType> events, SimulationContext simulationContext);

    private static bool UnlockOnMonday(DateTime dateTime)
        => dateTime.DayOfWeek == DayOfWeek.Monday;

    private static bool UnlockOnTuesday(DateTime dateTime)
        => dateTime.DayOfWeek == DayOfWeek.Tuesday;

    private static bool UnlockOnWednesday(DateTime dateTime)
        => dateTime.DayOfWeek == DayOfWeek.Wednesday;

    private static bool UnlockOnThursday(DateTime dateTime)
        => dateTime.DayOfWeek == DayOfWeek.Thursday;

    private static bool UnlockOnFriday(DateTime dateTime)
        => dateTime.DayOfWeek == DayOfWeek.Friday;

    private static bool UnlockOnSaturday(DateTime dateTime)
        => dateTime.DayOfWeek == DayOfWeek.Saturday;

    private static bool UnlockOnSunday(DateTime dateTime)
        => dateTime.DayOfWeek == DayOfWeek.Sunday;

    private static bool UnlockDuringSpring(DateTime dateTime)
        => dateTime.Month == 3 || dateTime.Month == 4 || dateTime.Month == 5;

    private static bool UnlockDuringSummer(DateTime dateTime)
        => dateTime.Month == 6 || dateTime.Month == 7 || dateTime.Month == 8;

    private static bool UnlockDuringAutumn(DateTime dateTime)
        => dateTime.Month == 9 || dateTime.Month == 10 || dateTime.Month == 11;

    private static bool UnlockDuringWinter(DateTime dateTime)
        => dateTime.Month == 12 || dateTime.Month == 1 || dateTime.Month == 2;

    private static bool UnlockDuringDecember(DateTime dateTime)
        => dateTime.Month == 12;

    private static bool UnlockDuringAprilFools(DateTime dateTime)
        => dateTime.Month == 4 && dateTime.Day == 1;
    private static bool UnlockOnFriday13th(DateTime dateTime)
        => dateTime.Day == 13 && dateTime.DayOfWeek == DayOfWeek.Friday;

    private static bool UnlockDuringEaster(DateTime dateTime)
        => dateTime.Month == 4 && dateTime.Day >= 1 && dateTime.Day <= 7;

    private static bool UnlockDuringPentecost(DateTime dateTime)
        => dateTime.Month == 6 && (dateTime.Day == 8 || dateTime.Day == 9);

    private static bool UnlockDuringValentine(DateTime dateTime)
        => dateTime.Month == 2 && dateTime.Day == 14;

    public static bool UnlockDuringNewYear(DateTime dateTime)
        => (dateTime.Month == 1 && dateTime.Day == 1) || (dateTime.Month == 12 && dateTime.Day == 31);

    public static bool UnlockDuringSfBirthday(DateTime dateTime)
        => dateTime.Month == 6 && (dateTime.Day == 20 || dateTime.Day == 21 || dateTime.Day == 23);

    private static bool UnlockDuringMushroomEvent(List<EventType> events)
        => events.Contains(EventType.Mushroom);

    private static bool UnlockDuringExperienceEvent(List<EventType> events)
        => events.Contains(EventType.Experience);

    private static bool UnlockDuringGoldEvent(List<EventType> events)
        => events.Contains(EventType.Gold);

    private static bool UnlockDuringShoppingSpreeExtravaganzEvent(List<EventType> events)
        => events.Contains(EventType.EpicShop);

    private static bool UnlockDuringToiletEvent(List<EventType> events)
        => events.Contains(EventType.Toilet);

    private static bool UnlockWhenTowerFinished(SimulationContext simulationContext)
        => simulationContext.GoldBonus.Tower == 100;

    private static bool UnlockWhenGemMine20Reached(SimulationContext simulationContext)
        => simulationContext.GemMineLevel >= 20;

    private static bool UnlockWhenSoloPortalFinsihed(SimulationContext simulationContext)
        => simulationContext.SoloPortal == 50;

    private static bool UnlockWhenAura20Reached(SimulationContext simulationContext)
        => simulationContext.Aura >= 20;
}

