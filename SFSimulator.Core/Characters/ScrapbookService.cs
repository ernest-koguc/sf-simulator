﻿namespace SFSimulator.Core;
public class ScrapbookService : IScrapbookService
{
    private Dictionary<int, int> LevelItemsMap = new()
    {
        // Normal + Epic Items 
        { 1, 249 },
        // Normal Items
        { 5, 129 },
        // Expeditions
        { 6, 20 },
        // Normal Items
        { 10, 129 },
        { 15, 129 },
        // Expeditions
        { 16, 20 },
        // Training Camp
        { 19, 10 },
        // Normal Items
        { 20, 129 },
        { 25, 129 },
        // Expeditions
        { 26, 20 },
        // Normal Items
        { 30, 124 },
        { 35, 124 },
        // Expeditions
        { 36, 13 },
        // Normal Items
        { 40, 128 },
        { 45, 122 },
        // Seasonal Epics
        { 50, 132 },
        // Toilet + Epic
        { 100, 48 },
        { 150, 22 },
        { 200, 22 },
        { 250, 22 },
        { 300, 22 },
        { 350, 22 },
    };

    public void InitScrapbook(IEnumerable<Dungeon> dungeons, SimulationContext simulationContext)
    {
        var initialPictures = GetCurrentPictures(simulationContext.ExperienceBonus.ScrapbookFillness);

        var monsterPictures = dungeons
            .SelectMany(d => d.DungeonEnemies)
            .Where(dE => dE.IsDefeated && IsNotMirrorDungeon(dE) && IsViablePictureDungeon(dE))
            .Count()
            + simulationContext.SoloPortal
            + simulationContext.GuildPortal
            + Math.Min(50, simulationContext.GuildRaids);

        var newItems = monsterPictures + LevelItemsMap.Where(x => x.Key <= simulationContext.Level).Sum(x => x.Value);
        if (newItems >= initialPictures)
        {
            simulationContext.ExperienceBonus.ScrapbookFillness = Math.Min(100, (decimal)newItems / CoreShared.SCRAPBOOK_LIMIT * 100);
            LevelItemsMap = LevelItemsMap.Where(x => x.Key > simulationContext.Level).ToDictionary();
            return;
        }

        var difference = initialPictures - newItems;
        newItems = initialPictures;
        LevelItemsMap = LevelItemsMap.Where(x => x.Key <= simulationContext.Level).ToDictionary();
        foreach (var key in LevelItemsMap.Keys)
        {
            var levelItem = LevelItemsMap.First(x => x.Key == key);
            if (difference <= 0)
            {
                break;
            }

            var toAdd = Math.Min(difference, levelItem.Value);
            difference -= toAdd;
            LevelItemsMap[key] -= (int)toAdd;
        }
    }

    public void UpdateScrapbook(SimulationContext simulationContext)
    {
        var initialPictures = GetCurrentPictures(simulationContext.ExperienceBonus.ScrapbookFillness);
        var picturesToAdd = LevelItemsMap.Where(x => x.Key <= simulationContext.Level).Sum(x => x.Value);
        simulationContext.ExperienceBonus.ScrapbookFillness = Math.Min(100, (decimal)(initialPictures + picturesToAdd) / CoreShared.SCRAPBOOK_LIMIT * 100);
        LevelItemsMap = LevelItemsMap.Where(x => x.Key > simulationContext.Level).ToDictionary();
    }
    public void UpdateScrapbook(SimulationContext simulationContext, DungeonSimulationResult dungeonResult)
    {
        if (dungeonResult.Succeeded
            && IsViablePictureDungeon(dungeonResult.DungeonEnemy)
            && IsNotMirrorDungeon(dungeonResult.DungeonEnemy))
        {
            var initialPictures = GetCurrentPictures(simulationContext.ExperienceBonus.ScrapbookFillness);
            simulationContext.ExperienceBonus.ScrapbookFillness = Math.Min(100, (decimal)(initialPictures + 1) / CoreShared.SCRAPBOOK_LIMIT * 100);
        }
    }
    public void UpdateScrapbook(SimulationContext simulationContext, int guildRaidPictures)
    {
        var initialPictures = GetCurrentPictures(simulationContext.ExperienceBonus.ScrapbookFillness);
        simulationContext.ExperienceBonus.ScrapbookFillness = Math.Min(100, (decimal)(initialPictures + guildRaidPictures) / CoreShared.SCRAPBOOK_LIMIT * 100);
    }

    private bool IsNotMirrorDungeon(DungeonEnemy dungeonEnemy) => dungeonEnemy is not { Dungeon: { Position: 9 }, Position: 10 };
    private bool IsViablePictureDungeon(DungeonEnemy dungeonEnemy) => dungeonEnemy.Dungeon.Type is DungeonTypeEnum.Default or DungeonTypeEnum.Tower or DungeonTypeEnum.LoopOfIdols;

    private int GetCurrentPictures(decimal scrapbookFillness)
        => (int)(scrapbookFillness * CoreShared.SCRAPBOOK_LIMIT / 100);
}

