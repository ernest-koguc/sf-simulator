using QuestSimulator.Calendars;
using QuestSimulator.Characters;
using QuestSimulator.FileReaders;
using QuestSimulator.Games;
using QuestSimulator.Items;
using QuestSimulator.Quests;
using QuestSimulator.TavernEvents;
using QuestSimulator.Thirst;
using QuestSimulator.Utility;

namespace QuestSimulator.IoC
{
    public static class TypesMapper
    {
        public static IEnumerable<TypeMap> Types() => new[]
        {
            new TypeMap(typeof(CharacterHelper), typeof(ICharacterHelper), TypeMapOption.None),

            new TypeMap(typeof(ValuesReader), typeof(IValuesReader), TypeMapOption.None),

            new TypeMap(typeof(GameSimulator), typeof(IGameSimulator), TypeMapOption.None),
            new TypeMap(typeof(CalendarRewardProvider), typeof(ICalendarRewardProvider), TypeMapOption.None),
            new TypeMap(typeof(EventScheduler), typeof(IEventScheduler), TypeMapOption.None),


            new TypeMap(typeof(QuestHelper), typeof(IQuestHelper), TypeMapOption.None),
            new TypeMap(typeof(QuestFactory), typeof(IQuestFactory), TypeMapOption.None),
            new TypeMap(typeof(QuestChooser), typeof(IQuestChooser), TypeMapOption.None),

            new TypeMap(typeof(ThirstSimulator), typeof(IThirstSimulator), TypeMapOption.None),

            new TypeMap(typeof(CurvesHelper), typeof(ICurvesHelper), TypeMapOption.None),
            new TypeMap(typeof(Random), typeof(Random), TypeMapOption.None),

            new TypeMap(typeof(ItemGenerator), typeof(IItemGenerator), TypeMapOption.None)
        };
    }
}
