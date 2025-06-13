using System.ComponentModel.DataAnnotations;

namespace SFSimulator.Core;

public enum ClassType
{
    Mirror = -1,
    Bert = 0,
    Warrior,
    Mage,
    Scout,
    Assassin,
    [Display(Name = "Battle Mage")]
    BattleMage,
    Berserker,
    [Display(Name = "Demon Hunter")]
    DemonHunter,
    Druid,
    Bard,
    Necromancer,
    Paladin
}