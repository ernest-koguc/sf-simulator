using System.ComponentModel.DataAnnotations;

namespace SFSimulator.Core;

// TODO: at some point we should reshuffle the enums and possible make that level start at 100 
// and base stat at 200 or something like that so that we have enough room reserved
public enum AchievementType
{
    [Display(Name = "Start")]
    SimulationStart = 0,
    [Display(Name = "Level 100 reached")]
    Level100 = 1,
    [Display(Name = "Level 200 reached")]
    Level200 = 2,
    [Display(Name = "Level 300 reached")]
    Level300 = 3,
    [Display(Name = "Level 400 reached")]
    Level400 = 4,
    [Display(Name = "Level 500 reached")]
    Level500 = 5,
    [Display(Name = "Level 600 reached")]
    Level600 = 6,
    [Display(Name = "Level 700 reached")]
    Level700 = 7,
    [Display(Name = "Level 800 reached")]
    Level800 = 8,
    [Display(Name = "Level 900 reached")]
    Level900 = 34,
    [Display(Name = "Level 1000 reached")]
    Level1000 = 35,
    [Display(Name = "1 000 base stats reached")]
    BaseStat1000 = 9,
    [Display(Name = "10 000 base stats reached")]
    BaseStat10000 = 10,
    [Display(Name = "50 000 base stats reached")]
    BaseStat50000 = 11,
    [Display(Name = "75 000 base stats reached")]
    BaseStat75000 = 12,
    [Display(Name = "100 000 base stats reached")]
    BaseStat100000 = 13,
    [Display(Name = "150 000 base stats reached")]
    BaseStat150000 = 14,
    [Display(Name = "200 000 base stats reached")]
    BaseStat200000 = 15,
    [Display(Name = "250 000 base stats reached")]
    BaseStat250000 = 16,
    [Display(Name = "300 000 base stats reached")]
    BaseStat300000 = 17,
    [Display(Name = "350 000 base stats reached")]
    BaseStat350000 = 18,
    [Display(Name = "400 000 base stats reached")]
    BaseStat400000 = 19,
    [Display(Name = "500 000 base stats reached")]
    BaseStat500000 = 20,
    [Display(Name = "600 000 base stats reached")]
    BaseStat600000 = 21,
    [Display(Name = "700 000 base stats reached")]
    BaseStat700000 = 22,
    [Display(Name = "800 000 base stats reached")]
    BaseStat800000 = 23,
    [Display(Name = "900 000 base stats reached")]
    BaseStat900000 = 24,
    [Display(Name = "1 000 000 base stats reached")]
    BaseStat1000000 = 25,
    [Display(Name = "Tower finished")]
    TowerFinished = 26,
    [Display(Name = "Light World finished")]
    LightWorldFinished = 27,
    [Display(Name = "Shadow World finished")]
    ShadowWorldFinished = 28,
    [Display(Name = "Loops of the Idols finished")]
    LoopOfTheIdolsFinsihed = 29,
    [Display(Name = "Twister finished")]
    TwisterFinished = 30,
    [Display(Name = "Solo Portal finished")]
    SoloPortalFinished = 31,
    [Display(Name = "All dungeons finished")]
    AllDungeonsFinished = 32,
    [Display(Name = "Finish")]
    SimulationFinish = 33,
}