using System.Collections.Frozen;

namespace SFSimulator.Core;

public class RuneValueProvider : IRuneValueProvider
{
    public FrozenDictionary<RuneType, FrozenSet<RuneRange>> RuneRanges { get; set; } = default!;

    public RuneValueProvider()
    {
        InstantiateRuneRanges();
    }

    public int GetRuneValue(RuneType runeType, int runesQuantity)
    {
        if (runeType == RuneType.None)
            return 0;

        if (!RuneRanges.TryGetValue(runeType, out var value))
            throw new ArgumentException("Rune type is not supported", nameof(runeType));

        var ranges = value;
        var runeValue = ranges.LastOrDefault(v => v.RunesQuantity <= runesQuantity).RuneValue;
        return runeValue;
    }


    private void InstantiateRuneRanges()
    {
        RuneRanges = new Dictionary<RuneType, FrozenSet<RuneRange>>
        {
            { RuneType.ColdDamage, GetWeaponRanges() },
            { RuneType.FireDamage, GetWeaponRanges() },
            { RuneType.LightningDamage, GetWeaponRanges() },
            { RuneType.ColdResistance, GetSingleElementResitanceRanges() },
            { RuneType.FireResistance, GetSingleElementResitanceRanges() },
            { RuneType.LightningResistance, GetSingleElementResitanceRanges() },
            { RuneType.TotalResistance, GetTotalResitanceRanges() },
            { RuneType.HealthBonus, GetHitPointsRanges() },
            { RuneType.GoldBonus, GetGoldBonusRanges() },
            { RuneType.ExperienceBonus, GetExperienceBonusRanges() },
            { RuneType.ItemQuality, GetItemQualityRanges() },
        }.ToFrozenDictionary();
    }

    #region WeaponRanges

    private static FrozenSet<RuneRange> GetWeaponRanges()
    {
        return new HashSet<RuneRange>
        {
            new (3, 2),
            new (4, 3),
            new (6, 4),
            new (8, 5),
            new (9, 6),
            new (11, 7),
            new (12, 8),
            new (14, 9),
            new (16, 10),
            new (17, 11),
            new (19, 12),
            new (21, 13),
            new (22, 14),
            new (24, 15),
            new (25, 16),
            new (27, 17),
            new (29, 18),
            new (30, 19),
            new (32, 20),
            new (34, 21),
            new (35, 22),
            new (37, 23),
            new (39, 24),
            new (40, 25),
            new (42, 26),
            new (44, 27),
            new (45, 28),
            new (47, 29),
            new (49, 30),
            new (51, 31),
            new (53, 32),
            new (54, 33),
            new (56, 34),
            new (57, 35),
            new (59, 36),
            new (61, 37),
            new (62, 38),
            new (65, 39),
            new (66, 40),
            new (69, 41),
            new (70, 42),
            new (71, 43),
            new (73, 44),
            new (74, 45),
            new (76, 46),
            new (77, 47),
            new (78, 48),
            new (79, 49),
            new (80, 50),
            new (84, 51),
            new (85, 52),
            new (94, 56),
            new (95, 57),
            new (96, 58),
            new (101, 60)
        }.ToFrozenSet();
    }

    #endregion

    #region SingleResistanceRanges

