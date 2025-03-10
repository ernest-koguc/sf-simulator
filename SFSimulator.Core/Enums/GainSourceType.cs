using System.ComponentModel.DataAnnotations;

namespace SFSimulator.Core;

public enum GainSource
{
    [Display(Name = "Expedition")]
    Expedition = 0,
    [Display(Name = "Guard")]
    Guard = 1,
    [Display(Name = "Arena")]
    Arena = 2,
    [Display(Name = "Daily Tasks")]
    DailyTasks = 3,
    [Display(Name = "Academy")]
    Academy = 4,
    [Display(Name = "Gold Pit")]
    GoldPit = 5,
    [Display(Name = "Time Machine")]
    TimeMachine = 6,
    [Display(Name = "Wheel")]
    Wheel = 7,
    [Display(Name = "Calendar")]
    Calendar = 8,
    [Display(Name = "Gem")]
    Gem = 9,
    [Display(Name = "Item")]
    Item = 10,
    [Display(Name = "Dice Game")]
    DiceGame = 11,
    [Display(Name = "Guild Fight")]
    GuildFight = 12,
    [Display(Name = "Weekly Tasks")]
    WeeklyTasks = 13,
    [Display(Name = "Dungeon")]
    Dungeon = 14,
    [Display(Name = "Pets")]
    Pets = 15,
    [Display(Name = "Total")]
    Total = 1000
}