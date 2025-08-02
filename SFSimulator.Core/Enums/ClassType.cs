using System.ComponentModel.DataAnnotations;

namespace SFSimulator.Core;

public enum ClassType
{
    Mirror = -1,
    Bert = 0,
    Warrior = 1,
    Mage = 2,
    Scout = 3,
    Assassin = 4,
    [Display(Name = "Battle Mage")]
    BattleMage = 5,
    Berserker = 6,
    [Display(Name = "Demon Hunter")]
    DemonHunter = 7,
    Druid = 8,
    Bard = 9,
    Necromancer = 10,
    Paladin = 11,
}