    private static FrozenSet<RuneRange> GetSingleElementResitanceRanges()
    {
        return new HashSet<RuneRange>
        {
            new (3,3),
            new (4,4),
            new (6,5),
            new (8,6),
            new (9,7),
            new (10,8),
            new (12,9),
            new (13,10),
            new (14,11),
            new (15,12),
            new (17,13),
            new (18,14),
            new (20,15),
            new (21,16),
            new (22,17),
            new (23,18),
            new (25,19),
            new (26,20),
            new (27,21),
            new (28,22),
            new (30,23),
            new (31,24),
            new (32,25),
            new (33,26),
            new (35,27),
            new (36,28),
            new (38,29),
            new (40,30),
            new (41,31),
            new (42,32),
            new (43,33),
            new (44,34),
            new (46,35),
            new (47,36),
            new (49,37),
            new (50,38),
            new (52,39),
            new (53,40),
            new (54,41),
            new (56,42),
            new (57,43),
            new (58,44),
            new (59,45),
            new (61,46),
            new (63,47),
            new (64,48),
            new (65,49),
            new (67,50),
            new (68,51),
            new (69,52),
            new (70,53),
            new (71,54),
            new (74,55),
            new (75,56),
            new (77,57),
            new (78,58),
            new (79,59),
            new (80,60),
            new (81,61),
            new (82,62),
            new (84,63),
            new (85,64),
            new (88,65),
            new (89,66),
            new (90,67),
            new (91,68),
            new (92,69),
            new (94,70),
            new (95,71),
            new (96,72),
            new (97,73),
            new (98,74),
            new (100,75),
        }.ToFrozenSet();
    }

    #endregion

    #region TotalResistanceRanges

    private static FrozenSet<RuneRange> GetTotalResitanceRanges()
    {
        return new HashSet<RuneRange>
        {
            new (3,2),
            new (8,3),
            new (13,4),
            new (18,5),
            new (23,6),
            new (26,7),
            new (30,8),
            new (34,9),
            new (38,10),
            new (42,11),
            new (46,12),
            new (50,13),
            new (54,14),
            new (58,15),
            new (62,16),
            new (69,17),
            new (70,18),
            new (74,19),
            new (79,20),
            new (82,21),
            new (86,22),
            new (100,25),
        }.ToFrozenSet();
    }

    #endregion

    #region HitPointsRanges 

    private static FrozenSet<RuneRange> GetHitPointsRanges()
    {
        return new HashSet<RuneRange>
        {
            new (3,1),
            new (6,2),
            new (16,3),
            new (23,4),
            new (30,5),
            new (36,6),
            new (43,7),
            new (50,8),
            new (56,9),
            new (63,10),
            new (70,11),
            new (79,12),
            new (84,13),
            new (91,14),
            new (98,15),
        }.ToFrozenSet();

    }

    #endregion

    #region GoldBonusRanges

    private static FrozenSet<RuneRange> GetGoldBonusRanges()
    {
        return new HashSet<RuneRange>
        {
            new (3,1),
            new (9,2),
            new (25,3),
            new (33,4),
            new (45,5),
            new (55,6),
            new (65,7),
            new (80,8),
            new (85,9),
            new (96,10),
        }.ToFrozenSet();
    }

    #endregion

    #region GoldBonusRanges

    private static FrozenSet<RuneRange> GetExperienceBonusRanges()
    {
        return new HashSet<RuneRange>
        {
            new (3,2),
            new (5,3),
            new (7,4),
            new (9,5),
            new (11,6),
            new (13,7),
            new (15,8),
            new (17,9),
            new (19,10),
            new (21,11),
            new (23,12),
            new (25,13),
            new (27,14),
            new (29,15),
            new (31,16),
            new (33,17),
            new (35,18),
            new (37,19),
            new (39,20),
            new (41,21),
            new (43,22),
            new (45,23),
            new (47,24),
            new (49,25),
            new (51,26),
            new (53,27),
            new (55,28),
            new (57,29),
            new (59,30),
            new (61,31),
            new (63,32),
            new (65,33),
            new (67,34),
            new (69,35),
            new (71,36),
            new (73,37),
            new (75,38),
            new (77,39),
            new (79,40),
            new (81,41),
            new (83,42),
            new (85,43),
            new (87,44),
            new (89,45),
            new (91,46),
            new (93,47),
            new (95,48),
            new (97,49),
            new (99,50),
        }.ToFrozenSet();
    }

    #endregion

    #region ItemQualityRanges
    private static FrozenSet<RuneRange> GetItemQualityRanges()
    {

        return new HashSet<RuneRange>
        {
            new (1, 1),
            new (18, 2),
            new (50, 3),
            new (70, 4),
            new (92, 5),
        }.ToFrozenSet();
    }
    #endregion
}

public readonly record struct RuneRange(int RunesQuantity, int RuneValue);