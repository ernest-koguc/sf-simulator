using Radzen;
using SFSimulator.Core;

namespace SFSimulator.Frontend;

public static class AchievementTypeExtensions
{
    public static PointStyle ToPointStyle(this AchievementType type)
    {
        return type switch
        {
            AchievementType.SimulationStart => PointStyle.Info,

            AchievementType.Level100 => PointStyle.Primary,
            AchievementType.Level200 => PointStyle.Primary,
            AchievementType.Level300 => PointStyle.Primary,
            AchievementType.Level400 => PointStyle.Primary,
            AchievementType.Level500 => PointStyle.Primary,
            AchievementType.Level600 => PointStyle.Primary,
            AchievementType.Level700 => PointStyle.Primary,
            AchievementType.Level800 => PointStyle.Primary,

            AchievementType.BaseStat1000 => PointStyle.Secondary,
            AchievementType.BaseStat10000 => PointStyle.Secondary,
            AchievementType.BaseStat50000 => PointStyle.Secondary,
            AchievementType.BaseStat75000 => PointStyle.Secondary,
            AchievementType.BaseStat100000 => PointStyle.Secondary,
            AchievementType.BaseStat150000 => PointStyle.Secondary,
            AchievementType.BaseStat200000 => PointStyle.Secondary,
            AchievementType.BaseStat250000 => PointStyle.Secondary,
            AchievementType.BaseStat300000 => PointStyle.Secondary,
            AchievementType.BaseStat350000 => PointStyle.Secondary,
            AchievementType.BaseStat400000 => PointStyle.Secondary,
            AchievementType.BaseStat500000 => PointStyle.Secondary,
            AchievementType.BaseStat600000 => PointStyle.Secondary,
            AchievementType.BaseStat700000 => PointStyle.Secondary,
            AchievementType.BaseStat800000 => PointStyle.Secondary,
            AchievementType.BaseStat900000 => PointStyle.Secondary,
            AchievementType.BaseStat1000000 => PointStyle.Secondary,

            AchievementType.LightWorldFinished => PointStyle.Warning,
            AchievementType.ShadowWorldFinished => PointStyle.Warning,
            AchievementType.SoloPortalFinished => PointStyle.Warning,
            AchievementType.LoopOfTheIdolsFinsihed => PointStyle.Warning,
            AchievementType.TwisterFinished => PointStyle.Warning,
            AchievementType.TowerFinished => PointStyle.Warning,
            AchievementType.AllDungeonsFinished => PointStyle.Warning,

            AchievementType.SimulationFinish => PointStyle.Danger,

            _ => PointStyle.Dark,
        };
    }
}
