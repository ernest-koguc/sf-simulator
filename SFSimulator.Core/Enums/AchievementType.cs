using System.ComponentModel.DataAnnotations;

namespace SFSimulator.Core;
public enum AchievementType
{
    [Display(Name = "Start")]
    SimulationStart,
    [Display(Name = "Level 100 reached")]
    Level100,
    [Display(Name = "Level 200 reached")]
    Level200,
    [Display(Name = "Level 300 reached")]
    Level300,
    [Display(Name = "Level 400 reached")]
    Level400,
    [Display(Name = "Level 500 reached")]
    Level500,
    [Display(Name = "Level 600 reached")]
    Level600,
    [Display(Name = "Level 700 reached")]
    Level700,
    [Display(Name = "Level 800 reached")]
    Level800,
    [Display(Name = "1 000 base stats reached")]
    BaseStat1000,
    [Display(Name = "10 000 base stats reached")]
    BaseStat10000,
    [Display(Name = "50 000 base stats reached")]
    BaseStat50000,
    [Display(Name = "75 000 base stats reached")]
    BaseStat75000,
    [Display(Name = "100 000 base stats reached")]
    BaseStat100000,
    [Display(Name = "150 000 base stats reached")]
    BaseStat150000,
    [Display(Name = "200 000 base stats reached")]
    BaseStat200000,
    [Display(Name = "250 000 base stats reached")]
    BaseStat250000,
    [Display(Name = "300 000 base stats reached")]
    BaseStat300000,
    [Display(Name = "350 000 base stats reached")]
    BaseStat350000,
    [Display(Name = "400 000 base stats reached")]
    BaseStat400000,
    [Display(Name = "500 000 base stats reached")]
    BaseStat500000,
    [Display(Name = "600 000 base stats reached")]
    BaseStat600000,
    [Display(Name = "700 000 base stats reached")]
    BaseStat700000,
    [Display(Name = "800 000 base stats reached")]
    BaseStat800000,
    [Display(Name = "900 000 base stats reached")]
    BaseStat900000,
    [Display(Name = "1 000 000 base stats reached")]
    BaseStat1000000,
    [Display(Name = "Tower finished")]
    TowerFinished,
    [Display(Name = "Light World finished")]
    LightWorldFinished,
    [Display(Name = "Shadow World finished")]
    ShadowWorldFinished,
    [Display(Name = "Loops of the Idols finished")]
    LoopOfTheIdolsFinsihed,
    [Display(Name = "Twister finished")]
    TwisterFinished,
    [Display(Name = "Solo Portal finished")]
    SoloPortalFinished,
    [Display(Name = "All dungeons finished")]
    AllDungeonsFinished,
    [Display(Name = "Finish")]
    SimulationFinish
}
