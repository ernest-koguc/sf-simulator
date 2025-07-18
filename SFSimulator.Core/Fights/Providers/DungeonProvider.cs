﻿namespace SFSimulator.Core;

public class DungeonProvider : IDungeonProvider
{
    private List<Dungeon> Dungeons { get; set; } = default!;

    public DungeonProvider()
    {
        Dungeons = InitDungeons();
    }

    public List<DungeonEnemy> GetFightablesDungeonEnemies(SimulationContext simulationContext)
    {
        return Dungeons
            .Where(d => d.IsUnlocked && !d.IsDefeated)
            .Select(d => d.DungeonEnemies.OrderBy(e => e.Position).First(e => !e.IsDefeated))
            .InitMirrorEnemy(simulationContext)
            .ToList();
    }

    public List<Dungeon> GetAllDungeons(SimulationContext simulationContext) => Dungeons.InitMirrorEnemy(simulationContext).ToList();

    public bool IsValidEnemy(int dungeonPosition, int dungeonEnemyPosition)
    {
        return Dungeons
            .FirstOrDefault(d => d.Position == dungeonPosition)
            ?.DungeonEnemies
            .FirstOrDefault(e => e.Position == dungeonEnemyPosition) is not null;
    }

    public DungeonEnemy GetDungeonEnemy(int dungeonPositon, int dungeonEnemyPosition)
    {
        var dungeon = Dungeons.FirstOrDefault(d => d.Position == dungeonPositon) ?? throw new ArgumentOutOfRangeException(nameof(dungeonPositon));
        var enemy = dungeon.DungeonEnemies.FirstOrDefault(e => e.Position == dungeonEnemyPosition) ?? throw new ArgumentOutOfRangeException(nameof(dungeonEnemyPosition));
        return enemy;
    }

    public List<Dungeon> InitDungeons()
    {
        var dungeons = new List<Dungeon>()
        {
            #region Light World
            new Dungeon
            {
                Name = DungeonNames.DesecratedCatacombs,
                Position = 1,
                IsDefeated = false,
                IsUnlocked = true,
                UnlockResolve = c => c.CharacterLevel >= 10,
                DungeonEnemies = new List<DungeonEnemy>()
                {
                    new DungeonEnemy(position: 1, @class: ClassType.Mage, level: 10, strength: 48, dexterity: 52, intelligence: 104, constitution: 77, luck: 47, health: 1694, minWeaponDmg: 29, maxWeaponDmg: 52 ),
                    new DungeonEnemy(position: 2, @class: ClassType.Warrior, level: 12, strength: 120, dexterity: 68, intelligence: 59, constitution: 101, luck: 51, health: 6565, minWeaponDmg: 13, maxWeaponDmg: 28 ),
                    new DungeonEnemy(position: 3, @class: ClassType.Warrior, level: 14, strength: 149, dexterity: 78, intelligence: 69, constitution: 124, luck: 65, health: 9300, minWeaponDmg: 17, maxWeaponDmg: 34 ),
                    new DungeonEnemy(position: 4, @class: ClassType.Scout, level: 16, strength: 84, dexterity: 195, intelligence: 83, constitution: 131, luck: 94, health: 8908, minWeaponDmg: 26, maxWeaponDmg: 47 ),
                    new DungeonEnemy(position: 5, @class: ClassType.Warrior, level: 18, strength: 214, dexterity: 101, intelligence: 89, constitution: 169, luck: 93, health: 16055, minWeaponDmg: 25, maxWeaponDmg: 43 ),
                    new DungeonEnemy(position: 6, @class: ClassType.Mage, level: 22, strength: 97, dexterity: 99, intelligence: 303, constitution: 198, luck: 137, health: 9108, minWeaponDmg: 71, maxWeaponDmg: 116 ),
                    new DungeonEnemy(position: 7, @class: ClassType.Warrior, level: 26, strength: 359, dexterity: 135, intelligence: 122, constitution: 260, luck: 142, health: 35100, minWeaponDmg: 33, maxWeaponDmg: 63 ),
                    new DungeonEnemy(position: 8, @class: ClassType.Mage, level: 30, strength: 126, dexterity: 130, intelligence: 460, constitution: 279, luck: 193, health: 17298, minWeaponDmg: 97, maxWeaponDmg: 160 ),
                    new DungeonEnemy(position: 9, @class: ClassType.Warrior, level: 40, strength: 614, dexterity: 207, intelligence: 191, constitution: 445, luck: 238, health: 91225, minWeaponDmg: 54, maxWeaponDmg: 95 ),
                    new DungeonEnemy(position: 10, @class: ClassType.Scout, level: 50, strength: 221, dexterity: 847, intelligence: 213, constitution: 561, luck: 292, health: 114444, minWeaponDmg: 86, maxWeaponDmg: 151 )
                }
            },
            new Dungeon
            {
                Name = DungeonNames.MinesOfGloria,
                Position = 2,
                IsDefeated = false,
                IsUnlocked = true,
                UnlockResolve = c => c.CharacterLevel >= 20,
                DungeonEnemies = new List<DungeonEnemy>
                {
                    new DungeonEnemy(position: 1, @class: ClassType.Scout, level: 20, strength: 101, dexterity: 264, intelligence: 101, constitution: 174, luck: 119, health: 14616, minWeaponDmg: 35, maxWeaponDmg: 58 ),
                    new DungeonEnemy(position: 2, @class: ClassType.Warrior, level: 24, strength: 317, dexterity: 126, intelligence: 117, constitution: 238, luck: 130, health: 29750, minWeaponDmg: 33, maxWeaponDmg: 58 ),
                    new DungeonEnemy(position: 3, @class: ClassType.Warrior, level: 28, strength: 393, dexterity: 138, intelligence: 125, constitution: 284, luck: 152, health: 41180, minWeaponDmg: 40, maxWeaponDmg: 68 ),
                    new DungeonEnemy(position: 4, @class: ClassType.Scout, level: 34, strength: 143, dexterity: 554, intelligence: 144, constitution: 303, luck: 216, health: 42420, minWeaponDmg: 64, maxWeaponDmg: 98 ),
                    new DungeonEnemy(position: 5, @class: ClassType.Warrior, level: 38, strength: 592, dexterity: 178, intelligence: 162, constitution: 398, luck: 195, health: 77610, minWeaponDmg: 58, maxWeaponDmg: 89 ),
                    new DungeonEnemy(position: 6, @class: ClassType.Mage, level: 44, strength: 191, dexterity: 190, intelligence: 780, constitution: 411, luck: 259, health: 36990, minWeaponDmg: 143, maxWeaponDmg: 241 ),
                    new DungeonEnemy(position: 7, @class: ClassType.Warrior, level: 48, strength: 744, dexterity: 243, intelligence: 230, constitution: 563, luck: 246, health: 137935, minWeaponDmg: 69, maxWeaponDmg: 122 ),
                    new DungeonEnemy(position: 8, @class: ClassType.Scout, level: 56, strength: 250, dexterity: 960, intelligence: 240, constitution: 680, luck: 345, health: 155040, minWeaponDmg: 92, maxWeaponDmg: 160 ),
                    new DungeonEnemy(position: 9, @class: ClassType.Scout, level: 66, strength: 300, dexterity: 1160, intelligence: 290, constitution: 880, luck: 420, health: 235840, minWeaponDmg: 102, maxWeaponDmg: 175 ),
                    new DungeonEnemy(position: 10, @class: ClassType.Warrior, level: 70, strength: 1240, dexterity: 385, intelligence: 360, constitution: 960, luck: 340, health: 340800, minWeaponDmg: 85, maxWeaponDmg: 143 )
                }
            },
            new Dungeon
            {
                Name = DungeonNames.RuinsOfGnark,
                Position = 3,
                IsDefeated = false,
                IsUnlocked = true,
                UnlockResolve = c => c.CharacterLevel >= 30,
                DungeonEnemies = new List<DungeonEnemy>
                {
                    new DungeonEnemy(position: 1, @class: ClassType.Scout, level: 32, strength: 155, dexterity: 486, intelligence: 161, constitution: 276, luck: 205, health: 36432, minWeaponDmg: 58, maxWeaponDmg: 95 ),
                    new DungeonEnemy(position: 2, @class: ClassType.Scout, level: 36, strength: 141, dexterity: 602, intelligence: 149, constitution: 344, luck: 230, health: 50912, minWeaponDmg: 60, maxWeaponDmg: 108 ),
                    new DungeonEnemy(position: 3, @class: ClassType.Scout, level: 42, strength: 205, dexterity: 726, intelligence: 224, constitution: 403, luck: 247, health: 69316, minWeaponDmg: 71, maxWeaponDmg: 124 ),
                    new DungeonEnemy(position: 4, @class: ClassType.Warrior, level: 46, strength: 768, dexterity: 215, intelligence: 183, constitution: 539, luck: 249, health: 126665, minWeaponDmg: 66, maxWeaponDmg: 115 ),
                    new DungeonEnemy(position: 5, @class: ClassType.Warrior, level: 54, strength: 920, dexterity: 265, intelligence: 240, constitution: 640, luck: 260, health: 176000, minWeaponDmg: 69, maxWeaponDmg: 127 ),
                    new DungeonEnemy(position: 6, @class: ClassType.Scout, level: 60, strength: 270, dexterity: 1040, intelligence: 260, constitution: 760, luck: 375, health: 185440, minWeaponDmg: 97, maxWeaponDmg: 165 ),
                    new DungeonEnemy(position: 7, @class: ClassType.Warrior, level: 64, strength: 1120, dexterity: 340, intelligence: 315, constitution: 840, luck: 310, health: 273000, minWeaponDmg: 79, maxWeaponDmg: 137 ),
                    new DungeonEnemy(position: 8, @class: ClassType.Scout, level: 76, strength: 350, dexterity: 1360, intelligence: 340, constitution: 1080, luck: 495, health: 332640, minWeaponDmg: 112, maxWeaponDmg: 190 ),
                    new DungeonEnemy(position: 9, @class: ClassType.Warrior, level: 86, strength: 1560, dexterity: 505, intelligence: 480, constitution: 1280, luck: 420, health: 556800, minWeaponDmg: 101, maxWeaponDmg: 159 ),
                    new DungeonEnemy(position: 10, @class: ClassType.Warrior, level: 90, strength: 1640, dexterity: 535, intelligence: 510, constitution: 1360, luck: 440, health: 618800, minWeaponDmg: 105, maxWeaponDmg: 163 )
                }
            },
            new Dungeon
            {
                Name = DungeonNames.CutthroatGrotto,
                Position = 4,
                IsDefeated = false,
                IsUnlocked = true,
                UnlockResolve = c => c.CharacterLevel >= 40,
                DungeonEnemies = new List<DungeonEnemy>
                {
                    new DungeonEnemy(position: 1, @class: ClassType.Scout, level: 52, strength: 230, dexterity: 880, intelligence: 220, constitution: 601, luck: 315, health: 127412, minWeaponDmg: 88, maxWeaponDmg: 154 ),
                    new DungeonEnemy(position: 2, @class: ClassType.Scout, level: 58, strength: 260, dexterity: 1000, intelligence: 250, constitution: 720, luck: 360, health: 169920, minWeaponDmg: 94, maxWeaponDmg: 163 ),
                    new DungeonEnemy(position: 3, @class: ClassType.Warrior, level: 62, strength: 1080, dexterity: 325, intelligence: 300, constitution: 800, luck: 300, health: 252000, minWeaponDmg: 77, maxWeaponDmg: 135 ),
                    new DungeonEnemy(position: 4, @class: ClassType.Warrior, level: 68, strength: 1200, dexterity: 370, intelligence: 345, constitution: 920, luck: 330, health: 317400, minWeaponDmg: 83, maxWeaponDmg: 141 ),
                    new DungeonEnemy(position: 5, @class: ClassType.Warrior, level: 74, strength: 1320, dexterity: 415, intelligence: 390, constitution: 1040, luck: 360, health: 390000, minWeaponDmg: 89, maxWeaponDmg: 147 ),
                    new DungeonEnemy(position: 6, @class: ClassType.Scout, level: 82, strength: 380, dexterity: 1480, intelligence: 370, constitution: 1200, luck: 540, health: 398400, minWeaponDmg: 118, maxWeaponDmg: 199 ),
                    new DungeonEnemy(position: 7, @class: ClassType.Warrior, level: 84, strength: 1520, dexterity: 490, intelligence: 465, constitution: 1240, luck: 410, health: 527000, minWeaponDmg: 99, maxWeaponDmg: 157 ),
                    new DungeonEnemy(position: 8, @class: ClassType.Warrior, level: 96, strength: 1760, dexterity: 580, intelligence: 555, constitution: 1480, luck: 470, health: 717800, minWeaponDmg: 111, maxWeaponDmg: 169 ),
                    new DungeonEnemy(position: 9, @class: ClassType.Scout, level: 102, strength: 480, dexterity: 1880, intelligence: 470, constitution: 1600, luck: 690, health: 659200, minWeaponDmg: 138, maxWeaponDmg: 229 ),
                    new DungeonEnemy(position: 10, @class: ClassType.Scout, level: 110, strength: 520, dexterity: 2040, intelligence: 510, constitution: 1760, luck: 750, health: 781440, minWeaponDmg: 146, maxWeaponDmg: 241 )
                }
            },
            new Dungeon
            {
                Name = DungeonNames.EmeraldScaleAltar,
                Position = 5,
                IsDefeated = false,
                IsUnlocked = true,
                UnlockResolve = c => c.CharacterLevel >= 50,
                DungeonEnemies = new List<DungeonEnemy>
                {
                    new DungeonEnemy(position: 1, @class: ClassType.Scout, level: 72, strength: 330, dexterity: 1280, intelligence: 320, constitution: 1000, luck: 465, health: 292000, minWeaponDmg: 108, maxWeaponDmg: 184 ),
                    new DungeonEnemy(position: 2, @class: ClassType.Scout, level: 78, strength: 360, dexterity: 1400, intelligence: 350, constitution: 1120, luck: 510, health: 353920, minWeaponDmg: 114, maxWeaponDmg: 193 ),
                    new DungeonEnemy(position: 3, @class: ClassType.Scout, level: 80, strength: 370, dexterity: 1440, intelligence: 360, constitution: 1160, luck: 525, health: 375840, minWeaponDmg: 116, maxWeaponDmg: 196 ),
                    new DungeonEnemy(position: 4, @class: ClassType.Scout, level: 88, strength: 410, dexterity: 1600, intelligence: 400, constitution: 1320, luck: 585, health: 469920, minWeaponDmg: 125, maxWeaponDmg: 208 ),
                    new DungeonEnemy(position: 5, @class: ClassType.Warrior, level: 94, strength: 1720, dexterity: 565, intelligence: 540, constitution: 1440, luck: 460, health: 684000, minWeaponDmg: 109, maxWeaponDmg: 167 ),
                    new DungeonEnemy(position: 6, @class: ClassType.Scout, level: 100, strength: 470, dexterity: 1840, intelligence: 460, constitution: 1560, luck: 675, health: 630240, minWeaponDmg: 136, maxWeaponDmg: 226 ),
                    new DungeonEnemy(position: 7, @class: ClassType.Warrior, level: 108, strength: 2000, dexterity: 670, intelligence: 645, constitution: 1720, luck: 530, health: 937400, minWeaponDmg: 123, maxWeaponDmg: 181 ),
                    new DungeonEnemy(position: 8, @class: ClassType.Mage, level: 114, strength: 520, dexterity: 540, intelligence: 2200, constitution: 1760, luck: 775, health: 404800, minWeaponDmg: 287, maxWeaponDmg: 432 ),
                    new DungeonEnemy(position: 9, @class: ClassType.Warrior, level: 122, strength: 2280, dexterity: 775, intelligence: 750, constitution: 2000, luck: 600, health: 1230000, minWeaponDmg: 137, maxWeaponDmg: 195 ),
                    new DungeonEnemy(position: 10, @class: ClassType.Scout, level: 130, strength: 620, dexterity: 2440, intelligence: 610, constitution: 2160, luck: 900, health: 1131840, minWeaponDmg: 166, maxWeaponDmg: 271 )
                }
            },
            new Dungeon
            {
                Name = DungeonNames.ToxicTree,
                Position = 6,
                IsDefeated = false,
                IsUnlocked = true,
                UnlockResolve = c => c.CharacterLevel >= 70,
                DungeonEnemies = new List<DungeonEnemy>
                {
                    new DungeonEnemy(position: 1, @class: ClassType.Warrior, level: 92, strength: 1680, dexterity: 550, intelligence: 525, constitution: 1400, luck: 450, health: 651000, minWeaponDmg: 107, maxWeaponDmg: 165 ),
                    new DungeonEnemy(position: 2, @class: ClassType.Scout, level: 98, strength: 460, dexterity: 1800, intelligence: 450, constitution: 1520, luck: 660, health: 601920, minWeaponDmg: 134, maxWeaponDmg: 223 ),
                    new DungeonEnemy(position: 3, @class: ClassType.Mage, level: 104, strength: 470, dexterity: 490, intelligence: 2000, constitution: 1560, luck: 700, health: 327600, minWeaponDmg: 268, maxWeaponDmg: 407 ),
                    new DungeonEnemy(position: 4, @class: ClassType.Scout, level: 106, strength: 500, dexterity: 1960, intelligence: 490, constitution: 1680, luck: 720, health: 719040, minWeaponDmg: 142, maxWeaponDmg: 235 ),
                    new DungeonEnemy(position: 5, @class: ClassType.Scout, level: 118, strength: 560, dexterity: 2200, intelligence: 550, constitution: 1920, luck: 810, health: 913920, minWeaponDmg: 154, maxWeaponDmg: 253 ),
                    new DungeonEnemy(position: 6, @class: ClassType.Warrior, level: 124, strength: 2320, dexterity: 790, intelligence: 765, constitution: 2040, luck: 610, health: 1275000, minWeaponDmg: 139, maxWeaponDmg: 197 ),
                    new DungeonEnemy(position: 7, @class: ClassType.Scout, level: 128, strength: 610, dexterity: 2400, intelligence: 600, constitution: 2120, luck: 885, health: 1093920, minWeaponDmg: 164, maxWeaponDmg: 268 ),
                    new DungeonEnemy(position: 8, @class: ClassType.Mage, level: 136, strength: 630, dexterity: 650, intelligence: 2640, constitution: 2200, luck: 940, health: 602800, minWeaponDmg: 332, maxWeaponDmg: 487 ),
                    new DungeonEnemy(position: 9, @class: ClassType.Scout, level: 144, strength: 690, dexterity: 2720, intelligence: 680, constitution: 2440, luck: 1005, health: 1415200, minWeaponDmg: 180, maxWeaponDmg: 292 ),
                    new DungeonEnemy(position: 10, @class: ClassType.Scout, level: 150, strength: 720, dexterity: 2840, intelligence: 710, constitution: 2560, luck: 1050, health: 1546240, minWeaponDmg: 186, maxWeaponDmg: 301 )
                }
            },
            new Dungeon
            {
                Name = DungeonNames.MagmaStream,
                Position = 7,
                IsDefeated = false,
                IsUnlocked = true,
                UnlockResolve = c => c.CharacterLevel >= 80,
                DungeonEnemies = new List<DungeonEnemy>
                {
                    new DungeonEnemy(position: 1, @class: ClassType.Scout, level: 112, strength: 530, dexterity: 2080, intelligence: 520, constitution: 1800, luck: 765, health: 813600, minWeaponDmg: 148, maxWeaponDmg: 244 ),
                    new DungeonEnemy(position: 2, @class: ClassType.Scout, level: 116, strength: 550, dexterity: 2160, intelligence: 540, constitution: 1880, luck: 795, health: 879840, minWeaponDmg: 152, maxWeaponDmg: 249 ),
                    new DungeonEnemy(position: 3, @class: ClassType.Mage, level: 120, strength: 550, dexterity: 570, intelligence: 2320, constitution: 1880, luck: 820, health: 454960, minWeaponDmg: 299, maxWeaponDmg: 447 ),
                    new DungeonEnemy(position: 4, @class: ClassType.Warrior, level: 126, strength: 2360, dexterity: 805, intelligence: 780, constitution: 2080, luck: 620, health: 1320800, minWeaponDmg: 141, maxWeaponDmg: 199 ),
                    new DungeonEnemy(position: 5, @class: ClassType.Warrior, level: 134, strength: 2520, dexterity: 865, intelligence: 840, constitution: 2240, luck: 660, health: 1512000, minWeaponDmg: 149, maxWeaponDmg: 207 ),
                    new DungeonEnemy(position: 6, @class: ClassType.Warrior, level: 138, strength: 2600, dexterity: 895, intelligence: 870, constitution: 2320, luck: 680, health: 1612400, minWeaponDmg: 153, maxWeaponDmg: 211 ),
                    new DungeonEnemy(position: 7, @class: ClassType.Mage, level: 142, strength: 660, dexterity: 680, intelligence: 2760, constitution: 2320, luck: 985, health: 663520, minWeaponDmg: 343, maxWeaponDmg: 496 ),
                    new DungeonEnemy(position: 8, @class: ClassType.Warrior, level: 146, strength: 2760, dexterity: 955, intelligence: 930, constitution: 2480, luck: 720, health: 1822800, minWeaponDmg: 161, maxWeaponDmg: 219 ),
                    new DungeonEnemy(position: 9, @class: ClassType.Warrior, level: 148, strength: 2800, dexterity: 970, intelligence: 945, constitution: 2520, luck: 730, health: 1877400, minWeaponDmg: 163, maxWeaponDmg: 221 ),
                    new DungeonEnemy(position: 10, @class: ClassType.Warrior, level: 170, strength: 3240, dexterity: 1135, intelligence: 1110, constitution: 2960, luck: 840, health: 2530800, minWeaponDmg: 185, maxWeaponDmg: 243 )
                }
            },
            new Dungeon
            {
                Name = DungeonNames.FrostBloodTemple,
                Position = 8,
                IsDefeated = false,
                IsUnlocked = true,
                UnlockResolve = c => c.CharacterLevel >= 95,
                DungeonEnemies = new List<DungeonEnemy>
                {
                    new DungeonEnemy(position: 1, @class: ClassType.Warrior, level: 132, strength: 2480, dexterity: 850, intelligence: 825, constitution: 2200, luck: 650, health: 1463000, minWeaponDmg: 147, maxWeaponDmg: 204 ),
                    new DungeonEnemy(position: 2, @class: ClassType.Scout, level: 140, strength: 670, dexterity: 2640, intelligence: 660, constitution: 2360, luck: 975, health: 1331040, minWeaponDmg: 176, maxWeaponDmg: 286 ),
                    new DungeonEnemy(position: 3, @class: ClassType.Warrior, level: 154, strength: 2920, dexterity: 1015, intelligence: 990, constitution: 2640, luck: 760, health: 2046000, minWeaponDmg: 169, maxWeaponDmg: 227 ),
                    new DungeonEnemy(position: 4, @class: ClassType.Scout, level: 158, strength: 760, dexterity: 3000, intelligence: 750, constitution: 2720, luck: 1110, health: 1729920, minWeaponDmg: 194, maxWeaponDmg: 313 ),
                    new DungeonEnemy(position: 5, @class: ClassType.Scout, level: 164, strength: 790, dexterity: 3120, intelligence: 780, constitution: 2840, luck: 1155, health: 1874400, minWeaponDmg: 200, maxWeaponDmg: 322 ),
                    new DungeonEnemy(position: 6, @class: ClassType.Mage, level: 168, strength: 790, dexterity: 810, intelligence: 3280, constitution: 2840, luck: 1180, health: 959920, minWeaponDmg: 395, maxWeaponDmg: 565 ),
                    new DungeonEnemy(position: 7, @class: ClassType.Warrior, level: 172, strength: 3280, dexterity: 1150, intelligence: 1125, constitution: 3000, luck: 850, health: 2595000, minWeaponDmg: 187, maxWeaponDmg: 245 ),
                    new DungeonEnemy(position: 8, @class: ClassType.Scout, level: 180, strength: 870, dexterity: 3440, intelligence: 860, constitution: 3160, luck: 1275, health: 2287840, minWeaponDmg: 216, maxWeaponDmg: 346 ),
                    new DungeonEnemy(position: 9, @class: ClassType.Mage, level: 185, strength: 875, dexterity: 895, intelligence: 3620, constitution: 3180, luck: 1305, health: 1182960, minWeaponDmg: 429, maxWeaponDmg: 608 ),
                    new DungeonEnemy(position: 10, @class: ClassType.Mage, level: 200, strength: 950, dexterity: 970, intelligence: 3920, constitution: 3480, luck: 1410, health: 1398960, minWeaponDmg: 459, maxWeaponDmg: 645 )
                }
            },
            new Dungeon
            {
                Name = DungeonNames.PyramidsOfMadness,
                Position = 9,
                IsDefeated = false,
                IsUnlocked = true,
                UnlockResolve = c => c.CharacterLevel >= 110,
                DungeonEnemies = new List<DungeonEnemy>
                {
                    new DungeonEnemy(position: 1, @class: ClassType.Warrior, level: 152, strength: 2880, dexterity: 1000, intelligence: 975, constitution: 2600, luck: 750, health: 1989000, minWeaponDmg: 167, maxWeaponDmg: 225 ),
                    new DungeonEnemy(position: 2, @class: ClassType.Mage, level: 156, strength: 730, dexterity: 750, intelligence: 3040, constitution: 2600, luck: 1090, health: 816400, minWeaponDmg: 374, maxWeaponDmg: 536 ),
                    new DungeonEnemy(position: 3, @class: ClassType.Scout, level: 160, strength: 770, dexterity: 3040, intelligence: 760, constitution: 2760, luck: 1125, health: 1777440, minWeaponDmg: 196, maxWeaponDmg: 316 ),
                    new DungeonEnemy(position: 4, @class: ClassType.Warrior, level: 162, strength: 3080, dexterity: 1075, intelligence: 1050, constitution: 2800, luck: 800, health: 2282000, minWeaponDmg: 177, maxWeaponDmg: 235 ),
                    new DungeonEnemy(position: 5, @class: ClassType.Mage, level: 166, strength: 780, dexterity: 800, intelligence: 3240, constitution: 2800, luck: 1165, health: 935200, minWeaponDmg: 391, maxWeaponDmg: 562 ),
                    new DungeonEnemy(position: 6, @class: ClassType.Warrior, level: 174, strength: 3320, dexterity: 1165, intelligence: 1140, constitution: 3040, luck: 860, health: 2660000, minWeaponDmg: 189, maxWeaponDmg: 247 ),
                    new DungeonEnemy(position: 7, @class: ClassType.Scout, level: 176, strength: 850, dexterity: 3360, intelligence: 840, constitution: 3080, luck: 1245, health: 2180640, minWeaponDmg: 212, maxWeaponDmg: 340 ),
                    new DungeonEnemy(position: 8, @class: ClassType.Scout, level: 178, strength: 860, dexterity: 3400, intelligence: 850, constitution: 3120, luck: 1260, health: 2233920, minWeaponDmg: 214, maxWeaponDmg: 343 ),
                    new DungeonEnemy(position: 9, @class: ClassType.Mage, level: 190, strength: 900, dexterity: 920, intelligence: 3720, constitution: 3280, luck: 1340, health: 1252960, minWeaponDmg: 439, maxWeaponDmg: 621 ),
                    new DungeonEnemy(position: 10, @class: ClassType.Mirror, level: 195, strength: 0, dexterity: 0, intelligence: 0, constitution: 0, luck: 0, health: 0, minWeaponDmg: 0, maxWeaponDmg: 0, armor: 0)
                }
            },
            new Dungeon
            {
                Name = DungeonNames.BlackSkullFortress,
                Position = 10,
                IsDefeated = false,
                IsUnlocked = true,
                UnlockResolve = c => c.Dungeons.Where(d => d.Position <= 9).All(d => d.IsDefeated),
                DungeonEnemies = new List<DungeonEnemy>
                {
                    new DungeonEnemy(position: 1, @class: ClassType.Scout, level: 205, strength: 995, dexterity: 3940, intelligence: 985, constitution: 3660, luck: 1450, health: 3015840, minWeaponDmg: 241, maxWeaponDmg: 386 ),
                    new DungeonEnemy(position: 2, @class: ClassType.Warrior, level: 210, strength: 4040, dexterity: 1420, intelligence: 1395, constitution: 3760, luck: 1010, health: 3966800, minWeaponDmg: 225, maxWeaponDmg: 284 ),
                    new DungeonEnemy(position: 3, @class: ClassType.Warrior, level: 215, strength: 4140, dexterity: 1455, intelligence: 1430, constitution: 3860, luck: 1030, health: 4168800, minWeaponDmg: 229, maxWeaponDmg: 289 ),
                    new DungeonEnemy(position: 4, @class: ClassType.Warrior, level: 220, strength: 4240, dexterity: 1490, intelligence: 1465, constitution: 3960, luck: 1050, health: 4375800, minWeaponDmg: 235, maxWeaponDmg: 294 ),
                    new DungeonEnemy(position: 5, @class: ClassType.Scout, level: 225, strength: 1095, dexterity: 4340, intelligence: 1085, constitution: 4060, luck: 1590, health: 3670240, minWeaponDmg: 261, maxWeaponDmg: 418 ),
                    new DungeonEnemy(position: 6, @class: ClassType.Scout, level: 230, strength: 1120, dexterity: 4440, intelligence: 1110, constitution: 4160, luck: 1625, health: 3843840, minWeaponDmg: 266, maxWeaponDmg: 426 ),
                    new DungeonEnemy(position: 7, @class: ClassType.Mage, level: 235, strength: 1125, dexterity: 1145, intelligence: 4620, constitution: 4180, luck: 1655, health: 1972960, minWeaponDmg: 528, maxWeaponDmg: 727 ),
                    new DungeonEnemy(position: 8, @class: ClassType.Warrior, level: 240, strength: 4640, dexterity: 1630, intelligence: 1605, constitution: 4360, luck: 1130, health: 5253800, minWeaponDmg: 255, maxWeaponDmg: 314 ),
                    new DungeonEnemy(position: 9, @class: ClassType.Warrior, level: 245, strength: 4740, dexterity: 1665, intelligence: 1640, constitution: 4460, luck: 1150, health: 5485800, minWeaponDmg: 260, maxWeaponDmg: 319 ),
                    new DungeonEnemy(position: 10, @class: ClassType.Warrior, level: 250, strength: 4840, dexterity: 1700, intelligence: 1675, constitution: 4560, luck: 1170, health: 5722800, minWeaponDmg: 265, maxWeaponDmg: 324 )
                }
            },
            new Dungeon
            {
                Name = DungeonNames.CircusOfTerror,
                Position = 11,
                IsDefeated = false,
                IsUnlocked = true,
                UnlockResolve = c => c.Dungeons.First(d => d.Position == 10).IsDefeated,
                DungeonEnemies = new List<DungeonEnemy>
                {
                    new DungeonEnemy(position: 1, @class: ClassType.Warrior, level: 255, strength: 4940, dexterity: 1735, intelligence: 1710, constitution: 4660, luck: 1190, health: 5964800, minWeaponDmg: 270, maxWeaponDmg: 339 ),
                    new DungeonEnemy(position: 2, @class: ClassType.Scout, level: 260, strength: 1270, dexterity: 5040, intelligence: 1260, constitution: 4760, luck: 1835, health: 4969440, minWeaponDmg: 306, maxWeaponDmg: 488 ),
                    new DungeonEnemy(position: 3, @class: ClassType.Warrior, level: 265, strength: 5140, dexterity: 1805, intelligence: 1780, constitution: 4860, luck: 1230, health: 6463800, minWeaponDmg: 280, maxWeaponDmg: 369 ),
                    new DungeonEnemy(position: 4, @class: ClassType.Warrior, level: 270, strength: 5240, dexterity: 1840, intelligence: 1815, constitution: 4960, luck: 1250, health: 6720800, minWeaponDmg: 285, maxWeaponDmg: 384 ),
                    new DungeonEnemy(position: 5, @class: ClassType.Mage, level: 275, strength: 1325, dexterity: 1345, intelligence: 5420, constitution: 4980, luck: 1935, health: 2748960, minWeaponDmg: 609, maxWeaponDmg: 939 ),
                    new DungeonEnemy(position: 6, @class: ClassType.Scout, level: 280, strength: 1370, dexterity: 5440, intelligence: 1360, constitution: 5160, luck: 1975, health: 5799840, minWeaponDmg: 346, maxWeaponDmg: 548 ),
                    new DungeonEnemy(position: 7, @class: ClassType.Warrior, level: 285, strength: 5540, dexterity: 1945, intelligence: 1920, constitution: 5260, luck: 1310, health: 7521800, minWeaponDmg: 300, maxWeaponDmg: 429 ),
                    new DungeonEnemy(position: 8, @class: ClassType.Scout, level: 290, strength: 1420, dexterity: 5640, intelligence: 1410, constitution: 5360, luck: 2045, health: 6239040, minWeaponDmg: 366, maxWeaponDmg: 578 ),
                    new DungeonEnemy(position: 9, @class: ClassType.Mage, level: 295, strength: 1425, dexterity: 1445, intelligence: 5820, constitution: 5380, luck: 2075, health: 3184960, minWeaponDmg: 653, maxWeaponDmg: 1079 ),
                    new DungeonEnemy(position: 10, @class: ClassType.Warrior, level: 300, strength: 5840, dexterity: 2050, intelligence: 2025, constitution: 5560, luck: 1370, health: 8367800, minWeaponDmg: 315, maxWeaponDmg: 474 )
                }
            },
            new Dungeon
            {
                Name = DungeonNames.Hell,
                Position = 12,
                IsDefeated = false,
                IsUnlocked = true,
                UnlockResolve = c => c.Dungeons.First(d => d.Position == 11).IsDefeated,
                DungeonEnemies = new List<DungeonEnemy>
                {
                    new DungeonEnemy(position: 1, @class: ClassType.Mage, level: 305, strength: 1475, dexterity: 1495, intelligence: 6020, constitution: 5580, luck: 2145, health: 3414960, minWeaponDmg: 669, maxWeaponDmg: 1150 ),
                    new DungeonEnemy(position: 2, @class: ClassType.Mage, level: 310, strength: 1500, dexterity: 1520, intelligence: 6120, constitution: 5680, luck: 2180, health: 3532960, minWeaponDmg: 679, maxWeaponDmg: 1185 ),
                    new DungeonEnemy(position: 3, @class: ClassType.Warrior, level: 315, strength: 6140, dexterity: 2155, intelligence: 2130, constitution: 5860, luck: 1430, health: 9258800, minWeaponDmg: 330, maxWeaponDmg: 519 ),
                    new DungeonEnemy(position: 4, @class: ClassType.Scout, level: 320, strength: 1570, dexterity: 6240, intelligence: 1560, constitution: 5960, luck: 2255, health: 7652640, minWeaponDmg: 426, maxWeaponDmg: 667 ),
                    new DungeonEnemy(position: 5, @class: ClassType.Mage, level: 325, strength: 1575, dexterity: 1595, intelligence: 6420, constitution: 5980, luck: 2285, health: 3898960, minWeaponDmg: 711, maxWeaponDmg: 1289 ),
                    new DungeonEnemy(position: 6, @class: ClassType.Warrior, level: 330, strength: 6440, dexterity: 2260, intelligence: 2235, constitution: 6160, luck: 1490, health: 10194800, minWeaponDmg: 345, maxWeaponDmg: 564 ),
                    new DungeonEnemy(position: 7, @class: ClassType.Scout, level: 335, strength: 1645, dexterity: 6540, intelligence: 1635, constitution: 6260, luck: 2360, health: 8413440, minWeaponDmg: 457, maxWeaponDmg: 713 ),
                    new DungeonEnemy(position: 8, @class: ClassType.Warrior, level: 340, strength: 6640, dexterity: 2330, intelligence: 2305, constitution: 6360, luck: 1530, health: 10843800, minWeaponDmg: 356, maxWeaponDmg: 594 ),
                    new DungeonEnemy(position: 9, @class: ClassType.Warrior, level: 345, strength: 6740, dexterity: 2365, intelligence: 2340, constitution: 6460, luck: 1550, health: 11175800, minWeaponDmg: 360, maxWeaponDmg: 609 ),
                    new DungeonEnemy(position: 10, @class: ClassType.Warrior, level: 350, strength: 6840, dexterity: 2400, intelligence: 2375, constitution: 6560, luck: 1570, health: 11512800, minWeaponDmg: 365, maxWeaponDmg: 624 )
                }
            },
            new Dungeon
            {
                Name = DungeonNames.DragonsHoard,
                Position = 13,
                IsDefeated = false,
                IsUnlocked = true,
                UnlockResolve = c => c.CharacterLevel >= 210,
                DungeonEnemies = new List<DungeonEnemy>
                {
                    new DungeonEnemy(position: 1, @class: ClassType.Scout, level: 210, strength: 3100, dexterity: 6200, intelligence: 3100, constitution: 10500, luck: 3100, health: 16000000, minWeaponDmg: 559, maxWeaponDmg: 787, dungeonRuneBonuses: new DungeonEnemyRuneBonuses { DamageRuneType = RuneType.FireDamage, FireResistance = 25, ColdResistance = 0, LightningResistance = 0, DamageBonus = 25 }, armor: 10750 ),
                    new DungeonEnemy(position: 2, @class: ClassType.Warrior, level: 213, strength: 6560, dexterity: 3280, intelligence: 3280, constitution: 12000, luck: 3280, health: 20500000, minWeaponDmg: 561, maxWeaponDmg: 772, dungeonRuneBonuses: new DungeonEnemyRuneBonuses { DamageRuneType = RuneType.LightningDamage, FireResistance = 0, ColdResistance = 0, LightningResistance = 25, DamageBonus = 25 }, armor: 11000 ),
                    new DungeonEnemy(position: 3, @class: ClassType.Mage, level: 216, strength: 3460, dexterity: 3460, intelligence: 6920, constitution: 13500, luck: 3460, health: 25000000, minWeaponDmg: 567, maxWeaponDmg: 787, dungeonRuneBonuses: new DungeonEnemyRuneBonuses { DamageRuneType = RuneType.LightningDamage, FireResistance = 0, ColdResistance = 0, LightningResistance = 25, DamageBonus = 25 }, armor: 11250 ),
                    new DungeonEnemy(position: 4, @class: ClassType.Scout, level: 219, strength: 3640, dexterity: 7280, intelligence: 3640, constitution: 15000, luck: 3640, health: 29500000, minWeaponDmg: 582, maxWeaponDmg: 819, dungeonRuneBonuses: new DungeonEnemyRuneBonuses { DamageRuneType = RuneType.ColdDamage, FireResistance = 0, ColdResistance = 25, LightningResistance = 0, DamageBonus = 25 }, armor: 11500 ),
                    new DungeonEnemy(position: 5, @class: ClassType.Scout, level: 222, strength: 3820, dexterity: 7640, intelligence: 3820, constitution: 16500, luck: 3820, health: 34000000, minWeaponDmg: 598, maxWeaponDmg: 837, dungeonRuneBonuses: new DungeonEnemyRuneBonuses { DamageRuneType = RuneType.FireDamage, FireResistance = 25, ColdResistance = 0, LightningResistance = 0, DamageBonus = 25 }, armor: 11750 ),
                    new DungeonEnemy(position: 6, @class: ClassType.Scout, level: 225, strength: 4000, dexterity: 8000, intelligence: 4000, constitution: 18000, luck: 4000, health: 38500000, minWeaponDmg: 601, maxWeaponDmg: 850, dungeonRuneBonuses: new DungeonEnemyRuneBonuses { DamageRuneType = RuneType.LightningDamage, FireResistance = 0, ColdResistance = 0, LightningResistance = 25, DamageBonus = 25 }, armor: 12000 ),
                    new DungeonEnemy(position: 7, @class: ClassType.Warrior, level: 228, strength: 8360, dexterity: 4180, intelligence: 4180, constitution: 19500, luck: 4180, health: 43000000, minWeaponDmg: 614, maxWeaponDmg: 862, dungeonRuneBonuses: new DungeonEnemyRuneBonuses { DamageRuneType = RuneType.LightningDamage, FireResistance = 0, ColdResistance = 0, LightningResistance = 25, DamageBonus = 25 }, armor: 12250 ),
                    new DungeonEnemy(position: 8, @class: ClassType.Warrior, level: 231, strength: 8720, dexterity: 4360, intelligence: 4360, constitution: 21000, luck: 4360, health: 47500000, minWeaponDmg: 626, maxWeaponDmg: 874, dungeonRuneBonuses: new DungeonEnemyRuneBonuses { DamageRuneType = RuneType.FireDamage, FireResistance = 25, ColdResistance = 0, LightningResistance = 0, DamageBonus = 25 }, armor: 12500 ),
                    new DungeonEnemy(position: 9, @class: ClassType.Scout, level: 234, strength: 4540, dexterity: 9080, intelligence: 4540, constitution: 22500, luck: 4540, health: 52000000, minWeaponDmg: 637, maxWeaponDmg: 885, dungeonRuneBonuses: new DungeonEnemyRuneBonuses { DamageRuneType = RuneType.ColdDamage, FireResistance = 0, ColdResistance = 25, LightningResistance = 0, DamageBonus = 25 }, armor: 12750 ),
                    new DungeonEnemy(position: 10, @class: ClassType.Mage, level: 240, strength: 4900, dexterity: 4900, intelligence: 9800, constitution: 25500, luck: 4900, health: 61000000, minWeaponDmg: 663, maxWeaponDmg: 909, dungeonRuneBonuses: new DungeonEnemyRuneBonuses { DamageRuneType = RuneType.ColdDamage, FireResistance = 0, ColdResistance = 25, LightningResistance = 0, DamageBonus = 25 }, armor: 13000 )
                }
            },
            new Dungeon
            {
                Name = DungeonNames.HouseOfHorrors,
                Position = 14,
                IsDefeated = false,
                IsUnlocked = true,
                UnlockResolve = c => c.CharacterLevel >= 240,
                DungeonEnemies = new List<DungeonEnemy>
                {
                    new DungeonEnemy(position: 1, @class: ClassType.Mage, level: 240, strength: 4900, dexterity: 4900, intelligence: 9800, constitution: 25500, luck: 4900, health: 61000000, minWeaponDmg: 662, maxWeaponDmg: 908, dungeonRuneBonuses: new DungeonEnemyRuneBonuses { DamageRuneType = RuneType.ColdDamage, FireResistance = 0, ColdResistance = 25, LightningResistance = 0, DamageBonus = 25 }, armor: 13250 ),
                    new DungeonEnemy(position: 2, @class: ClassType.Warrior, level: 243, strength: 10160, dexterity: 5080, intelligence: 5080, constitution: 27000, luck: 5080, health: 65500000, minWeaponDmg: 673, maxWeaponDmg: 922, dungeonRuneBonuses: new DungeonEnemyRuneBonuses { DamageRuneType = RuneType.LightningDamage, FireResistance = 0, ColdResistance = 0, LightningResistance = 25, DamageBonus = 25 }, armor: 13500 ),
                    new DungeonEnemy(position: 3, @class: ClassType.Warrior, level: 246, strength: 10520, dexterity: 5260, intelligence: 5260, constitution: 28500, luck: 5260, health: 70000000, minWeaponDmg: 685, maxWeaponDmg: 933, dungeonRuneBonuses: new DungeonEnemyRuneBonuses { DamageRuneType = RuneType.LightningDamage, FireResistance = 0, ColdResistance = 0, LightningResistance = 25, DamageBonus = 25 }, armor: 13750 ),
                    new DungeonEnemy(position: 4, @class: ClassType.Warrior, level: 249, strength: 10880, dexterity: 5440, intelligence: 5440, constitution: 30000, luck: 5440, health: 74500000, minWeaponDmg: 697, maxWeaponDmg: 946, dungeonRuneBonuses: new DungeonEnemyRuneBonuses { DamageRuneType = RuneType.FireDamage, FireResistance = 25, ColdResistance = 0, LightningResistance = 0, DamageBonus = 25 }, armor: 14000 ),
                    new DungeonEnemy(position: 5, @class: ClassType.Scout, level: 252, strength: 5620, dexterity: 11240, intelligence: 5620, constitution: 31500, luck: 5620, health: 79000000, minWeaponDmg: 709, maxWeaponDmg: 958, dungeonRuneBonuses: new DungeonEnemyRuneBonuses { DamageRuneType = RuneType.ColdDamage,FireResistance = 0, ColdResistance = 25, LightningResistance = 0, DamageBonus = 25 }, armor: 14250 ),
                    new DungeonEnemy(position: 6, @class: ClassType.Mage, level: 255, strength: 5800, dexterity: 5800, intelligence: 11600, constitution: 33000, luck: 5800, health: 83500000, minWeaponDmg: 721, maxWeaponDmg: 966, dungeonRuneBonuses: new DungeonEnemyRuneBonuses { DamageRuneType = RuneType.FireDamage, FireResistance = 25, ColdResistance = 0, LightningResistance = 0, DamageBonus = 25 }, armor: 14500 ),
                    new DungeonEnemy(position: 7, @class: ClassType.Scout, level: 258, strength: 5980, dexterity: 11960, intelligence: 5980, constitution: 34500, luck: 5980, health: 88000000, minWeaponDmg: 733, maxWeaponDmg: 982, dungeonRuneBonuses: new DungeonEnemyRuneBonuses { DamageRuneType = RuneType.LightningDamage, FireResistance = 0, ColdResistance = 0, LightningResistance = 25, DamageBonus = 25 }, armor: 14750 ),
                    new DungeonEnemy(position: 8, @class: ClassType.Mage, level: 261, strength: 6160, dexterity: 6160, intelligence: 12320, constitution: 36000, luck: 6160, health: 92500000, minWeaponDmg: 744, maxWeaponDmg: 994, dungeonRuneBonuses: new DungeonEnemyRuneBonuses { DamageRuneType = RuneType.FireDamage, FireResistance = 25, ColdResistance = 0, LightningResistance = 0, DamageBonus = 25 }, armor: 15000 ),
                    new DungeonEnemy(position: 9, @class: ClassType.Mage, level: 264, strength: 6340, dexterity: 6340, intelligence: 12680, constitution: 37500, luck: 6340, health: 97000000, minWeaponDmg: 756, maxWeaponDmg: 1002, dungeonRuneBonuses: new DungeonEnemyRuneBonuses { DamageRuneType = RuneType.FireDamage, FireResistance = 25, ColdResistance = 0, LightningResistance = 0, DamageBonus = 25 }, armor: 15250 ),
                    new DungeonEnemy(position: 10, @class: ClassType.Scout, level: 270, strength: 6700, dexterity: 13400, intelligence: 6700, constitution: 40500, luck: 6700, health: 106000000, minWeaponDmg: 781, maxWeaponDmg: 1029, dungeonRuneBonuses: new DungeonEnemyRuneBonuses { DamageRuneType = RuneType.ColdDamage, FireResistance = 0, ColdResistance = 25, LightningResistance = 0, DamageBonus = 25 }, armor: 15500 )
                }
            },
            new Dungeon
            {
                Name = DungeonNames.ThirteenthFloor,
                Position = 15,
                IsDefeated = false,
                IsUnlocked = true,
                UnlockResolve = c => c.Dungeons.First(d => d.Position == 12).IsUnlocked,
                DungeonEnemies = new List<DungeonEnemy>
                {
                    new DungeonEnemy(position: 1, @class: ClassType.Warrior, level: 355, strength: 7570, dexterity: 2655, intelligence: 2630, constitution: 7290, luck: 1716, health: 12976200, minWeaponDmg: 401, maxWeaponDmg: 733 ),
                    new DungeonEnemy(position: 2, @class: ClassType.Mage, level: 360, strength: 1970, dexterity: 1990, intelligence: 8000, constitution: 7560, luck: 2838, health: 5458320, minWeaponDmg: 870, maxWeaponDmg: 1843 ),
                    new DungeonEnemy(position: 3, @class: ClassType.Warrior, level: 365, strength: 8290, dexterity: 2908, intelligence: 2882, constitution: 8010, luck: 1860, health: 14658300, minWeaponDmg: 438, maxWeaponDmg: 841 ),
                    new DungeonEnemy(position: 4, @class: ClassType.Mage, level: 370, strength: 2160, dexterity: 2180, intelligence: 8760, constitution: 8320, luck: 3104, health: 6173440, minWeaponDmg: 943, maxWeaponDmg: 2108 ),
                    new DungeonEnemy(position: 5, @class: ClassType.Warrior, level: 375, strength: 9340, dexterity: 3275, intelligence: 3250, constitution: 9060, luck: 2070, health: 17032800, minWeaponDmg: 490, maxWeaponDmg: 999 ),
                    new DungeonEnemy(position: 6, @class: ClassType.Scout, level: 380, strength: 2682, dexterity: 10690, intelligence: 2672, constitution: 10410, luck: 3812, health: 15864840, minWeaponDmg: 871, maxWeaponDmg: 1335 ),
                    new DungeonEnemy(position: 7, @class: ClassType.Mage, level: 385, strength: 2888, dexterity: 2908, intelligence: 11670, constitution: 11230, luck: 4122, health: 8669560, minWeaponDmg: 1235, maxWeaponDmg: 3121 ),
                    new DungeonEnemy(position: 8, @class: ClassType.Warrior, level: 390, strength: 12540, dexterity: 4395, intelligence: 4370, constitution: 12260, luck: 2710, health: 23968300, minWeaponDmg: 650, maxWeaponDmg: 1479 ),
                    new DungeonEnemy(position: 9, @class: ClassType.Warrior, level: 395, strength: 13540, dexterity: 4745, intelligence: 4720, constitution: 13260, luck: 2910, health: 26254800, minWeaponDmg: 700, maxWeaponDmg: 1629 ),
                    new DungeonEnemy(position: 10, @class: ClassType.Warrior, level: 400, strength: 16840, dexterity: 5900, intelligence: 5875, constitution: 16560, luck: 3570, health: 33202800, minWeaponDmg: 866, maxWeaponDmg: 2124 )
                }
            },
            new Dungeon
            {
                Name = DungeonNames.ThirdLeagueOfSuperheroes,
                Position = 16,
                IsDefeated = false,
                IsUnlocked = true,
                UnlockResolve = c => c.CharacterLevel >= 280,
                DungeonEnemies = new List<DungeonEnemy>
                {
                    new DungeonEnemy(position: 1, @class: ClassType.Mage, level: 280, strength: 7300, dexterity: 7300, intelligence: 14600, constitution: 45500, luck: 7300, health: 121000000, minWeaponDmg: 820, maxWeaponDmg: 1069, dungeonRuneBonuses: new DungeonEnemyRuneBonuses { DamageRuneType = RuneType.LightningDamage, FireResistance = 0, ColdResistance = 0, LightningResistance = 25, DamageBonus = 25 }, armor: 16500 ),
                    new DungeonEnemy(position: 2, @class: ClassType.Warrior, level: 283, strength: 14960, dexterity: 7480, intelligence: 7480, constitution: 47000, luck: 7480, health: 125500000, minWeaponDmg: 832, maxWeaponDmg: 1080, dungeonRuneBonuses: new DungeonEnemyRuneBonuses { DamageRuneType = RuneType.LightningDamage, FireResistance = 0, ColdResistance = 0, LightningResistance = 25, DamageBonus = 25 }, armor: 16750 ),
                    new DungeonEnemy(position: 3, @class: ClassType.Scout, level: 286, strength: 7660, dexterity: 15320, intelligence: 7660, constitution: 48500, luck: 7660, health: 130000000, minWeaponDmg: 847, maxWeaponDmg: 1094, dungeonRuneBonuses: new DungeonEnemyRuneBonuses { DamageRuneType = RuneType.ColdDamage, FireResistance = 0, ColdResistance = 25, LightningResistance = 0, DamageBonus = 25 }, armor: 17000 ),
                    new DungeonEnemy(position: 4, @class: ClassType.Warrior, level: 289, strength: 15680, dexterity: 7840, intelligence: 7840, constitution: 50000, luck: 7840, health: 134500000, minWeaponDmg: 857, maxWeaponDmg: 1106, dungeonRuneBonuses: new DungeonEnemyRuneBonuses { DamageRuneType = RuneType.FireDamage, FireResistance = 25, ColdResistance = 0, LightningResistance = 0, DamageBonus = 25 }, armor: 17250 ),
                    new DungeonEnemy(position: 5, @class: ClassType.Scout, level: 292, strength: 8020, dexterity: 16040, intelligence: 8020, constitution: 51500, luck: 8020, health: 139000000, minWeaponDmg: 868, maxWeaponDmg: 1118, dungeonRuneBonuses: new DungeonEnemyRuneBonuses { DamageRuneType = RuneType.ColdDamage, FireResistance = 0, ColdResistance = 25, LightningResistance = 0, DamageBonus = 25 }, armor: 17500 ),
                    new DungeonEnemy(position: 6, @class: ClassType.Warrior, level: 295, strength: 16400, dexterity: 8200, intelligence: 8200, constitution: 53000, luck: 8200, health: 143500000, minWeaponDmg: 880, maxWeaponDmg: 1130, dungeonRuneBonuses: new DungeonEnemyRuneBonuses { DamageRuneType = RuneType.LightningDamage, FireResistance = 0, ColdResistance = 0, LightningResistance = 25, DamageBonus = 25 }, armor: 17750 ),
                    new DungeonEnemy(position: 7, @class: ClassType.Scout, level: 298, strength: 8380, dexterity: 16760, intelligence: 8380, constitution: 54500, luck: 8380, health: 148000000, minWeaponDmg: 894, maxWeaponDmg: 1142, dungeonRuneBonuses: new DungeonEnemyRuneBonuses { DamageRuneType = RuneType.LightningDamage, FireResistance = 0, ColdResistance = 0, LightningResistance = 25, DamageBonus = 25 }, armor: 18000 ),
                    new DungeonEnemy(position: 8, @class: ClassType.Mage, level: 301, strength: 8560, dexterity: 8560, intelligence: 17120, constitution: 56000, luck: 8560, health: 152500000, minWeaponDmg: 908, maxWeaponDmg: 1152, dungeonRuneBonuses: new DungeonEnemyRuneBonuses { DamageRuneType = RuneType.FireDamage, FireResistance = 25, ColdResistance = 0, LightningResistance = 0, DamageBonus = 25 }, armor: 18250 ),
                    new DungeonEnemy(position: 9, @class: ClassType.Mage, level: 304, strength: 8740, dexterity: 8740, intelligence: 17480, constitution: 57500, luck: 8740, health: 157000000, minWeaponDmg: 918, maxWeaponDmg: 1162, dungeonRuneBonuses: new DungeonEnemyRuneBonuses { DamageRuneType = RuneType.FireDamage, FireResistance = 25, ColdResistance = 0, LightningResistance = 0, DamageBonus = 25 }, armor: 18500 ),
                    new DungeonEnemy(position: 10, @class: ClassType.Scout, level: 310, strength: 9100, dexterity: 18200, intelligence: 9100, constitution: 60500, luck: 9100, health: 166000000, minWeaponDmg: 941, maxWeaponDmg: 1189, dungeonRuneBonuses: new DungeonEnemyRuneBonuses { DamageRuneType = RuneType.ColdDamage, FireResistance = 0, ColdResistance = 25, LightningResistance = 0, DamageBonus = 25 }, armor: 18750 )
                }
            },
            new Dungeon
            {
                Name = DungeonNames.TimeHonoredSchoolOfMagic,
                Position = 17,
                IsDefeated = false,
                IsUnlocked = true,
                UnlockResolve = c => c.CharacterLevel >= 200,
                DungeonEnemies = new List<DungeonEnemy>
                {
                    new DungeonEnemy(position: 1, @class: ClassType.Scout, level: 250, strength: 1400, dexterity: 11000, intelligence: 1400, constitution: 35000, luck: 4500, health: 62500000, minWeaponDmg: 600, maxWeaponDmg: 649 ),
                    new DungeonEnemy(position: 2, @class: ClassType.Warrior, level: 257, strength: 9722, dexterity: 2404, intelligence: 2426, constitution: 43730, luck: 4764, health: 79681528, minWeaponDmg: 279, maxWeaponDmg: 787 ),
                    new DungeonEnemy(position: 3, @class: ClassType.Scout, level: 265, strength: 1550, dexterity: 11290, intelligence: 1560, constitution: 44000, luck: 4800, health: 68425000, minWeaponDmg: 681, maxWeaponDmg: 727 ),
                    new DungeonEnemy(position: 4, @class: ClassType.Scout, level: 272, strength: 1965, dexterity: 12320, intelligence: 1980, constitution: 46000, luck: 5940, health: 73970000, minWeaponDmg: 466, maxWeaponDmg: 989 ),
                    new DungeonEnemy(position: 5, @class: ClassType.Mage, level: 280, strength: 3869, dexterity: 3865, intelligence: 12249, constitution: 49255, luck: 7662, health: 39995056, minWeaponDmg: 764, maxWeaponDmg: 1927 ),
                    new DungeonEnemy(position: 6, @class: ClassType.Scout, level: 287, strength: 4465, dexterity: 12440, intelligence: 4430, constitution: 54100, luck: 7960, health: 94220000, minWeaponDmg: 702, maxWeaponDmg: 816 ),
                    new DungeonEnemy(position: 7, @class: ClassType.Scout, level: 295, strength: 3910, dexterity: 12980, intelligence: 3850, constitution: 54500, luck: 7970, health: 96325000, minWeaponDmg: 402, maxWeaponDmg: 1167 ),
                    new DungeonEnemy(position: 8, @class: ClassType.Warrior, level: 302, strength: 14874, dexterity: 3132, intelligence: 1986, constitution: 58295, luck: 8022, health: 138640080, minWeaponDmg: 564, maxWeaponDmg: 675 ),
                    new DungeonEnemy(position: 9, @class: ClassType.Warrior, level: 310, strength: 14470, dexterity: 5540, intelligence: 5569, constitution: 70050, luck: 10197, health: 174284400, minWeaponDmg: 446, maxWeaponDmg: 945 ),
                    new DungeonEnemy(position: 10, @class: ClassType.Mage, level: 317, strength: 5911, dexterity: 4338, intelligence: 12174, constitution: 70050, luck: 8540, health: 71282872, minWeaponDmg: 1027, maxWeaponDmg: 2059 )
                }
            },
            new Dungeon
            {
                Name = DungeonNames.Hemorridor,
                Position = 18,
                IsDefeated = false,
                IsUnlocked = true,
                UnlockResolve = c => c.Dungeons.First(d => d.Position == 11).DungeonEnemies.First(e => e.Position == 5).IsDefeated,
                DungeonEnemies = new List<DungeonEnemy>
            {
                new DungeonEnemy(position: 1, @class: ClassType.Warrior, level: 200, strength: 8800, dexterity: 1120, intelligence: 1120, constitution: 28000, luck: 3600, health: 28140000, minWeaponDmg: 684, maxWeaponDmg: 1154 ),
                    new DungeonEnemy(position: 2, @class: ClassType.Warrior, level: 213, strength: 8069, dexterity: 1995, intelligence: 2014, constitution: 36296, luck: 3954, health: 38836720, minWeaponDmg: 728, maxWeaponDmg: 1229 ),
                    new DungeonEnemy(position: 3, @class: ClassType.Mage, level: 228, strength: 1333, dexterity: 1333, intelligence: 9709, constitution: 37840, luck: 4128, health: 17330720, minWeaponDmg: 1752, maxWeaponDmg: 2959 ),
                    new DungeonEnemy(position: 4, @class: ClassType.Scout, level: 242, strength: 1749, dexterity: 10965, intelligence: 1762, constitution: 40940, luck: 5287, health: 39793680, minWeaponDmg: 1034, maxWeaponDmg: 1747 ),
                    new DungeonEnemy(position: 5, @class: ClassType.Scout, level: 258, strength: 3559, dexterity: 11269, intelligence: 3559, constitution: 45315, luck: 7049, health: 46946340, minWeaponDmg: 1102, maxWeaponDmg: 1861 ),
                    new DungeonEnemy(position: 6, @class: ClassType.Warrior, level: 273, strength: 11818, dexterity: 4209, intelligence: 4209, constitution: 51395, luck: 7562, health: 70411152, minWeaponDmg: 932, maxWeaponDmg: 1574 ),
                    new DungeonEnemy(position: 7, @class: ClassType.Scout, level: 289, strength: 3832, dexterity: 12720, intelligence: 3773, constitution: 53410, luck: 7811, health: 61955600, minWeaponDmg: 1232, maxWeaponDmg: 2084 ),
                    new DungeonEnemy(position: 8, @class: ClassType.Mage, level: 305, strength: 3163, dexterity: 3163, intelligence: 15023, constitution: 58878, luck: 8102, health: 36033336, minWeaponDmg: 2340, maxWeaponDmg: 3959 ),
                    new DungeonEnemy(position: 9, @class: ClassType.BattleMage, level: 319, strength: 14904, dexterity: 5706, intelligence: 5736, constitution: 72152, luck: 10503, health: 115443200, minWeaponDmg: 1088, maxWeaponDmg: 1839 ),
                    new DungeonEnemy(position: 10, @class: ClassType.Scout, level: 333, strength: 6207, dexterity: 12783, intelligence: 6207, constitution: 73553, luck: 8967, health: 98266808, minWeaponDmg: 1420, maxWeaponDmg: 2399 )
                }
            },
            new Dungeon
            {
                Name = DungeonNames.Easteros,
                Position = 19,
                IsDefeated = false,
                IsUnlocked = true,
                UnlockResolve = c => c.Dungeons.First(d => d.Position == 11).IsUnlocked,
                DungeonEnemies = new List<DungeonEnemy>
                {
                    new DungeonEnemy(position: 1, @class: ClassType.Warrior, level: 310, strength: 6040, dexterity: 2120, intelligence: 2095, constitution: 5760, luck: 1410, health: 8956800, minWeaponDmg: 325, maxWeaponDmg: 504 ),
                    new DungeonEnemy(position: 2, @class: ClassType.Warrior, level: 320, strength: 6240, dexterity: 2190, intelligence: 2165, constitution: 5960, luck: 1450, health: 9565800, minWeaponDmg: 335, maxWeaponDmg: 534 ),
                    new DungeonEnemy(position: 3, @class: ClassType.Mage, level: 330, strength: 3200, dexterity: 3240, intelligence: 13040, constitution: 12160, luck: 4640, health: 8049920, minWeaponDmg: 719, maxWeaponDmg: 1325 ),
                    new DungeonEnemy(position: 4, @class: ClassType.Warrior, level: 340, strength: 13280, dexterity: 4660, intelligence: 4610, constitution: 12720, luck: 3060, health: 21687600, minWeaponDmg: 355, maxWeaponDmg: 594 ),
                    new DungeonEnemy(position: 5, @class: ClassType.Warrior, level: 350, strength: 13680, dexterity: 4800, intelligence: 4750, constitution: 13120, luck: 3140, health: 23025600, minWeaponDmg: 365, maxWeaponDmg: 624 ),
                    new DungeonEnemy(position: 6, @class: ClassType.Mage, level: 360, strength: 5910, dexterity: 5970, intelligence: 24000, constitution: 22680, luck: 8514, health: 16374960, minWeaponDmg: 867, maxWeaponDmg: 1842 ),
                    new DungeonEnemy(position: 7, @class: ClassType.Scout, level: 370, strength: 6540, dexterity: 26040, intelligence: 6510, constitution: 25200, luck: 9327, health: 37396800, minWeaponDmg: 670, maxWeaponDmg: 1034 ),
                    new DungeonEnemy(position: 8, @class: ClassType.Warrior, level: 380, strength: 22000, dexterity: 7200, intelligence: 7000, constitution: 25000, luck: 7020, health: 59493152, minWeaponDmg: 420, maxWeaponDmg: 699 ),
                    new DungeonEnemy(position: 9, @class: ClassType.Warrior, level: 390, strength: 35000, dexterity: 11800, intelligence: 12000, constitution: 40040, luck: 8000, health: 95873200, minWeaponDmg: 440, maxWeaponDmg: 819 ),
                    new DungeonEnemy(position: 10, @class: ClassType.Mage, level: 400, strength: 8500, dexterity: 8500, intelligence: 32000, constitution: 30000, luck: 12000, health: 52867840, minWeaponDmg: 900, maxWeaponDmg: 2049 )
                }
            },
            new Dungeon
            {
                Name = DungeonNames.DojoOfChildhoodHeros,
                Position = 20,
                IsDefeated = false,
                IsUnlocked = true,
                UnlockResolve = c => c.CharacterLevel >= 340,
                DungeonEnemies = new List<DungeonEnemy>
                {
                    new DungeonEnemy(position: 1, @class: ClassType.Warrior, level: 313, strength: 18560, dexterity: 9280, intelligence: 9280, constitution: 62000, luck: 9280, health: 170500000, minWeaponDmg: 953, maxWeaponDmg: 1199, dungeonRuneBonuses: new DungeonEnemyRuneBonuses { DamageRuneType = RuneType.LightningDamage, FireResistance = 0, ColdResistance = 0, LightningResistance = 25, DamageBonus = 25 }, armor: 19000 ),
                    new DungeonEnemy(position: 2, @class: ClassType.Warrior, level: 316, strength: 18920, dexterity: 9460, intelligence: 9460, constitution: 63500, luck: 9460, health: 175000000, minWeaponDmg: 964, maxWeaponDmg: 1211, dungeonRuneBonuses: new DungeonEnemyRuneBonuses { DamageRuneType = RuneType.FireDamage, FireResistance = 25, ColdResistance = 0, LightningResistance = 0, DamageBonus = 25 }, armor: 19250 ),
                    new DungeonEnemy(position: 3, @class: ClassType.Mage, level: 319, strength: 9640, dexterity: 9640, intelligence: 19280, constitution: 65000, luck: 9640, health: 179500000, minWeaponDmg: 977, maxWeaponDmg: 1225, dungeonRuneBonuses: new DungeonEnemyRuneBonuses { DamageRuneType = RuneType.ColdDamage, FireResistance = 0, ColdResistance = 25, LightningResistance = 0, DamageBonus = 25 }, armor: 19500 ),
                    new DungeonEnemy(position: 4, @class: ClassType.Warrior, level: 322, strength: 19640, dexterity: 9820, intelligence: 9820, constitution: 66500, luck: 9820, health: 184000000, minWeaponDmg: 989, maxWeaponDmg: 1237, dungeonRuneBonuses: new DungeonEnemyRuneBonuses { DamageRuneType = RuneType.ColdDamage, FireResistance = 0, ColdResistance = 25, LightningResistance = 0, DamageBonus = 25 }, armor: 19750 ),
                    new DungeonEnemy(position: 5, @class: ClassType.Mage, level: 325, strength: 10000, dexterity: 10000, intelligence: 20000, constitution: 68000, luck: 10000, health: 188500000, minWeaponDmg: 1000, maxWeaponDmg: 1247, dungeonRuneBonuses: new DungeonEnemyRuneBonuses { DamageRuneType = RuneType.LightningDamage, FireResistance = 0, ColdResistance = 0, LightningResistance = 25, DamageBonus = 25 }, armor: 20000 ),
                    new DungeonEnemy(position: 6, @class: ClassType.Scout, level: 328, strength: 10180, dexterity: 20360, intelligence: 10180, constitution: 69500, luck: 10180, health: 193000000, minWeaponDmg: 1016, maxWeaponDmg: 1254, dungeonRuneBonuses: new DungeonEnemyRuneBonuses { DamageRuneType = RuneType.LightningDamage, FireResistance = 0, ColdResistance = 0, LightningResistance = 25, DamageBonus = 25 }, armor: 20250 ),
                    new DungeonEnemy(position: 7, @class: ClassType.Mage, level: 331, strength: 10360, dexterity: 10360, intelligence: 20720, constitution: 71000, luck: 10360, health: 197500000, minWeaponDmg: 1024, maxWeaponDmg: 1274, dungeonRuneBonuses: new DungeonEnemyRuneBonuses { DamageRuneType = RuneType.FireDamage, FireResistance = 25, ColdResistance = 0, LightningResistance = 0, DamageBonus = 25 }, armor: 20500 ),
                    new DungeonEnemy(position: 8, @class: ClassType.Mage, level: 334, strength: 10540, dexterity: 10540, intelligence: 21080, constitution: 72500, luck: 10540, health: 202000000, minWeaponDmg: 1037, maxWeaponDmg: 1286, dungeonRuneBonuses: new DungeonEnemyRuneBonuses { DamageRuneType = RuneType.FireDamage, FireResistance = 25, ColdResistance = 0, LightningResistance = 0, DamageBonus = 25 }, armor: 20750 ),
                    new DungeonEnemy(position: 9, @class: ClassType.Warrior, level: 337, strength: 21440, dexterity: 10720, intelligence: 10720, constitution: 74000, luck: 10720, health: 206500000, minWeaponDmg: 1050, maxWeaponDmg: 1298, dungeonRuneBonuses: new DungeonEnemyRuneBonuses { DamageRuneType = RuneType.ColdDamage, FireResistance = 0, ColdResistance = 25, LightningResistance = 0, DamageBonus = 25 }, armor: 21000 ),
                    new DungeonEnemy(position: 10, @class: ClassType.Mage, level: 340, strength: 10900, dexterity: 10900, intelligence: 21800, constitution: 75500, luck: 10900, health: 211000000, minWeaponDmg: 1061, maxWeaponDmg: 1310, dungeonRuneBonuses: new DungeonEnemyRuneBonuses { DamageRuneType = RuneType.LightningDamage, FireResistance = 0, ColdResistance = 0, LightningResistance = 25, DamageBonus = 25 }, armor: 21250 )
                }
            },
            new Dungeon
            {
                Name = DungeonNames.TavernOfDarkDoppelgangers,
                Position = 21,
                IsDefeated = false,
                IsUnlocked = true,
                UnlockResolve = c => c.CharacterLevel >= 270,
                DungeonEnemies = new List<DungeonEnemy>
                {
                    new DungeonEnemy(position: 1, @class: ClassType.Mage, level: 410, strength: 7000, dexterity: 7000, intelligence: 20000, constitution: 18000, luck: 4000, health: 10000000, minWeaponDmg: 1416, maxWeaponDmg: 2290, armorMultiplier: 0.5 ),
                    new DungeonEnemy(position: 2, @class: ClassType.Warrior, level: 420, strength: 24000, dexterity: 8500, intelligence: 8500, constitution: 20000, luck: 5000, health: 30000000, minWeaponDmg: 651, maxWeaponDmg: 1049, armorMultiplier: 0.5 ),
                    new DungeonEnemy(position: 3, @class: ClassType.Warrior, level: 430, strength: 29000, dexterity: 10000, intelligence: 10000, constitution: 22000, luck: 6000, health: 25000000, minWeaponDmg: 650, maxWeaponDmg: 1096, armorMultiplier: 0.5 ),
                    new DungeonEnemy(position: 4, @class: ClassType.Scout, level: 440, strength: 12000, dexterity: 35000, intelligence: 12000, constitution: 24000, luck: 7500, health: 35000000, minWeaponDmg: 850, maxWeaponDmg: 1396, armorMultiplier: 0.5 ),
                    new DungeonEnemy(position: 5, @class: ClassType.Warrior, level: 450, strength: 42000, dexterity: 15000, intelligence: 15000, constitution: 26000, luck: 9000, health: 10000000, minWeaponDmg: 703, maxWeaponDmg: 1136, armorMultiplier: 0.5 ),
                    new DungeonEnemy(position: 6, @class: ClassType.Mage, level: 460, strength: 18000, dexterity: 18000, intelligence: 50000, constitution: 29000, luck: 11000, health: 25000000, minWeaponDmg: 1551, maxWeaponDmg: 2599, armorMultiplier: 0.5 ),
                    new DungeonEnemy(position: 7, @class: ClassType.Scout, level: 470, strength: 21000, dexterity: 60000, intelligence: 21000, constitution: 32000, luck: 13000, health: 35000000, minWeaponDmg: 902, maxWeaponDmg: 1449, armorMultiplier: 0.5 ),
                    new DungeonEnemy(position: 8, @class: ClassType.Mage, level: 480, strength: 25000, dexterity: 25000, intelligence: 72000, constitution: 35000, luck: 15000, health: 50000000, minWeaponDmg: 1605, maxWeaponDmg: 2698, armorMultiplier: 0.5 ),
                    new DungeonEnemy(position: 9, @class: ClassType.Warrior, level: 490, strength: 86000, dexterity: 30000, intelligence: 30000, constitution: 39000, luck: 19000, health: 30000000, minWeaponDmg: 750, maxWeaponDmg: 1244, armorMultiplier: 0.5 ),
                    new DungeonEnemy(position: 10, @class: ClassType.Mage, level: 500, strength: 35000, dexterity: 35000, intelligence: 90000, constitution: 43000, luck: 21000, health: 50000000, minWeaponDmg: 1701, maxWeaponDmg: 2798, armorMultiplier: 0.5 )
                }
            },
            new Dungeon
            {
                Name = DungeonNames.CityOfIntrigues,
                Position = 22,
                IsDefeated = false,
                IsUnlocked = true,
                UnlockResolve = c => c.Dungeons.First(d => d.Position == 19).IsDefeated,
                DungeonEnemies = new List<DungeonEnemy>
                {
                    new DungeonEnemy(position: 1, @class: ClassType.Scout, level: 410, strength: 8720, dexterity: 34720, intelligence: 8680, constitution: 33600, luck: 12436, health: 55238400, minWeaponDmg: 671, maxWeaponDmg: 1034 ),
                    new DungeonEnemy(position: 2, @class: ClassType.Mage, level: 420, strength: 11100, dexterity: 11200, intelligence: 45000, constitution: 42800, luck: 15940, health: 36037600, minWeaponDmg: 967, maxWeaponDmg: 2193 ),
                    new DungeonEnemy(position: 3, @class: ClassType.Warrior, level: 430, strength: 45800, dexterity: 16060, intelligence: 15935, constitution: 44400, luck: 10170, health: 95682000, minWeaponDmg: 481, maxWeaponDmg: 971 ),
                    new DungeonEnemy(position: 4, @class: ClassType.Scout, level: 440, strength: 11800, dexterity: 47000, intelligence: 11750, constitution: 45600, luck: 16805, health: 80438400, minWeaponDmg: 743, maxWeaponDmg: 1142 ),
                    new DungeonEnemy(position: 5, @class: ClassType.Warrior, level: 450, strength: 57840, dexterity: 20280, intelligence: 20130, constitution: 56160, luck: 12780, health: 126640800, minWeaponDmg: 505, maxWeaponDmg: 1044 ),
                    new DungeonEnemy(position: 6, @class: ClassType.Warrior, level: 460, strength: 59280, dexterity: 20784, intelligence: 20634, constitution: 57600, luck: 13068, health: 132768000, minWeaponDmg: 517, maxWeaponDmg: 1080 ),
                    new DungeonEnemy(position: 7, @class: ClassType.Mage, level: 470, strength: 15120, dexterity: 15240, intelligence: 61200, constitution: 58560, luck: 21648, health: 55163520, minWeaponDmg: 1087, maxWeaponDmg: 2611 ),
                    new DungeonEnemy(position: 8, @class: ClassType.Mage, level: 480, strength: 18060, dexterity: 18200, intelligence: 73080, constitution: 70000, luck: 25844, health: 67340000, minWeaponDmg: 1120, maxWeaponDmg: 2695 ),
                    new DungeonEnemy(position: 9, @class: ClassType.Warrior, level: 490, strength: 74200, dexterity: 26012, intelligence: 25837, constitution: 72240, luck: 16254, health: 177349200, minWeaponDmg: 553, maxWeaponDmg: 1188 ),
                    new DungeonEnemy(position: 10, @class: ClassType.Scout, level: 500, strength: 19040, dexterity: 75880, intelligence: 18970, constitution: 73920, luck: 27055, health: 148135680, minWeaponDmg: 886, maxWeaponDmg: 1358 )
                }
            },
            new Dungeon
            {
                Name = DungeonNames.SchoolOfMagicExpress,
                Position = 23,
                IsDefeated = false,
                IsUnlocked = true,
                UnlockResolve = c => c.Dungeons.First(d => d.Position == 17).IsDefeated,
                DungeonEnemies = new List<DungeonEnemy>
                {
                    new DungeonEnemy(position: 1, @class: ClassType.Mage, level: 325, strength: 3221, dexterity: 3221, intelligence: 16577, constitution: 61790, luck: 8964, health: 64459324, minWeaponDmg: 861, maxWeaponDmg: 2432 ),
                    new DungeonEnemy(position: 2, @class: ClassType.Mage, level: 332, strength: 6128, dexterity: 6057, intelligence: 17028, constitution: 76695, luck: 10905, health: 83003160, minWeaponDmg: 1291, maxWeaponDmg: 2100 ),
                    new DungeonEnemy(position: 3, @class: ClassType.Mage, level: 340, strength: 5151, dexterity: 5237, intelligence: 18553, constitution: 79880, luck: 12130, health: 90152560, minWeaponDmg: 992, maxWeaponDmg: 2507 ),
                    new DungeonEnemy(position: 4, @class: ClassType.Scout, level: 347, strength: 5875, dexterity: 20170, intelligence: 5870, constitution: 72300, luck: 9320, health: 168640000, minWeaponDmg: 540, maxWeaponDmg: 1439 ),
                    new DungeonEnemy(position: 5, @class: ClassType.Mage, level: 355, strength: 5650, dexterity: 5744, intelligence: 19876, constitution: 86520, luck: 12519, health: 104723808, minWeaponDmg: 1557, maxWeaponDmg: 2132 ),
                    new DungeonEnemy(position: 6, @class: ClassType.Warrior, level: 362, strength: 19163, dexterity: 9228, intelligence: 9077, constitution: 96410, luck: 13359, health: 306222272, minWeaponDmg: 490, maxWeaponDmg: 1181 ),
                    new DungeonEnemy(position: 7, @class: ClassType.Mage, level: 370, strength: 7773, dexterity: 7772, intelligence: 19358, constitution: 91315, luck: 13191, health: 116878632, minWeaponDmg: 1451, maxWeaponDmg: 2356 ),
                    new DungeonEnemy(position: 8, @class: ClassType.Mage, level: 377, strength: 5774, dexterity: 5775, intelligence: 24520, constitution: 96430, luck: 13751, health: 125754360, minWeaponDmg: 1592, maxWeaponDmg: 2385 ),
                    new DungeonEnemy(position: 9, @class: ClassType.Scout, level: 385, strength: 12400, dexterity: 29300, intelligence: 12600, constitution: 130350, luck: 18640, health: 379430016, minWeaponDmg: 848, maxWeaponDmg: 2091 ),
                    new DungeonEnemy(position: 10, @class: ClassType.Mage, level: 400, strength: 8447, dexterity: 8378, intelligence: 30585, constitution: 126470, luck: 18375, health: 190324704, minWeaponDmg: 1912, maxWeaponDmg: 2949 )
                }
            },
            new Dungeon
            {
                Name = DungeonNames.NordicGods,
                Position = 24,
                IsDefeated = false,
                IsUnlocked = true,
                UnlockResolve = c => c.CharacterLevel >= 180,
                DungeonEnemies = new List<DungeonEnemy>
                {
                    new DungeonEnemy(position: 1, @class: ClassType.Warrior, level: 210, strength: 8000, dexterity: 2000, intelligence: 2000, constitution: 36000, luck: 4000, health: 43560000, dungeonRuneBonuses: new DungeonEnemyRuneBonuses { DamageRuneType = RuneType.ColdDamage, FireResistance = 5, ColdResistance = 60, LightningResistance = 5, DamageBonus = 30 }, minWeaponDmg: 728, maxWeaponDmg: 1229),
                    new DungeonEnemy(position: 2, @class: ClassType.Warrior, level: 240, strength: 10965, dexterity: 1762, intelligence: 12000, constitution: 40500, luck: 5000, health: 55687500, dungeonRuneBonuses: new DungeonEnemyRuneBonuses { DamageRuneType = RuneType.LightningDamage, FireResistance = 10, ColdResistance = 10, LightningResistance = 60, DamageBonus = 30 }, minWeaponDmg: 840, maxWeaponDmg: 1439),
                    new DungeonEnemy(position: 3, @class: ClassType.Mage, level: 270, strength: 4000, dexterity: 4000, intelligence: 11500, constitution: 51000, luck: 7500, health: 31416000, dungeonRuneBonuses: new DungeonEnemyRuneBonuses { DamageRuneType = RuneType.FireDamage, FireResistance = 35, ColdResistance = 35, LightningResistance = 35, DamageBonus = 30 }, minWeaponDmg: 2126, maxWeaponDmg: 3643),
                    new DungeonEnemy(position: 4, @class: ClassType.Warrior, level: 305, strength: 15000, dexterity: 3500, intelligence: 3500, constitution: 58500, luck: 8000, health: 101351256, dungeonRuneBonuses: new DungeonEnemyRuneBonuses { DamageRuneType = RuneType.LightningDamage, FireResistance = 15, ColdResistance = 15, LightningResistance = 60, DamageBonus = 30 }, minWeaponDmg: 1067, maxWeaponDmg: 1829),
                    new DungeonEnemy(position: 5, @class: ClassType.Warrior, level: 330, strength: 12500, dexterity: 6000, intelligence: 6000, constitution: 73500, luck: 9000, health: 137445008, dungeonRuneBonuses: new DungeonEnemyRuneBonuses { DamageRuneType = RuneType.FireDamage, FireResistance = 60, ColdResistance = 30, LightningResistance = 30, DamageBonus = 40 }, minWeaponDmg: 1155, maxWeaponDmg: 1979),
                    new DungeonEnemy(position: 6, @class: ClassType.Assassin, level: 360, strength: 6500, dexterity: 18500, intelligence: 6500, constitution: 83500, luck: 11500, health: 135938000, dungeonRuneBonuses: new DungeonEnemyRuneBonuses { DamageRuneType = RuneType.ColdDamage, FireResistance = 40, ColdResistance = 40, LightningResistance = 40, DamageBonus = 45 }, minWeaponDmg: 2524, maxWeaponDmg: 4318),
                    new DungeonEnemy(position: 7, @class: ClassType.Berserker, level: 390, strength: 22500, dexterity: 6500, intelligence: 6500, constitution: 81500, luck: 10500, health: 143440000, dungeonRuneBonuses: new DungeonEnemyRuneBonuses { DamageRuneType = RuneType.ColdDamage, FireResistance = 10, ColdResistance = 50, LightningResistance = 10, DamageBonus = 45 }, minWeaponDmg: 1366, maxWeaponDmg: 3435),
                    new DungeonEnemy(position: 8, @class: ClassType.Scout, level: 420, strength: 10500, dexterity: 22500, intelligence: 10500, constitution: 112500, luck: 15500, health: 212850000, dungeonRuneBonuses: new DungeonEnemyRuneBonuses { DamageRuneType = RuneType.LightningDamage, FireResistance = 50, ColdResistance = 50, LightningResistance = 50, DamageBonus = 50 }, minWeaponDmg: 1837, maxWeaponDmg: 3148),
                    new DungeonEnemy(position: 9, @class: ClassType.Berserker, level: 455, strength: 29500, dexterity: 7000, intelligence: 7000, constitution: 115000, luck: 16000, health: 235290000, dungeonRuneBonuses: new DungeonEnemyRuneBonuses { DamageRuneType = RuneType.LightningDamage, FireResistance = 50, ColdResistance = 50, LightningResistance = 50, DamageBonus = 55 }, minWeaponDmg: 1595, maxWeaponDmg: 2728),
                    new DungeonEnemy(position: 10, @class: ClassType.Berserker, level: 500, strength: 38500, dexterity: 10500, intelligence: 10500, constitution: 158000, luck: 23000, health: 354552000, dungeonRuneBonuses: new DungeonEnemyRuneBonuses { DamageRuneType = RuneType.FireDamage, FireResistance = 60, ColdResistance = 50, LightningResistance = 50, DamageBonus = 60 }, minWeaponDmg: 1751, maxWeaponDmg: 2989)
                }
            },
            new Dungeon
            {
                Name = DungeonNames.AshMountain,
                Position = 25,
                IsDefeated = false,
                IsUnlocked = true,
                UnlockResolve = c => c.Dungeons.First(d => d.Position == 18).IsDefeated,
                DungeonEnemies = new List<DungeonEnemy>
                {
                    new DungeonEnemy(position: 1, @class: ClassType.BattleMage, level: 348, strength: 17737, dexterity: 3446, intelligence: 3446, constitution: 66115, luck: 9591, health: 115370672, minWeaponDmg: 1187, maxWeaponDmg: 2007, armor: 7000 ),
                    new DungeonEnemy(position: 2, @class: ClassType.Warrior, level: 362, strength: 18561, dexterity: 6602, intelligence: 6602, constitution: 83598, luck: 11886, health: 151730368, minWeaponDmg: 1234, maxWeaponDmg: 2087 ),
                    new DungeonEnemy(position: 3, @class: ClassType.Mage, level: 377, strength: 5718, dexterity: 5813, intelligence: 20594, constitution: 88667, luck: 13464, health: 67032252, minWeaponDmg: 2895, maxWeaponDmg: 4886 ),
                    new DungeonEnemy(position: 4, @class: ClassType.Assassin, level: 392, strength: 6639, dexterity: 22792, intelligence: 6633, constitution: 81699, luck: 10532, health: 128430832, minWeaponDmg: 2673, maxWeaponDmg: 4514 ),
                    new DungeonEnemy(position: 5, @class: ClassType.Mage, level: 408, strength: 6497, dexterity: 6606, intelligence: 22857, constitution: 99498, luck: 14397, health: 81389360, minWeaponDmg: 3130, maxWeaponDmg: 5286 ),
                    new DungeonEnemy(position: 6, @class: ClassType.Warrior, level: 424, strength: 22421, dexterity: 10797, intelligence: 10620, constitution: 112800, luck: 15630, health: 239700000, minWeaponDmg: 1446, maxWeaponDmg: 2441 ),
                    new DungeonEnemy(position: 7, @class: ClassType.Scout, level: 440, strength: 9250, dexterity: 23036, intelligence: 9250, constitution: 108665, luck: 15697, health: 191685056, minWeaponDmg: 1874, maxWeaponDmg: 3167 ),
                    new DungeonEnemy(position: 8, @class: ClassType.BattleMage, level: 456, strength: 29669, dexterity: 6988, intelligence: 6988, constitution: 116680, luck: 16639, health: 266613792, minWeaponDmg: 1554, maxWeaponDmg: 2627, armor: 7000 ),
                    new DungeonEnemy(position: 9, @class: ClassType.Warrior, level: 474, strength: 36039, dexterity: 15498, intelligence: 15498, constitution: 160331, luck: 22927, health: 380786112, minWeaponDmg: 1616, maxWeaponDmg: 2725 ),
                    new DungeonEnemy(position: 10, @class: ClassType.Warrior, level: 500, strength: 38231, dexterity: 10473, intelligence: 10474, constitution: 158088, luck: 22969, health: 396010432, minWeaponDmg: 1704, maxWeaponDmg: 2879 )
                }
            },
            new Dungeon
            {
                Name = DungeonNames.PlayaHQ,
                Position = 26,
                IsDefeated = false,
                IsUnlocked = true,
                UnlockResolve = c => c.Dungeons.First(d => d.Position == 15).IsDefeated,
                DungeonEnemies = new List<DungeonEnemy>
                {
                    new DungeonEnemy(position: 1, @class: ClassType.Scout, level: 410, strength: 7080, dexterity: 20200, intelligence: 7050, constitution: 18210, luck: 4280, health: 29937240, minWeaponDmg: 1299, maxWeaponDmg: 3185 ),
                    new DungeonEnemy(position: 2, @class: ClassType.Warrior, level: 420, strength: 24240, dexterity: 8490, intelligence: 8460, constitution: 20030, luck: 5130, health: 42163152, minWeaponDmg: 1247, maxWeaponDmg: 3060 ),
                    new DungeonEnemy(position: 3, @class: ClassType.Mage, level: 430, strength: 10150, dexterity: 10180, intelligence: 29080, constitution: 22030, luck: 6150, health: 18989860, minWeaponDmg: 3364, maxWeaponDmg: 8263 ),
                    new DungeonEnemy(position: 4, @class: ClassType.Warrior, level: 440, strength: 34890, dexterity: 12210, intelligence: 12180, constitution: 24230, luck: 7380, health: 53427152, minWeaponDmg: 1796, maxWeaponDmg: 4404 ),
                    new DungeonEnemy(position: 5, @class: ClassType.Mage, level: 450, strength: 14610, dexterity: 14650, intelligence: 41860, constitution: 26650, luck: 8850, health: 24038300, minWeaponDmg: 4848, maxWeaponDmg: 11901 ),
                    new DungeonEnemy(position: 6, @class: ClassType.Warrior, level: 460, strength: 50230, dexterity: 17580, intelligence: 17530, constitution: 29310, luck: 10620, health: 67559552, minWeaponDmg: 2584, maxWeaponDmg: 6343 ),
                    new DungeonEnemy(position: 7, @class: ClassType.Warrior, level: 470, strength: 60270, dexterity: 21090, intelligence: 21030, constitution: 32240, luck: 12740, health: 75925200, minWeaponDmg: 3101, maxWeaponDmg: 7612 ),
                    new DungeonEnemy(position: 8, @class: ClassType.Warrior, level: 480, strength: 72320, dexterity: 25300, intelligence: 25230, constitution: 35460, luck: 15280, health: 85281296, minWeaponDmg: 3722, maxWeaponDmg: 9140 ),
                    new DungeonEnemy(position: 9, @class: ClassType.Scout, level: 490, strength: 30360, dexterity: 86780, intelligence: 30270, constitution: 39000, luck: 18330, health: 76596000, minWeaponDmg: 5596, maxWeaponDmg: 13705 ),
                    new DungeonEnemy(position: 10, @class: ClassType.Mage, level: 500, strength: 36430, dexterity: 36320, intelligence: 104130, constitution: 42900, luck: 21990, health: 42985800, minWeaponDmg: 12063, maxWeaponDmg: 29600 )
                }
            },
            new Dungeon
            {
                Name = DungeonNames.MountOlympus,
                Position = 27,
                IsDefeated = false,
                IsUnlocked = true,
                UnlockResolve = c => c.CharacterLevel >= 200,
                DungeonEnemies = new List<DungeonEnemy>
            {
                    new DungeonEnemy(position: 1, @class: ClassType.Mage, level: 210, strength: 2000, dexterity: 2000, intelligence: 8000, constitution: 80000, luck: 4000, health: 52800000, dungeonRuneBonuses: new DungeonEnemyRuneBonuses { DamageRuneType = RuneType.ColdDamage, FireResistance = 5, ColdResistance = 60, LightningResistance = 5, DamageBonus = 30 }, minWeaponDmg: 630, maxWeaponDmg: 1265 ),
                    new DungeonEnemy(position: 2, @class: ClassType.Warrior, level: 240, strength: 12000, dexterity: 4000, intelligence: 4000, constitution: 100000, luck: 5000, health: 187500000, dungeonRuneBonuses: new DungeonEnemyRuneBonuses { DamageRuneType = RuneType.LightningDamage, FireResistance = 10, ColdResistance = 10, LightningResistance = 60, DamageBonus = 35 }, minWeaponDmg: 320, maxWeaponDmg: 639 ),
                    new DungeonEnemy(position: 3, @class: ClassType.Scout, level: 270, strength: 6000, dexterity: 16000, intelligence: 6000, constitution: 120000, luck: 6000, health: 201600000, dungeonRuneBonuses: new DungeonEnemyRuneBonuses { DamageRuneType = RuneType.FireDamage, FireResistance = 60, ColdResistance = 15, LightningResistance = 15, DamageBonus = 35 }, minWeaponDmg: 450, maxWeaponDmg: 899 ),
                    new DungeonEnemy(position: 4, @class: ClassType.Mage, level: 305, strength: 8000, dexterity: 8000, intelligence: 20000, constitution: 140000, luck: 7000, health: 132300000, dungeonRuneBonuses: new DungeonEnemyRuneBonuses { DamageRuneType = RuneType.ColdDamage, FireResistance = 20, ColdResistance = 60, LightningResistance = 20, DamageBonus = 40 }, minWeaponDmg: 920, maxWeaponDmg: 1829 ),
                    new DungeonEnemy(position: 5, @class: ClassType.Mage, level: 330, strength: 10000, dexterity: 10000, intelligence: 24000, constitution: 160000, luck: 8000, health: 163200000, dungeonRuneBonuses: new DungeonEnemyRuneBonuses { DamageRuneType = RuneType.FireDamage, FireResistance = 60, ColdResistance = 25, LightningResistance = 25, DamageBonus = 40 }, minWeaponDmg: 1000, maxWeaponDmg: 1989 ),
                    new DungeonEnemy(position: 6, @class: ClassType.Warrior, level: 360, strength: 28000, dexterity: 12000, intelligence: 12000, constitution: 180000, luck: 10000, health: 499500000, dungeonRuneBonuses: new DungeonEnemyRuneBonuses { DamageRuneType = RuneType.LightningDamage, FireResistance = 40, ColdResistance = 40, LightningResistance = 40, DamageBonus = 45 }, minWeaponDmg: 480, maxWeaponDmg: 959 ),
                    new DungeonEnemy(position: 7, @class: ClassType.Warrior, level: 390, strength: 32000, dexterity: 14000, intelligence: 14000, constitution: 200000, luck: 11000, health: 600000000, dungeonRuneBonuses: new DungeonEnemyRuneBonuses { DamageRuneType = RuneType.FireDamage, FireResistance = 60, ColdResistance = 60, LightningResistance = 60, DamageBonus = 45 }, minWeaponDmg: 520, maxWeaponDmg: 1049 ),
                    new DungeonEnemy(position: 8, @class: ClassType.Scout, level: 420, strength: 16000, dexterity: 36000, intelligence: 16000, constitution: 220000, luck: 12000, health: 567600000, dungeonRuneBonuses: new DungeonEnemyRuneBonuses { DamageRuneType = RuneType.ColdDamage, FireResistance = 60, ColdResistance = 60, LightningResistance = 60, DamageBonus = 50 }, minWeaponDmg: 700, maxWeaponDmg: 1399 ),
                    new DungeonEnemy(position: 9, @class: ClassType.Scout, level: 455, strength: 28000, dexterity: 40000, intelligence: 28000, constitution: 250000, luck: 13000, health: 697500032, dungeonRuneBonuses: new DungeonEnemyRuneBonuses { DamageRuneType = RuneType.FireDamage, FireResistance = 60, ColdResistance = 60, LightningResistance = 60, DamageBonus = 60 }, minWeaponDmg: 760, maxWeaponDmg: 1519 ),
                    new DungeonEnemy(position: 10, @class: ClassType.Mage, level: 500, strength: 33333, dexterity: 33333, intelligence: 44444, constitution: 300000, luck: 15000, health: 459000000, dungeonRuneBonuses: new DungeonEnemyRuneBonuses { DamageRuneType = RuneType.LightningDamage, FireResistance = 60, ColdResistance = 60, LightningResistance = 60, DamageBonus = 60 }, minWeaponDmg: 1500, maxWeaponDmg: 2996 )
                }
            },
            new Dungeon
            {
                Name = DungeonNames.MonsterGrotto,
                Position = 28,
                IsDefeated = false,
                IsUnlocked = true,
                UnlockResolve = c => c.CharacterLevel >= 480,
                DungeonEnemies = new List<DungeonEnemy>
                {
                    new DungeonEnemy(position: 1, @class: ClassType.Warrior, level: 480, strength: 66000, dexterity: 35000, intelligence: 35000, constitution: 174000, luck: 35000, health: 540000000, minWeaponDmg: 1901, maxWeaponDmg: 2144, dungeonRuneBonuses: new DungeonEnemyRuneBonuses { DamageRuneType = RuneType.LightningDamage, FireResistance = 0, ColdResistance = 0, LightningResistance = 25, DamageBonus = 25 } ),
                    new DungeonEnemy(position: 2, @class: ClassType.Mage, level: 483, strength: 35750, dexterity: 35750, intelligence: 67350, constitution: 176400, luck: 35750, health: 549000000, minWeaponDmg: 1918, maxWeaponDmg: 2162, dungeonRuneBonuses: new DungeonEnemyRuneBonuses { DamageRuneType = RuneType.ColdDamage, FireResistance = 0, ColdResistance = 25, LightningResistance = 0, DamageBonus = 25 } ),
                    new DungeonEnemy(position: 3, @class: ClassType.Scout, level: 486, strength: 36500, dexterity: 68700, intelligence: 36500, constitution: 178800, luck: 36500, health: 558000000, minWeaponDmg: 1931, maxWeaponDmg: 2174, dungeonRuneBonuses: new DungeonEnemyRuneBonuses { DamageRuneType = RuneType.LightningDamage, FireResistance = 0, ColdResistance = 0, LightningResistance = 25, DamageBonus = 25 } ),
                    new DungeonEnemy(position: 4, @class: ClassType.Warrior, level: 489, strength: 70050, dexterity: 37250, intelligence: 37250, constitution: 181200, luck: 37250, health: 567000000, minWeaponDmg: 1948, maxWeaponDmg: 2189, dungeonRuneBonuses: new DungeonEnemyRuneBonuses { DamageRuneType = RuneType.FireDamage, FireResistance = 25, ColdResistance = 0, LightningResistance = 0, DamageBonus = 25 } ),
                    new DungeonEnemy(position: 5, @class: ClassType.Mage, level: 492, strength: 38000, dexterity: 38000, intelligence: 71400, constitution: 183600, luck: 38000, health: 576000000, minWeaponDmg: 1962, maxWeaponDmg: 2202, dungeonRuneBonuses: new DungeonEnemyRuneBonuses { DamageRuneType = RuneType.ColdDamage, FireResistance = 0, ColdResistance = 25, LightningResistance = 0, DamageBonus = 25 } ),
                    new DungeonEnemy(position: 6, @class: ClassType.Warrior, level: 495, strength: 72750, dexterity: 38750, intelligence: 38750, constitution: 186000, luck: 38750, health: 585000000, minWeaponDmg: 1980, maxWeaponDmg: 2216, dungeonRuneBonuses: new DungeonEnemyRuneBonuses { DamageRuneType = RuneType.FireDamage, FireResistance = 25, ColdResistance = 0, LightningResistance = 0, DamageBonus = 25 } ),
                    new DungeonEnemy(position: 7, @class: ClassType.Scout, level: 498, strength: 39500, dexterity: 74100, intelligence: 39500, constitution: 188400, luck: 39500, health: 594000000, minWeaponDmg: 1996, maxWeaponDmg: 2235, dungeonRuneBonuses: new DungeonEnemyRuneBonuses { DamageRuneType = RuneType.LightningDamage, FireResistance = 0, ColdResistance = 0, LightningResistance = 25, DamageBonus = 25 } ),
                    new DungeonEnemy(position: 8, @class: ClassType.Scout, level: 501, strength: 40250, dexterity: 80500, intelligence: 40250, constitution: 190800, luck: 40250, health: 603000000, minWeaponDmg: 2017, maxWeaponDmg: 2254, dungeonRuneBonuses: new DungeonEnemyRuneBonuses { DamageRuneType = RuneType.FireDamage, FireResistance = 25, ColdResistance = 0, LightningResistance = 0, DamageBonus = 25 } ),
                    new DungeonEnemy(position: 9, @class: ClassType.Warrior, level: 504, strength: 82000, dexterity: 41000, intelligence: 41000, constitution: 193200, luck: 41000, health: 612000000, minWeaponDmg: 2020, maxWeaponDmg: 2267, dungeonRuneBonuses: new DungeonEnemyRuneBonuses { DamageRuneType = RuneType.ColdDamage, FireResistance = 0, ColdResistance = 25, LightningResistance = 0, DamageBonus = 25 } ),
                    new DungeonEnemy(position: 10, @class: ClassType.Warrior, level: 510, strength: 85000, dexterity: 42500, intelligence: 42500, constitution: 198000, luck: 42500, health: 630000000, minWeaponDmg: 2064, maxWeaponDmg: 2296, dungeonRuneBonuses: new DungeonEnemyRuneBonuses { DamageRuneType = RuneType.FireDamage, FireResistance = 25, ColdResistance = 0, LightningResistance = 0, DamageBonus = 25 } )
                }
            },
            #endregion

            #region Tower
            
            new Dungeon
            {
                Name = DungeonNames.Tower,
                Position = 98,
                IsDefeated = false,
                IsUnlocked = true,
                Type = DungeonTypeEnum.Tower,
                UnlockResolve = c => c.Dungeons.Where(d => d.Position <= 9).All(d => d.IsDefeated),
                DungeonEnemies = new List<DungeonEnemy>
                {
                    new DungeonEnemy(position: 1, @class: ClassType.Bert, level: 200, strength: 4194, dexterity: 1697, intelligence: 1665, constitution: 15940, luck: 2589, health: 16019700, minWeaponDmg: 268, maxWeaponDmg: 534, armorMultiplier: 1.5),
                    new DungeonEnemy(position: 2, @class: ClassType.Mage, level: 202, strength: 1714, dexterity: 1678, intelligence: 4242, constitution: 16140, luck: 2622, health: 6552840, minWeaponDmg: 610, maxWeaponDmg: 1217, armorMultiplier: 1.5),
                    new DungeonEnemy(position: 3, @class: ClassType.Scout, level: 204, strength: 1730, dexterity: 4292, intelligence: 1695, constitution: 16328, luck: 2654, health: 13388960, minWeaponDmg: 342, maxWeaponDmg: 681, armorMultiplier: 1.5),
                    new DungeonEnemy(position: 4, @class: ClassType.Bert, level: 206, strength: 4340, dexterity: 1746, intelligence: 1715, constitution: 16512, luck: 2690, health: 17089920, minWeaponDmg: 276, maxWeaponDmg: 551, armorMultiplier: 1.5),
                    new DungeonEnemy(position: 5, @class: ClassType.Bert, level: 208, strength: 4385, dexterity: 1763, intelligence: 1733, constitution: 16712, luck: 2726, health: 17464040, minWeaponDmg: 279, maxWeaponDmg: 556, armorMultiplier: 1.5),
                    new DungeonEnemy(position: 6, @class: ClassType.Mage, level: 210, strength: 1782, dexterity: 1747, intelligence: 4434, constitution: 16896, luck: 2757, health: 7130112, minWeaponDmg: 634, maxWeaponDmg: 1265, armorMultiplier: 1.5),
                    new DungeonEnemy(position: 7, @class: ClassType.Bert, level: 212, strength: 4482, dexterity: 1794, intelligence: 1766, constitution: 17100, luck: 2790, health: 18211500, minWeaponDmg: 284, maxWeaponDmg: 568, armorMultiplier: 1.5),
                    new DungeonEnemy(position: 8, @class: ClassType.Mage, level: 214, strength: 1812, dexterity: 1786, intelligence: 4529, constitution: 17284, luck: 2822, health: 7432120, minWeaponDmg: 646, maxWeaponDmg: 1289, armorMultiplier: 1.5),
                    new DungeonEnemy(position: 9, @class: ClassType.Mage, level: 216, strength: 1828, dexterity: 1800, intelligence: 4578, constitution: 17484, luck: 2858, health: 7588056, minWeaponDmg: 651, maxWeaponDmg: 1300, armorMultiplier: 1.5),
                    new DungeonEnemy(position: 10, @class: ClassType.Bert, level: 218, strength: 4627, dexterity: 1846, intelligence: 1818, constitution: 17680, luck: 2890, health: 19359600, minWeaponDmg: 292, maxWeaponDmg: 583, armorMultiplier: 1.5),
                    new DungeonEnemy(position: 11, @class: ClassType.Mage, level: 220, strength: 1861, dexterity: 1835, intelligence: 4674, constitution: 17860, luck: 2926, health: 7894120, minWeaponDmg: 663, maxWeaponDmg: 1324, armorMultiplier: 1.5),
                    new DungeonEnemy(position: 12, @class: ClassType.Mage, level: 222, strength: 1878, dexterity: 1854, intelligence: 4722, constitution: 18064, luck: 2957, health: 8056544, minWeaponDmg: 670, maxWeaponDmg: 1337, armorMultiplier: 1.5),
                    new DungeonEnemy(position: 13, @class: ClassType.Bert, level: 224, strength: 4771, dexterity: 1898, intelligence: 1869, constitution: 18240, luck: 2991, health: 20520000, minWeaponDmg: 299, maxWeaponDmg: 600, armorMultiplier: 1.5),
                    new DungeonEnemy(position: 14, @class: ClassType.Bert, level: 226, strength: 4820, dexterity: 1909, intelligence: 1887, constitution: 18440, luck: 3027, health: 20929400, minWeaponDmg: 303, maxWeaponDmg: 604, armorMultiplier: 1.5),
                    new DungeonEnemy(position: 15, @class: ClassType.Bert, level: 228, strength: 4870, dexterity: 1928, intelligence: 1907, constitution: 18620, luck: 3060, health: 21319900, minWeaponDmg: 306, maxWeaponDmg: 609, armorMultiplier: 1.5),
                    new DungeonEnemy(position: 16, @class: ClassType.Mage, level: 230, strength: 1943, dexterity: 1921, intelligence: 4916, constitution: 18824, luck: 3094, health: 8696688, minWeaponDmg: 694, maxWeaponDmg: 1385, armorMultiplier: 1.5),
                    new DungeonEnemy(position: 17, @class: ClassType.Bert, level: 232, strength: 4964, dexterity: 1962, intelligence: 1940, constitution: 19020, luck: 3126, health: 22158300, minWeaponDmg: 311, maxWeaponDmg: 620, armorMultiplier: 1.5),
                    new DungeonEnemy(position: 18, @class: ClassType.Scout, level: 234, strength: 1977, dexterity: 5012, intelligence: 1956, constitution: 19204, luck: 3160, health: 18051760, minWeaponDmg: 392, maxWeaponDmg: 783, armorMultiplier: 1.5),
                    new DungeonEnemy(position: 19, @class: ClassType.Bert, level: 236, strength: 5059, dexterity: 1993, intelligence: 1975, constitution: 19392, luck: 3198, health: 22979520, minWeaponDmg: 316, maxWeaponDmg: 631, armorMultiplier: 1.5),
                    new DungeonEnemy(position: 20, @class: ClassType.Scout, level: 238, strength: 2009, dexterity: 5109, intelligence: 1990, constitution: 19584, luck: 3230, health: 18722304, minWeaponDmg: 399, maxWeaponDmg: 796, armorMultiplier: 1.5),
                    new DungeonEnemy(position: 21, @class: ClassType.Bert, level: 240, strength: 5157, dexterity: 2024, intelligence: 2009, constitution: 19780, luck: 3262, health: 23834900, minWeaponDmg: 322, maxWeaponDmg: 641, armorMultiplier: 1.5),
                    new DungeonEnemy(position: 22, @class: ClassType.Bert, level: 242, strength: 5206, dexterity: 2043, intelligence: 2028, constitution: 19960, luck: 3295, health: 24251400, minWeaponDmg: 324, maxWeaponDmg: 647, armorMultiplier: 1.5),
                    new DungeonEnemy(position: 23, @class: ClassType.Bert, level: 244, strength: 5252, dexterity: 2058, intelligence: 2042, constitution: 20160, luck: 3330, health: 24696000, minWeaponDmg: 327, maxWeaponDmg: 652, armorMultiplier: 1.5),
                    new DungeonEnemy(position: 24, @class: ClassType.Bert, level: 246, strength: 5302, dexterity: 2077, intelligence: 2061, constitution: 20360, luck: 3362, health: 25144600, minWeaponDmg: 330, maxWeaponDmg: 657, armorMultiplier: 1.5),
                    new DungeonEnemy(position: 25, @class: ClassType.Mage, level: 248, strength: 2090, dexterity: 2078, intelligence: 5348, constitution: 20544, luck: 3398, health: 10230912, minWeaponDmg: 747, maxWeaponDmg: 1492, armorMultiplier: 1.5),
                    new DungeonEnemy(position: 26, @class: ClassType.Scout, level: 250, strength: 2109, dexterity: 5398, intelligence: 2098, constitution: 20728, luck: 3430, health: 20810912, minWeaponDmg: 419, maxWeaponDmg: 836, armorMultiplier: 1.5),
                    new DungeonEnemy(position: 27, @class: ClassType.Scout, level: 252, strength: 2139, dexterity: 5448, intelligence: 2128, constitution: 20944, luck: 3476, health: 21195328, minWeaponDmg: 422, maxWeaponDmg: 841, armorMultiplier: 1.5),
                    new DungeonEnemy(position: 28, @class: ClassType.Bert, level: 254, strength: 5498, dexterity: 2173, intelligence: 2162, constitution: 21160, luck: 3522, health: 26979000, minWeaponDmg: 340, maxWeaponDmg: 679, armorMultiplier: 1.5),
                    new DungeonEnemy(position: 29, @class: ClassType.Bert, level: 256, strength: 5549, dexterity: 2207, intelligence: 2198, constitution: 21356, luck: 3567, health: 27442460, minWeaponDmg: 343, maxWeaponDmg: 684, armorMultiplier: 1.5),
                    new DungeonEnemy(position: 30, @class: ClassType.Bert, level: 258, strength: 5596, dexterity: 2241, intelligence: 2228, constitution: 21572, luck: 3613, health: 27935740, minWeaponDmg: 346, maxWeaponDmg: 690, armorMultiplier: 1.5),
                    new DungeonEnemy(position: 31, @class: ClassType.Bert, level: 260, strength: 5646, dexterity: 2275, intelligence: 2263, constitution: 21792, luck: 3657, health: 28438560, minWeaponDmg: 348, maxWeaponDmg: 695, armorMultiplier: 1.5),
                    new DungeonEnemy(position: 32, @class: ClassType.Mage, level: 262, strength: 2306, dexterity: 2296, intelligence: 5694, constitution: 21992, luck: 3705, health: 11567792, minWeaponDmg: 790, maxWeaponDmg: 1577, armorMultiplier: 1.5),
                    new DungeonEnemy(position: 33, @class: ClassType.Bert, level: 264, strength: 5744, dexterity: 2340, intelligence: 2331, constitution: 22204, luck: 3751, health: 29420300, minWeaponDmg: 354, maxWeaponDmg: 705, armorMultiplier: 1.5),
                    new DungeonEnemy(position: 34, @class: ClassType.Bert, level: 266, strength: 5796, dexterity: 2377, intelligence: 2362, constitution: 22412, luck: 3794, health: 29920020, minWeaponDmg: 356, maxWeaponDmg: 711, armorMultiplier: 1.5),
                    new DungeonEnemy(position: 35, @class: ClassType.Bert, level: 268, strength: 5846, dexterity: 2405, intelligence: 2396, constitution: 22628, luck: 3840, health: 30434660, minWeaponDmg: 359, maxWeaponDmg: 716, armorMultiplier: 1.5),
                    new DungeonEnemy(position: 36, @class: ClassType.Scout, level: 270, strength: 2442, dexterity: 5894, intelligence: 2430, constitution: 22828, luck: 3885, health: 24745552, minWeaponDmg: 452, maxWeaponDmg: 903, armorMultiplier: 1.5),
                    new DungeonEnemy(position: 37, @class: ClassType.Scout, level: 272, strength: 2472, dexterity: 5945, intelligence: 2465, constitution: 23044, luck: 3934, health: 25164048, minWeaponDmg: 455, maxWeaponDmg: 908, armorMultiplier: 1.5),
                    new DungeonEnemy(position: 38, @class: ClassType.Bert, level: 274, strength: 5995, dexterity: 2507, intelligence: 2498, constitution: 23264, luck: 3975, health: 31988000, minWeaponDmg: 367, maxWeaponDmg: 732, armorMultiplier: 1.5),
                    new DungeonEnemy(position: 39, @class: ClassType.Bert, level: 276, strength: 6046, dexterity: 2538, intelligence: 2531, constitution: 23452, luck: 4022, health: 32481020, minWeaponDmg: 370, maxWeaponDmg: 737, armorMultiplier: 1.5),
                    new DungeonEnemy(position: 40, @class: ClassType.Bert, level: 278, strength: 6092, dexterity: 2572, intelligence: 2566, constitution: 23668, luck: 4069, health: 33016860, minWeaponDmg: 372, maxWeaponDmg: 743, armorMultiplier: 1.5),
                    new DungeonEnemy(position: 41, @class: ClassType.Mage, level: 280, strength: 2609, dexterity: 2597, intelligence: 6144, constitution: 23872, luck: 4113, health: 13416064, minWeaponDmg: 843, maxWeaponDmg: 1684, armorMultiplier: 1.5),
                    new DungeonEnemy(position: 42, @class: ClassType.Mage, level: 282, strength: 2638, dexterity: 2632, intelligence: 6195, constitution: 24088, luck: 4161, health: 13633808, minWeaponDmg: 850, maxWeaponDmg: 1697, armorMultiplier: 1.5),
                    new DungeonEnemy(position: 43, @class: ClassType.Bert, level: 284, strength: 6245, dexterity: 2671, intelligence: 2668, constitution: 24292, luck: 4203, health: 34616100, minWeaponDmg: 380, maxWeaponDmg: 759, armorMultiplier: 1.5),
                    new DungeonEnemy(position: 44, @class: ClassType.Scout, level: 286, strength: 2704, dexterity: 6293, intelligence: 2700, constitution: 24508, luck: 4252, health: 28135184, minWeaponDmg: 479, maxWeaponDmg: 956, armorMultiplier: 1.5),
                    new DungeonEnemy(position: 45, @class: ClassType.Scout, level: 288, strength: 2738, dexterity: 6346, intelligence: 2731, constitution: 24716, luck: 4294, health: 28571696, minWeaponDmg: 482, maxWeaponDmg: 961, armorMultiplier: 1.5),
                    new DungeonEnemy(position: 46, @class: ClassType.Bert, level: 290, strength: 6396, dexterity: 2770, intelligence: 2765, constitution: 24924, luck: 4340, health: 36264420, minWeaponDmg: 388, maxWeaponDmg: 775, armorMultiplier: 1.5),
                    new DungeonEnemy(position: 47, @class: ClassType.Scout, level: 292, strength: 2806, dexterity: 6442, intelligence: 2802, constitution: 25132, luck: 4386, health: 29454704, minWeaponDmg: 488, maxWeaponDmg: 975, armorMultiplier: 1.5),
                    new DungeonEnemy(position: 48, @class: ClassType.Mage, level: 294, strength: 2841, dexterity: 2832, intelligence: 6492, constitution: 25344, luck: 4431, health: 14952960, minWeaponDmg: 886, maxWeaponDmg: 1769, armorMultiplier: 1.5),
                    new DungeonEnemy(position: 49, @class: ClassType.Mage, level: 296, strength: 2870, dexterity: 2867, intelligence: 6543, constitution: 25556, luck: 4479, health: 15180264, minWeaponDmg: 891, maxWeaponDmg: 1780, armorMultiplier: 1.5),
                    new DungeonEnemy(position: 50, @class: ClassType.Bert, level: 298, strength: 6593, dexterity: 2905, intelligence: 2902, constitution: 25764, luck: 4523, health: 38517180, minWeaponDmg: 399, maxWeaponDmg: 796, armorMultiplier: 1.5),
                    new DungeonEnemy(position: 51, @class: ClassType.Bert, level: 300, strength: 6640, dexterity: 2937, intelligence: 2932, constitution: 25976, luck: 4569, health: 39093880, minWeaponDmg: 402, maxWeaponDmg: 801, armorMultiplier: 1.5),
                    new DungeonEnemy(position: 52, @class: ClassType.Scout, level: 302, strength: 2974, dexterity: 6711, intelligence: 2971, constitution: 26224, luck: 4610, health: 31783488, minWeaponDmg: 506, maxWeaponDmg: 1009, armorMultiplier: 1.5),
                    new DungeonEnemy(position: 53, @class: ClassType.Bert, level: 304, strength: 6776, dexterity: 3010, intelligence: 3013, constitution: 26464, luck: 4654, health: 40357600, minWeaponDmg: 407, maxWeaponDmg: 812, armorMultiplier: 1.5),
                    new DungeonEnemy(position: 54, @class: ClassType.Mage, level: 306, strength: 3048, dexterity: 3053, intelligence: 6840, constitution: 26728, luck: 4697, health: 16410992, minWeaponDmg: 922, maxWeaponDmg: 1841, armorMultiplier: 1.5),
                    new DungeonEnemy(position: 55, @class: ClassType.Bert, level: 308, strength: 6906, dexterity: 3089, intelligence: 3091, constitution: 26964, luck: 4741, health: 41659380, minWeaponDmg: 412, maxWeaponDmg: 823, armorMultiplier: 1.5),
                    new DungeonEnemy(position: 56, @class: ClassType.Bert, level: 310, strength: 6973, dexterity: 3121, intelligence: 3132, constitution: 27220, luck: 4784, health: 42327100, minWeaponDmg: 415, maxWeaponDmg: 828, armorMultiplier: 1.5),
                    new DungeonEnemy(position: 57, @class: ClassType.Bert, level: 312, strength: 7040, dexterity: 3160, intelligence: 3173, constitution: 27456, luck: 4828, health: 42968640, minWeaponDmg: 418, maxWeaponDmg: 833, armorMultiplier: 1.5),
                    new DungeonEnemy(position: 58, @class: ClassType.Scout, level: 314, strength: 3196, dexterity: 7105, intelligence: 3212, constitution: 27708, luck: 4875, health: 34912080, minWeaponDmg: 526, maxWeaponDmg: 1049, armorMultiplier: 1.5),
                    new DungeonEnemy(position: 59, @class: ClassType.Mage, level: 316, strength: 3236, dexterity: 3250, intelligence: 7173, constitution: 27948, luck: 4914, health: 17719032, minWeaponDmg: 951, maxWeaponDmg: 1900, armorMultiplier: 1.5),
                    new DungeonEnemy(position: 60, @class: ClassType.Bert, level: 318, strength: 7240, dexterity: 3270, intelligence: 3291, constitution: 28196, luck: 4958, health: 44972620, minWeaponDmg: 426, maxWeaponDmg: 849, armorMultiplier: 1.5),
                    new DungeonEnemy(position: 61, @class: ClassType.Bert, level: 320, strength: 7303, dexterity: 3309, intelligence: 3331, constitution: 28448, luck: 5005, health: 45659040, minWeaponDmg: 428, maxWeaponDmg: 855, armorMultiplier: 1.5),
                    new DungeonEnemy(position: 62, @class: ClassType.Bert, level: 322, strength: 7370, dexterity: 3348, intelligence: 3368, constitution: 28692, luck: 5043, health: 46337580, minWeaponDmg: 431, maxWeaponDmg: 860, armorMultiplier: 1.5),
                    new DungeonEnemy(position: 63, @class: ClassType.Bert, level: 324, strength: 7436, dexterity: 3382, intelligence: 3409, constitution: 28944, luck: 5088, health: 47034000, minWeaponDmg: 434, maxWeaponDmg: 865, armorMultiplier: 1.5),
                    new DungeonEnemy(position: 64, @class: ClassType.Scout, level: 326, strength: 3422, dexterity: 7501, intelligence: 3448, constitution: 29184, luck: 5132, health: 38172672, minWeaponDmg: 546, maxWeaponDmg: 1089, armorMultiplier: 1.5),
                    new DungeonEnemy(position: 65, @class: ClassType.Bert, level: 328, strength: 7567, dexterity: 3458, intelligence: 3487, constitution: 29436, luck: 5177, health: 48422220, minWeaponDmg: 439, maxWeaponDmg: 876, armorMultiplier: 1.5),
                    new DungeonEnemy(position: 66, @class: ClassType.Bert, level: 330, strength: 7634, dexterity: 3495, intelligence: 3528, constitution: 29696, luck: 5217, health: 49146880, minWeaponDmg: 442, maxWeaponDmg: 881, armorMultiplier: 1.5),
                    new DungeonEnemy(position: 67, @class: ClassType.Scout, level: 332, strength: 3532, dexterity: 7700, intelligence: 3567, constitution: 29936, luck: 5262, health: 39874752, minWeaponDmg: 555, maxWeaponDmg: 1108, armorMultiplier: 1.5),
                    new DungeonEnemy(position: 68, @class: ClassType.Mage, level: 334, strength: 3568, dexterity: 3609, intelligence: 7768, constitution: 30188, luck: 5305, health: 20225960, minWeaponDmg: 1006, maxWeaponDmg: 2009, armorMultiplier: 1.5),
                    new DungeonEnemy(position: 69, @class: ClassType.Bert, level: 336, strength: 7833, dexterity: 3609, intelligence: 3645, constitution: 30424, luck: 5347, health: 51264440, minWeaponDmg: 450, maxWeaponDmg: 897, armorMultiplier: 1.5),
                    new DungeonEnemy(position: 70, @class: ClassType.Bert, level: 338, strength: 7900, dexterity: 3641, intelligence: 3687, constitution: 30676, luck: 5392, health: 51995820, minWeaponDmg: 452, maxWeaponDmg: 903, armorMultiplier: 1.5),
                    new DungeonEnemy(position: 71, @class: ClassType.Bert, level: 340, strength: 7967, dexterity: 3680, intelligence: 3728, constitution: 30912, luck: 5436, health: 52704960, minWeaponDmg: 455, maxWeaponDmg: 908, armorMultiplier: 1.5),
                    new DungeonEnemy(position: 72, @class: ClassType.Bert, level: 342, strength: 8031, dexterity: 3717, intelligence: 3764, constitution: 31168, luck: 5480, health: 53453120, minWeaponDmg: 458, maxWeaponDmg: 913, armorMultiplier: 1.5),
                    new DungeonEnemy(position: 73, @class: ClassType.Mage, level: 344, strength: 3756, dexterity: 3805, intelligence: 8100, constitution: 31408, luck: 5522, health: 21671520, minWeaponDmg: 1035, maxWeaponDmg: 2068, armorMultiplier: 1.5),
                    new DungeonEnemy(position: 74, @class: ClassType.Bert, level: 346, strength: 8167, dexterity: 3790, intelligence: 3844, constitution: 31656, luck: 5566, health: 54923160, minWeaponDmg: 462, maxWeaponDmg: 924, armorMultiplier: 1.5),
                    new DungeonEnemy(position: 75, @class: ClassType.Bert, level: 348, strength: 8229, dexterity: 3829, intelligence: 3886, constitution: 31908, luck: 5611, health: 55679460, minWeaponDmg: 466, maxWeaponDmg: 929, armorMultiplier: 1.5),
                    new DungeonEnemy(position: 76, @class: ClassType.Bert, level: 350, strength: 8297, dexterity: 3868, intelligence: 3923, constitution: 32152, luck: 5651, health: 56426760, minWeaponDmg: 468, maxWeaponDmg: 935, armorMultiplier: 1.5),
                    new DungeonEnemy(position: 77, @class: ClassType.Bert, level: 352, strength: 8541, dexterity: 3976, intelligence: 4007, constitution: 33072, luck: 5767, health: 58372080, minWeaponDmg: 471, maxWeaponDmg: 940, armorMultiplier: 1.5),
                    new DungeonEnemy(position: 78, @class: ClassType.Bert, level: 354, strength: 8787, dexterity: 4088, intelligence: 4093, constitution: 33976, luck: 5881, health: 60307400, minWeaponDmg: 474, maxWeaponDmg: 945, armorMultiplier: 1.5),
                    new DungeonEnemy(position: 79, @class: ClassType.Scout, level: 356, strength: 4199, dexterity: 9029, intelligence: 4175, constitution: 34896, luck: 5997, health: 49831488, minWeaponDmg: 595, maxWeaponDmg: 1188, armorMultiplier: 1.5),
                    new DungeonEnemy(position: 80, @class: ClassType.Bert, level: 358, strength: 9274, dexterity: 4313, intelligence: 4256, constitution: 35824, luck: 6107, health: 64304080, minWeaponDmg: 479, maxWeaponDmg: 956, armorMultiplier: 1.5),
                    new DungeonEnemy(position: 81, @class: ClassType.Bert, level: 360, strength: 9996, dexterity: 4642, intelligence: 4556, constitution: 38556, luck: 6534, health: 69593584, minWeaponDmg: 482, maxWeaponDmg: 961, armorMultiplier: 1.5),
                    new DungeonEnemy(position: 82, @class: ClassType.Bert, level: 362, strength: 10761, dexterity: 4999, intelligence: 4877, constitution: 41494, luck: 6989, health: 75311608, minWeaponDmg: 484, maxWeaponDmg: 967, armorMultiplier: 1.5),
                    new DungeonEnemy(position: 83, @class: ClassType.Bert, level: 364, strength: 11583, dexterity: 5380, intelligence: 5213, constitution: 44629, luck: 7466, health: 81447928, minWeaponDmg: 487, maxWeaponDmg: 972, armorMultiplier: 1.5),
                    new DungeonEnemy(position: 84, @class: ClassType.Bert, level: 366, strength: 12460, dexterity: 5781, intelligence: 5578, constitution: 47979, luck: 7980, health: 88041464, minWeaponDmg: 490, maxWeaponDmg: 977, armorMultiplier: 1.5),
                    new DungeonEnemy(position: 85, @class: ClassType.Bert, level: 368, strength: 13397, dexterity: 6214, intelligence: 5967, constitution: 51532, luck: 8525, health: 95076544, minWeaponDmg: 492, maxWeaponDmg: 983, armorMultiplier: 1.5),
                    new DungeonEnemy(position: 86, @class: ClassType.Mage, level: 370, strength: 6418, dexterity: 6128, intelligence: 13843, constitution: 53237, luck: 8757, health: 39501856, minWeaponDmg: 1114, maxWeaponDmg: 2225, armorMultiplier: 1.5),
                    new DungeonEnemy(position: 87, @class: ClassType.Bert, level: 372, strength: 14300, dexterity: 6631, intelligence: 6299, constitution: 54963, luck: 8990, health: 102505992, minWeaponDmg: 498, maxWeaponDmg: 993, armorMultiplier: 1.5),
                    new DungeonEnemy(position: 88, @class: ClassType.Bert, level: 374, strength: 14765, dexterity: 6840, intelligence: 6470, constitution: 56701, luck: 9233, health: 106314376, minWeaponDmg: 500, maxWeaponDmg: 999, armorMultiplier: 1.5),
                    new DungeonEnemy(position: 89, @class: ClassType.Bert, level: 376, strength: 15233, dexterity: 7059, intelligence: 6649, constitution: 58485, luck: 9478, health: 110244224, minWeaponDmg: 503, maxWeaponDmg: 1004, armorMultiplier: 1.5),
                    new DungeonEnemy(position: 90, @class: ClassType.Bert, level: 378, strength: 15716, dexterity: 7280, intelligence: 6822, constitution: 60298, luck: 9716, health: 114264712, minWeaponDmg: 506, maxWeaponDmg: 1009, armorMultiplier: 1.5),
                    new DungeonEnemy(position: 91, @class: ClassType.Bert, level: 380, strength: 16203, dexterity: 7500, intelligence: 7006, constitution: 62140, luck: 9973, health: 118376704, minWeaponDmg: 508, maxWeaponDmg: 1015, armorMultiplier: 1.5),
                    new DungeonEnemy(position: 92, @class: ClassType.Bert, level: 382, strength: 16700, dexterity: 7728, intelligence: 7191, constitution: 64001, luck: 10226, health: 122561912, minWeaponDmg: 511, maxWeaponDmg: 1020, armorMultiplier: 1.5),
                    new DungeonEnemy(position: 93, @class: ClassType.Bert, level: 384, strength: 17199, dexterity: 7960, intelligence: 7373, constitution: 65912, luck: 10492, health: 126880600, minWeaponDmg: 514, maxWeaponDmg: 1025, armorMultiplier: 1.5),
                    new DungeonEnemy(position: 94, @class: ClassType.Bert, level: 386, strength: 17717, dexterity: 8198, intelligence: 7566, constitution: 67861, luck: 10748, health: 131311032, minWeaponDmg: 516, maxWeaponDmg: 1031, armorMultiplier: 1.5),
                    new DungeonEnemy(position: 95, @class: ClassType.Bert, level: 388, strength: 18240, dexterity: 8432, intelligence: 7757, constitution: 69809, luck: 11021, health: 135778512, minWeaponDmg: 519, maxWeaponDmg: 1036, armorMultiplier: 1.5),
                    new DungeonEnemy(position: 96, @class: ClassType.Bert, level: 390, strength: 18766, dexterity: 8678, intelligence: 7954, constitution: 71816, luck: 11296, health: 140400288, minWeaponDmg: 522, maxWeaponDmg: 1041, armorMultiplier: 1.5),
                    new DungeonEnemy(position: 97, @class: ClassType.Bert, level: 392, strength: 19306, dexterity: 8927, intelligence: 8151, constitution: 73848, luck: 11569, health: 145111328, minWeaponDmg: 524, maxWeaponDmg: 1047, armorMultiplier: 1.5),
                    new DungeonEnemy(position: 98, @class: ClassType.Bert, level: 394, strength: 19856, dexterity: 9175, intelligence: 8354, constitution: 75919, luck: 11850, health: 149940032, minWeaponDmg: 527, maxWeaponDmg: 1052, armorMultiplier: 1.5),
                    new DungeonEnemy(position: 99, @class: ClassType.Bert, level: 396, strength: 20412, dexterity: 9432, intelligence: 8563, constitution: 78007, luck: 12138, health: 154843888, minWeaponDmg: 530, maxWeaponDmg: 1057, armorMultiplier: 1.5),
                    new DungeonEnemy(position: 100, @class: ClassType.Bert, level: 398, strength: 20977, dexterity: 9686, intelligence: 8768, constitution: 80151, luck: 12429, health: 159901248, minWeaponDmg: 532, maxWeaponDmg: 1063, armorMultiplier: 1.5),
                }
            },

            #endregion

            #region Twister

            new Dungeon
            {
                Name = DungeonNames.Twister,
                Position = 99,
                IsDefeated = false,
                IsUnlocked = true,
                Type = DungeonTypeEnum.Twister,
                UnlockResolve = c => c.Dungeons.First(d => d.Type == DungeonTypeEnum.Tower).IsUnlocked,
                DungeonEnemies =
                [
                    // Not confirmed: 
                    new (position: 1, @class: ClassType.Warrior, level: 100, strength: 1_840, dexterity: 610, intelligence: 585, constitution: 2_179, luck: 490, health: 1_100_395, minWeaponDmg: 1, maxWeaponDmg: 1),
                    new (position: 2, @class: ClassType.Mage, level: 105, strength: 480, dexterity: 500, intelligence: 2_040, constitution: 1_680, luck: 715, health: 356_160, minWeaponDmg: 1, maxWeaponDmg: 1),
                    new (position: 3, @class: ClassType.Scout, level: 110, strength: 520, dexterity: 2_040, intelligence: 510, constitution: 1_848, luck: 750, health: 820_512, minWeaponDmg: 1, maxWeaponDmg: 1),
                    new (position: 4, @class: ClassType.Scout, level: 115, strength: 550, dexterity: 2_160, intelligence: 540, constitution: 1_974, luck: 795, health: 915_936, minWeaponDmg: 1, maxWeaponDmg: 1),
                    new (position: 5, @class: ClassType.Mage, level: 120, strength: 550, dexterity: 570, intelligence: 2_320, constitution: 1_974, luck: 820, health: 477_708, minWeaponDmg: 1, maxWeaponDmg: 1),
                    new (position: 6, @class: ClassType.Scout, level: 125, strength: 600, dexterity: 2_360, intelligence: 590, constitution: 2_184, luck: 870, health: 1_100_736, minWeaponDmg: 1, maxWeaponDmg: 1),
                    new (position: 7, @class: ClassType.Scout, level: 130, strength: 620, dexterity: 2_440, intelligence: 610, constitution: 2_268, luck: 900, health: 1_188_432, minWeaponDmg: 1, maxWeaponDmg: 1),
                    new (position: 8, @class: ClassType.Scout, level: 135, strength: 650, dexterity: 2_560, intelligence: 640, constitution: 2_394, luck: 945, health: 1_302_336, minWeaponDmg: 1, maxWeaponDmg: 1),
                    new (position: 9, @class: ClassType.Mage, level: 140, strength: 650, dexterity: 670, intelligence: 2_720, constitution: 2_394, luck: 970, health: 675_108, minWeaponDmg: 1, maxWeaponDmg: 1),
                    new (position: 10, @class: ClassType.Scout, level: 145, strength: 700, dexterity: 2_760, intelligence: 690, constitution: 2_604, luck: 1_020, health: 1_520_736, minWeaponDmg: 1, maxWeaponDmg: 1),
                    new (position: 11, @class: ClassType.Mage, level: 150, strength: 700, dexterity: 720, intelligence: 2_920, constitution: 2_604, luck: 1_045, health: 786_408, minWeaponDmg: 1, maxWeaponDmg: 1),
                    new (position: 12, @class: ClassType.Mage, level: 152, strength: 710, dexterity: 730, intelligence: 2_960, constitution: 2_646, luck: 1_060, health: 809_676, minWeaponDmg: 1, maxWeaponDmg: 1),
                    new (position: 13, @class: ClassType.Warrior, level: 154, strength: 2_920, dexterity: 1_015, intelligence: 990, constitution: 3_687, luck: 760, health: 2_857_425, minWeaponDmg: 1, maxWeaponDmg: 1),
                    new (position: 14, @class: ClassType.Warrior, level: 156, strength: 2_960, dexterity: 1_030, intelligence: 1_005, constitution: 3_742, luck: 770, health: 2_937_470, minWeaponDmg: 1, maxWeaponDmg: 1),
                    new (position: 15, @class: ClassType.Warrior, level: 158, strength: 3_000, dexterity: 1_045, intelligence: 1_020, constitution: 3_799, luck: 780, health: 3_020_205, minWeaponDmg: 1, maxWeaponDmg: 1),
                    new (position: 16, @class: ClassType.Mage, level: 160, strength: 750, dexterity: 770, intelligence: 3_120, constitution: 2_814, luck: 1_120, health: 906_108, minWeaponDmg: 1, maxWeaponDmg: 1),
                    new (position: 17, @class: ClassType.Warrior, level: 162, strength: 3_080, dexterity: 1_075, intelligence: 1_050, constitution: 3_910, luck: 800, health: 3_186_650, minWeaponDmg: 1, maxWeaponDmg: 1),
                    new (position: 18, @class: ClassType.Mage, level: 164, strength: 770, dexterity: 790, intelligence: 3_200, constitution: 2_898, luck: 1_150, health: 956_340, minWeaponDmg: 1, maxWeaponDmg: 1),
                    new (position: 19, @class: ClassType.Warrior, level: 166, strength: 3_160, dexterity: 1_105, intelligence: 1_080, constitution: 4_022, luck: 820, health: 3_358_370, minWeaponDmg: 1, maxWeaponDmg: 1),
                    new (position: 20, @class: ClassType.Warrior, level: 168, strength: 3_200, dexterity: 1_120, intelligence: 1_095, constitution: 4_078, luck: 830, health: 3_445_910, minWeaponDmg: 1, maxWeaponDmg: 1),
                    new (position: 21, @class: ClassType.Mage, level: 170, strength: 800, dexterity: 820, intelligence: 3_320, constitution: 3_024, luck: 1_195, health: 1_034_208, minWeaponDmg: 1, maxWeaponDmg: 1),
                    new (position: 22, @class: ClassType.Mage, level: 172, strength: 810, dexterity: 830, intelligence: 3_360, constitution: 3_066, luck: 1_210, health: 1_060_836, minWeaponDmg: 1, maxWeaponDmg: 1),
                    new (position: 23, @class: ClassType.Mage, level: 174, strength: 820, dexterity: 840, intelligence: 3_400, constitution: 3_108, luck: 1_225, health: 1_087_800, minWeaponDmg: 1, maxWeaponDmg: 1),
                    new (position: 24, @class: ClassType.Mage, level: 176, strength: 830, dexterity: 850, intelligence: 2_440, constitution: 3_150, luck: 1_240, health: 1_115_100, minWeaponDmg: 1, maxWeaponDmg: 1),
                    new (position: 25, @class: ClassType.Scout, level: 178, strength: 860, dexterity: 3_400, intelligence: 850, constitution: 3_276, luck: 1_260, health: 2_345_616, minWeaponDmg: 1, maxWeaponDmg: 1),
                    new (position: 26, @class: ClassType.Scout, level: 180, strength: 870, dexterity: 3_440, intelligence: 860, constitution: 3_318, luck: 1_275, health: 2_402_232, minWeaponDmg: 1, maxWeaponDmg: 1),
                    new (position: 27, @class: ClassType.Mage, level: 182, strength: 875, dexterity: 895, intelligence: 3_620, constitution: 3_339, luck: 1_305, health: 1_222_074, minWeaponDmg: 1, maxWeaponDmg: 1),
                    new (position: 28, @class: ClassType.Mage, level: 184, strength: 875, dexterity: 895, intelligence: 3_620, constitution: 3_339, luck: 1_305, health: 1_235_430, minWeaponDmg: 1, maxWeaponDmg: 1),
                    new (position: 29, @class: ClassType.Mage, level: 186, strength: 900, dexterity: 920, intelligence: 3_720, constitution: 3_444, luck: 1_340, health: 1_288_056, minWeaponDmg: 1, maxWeaponDmg: 1),
                    new (position: 30, @class: ClassType.Warrior, level: 188, strength: 3_640, dexterity: 1_280, intelligence: 1_255, constitution: 4_692, luck: 930, health: 4_433_940, minWeaponDmg: 1, maxWeaponDmg: 1),
                    new (position: 31, @class: ClassType.Warrior, level: 190, strength: 3_640, dexterity: 1_280, intelligence: 1_255, constitution: 4_692, luck: 930, health: 4_480_860, minWeaponDmg: 1, maxWeaponDmg: 1),
                    new (position: 32, @class: ClassType.Warrior, level: 192, strength: 3_740, dexterity: 1_315, intelligence: 1_290, constitution: 4_832, luck: 950, health: 4_662_880, minWeaponDmg: 1, maxWeaponDmg: 1),
                    new (position: 33, @class: ClassType.Mage, level: 194, strength: 925, dexterity: 945, intelligence: 3_820, constitution: 3_549, luck: 1_375, health: 1_384_110, minWeaponDmg: 1, maxWeaponDmg: 1),
                    new (position: 34, @class: ClassType.Mage, level: 196, strength: 950, dexterity: 970, intelligence: 3_920, constitution: 3_654, luck: 1_410, health: 1_439_676, minWeaponDmg: 1, maxWeaponDmg: 1),
                    new (position: 35, @class: ClassType.Mage, level: 198, strength: 950, dexterity: 970, intelligence: 3_920, constitution: 3_654, luck: 1_410, health: 1_454_292, minWeaponDmg: 1, maxWeaponDmg: 1),
                    new (position: 36, @class: ClassType.Mage, level: 200, strength: 950, dexterity: 970, intelligence: 3_920, constitution: 3_654, luck: 1_410, health: 1_468_908, minWeaponDmg: 1, maxWeaponDmg: 1),
                    new (position: 37, @class: ClassType.Scout, level: 201, strength: 995, dexterity: 3_940, intelligence: 985, constitution: 3_843, luck: 1_450, health: 3_105_144, minWeaponDmg: 1, maxWeaponDmg: 1),
                    new (position: 38, @class: ClassType.Scout, level: 202, strength: 995, dexterity: 3_940, intelligence: 985, constitution: 3_843, luck: 1_450, health: 3_120_516, minWeaponDmg: 1, maxWeaponDmg: 1),
                    new (position: 39, @class: ClassType.Mage, level: 203, strength: 975, dexterity: 995, intelligence: 4_020, constitution: 3_759, luck: 1_445, health: 1_533_672, minWeaponDmg: 1, maxWeaponDmg: 1),
                    new (position: 40, @class: ClassType.Scout, level: 204, strength: 995, dexterity: 3_940, intelligence: 985, constitution: 3_843, luck: 1_450, health: 3_151_260, minWeaponDmg: 1, maxWeaponDmg: 1),
                    new (position: 41, @class: ClassType.Scout, level: 205, strength: 995, dexterity: 3_940, intelligence: 985, constitution: 3_843, luck: 1_450, health: 3_166_632, minWeaponDmg: 1, maxWeaponDmg: 1),
                    new (position: 42, @class: ClassType.Warrior, level: 206, strength: 4_040, dexterity: 1_420, intelligence: 1_395, constitution: 5_251, luck: 1_010, health: 5_434_785, minWeaponDmg: 1, maxWeaponDmg: 1),
                    new (position: 43, @class: ClassType.Warrior, level: 207, strength: 4_040, dexterity: 1_420, intelligence: 1_395, constitution: 5_251, luck: 1_010, health: 5_461_040, minWeaponDmg: 1, maxWeaponDmg: 1),
                    new (position: 44, @class: ClassType.Mage, level: 208, strength: 1_000, dexterity: 1_020, intelligence: 4_120, constitution: 3_864, luck: 1_480, health: 1_615_152, minWeaponDmg: 1, maxWeaponDmg: 1),
                    new (position: 45, @class: ClassType.Scout, level: 209, strength: 1_020, dexterity: 4_040, intelligence: 1_010, constitution: 3_948, luck: 1_485, health: 3_316_320, minWeaponDmg: 1, maxWeaponDmg: 1),
                    new (position: 46, @class: ClassType.Mage, level: 210, strength: 1_000, dexterity: 1_020, intelligence: 4_120, constitution: 3_864, luck: 1_480, health: 1_630_608, minWeaponDmg: 1, maxWeaponDmg: 1),
                    new (position: 47, @class: ClassType.Scout, level: 211, strength: 1_045, dexterity: 4_140, intelligence: 1_035, constitution: 4_053, luck: 1_520, health: 3_436_944, minWeaponDmg: 1, maxWeaponDmg: 1),
                    new (position: 48, @class: ClassType.Mage, level: 212, strength: 1_025, dexterity: 1_045, intelligence: 4_220, constitution: 3_969, luck: 1_515, health: 1_690_794, minWeaponDmg: 1, maxWeaponDmg: 1),
                    new (position: 49, @class: ClassType.Warrior, level: 213, strength: 4_140, dexterity: 1_455, intelligence: 1_430, constitution: 5_391, luck: 1_030, health: 5_768_370, minWeaponDmg: 1, maxWeaponDmg: 1),
                    new (position: 50, @class: ClassType.Warrior, level: 214, strength: 4140, dexterity: 1455, intelligence: 1430, constitution: 5391, luck: 1030, health: 5_795_325, minWeaponDmg: 235, maxWeaponDmg: 470 ),

                    new (position: 51, @class: ClassType.Scout, level: 215, strength: 1_045, dexterity: 4_140, intelligence: 1_035, constitution: 4_053, luck: 1_520, health: 3_501_792, minWeaponDmg: 1, maxWeaponDmg: 1),
                    new (position: 52, @class: ClassType.Scout, level: 216, strength: 1_070, dexterity: 4_240, intelligence: 1_060, constitution: 4_158, luck: 1_555, health: 3_609_144, minWeaponDmg: 1, maxWeaponDmg: 1),
                    new (position: 53, @class: ClassType.Mage, level: 217, strength: 1_050, dexterity: 1_070, intelligence: 4_320, constitution: 4_074, luck: 1_550, health: 1_776_264, minWeaponDmg: 1, maxWeaponDmg: 1),
                    new (position: 54, @class: ClassType.Warrior, level: 218, strength: 4_240, dexterity: 1_490, intelligence: 1_465, constitution: 5_530, luck: 1_050, health: 6_055_350, minWeaponDmg: 1, maxWeaponDmg: 1),
                    new (position: 55, @class: ClassType.Mage, level: 219, strength: 1_050, dexterity: 1_070, intelligence: 4_320, constitution: 4_074, luck: 1_550, health: 1_792_560, minWeaponDmg: 1, maxWeaponDmg: 1),
                    new (position: 56, @class: ClassType.Warrior, level: 220, strength: 4_240, dexterity: 1_490, intelligence: 1_465, constitution: 5_530, luck: 1_050, health: 6_110_650, minWeaponDmg: 1, maxWeaponDmg: 1),
                    new (position: 57, @class: ClassType.Warrior, level: 221, strength: 4_340, dexterity: 1_525, intelligence: 1_500, constitution: 5_670, luck: 1_070, health: 6_293_700, minWeaponDmg: 1, maxWeaponDmg: 1),
                    new (position: 58, @class: ClassType.Mage, level: 222, strength: 1_075, dexterity: 1_095, intelligence: 4_420, constitution: 4_179, luck: 1_585, health: 1_863_834, minWeaponDmg: 1, maxWeaponDmg: 1),
                    new (position: 59, @class: ClassType.Scout, level: 223, strength: 1_095, dexterity: 4_340, intelligence: 1_085, constitution: 4_263, luck: 1_590, health: 3_819_648, minWeaponDmg: 1, maxWeaponDmg: 1),
                    new (position: 60, @class: ClassType.Scout, level: 224, strength: 1_095, dexterity: 4_340, intelligence: 1_085, constitution: 4_263, luck: 1_590, health: 3_836_700, minWeaponDmg: 1, maxWeaponDmg: 1),
                    new (position: 61, @class: ClassType.Mage, level: 225, strength: 1_075, dexterity: 1_095, intelligence: 4_420, constitution: 4_179, luck: 1_585, health: 1_888_908, minWeaponDmg: 1, maxWeaponDmg: 1),
                    new (position: 62, @class: ClassType.Scout, level: 226, strength: 1_120, dexterity: 4_440, intelligence: 1_110, constitution: 4_368, luck: 1_625, health: 3_966_144, minWeaponDmg: 1, maxWeaponDmg: 1),
                    new (position: 63, @class: ClassType.Mage, level: 227, strength: 1_100, dexterity: 1_120, intelligence: 4_520, constitution: 4_284, luck: 1_620, health: 1_953_504, minWeaponDmg: 1, maxWeaponDmg: 1),
                    new (position: 64, @class: ClassType.Warrior, level: 228, strength: 4_440, dexterity: 1_560, intelligence: 1_535, constitution: 5_810, luck: 1_090, health: 6_652_450, minWeaponDmg: 1, maxWeaponDmg: 1),
                    new (position: 65, @class: ClassType.Warrior, level: 229, strength: 4_440, dexterity: 1_560, intelligence: 1_535, constitution: 5_810, luck: 1_090, health: 6_681_500, minWeaponDmg: 1, maxWeaponDmg: 1),
                    new (position: 66, @class: ClassType.Mage, level: 230, strength: 1_100, dexterity: 1_120, intelligence: 4_520, constitution: 4_284, luck: 1_620, health: 1_979_208, minWeaponDmg: 1, maxWeaponDmg: 1),
                    new (position: 67, @class: ClassType.Mage, level: 231, strength: 1_125, dexterity: 1_145, intelligence: 4_620, constitution: 4_389, luck: 1_655, health: 2_036_496, minWeaponDmg: 1, maxWeaponDmg: 1),
                    new (position: 68, @class: ClassType.Mage, level: 232, strength: 1_125, dexterity: 1_145, intelligence: 4_620, constitution: 4_389, luck: 1_655, health: 2_045_274, minWeaponDmg: 1, maxWeaponDmg: 1),
                    new (position: 69, @class: ClassType.Warrior, level: 233, strength: 4_540, dexterity: 1_595, intelligence: 1_570, constitution: 5_949, luck: 1_110, health: 6_960_330, minWeaponDmg: 1, maxWeaponDmg: 1),
                    new (position: 70, @class: ClassType.Mage, level: 234, strength: 1_125, dexterity: 1_145, intelligence: 4_620, constitution: 4_389, luck: 1_655, health: 2_062_830, minWeaponDmg: 1, maxWeaponDmg: 1),
                    new (position: 71, @class: ClassType.Warrior, level: 235, strength: 4_540, dexterity: 1_595, intelligence: 1_570, constitution: 5_949, luck: 1_110, health: 7_019_820, minWeaponDmg: 1, maxWeaponDmg: 1),
                    new (position: 72, @class: ClassType.Warrior, level: 236, strength: 4_640, dexterity: 1_630, intelligence: 1_605, constitution: 6_089, luck: 1_130, health: 7_215_465, minWeaponDmg: 1, maxWeaponDmg: 1),
                    new (position: 73, @class: ClassType.Scout, level: 237, strength: 1_170, dexterity: 4_640, intelligence: 1_160, constitution: 4_578, luck: 1_695, health: 4_358_256, minWeaponDmg: 1, maxWeaponDmg: 1),
                    new (position: 74, @class: ClassType.Scout, level: 238, strength: 1_170, dexterity: 4_640, intelligence: 1_160, constitution: 4_578, luck: 1_695, health: 4_376_568, minWeaponDmg: 1, maxWeaponDmg: 1),
                    new (position: 75, @class: ClassType.Scout, level: 239, strength: 1_170, dexterity: 4_640, intelligence: 1_160, constitution: 4_578, luck: 1_695, health: 4_394_880, minWeaponDmg: 1, maxWeaponDmg: 1),
                    new (position: 76, @class: ClassType.Warrior, level: 240, strength: 4_640, dexterity: 1_630, intelligence: 1_605, constitution: 6_089, luck: 1_130, health: 7_337_245, minWeaponDmg: 1, maxWeaponDmg: 1),
                    new (position: 77, @class: ClassType.Mage, level: 241, strength: 1_175, dexterity: 1_195, intelligence: 4_820, constitution: 4_599, luck: 1_725, health: 2_225_916, minWeaponDmg: 1, maxWeaponDmg: 1),
                    new (position: 78, @class: ClassType.Warrior, level: 242, strength: 4_740, dexterity: 1_665, intelligence: 1_640, constitution: 6_229, luck: 1_150, health: 7_568_235, minWeaponDmg: 1, maxWeaponDmg: 1),
                    new (position: 79, @class: ClassType.Scout, level: 243, strength: 1_195, dexterity: 4_740, intelligence: 1_185, constitution: 4_683, luck: 1_730, health: 4_570_608, minWeaponDmg: 1, maxWeaponDmg: 1),
                    new (position: 80, @class: ClassType.Mage, level: 244, strength: 1_175, dexterity: 1_195, intelligence: 4_820, constitution: 4_599, luck: 1_725, health: 2_253_510, minWeaponDmg: 1, maxWeaponDmg: 1),
                    new (position: 81, @class: ClassType.Scout, level: 245, strength: 1_195, dexterity: 4_740, intelligence: 1_185, constitution: 4_683, luck: 1_730, health: 4_608_072, minWeaponDmg: 1, maxWeaponDmg: 1),
                    new (position: 82, @class: ClassType.Mage, level: 246, strength: 1_200, dexterity: 1_220, intelligence: 4_920, constitution: 4_704, luck: 1_760, health: 2_323_776, minWeaponDmg: 1, maxWeaponDmg: 1),
                    new (position: 83, @class: ClassType.Warrior, level: 247, strength: 4_840, dexterity: 1_700, intelligence: 1_675, constitution: 6_368, luck: 1_170, health: 7_896_320, minWeaponDmg: 1, maxWeaponDmg: 1),
                    new (position: 84, @class: ClassType.Mage, level: 248, strength: 1_200, dexterity: 1_220, intelligence: 4_920, constitution: 4_704, luck: 1_760, health: 2_342_592, minWeaponDmg: 1, maxWeaponDmg: 1),
                    new (position: 85, @class: ClassType.Mage, level: 249, strength: 1_200, dexterity: 1_220, intelligence: 4_920, constitution: 4_704, luck: 1_760, health: 2_352_000, minWeaponDmg: 1, maxWeaponDmg: 1),
                    new (position: 86, @class: ClassType.Mage, level: 250, strength: 1_200, dexterity: 1_220, intelligence: 4_920, constitution: 4_704, luck: 1_760, health: 2_361_408, minWeaponDmg: 1, maxWeaponDmg: 1),
                    new (position: 87, @class: ClassType.Scout, level: 250, strength: 1_220, dexterity: 4_840, intelligence: 1_210, constitution: 4_788, luck: 1_765, health: 4_807_152, minWeaponDmg: 1, maxWeaponDmg: 1),
                    new (position: 88, @class: ClassType.Warrior, level: 251, strength: 4_860, dexterity: 1_707, intelligence: 1_682, constitution: 6_396, luck: 1_174, health: 8_058_960, minWeaponDmg: 1, maxWeaponDmg: 1),
                    new (position: 89, @class: ClassType.Warrior, level: 252, strength: 4_880, dexterity: 1_714, intelligence: 1_689, constitution: 6_424, luck: 1_178, health: 8_126_360, minWeaponDmg: 1, maxWeaponDmg: 1),
                    new (position: 90, @class: ClassType.Mage, level: 252, strength: 1_210, dexterity: 1_230, intelligence: 4_960, constitution: 4_746, luck: 1_774, health: 2_401_476, minWeaponDmg: 1, maxWeaponDmg: 1),
                    new (position: 91, @class: ClassType.Warrior, level: 252, strength: 4_880, dexterity: 1_714, intelligence: 1_689, constitution: 6_424, luck: 1_178, health: 8_126_360, minWeaponDmg: 1, maxWeaponDmg: 1),
                    new (position: 92, @class: ClassType.Mage, level: 253, strength: 1_215, dexterity: 1_235, intelligence: 4_980, constitution: 4_767, luck: 1_781, health: 2_421_636, minWeaponDmg: 1, maxWeaponDmg: 1),
                    new (position: 93, @class: ClassType.Warrior, level: 254, strength: 4_920, dexterity: 1_728, intelligence: 1_703, constitution: 6_480, luck: 1_186, health: 8_262_000, minWeaponDmg: 1, maxWeaponDmg: 1),
                    new (position: 94, @class: ClassType.Warrior, level: 254, strength: 4_920, dexterity: 1_728, intelligence: 1_703, constitution: 6_480, luck: 1_186, health: 8_262_000, minWeaponDmg: 1, maxWeaponDmg: 1),
                    new (position: 95, @class: ClassType.Scout, level: 254, strength: 1_240, dexterity: 4_920, intelligence: 1_230, constitution: 4_872, luck: 1_793, health: 4_969_440, minWeaponDmg: 1, maxWeaponDmg: 1),
                    new (position: 96, @class: ClassType.Warrior, level: 255, strength: 4_940, dexterity: 1_735, intelligence: 1_710, constitution: 6_508, luck: 1_190, health: 8_330_240, minWeaponDmg: 1, maxWeaponDmg: 1),
                    new (position: 97, @class: ClassType.Mage, level: 256, strength: 1_230, dexterity: 1_250, intelligence: 5_040, constitution: 4_830, luck: 1_802, health: 2_482_620, minWeaponDmg: 1, maxWeaponDmg: 1),
                    new (position: 98, @class: ClassType.Scout, level: 256, strength: 1_250, dexterity: 4_960, intelligence: 1_240, constitution: 4_914, luck: 1_807, health: 5_051_592, minWeaponDmg: 1, maxWeaponDmg: 1),
                    new (position: 99, @class: ClassType.Warrior, level: 256, strength: 4_960, dexterity: 1_742, intelligence: 1_717, constitution: 6_535, luck: 1_194, health: 8_397_475, minWeaponDmg: 1, maxWeaponDmg: 1),
                    new (position: 100, @class: ClassType.Mage, level: 257, strength: 1235, dexterity: 1255, intelligence: 5060, constitution: 4851, luck: 1809, health: 2_503_116, minWeaponDmg: 636, maxWeaponDmg: 1272 ),

                    new (position: 101, @class: ClassType.Scout, level: 258, strength: 1_273, dexterity: 5_050, intelligence: 1_263, constitution: 5_005, luck: 1_839, health: 5_185_180, minWeaponDmg: 1, maxWeaponDmg: 1),
                    new (position: 102, @class: ClassType.Warrior, level: 258, strength: 5_100, dexterity: 1_791, intelligence: 1_766, constitution: 6_724, luck: 1_226, health: 8_707_580, minWeaponDmg: 1, maxWeaponDmg: 1),
                    new (position: 103, @class: ClassType.Mage, level: 258, strength: 1_277, dexterity: 1_298, intelligence: 5_232, constitution: 5_018, luck: 1_870, health: 2_599_324, minWeaponDmg: 1, maxWeaponDmg: 1),
                    new (position: 104, @class: ClassType.Mage, level: 259, strength: 1_295, dexterity: 1_316, intelligence: 5_304, constitution: 5_088, luck: 1_896, health: 2_645_760, minWeaponDmg: 1, maxWeaponDmg: 1),
                    new (position: 105, @class: ClassType.Scout, level: 260, strength: 1_334, dexterity: 5_292, intelligence: 1_323, constitution: 5_248, luck: 1_927, health: 5_478_912, minWeaponDmg: 1, maxWeaponDmg: 1),
                    new (position: 106, @class: ClassType.Mage, level: 260, strength: 1_325, dexterity: 1_346, intelligence: 5_427, constitution: 5_209, luck: 1_940, health: 2_719_098, minWeaponDmg: 1, maxWeaponDmg: 1),
                    new (position: 107, @class: ClassType.Mage, level: 260, strength: 1_338, dexterity: 1_359, intelligence: 5_478, constitution: 5_258, luck: 1_958, health: 2_744_676, minWeaponDmg: 1, maxWeaponDmg: 1),
                    new (position: 108, @class: ClassType.Mage, level: 261, strength: 1_355, dexterity: 1_377, intelligence: 5_551, constitution: 5_330, luck: 1_984, health: 2_792_920, minWeaponDmg: 1, maxWeaponDmg: 1),
                    new (position: 109, @class: ClassType.Warrior, level: 262, strength: 5_537, dexterity: 1_945, intelligence: 1_917, constitution: 7_307, luck: 1_328, health: 9_608_705, minWeaponDmg: 1, maxWeaponDmg: 1),
                    new (position: 110, @class: ClassType.Mage, level: 262, strength: 1_386, dexterity: 1_408, intelligence: 5_676, constitution: 5_452, luck: 2_028, health: 2_867_752, minWeaponDmg: 1, maxWeaponDmg: 1),
                    new (position: 111, @class: ClassType.Scout, level: 262, strength: 1_421, dexterity: 5_639, intelligence: 1_410, constitution: 5_594, luck: 2_052, health: 5_884_888, minWeaponDmg: 1, maxWeaponDmg: 1),
                    new (position: 112, @class: ClassType.Scout, level: 263, strength: 1_439, dexterity: 5_712, intelligence: 1_428, constitution: 5_668, luck: 2_079, health: 5_985_408, minWeaponDmg: 1, maxWeaponDmg: 1),
                    new (position: 113, @class: ClassType.Scout, level: 264, strength: 1_458, dexterity: 5_786, intelligence: 1_446, constitution: 5_742, luck: 2_105, health: 6_086_520, minWeaponDmg: 1, maxWeaponDmg: 1),
                    new (position: 114, @class: ClassType.Warrior, level: 264, strength: 5_837, dexterity: 2_050, intelligence: 2_021, constitution: 7_705, luck: 1_398, health: 10_209_125, minWeaponDmg: 1, maxWeaponDmg: 1),
                    new (position: 115, @class: ClassType.Warrior, level: 264, strength: 5_888, dexterity: 2_068, intelligence: 2_039, constitution: 7_773, luck: 1_410, health: 10_299_225, minWeaponDmg: 1, maxWeaponDmg: 1),
                    new (position: 116, @class: ClassType.Warrior, level: 265, strength: 5_962, dexterity: 2_094, intelligence: 2_065, constitution: 7_873, luck: 1_427, health: 10_471_090, minWeaponDmg: 1, maxWeaponDmg: 1),
                    new (position: 117, @class: ClassType.Mage, level: 266, strength: 1_498, dexterity: 1_521, intelligence: 6_131, constitution: 5_897, luck: 2_190, health: 3_148_998, minWeaponDmg: 1, maxWeaponDmg: 1),
                    new (position: 118, @class: ClassType.Warrior, level: 266, strength: 6_089, dexterity: 2_138, intelligence: 2_109, constitution: 8_041, luck: 1_456, health: 10_734_735, minWeaponDmg: 1, maxWeaponDmg: 1),
                    new (position: 119, @class: ClassType.Mage, level: 266, strength: 1_523, dexterity: 1_547, intelligence: 6_236, constitution: 5_998, luck: 2_228, health: 3_202_932, minWeaponDmg: 1, maxWeaponDmg: 1),
                    new (position: 120, @class: ClassType.Scout, level: 267, strength: 1_566, dexterity: 6_216, intelligence: 1_554, constitution: 6_174, luck: 2_261, health: 6_618_528, minWeaponDmg: 1, maxWeaponDmg: 1),
                    new (position: 121, @class: ClassType.Warrior, level: 268, strength: 6_292, dexterity: 2_209, intelligence: 2_176, constitution: 8_314, luck: 1_503, health: 11_182_330, minWeaponDmg: 1, maxWeaponDmg: 1),
                    new (position: 122, @class: ClassType.Scout, level: 268, strength: 1_598, dexterity: 6_344, intelligence: 1_586, constitution: 6_302, luck: 2_307, health: 6_780_952, minWeaponDmg: 1, maxWeaponDmg: 1),
                    new (position: 123, @class: ClassType.Scout, level: 268, strength: 1_611, dexterity: 6_396, intelligence: 1_599, constitution: 6_355, luck: 2_326, health: 6_837_980, minWeaponDmg: 1, maxWeaponDmg: 1),
                    new (position: 124, @class: ClassType.Mage, level: 269, strength: 1_606, dexterity: 1_631, intelligence: 6_572, constitution: 6_327, luck: 2_347, health: 3_416_580, minWeaponDmg: 1, maxWeaponDmg: 1),
                    new (position: 125, @class: ClassType.Warrior, level: 270, strength: 6_550, dexterity: 2_300, intelligence: 2_269, constitution: 8_658, luck: 1_562, health: 11_731_590, minWeaponDmg: 1, maxWeaponDmg: 1),
                    new (position: 126, @class: ClassType.Scout, level: 270, strength: 1_663, dexterity: 6_602, intelligence: 1_651, constitution: 6_563, luck: 2_400, health: 7_114_292, minWeaponDmg: 1, maxWeaponDmg: 1),
                    new (position: 127, @class: ClassType.Warrior, level: 270, strength: 6_655, dexterity: 2_337, intelligence: 2_305, constitution: 8_797, luck: 1_588, health: 11_919_935, minWeaponDmg: 1, maxWeaponDmg: 1),
                    new (position: 128, @class: ClassType.Mage, level: 271, strength: 1_670, dexterity: 1_696, intelligence: 6_835, constitution: 6_586, luck: 2_441, health: 3_582_784, minWeaponDmg: 1, maxWeaponDmg: 1),
                    new (position: 129, @class: ClassType.Mage, level: 272, strength: 1_690, dexterity: 1_716, intelligence: 6_914, constitution: 6_664, luck: 2_469, health: 3_638_544, minWeaponDmg: 1, maxWeaponDmg: 1),
                    new (position: 130, @class: ClassType.Warrior, level: 272, strength: 6_864, dexterity: 2_410, intelligence: 2_378, constitution: 9_077, luck: 1_635, health: 12_390_105, minWeaponDmg: 1, maxWeaponDmg: 1),
                    new (position: 131, @class: ClassType.Mage, level: 272, strength: 1_716, dexterity: 1_742, intelligence: 7_022, constitution: 6_767, luck: 2_507, health: 3_694_782, minWeaponDmg: 1, maxWeaponDmg: 1),
                    new (position: 132, @class: ClassType.Warrior, level: 273, strength: 6_996, dexterity: 2_457, intelligence: 2_424, constitution: 9_255, luck: 1_666, health: 12_679_350, minWeaponDmg: 1, maxWeaponDmg: 1),
                    new (position: 133, @class: ClassType.Mage, level: 274, strength: 1_756, dexterity: 1_782, intelligence: 7_182, constitution: 6_927, luck: 2_564, health: 3_809_850, minWeaponDmg: 1, maxWeaponDmg: 1),
                    new (position: 134, @class: ClassType.Scout, level: 274, strength: 1_796, dexterity: 7_129, intelligence: 1_782, constitution: 7_092, luck: 2_590, health: 7_801_200, minWeaponDmg: 1, maxWeaponDmg: 1),
                    new (position: 135, @class: ClassType.Scout, level: 274, strength: 1_809, dexterity: 7_182, intelligence: 1_796, constitution: 7_144, luck: 2_610, health: 7_858_400, minWeaponDmg: 1, maxWeaponDmg: 1),
                    new (position: 136, @class: ClassType.Mage, level: 275, strength: 1_802, dexterity: 1_829, intelligence: 7_371, constitution: 7_112, luck: 2_632, health: 3_925_824, minWeaponDmg: 1, maxWeaponDmg: 1),
                    new (position: 137, @class: ClassType.Warrior, level: 276, strength: 7_343, dexterity: 2_578, intelligence: 2_544, constitution: 9_719, luck: 1_745, health: 13_460_815, minWeaponDmg: 1, maxWeaponDmg: 1),
                    new (position: 138, @class: ClassType.Warrior, level: 276, strength: 7_397, dexterity: 2_597, intelligence: 2_563, constitution: 9_789, luck: 1_758, health: 13_557_765, minWeaponDmg: 1, maxWeaponDmg: 1),
                    new (position: 139, @class: ClassType.Mage, level: 276, strength: 1_849, dexterity: 1_876, intelligence: 7_562, constitution: 7_298, luck: 2_699, health: 4_043_092, minWeaponDmg: 1, maxWeaponDmg: 1),
                    new (position: 140, @class: ClassType.Scout, level: 277, strength: 1_897, dexterity: 7_532, intelligence: 1_883, constitution: 7_497, luck: 2_736, health: 8_336_664, minWeaponDmg: 1, maxWeaponDmg: 1),
                    new (position: 141, @class: ClassType.Mage, level: 278, strength: 1_889, dexterity: 1_918, intelligence: 7_727, constitution: 7_461, luck: 2_758, health: 4_163_238, minWeaponDmg: 1, maxWeaponDmg: 1),
                    new (position: 142, @class: ClassType.Warrior, level: 278, strength: 7_668, dexterity: 2_692, intelligence: 2_657, constitution: 10_154, luck: 1_820, health: 14_164_830, minWeaponDmg: 1, maxWeaponDmg: 1),
                    new (position: 143, @class: ClassType.Warrior, level: 278, strength: 7_722, dexterity: 2_711, intelligence: 2_676, constitution: 10_225, luck: 1_833, health: 14_263_875, minWeaponDmg: 1, maxWeaponDmg: 1),
                    new (position: 144, @class: ClassType.Mage, level: 279, strength: 1_937, dexterity: 1_966, intelligence: 7_920, constitution: 7_650, luck: 2_827, health: 4_284_000, minWeaponDmg: 1, maxWeaponDmg: 1),
                    new (position: 145, @class: ClassType.Mage, level: 280, strength: 1_957, dexterity: 1_986, intelligence: 8_004, constitution: 7_734, luck: 2_856, health: 4_346_508, minWeaponDmg: 1, maxWeaponDmg: 1),
                    new (position: 146, @class: ClassType.Scout, level: 280, strength: 2_000, dexterity: 7_942, intelligence: 1_986, constitution: 7_911, luck: 2_883, health: 8_891_964, minWeaponDmg: 1, maxWeaponDmg: 1),
                    new (position: 147, @class: ClassType.Scout, level: 280, strength: 2_014, dexterity: 7_997, intelligence: 1_999, constitution: 7_964, luck: 2_903, health: 8_951_536, minWeaponDmg: 1, maxWeaponDmg: 1),
                    new (position: 148, @class: ClassType.Mage, level: 281, strength: 2_005, dexterity: 2_035, intelligence: 8_199, constitution: 7_925, luck: 2_926, health: 4_469_700, minWeaponDmg: 1, maxWeaponDmg: 1),
                    new (position: 149, @class: ClassType.Scout, level: 282, strength: 2_056, dexterity: 8_165, intelligence: 2_041, constitution: 8_135, luck: 2_964, health: 9_208_820, minWeaponDmg: 1, maxWeaponDmg: 1),
                    new (position: 150, @class: ClassType.Mage, level: 282, strength: 2040, dexterity: 2070, intelligence: 8340, constitution: 8064, luck: 2976, health: 4_564_224, minWeaponDmg: 698, maxWeaponDmg: 1396 ),

                    new (position: 151, @class: ClassType.Scout, level: 282, strength: 2_084, dexterity: 8_275, intelligence: 2_069, constitution: 8_245, luck: 3_003, health: 9_333_340, minWeaponDmg: 1, maxWeaponDmg: 1),
                    new (position: 152, @class: ClassType.Mage, level: 283, strength: 2_075, dexterity: 2_105, intelligence: 8_482, constitution: 8_204, luck: 3_026, health: 4_659_872, minWeaponDmg: 1, maxWeaponDmg: 1),
                    new (position: 153, @class: ClassType.Scout, level: 284, strength: 2_127, dexterity: 8_446, intelligence: 2_111, constitution: 8_418, luck: 3_065, health: 9_596_520, minWeaponDmg: 1, maxWeaponDmg: 1),
                    new (position: 154, @class: ClassType.Mage, level: 284, strength: 2_110, dexterity: 2_141, intelligence: 8_624, constitution: 8_343, luck: 3_077, health: 4_755_510, minWeaponDmg: 1, maxWeaponDmg: 1),
                    new (position: 155, @class: ClassType.Scout, level: 284, strength: 2_155, dexterity: 8_556, intelligence: 2_139, constitution: 8_528, luck: 3_105, health: 9_721_920, minWeaponDmg: 1, maxWeaponDmg: 1),
                    new (position: 156, @class: ClassType.Mage, level: 285, strength: 2_145, dexterity: 2_176, intelligence: 8_767, constitution: 8_485, luck: 3_128, health: 4_853_420, minWeaponDmg: 1, maxWeaponDmg: 1),
                    new (position: 157, @class: ClassType.Warrior, level: 286, strength: 8_729, dexterity: 3_065, intelligence: 3_025, constitution: 11_576, luck: 2_063, health: 16_611_560, minWeaponDmg: 1, maxWeaponDmg: 1),
                    new (position: 158, @class: ClassType.Warrior, level: 286, strength: 8_785, dexterity: 3_084, intelligence: 3_045, constitution: 11_650, luck: 2_076, health: 16_717_750, minWeaponDmg: 1, maxWeaponDmg: 1),
                    new (position: 159, @class: ClassType.Warrior, level: 286, strength: 8_840, dexterity: 3_104, intelligence: 3_064, constitution: 11_723, luck: 2_089, health: 16_822_504, minWeaponDmg: 1, maxWeaponDmg: 1),
                    new (position: 160, @class: ClassType.Mage, level: 287, strength: 2_216, dexterity: 2_248, intelligence: 9_056, constitution: 8_770, luck: 3_230, health: 5_051_520, minWeaponDmg: 1, maxWeaponDmg: 1),
                    new (position: 161, @class: ClassType.Scout, level: 288, strength: 2_270, dexterity: 9_016, intelligence: 2_254, constitution: 8_993, luck: 3_270, health: 10_395_908, minWeaponDmg: 1, maxWeaponDmg: 1),
                    new (position: 162, @class: ClassType.Mage, level: 288, strength: 2_252, dexterity: 2_284, intelligence: 9_202, constitution: 8_913, luck: 3_282, health: 5_151_714, minWeaponDmg: 1, maxWeaponDmg: 1),
                    new (position: 163, @class: ClassType.Scout, level: 288, strength: 2_298, dexterity: 9_128, intelligence: 2_282, constitution: 9_106, luck: 3_311, health: 10_526_536, minWeaponDmg: 1, maxWeaponDmg: 1),
                    new (position: 164, @class: ClassType.Scout, level: 289, strength: 2_321, dexterity: 9_217, intelligence: 2_304, constitution: 9_196, luck: 3_342, health: 10_667_360, minWeaponDmg: 1, maxWeaponDmg: 1),
                    new (position: 165, @class: ClassType.Mage, level: 290, strength: 2_310, dexterity: 2_343, intelligence: 9_438, constitution: 9_148, luck: 3_366, health: 5_324_136, minWeaponDmg: 1, maxWeaponDmg: 1),
                    new (position: 166, @class: ClassType.Warrior, level: 290, strength: 9_362, dexterity: 3_287, intelligence: 3_245, constitution: 12_426, luck: 2_208, health: 18_079_830, minWeaponDmg: 1, maxWeaponDmg: 1),
                    new (position: 167, @class: ClassType.Mage, level: 290, strength: 2_338, dexterity: 2_371, intelligence: 9_552, constitution: 9_259, luck: 3_407, health: 5_388_738, minWeaponDmg: 1, maxWeaponDmg: 1),
                    new (position: 168, @class: ClassType.Warrior, level: 291, strength: 9_509, dexterity: 3_338, intelligence: 3_296, constitution: 12_621, luck: 2_241, health: 18_426_660, minWeaponDmg: 1, maxWeaponDmg: 1),
                    new (position: 169, @class: ClassType.Scout, level: 292, strength: 2_417, dexterity: 9_599, intelligence: 2_400, constitution: 9_582, luck: 3_480, health: 11_230_104, minWeaponDmg: 1, maxWeaponDmg: 1),
                    new (position: 170, @class: ClassType.Warrior, level: 292, strength: 9_656, dexterity: 3_390, intelligence: 3_347, constitution: 12_819, luck: 2_275, health: 18_779_836, minWeaponDmg: 1, maxWeaponDmg: 1),
                    new (position: 171, @class: ClassType.Warrior, level: 292, strength: 9_713, dexterity: 3_410, intelligence: 3_367, constitution: 12_895, luck: 2_288, health: 18_891_176, minWeaponDmg: 1, maxWeaponDmg: 1),
                    new (position: 172, @class: ClassType.Warrior, level: 293, strength: 9_804, dexterity: 3_442, intelligence: 3_399, constitution: 13_019, luck: 2_308, health: 19_137_930, minWeaponDmg: 1, maxWeaponDmg: 1),
                    new (position: 173, @class: ClassType.Warrior, level: 294, strength: 9_896, dexterity: 3_474, intelligence: 3_431, constitution: 13_143, luck: 2_329, health: 19_385_924, minWeaponDmg: 1, maxWeaponDmg: 1),
                    new (position: 174, @class: ClassType.Scout, level: 294, strength: 2_506, dexterity: 9_953, intelligence: 2_488, constitution: 9_939, luck: 3_607, health: 11_728_020, minWeaponDmg: 1, maxWeaponDmg: 1),
                    new (position: 175, @class: ClassType.Scout, level: 294, strength: 2_520, dexterity: 10_010, intelligence: 2_502, constitution: 9_996, luck: 3_628, health: 11_795_280, minWeaponDmg: 1, maxWeaponDmg: 1),
                    new (position: 176, @class: ClassType.Scout, level: 295, strength: 2_543, dexterity: 10_102, intelligence: 2_526, constitution: 10_091, luck: 3_661, health: 11_947_744, minWeaponDmg: 1, maxWeaponDmg: 1),
                    new (position: 177, @class: ClassType.Scout, level: 296, strength: 2_567, dexterity: 10_195, intelligence: 2_549, constitution: 10_185, luck: 3_694, health: 12_099_780, minWeaponDmg: 1, maxWeaponDmg: 1),
                    new (position: 178, @class: ClassType.Mage, level: 296, strength: 2_545, dexterity: 2_581, intelligence: 10_395, constitution: 10_093, luck: 3_706, health: 5_995_242, minWeaponDmg: 1, maxWeaponDmg: 1),
                    new (position: 179, @class: ClassType.Warrior, level: 296, strength: 10_310, dexterity: 3_619, intelligence: 3_575, constitution: 13_698, luck: 2_424, health: 20_341_530, minWeaponDmg: 1, maxWeaponDmg: 1),
                    new (position: 180, @class: ClassType.Warrior, level: 297, strength: 10_404, dexterity: 3_652, intelligence: 3_607, constitution: 13_825, luck: 2_444, health: 20_599_250, minWeaponDmg: 1, maxWeaponDmg: 1),
                    new (position: 181, @class: ClassType.Scout, level: 298, strength: 2_643, dexterity: 10_498, intelligence: 2_625, constitution: 10_491, luck: 3_803, health: 12_547_236, minWeaponDmg: 1, maxWeaponDmg: 1),
                    new (position: 182, @class: ClassType.Warrior, level: 298, strength: 10_556, dexterity: 3_706, intelligence: 3_660, constitution: 14_030, luck: 2_479, health: 20_974_850, minWeaponDmg: 1, maxWeaponDmg: 1),
                    new (position: 183, @class: ClassType.Scout, level: 298, strength: 2_672, dexterity: 10_614, intelligence: 2_654, constitution: 10_607, luck: 3_845, health: 12_685_972, minWeaponDmg: 1, maxWeaponDmg: 1),
                    new (position: 184, @class: ClassType.Scout, level: 299, strength: 2_696, dexterity: 10_709, intelligence: 2_677, constitution: 10_704, luck: 3_879, health: 12_844_800, minWeaponDmg: 1, maxWeaponDmg: 1),
                    new (position: 185, @class: ClassType.Warrior, level: 300, strength: 10_804, dexterity: 3_793, intelligence: 3_746, constitution: 14_365, luck: 2_535, health: 21_619_324, minWeaponDmg: 1, maxWeaponDmg: 1),
                    new (position: 186, @class: ClassType.Warrior, level: 300, strength: 10_862, dexterity: 3_813, intelligence: 3_767, constitution: 14_443, luck: 2_548, health: 21_736_716, minWeaponDmg: 1, maxWeaponDmg: 1),
                    new (position: 187, @class: ClassType.Warrior, level: 300, strength: 10_921, dexterity: 3_834, intelligence: 3_787, constitution: 14_520, luck: 2_562, health: 21_852_600, minWeaponDmg: 1, maxWeaponDmg: 1),
                    new (position: 188, @class: ClassType.Scout, level: 301, strength: 2_773, dexterity: 11_017, intelligence: 2_754, constitution: 11_015, luck: 3_989, health: 13_306_120, minWeaponDmg: 1, maxWeaponDmg: 1),
                    new (position: 189, @class: ClassType.Scout, level: 301, strength: 2_788, dexterity: 11_075, intelligence: 2_769, constitution: 11_073, luck: 4_011, health: 13_376_184, minWeaponDmg: 1, maxWeaponDmg: 1),
                    new (position: 190, @class: ClassType.Mage, level: 301, strength: 2_764, dexterity: 2_802, intelligence: 11_286, constitution: 10_973, luck: 4_022, health: 6_627_692, minWeaponDmg: 1, maxWeaponDmg: 1),
                    new (position: 191, @class: ClassType.Warrior, level: 302, strength: 11_231, dexterity: 3_942, intelligence: 3_894, constitution: 14_937, luck: 2_632, health: 22_629_556, minWeaponDmg: 1, maxWeaponDmg: 1),
                    new (position: 192, @class: ClassType.Warrior, level: 302, strength: 11_290, dexterity: 3_963, intelligence: 3_915, constitution: 15_015, luck: 2_646, health: 22_747_724, minWeaponDmg: 1, maxWeaponDmg: 1),
                    new (position: 193, @class: ClassType.Mage, level: 302, strength: 2_818, dexterity: 2_856, intelligence: 11_503, constitution: 11_187, luck: 4_099, health: 6_779_322, minWeaponDmg: 1, maxWeaponDmg: 1),
                    new (position: 194, @class: ClassType.Scout, level: 303, strength: 2_881, dexterity: 11_446, intelligence: 2_861, constitution: 11_448, luck: 4_144, health: 13_920_768, minWeaponDmg: 1, maxWeaponDmg: 1),
                    new (position: 195, @class: ClassType.Mage, level: 303, strength: 2_857, dexterity: 2_896, intelligence: 11_661, constitution: 11_343, luck: 4_155, health: 6_896_544, minWeaponDmg: 1, maxWeaponDmg: 1),
                    new (position: 196, @class: ClassType.Warrior, level: 303, strength: 11_564, dexterity: 4_059, intelligence: 4_010, constitution: 15_384, luck: 2_709, health: 23_383_680, minWeaponDmg: 1, maxWeaponDmg: 1),
                    new (position: 197, @class: ClassType.Mage, level: 304, strength: 2_896, dexterity: 2_935, intelligence: 11_820, constitution: 11_501, luck: 4_212, health: 7_015_610, minWeaponDmg: 1, maxWeaponDmg: 1),
                    new (position: 198, @class: ClassType.Warrior, level: 304, strength: 11_722, dexterity: 4_114, intelligence: 4_065, constitution: 15_595, luck: 2_744, health: 23_782_376, minWeaponDmg: 1, maxWeaponDmg: 1),
                    new (position: 199, @class: ClassType.Mage, level: 304, strength: 2_925, dexterity: 2_965, intelligence: 11_940, constitution: 11_617, luck: 4_255, health: 7_086_370, minWeaponDmg: 1, maxWeaponDmg: 1),
                    new (position: 200, @class: ClassType.Warrior, level: 305, strength: 11880, dexterity: 4170, intelligence: 4120, constitution: 15809, luck: 2780, health: 24_187_770, minWeaponDmg: 335, maxWeaponDmg: 671 ),

                    new (position: 201, @class: ClassType.Warrior, level: 305, strength: 11_939, dexterity: 4_191, intelligence: 4_141, constitution: 15_888, luck: 2_794, health: 24_308_640, minWeaponDmg: 1, maxWeaponDmg: 1),
                    new (position: 202, @class: ClassType.Mage, level: 305, strength: 2_980, dexterity: 3_020, intelligence: 12_160, constitution: 11_836, luck: 4_333, health: 7_243_632, minWeaponDmg: 1, maxWeaponDmg: 1),
                    new (position: 203, @class: ClassType.Mage, level: 306, strength: 3_004, dexterity: 3_045, intelligence: 12_261, constitution: 11_936, luck: 4_369, health: 7_328_704, minWeaponDmg: 1, maxWeaponDmg: 1),
                    new (position: 204, @class: ClassType.Warrior, level: 306, strength: 12_158, dexterity: 4_268, intelligence: 4_217, constitution: 16_181, luck: 2_844, health: 24_837_836, minWeaponDmg: 1, maxWeaponDmg: 1),
                    new (position: 205, @class: ClassType.Warrior, level: 306, strength: 12_218, dexterity: 4_289, intelligence: 4_237, constitution: 16_260, luck: 2_858, health: 24_959_100, minWeaponDmg: 1, maxWeaponDmg: 1),
                    new (position: 206, @class: ClassType.Warrior, level: 307, strength: 12_319, dexterity: 4_324, intelligence: 4_272, constitution: 16_398, luck: 2_880, health: 25_252_920, minWeaponDmg: 1, maxWeaponDmg: 1),
                    new (position: 207, @class: ClassType.Scout, level: 307, strength: 3_115, dexterity: 12_379, intelligence: 3_095, constitution: 12_389, luck: 4_479, health: 15_263_248, minWeaponDmg: 1, maxWeaponDmg: 1),
                    new (position: 208, @class: ClassType.Warrior, level: 307, strength: 12_438, dexterity: 4_366, intelligence: 4_314, constitution: 16_556, luck: 2_908, health: 25_496_240, minWeaponDmg: 1, maxWeaponDmg: 1),
                    new (position: 209, @class: ClassType.Warrior, level: 308, strength: 12_540, dexterity: 4_402, intelligence: 4_349, constitution: 16_696, luck: 2_930, health: 25_795_320, minWeaponDmg: 1, maxWeaponDmg: 1),
                    new (position: 210, @class: ClassType.Mage, level: 308, strength: 3_129, dexterity: 3_171, intelligence: 12_768, constitution: 12_436, luck: 4_549, health: 7_685_448, minWeaponDmg: 1, maxWeaponDmg: 1),
                    new (position: 211, @class: ClassType.Scout, level: 308, strength: 3_186, dexterity: 12_660, intelligence: 3_165, constitution: 12_672, luck: 4_581, health: 15_662_592, minWeaponDmg: 1, maxWeaponDmg: 1),
                    new (position: 212, @class: ClassType.Mage, level: 309, strength: 3_169, dexterity: 3_212, intelligence: 12_932, constitution: 12_599, luck: 4_607, health: 7_811_380, minWeaponDmg: 1, maxWeaponDmg: 1),
                    new (position: 213, @class: ClassType.Mage, level: 309, strength: 3_184, dexterity: 3_227, intelligence: 12_993, constitution: 12_659, luck: 4_628, health: 7_848_580, minWeaponDmg: 1, maxWeaponDmg: 1),
                    new (position: 214, @class: ClassType.Scout, level: 309, strength: 3_242, dexterity: 12_883, intelligence: 3_221, constitution: 12_898, luck: 4_661, health: 15_993_520, minWeaponDmg: 1, maxWeaponDmg: 1),
                    new (position: 215, @class: ClassType.Scout, level: 310, strength: 3_268, dexterity: 12_986, intelligence: 3_246, constitution: 13_003, luck: 4_698, health: 16_175_732, minWeaponDmg: 1, maxWeaponDmg: 1),
                    new (position: 216, @class: ClassType.Mage, level: 310, strength: 3_240, dexterity: 3_283, intelligence: 13_219, constitution: 12_882, luck: 4_709, health: 8_012_604, minWeaponDmg: 1, maxWeaponDmg: 1),
                    new (position: 217, @class: ClassType.Warrior, level: 310, strength: 13_107, dexterity: 4_600, intelligence: 4_546, constitution: 17_455, luck: 3_060, health: 27_142_524, minWeaponDmg: 1, maxWeaponDmg: 1),
                    new (position: 218, @class: ClassType.Warrior, level: 311, strength: 13_211, dexterity: 4_637, intelligence: 4_582, constitution: 17_596, luck: 3_083, health: 27_449_760, minWeaponDmg: 1, maxWeaponDmg: 1),
                    new (position: 219, @class: ClassType.Mage, level: 311, strength: 3_296, dexterity: 3_340, intelligence: 13_447, constitution: 13_107, luck: 4_790, health: 8_178_768, minWeaponDmg: 1, maxWeaponDmg: 1),
                    new (position: 220, @class: ClassType.Warrior, level: 311, strength: 13_332, dexterity: 4_679, intelligence: 4_624, constitution: 17_757, luck: 3_111, health: 27_700_920, minWeaponDmg: 1, maxWeaponDmg: 1),
                    new (position: 221, @class: ClassType.Scout, level: 312, strength: 3_381, dexterity: 13_437, intelligence: 3_359, constitution: 13_459, luck: 4_860, health: 16_850_668, minWeaponDmg: 1, maxWeaponDmg: 1),
                    new (position: 222, @class: ClassType.Mage, level: 312, strength: 3_352, dexterity: 3_397, intelligence: 13_675, constitution: 13_333, luck: 4_871, health: 8_346_458, minWeaponDmg: 1, maxWeaponDmg: 1),
                    new (position: 223, @class: ClassType.Warrior, level: 312, strength: 13_558, dexterity: 4_759, intelligence: 4_703, constitution: 18_062, luck: 3_162, health: 28_267_030, minWeaponDmg: 1, maxWeaponDmg: 1),
                    new (position: 224, @class: ClassType.Mage, level: 313, strength: 3_394, dexterity: 3_438, intelligence: 13_843, constitution: 13_501, luck: 4_930, health: 8_478_628, minWeaponDmg: 1, maxWeaponDmg: 1),
                    new (position: 225, @class: ClassType.Warrior, level: 313, strength: 13_725, dexterity: 4_817, intelligence: 4_761, constitution: 18_288, luck: 3_200, health: 28_712_160, minWeaponDmg: 1, maxWeaponDmg: 1),
                    new (position: 226, @class: ClassType.Warrior, level: 313, strength: 13_786, dexterity: 4_839, intelligence: 4_782, constitution: 18_370, luck: 3_214, health: 28_840_900, minWeaponDmg: 1, maxWeaponDmg: 1),
                    new (position: 227, @class: ClassType.Warrior, level: 314, strength: 13_892, dexterity: 4_876, intelligence: 4_819, constitution: 18_513, luck: 3_237, health: 29_157_976, minWeaponDmg: 1, maxWeaponDmg: 1),
                    new (position: 228, @class: ClassType.Warrior, level: 314, strength: 13_954, dexterity: 4_897, intelligence: 4_840, constitution: 18_594, luck: 3_251, health: 29_285_550, minWeaponDmg: 1, maxWeaponDmg: 1),
                    new (position: 229, @class: ClassType.Mage, level: 314, strength: 3_481, dexterity: 3_527, intelligence: 14_198, constitution: 13_850, luck: 5_056, health: 8_725_500, minWeaponDmg: 1, maxWeaponDmg: 1),
                    new (position: 230, @class: ClassType.Warrior, level: 315, strength: 14_122, dexterity: 4_956, intelligence: 4_899, constitution: 18_822, luck: 3_289, health: 29_738_760, minWeaponDmg: 1, maxWeaponDmg: 1),
                    new (position: 231, @class: ClassType.Warrior, level: 315, strength: 14_183, dexterity: 4_978, intelligence: 4_920, constitution: 18_904, luck: 3_303, health: 29_868_320, minWeaponDmg: 1, maxWeaponDmg: 1),
                    new (position: 232, @class: ClassType.Scout, level: 315, strength: 3_584, dexterity: 14_245, intelligence: 3_561, constitution: 14_275, luck: 5_150, health: 18_043_600, minWeaponDmg: 1, maxWeaponDmg: 1),
                    new (position: 233, @class: ClassType.Warrior, level: 316, strength: 14_353, dexterity: 5_037, intelligence: 4_979, constitution: 19_132, luck: 3_341, health: 30_324_220, minWeaponDmg: 1, maxWeaponDmg: 1),
                    new (position: 234, @class: ClassType.Mage, level: 316, strength: 3_580, dexterity: 3_627, intelligence: 14_602, constitution: 14_251, luck: 5_199, health: 9_035_134, minWeaponDmg: 1, maxWeaponDmg: 1),
                    new (position: 235, @class: ClassType.Scout, level: 316, strength: 3_643, dexterity: 14_476, intelligence: 3_619, constitution: 14_509, luck: 5_233, health: 18_397_412, minWeaponDmg: 1, maxWeaponDmg: 1),
                    new (position: 236, @class: ClassType.Warrior, level: 316, strength: 14_538, dexterity: 5_102, intelligence: 5_043, constitution: 19_378, luck: 3_384, health: 30_714_130, minWeaponDmg: 1, maxWeaponDmg: 1),
                    new (position: 237, @class: ClassType.Scout, level: 317, strength: 3_685, dexterity: 14_647, intelligence: 3_662, constitution: 14_682, luck: 5_295, health: 18_675_504, minWeaponDmg: 1, maxWeaponDmg: 1),
                    new (position: 238, @class: ClassType.Warrior, level: 317, strength: 14_708, dexterity: 5_162, intelligence: 5_103, constitution: 19_610, luck: 3_422, health: 31_179_900, minWeaponDmg: 1, maxWeaponDmg: 1),
                    new (position: 239, @class: ClassType.Mage, level: 317, strength: 3_669, dexterity: 3_716, intelligence: 14_961, constitution: 14_606, luck: 5_327, health: 9_289_416, minWeaponDmg: 1, maxWeaponDmg: 1),
                    new (position: 240, @class: ClassType.Scout, level: 318, strength: 3_744, dexterity: 14_880, intelligence: 3_720, constitution: 14_918, luck: 5_378, health: 19_035_368, minWeaponDmg: 1, maxWeaponDmg: 1),
                    new (position: 241, @class: ClassType.Scout, level: 318, strength: 3_760, dexterity: 14_942, intelligence: 3_736, constitution: 14_980, luck: 5_401, health: 19_114_480, minWeaponDmg: 1, maxWeaponDmg: 1),
                    new (position: 242, @class: ClassType.Scout, level: 318, strength: 3_775, dexterity: 15_004, intelligence: 3_751, constitution: 15_042, luck: 5_423, health: 19_193_592, minWeaponDmg: 1, maxWeaponDmg: 1),
                    new (position: 243, @class: ClassType.Scout, level: 319, strength: 3_803, dexterity: 15_115, intelligence: 3_779, constitution: 15_156, luck: 5_463, health: 19_399_680, minWeaponDmg: 1, maxWeaponDmg: 1),
                    new (position: 244, @class: ClassType.Mage, level: 319, strength: 3_770, dexterity: 3_819, intelligence: 15_372, constitution: 15_013, luck: 5_473, health: 9_608_320, minWeaponDmg: 1, maxWeaponDmg: 1),
                    new (position: 245, @class: ClassType.Warrior, level: 319, strength: 15_239, dexterity: 5_348, intelligence: 5_287, constitution: 20_323, luck: 3_543, health: 32_516_800, minWeaponDmg: 1, maxWeaponDmg: 1),
                    new (position: 246, @class: ClassType.Warrior, level: 320, strength: 15_350, dexterity: 5_387, intelligence: 5_326, constitution: 20_475, luck: 3_567, health: 32_862_376, minWeaponDmg: 1, maxWeaponDmg: 1),
                    new (position: 247, @class: ClassType.Mage, level: 320, strength: 3_829, dexterity: 3_878, intelligence: 15_610, constitution: 15_250, luck: 5_558, health: 9_790_500, minWeaponDmg: 1, maxWeaponDmg: 1),
                    new (position: 248, @class: ClassType.Scout, level: 320, strength: 3_894, dexterity: 15_475, intelligence: 3_869, constitution: 15_520, luck: 5_592, health: 19_927_680, minWeaponDmg: 1, maxWeaponDmg: 1),
                    new (position: 249, @class: ClassType.Warrior, level: 321, strength: 15_587, dexterity: 5_471, intelligence: 5_408, constitution: 20_793, luck: 3_620, health: 33_476_730, minWeaponDmg: 1, maxWeaponDmg: 1),
                    new (position: 250, @class: ClassType.Mage, level: 321, strength: 3888, dexterity: 3938, intelligence: 15850, constitution: 15488, luck: 5642, health: 9_974_272, minWeaponDmg: 794, maxWeaponDmg: 1589 ),

                    new (position: 251, @class: ClassType.Warrior, level: 321, strength: 15_713, dexterity: 5_514, intelligence: 5_452, constitution: 20_960, luck: 3_650, health: 33_745_600, minWeaponDmg: 1, maxWeaponDmg: 1),
                    new (position: 252, @class: ClassType.Scout, level: 322, strength: 3_982, dexterity: 15_826, intelligence: 3_956, constitution: 15_876, luck: 5_718, health: 20_511_792, minWeaponDmg: 1, maxWeaponDmg: 1),
                    new (position: 253, @class: ClassType.Scout, level: 322, strength: 3_997, dexterity: 15_888, intelligence: 3_972, constitution: 15_939, luck: 5_741, health: 20_593_188, minWeaponDmg: 1, maxWeaponDmg: 1),
                    new (position: 254, @class: ClassType.Scout, level: 322, strength: 4_013, dexterity: 15_951, intelligence: 3_988, constitution: 16_002, luck: 5_763, health: 20_674_584, minWeaponDmg: 1, maxWeaponDmg: 1),
                    new (position: 255, @class: ClassType.Warrior, level: 323, strength: 16_065, dexterity: 5_638, intelligence: 5_574, constitution: 21_439, luck: 3_728, health: 34_731_180, minWeaponDmg: 1, maxWeaponDmg: 1),
                    new (position: 256, @class: ClassType.Scout, level: 323, strength: 4_058, dexterity: 16_128, intelligence: 4_032, constitution: 16_182, luck: 5_827, health: 20_971_872, minWeaponDmg: 1, maxWeaponDmg: 1),
                    new (position: 257, @class: ClassType.Warrior, level: 323, strength: 16_191, dexterity: 5_682, intelligence: 5_618, constitution: 21_607, luck: 3_757, health: 35_003_340, minWeaponDmg: 1, maxWeaponDmg: 1),
                    new (position: 258, @class: ClassType.Scout, level: 324, strength: 4_102, dexterity: 16_306, intelligence: 4_076, constitution: 16_362, luck: 5_890, health: 21_270_600, minWeaponDmg: 1, maxWeaponDmg: 1),
                    new (position: 259, @class: ClassType.Mage, level: 324, strength: 4_066, dexterity: 4_118, intelligence: 16_576, constitution: 16_208, luck: 5_900, health: 10_535_200, minWeaponDmg: 1, maxWeaponDmg: 1),
                    new (position: 260, @class: ClassType.Warrior, level: 324, strength: 16_432, dexterity: 5_767, intelligence: 5_702, constitution: 21_930, luck: 3_812, health: 35_636_248, minWeaponDmg: 1, maxWeaponDmg: 1),
                    new (position: 261, @class: ClassType.Warrior, level: 325, strength: 16_547, dexterity: 5_807, intelligence: 5_742, constitution: 22_089, luck: 3_837, health: 36_005_072, minWeaponDmg: 1, maxWeaponDmg: 1),
                    new (position: 262, @class: ClassType.Warrior, level: 325, strength: 16_611, dexterity: 5_830, intelligence: 5_764, constitution: 22_173, luck: 3_851, health: 36_141_992, minWeaponDmg: 1, maxWeaponDmg: 1),
                    new (position: 263, @class: ClassType.Scout, level: 325, strength: 4_195, dexterity: 16_674, intelligence: 4_169, constitution: 16_735, luck: 6_023, health: 21_822_440, minWeaponDmg: 1, maxWeaponDmg: 1),
                    new (position: 264, @class: ClassType.Warrior, level: 326, strength: 16_790, dexterity: 5_892, intelligence: 5_826, constitution: 22_414, luck: 3_891, health: 36_646_888, minWeaponDmg: 1, maxWeaponDmg: 1),
                    new (position: 265, @class: ClassType.Scout, level: 326, strength: 4_240, dexterity: 16_854, intelligence: 4_213, constitution: 16_918, luck: 6_087, health: 22_128_744, minWeaponDmg: 1, maxWeaponDmg: 1),
                    new (position: 266, @class: ClassType.Warrior, level: 326, strength: 16_918, dexterity: 5_937, intelligence: 5_871, constitution: 22_584, luck: 3_921, health: 36_924_840, minWeaponDmg: 1, maxWeaponDmg: 1),
                    new (position: 267, @class: ClassType.Warrior, level: 327, strength: 17_035, dexterity: 5_978, intelligence: 5_911, constitution: 22_745, luck: 3_946, health: 37_301_800, minWeaponDmg: 1, maxWeaponDmg: 1),
                    new (position: 268, @class: ClassType.Warrior, level: 327, strength: 17_098, dexterity: 6_001, intelligence: 5_934, constitution: 22_830, luck: 3_961, health: 37_441_200, minWeaponDmg: 1, maxWeaponDmg: 1),
                    new (position: 269, @class: ClassType.Scout, level: 327, strength: 4_317, dexterity: 17_162, intelligence: 4_291, constitution: 17_229, luck: 6_198, health: 22_604_448, minWeaponDmg: 1, maxWeaponDmg: 1),
                    new (position: 270, @class: ClassType.Warrior, level: 328, strength: 17_280, dexterity: 6_064, intelligence: 5_997, constitution: 23_077, luck: 4_001, health: 37_961_664, minWeaponDmg: 1, maxWeaponDmg: 1),
                    new (position: 271, @class: ClassType.Warrior, level: 328, strength: 17_344, dexterity: 6_087, intelligence: 6_019, constitution: 23_162, luck: 4_016, health: 38_101_488, minWeaponDmg: 1, maxWeaponDmg: 1),
                    new (position: 272, @class: ClassType.Warrior, level: 328, strength: 17_408, dexterity: 6_109, intelligence: 6_041, constitution: 23_248, luck: 4_031, health: 38_242_960, minWeaponDmg: 1, maxWeaponDmg: 1),
                    new (position: 273, @class: ClassType.Scout, level: 329, strength: 4_409, dexterity: 17_527, intelligence: 4_382, constitution: 17_600, luck: 6_328, health: 23_232_000, minWeaponDmg: 1, maxWeaponDmg: 1),
                    new (position: 274, @class: ClassType.Scout, level: 329, strength: 4_425, dexterity: 17_591, intelligence: 4_398, constitution: 17_665, luck: 6_351, health: 23_317_800, minWeaponDmg: 1, maxWeaponDmg: 1),
                    new (position: 275, @class: ClassType.Warrior, level: 329, strength: 17_655, dexterity: 6_196, intelligence: 6_127, constitution: 23_579, luck: 4_086, health: 38_905_352, minWeaponDmg: 1, maxWeaponDmg: 1),
                    new (position: 276, @class: ClassType.Mage, level: 330, strength: 4_416, dexterity: 4_471, intelligence: 17_995, constitution: 17_620, luck: 6_403, health: 11_664_440, minWeaponDmg: 1, maxWeaponDmg: 1),
                    new (position: 277, @class: ClassType.Mage, level: 330, strength: 4_432, dexterity: 4_487, intelligence: 18_060, constitution: 17_684, luck: 6_426, health: 11_706_808, minWeaponDmg: 1, maxWeaponDmg: 1),
                    new (position: 278, @class: ClassType.Scout, level: 330, strength: 4_504, dexterity: 17_903, intelligence: 4_476, constitution: 17_981, luck: 6_463, health: 23_806_844, minWeaponDmg: 1, maxWeaponDmg: 1),
                    new (position: 279, @class: ClassType.Scout, level: 331, strength: 4_534, dexterity: 18_023, intelligence: 4_506, constitution: 18_104, luck: 6_506, health: 24_042_112, minWeaponDmg: 1, maxWeaponDmg: 1),
                    new (position: 280, @class: ClassType.Scout, level: 331, strength: 4_550, dexterity: 18_088, intelligence: 4_522, constitution: 18_169, luck: 6_530, health: 24_128_432, minWeaponDmg: 1, maxWeaponDmg: 1),
                    new (position: 281, @class: ClassType.Warrior, level: 331, strength: 18_153, dexterity: 6_370, intelligence: 6_300, constitution: 24_250, luck: 4_198, health: 40_255_000, minWeaponDmg: 1, maxWeaponDmg: 1),
                    new (position: 282, @class: ClassType.Warrior, level: 332, strength: 18_274, dexterity: 6_413, intelligence: 6_342, constitution: 24_417, luck: 4_224, health: 40_654_304, minWeaponDmg: 1, maxWeaponDmg: 1),
                    new (position: 283, @class: ClassType.Mage, level: 332, strength: 4_556, dexterity: 4_613, intelligence: 18_565, constitution: 18_186, luck: 6_605, health: 12_111_876, minWeaponDmg: 1, maxWeaponDmg: 1),
                    new (position: 284, @class: ClassType.Mage, level: 332, strength: 4_572, dexterity: 4_629, intelligence: 18_630, constitution: 18_250, luck: 6_629, health: 12_154_500, minWeaponDmg: 1, maxWeaponDmg: 1),
                    new (position: 285, @class: ClassType.Mage, level: 333, strength: 4_603, dexterity: 4_660, intelligence: 18_753, constitution: 18_374, luck: 6_672, health: 12_273_832, minWeaponDmg: 1, maxWeaponDmg: 1),
                    new (position: 286, @class: ClassType.Warrior, level: 333, strength: 18_590, dexterity: 6_524, intelligence: 6_452, constitution: 24_844, luck: 4_296, health: 41_489_480, minWeaponDmg: 1, maxWeaponDmg: 1),
                    new (position: 287, @class: ClassType.Warrior, level: 333, strength: 18_655, dexterity: 6_546, intelligence: 6_475, constitution: 24_931, luck: 4_311, health: 41_634_768, minWeaponDmg: 1, maxWeaponDmg: 1),
                    new (position: 288, @class: ClassType.Scout, level: 334, strength: 4_723, dexterity: 18_778, intelligence: 4_694, constitution: 18_870, luck: 6_777, health: 25_285_800, minWeaponDmg: 1, maxWeaponDmg: 1),
                    new (position: 289, @class: ClassType.Warrior, level: 334, strength: 18_843, dexterity: 6_612, intelligence: 6_540, constitution: 25_183, luck: 4_352, health: 42_181_524, minWeaponDmg: 1, maxWeaponDmg: 1),
                    new (position: 290, @class: ClassType.Warrior, level: 334, strength: 18_908, dexterity: 6_635, intelligence: 6_563, constitution: 25_270, luck: 4_367, health: 42_327_248, minWeaponDmg: 1, maxWeaponDmg: 1),
                    new (position: 291, @class: ClassType.Mage, level: 335, strength: 4_729, dexterity: 4_787, intelligence: 19_264, constitution: 18_883, luck: 6_853, health: 12_689_376, minWeaponDmg: 1, maxWeaponDmg: 1),
                    new (position: 292, @class: ClassType.Mage, level: 335, strength: 4_745, dexterity: 4_803, intelligence: 19_330, constitution: 18_948, luck: 6_877, health: 12_733_056, minWeaponDmg: 1, maxWeaponDmg: 1),
                    new (position: 293, @class: ClassType.Mage, level: 335, strength: 4_761, dexterity: 4_820, intelligence: 19_397, constitution: 19_012, luck: 6_900, health: 12_776_064, minWeaponDmg: 1, maxWeaponDmg: 1),
                    new (position: 294, @class: ClassType.Scout, level: 336, strength: 4_851, dexterity: 19_286, intelligence: 4_822, constitution: 19_386, luck: 6_959, health: 26_132_328, minWeaponDmg: 1, maxWeaponDmg: 1),
                    new (position: 295, @class: ClassType.Mage, level: 336, strength: 4_809, dexterity: 4_868, intelligence: 19_588, constitution: 19_205, luck: 6_968, health: 12_944_170, minWeaponDmg: 1, maxWeaponDmg: 1),
                    new (position: 296, @class: ClassType.Mage, level: 336, strength: 4_825, dexterity: 4_884, intelligence: 19_654, constitution: 19_270, luck: 6_992, health: 12_987_980, minWeaponDmg: 1, maxWeaponDmg: 1),
                    new (position: 297, @class: ClassType.Warrior, level: 337, strength: 19_543, dexterity: 6_858, intelligence: 6_783, constitution: 26_130, luck: 4_508, health: 44_159_700, minWeaponDmg: 1, maxWeaponDmg: 1),
                    new (position: 298, @class: ClassType.Scout, level: 337, strength: 4_932, dexterity: 19_608, intelligence: 4_902, constitution: 19_713, luck: 7_075, health: 26_651_976, minWeaponDmg: 1, maxWeaponDmg: 1),
                    new (position: 299, @class: ClassType.Scout, level: 337, strength: 4_948, dexterity: 19_674, intelligence: 4_919, constitution: 19_779, luck: 7_098, health: 26_741_208, minWeaponDmg: 1, maxWeaponDmg: 1),
                    new (position: 300, @class: ClassType.Warrior, level: 338, strength: 19800, dexterity: 6948, intelligence: 6873, constitution: 26479, luck: 4566, health: 44_881_904, minWeaponDmg: 372, maxWeaponDmg: 744 ),

                    new (position: 301, @class: ClassType.Scout, level: 338, strength: 4_997, dexterity: 19_866, intelligence: 4_966, constitution: 19_974, luck: 7_167, health: 27_084_744, minWeaponDmg: 1, maxWeaponDmg: 1),
                    new (position: 302, @class: ClassType.Mage, level: 338, strength: 4_953, dexterity: 5_013, intelligence: 20_174, constitution: 19_787, luck: 7_176, health: 13_415_586, minWeaponDmg: 1, maxWeaponDmg: 1),
                    new (position: 303, @class: ClassType.Warrior, level: 339, strength: 20_059, dexterity: 7_039, intelligence: 6_963, constitution: 26_826, luck: 4_624, health: 45_604_200, minWeaponDmg: 1, maxWeaponDmg: 1),
                    new (position: 304, @class: ClassType.Scout, level: 339, strength: 5_062, dexterity: 20_125, intelligence: 5_031, constitution: 20_238, luck: 7_260, health: 27_523_680, minWeaponDmg: 1, maxWeaponDmg: 1),
                    new (position: 305, @class: ClassType.Mage, level: 339, strength: 5_017, dexterity: 5_078, intelligence: 20_435, constitution: 20_048, luck: 7_268, health: 13_632_640, minWeaponDmg: 1, maxWeaponDmg: 1),
                    new (position: 306, @class: ClassType.Mage, level: 340, strength: 5_049, dexterity: 5_110, intelligence: 20_563, constitution: 20_178, luck: 7_313, health: 13_761_396, minWeaponDmg: 1, maxWeaponDmg: 1),
                    new (position: 307, @class: ClassType.Warrior, level: 340, strength: 20_385, dexterity: 7_153, intelligence: 7_076, constitution: 27_267, luck: 4_697, health: 46_490_236, minWeaponDmg: 1, maxWeaponDmg: 1),
                    new (position: 308, @class: ClassType.Warrior, level: 340, strength: 20_451, dexterity: 7_176, intelligence: 7_099, constitution: 27_357, luck: 4_712, health: 46_643_684, minWeaponDmg: 1, maxWeaponDmg: 1),
                    new (position: 309, @class: ClassType.Scout, level: 341, strength: 5_176, dexterity: 20_579, intelligence: 5_145, constitution: 20_700, luck: 7_422, health: 28_317_600, minWeaponDmg: 1, maxWeaponDmg: 1),
                    new (position: 310, @class: ClassType.Warrior, level: 341, strength: 20_646, dexterity: 7_245, intelligence: 7_167, constitution: 27_619, luck: 4_755, health: 47_228_488, minWeaponDmg: 1, maxWeaponDmg: 1),
                    new (position: 311, @class: ClassType.Scout, level: 341, strength: 5_209, dexterity: 20_713, intelligence: 5_178, constitution: 20_834, luck: 7_470, health: 28_500_912, minWeaponDmg: 1, maxWeaponDmg: 1),
                    new (position: 312, @class: ClassType.Scout, level: 341, strength: 5_226, dexterity: 20_779, intelligence: 5_195, constitution: 20_901, luck: 7_494, health: 28_592_568, minWeaponDmg: 1, maxWeaponDmg: 1),
                    new (position: 313, @class: ClassType.Warrior, level: 342, strength: 20_908, dexterity: 7_337, intelligence: 7_258, constitution: 27_975, luck: 4_814, health: 47_977_124, minWeaponDmg: 1, maxWeaponDmg: 1),
                    new (position: 314, @class: ClassType.Warrior, level: 342, strength: 20_975, dexterity: 7_360, intelligence: 7_282, constitution: 28_064, luck: 4_829, health: 48_129_760, minWeaponDmg: 1, maxWeaponDmg: 1),
                    new (position: 315, @class: ClassType.Scout, level: 342, strength: 5_292, dexterity: 21_042, intelligence: 5_260, constitution: 21_168, luck: 7_588, health: 29_042_496, minWeaponDmg: 1, maxWeaponDmg: 1),
                    new (position: 316, @class: ClassType.Warrior, level: 342, strength: 21_109, dexterity: 7_407, intelligence: 7_328, constitution: 28_243, luck: 4_860, health: 48_436_744, minWeaponDmg: 1, maxWeaponDmg: 1),
                    new (position: 317, @class: ClassType.Warrior, level: 343, strength: 21_239, dexterity: 7_453, intelligence: 7_373, constitution: 28_422, luck: 4_888, health: 48_885_840, minWeaponDmg: 1, maxWeaponDmg: 1),
                    new (position: 318, @class: ClassType.Warrior, level: 343, strength: 21_306, dexterity: 7_476, intelligence: 7_397, constitution: 28_512, luck: 4_904, health: 49_040_640, minWeaponDmg: 1, maxWeaponDmg: 1),
                    new (position: 319, @class: ClassType.Scout, level: 343, strength: 5_375, dexterity: 21_373, intelligence: 5_343, constitution: 21_504, luck: 7_707, health: 29_589_504, minWeaponDmg: 1, maxWeaponDmg: 1),
                    new (position: 320, @class: ClassType.Mage, level: 343, strength: 5_328, dexterity: 5_392, intelligence: 21_696, constitution: 21_302, luck: 7_715, health: 14_655_776, minWeaponDmg: 1, maxWeaponDmg: 1),
                    new (position: 321, @class: ClassType.Mage, level: 344, strength: 5_361, dexterity: 5_425, intelligence: 21_828, constitution: 21_437, luck: 7_762, health: 14_791_530, minWeaponDmg: 1, maxWeaponDmg: 1),
                    new (position: 322, @class: ClassType.Scout, level: 344, strength: 5_442, dexterity: 21_638, intelligence: 5_410, constitution: 21_774, luck: 7_802, health: 30_048_120, minWeaponDmg: 1, maxWeaponDmg: 1),
                    new (position: 323, @class: ClassType.Mage, level: 344, strength: 5_394, dexterity: 5_459, intelligence: 21_964, constitution: 21_570, luck: 7_810, health: 14_883_300, minWeaponDmg: 1, maxWeaponDmg: 1),
                    new (position: 324, @class: ClassType.Scout, level: 344, strength: 5_476, dexterity: 21_773, intelligence: 5_443, constitution: 21_909, luck: 7_851, health: 30_234_420, minWeaponDmg: 1, maxWeaponDmg: 1),
                    new (position: 325, @class: ClassType.Mage, level: 345, strength: 5_444, dexterity: 5_509, intelligence: 22_165, constitution: 21_772, luck: 7_881, health: 15_066_224, minWeaponDmg: 1, maxWeaponDmg: 1),
                    new (position: 326, @class: ClassType.Warrior, level: 345, strength: 21_972, dexterity: 7_710, intelligence: 7_628, constitution: 29_411, luck: 5_053, health: 50_881_032, minWeaponDmg: 1, maxWeaponDmg: 1),
                    new (position: 327, @class: ClassType.Mage, level: 345, strength: 5_477, dexterity: 5_543, intelligence: 22_301, constitution: 21_906, luck: 7_930, health: 15_158_952, minWeaponDmg: 1, maxWeaponDmg: 1),
                    new (position: 328, @class: ClassType.Warrior, level: 345, strength: 22_107, dexterity: 7_757, intelligence: 7_675, constitution: 29_591, luck: 5_084, health: 51_192_432, minWeaponDmg: 1, maxWeaponDmg: 1),
                    new (position: 329, @class: ClassType.Mage, level: 346, strength: 5_527, dexterity: 5_593, intelligence: 22_504, constitution: 22_109, luck: 8_001, health: 15_343_646, minWeaponDmg: 1, maxWeaponDmg: 1),
                    new (position: 330, @class: ClassType.Scout, level: 346, strength: 5_610, dexterity: 22_308, intelligence: 5_577, constitution: 22_453, luck: 8_042, health: 31_164_764, minWeaponDmg: 1, maxWeaponDmg: 1),
                    new (position: 331, @class: ClassType.Warrior, level: 346, strength: 22_376, dexterity: 7_851, intelligence: 7_769, constitution: 29_952, luck: 5_144, health: 51_966_720, minWeaponDmg: 1, maxWeaponDmg: 1),
                    new (position: 332, @class: ClassType.Warrior, level: 346, strength: 22_443, dexterity: 7_875, intelligence: 7_792, constitution: 30_043, luck: 5_159, health: 52_124_604, minWeaponDmg: 1, maxWeaponDmg: 1),
                    new (position: 333, @class: ClassType.Warrior, level: 347, strength: 22_577, dexterity: 7_922, intelligence: 7_839, constitution: 30_227, luck: 5_188, health: 52_594_980, minWeaponDmg: 1, maxWeaponDmg: 1),
                    new (position: 334, @class: ClassType.Scout, level: 347, strength: 5_695, dexterity: 22_645, intelligence: 5_661, constitution: 22_796, luck: 8_163, health: 31_732_032, minWeaponDmg: 1, maxWeaponDmg: 1),
                    new (position: 335, @class: ClassType.Mage, level: 347, strength: 5_645, dexterity: 5_712, intelligence: 22_981, constitution: 22_582, luck: 8_171, health: 15_717_072, minWeaponDmg: 1, maxWeaponDmg: 1),
                    new (position: 336, @class: ClassType.Scout, level: 347, strength: 5_729, dexterity: 22_781, intelligence: 5_695, constitution: 22_932, luck: 8_212, health: 31_921_344, minWeaponDmg: 1, maxWeaponDmg: 1),
                    new (position: 337, @class: ClassType.Warrior, level: 348, strength: 22_916, dexterity: 8_041, intelligence: 7_957, constitution: 30_686, luck: 5_264, health: 53_547_072, minWeaponDmg: 1, maxWeaponDmg: 1),
                    new (position: 338, @class: ClassType.Scout, level: 348, strength: 5_780, dexterity: 22_984, intelligence: 5_746, constitution: 23_140, luck: 8_284, health: 32_303_440, minWeaponDmg: 1, maxWeaponDmg: 1),
                    new (position: 339, @class: ClassType.Scout, level: 348, strength: 5_797, dexterity: 23_052, intelligence: 5_763, constitution: 23_208, luck: 8_309, health: 32_398_368, minWeaponDmg: 1, maxWeaponDmg: 1),
                    new (position: 340, @class: ClassType.Mage, level: 348, strength: 5_746, dexterity: 5_814, intelligence: 23_392, constitution: 22_991, luck: 8_316, health: 16_047_718, minWeaponDmg: 1, maxWeaponDmg: 1),
                    new (position: 341, @class: ClassType.Warrior, level: 349, strength: 23_256, dexterity: 8_160, intelligence: 8_075, constitution: 31_143, luck: 5_340, health: 54_500_248, minWeaponDmg: 1, maxWeaponDmg: 1),
                    new (position: 342, @class: ClassType.Scout, level: 349, strength: 5_865, dexterity: 23_324, intelligence: 5_831, constitution: 23_485, luck: 8_406, health: 32_879_000, minWeaponDmg: 1, maxWeaponDmg: 1),
                    new (position: 343, @class: ClassType.Warrior, level: 349, strength: 23_393, dexterity: 8_208, intelligence: 8_122, constitution: 31_326, luck: 5_371, health: 54_820_500, minWeaponDmg: 1, maxWeaponDmg: 1),
                    new (position: 344, @class: ClassType.Mage, level: 349, strength: 5_831, dexterity: 5_900, intelligence: 23_736, constitution: 23_333, luck: 8_438, health: 16_333_100, minWeaponDmg: 1, maxWeaponDmg: 1),
                    new (position: 345, @class: ClassType.Scout, level: 350, strength: 5_934, dexterity: 23_598, intelligence: 5_900, constitution: 23_764, luck: 8_504, health: 33_364_656, minWeaponDmg: 1, maxWeaponDmg: 1),
                    new (position: 346, @class: ClassType.Warrior, level: 350, strength: 23_666, dexterity: 8_304, intelligence: 8_217, constitution: 31_697, luck: 5_432, health: 55_628_236, minWeaponDmg: 1, maxWeaponDmg: 1),
                    new (position: 347, @class: ClassType.Mage, level: 350, strength: 5_899, dexterity: 5_968, intelligence: 24_012, constitution: 23_610, luck: 8_536, health: 16_574_220, minWeaponDmg: 1, maxWeaponDmg: 1),
                    new (position: 348, @class: ClassType.Warrior, level: 350, strength: 23_803, dexterity: 8_352, intelligence: 8_265, constitution: 31_881, luck: 5_464, health: 55_951_156, minWeaponDmg: 1, maxWeaponDmg: 1),
                    new (position: 349, @class: ClassType.Scout, level: 351, strength: 6_373, dexterity: 25_351, intelligence: 6_338, constitution: 25_593, luck: 9_119, health: 36_034_944, minWeaponDmg: 1, maxWeaponDmg: 1),
                    new (position: 350, @class: ClassType.Mage, level: 351, strength: 6321, dexterity: 6391, intelligence: 25704, constitution: 25372, luck: 9128, health: 17_861_888, minWeaponDmg: 868, maxWeaponDmg: 1737 ),

                    new (position: 351, @class: ClassType.Warrior, level: 351, strength: 25_497, dexterity: 8_943, intelligence: 8_856, constitution: 34_234, luck: 5_809, health: 60_251_840, minWeaponDmg: 1, maxWeaponDmg: 1),
                    new (position: 352, @class: ClassType.Warrior, level: 351, strength: 25_569, dexterity: 8_969, intelligence: 8_881, constitution: 34_332, luck: 5_826, health: 60_424_320, minWeaponDmg: 1, maxWeaponDmg: 1),
                    new (position: 353, @class: ClassType.Scout, level: 352, strength: 6_467, dexterity: 25_727, intelligence: 6_432, constitution: 25_975, luck: 9_256, health: 36_676_700, minWeaponDmg: 1, maxWeaponDmg: 1),
                    new (position: 354, @class: ClassType.Warrior, level: 352, strength: 25_800, dexterity: 9_052, intelligence: 8_963, constitution: 34_646, luck: 5_876, health: 61_150_192, minWeaponDmg: 1, maxWeaponDmg: 1),
                    new (position: 355, @class: ClassType.Warrior, level: 352, strength: 25_872, dexterity: 9_077, intelligence: 8_989, constitution: 34_745, luck: 5_893, health: 61_324_924, minWeaponDmg: 1, maxWeaponDmg: 1),
                    new (position: 356, @class: ClassType.Scout, level: 352, strength: 6_522, dexterity: 25_945, intelligence: 6_486, constitution: 26_195, luck: 9_334, health: 36_987_340, minWeaponDmg: 1, maxWeaponDmg: 1),
                    new (position: 357, @class: ClassType.Mage, level: 353, strength: 6_490, dexterity: 6_562, intelligence: 26_389, constitution: 26_060, luck: 9_371, health: 18_450_480, minWeaponDmg: 1, maxWeaponDmg: 1),
                    new (position: 358, @class: ClassType.Warrior, level: 353, strength: 26_177, dexterity: 9_183, intelligence: 9_093, constitution: 35_158, luck: 5_957, health: 62_229_660, minWeaponDmg: 1, maxWeaponDmg: 1),
                    new (position: 359, @class: ClassType.Scout, level: 353, strength: 6_598, dexterity: 26_250, intelligence: 6_563, constitution: 26_507, luck: 9_442, health: 37_533_912, minWeaponDmg: 1, maxWeaponDmg: 1),
                    new (position: 360, @class: ClassType.Warrior, level: 353, strength: 26_323, dexterity: 9_234, intelligence: 9_144, constitution: 35_355, luck: 5_990, health: 62_578_352, minWeaponDmg: 1, maxWeaponDmg: 1),
                    new (position: 361, @class: ClassType.Warrior, level: 354, strength: 26_483, dexterity: 9_292, intelligence: 9_202, constitution: 35_570, luck: 6_025, health: 63_136_752, minWeaponDmg: 1, maxWeaponDmg: 1),
                    new (position: 362, @class: ClassType.Mage, level: 354, strength: 6_603, dexterity: 6_675, intelligence: 26_846, constitution: 26_516, luck: 9_535, health: 18_826_360, minWeaponDmg: 1, maxWeaponDmg: 1),
                    new (position: 363, @class: ClassType.Warrior, level: 354, strength: 26_630, dexterity: 9_344, intelligence: 9_253, constitution: 35_767, luck: 6_058, health: 63_486_424, minWeaponDmg: 1, maxWeaponDmg: 1),
                    new (position: 364, @class: ClassType.Warrior, level: 354, strength: 26_703, dexterity: 9_369, intelligence: 9_278, constitution: 35_866, luck: 6_075, health: 63_662_152, minWeaponDmg: 1, maxWeaponDmg: 1),
                    new (position: 365, @class: ClassType.Mage, level: 355, strength: 6_869, dexterity: 6_942, intelligence: 27_922, constitution: 27_632, luck: 9_910, health: 19_673_984, minWeaponDmg: 1, maxWeaponDmg: 1),
                    new (position: 366, @class: ClassType.Mage, level: 355, strength: 6_888, dexterity: 6_961, intelligence: 27_999, constitution: 27_708, luck: 9_937, health: 19_728_096, minWeaponDmg: 1, maxWeaponDmg: 1),
                    new (position: 367, @class: ClassType.Scout, level: 355, strength: 6_980, dexterity: 27_782, intelligence: 6_944, constitution: 28_092, luck: 9_982, health: 40_003_008, minWeaponDmg: 1, maxWeaponDmg: 1),
                    new (position: 368, @class: ClassType.Mage, level: 355, strength: 6_926, dexterity: 6_999, intelligence: 28_152, constitution: 27_860, luck: 9_991, health: 19_836_320, minWeaponDmg: 1, maxWeaponDmg: 1),
                    new (position: 369, @class: ClassType.Scout, level: 356, strength: 6_849, dexterity: 27_247, intelligence: 6_812, constitution: 27_525, luck: 9_797, health: 39_305_700, minWeaponDmg: 1, maxWeaponDmg: 1),
                    new (position: 370, @class: ClassType.Warrior, level: 356, strength: 27_321, dexterity: 9_583, intelligence: 9_491, constitution: 36_706, luck: 6_212, health: 65_520_208, minWeaponDmg: 1, maxWeaponDmg: 1),
                    new (position: 371, @class: ClassType.Warrior, level: 356, strength: 27_395, dexterity: 9_609, intelligence: 9_516, constitution: 36_805, luck: 6_229, health: 65_696_924, minWeaponDmg: 1, maxWeaponDmg: 1),
                    new (position: 372, @class: ClassType.Mage, level: 356, strength: 6_830, dexterity: 6_904, intelligence: 27_766, constitution: 27_435, luck: 9_858, health: 19_588_590, minWeaponDmg: 1, maxWeaponDmg: 1),
                    new (position: 373, @class: ClassType.Warrior, level: 357, strength: 27_632, dexterity: 9_694, intelligence: 9_601, constitution: 37_128, luck: 6_281, health: 66_459_120, minWeaponDmg: 1, maxWeaponDmg: 1),
                    new (position: 374, @class: ClassType.Warrior, level: 357, strength: 27_706, dexterity: 9_720, intelligence: 9_627, constitution: 37_228, luck: 6_298, health: 66_638_120, minWeaponDmg: 1, maxWeaponDmg: 1),
                    new (position: 375, @class: ClassType.Warrior, level: 357, strength: 27_780, dexterity: 9_746, intelligence: 9_652, constitution: 37_328, luck: 6_315, health: 66_817, minWeaponDmg: 1, maxWeaponDmg: 1),
                    new (position: 376, @class: ClassType.Scout, level: 357, strength: 7_001, dexterity: 27_854, intelligence: 6_964, constitution: 28_141, luck: 10_017, health: 40_297_912, minWeaponDmg: 1, maxWeaponDmg: 1),
                    new (position: 377, @class: ClassType.Scout, level: 358, strength: 7_042, dexterity: 28_019, intelligence: 7_005, constitution: 28_311, luck: 10_073, health: 40_654_596, minWeaponDmg: 1, maxWeaponDmg: 1),
                    new (position: 378, @class: ClassType.Mage, level: 358, strength: 6_985, dexterity: 7_061, intelligence: 28_395, constitution: 28_069, luck: 10_081, health: 20_153_542, minWeaponDmg: 1, maxWeaponDmg: 1),
                    new (position: 379, @class: ClassType.Warrior, level: 358, strength: 28_167, dexterity: 9_881, intelligence: 9_786, constitution: 37_853, luck: 6_398, health: 67_946_136, minWeaponDmg: 1, maxWeaponDmg: 1),
                    new (position: 380, @class: ClassType.Warrior, level: 358, strength: 28_242, dexterity: 9_907, intelligence: 9_812, constitution: 37_953, luck: 6_414, health: 68_125_632, minWeaponDmg: 1, maxWeaponDmg: 1),
                    new (position: 381, @class: ClassType.Warrior, level: 359, strength: 28_407, dexterity: 9_967, intelligence: 9_872, constitution: 38_181, luck: 6_450, health: 68_725_800, minWeaponDmg: 1, maxWeaponDmg: 1),
                    new (position: 382, @class: ClassType.Warrior, level: 359, strength: 28_482, dexterity: 9_993, intelligence: 9_898, constitution: 38_281, luck: 6_467, health: 68_905_800, minWeaponDmg: 1, maxWeaponDmg: 1),
                    new (position: 383, @class: ClassType.Warrior, level: 359, strength: 28_556, dexterity: 10_019, intelligence: 9_924, constitution: 38_382, luck: 6_486, health: 69_087_600, minWeaponDmg: 1, maxWeaponDmg: 1),
                    new (position: 384, @class: ClassType.Warrior, level: 359, strength: 28_631, dexterity: 10_045, intelligence: 9_949, constitution: 38_481, luck: 6_501, health: 69_265_800, minWeaponDmg: 1, maxWeaponDmg: 1),
                    new (position: 385, @class: ClassType.Warrior, level: 360, strength: 30_492, dexterity: 10_695, intelligence: 10_599, constitution: 41_076, luck: 6_876, health: 74_142_176, minWeaponDmg: 1, maxWeaponDmg: 1),
                    new (position: 386, @class: ClassType.Warrior, level: 360, strength: 30_571, dexterity: 10_723, intelligence: 10_627, constitution: 41_182, luck: 6_894, health: 74_333_512, minWeaponDmg: 1, maxWeaponDmg: 1),
                    new (position: 387, @class: ClassType.Warrior, level: 360, strength: 30_650, dexterity: 10_751, intelligence: 10_654, constitution: 41_289, luck: 6_912, health: 74_526_648, minWeaponDmg: 1, maxWeaponDmg: 1),
                    new (position: 388, @class: ClassType.Warrior, level: 360, strength: 30_730, dexterity: 10_779, intelligence: 10_683, constitution: 41_396, luck: 6_930, health: 74_719_776, minWeaponDmg: 1, maxWeaponDmg: 1),
                    new (position: 389, @class: ClassType.Warrior, level: 361, strength: 29_191, dexterity: 10_238, intelligence: 10_141, constitution: 39_244, luck: 6_625, health: 71_031_640, minWeaponDmg: 1, maxWeaponDmg: 1),
                    new (position: 390, @class: ClassType.Warrior, level: 361, strength: 29_266, dexterity: 10_265, intelligence: 10_167, constitution: 39_345, luck: 6_642, health: 71_214_448, minWeaponDmg: 1, maxWeaponDmg: 1),
                    new (position: 391, @class: ClassType.Mage, level: 361, strength: 7_296, dexterity: 7_374, intelligence: 29_653, constitution: 29_330, luck: 10_526, health: 21_234_920, minWeaponDmg: 1, maxWeaponDmg: 1),
                    new (position: 392, @class: ClassType.Warrior, level: 361, strength: 29_416, dexterity: 10_317, intelligence: 10_219, constitution: 39_546, luck: 6_676, health: 71_578_256, minWeaponDmg: 1, maxWeaponDmg: 1),
                    new (position: 393, @class: ClassType.Warrior, level: 362, strength: 29_585, dexterity: 10_379, intelligence: 10_281, constitution: 39_779, luck: 6_712, health: 72_198_888, minWeaponDmg: 1, maxWeaponDmg: 1),
                    new (position: 394, @class: ClassType.Warrior, level: 362, strength: 29_660, dexterity: 10_406, intelligence: 10_307, constitution: 39_881, luck: 6_730, health: 72_384_016, minWeaponDmg: 1, maxWeaponDmg: 1),
                    new (position: 395, @class: ClassType.Warrior, level: 362, strength: 29_736, dexterity: 10_432, intelligence: 10_333, constitution: 39_982, luck: 6_747, health: 72_567_328, minWeaponDmg: 1, maxWeaponDmg: 1),
                    new (position: 396, @class: ClassType.Mage, level: 362, strength: 7_413, dexterity: 7_492, intelligence: 30_128, constitution: 29_804, luck: 10_696, health: 21_637_704, minWeaponDmg: 1, maxWeaponDmg: 1),
                    new (position: 397, @class: ClassType.Mage, level: 363, strength: 7_456, dexterity: 7_535, intelligence: 30_299, constitution: 29_980, luck: 10_755, health: 21_825_440, minWeaponDmg: 1, maxWeaponDmg: 1),
                    new (position: 398, @class: ClassType.Warrior, level: 363, strength: 30_057, dexterity: 10_543, intelligence: 10_444, constitution: 40_420, luck: 6_814, health: 73_564_400, minWeaponDmg: 1, maxWeaponDmg: 1),
                    new (position: 399, @class: ClassType.Warrior, level: 363, strength: 30_132, dexterity: 10_570, intelligence: 10_470, constitution: 40_521, luck: 6_831, health: 73_748_224, minWeaponDmg: 1, maxWeaponDmg: 1),
                    new (position: 400, @class: ClassType.Mage, level: 363, strength: 7512, dexterity: 7592, intelligence: 30528, constitution: 30206, luck: 10836, health: 21_989_968, minWeaponDmg: 898, maxWeaponDmg: 1796 ),

                    new (position: 401, @class: ClassType.Mage, level: 364, strength: 7_555, dexterity: 7_635, intelligence: 30_701, constitution: 30_383, luck: 10_899, health: 22_179_590, minWeaponDmg: 1, maxWeaponDmg: 1),
                    new (position: 402, @class: ClassType.Mage, level: 364, strength: 7_574, dexterity: 7_654, intelligence: 30_777, constitution: 30_458, luck: 10_926, health: 22_234_340, minWeaponDmg: 1, maxWeaponDmg: 1),
                    new (position: 403, @class: ClassType.Scout, level: 364, strength: 7_673, dexterity: 30_531, intelligence: 7_633, constitution: 30_873, luck: 10_974, health: 45_074_580, minWeaponDmg: 1, maxWeaponDmg: 1),
                    new (position: 404, @class: ClassType.Warrior, level: 364, strength: 30_607, dexterity: 10_738, intelligence: 10_637, constitution: 41_164, luck: 6_937, health: 75_124_304, minWeaponDmg: 1, maxWeaponDmg: 1),
                    new (position: 405, @class: ClassType.Warrior, level: 365, strength: 33_574, dexterity: 11_777, intelligence: 11_672, constitution: 45_302, luck: 7_533, health: 82_902_656, minWeaponDmg: 1, maxWeaponDmg: 1),
                    new (position: 406, @class: ClassType.Warrior, level: 365, strength: 33_657, dexterity: 11_806, intelligence: 11_701, constitution: 45_414, luck: 7_552, health: 83_107_616, minWeaponDmg: 1, maxWeaponDmg: 1),
                    new (position: 407, @class: ClassType.Scout, level: 365, strength: 8_474, dexterity: 33_740, intelligence: 8_433, constitution: 34_231, luck: 12_096, health: 50_114_184, minWeaponDmg: 1, maxWeaponDmg: 1),
                    new (position: 408, @class: ClassType.Mage, level: 365, strength: 8_413, dexterity: 8_495, intelligence: 34_150, constitution: 33_972, luck: 12_109, health: 24_867_504, minWeaponDmg: 1, maxWeaponDmg: 1),
                    new (position: 409, @class: ClassType.Scout, level: 366, strength: 7_836, dexterity: 31_182, intelligence: 7_796, constitution: 31_539, luck: 11_203, health: 46_299_252, minWeaponDmg: 1, maxWeaponDmg: 1),
                    new (position: 410, @class: ClassType.Scout, level: 366, strength: 7_856, dexterity: 31_258, intelligence: 7_815, constitution: 31_616, luck: 11_230, health: 46_412_288, minWeaponDmg: 1, maxWeaponDmg: 1),
                    new (position: 411, @class: ClassType.Mage, level: 366, strength: 7_793, dexterity: 7_875, intelligence: 31_663, constitution: 31_348, luck: 11_237, health: 23_009_432, minWeaponDmg: 1, maxWeaponDmg: 1),
                    new (position: 412, @class: ClassType.Mage, level: 366, strength: 7_812, dexterity: 7_894, intelligence: 31_740, constitution: 31_424, luck: 11_264, health: 23_065_216, minWeaponDmg: 1, maxWeaponDmg: 1),
                    new (position: 413, @class: ClassType.Scout, level: 367, strength: 7_938, dexterity: 31_586, intelligence: 7_897, constitution: 31_952, luck: 11_349, health: 47_033_344, minWeaponDmg: 1, maxWeaponDmg: 1),
                    new (position: 414, @class: ClassType.Warrior, level: 367, strength: 31_663, dexterity: 11_108, intelligence: 11_004, constitution: 42_596, luck: 7_170, health: 78_376_640, minWeaponDmg: 1, maxWeaponDmg: 1),
                    new (position: 415, @class: ClassType.Warrior, level: 367, strength: 31_739, dexterity: 11_134, intelligence: 11_031, constitution: 42_699, luck: 7_188, health: 78_566_160, minWeaponDmg: 1, maxWeaponDmg: 1),
                    new (position: 416, @class: ClassType.Mage, level: 367, strength: 7_912, dexterity: 7_996, intelligence: 32_148, constitution: 31_834, luck: 11_411, health: 23_429_824, minWeaponDmg: 1, maxWeaponDmg: 1),
                    new (position: 417, @class: ClassType.Scout, level: 368, strength: 8_040, dexterity: 31_992, intelligence: 7_998, constitution: 32_366, luck: 11_493, health: 47_772_216, minWeaponDmg: 1, maxWeaponDmg: 1),
                    new (position: 418, @class: ClassType.Mage, level: 368, strength: 7_975, dexterity: 8_059, intelligence: 32_403, constitution: 32_092, luck: 11_499, health: 23_683_896, minWeaponDmg: 1, maxWeaponDmg: 1),
                    new (position: 419, @class: ClassType.Warrior, level: 368, strength: 32_146, dexterity: 11_275, intelligence: 11_171, constitution: 43_252, luck: 7_274, health: 79_799_936, minWeaponDmg: 1, maxWeaponDmg: 1),
                    new (position: 420, @class: ClassType.Mage, level: 368, strength: 8_014, dexterity: 8_098, intelligence: 32_558, constitution: 32_246, luck: 11_554, health: 23_797_548, minWeaponDmg: 1, maxWeaponDmg: 1),
                    new (position: 421, @class: ClassType.Scout, level: 369, strength: 8_142, dexterity: 32_400, intelligence: 8_100, constitution: 32_782, luck: 11_641, health: 48_517_360, minWeaponDmg: 1, maxWeaponDmg: 1),
                    new (position: 422, @class: ClassType.Mage, level: 369, strength: 8_077, dexterity: 8_161, intelligence: 32_815, constitution: 32_506, luck: 11_647, health: 24_054_440, minWeaponDmg: 1, maxWeaponDmg: 1),
                    new (position: 423, @class: ClassType.Warrior, level: 369, strength: 32_554, dexterity: 11_421, intelligence: 11_315, constitution: 43_806, luck: 7_364, health: 81_041_104, minWeaponDmg: 1, maxWeaponDmg: 1),
                    new (position: 424, @class: ClassType.Mage, level: 369, strength: 8_115, dexterity: 8_200, intelligence: 32_970, constitution: 32_660, luck: 11_702, health: 24_168_400, minWeaponDmg: 1, maxWeaponDmg: 1),
                    new (position: 425, @class: ClassType.Mage, level: 370, strength: 9_180, dexterity: 9_265, intelligence: 37_230, constitution: 37_128, luck: 13_192, health: 27_548_976, minWeaponDmg: 1, maxWeaponDmg: 1),
                    new (position: 426, @class: ClassType.Warrior, level: 370, strength: 36_977, dexterity: 12_967, intelligence: 12_861, constitution: 49_973, luck: 8_256, health: 92_699_912, minWeaponDmg: 1, maxWeaponDmg: 1),
                    new (position: 427, @class: ClassType.Warrior, level: 370, strength: 37_064, dexterity: 12_998, intelligence: 12_891, constitution: 50_089, luck: 8_275, health: 92_915_096, minWeaponDmg: 1, maxWeaponDmg: 1),
                    new (position: 428, @class: ClassType.Warrior, level: 370, strength: 37_150, dexterity: 13_028, intelligence: 12_921, constitution: 50_207, luck: 8_295, health: 93_133_984, minWeaponDmg: 1, maxWeaponDmg: 1),
                    new (position: 429, @class: ClassType.Mage, level: 371, strength: 8_263, dexterity: 8_348, intelligence: 33_565, constitution: 33_261, luck: 11_909, health: 24_746_184, minWeaponDmg: 1, maxWeaponDmg: 1),
                    new (position: 430, @class: ClassType.Warrior, level: 371, strength: 33_299, dexterity: 11_679, intelligence: 11_571, constitution: 44_820, luck: 7_529, health: 83_365_200, minWeaponDmg: 1, maxWeaponDmg: 1),
                    new (position: 431, @class: ClassType.Scout, level: 371, strength: 8_387, dexterity: 33_377, intelligence: 8_344, constitution: 33_779, luck: 11_986, health: 50_263_152, minWeaponDmg: 1, maxWeaponDmg: 1),
                    new (position: 432, @class: ClassType.Scout, level: 371, strength: 8_407, dexterity: 33_454, intelligence: 8_364, constitution: 33_856, luck: 12_014, health: 50_377_728, minWeaponDmg: 1, maxWeaponDmg: 1),
                    new (position: 433, @class: ClassType.Scout, level: 372, strength: 8_452, dexterity: 33_635, intelligence: 8_409, constitution: 34_044, luck: 12_081, health: 50_793_648, minWeaponDmg: 1, maxWeaponDmg: 1),
                    new (position: 434, @class: ClassType.Warrior, level: 372, strength: 33_713, dexterity: 11_826, intelligence: 11_718, constitution: 45_383, luck: 7_621, health: 84_639_296, minWeaponDmg: 1, maxWeaponDmg: 1),
                    new (position: 435, @class: ClassType.Scout, level: 372, strength: 8_491, dexterity: 33_791, intelligence: 8_448, constitution: 34_202, luck: 12_136, health: 51_029_384, minWeaponDmg: 1, maxWeaponDmg: 1),
                    new (position: 436, @class: ClassType.Warrior, level: 372, strength: 33_868, dexterity: 11_881, intelligence: 11_772, constitution: 45_592, luck: 7_656, health: 85_029_080, minWeaponDmg: 1, maxWeaponDmg: 1),
                    new (position: 437, @class: ClassType.Scout, level: 373, strength: 8_556, dexterity: 34_051, intelligence: 8_513, constitution: 34_468, luck: 12_227, health: 51_564_128, minWeaponDmg: 1, maxWeaponDmg: 1),
                    new (position: 438, @class: ClassType.Mage, level: 373, strength: 8_488, dexterity: 8_576, intelligence: 34_479, constitution: 34_180, luck: 12_233, health: 25_566_640, minWeaponDmg: 1, maxWeaponDmg: 1),
                    new (position: 439, @class: ClassType.Mage, level: 373, strength: 8_508, dexterity: 8_596, intelligence: 34_558, constitution: 34_257, luck: 12_261, health: 25_624_236, minWeaponDmg: 1, maxWeaponDmg: 1),
                    new (position: 440, @class: ClassType.Scout, level: 373, strength: 8_615, dexterity: 34_285, intelligence: 8_571, constitution: 34_706, luck: 12_311, health: 51_920_176, minWeaponDmg: 1, maxWeaponDmg: 1),
                    new (position: 441, @class: ClassType.Warrior, level: 374, strength: 34_469, dexterity: 12_092, intelligence: 11_982, constitution: 46_411, luck: 7_784, health: 87_020_624, minWeaponDmg: 1, maxWeaponDmg: 1),
                    new (position: 442, @class: ClassType.Mage, level: 374, strength: 8_592, dexterity: 8_681, intelligence: 34_900, constitution: 34_604, luck: 12_385, health: 25_953_000, minWeaponDmg: 1, maxWeaponDmg: 1),
                    new (position: 443, @class: ClassType.Warrior, level: 374, strength: 34_625, dexterity: 12_147, intelligence: 12_036, constitution: 46_622, luck: 7_819, health: 87_416_248, minWeaponDmg: 1, maxWeaponDmg: 1),
                    new (position: 444, @class: ClassType.Warrior, level: 374, strength: 34_703, dexterity: 12_174, intelligence: 12_063, constitution: 46_727, luck: 7_837, health: 87_613_128, minWeaponDmg: 1, maxWeaponDmg: 1),
                    new (position: 445, @class: ClassType.Scout, level: 375, strength: 10_435, dexterity: 41_563, intelligence: 10_391, constitution: 42_333, luck: 14_863, health: 63_668_832, minWeaponDmg: 1, maxWeaponDmg: 1),
                    new (position: 446, @class: ClassType.Mage, level: 375, strength: 10_369, dexterity: 10_459, intelligence: 42_013, constitution: 42_054, luck: 14_874, health: 31_624_608, minWeaponDmg: 1, maxWeaponDmg: 1),
                    new (position: 447, @class: ClassType.Warrior, level: 375, strength: 41_750, dexterity: 14_639, intelligence: 14_527, constitution: 56_556, luck: 9_253, health: 106_325_280, minWeaponDmg: 1, maxWeaponDmg: 1),
                    new (position: 448, @class: ClassType.Scout, level: 375, strength: 10_506, dexterity: 41_843, intelligence: 10_461, constitution: 42_618, luck: 14_963, health: 64_097_472, minWeaponDmg: 1, maxWeaponDmg: 1),
                    new (position: 449, @class: ClassType.Warrior, level: 376, strength: 35_309, dexterity: 12_383, intelligence: 12_271, constitution: 47_556, luck: 7_970, health: 89_643_056, minWeaponDmg: 1, maxWeaponDmg: 1),
                    new (position: 450, @class: ClassType.Warrior, level: 376, strength: 35388, dexterity: 12411, intelligence: 12298, constitution: 47662, luck: 7988, health: 89_842_872, minWeaponDmg: 413, maxWeaponDmg: 827 ),
                    new (position: 451, @class: ClassType.Warrior, level: 376, strength: 35_467, dexterity: 12_439, intelligence: 12_326, constitution: 47_767, luck: 8_005, health: 90_040_792, minWeaponDmg: 1, maxWeaponDmg: 1),
                    new (position: 452, @class: ClassType.Scout, level: 376, strength: 8_932, dexterity: 35_545, intelligence: 8_896, constitution: 35_994, luck: 12_760, health: 54_278_952, minWeaponDmg: 1, maxWeaponDmg: 1),
                    new (position: 453, @class: ClassType.Warrior, level: 377, strength: 35_733, dexterity: 12_535, intelligence: 12_421, constitution: 48_131, luck: 8_063, health: 90_967_592, minWeaponDmg: 1, maxWeaponDmg: 1),
                    new (position: 454, @class: ClassType.Warrior, level: 377, strength: 35_812, dexterity: 12_562, intelligence: 12_449, constitution: 48_237, luck: 8_081, health: 91_167_928, minWeaponDmg: 1, maxWeaponDmg: 1),
                    new (position: 455, @class: ClassType.Scout, level: 377, strength: 9_018, dexterity: 35_890, intelligence: 8_973, constitution: 36_347, luck: 12_886, health: 54_956_664, minWeaponDmg: 1, maxWeaponDmg: 1),
                    new (position: 456, @class: ClassType.Mage, level: 377, strength: 8_947, dexterity: 9_038, intelligence: 36_334, constitution: 36_044, luck: 12_891, health: 27_249_264, minWeaponDmg: 1, maxWeaponDmg: 1),
                    new (position: 457, @class: ClassType.Scout, level: 378, strength: 9_085, dexterity: 36_158, intelligence: 9_039, constitution: 36_622, luck: 12_979, health: 55_518_952, minWeaponDmg: 1, maxWeaponDmg: 1),
                    new (position: 458, @class: ClassType.Mage, level: 378, strength: 9_013, dexterity: 9_105, intelligence: 36_603, constitution: 36_317, luck: 12_984, health: 27_528_286, minWeaponDmg: 1, maxWeaponDmg: 1),
                    new (position: 459, @class: ClassType.Mage, level: 378, strength: 9_033, dexterity: 9_125, intelligence: 36_683, constitution: 36_397, luck: 13_013, health: 27_588_926, minWeaponDmg: 1, maxWeaponDmg: 1),
                    new (position: 460, @class: ClassType.Warrior, level: 378, strength: 36_395, dexterity: 12_765, intelligence: 12_650, constitution: 49_030, luck: 8_206, health: 92_911_848, minWeaponDmg: 1, maxWeaponDmg: 1),
                    new (position: 461, @class: ClassType.Scout, level: 379, strength: 9_192, dexterity: 36_585, intelligence: 9_146, constitution: 37_059, luck: 13_134, health: 56_329_680, minWeaponDmg: 1, maxWeaponDmg: 1),
                    new (position: 462, @class: ClassType.Mage, level: 379, strength: 9_120, dexterity: 9_212, intelligence: 37_034, constitution: 36_751, luck: 13_139, health: 27_930_760, minWeaponDmg: 1, maxWeaponDmg: 1),
                    new (position: 463, @class: ClassType.Warrior, level: 379, strength: 36_744, dexterity: 12_890, intelligence: 12_774, constitution: 49_500, luck: 8_283, health: 94_050_000, minWeaponDmg: 1, maxWeaponDmg: 1),
                    new (position: 464, @class: ClassType.Scout, level: 379, strength: 9_252, dexterity: 36_823, intelligence: 9_206, constitution: 37_300, luck: 13_219, health: 56_696_000, minWeaponDmg: 1, maxWeaponDmg: 1),
                    new (position: 465, @class: ClassType.Mage, level: 380, strength: 12_378, dexterity: 12_471, intelligence: 50_081, constitution: 50_437, luck: 17_707, health: 38_432_992, minWeaponDmg: 1, maxWeaponDmg: 1),
                    new (position: 466, @class: ClassType.Scout, level: 380, strength: 12_498, dexterity: 49_815, intelligence: 12_452, constitution: 50_937, luck: 17_764, health: 77_627_984, minWeaponDmg: 1, maxWeaponDmg: 1),
                    new (position: 467, @class: ClassType.Mage, level: 380, strength: 12_432, dexterity: 12_525, intelligence: 50_296, constitution: 50_653, luck: 17_783, health: 38_597_584, minWeaponDmg: 1, maxWeaponDmg: 1),
                    new (position: 468, @class: ClassType.Mage, level: 380, strength: 12_458, dexterity: 12_552, intelligence: 50_404, constitution: 50_761, luck: 17_821, health: 38_679_880, minWeaponDmg: 1, maxWeaponDmg: 1),
                    new (position: 469, @class: ClassType.Warrior, level: 381, strength: 37_445, dexterity: 13_132, intelligence: 13_015, constitution: 50_457, luck: 8_437, health: 96_372_872, minWeaponDmg: 1, maxWeaponDmg: 1),
                    new (position: 470, @class: ClassType.Warrior, level: 381, strength: 37_525, dexterity: 13_160, intelligence: 13_043, constitution: 50_564, luck: 8_455, health: 96_577_240, minWeaponDmg: 1, maxWeaponDmg: 1),
                    new (position: 471, @class: ClassType.Warrior, level: 381, strength: 37_605, dexterity: 13_188, intelligence: 13_070, constitution: 50_672, luck: 8_473, health: 96_783_520, minWeaponDmg: 1, maxWeaponDmg: 1),
                    new (position: 472, @class: ClassType.Warrior, level: 381, strength: 37_684, dexterity: 13_216, intelligence: 13_098, constitution: 50_779, luck: 8_491, health: 96_987_888, minWeaponDmg: 1, maxWeaponDmg: 1),
                    new (position: 473, @class: ClassType.Warrior, level: 382, strength: 37_878, dexterity: 13_287, intelligence: 13_168, constitution: 51_046, luck: 8_533, health: 97_753_088, minWeaponDmg: 1, maxWeaponDmg: 1),
                    new (position: 474, @class: ClassType.Mage, level: 382, strength: 9_442, dexterity: 9_537, intelligence: 38_337, constitution: 38_065, luck: 13_599, health: 29_157_790, minWeaponDmg: 1, maxWeaponDmg: 1),
                    new (position: 475, @class: ClassType.Warrior, level: 382, strength: 38_038, dexterity: 13_343, intelligence: 13_224, constitution: 51_261, luck: 8_569, health: 98_164_816, minWeaponDmg: 1, maxWeaponDmg: 1),
                    new (position: 476, @class: ClassType.Warrior, level: 382, strength: 38_118, dexterity: 13_371, intelligence: 13_252, constitution: 51_369, luck: 8_587, health: 98_371_632, minWeaponDmg: 1, maxWeaponDmg: 1),
                    new (position: 477, @class: ClassType.Warrior, level: 383, strength: 38_313, dexterity: 13_437, intelligence: 13_318, constitution: 51_638, luck: 8_624, health: 99_144_960, minWeaponDmg: 1, maxWeaponDmg: 1),
                    new (position: 478, @class: ClassType.Mage, level: 383, strength: 9_550, dexterity: 9_646, intelligence: 38_775, constitution: 38_506, luck: 13_752, health: 29_572_608, minWeaponDmg: 1, maxWeaponDmg: 1),
                    new (position: 479, @class: ClassType.Warrior, level: 383, strength: 38_473, dexterity: 13_493, intelligence: 13_374, constitution: 51_854, luck: 8_660, health: 99_559_680, minWeaponDmg: 1, maxWeaponDmg: 1),
                    new (position: 480, @class: ClassType.Scout, level: 383, strength: 9_686, dexterity: 38_554, intelligence: 9_638, constitution: 39_071, luck: 13_834, health: 60_013_056, minWeaponDmg: 1, maxWeaponDmg: 1),
                    new (position: 481, @class: ClassType.Warrior, level: 384, strength: 38_749, dexterity: 13_593, intelligence: 13_473, constitution: 52_232, luck: 8_721, health: 100_546_600, minWeaponDmg: 1, maxWeaponDmg: 1),
                    new (position: 482, @class: ClassType.Mage, level: 384, strength: 9_659, dexterity: 9_756, intelligence: 39_216, constitution: 38_950, luck: 13_911, health: 29_991_500, minWeaponDmg: 1, maxWeaponDmg: 1),
                    new (position: 483, @class: ClassType.Scout, level: 384, strength: 9_776, dexterity: 38_910, intelligence: 9_728, constitution: 39_436, luck: 13_964, health: 60_731_440, minWeaponDmg: 1, maxWeaponDmg: 1),
                    new (position: 484, @class: ClassType.Warrior, level: 384, strength: 38_991, dexterity: 13_678, intelligence: 13_557, constitution: 52_558, luck: 8_775, health: 101_174_152, minWeaponDmg: 1, maxWeaponDmg: 1),
                    new (position: 485, @class: ClassType.Scout, level: 385, strength: 14_104, dexterity: 56_211, intelligence: 14_055, constitution: 57_596, luck: 20_021, health: 88_928_224, minWeaponDmg: 1, maxWeaponDmg: 1),

                    //


                    new (position: 486, @class: ClassType.Warrior, level: 385, strength: 56327, dexterity: 19741, intelligence: 19625, constitution: 76759, luck: 12247, health: 148144864, minWeaponDmg: 426, maxWeaponDmg: 808 ),

                    // Not confirmed: 
                    new (position: 487, @class: ClassType.Scout, level: 385, strength: 14_162, dexterity: 56_443, intelligence: 14_113, constitution: 57_834, luck: 20_103, health: 89_295_696, minWeaponDmg: 1, maxWeaponDmg: 1),
                    new (position: 488, @class: ClassType.Warrior, level: 385, strength: 56_559, dexterity: 19_823, intelligence: 19_705, constitution: 77_075, luck: 12_298, health: 148_754_752, minWeaponDmg: 1, maxWeaponDmg: 1),
                    new (position: 489, @class: ClassType.Mage, level: 386, strength: 9_858, dexterity: 9_956, intelligence: 40_020, constitution: 39_761, luck: 14_191, health: 30_775_014, minWeaponDmg: 1, maxWeaponDmg: 1),
                    new (position: 490, @class: ClassType.Mage, level: 386, strength: 9_878, dexterity: 9_976, intelligence: 40_102, constitution: 39_843, luck: 14_220, health: 30_838_482, minWeaponDmg: 1, maxWeaponDmg: 1),
                    new (position: 491, @class: ClassType.Mage, level: 386, strength: 9_899, dexterity: 9_997, intelligence: 40_183, constitution: 39_924, luck: 14_249, health: 30_901_176, minWeaponDmg: 1, maxWeaponDmg: 1),
                    new (position: 492, @class: ClassType.Warrior, level: 386, strength: 39_872, dexterity: 13_983, intelligence: 13_860, constitution: 53_758, luck: 8_969, health: 104_021_728, minWeaponDmg: 1, maxWeaponDmg: 1),
                    new (position: 493, @class: ClassType.Mage, level: 387, strength: 9_968, dexterity: 10_067, intelligence: 40_465, constitution: 40_211, luck: 14_351, health: 31_203_736, minWeaponDmg: 1, maxWeaponDmg: 1),
                    new (position: 494, @class: ClassType.Mage, level: 387, strength: 9_989, dexterity: 10_087, intelligence: 40_548, constitution: 40_293, luck: 14_380, health: 31_267_368, minWeaponDmg: 1, maxWeaponDmg: 1),
                    new (position: 495, @class: ClassType.Warrior, level: 387, strength: 40_234, dexterity: 14_112, intelligence: 13_989, constitution: 54_251, luck: 9_049, health: 105_246_944, minWeaponDmg: 1, maxWeaponDmg: 1),
                    new (position: 496, @class: ClassType.Mage, level: 387, strength: 10_029, dexterity: 10_128, intelligence: 40_712, constitution: 40_455, luck: 14_439, health: 31_393_080, minWeaponDmg: 1, maxWeaponDmg: 1),
                    new (position: 497, @class: ClassType.Scout, level: 388, strength: 10_179, dexterity: 40_515, intelligence: 10_129, constitution: 41_080, luck: 14_532, health: 63_920_480, minWeaponDmg: 1, maxWeaponDmg: 1),
                    new (position: 498, @class: ClassType.Warrior, level: 388, strength: 40_597, dexterity: 14_238, intelligence: 14_113, constitution: 54_748, luck: 9_123, health: 106_484_864, minWeaponDmg: 1, maxWeaponDmg: 1),
                    new (position: 499, @class: ClassType.Warrior, level: 388, strength: 40_678, dexterity: 14_266, intelligence: 14_142, constitution: 54_857, luck: 9_142, health: 106_696_864, minWeaponDmg: 1, maxWeaponDmg: 1),
                    new (position: 500, @class: ClassType.Warrior, level: 388, strength: 40760, dexterity: 14295, intelligence: 14170, constitution: 54968, luck: 9160, health: 106_912_760, minWeaponDmg: 427, maxWeaponDmg: 853 ),

                    new (position: 501, @class: ClassType.Warrior, level: 389, strength: 40_962, dexterity: 14_369, intelligence: 14_243, constitution: 55_246, luck: 9_203, health: 107_729_696, minWeaponDmg: 1, maxWeaponDmg: 1),
                    new (position: 502, @class: ClassType.Scout, level: 389, strength: 10_311, dexterity: 41_044, intelligence: 10_261, constitution: 41_620, luck: 14_724, health: 64_927_200, minWeaponDmg: 1, maxWeaponDmg: 1),
                    new (position: 503, @class: ClassType.Scout, level: 389, strength: 10_332, dexterity: 41_125, intelligence: 10_281, constitution: 41_703, luck: 14_753, health: 65_056_680, minWeaponDmg: 1, maxWeaponDmg: 1),
                    new (position: 504, @class: ClassType.Mage, level: 389, strength: 10_251, dexterity: 10_352, intelligence: 41_610, constitution: 41_363, luck: 14_757, health: 32_263_140, minWeaponDmg: 1, maxWeaponDmg: 1),
                    new (position: 505, @class: ClassType.Scout, level: 390, strength: 15_882, dexterity: 63_327, intelligence: 15_832, constitution: 65_009, luck: 22_523, health: 101_674_080, minWeaponDmg: 1, maxWeaponDmg: 1),
                    new (position: 506, @class: ClassType.Warrior, level: 390, strength: 63_452, dexterity: 22_239, intelligence: 22_112, constitution: 86_633, luck: 13_713, health: 169_367_520, minWeaponDmg: 1, maxWeaponDmg: 1),
                    new (position: 507, @class: ClassType.Scout, level: 390, strength: 15_945, dexterity: 63_578, intelligence: 15_894, constitution: 65_266, luck: 22_612, health: 102_076_024, minWeaponDmg: 1, maxWeaponDmg: 1),
                    new (position: 508, @class: ClassType.Warrior, level: 390, strength: 63_703, dexterity: 22_327, intelligence: 22_200, constitution: 86_976, luck: 13_767, health: 170_038_080, minWeaponDmg: 1, maxWeaponDmg: 1),
                    new (position: 509, @class: ClassType.Scout, level: 391, strength: 10_516, dexterity: 41_860, intelligence: 10_465, constitution: 42_457, luck: 15_010, health: 66_572_576, minWeaponDmg: 1, maxWeaponDmg: 1),
                    new (position: 510, @class: ClassType.Mage, level: 391, strength: 10_435, dexterity: 10_537, intelligence: 42_350, constitution: 42_111, luck: 15_014, health: 33_015_024, minWeaponDmg: 1, maxWeaponDmg: 1),
                    new (position: 511, @class: ClassType.Warrior, level: 391, strength: 42_025, dexterity: 14_737, intelligence: 14_609, constitution: 56_692, luck: 9_438, health: 111_116_320, minWeaponDmg: 1, maxWeaponDmg: 1),
                    new (position: 512, @class: ClassType.Mage, level: 391, strength: 10_476, dexterity: 10_578, intelligence: 42_516, constitution: 42_277, luck: 15_073, health: 33_145_168, minWeaponDmg: 1, maxWeaponDmg: 1),
                    new (position: 513, @class: ClassType.Scout, level: 392, strength: 10_629, dexterity: 42_312, intelligence: 10_578, constitution: 42_920, luck: 15_175, health: 67_470_240, minWeaponDmg: 1, maxWeaponDmg: 1),
                    new (position: 514, @class: ClassType.Warrior, level: 392, strength: 42_395, dexterity: 14_870, intelligence: 14_742, constitution: 57_192, luck: 9_519, health: 112_382_280, minWeaponDmg: 1, maxWeaponDmg: 1),
                    new (position: 515, @class: ClassType.Mage, level: 392, strength: 10_568, dexterity: 10_671, intelligence: 42_889, constitution: 42_654, luck: 15_208, health: 33_526_044, minWeaponDmg: 1, maxWeaponDmg: 1),
                    new (position: 516, @class: ClassType.Scout, level: 392, strength: 10_692, dexterity: 42_560, intelligence: 10_640, constitution: 43_171, luck: 15_263, health: 67_864_816, minWeaponDmg: 1, maxWeaponDmg: 1),
                    new (position: 517, @class: ClassType.Mage, level: 393, strength: 10_640, dexterity: 10_743, intelligence: 43_180, constitution: 42_950, luck: 15_308, health: 33_844_600, minWeaponDmg: 1, maxWeaponDmg: 1),
                    new (position: 518, @class: ClassType.Mage, level: 393, strength: 10_660, dexterity: 10_764, intelligence: 43_263, constitution: 43_033, luck: 15_338, health: 33_910_004, minWeaponDmg: 1, maxWeaponDmg: 1),
                    new (position: 519, @class: ClassType.Warrior, level: 393, strength: 42_932, dexterity: 15_056, intelligence: 14_926, constitution: 57_923, luck: 9_633, health: 114_108_312, minWeaponDmg: 1, maxWeaponDmg: 1),
                    new (position: 520, @class: ClassType.Scout, level: 393, strength: 10_806, dexterity: 43_014, intelligence: 10_754, constitution: 43_636, luck: 15_423, health: 68_770_336, minWeaponDmg: 1, maxWeaponDmg: 1),
                    new (position: 521, @class: ClassType.Scout, level: 394, strength: 10_858, dexterity: 43_222, intelligence: 10_806, constitution: 43_851, luck: 15_500, health: 69_284_576, minWeaponDmg: 1, maxWeaponDmg: 1),
                    new (position: 522, @class: ClassType.Mage, level: 394, strength: 10_774, dexterity: 10_878, intelligence: 43_723, constitution: 43_497, luck: 15_503, health: 34_362_632, minWeaponDmg: 1, maxWeaponDmg: 1),
                    new (position: 523, @class: ClassType.Mage, level: 394, strength: 10_795, dexterity: 10_899, intelligence: 43_806, constitution: 43_580, luck: 15_533, health: 34_428_200, minWeaponDmg: 1, maxWeaponDmg: 1),
                    new (position: 524, @class: ClassType.Warrior, level: 394, strength: 43_471, dexterity: 15_248, intelligence: 15_117, constitution: 58_657, luck: 9_752, health: 115_847_576, minWeaponDmg: 1, maxWeaponDmg: 1),
                    new (position: 525, @class: ClassType.Scout, level: 395, strength: 17_824, dexterity: 71_085, intelligence: 17_771, constitution: 73_096, luck: 25_252, health: 115_784_064, minWeaponDmg: 1, maxWeaponDmg: 1),
                    new (position: 526, @class: ClassType.Mage, level: 395, strength: 17_752, dexterity: 17_858, intelligence: 71_641, constitution: 72_793, luck: 25_274, health: 57_652_056, minWeaponDmg: 1, maxWeaponDmg: 1),
                    new (position: 527, @class: ClassType.Scout, level: 395, strength: 17_892, dexterity: 71_356, intelligence: 17_839, constitution: 73_374, luck: 25_349, health: 116_224_416, minWeaponDmg: 1, maxWeaponDmg: 1),
                    new (position: 528, @class: ClassType.Warrior, level: 395, strength: 71_491, dexterity: 25_054, intelligence: 24_922, constitution: 97_774, luck: 15_365, health: 193_592_512, minWeaponDmg: 1, maxWeaponDmg: 1),
                    new (position: 529, @class: ClassType.Warrior, level: 396, strength: 44_140, dexterity: 15_479, intelligence: 15_346, constitution: 59_572, luck: 9_898, health: 118_250_416, minWeaponDmg: 1, maxWeaponDmg: 1),
                    new (position: 530, @class: ClassType.Mage, level: 396, strength: 11_003, dexterity: 11_109, intelligence: 44_647, constitution: 44_431, luck: 15_826, health: 35_278_216, minWeaponDmg: 1, maxWeaponDmg: 1),
                    new (position: 531, @class: ClassType.Scout, level: 396, strength: 11_130, dexterity: 44_307, intelligence: 11_077, constitution: 44_961, luck: 15_882, health: 71_398_064, minWeaponDmg: 1, maxWeaponDmg: 1),
                    new (position: 532, @class: ClassType.Mage, level: 396, strength: 11_044, dexterity: 11_151, intelligence: 44_816, constitution: 44_599, luck: 15_886, health: 35_411_608, minWeaponDmg: 1, maxWeaponDmg: 1),
                    new (position: 533, @class: ClassType.Scout, level: 397, strength: 11_204, dexterity: 44_601, intelligence: 11_150, constitution: 45_264, luck: 15_990, health: 72_060_288, minWeaponDmg: 1, maxWeaponDmg: 1),
                    new (position: 534, @class: ClassType.Mage, level: 397, strength: 11_118, dexterity: 11_225, intelligence: 45_112, constitution: 44_901, luck: 15_993, health: 35_741_196, minWeaponDmg: 1, maxWeaponDmg: 1),
                    new (position: 535, @class: ClassType.Scout, level: 397, strength: 11_246, dexterity: 44_769, intelligence: 11_192, constitution: 45_435, luck: 16_050, health: 72_332_520, minWeaponDmg: 1, maxWeaponDmg: 1),
                    new (position: 536, @class: ClassType.Scout, level: 397, strength: 11_267, dexterity: 44_852, intelligence: 11_213, constitution: 45_520, luck: 16_080, health: 72_467_840, minWeaponDmg: 1, maxWeaponDmg: 1),
                    new (position: 537, @class: ClassType.Warrior, level: 398, strength: 45_065, dexterity: 15_804, intelligence: 15_670, constitution: 60_834, luck: 10_096, health: 121_363_832, minWeaponDmg: 1, maxWeaponDmg: 1),
                    new (position: 538, @class: ClassType.Warrior, level: 398, strength: 45_149, dexterity: 15_833, intelligence: 15_699, constitution: 60_947, luck: 10_114, health: 121_589_264, minWeaponDmg: 1, maxWeaponDmg: 1),
                    new (position: 539, @class: ClassType.Warrior, level: 398, strength: 45_233, dexterity: 15_863, intelligence: 15_728, constitution: 61_061, luck: 10_133, health: 121_816_696, minWeaponDmg: 1, maxWeaponDmg: 1),
                    new (position: 540, @class: ClassType.Mage, level: 398, strength: 11_275, dexterity: 11_383, intelligence: 45_749, constitution: 45_542, luck: 16_216, health: 36_342_516, minWeaponDmg: 1, maxWeaponDmg: 1),
                    new (position: 541, @class: ClassType.Warrior, level: 399, strength: 45_531, dexterity: 15_970, intelligence: 15_835, constitution: 61_469, luck: 10_198, health: 122_938_000, minWeaponDmg: 1, maxWeaponDmg: 1),
                    new (position: 542, @class: ClassType.Warrior, level: 399, strength: 45_615, dexterity: 16_000, intelligence: 15_864, constitution: 61_583, luck: 10_217, health: 123_166_000, minWeaponDmg: 1, maxWeaponDmg: 1),
                    new (position: 543, @class: ClassType.Warrior, level: 399, strength: 45_699, dexterity: 16_029, intelligence: 15_894, constitution: 61_696, luck: 10_236, health: 123_392_000, minWeaponDmg: 1, maxWeaponDmg: 1),
                    new (position: 544, @class: ClassType.Scout, level: 399, strength: 11_500, dexterity: 45_783, intelligence: 11_446, constitution: 46_473, luck: 16_412, health: 74_356_800, minWeaponDmg: 1, maxWeaponDmg: 1),
                    new (position: 545, @class: ClassType.Mage, level: 400, strength: 22_890, dexterity: 22_999, intelligence: 92_214, constitution: 94_307, luck: 32_482, health: 75_634_216, minWeaponDmg: 1, maxWeaponDmg: 1),
                    new (position: 546, @class: ClassType.Warrior, level: 400, strength: 91_946, dexterity: 32_214, intelligence: 32_077, constitution: 126_269, luck: 19_492, health: 253_169_344, minWeaponDmg: 1, maxWeaponDmg: 1),
                    new (position: 547, @class: ClassType.Scout, level: 400, strength: 23_083, dexterity: 92_115, intelligence: 23_029, constitution: 95_112, luck: 32_629, health: 152_559_648, minWeaponDmg: 1, maxWeaponDmg: 1),
                    new (position: 548, @class: ClassType.Scout, level: 400, strength: 23_126, dexterity: 92_283, intelligence: 23_071, constitution: 95_286, luck: 32_688, health: 152_838_752, minWeaponDmg: 1, maxWeaponDmg: 1),
                    new (position: 549, @class: ClassType.Scout, level: 401, strength: 11_672, dexterity: 46_467, intelligence: 11_617, constitution: 47_177, luck: 16_651, health: 75_860_616, minWeaponDmg: 1, maxWeaponDmg: 1),
                    new (position: 550, @class: ClassType.Mage, level: 401, strength: 11583, dexterity: 11693, intelligence: 46992, constitution: 46801, luck: 16654, health: 37_628_004, minWeaponDmg: 992, maxWeaponDmg: 1985 ),

                    new (position: 551, @class: ClassType.Warrior, level: 401, strength: 46_637, dexterity: 16_354, intelligence: 16_216, constitution: 62_975, luck: 10_441, health: 126_579_752, minWeaponDmg: 1, maxWeaponDmg: 1),
                    new (position: 552, @class: ClassType.Warrior, level: 401, strength: 46_721, dexterity: 16_383, intelligence: 16_245, constitution: 63_089, luck: 10_460, health: 126_808_888, minWeaponDmg: 1, maxWeaponDmg: 1),
                    new (position: 553, @class: ClassType.Mage, level: 402, strength: 11_679, dexterity: 11_790, intelligence: 47_381, constitution: 47_195, luck: 16_795, health: 38_039_168, minWeaponDmg: 1, maxWeaponDmg: 1),
                    new (position: 554, @class: ClassType.Scout, level: 402, strength: 11_811, dexterity: 47_024, intelligence: 11_756, constitution: 47_746, luck: 16_853, health: 76_966_552, minWeaponDmg: 1, maxWeaponDmg: 1),
                    new (position: 555, @class: ClassType.Mage, level: 402, strength: 11_722, dexterity: 11_833, intelligence: 47_552, constitution: 47_366, luck: 16_855, health: 38_176_996, minWeaponDmg: 1, maxWeaponDmg: 1),
                    new (position: 556, @class: ClassType.Mage, level: 402, strength: 11_743, dexterity: 11_854, intelligence: 47_638, constitution: 47_452, luck: 16_886, health: 38_246_312, minWeaponDmg: 1, maxWeaponDmg: 1),
                    new (position: 557, @class: ClassType.Warrior, level: 403, strength: 47_412, dexterity: 16_626, intelligence: 16_487, constitution: 64_035, luck: 10_605, health: 129_350_704, minWeaponDmg: 1, maxWeaponDmg: 1),
                    new (position: 558, @class: ClassType.Warrior, level: 403, strength: 47_497, dexterity: 16_656, intelligence: 16_517, constitution: 64_150, luck: 10_624, health: 129_583_000, minWeaponDmg: 1, maxWeaponDmg: 1),
                    new (position: 559, @class: ClassType.Warrior, level: 403, strength: 47_582, dexterity: 16_686, intelligence: 16_546, constitution: 64_265, luck: 10_643, health: 129_815_296, minWeaponDmg: 1, maxWeaponDmg: 1),
                    new (position: 560, @class: ClassType.Mage, level: 403, strength: 11_861, dexterity: 11_973, intelligence: 48_115, constitution: 47_934, luck: 17_052, health: 38_730_672, minWeaponDmg: 1, maxWeaponDmg: 1),
                    new (position: 561, @class: ClassType.Scout, level: 404, strength: 12_028, dexterity: 47_887, intelligence: 11_972, constitution: 48_632, luck: 17_161, health: 78_783_840, minWeaponDmg: 1, maxWeaponDmg: 1),
                    new (position: 562, @class: ClassType.Mage, level: 404, strength: 11_937, dexterity: 12_049, intelligence: 48_422, constitution: 48_246, luck: 17_163, health: 39_079_260, minWeaponDmg: 1, maxWeaponDmg: 1),
                    new (position: 563, @class: ClassType.Scout, level: 404, strength: 12_071, dexterity: 48_058, intelligence: 12_014, constitution: 48_805, luck: 17_222, health: 79_064_096, minWeaponDmg: 1, maxWeaponDmg: 1),
                    new (position: 564, @class: ClassType.Mage, level: 404, strength: 11_979, dexterity: 12_092, intelligence: 48_594, constitution: 48_419, luck: 17_225, health: 39_219_392, minWeaponDmg: 1, maxWeaponDmg: 1),
                    new (position: 565, @class: ClassType.Mage, level: 405, strength: 12_034, dexterity: 12_148, intelligence: 48_816, constitution: 48_646, luck: 17_300, health: 39_500_552, minWeaponDmg: 1, maxWeaponDmg: 1),
                    new (position: 566, @class: ClassType.Scout, level: 405, strength: 12_169, dexterity: 48_450, intelligence: 12_112, constitution: 49_208, luck: 17_359, health: 79_913_792, minWeaponDmg: 1, maxWeaponDmg: 1),
                    new (position: 567, @class: ClassType.Scout, level: 405, strength: 12_190, dexterity: 48_535, intelligence: 12_134, constitution: 49_295, luck: 17_390, health: 80_055_080, minWeaponDmg: 1, maxWeaponDmg: 1),
                    new (position: 568, @class: ClassType.Scout, level: 405, strength: 12_212, dexterity: 48_621, intelligence: 12_155, constitution: 49_382, luck: 17_421, health: 80_196_368, minWeaponDmg: 1, maxWeaponDmg: 1),
                    new (position: 569, @class: ClassType.Scout, level: 406, strength: 12_268, dexterity: 48_843, intelligence: 12_211, constitution: 49_612, luck: 17_497, health: 80_768_336, minWeaponDmg: 1, maxWeaponDmg: 1),
                    new (position: 570, @class: ClassType.Scout, level: 406, strength: 12_289, dexterity: 48_929, intelligence: 12_232, constitution: 49_700, luck: 17_528, health: 80_911_600, minWeaponDmg: 1, maxWeaponDmg: 1),
                    new (position: 571, @class: ClassType.Mage, level: 406, strength: 12_197, dexterity: 12_311, intelligence: 49_471, constitution: 49_307, luck: 17_530, health: 40_135_896, minWeaponDmg: 1, maxWeaponDmg: 1),
                    new (position: 572, @class: ClassType.Warrior, level: 406, strength: 49_100, dexterity: 17_217, intelligence: 17_074, constitution: 66_331, luck: 10_977, health: 134_983_584, minWeaponDmg: 1, maxWeaponDmg: 1),
                    new (position: 573, @class: ClassType.Scout, level: 407, strength: 12_388, dexterity: 49_324, intelligence: 12_331, constitution: 50_105, luck: 17_671, health: 81_771_360, minWeaponDmg: 1, maxWeaponDmg: 1),
                    new (position: 574, @class: ClassType.Mage, level: 407, strength: 12_295, dexterity: 12_410, intelligence: 49_869, constitution: 49_711, luck: 17_673, health: 40_564_176, minWeaponDmg: 1, maxWeaponDmg: 1),
                    new (position: 575, @class: ClassType.Warrior, level: 407, strength: 49_496, dexterity: 17_359, intelligence: 17_216, constitution: 66_871, luck: 11_063, health: 136_416_832, minWeaponDmg: 1, maxWeaponDmg: 1),
                    new (position: 576, @class: ClassType.Scout, level: 407, strength: 12_453, dexterity: 49_582, intelligence: 12_396, constitution: 50_367, luck: 17_764, health: 82_198_944, minWeaponDmg: 1, maxWeaponDmg: 1),
                    new (position: 577, @class: ClassType.Mage, level: 408, strength: 12_394, dexterity: 12_509, intelligence: 50_268, constitution: 50_115, luck: 17_812, health: 40_994_072, minWeaponDmg: 1, maxWeaponDmg: 1),
                    new (position: 578, @class: ClassType.Warrior, level: 408, strength: 49_893, dexterity: 17_496, intelligence: 17_352, constitution: 67_414, luck: 11_144, health: 137_861_632, minWeaponDmg: 1, maxWeaponDmg: 1),
                    new (position: 579, @class: ClassType.Warrior, level: 408, strength: 49_979, dexterity: 17_526, intelligence: 17_382, constitution: 67_531, luck: 11_163, health: 138_100_896, minWeaponDmg: 1, maxWeaponDmg: 1),
                    new (position: 580, @class: ClassType.Scout, level: 408, strength: 12_574, dexterity: 50_066, intelligence: 12_516, constitution: 50_864, luck: 17_934, health: 83_213_504, minWeaponDmg: 1, maxWeaponDmg: 1),
                    new (position: 581, @class: ClassType.Warrior, level: 409, strength: 50_291, dexterity: 17_639, intelligence: 17_494, constitution: 67_959, luck: 11_231, health: 139_315_952, minWeaponDmg: 1, maxWeaponDmg: 1),
                    new (position: 582, @class: ClassType.Mage, level: 409, strength: 12_536, dexterity: 12_653, intelligence: 50_844, constitution: 50_697, luck: 18_019, health: 41_571_540, minWeaponDmg: 1, maxWeaponDmg: 1),
                    new (position: 583, @class: ClassType.Mage, level: 409, strength: 12_558, dexterity: 12_674, intelligence: 50_931, constitution: 50_784, luck: 18_050, health: 41_642_880, minWeaponDmg: 1, maxWeaponDmg: 1),
                    new (position: 584, @class: ClassType.Scout, level: 409, strength: 12_696, dexterity: 50_551, intelligence: 12_638, constitution: 51_362, luck: 18_110, health: 84_233_680, minWeaponDmg: 1, maxWeaponDmg: 1),
                    new (position: 585, @class: ClassType.Warrior, level: 410, strength: 50_778, dexterity: 17_807, intelligence: 17_661, constitution: 68_624, luck: 11_337, health: 141_022_320, minWeaponDmg: 1, maxWeaponDmg: 1),
                    new (position: 586, @class: ClassType.Mage, level: 410, strength: 12_658, dexterity: 12_775, intelligence: 51_334, constitution: 51_193, luck: 18_189, health: 42_080_648, minWeaponDmg: 1, maxWeaponDmg: 1),
                    new (position: 587, @class: ClassType.Warrior, level: 410, strength: 50_952, dexterity: 17_868, intelligence: 17_722, constitution: 68_859, luck: 11_376, health: 141_505_248, minWeaponDmg: 1, maxWeaponDmg: 1),
                    new (position: 588, @class: ClassType.Warrior, level: 410, strength: 51_038, dexterity: 17_899, intelligence: 17_752, constitution: 68_976, luck: 11_395, health: 141_745_680, minWeaponDmg: 1, maxWeaponDmg: 1),
                    new (position: 589, @class: ClassType.Mage, level: 411, strength: 12_758, dexterity: 12_876, intelligence: 51_738, constitution: 51_603, luck: 18_330, health: 42_520_872, minWeaponDmg: 1, maxWeaponDmg: 1),
                    new (position: 590, @class: ClassType.Mage, level: 411, strength: 12_779, dexterity: 12_897, intelligence: 51_826, constitution: 51_692, luck: 18_361, health: 42_594_208, minWeaponDmg: 1, maxWeaponDmg: 1),
                    new (position: 591, @class: ClassType.Warrior, level: 411, strength: 51_441, dexterity: 18_037, intelligence: 17_890, constitution: 69_527, luck: 11_483, health: 143_225_616, minWeaponDmg: 1, maxWeaponDmg: 1),
                    new (position: 592, @class: ClassType.Warrior, level: 411, strength: 51_528, dexterity: 18_068, intelligence: 17_920, constitution: 69_644, luck: 11_503, health: 143_466_640, minWeaponDmg: 1, maxWeaponDmg: 1),
                    new (position: 593, @class: ClassType.Scout, level: 412, strength: 12_999, dexterity: 51_757, intelligence: 12_939, constitution: 52_602, luck: 18_537, health: 86_898_504, minWeaponDmg: 1, maxWeaponDmg: 1),
                    new (position: 594, @class: ClassType.Scout, level: 412, strength: 13_020, dexterity: 51_844, intelligence: 12_961, constitution: 52_690, luck: 18_568, health: 87_043_880, minWeaponDmg: 1, maxWeaponDmg: 1),
                    new (position: 595, @class: ClassType.Warrior, level: 412, strength: 51_932, dexterity: 18_213, intelligence: 18_064, constitution: 70_197, luck: 11_591, health: 144_956_800, minWeaponDmg: 1, maxWeaponDmg: 1),
                    new (position: 596, @class: ClassType.Scout, level: 412, strength: 13_064, dexterity: 52_019, intelligence: 13_005, constitution: 52_868, luck: 18_631, health: 87_337_936, minWeaponDmg: 1, maxWeaponDmg: 1),
                    new (position: 597, @class: ClassType.Warrior, level: 413, strength: 52_249, dexterity: 18_322, intelligence: 18_173, constitution: 70_634, luck: 11_653, health: 146_212_384, minWeaponDmg: 1, maxWeaponDmg: 1),
                    new (position: 598, @class: ClassType.Warrior, level: 413, strength: 52_337, dexterity: 18_353, intelligence: 18_203, constitution: 70_752, luck: 11_673, health: 146_456_640, minWeaponDmg: 1, maxWeaponDmg: 1),
                    new (position: 599, @class: ClassType.Scout, level: 413, strength: 13_166, dexterity: 52_424, intelligence: 13_106, constitution: 53_284, luck: 18_773, health: 88_238_304, minWeaponDmg: 1, maxWeaponDmg: 1),
                    new (position: 600, @class: ClassType.Scout, level: 413, strength: 13188, dexterity: 52512, intelligence: 13128, constitution: 53374, luck: 18804, health: 88_387_344, minWeaponDmg: 568, maxWeaponDmg: 1135 ),

                    new (position: 601, @class: ClassType.Scout, level: 414, strength: 13_246, dexterity: 52_744, intelligence: 13_186, constitution: 53_614, luck: 18_889, health: 88_999_240, minWeaponDmg: 1, maxWeaponDmg: 1),
                    new (position: 602, @class: ClassType.Scout, level: 414, strength: 13_268, dexterity: 52_832, intelligence: 13_208, constitution: 53_703, luck: 18_921, health: 89_146_976, minWeaponDmg: 1, maxWeaponDmg: 1),
                    new (position: 603, @class: ClassType.Warrior, level: 414, strength: 52_919, dexterity: 18_560, intelligence: 18_410, constitution: 71_546, luck: 11_801, health: 148_457_952, minWeaponDmg: 1, maxWeaponDmg: 1),
                    new (position: 604, @class: ClassType.Mage, level: 414, strength: 13_191, dexterity: 13_312, intelligence: 53_490, constitution: 53_375, luck: 18_954, health: 44_301_248, minWeaponDmg: 1, maxWeaponDmg: 1),
                    new (position: 605, @class: ClassType.Warrior, level: 415, strength: 53_240, dexterity: 18_670, intelligence: 18_519, constitution: 71_987, luck: 11_870, health: 149_732_960, minWeaponDmg: 1, maxWeaponDmg: 1),
                    new (position: 606, @class: ClassType.Mage, level: 415, strength: 13_271, dexterity: 13_393, intelligence: 53_813, constitution: 53_703, luck: 19_065, health: 44_680_896, minWeaponDmg: 1, maxWeaponDmg: 1),
                    new (position: 607, @class: ClassType.Warrior, level: 415, strength: 53_416, dexterity: 18_732, intelligence: 18_580, constitution: 72_224, luck: 11_909, health: 150_225_920, minWeaponDmg: 1, maxWeaponDmg: 1),
                    new (position: 608, @class: ClassType.Scout, level: 415, strength: 13_437, dexterity: 53_504, intelligence: 13_376, constitution: 54_392, luck: 19_158, health: 90_508_288, minWeaponDmg: 1, maxWeaponDmg: 1),
                    new (position: 609, @class: ClassType.Warrior, level: 416, strength: 53_738, dexterity: 18_842, intelligence: 18_690, constitution: 72_667, luck: 11_979, health: 151_510_688, minWeaponDmg: 1, maxWeaponDmg: 1),
                    new (position: 610, @class: ClassType.Warrior, level: 416, strength: 53_826, dexterity: 18_873, intelligence: 18_721, constitution: 72_786, luck: 11_999, health: 151_758_816, minWeaponDmg: 1, maxWeaponDmg: 1),
                    new (position: 611, @class: ClassType.Scout, level: 416, strength: 13_540, dexterity: 53_915, intelligence: 13_479, constitution: 54_814, luck: 19_301, health: 91_429_752, minWeaponDmg: 1, maxWeaponDmg: 1),
                    new (position: 612, @class: ClassType.Warrior, level: 416, strength: 54_003, dexterity: 18_935, intelligence: 18_782, constitution: 73_025, luck: 12_038, health: 152_257_120, minWeaponDmg: 1, maxWeaponDmg: 1),
                    new (position: 613, @class: ClassType.Scout, level: 417, strength: 13_621, dexterity: 54_238, intelligence: 13_560, constitution: 55_148, luck: 19_420, health: 92_207_456, minWeaponDmg: 1, maxWeaponDmg: 1),
                    new (position: 614, @class: ClassType.Mage, level: 417, strength: 13_520, dexterity: 13_643, intelligence: 54_818, constitution: 54_722, luck: 19_421, health: 45_747_592, minWeaponDmg: 1, maxWeaponDmg: 1),
                    new (position: 615, @class: ClassType.Mage, level: 417, strength: 13_542, dexterity: 13_665, intelligence: 54_907, constitution: 54_811, luck: 19_452, health: 45_821_996, minWeaponDmg: 1, maxWeaponDmg: 1),
                    new (position: 616, @class: ClassType.Warrior, level: 417, strength: 54_504, dexterity: 19_114, intelligence: 18_960, constitution: 73_703, luck: 12_148, health: 154_039_264, minWeaponDmg: 1, maxWeaponDmg: 1),
                    new (position: 617, @class: ClassType.Scout, level: 418, strength: 13_747, dexterity: 54_740, intelligence: 13_685, constitution: 55_664, luck: 19_596, health: 93_292_864, minWeaponDmg: 1, maxWeaponDmg: 1),
                    new (position: 618, @class: ClassType.Scout, level: 418, strength: 13_769, dexterity: 54_829, intelligence: 13_707, constitution: 55_754, luck: 19_628, health: 93_443_704, minWeaponDmg: 1, maxWeaponDmg: 1),
                    new (position: 619, @class: ClassType.Scout, level: 418, strength: 13_791, dexterity: 54_918, intelligence: 13_729, constitution: 55_843, luck: 19_659, health: 93_592_864, minWeaponDmg: 1, maxWeaponDmg: 1),
                    new (position: 620, @class: ClassType.Mage, level: 418, strength: 13_690, dexterity: 13_814, intelligence: 55_502, constitution: 55_413, luck: 19_660, health: 46_436_096, minWeaponDmg: 1, maxWeaponDmg: 1),
                    new (position: 621, @class: ClassType.Scout, level: 419, strength: 13_873, dexterity: 55_244, intelligence: 13_811, constitution: 56_180, luck: 19_779, health: 94_382_400, minWeaponDmg: 1, maxWeaponDmg: 1),
                    new (position: 622, @class: ClassType.Scout, level: 419, strength: 13_895, dexterity: 55_333, intelligence: 13_833, constitution: 56_272, luck: 19_811, health: 94_536_960, minWeaponDmg: 1, maxWeaponDmg: 1),
                    new (position: 623, @class: ClassType.Warrior, level: 419, strength: 55_422, dexterity: 19_438, intelligence: 19_282, constitution: 74_960, luck: 12_342, health: 157_416_000, minWeaponDmg: 1, maxWeaponDmg: 1),
                    new (position: 624, @class: ClassType.Scout, level: 419, strength: 13_940, dexterity: 55_511, intelligence: 13_878, constitution: 56_452, luck: 19_874, health: 94_839_360, minWeaponDmg: 1, maxWeaponDmg: 1),
                    new (position: 625, @class: ClassType.Mage, level: 420, strength: 13_875, dexterity: 14_000, intelligence: 56_250, constitution: 56_175, luck: 19_925, health: 47_299_352, minWeaponDmg: 1, maxWeaponDmg: 1),
                    new (position: 626, @class: ClassType.Mage, level: 420, strength: 13_897, dexterity: 14_022, intelligence: 56_340, constitution: 56_265, luck: 19_957, health: 47_375_128, minWeaponDmg: 1, maxWeaponDmg: 1),
                    new (position: 627, @class: ClassType.Scout, level: 420, strength: 14_045, dexterity: 55_928, intelligence: 13_982, constitution: 56_882, luck: 20_020, health: 95_789_288, minWeaponDmg: 1, maxWeaponDmg: 1),
                    new (position: 628, @class: ClassType.Scout, level: 420, strength: 14_067, dexterity: 56_018, intelligence: 14_004, constitution: 56_972, luck: 20_052, health: 95_940_848, minWeaponDmg: 1, maxWeaponDmg: 1),
                    new (position: 629, @class: ClassType.Warrior, level: 421, strength: 56_258, dexterity: 19_725, intelligence: 19_568, constitution: 76_104, luck: 12_523, health: 160_579_440, minWeaponDmg: 1, maxWeaponDmg: 1),
                    new (position: 630, @class: ClassType.Warrior, level: 421, strength: 56_347, dexterity: 19_757, intelligence: 19_599, constitution: 76_225, luck: 12_543, health: 160_834_752, minWeaponDmg: 1, maxWeaponDmg: 1),
                    new (position: 631, @class: ClassType.Mage, level: 421, strength: 14_046, dexterity: 14_172, intelligence: 56_941, constitution: 56_873, luck: 20_167, health: 48_000_812, minWeaponDmg: 1, maxWeaponDmg: 1),
                    new (position: 632, @class: ClassType.Mage, level: 421, strength: 14_068, dexterity: 14_195, intelligence: 57_032, constitution: 56_964, luck: 20_199, health: 48_077_616, minWeaponDmg: 1, maxWeaponDmg: 1),
                    new (position: 633, @class: ClassType.Mage, level: 422, strength: 14_129, dexterity: 14_255, intelligence: 57_274, constitution: 57_213, luck: 20_288, health: 48_402_200, minWeaponDmg: 1, maxWeaponDmg: 1),
                    new (position: 634, @class: ClassType.Scout, level: 422, strength: 14_278, dexterity: 56_857, intelligence: 14_214, constitution: 57_836, luck: 20_351, health: 97_858_512, minWeaponDmg: 1, maxWeaponDmg: 1),
                    new (position: 635, @class: ClassType.Mage, level: 422, strength: 14_173, dexterity: 14_300, intelligence: 57_455, constitution: 57_394, luck: 20_352, health: 48_555_324, minWeaponDmg: 1, maxWeaponDmg: 1),
                    new (position: 636, @class: ClassType.Scout, level: 422, strength: 14_323, dexterity: 57_036, intelligence: 14_259, constitution: 58_019, luck: 20_416, health: 98_168_144, minWeaponDmg: 1, maxWeaponDmg: 1),
                    new (position: 637, @class: ClassType.Mage, level: 423, strength: 14_256, dexterity: 14_383, intelligence: 57_789, constitution: 57_735, luck: 20_467, health: 48_959_280, minWeaponDmg: 1, maxWeaponDmg: 1),
                    new (position: 638, @class: ClassType.Warrior, level: 423, strength: 57_369, dexterity: 20_116, intelligence: 19_957, constitution: 77_621, luck: 12_760, health: 164_556_512, minWeaponDmg: 1, maxWeaponDmg: 1),
                    new (position: 639, @class: ClassType.Warrior, level: 423, strength: 57_459, dexterity: 20_148, intelligence: 19_988, constitution: 77_743, luck: 12_780, health: 164_815_168, minWeaponDmg: 1, maxWeaponDmg: 1),
                    new (position: 640, @class: ClassType.Warrior, level: 423, strength: 57_549, dexterity: 20_179, intelligence: 20_019, constitution: 77_865, luck: 12_800, health: 165_073_792, minWeaponDmg: 1, maxWeaponDmg: 1),
                    new (position: 641, @class: ClassType.Warrior, level: 424, strength: 57_793, dexterity: 20_268, intelligence: 20_108, constitution: 78_202, luck: 12_852, health: 166_179_248, minWeaponDmg: 1, maxWeaponDmg: 1),
                    new (position: 642, @class: ClassType.Mage, level: 424, strength: 14_406, dexterity: 14_535, intelligence: 58_396, constitution: 58_351, luck: 20_685, health: 49_598_352, minWeaponDmg: 1, maxWeaponDmg: 1),
                    new (position: 643, @class: ClassType.Mage, level: 424, strength: 14_429, dexterity: 14_558, intelligence: 58_487, constitution: 58_441, luck: 20_717, health: 49_674_848, minWeaponDmg: 1, maxWeaponDmg: 1),
                    new (position: 644, @class: ClassType.Warrior, level: 424, strength: 58_063, dexterity: 20_363, intelligence: 20_202, constitution: 78_567, luck: 12_912, health: 166_954_880, minWeaponDmg: 1, maxWeaponDmg: 1),
                    new (position: 645, @class: ClassType.Mage, level: 425, strength: 14_513, dexterity: 14_642, intelligence: 58_824, constitution: 58_785, luck: 20_834, health: 50_084_820, minWeaponDmg: 1, maxWeaponDmg: 1),
                    new (position: 646, @class: ClassType.Scout, level: 425, strength: 14_664, dexterity: 58_398, intelligence: 14_600, constitution: 59_420, luck: 20_898, health: 101_251_680, minWeaponDmg: 1, maxWeaponDmg: 1),
                    new (position: 647, @class: ClassType.Scout, level: 425, strength: 14_687, dexterity: 58_489, intelligence: 14_622, constitution: 59_511, luck: 20_930, health: 101_406_744, minWeaponDmg: 1, maxWeaponDmg: 1),
                    new (position: 648, @class: ClassType.Mage, level: 425, strength: 14_580, dexterity: 14_710, intelligence: 59_098, constitution: 59_058, luck: 20_930, health: 50_317_416, minWeaponDmg: 1, maxWeaponDmg: 1),
                    new (position: 649, @class: ClassType.Scout, level: 426, strength: 14_771, dexterity: 58_825, intelligence: 14_706, constitution: 59_858, luck: 21_047, health: 102_237_464, minWeaponDmg: 1, maxWeaponDmg: 1),
                    new (position: 650, @class: ClassType.Mage, level: 426, strength: 14664, dexterity: 14794, intelligence: 59436, constitution: 59405, luck: 21047, health: 50_731_872, minWeaponDmg: 1054, maxWeaponDmg: 2108 ),
                    //

                    new (position: 651, @class: ClassType.Warrior, level: 426, strength: 59_007, dexterity: 20_689, intelligence: 20_526, constitution: 79_859, luck: 13_118, health: 170_498_960, minWeaponDmg: 1, maxWeaponDmg: 1),
                    new (position: 652, @class: ClassType.Scout, level: 426, strength: 14_840, dexterity: 59_097, intelligence: 14_774, constitution: 60_136, luck: 21_144, health: 102_712_288, minWeaponDmg: 1, maxWeaponDmg: 1),
                    new (position: 653, @class: ClassType.Mage, level: 427, strength: 14_771, dexterity: 14_901, intelligence: 59_867, constitution: 59_844, luck: 21_203, health: 51_226_464, minWeaponDmg: 1, maxWeaponDmg: 1),
                    new (position: 654, @class: ClassType.Warrior, level: 427, strength: 59_436, dexterity: 20_843, intelligence: 20_679, constitution: 80_447, luck: 13_211, health: 172_156_576, minWeaponDmg: 1, maxWeaponDmg: 1),
                    new (position: 655, @class: ClassType.Warrior, level: 427, strength: 59_526, dexterity: 20_875, intelligence: 20_711, constitution: 80_570, luck: 13_231, health: 172_419_808, minWeaponDmg: 1, maxWeaponDmg: 1),
                    new (position: 656, @class: ClassType.Mage, level: 427, strength: 14_839, dexterity: 14_970, intelligence: 60_142, constitution: 60_119, luck: 21_300, health: 51_461_864, minWeaponDmg: 1, maxWeaponDmg: 1),
                    new (position: 657, @class: ClassType.Warrior, level: 428, strength: 59_866, dexterity: 20_991, intelligence: 20_827, constitution: 81_037, luck: 13_298, health: 173_824_368, minWeaponDmg: 1, maxWeaponDmg: 1),
                    //

                    new (position: 658, @class: ClassType.Warrior, level: 428, strength: 59957, dexterity: 21023, intelligence: 20859, constitution: 81160, luck: 13318, health: 174088192, minWeaponDmg: 469, maxWeaponDmg: 943 ),
                    
                    // Not confirmed: 
                    new (position: 659, @class: ClassType.Mage, level: 428, strength: 14_946, dexterity: 15_078, intelligence: 60_575, constitution: 60_560, luck: 21_450, health: 51_960_480, minWeaponDmg: 1, maxWeaponDmg: 1),
                    new (position: 660, @class: ClassType.Mage, level: 428, strength: 14_969, dexterity: 15_101, intelligence: 60_667, constitution: 60_651, luck: 21_483, health: 52_038_560, minWeaponDmg: 1, maxWeaponDmg: 1),
                    new (position: 661, @class: ClassType.Scout, level: 429, strength: 15_163, dexterity: 60_389, intelligence: 15_097, constitution: 61_465, luck: 21_608, health: 105_719_800, minWeaponDmg: 1, maxWeaponDmg: 1),
                    new (position: 662, @class: ClassType.Scout, level: 429, strength: 15_186, dexterity: 60_480, intelligence: 15_120, constitution: 61_558, luck: 21_641, health: 105_879_760, minWeaponDmg: 1, maxWeaponDmg: 1),
                    new (position: 663, @class: ClassType.Mage, level: 429, strength: 15_077, dexterity: 15_209, intelligence: 61_102, constitution: 61_094, luck: 21_640, health: 52_540_840, minWeaponDmg: 1, maxWeaponDmg: 1),
                    new (position: 664, @class: ClassType.Mage, level: 429, strength: 15_099, dexterity: 15_232, intelligence: 61_194, constitution: 61_187, luck: 21_673, health: 52_620_820, minWeaponDmg: 1, maxWeaponDmg: 1),
                    new (position: 665, @class: ClassType.Mage, level: 430, strength: 15_162, dexterity: 15_295, intelligence: 61_446, constitution: 61_446, luck: 21_759, health: 52_966_452, minWeaponDmg: 1, maxWeaponDmg: 1),
                    new (position: 666, @class: ClassType.Scout, level: 430, strength: 15_318, dexterity: 61_006, intelligence: 15_251, constitution: 62_098, luck: 21_825, health: 107_056_952, minWeaponDmg: 1, maxWeaponDmg: 1),
                    new (position: 667, @class: ClassType.Scout, level: 430, strength: 15_341, dexterity: 61_097, intelligence: 15_274, constitution: 62_192, luck: 21_858, health: 107_219_008, minWeaponDmg: 1, maxWeaponDmg: 1),
                    new (position: 668, @class: ClassType.Warrior, level: 430, strength: 61_189, dexterity: 21_456, intelligence: 21_289, constitution: 82_836, luck: 13_587, health: 178_511_584, minWeaponDmg: 1, maxWeaponDmg: 1),
                    new (position: 669, @class: ClassType.Scout, level: 431, strength: 15_427, dexterity: 61_441, intelligence: 15_360, constitution: 62_546, luck: 21_977, health: 108_079_488, minWeaponDmg: 1, maxWeaponDmg: 1),
                    new (position: 670, @class: ClassType.Warrior, level: 431, strength: 61_533, dexterity: 21_574, intelligence: 21_406, constitution: 83_308, luck: 13_661, health: 179_945_280, minWeaponDmg: 1, maxWeaponDmg: 1),
                    new (position: 671, @class: ClassType.Scout, level: 431, strength: 15_473, dexterity: 61_625, intelligence: 15_406, constitution: 62_733, luck: 22_042, health: 108_402_624, minWeaponDmg: 1, maxWeaponDmg: 1),
                    new (position: 672, @class: ClassType.Warrior, level: 431, strength: 61_716, dexterity: 21_638, intelligence: 21_470, constitution: 83_557, luck: 13_702, health: 180_483_120, minWeaponDmg: 1, maxWeaponDmg: 1),
                    new (position: 673, @class: ClassType.Warrior, level: 432, strength: 61_970, dexterity: 21_731, intelligence: 21_563, constitution: 83_908, luck: 13_756, health: 181_660_816, minWeaponDmg: 1, maxWeaponDmg: 1),
                    new (position: 674, @class: ClassType.Mage, level: 432, strength: 15_448, dexterity: 15_583, intelligence: 62_601, constitution: 62_618, luck: 22_168, health: 54_227_188, minWeaponDmg: 1, maxWeaponDmg: 1),
                    new (position: 675, @class: ClassType.Warrior, level: 432, strength: 62_154, dexterity: 21_796, intelligence: 21_627, constitution: 84_158, luck: 13_797, health: 182_202_064, minWeaponDmg: 1, maxWeaponDmg: 1),
                    new (position: 676, @class: ClassType.Scout, level: 432, strength: 15_629, dexterity: 62_246, intelligence: 15_562, constitution: 63_371, luck: 22_267, health: 109_758_576, minWeaponDmg: 1, maxWeaponDmg: 1),
                    new (position: 677, @class: ClassType.Scout, level: 433, strength: 15_693, dexterity: 62_501, intelligence: 15_625, constitution: 63_635, luck: 22_355, health: 110_470_360, minWeaponDmg: 1, maxWeaponDmg: 1),
                    new (position: 678, @class: ClassType.Mage, level: 433, strength: 15_580, dexterity: 15_716, intelligence: 63_135, constitution: 63_160, luck: 22_354, health: 54_822_880, minWeaponDmg: 1, maxWeaponDmg: 1),
                    new (position: 679, @class: ClassType.Warrior, level: 433, strength: 62_685, dexterity: 21_979, intelligence: 21_809, constitution: 84_884, luck: 13_906, health: 184_198_272, minWeaponDmg: 1, maxWeaponDmg: 1),
                    new (position: 680, @class: ClassType.Warrior, level: 433, strength: 62_778, dexterity: 22_012, intelligence: 21_842, constitution: 85_009, luck: 13_926, health: 184_469_536, minWeaponDmg: 1, maxWeaponDmg: 1),
                    new (position: 681, @class: ClassType.Scout, level: 434, strength: 15_826, dexterity: 63_033, intelligence: 15_758, constitution: 64_183, luck: 22_548, health: 111_678_416, minWeaponDmg: 1, maxWeaponDmg: 1),
                    new (position: 682, @class: ClassType.Scout, level: 434, strength: 15_850, dexterity: 63_126, intelligence: 15_781, constitution: 64_277, luck: 22_581, health: 111_841_984, minWeaponDmg: 1, maxWeaponDmg: 1),
                    new (position: 683, @class: ClassType.Scout, level: 434, strength: 15_873, dexterity: 63_218, intelligence: 15_805, constitution: 64_371, luck: 22_614, health: 112_005_536, minWeaponDmg: 1, maxWeaponDmg: 1),
                    new (position: 684, @class: ClassType.Scout, level: 434, strength: 15_896, dexterity: 63_311, intelligence: 15_828, constitution: 64_466, luck: 22_647, health: 112_170_840, minWeaponDmg: 1, maxWeaponDmg: 1),
                    new (position: 685, @class: ClassType.Scout, level: 435, strength: 15_960, dexterity: 63_568, intelligence: 15_892, constitution: 64_733, luck: 22_735, health: 112_894_352, minWeaponDmg: 1, maxWeaponDmg: 1),
                    new (position: 686, @class: ClassType.Mage, level: 435, strength: 15_847, dexterity: 15_984, intelligence: 64_210, constitution: 64_251, luck: 22_734, health: 56_026_872, minWeaponDmg: 1, maxWeaponDmg: 1),
                    new (position: 687, @class: ClassType.Warrior, level: 435, strength: 63_754, dexterity: 22_355, intelligence: 22_183, constitution: 86_346, luck: 14_138, health: 188_234_272, minWeaponDmg: 1, maxWeaponDmg: 1),
                    new (position: 688, @class: ClassType.Warrior, level: 435, strength: 63_846, dexterity: 22_388, intelligence: 22_216, constitution: 86_472, luck: 14_159, health: 188_508_960, minWeaponDmg: 1, maxWeaponDmg: 1),
                    new (position: 689, @class: ClassType.Warrior, level: 436, strength: 64_105, dexterity: 22_475, intelligence: 22_303, constitution: 86_829, luck: 14_214, health: 189_721_360, minWeaponDmg: 1, maxWeaponDmg: 1),
                    new (position: 690, @class: ClassType.Mage, level: 436, strength: 15_980, dexterity: 16_118, intelligence: 64_750, constitution: 64_800, luck: 22_922, health: 56_635_200, minWeaponDmg: 1, maxWeaponDmg: 1),
                    new (position: 691, @class: ClassType.Mage, level: 436, strength: 16_004, dexterity: 16_142, intelligence: 64_843, constitution: 64_893, luck: 22_955, health: 56_716_480, minWeaponDmg: 1, maxWeaponDmg: 1),
                    new (position: 692, @class: ClassType.Warrior, level: 436, strength: 64_384, dexterity: 22_573, intelligence: 22_400, constitution: 87_207, luck: 14_276, health: 190_547_296, minWeaponDmg: 1, maxWeaponDmg: 1),
                    new (position: 693, @class: ClassType.Warrior, level: 437, strength: 64_643, dexterity: 22_668, intelligence: 22_495, constitution: 87_566, luck: 14_331, health: 191_769_536, minWeaponDmg: 1, maxWeaponDmg: 1),
                    new (position: 694, @class: ClassType.Warrior, level: 437, strength: 64_736, dexterity: 22_701, intelligence: 22_527, constitution: 87_692, luck: 14_352, health: 192_045_472, minWeaponDmg: 1, maxWeaponDmg: 1),
                    new (position: 695, @class: ClassType.Warrior, level: 437, strength: 64_830, dexterity: 22_733, intelligence: 22_560, constitution: 87_818, luck: 14_373, health: 192_321_424, minWeaponDmg: 1, maxWeaponDmg: 1),
                    new (position: 696, @class: ClassType.Warrior, level: 437, strength: 64_923, dexterity: 22_766, intelligence: 22_592, constitution: 87_945, luck: 14_393, health: 192_599_552, minWeaponDmg: 1, maxWeaponDmg: 1),
                    new (position: 697, @class: ClassType.Mage, level: 438, strength: 16_226, dexterity: 16_366, intelligence: 65_741, constitution: 65_808, luck: 23_273, health: 57_779_424, minWeaponDmg: 1, maxWeaponDmg: 1),
                    new (position: 698, @class: ClassType.Warrior, level: 438, strength: 65_277, dexterity: 22_887, intelligence: 22_713, constitution: 88_432, luck: 14_463, health: 194_108_240, minWeaponDmg: 1, maxWeaponDmg: 1),
                    new (position: 699, @class: ClassType.Warrior, level: 438, strength: 65_370, dexterity: 22_920, intelligence: 22_745, constitution: 88_558, luck: 14_483, health: 194_384_816, minWeaponDmg: 1, maxWeaponDmg: 1),
                    new (position: 700, @class: ClassType.Mage, level: 438, strength: 16296, dexterity: 16436, intelligence: 66024, constitution: 66091, luck: 23373, health: 58_027_896, minWeaponDmg: 1084, maxWeaponDmg: 2168 ),
                    new (position: 701, @class: ClassType.Warrior, level: 439, strength: 65_726, dexterity: 23_049, intelligence: 22_874, constitution: 89_047, luck: 14_560, health: 195_903_392, minWeaponDmg: 1, maxWeaponDmg: 1),
                    new (position: 702, @class: ClassType.Warrior, level: 439, strength: 65_820, dexterity: 23_082, intelligence: 22_906, constitution: 89_174, luck: 14_581, health: 196_182_800, minWeaponDmg: 1, maxWeaponDmg: 1),
                    new (position: 703, @class: ClassType.Warrior, level: 439, strength: 65_913, dexterity: 23_115, intelligence: 22_939, constitution: 89_301, luck: 14_601, health: 196_462_208, minWeaponDmg: 1, maxWeaponDmg: 1),
                    new (position: 704, @class: ClassType.Mage, level: 439, strength: 16_431, dexterity: 16_572, intelligence: 66_570, constitution: 66_647, luck: 23_570, health: 58_649_360, minWeaponDmg: 1, maxWeaponDmg: 1),
                    new (position: 705, @class: ClassType.Warrior, level: 440, strength: 66_270, dexterity: 23_237, intelligence: 23_061, constitution: 89_792, luck: 14_678, health: 197_991_360, minWeaponDmg: 1, maxWeaponDmg: 1),
                    new (position: 706, @class: ClassType.Warrior, level: 440, strength: 66_364, dexterity: 23_270, intelligence: 23_093, constitution: 89_920, luck: 14_699, health: 198_273_600, minWeaponDmg: 1, maxWeaponDmg: 1),
                    new (position: 707, @class: ClassType.Mage, level: 440, strength: 16_544, dexterity: 16_685, intelligence: 67_024, constitution: 67_109, luck: 23_727, health: 59_190_136, minWeaponDmg: 1, maxWeaponDmg: 1),
                    new (position: 708, @class: ClassType.Warrior, level: 440, strength: 66_552, dexterity: 23_336, intelligence: 23_159, constitution: 90_174, luck: 14_741, health: 198_833_664, minWeaponDmg: 1, maxWeaponDmg: 1),
                    new (position: 709, @class: ClassType.Warrior, level: 441, strength: 66_816, dexterity: 23_425, intelligence: 23_248, constitution: 90_540, luck: 14_797, health: 200_093_408, minWeaponDmg: 1, maxWeaponDmg: 1),
                    new (position: 710, @class: ClassType.Warrior, level: 441, strength: 66_910, dexterity: 23_458, intelligence: 23_281, constitution: 90_668, luck: 14_818, health: 200_376_288, minWeaponDmg: 1, maxWeaponDmg: 1),
                    //

                    new (position: 711, @class: ClassType.Warrior, level: 441, strength: 67005, dexterity: 23491, intelligence: 23314, constitution: 90796, luck: 14839, health: 200659168, minWeaponDmg: 483, maxWeaponDmg: 990 ),

                    // Not confirmed: 
                    new (position: 712, @class: ClassType.Mage, level: 441, strength: 16_704, dexterity: 16_846, intelligence: 67_668, constitution: 67_763, luck: 23_952, health: 59_902_492, minWeaponDmg: 1, maxWeaponDmg: 1),
                    new (position: 713, @class: ClassType.Warrior, level: 442, strength: 67_364, dexterity: 23_622, intelligence: 23_443, constitution: 91_283, luck: 14_916, health: 202_191_840, minWeaponDmg: 1, maxWeaponDmg: 1),
                    new (position: 714, @class: ClassType.Mage, level: 442, strength: 16_793, dexterity: 16_936, intelligence: 68_030, constitution: 68_132, luck: 24_083, health: 60_364_952, minWeaponDmg: 1, maxWeaponDmg: 1),
                    new (position: 715, @class: ClassType.Scout, level: 442, strength: 16_960, dexterity: 67_553, intelligence: 16_888, constitution: 68_829, luck: 24_153, health: 121_964_992, minWeaponDmg: 1, maxWeaponDmg: 1),
                    new (position: 716, @class: ClassType.Warrior, level: 442, strength: 67_648, dexterity: 23_721, intelligence: 23_542, constitution: 91_667, luck: 14_979, health: 203_042_400, minWeaponDmg: 1, maxWeaponDmg: 1),
                    new (position: 717, @class: ClassType.Mage, level: 443, strength: 16_907, dexterity: 17_050, intelligence: 68_488, constitution: 68_600, luck: 24_242, health: 60_916_800, minWeaponDmg: 1, maxWeaponDmg: 1),
                    new (position: 718, @class: ClassType.Warrior, level: 443, strength: 68_009, dexterity: 23_845, intelligence: 23_665, constitution: 92_165, luck: 15_049, health: 204_606_304, minWeaponDmg: 1, maxWeaponDmg: 1),
                    new (position: 719, @class: ClassType.Warrior, level: 443, strength: 68_104, dexterity: 23_878, intelligence: 23_698, constitution: 92_293, luck: 15_070, health: 204_890_464, minWeaponDmg: 1, maxWeaponDmg: 1),
                    new (position: 720, @class: ClassType.Mage, level: 443, strength: 16_978, dexterity: 17_122, intelligence: 68_774, constitution: 68_886, luck: 24_343, health: 61_170_768, minWeaponDmg: 1, maxWeaponDmg: 1),
                    new (position: 721, @class: ClassType.Mage, level: 444, strength: 17_044, dexterity: 17_189, intelligence: 69_043, constitution: 69_165, luck: 24_442, health: 61_556_848, minWeaponDmg: 1, maxWeaponDmg: 1),
                    new (position: 722, @class: ClassType.Scout, level: 444, strength: 17_212, dexterity: 68_561, intelligence: 17_140, constitution: 69_867, luck: 24_512, health: 124_363_264, minWeaponDmg: 1, maxWeaponDmg: 1),
                    new (position: 723, @class: ClassType.Mage, level: 444, strength: 17_092, dexterity: 17_236, intelligence: 69_234, constitution: 69_356, luck: 24_510, health: 61_726_840, minWeaponDmg: 1, maxWeaponDmg: 1),
                    new (position: 724, @class: ClassType.Scout, level: 444, strength: 17_260, dexterity: 68_751, intelligence: 17_188, constitution: 70_060, luck: 24_580, health: 124_706_800, minWeaponDmg: 1, maxWeaponDmg: 1),
                    new (position: 725, @class: ClassType.Scout, level: 445, strength: 17_328, dexterity: 69_020, intelligence: 17_255, constitution: 70_340, luck: 24_672, health: 125_486_560, minWeaponDmg: 1, maxWeaponDmg: 1),
                    new (position: 726, @class: ClassType.Mage, level: 445, strength: 17_206, dexterity: 17_351, intelligence: 69_696, constitution: 69_827, luck: 24_669, health: 62_285_684, minWeaponDmg: 1, maxWeaponDmg: 1),
                    new (position: 727, @class: ClassType.Scout, level: 445, strength: 17_375, dexterity: 69_210, intelligence: 17_303, constitution: 70_534, luck: 24_740, health: 125_832_656, minWeaponDmg: 1, maxWeaponDmg: 1),
                    new (position: 728, @class: ClassType.Warrior, level: 445, strength: 69_306, dexterity: 24_301, intelligence: 24_119, constitution: 93_937, luck: 15_332, health: 209_479_504, minWeaponDmg: 1, maxWeaponDmg: 1),
                    new (position: 729, @class: ClassType.Warrior, level: 446, strength: 69_576, dexterity: 24_392, intelligence: 24_210, constitution: 94_311, luck: 15_389, health: 210_785_088, minWeaponDmg: 1, maxWeaponDmg: 1),
                    new (position: 730, @class: ClassType.Warrior, level: 446, strength: 69_671, dexterity: 24_426, intelligence: 24_243, constitution: 94_440, luck: 15_410, health: 211_073_408, minWeaponDmg: 1, maxWeaponDmg: 1),
                    new (position: 731, @class: ClassType.Mage, level: 446, strength: 17_369, dexterity: 17_515, intelligence: 70_351, constitution: 70_492, luck: 24_898, health: 63_019_848, minWeaponDmg: 1, maxWeaponDmg: 1),
                    new (position: 732, @class: ClassType.Scout, level: 446, strength: 17_539, dexterity: 69_862, intelligence: 17_466, constitution: 71_203, luck: 24_969, health: 127_310_960, minWeaponDmg: 1, maxWeaponDmg: 1),
                    new (position: 733, @class: ClassType.Scout, level: 447, strength: 17_607, dexterity: 70_133, intelligence: 17_533, constitution: 71_485, luck: 25_069, health: 128_101_120, minWeaponDmg: 1, maxWeaponDmg: 1),
                    new (position: 734, @class: ClassType.Scout, level: 447, strength: 17_631, dexterity: 70_229, intelligence: 17_557, constitution: 71_583, luck: 25_103, health: 128_276_736, minWeaponDmg: 1, maxWeaponDmg: 1),
                    new (position: 735, @class: ClassType.Mage, level: 447, strength: 17_508, dexterity: 17_655, intelligence: 70_913, constitution: 71_063, luck: 25_100, health: 63_672_448, minWeaponDmg: 1, maxWeaponDmg: 1),
                    new (position: 736, @class: ClassType.Warrior, level: 447, strength: 70_420, dexterity: 24_693, intelligence: 24_509, constitution: 95_464, luck: 15_574, health: 213_839_360, minWeaponDmg: 1, maxWeaponDmg: 1),
                    new (position: 737, @class: ClassType.Scout, level: 448, strength: 17_747, dexterity: 70_693, intelligence: 17_673, constitution: 72_060, luck: 25_264, health: 129_419_760, minWeaponDmg: 1, maxWeaponDmg: 1),
                    new (position: 738, @class: ClassType.Mage, level: 448, strength: 17_623, dexterity: 17_771, intelligence: 71_379, constitution: 71_539, luck: 25_262, health: 64_242_024, minWeaponDmg: 1, maxWeaponDmg: 1),
                    new (position: 739, @class: ClassType.Warrior, level: 448, strength: 70_885, dexterity: 24_853, intelligence: 24_668, constitution: 96_101, luck: 15_667, health: 215_746_752, minWeaponDmg: 1, maxWeaponDmg: 1),
                    new (position: 740, @class: ClassType.Scout, level: 448, strength: 17_819, dexterity: 70_981, intelligence: 17_745, constitution: 72_354, luck: 25_367, health: 129_947_784, minWeaponDmg: 1, maxWeaponDmg: 1),
                    new (position: 741, @class: ClassType.Scout, level: 449, strength: 17_888, dexterity: 71_255, intelligence: 17_814, constitution: 72_639, luck: 25_468, health: 130_750_200, minWeaponDmg: 1, maxWeaponDmg: 1),
                    new (position: 742, @class: ClassType.Mage, level: 449, strength: 17_763, dexterity: 17_912, intelligence: 71_944, constitution: 72_114, luck: 25_465, health: 64_902_600, minWeaponDmg: 1, maxWeaponDmg: 1),
                    new (position: 743, @class: ClassType.Warrior, level: 449, strength: 71_447, dexterity: 25_054, intelligence: 24_868, constitution: 96_871, luck: 15_789, health: 217_959_744, minWeaponDmg: 1, maxWeaponDmg: 1),
                    new (position: 744, @class: ClassType.Scout, level: 449, strength: 17_960, dexterity: 71_543, intelligence: 17_886, constitution: 72_933, luck: 25_571, health: 131_279_400, minWeaponDmg: 1, maxWeaponDmg: 1),
                    new (position: 745, @class: ClassType.Scout, level: 450, strength: 18_029, dexterity: 71_818, intelligence: 17_955, constitution: 73_219, luck: 25_665, health: 132_087_072, minWeaponDmg: 1, maxWeaponDmg: 1),
                    new (position: 746, @class: ClassType.Warrior, level: 450, strength: 71_914, dexterity: 25_215, intelligence: 25_028, constitution: 97_514, luck: 15_890, health: 219_894_064, minWeaponDmg: 1, maxWeaponDmg: 1),
                    new (position: 747, @class: ClassType.Warrior, level: 450, strength: 72_011, dexterity: 25_249, intelligence: 25_062, constitution: 97_644, luck: 15_911, health: 220_187_216, minWeaponDmg: 1, maxWeaponDmg: 1),
                    new (position: 748, @class: ClassType.Scout, level: 450, strength: 18_102, dexterity: 72_107, intelligence: 18_027, constitution: 73_514, luck: 25_769, health: 132_619_256, minWeaponDmg: 1, maxWeaponDmg: 1),
                    new (position: 749, @class: ClassType.Warrior, level: 450, strength: 72_204, dexterity: 25_316, intelligence: 25_129, constitution: 97_905, luck: 15_954, health: 220_775_776, minWeaponDmg: 1, maxWeaponDmg: 1),
                    new (position: 750, @class: ClassType.Warrior, level: 451, strength: 72480, dexterity: 25410, intelligence: 25222, constitution: 98288, luck: 16012, health: 222_130_880, minWeaponDmg: 496, maxWeaponDmg: 992 ),
                    new (position: 751, @class: ClassType.Warrior, level: 451, strength: 72_577, dexterity: 25_444, intelligence: 25_256, constitution: 98_419, luck: 16_034, health: 222_426_944, minWeaponDmg: 1, maxWeaponDmg: 1),
                    new (position: 752, @class: ClassType.Scout, level: 451, strength: 18_244, dexterity: 72_673, intelligence: 18_168, constitution: 74_096, luck: 25_967, health: 133_965_568, minWeaponDmg: 1, maxWeaponDmg: 1),
                    new (position: 753, @class: ClassType.Mage, level: 451, strength: 18_117, dexterity: 18_268, intelligence: 73_372, constitution: 73_562, luck: 25_963, health: 66_500_048, minWeaponDmg: 1, maxWeaponDmg: 1),
                    new (position: 754, @class: ClassType.Scout, level: 451, strength: 18_292, dexterity: 72_867, intelligence: 18_217, constitution: 74_293, luck: 26_036, health: 134_321_744, minWeaponDmg: 1, maxWeaponDmg: 1),
                    new (position: 755, @class: ClassType.Scout, level: 452, strength: 18_362, dexterity: 73_144, intelligence: 18_286, constitution: 74_582, luck: 26_138, health: 135_142_592, minWeaponDmg: 1, maxWeaponDmg: 1),
                    new (position: 756, @class: ClassType.Mage, level: 452, strength: 18_235, dexterity: 18_386, intelligence: 73_846, constitution: 74_046, luck: 26_135, health: 67_085_676, minWeaponDmg: 1, maxWeaponDmg: 1),
                    new (position: 757, @class: ClassType.Scout, level: 452, strength: 18_410, dexterity: 73_338, intelligence: 18_335, constitution: 74_780, luck: 26_207, health: 135_501_360, minWeaponDmg: 1, maxWeaponDmg: 1),
                    new (position: 758, @class: ClassType.Scout, level: 452, strength: 18_435, dexterity: 73_435, intelligence: 18_359, constitution: 74_879, luck: 26_242, health: 135_680_752, minWeaponDmg: 1, maxWeaponDmg: 1),
                    new (position: 759, @class: ClassType.Mage, level: 452, strength: 18_307, dexterity: 18_459, intelligence: 74_139, constitution: 74_340, luck: 26_239, health: 67_352_040, minWeaponDmg: 1, maxWeaponDmg: 1),
                    new (position: 760, @class: ClassType.Mage, level: 453, strength: 18_377, dexterity: 18_529, intelligence: 74_419, constitution: 74_629, luck: 26_334, health: 67_763_136, minWeaponDmg: 1, maxWeaponDmg: 1),
                    new (position: 761, @class: ClassType.Warrior, level: 453, strength: 73_908, dexterity: 25_912, intelligence: 25_722, constitution: 100_240, luck: 16_316, health: 227_544_800, minWeaponDmg: 1, maxWeaponDmg: 1),
                    new (position: 762, @class: ClassType.Mage, level: 453, strength: 18_425, dexterity: 18_578, intelligence: 74_615, constitution: 74_825, luck: 26_403, health: 67_941_104, minWeaponDmg: 1, maxWeaponDmg: 1),
                    new (position: 763, @class: ClassType.Warrior, level: 453, strength: 74_103, dexterity: 25_980, intelligence: 25_789, constitution: 100_504, luck: 16_359, health: 228_144_080, minWeaponDmg: 1, maxWeaponDmg: 1),
                    new (position: 764, @class: ClassType.Mage, level: 453, strength: 18_474, dexterity: 18_626, intelligence: 74_811, constitution: 75_021, luck: 26_473, health: 68_119_072, minWeaponDmg: 1, maxWeaponDmg: 1),
                    new (position: 765, @class: ClassType.Warrior, level: 454, strength: 74_480, dexterity: 26_117, intelligence: 25_926, constitution: 101_016, luck: 16_440, health: 229_811_392, minWeaponDmg: 1, maxWeaponDmg: 1),
                    new (position: 766, @class: ClassType.Warrior, level: 454, strength: 74_578, dexterity: 26_151, intelligence: 25_960, constitution: 101_149, luck: 16_461, health: 230_113_968, minWeaponDmg: 1, maxWeaponDmg: 1),
                    new (position: 767, @class: ClassType.Scout, level: 454, strength: 18_745, dexterity: 74_675, intelligence: 18_669, constitution: 76_154, luck: 26_684, health: 138_600_288, minWeaponDmg: 1, maxWeaponDmg: 1),
                    new (position: 768, @class: ClassType.Warrior, level: 454, strength: 74_772, dexterity: 26_220, intelligence: 26_028, constitution: 101_413, luck: 16_504, health: 230_714_576, minWeaponDmg: 1, maxWeaponDmg: 1),
                    new (position: 769, @class: ClassType.Warrior, level: 454, strength: 74_870, dexterity: 26_254, intelligence: 26_061, constitution: 101_544, luck: 16_526, health: 231_012_608, minWeaponDmg: 1, maxWeaponDmg: 1),
                    new (position: 770, @class: ClassType.Scout, level: 455, strength: 18_865, dexterity: 75_152, intelligence: 18_788, constitution: 76_646, luck: 26_850, health: 139_802_304, minWeaponDmg: 1, maxWeaponDmg: 1),
                    new (position: 771, @class: ClassType.Mage, level: 455, strength: 18_735, dexterity: 18_889, intelligence: 75_866, constitution: 76_098, luck: 26_846, health: 69_401_376, minWeaponDmg: 1, maxWeaponDmg: 1),
                    new (position: 772, @class: ClassType.Scout, level: 455, strength: 18_914, dexterity: 75_347, intelligence: 18_837, constitution: 76_845, luck: 26_920, health: 140_165_280, minWeaponDmg: 1, maxWeaponDmg: 1),
                    new (position: 773, @class: ClassType.Mage, level: 455, strength: 18_784, dexterity: 18_939, intelligence: 76_063, constitution: 76_295, luck: 26_916, health: 69_581_040, minWeaponDmg: 1, maxWeaponDmg: 1),
                    new (position: 774, @class: ClassType.Scout, level: 455, strength: 18_963, dexterity: 75_542, intelligence: 18_886, constitution: 77_044, luck: 26_989, health: 140_528_256, minWeaponDmg: 1, maxWeaponDmg: 1),
                    new (position: 775, @class: ClassType.Mage, level: 456, strength: 18_879, dexterity: 19_034, intelligence: 76_446, constitution: 76_688, luck: 27_048, health: 70_092_832, minWeaponDmg: 1, maxWeaponDmg: 1),
                    new (position: 776, @class: ClassType.Scout, level: 456, strength: 19_059, dexterity: 75_924, intelligence: 18_981, constitution: 77_439, luck: 27_121, health: 141_558_496, minWeaponDmg: 1, maxWeaponDmg: 1),
                    //

                    new (position: 777, @class: ClassType.Warrior, level: 456, strength: 76022, dexterity: 26651, intelligence: 26457, constitution: 103124, luck: 16775, health: 235638336, minWeaponDmg: 503, maxWeaponDmg: 1044 ),

                    // Not confirmed: 
                    new (position: 778, @class: ClassType.Mage, level: 456, strength: 18_952, dexterity: 19_108, intelligence: 76_742, constitution: 76_985, luck: 27_152, health: 70_364_288, minWeaponDmg: 1, maxWeaponDmg: 1),
                    new (position: 779, @class: ClassType.Mage, level: 456, strength: 18_976, dexterity: 19_132, intelligence: 76_841, constitution: 77_084, luck: 27_187, health: 70_454_776, minWeaponDmg: 1, maxWeaponDmg: 1),
                    new (position: 780, @class: ClassType.Warrior, level: 457, strength: 76_502, dexterity: 26_824, intelligence: 26_629, constitution: 103_784, luck: 16_879, health: 237_665_360, minWeaponDmg: 1, maxWeaponDmg: 1),
                    new (position: 781, @class: ClassType.Mage, level: 457, strength: 19_072, dexterity: 19_228, intelligence: 77_225, constitution: 77_478, luck: 27_327, health: 70_969_848, minWeaponDmg: 1, maxWeaponDmg: 1),
                    new (position: 782, @class: ClassType.Scout, level: 457, strength: 19_253, dexterity: 76_699, intelligence: 19_175, constitution: 78_234, luck: 27_401, health: 143_324_688, minWeaponDmg: 1, maxWeaponDmg: 1),
                    new (position: 783, @class: ClassType.Scout, level: 457, strength: 19_277, dexterity: 76_797, intelligence: 19_199, constitution: 78_334, luck: 27_436, health: 143_507_888, minWeaponDmg: 1, maxWeaponDmg: 1),
                    new (position: 784, @class: ClassType.Mage, level: 457, strength: 19_145, dexterity: 19_302, intelligence: 77_522, constitution: 77_776, luck: 27_432, health: 71_242_816, minWeaponDmg: 1, maxWeaponDmg: 1),
                    new (position: 785, @class: ClassType.Scout, level: 458, strength: 19_374, dexterity: 77_181, intelligence: 19_295, constitution: 78_732, luck: 27_569, health: 144_551_952, minWeaponDmg: 1, maxWeaponDmg: 1),
                    new (position: 786, @class: ClassType.Mage, level: 458, strength: 19_241, dexterity: 19_398, intelligence: 77_908, constitution: 78_173, luck: 27_565, health: 71_762_816, minWeaponDmg: 1, maxWeaponDmg: 1),
                    new (position: 787, @class: ClassType.Warrior, level: 458, strength: 77_378, dexterity: 27_128, intelligence: 26_931, constitution: 104_979, luck: 17_062, health: 240_926_800, minWeaponDmg: 1, maxWeaponDmg: 1),
                    new (position: 788, @class: ClassType.Mage, level: 458, strength: 19_290, dexterity: 19_448, intelligence: 78_107, constitution: 78_371, luck: 27_635, health: 71_944_576, minWeaponDmg: 1, maxWeaponDmg: 1),
                    new (position: 789, @class: ClassType.Warrior, level: 458, strength: 77_574, dexterity: 27_197, intelligence: 27_000, constitution: 105_247, luck: 17_106, health: 241_541_872, minWeaponDmg: 1, maxWeaponDmg: 1),
                    new (position: 790, @class: ClassType.Mage, level: 459, strength: 19_387, dexterity: 19_545, intelligence: 78_494, constitution: 78_769, luck: 27_776, health: 72_467_480, minWeaponDmg: 1, maxWeaponDmg: 1),
                    new (position: 791, @class: ClassType.Scout, level: 459, strength: 19_569, dexterity: 77_961, intelligence: 19_490, constitution: 79_533, luck: 27_851, health: 146_340_720, minWeaponDmg: 1, maxWeaponDmg: 1),
                    new (position: 792, @class: ClassType.Scout, level: 459, strength: 19_594, dexterity: 78_060, intelligence: 19_515, constitution: 79_634, luck: 27_886, health: 146_526_560, minWeaponDmg: 1, maxWeaponDmg: 1),
                    new (position: 793, @class: ClassType.Warrior, level: 459, strength: 78_158, dexterity: 27_406, intelligence: 27_208, constitution: 106_046, luck: 17_232, health: 243_905_792, minWeaponDmg: 1, maxWeaponDmg: 1),
                    new (position: 794, @class: ClassType.Mage, level: 459, strength: 19_485, dexterity: 19_644, intelligence: 78_892, constitution: 79_168, luck: 27_917, health: 72_834_560, minWeaponDmg: 1, maxWeaponDmg: 1),
                    new (position: 795, @class: ClassType.Mage, level: 460, strength: 19_557, dexterity: 19_716, intelligence: 79_182, constitution: 79_468, luck: 28_016, health: 73_269_496, minWeaponDmg: 1, maxWeaponDmg: 1),
                    new (position: 796, @class: ClassType.Scout, level: 460, strength: 19_741, dexterity: 78_645, intelligence: 19_661, constitution: 80_237, luck: 28_091, health: 147_957_024, minWeaponDmg: 1, maxWeaponDmg: 1),
                    new (position: 797, @class: ClassType.Scout, level: 460, strength: 19_766, dexterity: 78_744, intelligence: 19_686, constitution: 80_338, luck: 28_126, health: 148_143_264, minWeaponDmg: 1, maxWeaponDmg: 1),
                    new (position: 798, @class: ClassType.Mage, level: 460, strength: 19_631, dexterity: 19_790, intelligence: 79_481, constitution: 79_769, luck: 28_122, health: 73_547_016, minWeaponDmg: 1, maxWeaponDmg: 1),
                    new (position: 799, @class: ClassType.Warrior, level: 460, strength: 78_941, dexterity: 27_677, intelligence: 27_478, constitution: 107_117, luck: 17_402, health: 246_904_688, minWeaponDmg: 1, maxWeaponDmg: 1),
                    new (position: 800, @class: ClassType.Warrior, level: 461, strength: 79232, dexterity: 27776, intelligence: 27576, constitution: 107520, luck: 17464, health: 248_371_200, minWeaponDmg: 507, maxWeaponDmg: 1014 ),
                    new (position: 801, @class: ClassType.Scout, level: 461, strength: 19_913, dexterity: 79_331, intelligence: 19_833, constitution: 80_942, luck: 28_331, health: 149_580_816, minWeaponDmg: 1, maxWeaponDmg: 1),
                    new (position: 802, @class: ClassType.Warrior, level: 461, strength: 79_430, dexterity: 27_845, intelligence: 27_645, constitution: 107_789, luck: 17_508, health: 248_992_592, minWeaponDmg: 1, maxWeaponDmg: 1),
                    new (position: 803, @class: ClassType.Mage, level: 461, strength: 19_802, dexterity: 19_963, intelligence: 80_172, constitution: 80_470, luck: 28_362, health: 74_354_280, minWeaponDmg: 1, maxWeaponDmg: 1),
                    new (position: 804, @class: ClassType.Mage, level: 461, strength: 19_827, dexterity: 19_987, intelligence: 80_271, constitution: 80_571, luck: 28_397, health: 74_447_600, minWeaponDmg: 1, maxWeaponDmg: 1),
                    new (position: 805, @class: ClassType.Mage, level: 462, strength: 19_900, dexterity: 20_061, intelligence: 80_564, constitution: 80_873, luck: 28_505, health: 74_888_400, minWeaponDmg: 1, maxWeaponDmg: 1),
                    new (position: 806, @class: ClassType.Mage, level: 462, strength: 19_924, dexterity: 20_086, intelligence: 80_664, constitution: 80_974, luck: 28_540, health: 74_981_920, minWeaponDmg: 1, maxWeaponDmg: 1),
                    new (position: 807, @class: ClassType.Mage, level: 462, strength: 19_949, dexterity: 20_110, intelligence: 80_765, constitution: 81_075, luck: 28_576, health: 75_075_448, minWeaponDmg: 1, maxWeaponDmg: 1),
                    new (position: 808, @class: ClassType.Scout, level: 462, strength: 20_135, dexterity: 80_218, intelligence: 20_055, constitution: 81_854, luck: 28_652, health: 151_593_600, minWeaponDmg: 1, maxWeaponDmg: 1),
                    new (position: 809, @class: ClassType.Mage, level: 462, strength: 19_998, dexterity: 20_160, intelligence: 80_965, constitution: 81_275, luck: 28_647, health: 75_260_648, minWeaponDmg: 1, maxWeaponDmg: 1),
                    new (position: 810, @class: ClassType.Mage, level: 463, strength: 20_072, dexterity: 20_234, intelligence: 81_259, constitution: 81_580, luck: 28_747, health: 75_706_240, minWeaponDmg: 1, maxWeaponDmg: 1),
                    new (position: 811, @class: ClassType.Mage, level: 463, strength: 20_097, dexterity: 20_259, intelligence: 81_360, constitution: 81_681, luck: 28_782, health: 75_799_968, minWeaponDmg: 1, maxWeaponDmg: 1),
                    new (position: 812, @class: ClassType.Mage, level: 463, strength: 20_121, dexterity: 20_284, intelligence: 81_460, constitution: 81_781, luck: 28_818, health: 75_892_768, minWeaponDmg: 1, maxWeaponDmg: 1),
                    new (position: 813, @class: ClassType.Warrior, level: 463, strength: 80_910, dexterity: 28_366, intelligence: 28_162, constitution: 109_813, luck: 17_821, health: 254_766_160, minWeaponDmg: 1, maxWeaponDmg: 1),
                    new (position: 814, @class: ClassType.Scout, level: 463, strength: 20_334, dexterity: 81_009, intelligence: 20_252, constitution: 82_666, luck: 28_930, health: 153_428_096, minWeaponDmg: 1, maxWeaponDmg: 1),
                    new (position: 815, @class: ClassType.Scout, level: 464, strength: 20_408, dexterity: 81_304, intelligence: 20_326, constitution: 82_973, luck: 29_038, health: 154_329_776, minWeaponDmg: 1, maxWeaponDmg: 1),
                    new (position: 816, @class: ClassType.Scout, level: 464, strength: 20_433, dexterity: 81_404, intelligence: 20_351, constitution: 83_075, luck: 29_074, health: 154_519_504, minWeaponDmg: 1, maxWeaponDmg: 1),
                    new (position: 817, @class: ClassType.Scout, level: 464, strength: 20_458, dexterity: 81_504, intelligence: 20_376, constitution: 83_177, luck: 29_110, health: 154_709_216, minWeaponDmg: 1, maxWeaponDmg: 1),
                    new (position: 818, @class: ClassType.Scout, level: 464, strength: 20_483, dexterity: 81_604, intelligence: 20_401, constitution: 83_279, luck: 29_145, health: 154_898_944, minWeaponDmg: 1, maxWeaponDmg: 1),
                    new (position: 819, @class: ClassType.Warrior, level: 464, strength: 81_703, dexterity: 28_649, intelligence: 28_444, constitution: 110_899, luck: 17_993, health: 257_840_176, minWeaponDmg: 1, maxWeaponDmg: 1),
                    new (position: 820, @class: ClassType.Mage, level: 465, strength: 20_418, dexterity: 20_582, intelligence: 82_656, constitution: 83_000, luck: 29_241, health: 77_356_000, minWeaponDmg: 1, maxWeaponDmg: 1),
                    new (position: 821, @class: ClassType.Scout, level: 465, strength: 20_607, dexterity: 82_100, intelligence: 20_525, constitution: 83_791, luck: 29_318, health: 156_186_432, minWeaponDmg: 1, maxWeaponDmg: 1),
                    new (position: 822, @class: ClassType.Warrior, level: 465, strength: 82_200, dexterity: 28_819, intelligence: 28_614, constitution: 111_581, luck: 18_100, health: 259_983_728, minWeaponDmg: 1, maxWeaponDmg: 1),
                    new (position: 823, @class: ClassType.Mage, level: 465, strength: 20_493, dexterity: 20_657, intelligence: 82_958, constitution: 83_304, luck: 29_348, health: 77_639_328, minWeaponDmg: 1, maxWeaponDmg: 1),
                    new (position: 824, @class: ClassType.Warrior, level: 465, strength: 82_400, dexterity: 28_889, intelligence: 28_683, constitution: 111_853, luck: 18_144, health: 260_617_488, minWeaponDmg: 1, maxWeaponDmg: 1),
                    new (position: 825, @class: ClassType.Warrior, level: 466, strength: 82_698, dexterity: 28_990, intelligence: 28_784, constitution: 112_266, luck: 18_208, health: 262_141_104, minWeaponDmg: 1, maxWeaponDmg: 1),
                    new (position: 826, @class: ClassType.Warrior, level: 466, strength: 82_798, dexterity: 29_026, intelligence: 28_819, constitution: 112_403, luck: 18_230, health: 262_461_008, minWeaponDmg: 1, maxWeaponDmg: 1),
                    new (position: 827, @class: ClassType.Mage, level: 466, strength: 20_642, dexterity: 20_807, intelligence: 83_560, constitution: 83_917, luck: 29_557, health: 78_378_480, minWeaponDmg: 1, maxWeaponDmg: 1),
                    new (position: 828, @class: ClassType.Mage, level: 466, strength: 20_667, dexterity: 20_832, intelligence: 83_661, constitution: 84_019, luck: 29_593, health: 78_473_744, minWeaponDmg: 1, maxWeaponDmg: 1),
                    new (position: 829, @class: ClassType.Mage, level: 466, strength: 20_692, dexterity: 20_858, intelligence: 83_762, constitution: 84_121, luck: 29_628, health: 78_569_016, minWeaponDmg: 1, maxWeaponDmg: 1),
                    //

                    new (position: 830, @class: ClassType.Warrior, level: 467, strength: 83398, dexterity: 29241, intelligence: 29033, constitution: 113216, luck: 18360, health: 264925440, minWeaponDmg: 519, maxWeaponDmg: 1082 ),

                    // Not confirmed:
                    new (position: 831, @class: ClassType.Scout, level: 467, strength: 20_958, dexterity: 83_499, intelligence: 20_875, constitution: 85_231, luck: 29_816, health: 159_552_432, minWeaponDmg: 1, maxWeaponDmg: 1),
                    new (position: 832, @class: ClassType.Mage, level: 467, strength: 20_817, dexterity: 20_983, intelligence: 84_265, constitution: 84_634, luck: 29_811, health: 79_217_424, minWeaponDmg: 1, maxWeaponDmg: 1),
                    new (position: 833, @class: ClassType.Mage, level: 467, strength: 20_842, dexterity: 21_008, intelligence: 84_366, constitution: 84_736, luck: 29_846, health: 79_312_896, minWeaponDmg: 1, maxWeaponDmg: 1),
                    new (position: 834, @class: ClassType.Warrior, level: 467, strength: 83_800, dexterity: 29_382, intelligence: 29_173, constitution: 113_762, luck: 18_448, health: 266_203_072, minWeaponDmg: 1, maxWeaponDmg: 1),
                    new (position: 835, @class: ClassType.Warrior, level: 468, strength: 84_101, dexterity: 29_484, intelligence: 29_275, constitution: 114_179, luck: 18_504, health: 267_749_760, minWeaponDmg: 1, maxWeaponDmg: 1),
                    new (position: 836, @class: ClassType.Scout, level: 468, strength: 21_134, dexterity: 84_202, intelligence: 21_050, constitution: 85_954, luck: 30_063, health: 161_249_696, minWeaponDmg: 1, maxWeaponDmg: 1),
                    new (position: 837, @class: ClassType.Scout, level: 468, strength: 21_159, dexterity: 84_303, intelligence: 21_076, constitution: 86_057, luck: 30_099, health: 161_442_928, minWeaponDmg: 1, maxWeaponDmg: 1),
                    new (position: 838, @class: ClassType.Mage, level: 468, strength: 21_017, dexterity: 21_185, intelligence: 85_074, constitution: 85_456, luck: 30_093, health: 80_157_728, minWeaponDmg: 1, maxWeaponDmg: 1),
                    new (position: 839, @class: ClassType.Mage, level: 468, strength: 21_042, dexterity: 21_210, intelligence: 85_175, constitution: 85_558, luck: 30_128, health: 80_253_408, minWeaponDmg: 1, maxWeaponDmg: 1),
                    new (position: 840, @class: ClassType.Scout, level: 469, strength: 21_286, dexterity: 84_806, intelligence: 21_202, constitution: 86_577, luck: 30_282, health: 162_764_768, minWeaponDmg: 1, maxWeaponDmg: 1),
                    new (position: 841, @class: ClassType.Mage, level: 469, strength: 21_143, dexterity: 21_311, intelligence: 85_580, constitution: 85_974, luck: 30_276, health: 80_815_560, minWeaponDmg: 1, maxWeaponDmg: 1),
                    new (position: 842, @class: ClassType.Warrior, level: 469, strength: 85_008, dexterity: 29_807, intelligence: 29_596, constitution: 115_419, luck: 18_701, health: 271_234_656, minWeaponDmg: 1, maxWeaponDmg: 1),
                    new (position: 843, @class: ClassType.Scout, level: 469, strength: 21_362, dexterity: 85_109, intelligence: 21_277, constitution: 86_886, luck: 30_390, health: 163_345_680, minWeaponDmg: 1, maxWeaponDmg: 1),
                    new (position: 844, @class: ClassType.Scout, level: 469, strength: 21_387, dexterity: 85_210, intelligence: 21_303, constitution: 86_989, luck: 30_426, health: 163_539_328, minWeaponDmg: 1, maxWeaponDmg: 1),
                    new (position: 845, @class: ClassType.Mage, level: 470, strength: 21_294, dexterity: 21_463, intelligence: 86_190, constitution: 86_596, luck: 30_488, health: 81_573_432, minWeaponDmg: 1, maxWeaponDmg: 1),
                    new (position: 846, @class: ClassType.Scout, level: 470, strength: 21_488, dexterity: 85_615, intelligence: 21_404, constitution: 87_408, luck: 30_566, health: 164_676_672, minWeaponDmg: 1, maxWeaponDmg: 1),
                    new (position: 847, @class: ClassType.Mage, level: 470, strength: 21_344, dexterity: 21_514, intelligence: 86_394, constitution: 86_800, luck: 30_560, health: 81_765_600, minWeaponDmg: 1, maxWeaponDmg: 1),
                    new (position: 848, @class: ClassType.Scout, level: 470, strength: 21_539, dexterity: 85_818, intelligence: 21_454, constitution: 87_615, luck: 30_638, health: 165_066_656, minWeaponDmg: 1, maxWeaponDmg: 1),
                    new (position: 849, @class: ClassType.Scout, level: 470, strength: 21_565, dexterity: 85_919, intelligence: 21_480, constitution: 87_719, luck: 30_674, health: 165_262_592, minWeaponDmg: 1, maxWeaponDmg: 1),
                    new (position: 850, @class: ClassType.Mage, level: 471, strength: 21_471, dexterity: 21_641, intelligence: 86_904, constitution: 87_322, luck: 30_736, health: 82_431_968, minWeaponDmg: 1, maxWeaponDmg: 1),
                    new (position: 851, @class: ClassType.Warrior, level: 471, strength: 86_325, dexterity: 30_262, intelligence: 30_049, constitution: 117_225, luck: 18_986, health: 276_651_008, minWeaponDmg: 1, maxWeaponDmg: 1),
                    new (position: 852, @class: ClassType.Warrior, level: 471, strength: 86_427, dexterity: 30_297, intelligence: 30_084, constitution: 117_363, luck: 19_008, health: 276_976_672, minWeaponDmg: 1, maxWeaponDmg: 1),
                    new (position: 853, @class: ClassType.Scout, level: 471, strength: 21_717, dexterity: 86_528, intelligence: 21_632, constitution: 88_347, luck: 30_887, health: 166_799_136, minWeaponDmg: 1, maxWeaponDmg: 1),
                    new (position: 854, @class: ClassType.Warrior, level: 471, strength: 86_630, dexterity: 30_368, intelligence: 30_155, constitution: 117_638, luck: 19_053, health: 277_625_664, minWeaponDmg: 1, maxWeaponDmg: 1),
                    new (position: 855, @class: ClassType.Scout, level: 472, strength: 21_820, dexterity: 86_936, intelligence: 21_734, constitution: 88_769, luck: 31_037, health: 167_950_944, minWeaponDmg: 1, maxWeaponDmg: 1),
                    new (position: 856, @class: ClassType.Warrior, level: 472, strength: 87_038, dexterity: 30_516, intelligence: 30_302, constitution: 118_202, luck: 19_140, health: 279_547_744, minWeaponDmg: 1, maxWeaponDmg: 1),
                    new (position: 857, @class: ClassType.Warrior, level: 472, strength: 87_140, dexterity: 30_552, intelligence: 30_338, constitution: 118_339, luck: 19_163, health: 279_871_744, minWeaponDmg: 1, maxWeaponDmg: 1),
                    new (position: 858, @class: ClassType.Scout, level: 472, strength: 21_896, dexterity: 87_241, intelligence: 21_810, constitution: 89_081, luck: 31_145, health: 168_541_248, minWeaponDmg: 1, maxWeaponDmg: 1),
                    new (position: 859, @class: ClassType.Scout, level: 472, strength: 21_922, dexterity: 87_343, intelligence: 21_836, constitution: 89_185, luck: 31_182, health: 168_738_016, minWeaponDmg: 1, maxWeaponDmg: 1),
                    new (position: 860, @class: ClassType.Scout, level: 473, strength: 21_999, dexterity: 87_651, intelligence: 21_913, constitution: 89_505, luck: 31_287, health: 169_701_472, minWeaponDmg: 1, maxWeaponDmg: 1),
                    new (position: 861, @class: ClassType.Scout, level: 473, strength: 22_024, dexterity: 87_753, intelligence: 21_938, constitution: 89_609, luck: 31_323, health: 169_898_656, minWeaponDmg: 1, maxWeaponDmg: 1),
                    new (position: 862, @class: ClassType.Warrior, level: 473, strength: 87_855, dexterity: 30_799, intelligence: 30_584, constitution: 119_319, luck: 19_309, health: 282_786_016, minWeaponDmg: 1, maxWeaponDmg: 1),
                    new (position: 863, @class: ClassType.Warrior, level: 473, strength: 87_957, dexterity: 30_835, intelligence: 30_619, constitution: 119_457, luck: 19_331, health: 283_113_088, minWeaponDmg: 1, maxWeaponDmg: 1),
                    new (position: 864, @class: ClassType.Mage, level: 473, strength: 21_928, dexterity: 22_101, intelligence: 88_750, constitution: 89_195, luck: 31_389, health: 84_556_864, minWeaponDmg: 1, maxWeaponDmg: 1),
                    new (position: 865, @class: ClassType.Mage, level: 474, strength: 22_006, dexterity: 22_179, intelligence: 89_060, constitution: 89_517, luck: 31_503, health: 85_041_152, minWeaponDmg: 1, maxWeaponDmg: 1),
                    new (position: 866, @class: ClassType.Mage, level: 474, strength: 22_031, dexterity: 22_204, intelligence: 89_163, constitution: 89_621, luck: 31_540, health: 85_139_952, minWeaponDmg: 1, maxWeaponDmg: 1),
                    //

                    new (position: 867, @class: ClassType.Warrior, level: 474, strength: 88573, dexterity: 31056, intelligence: 30839, constitution: 120303, luck: 19464, health: 285719616, minWeaponDmg: 528, maxWeaponDmg: 1108 ),
                    new (position: 868, @class: ClassType.Warrior, level: 474, strength: 88675, dexterity: 31092, intelligence: 30875, constitution: 120441, luck: 19487, health: 286047360, minWeaponDmg: 524, maxWeaponDmg: 1108 ),
                    new (position: 869, @class: ClassType.Warrior, level: 474, strength: 88777, dexterity: 31128, intelligence: 30910, constitution: 120580, luck: 19509, health: 286377504, minWeaponDmg: 525, maxWeaponDmg: 1108 ),
                    new (position: 870, @class: ClassType.Warrior, level: 475, strength: 89088, dexterity: 31233, intelligence: 31015, constitution: 121011, luck: 19575, health: 288006176, minWeaponDmg: 525, maxWeaponDmg: 1110 ),
                    new (position: 871, @class: ClassType.Warrior, level: 475, strength: 89190, dexterity: 31269, intelligence: 31051, constitution: 121150, luck: 19598, health: 288336992, minWeaponDmg: 527, maxWeaponDmg: 1110 ),
                    new (position: 872, @class: ClassType.Warrior, level: 475, strength: 89293, dexterity: 31305, intelligence: 31087, constitution: 121290, luck: 19620, health: 288670208, minWeaponDmg: 528, maxWeaponDmg: 1111 ),
                    new (position: 873, @class: ClassType.Warrior, level: 475, strength: 89395, dexterity: 31341, intelligence: 31122, constitution: 121428, luck: 19642, health: 288998656, minWeaponDmg: 525, maxWeaponDmg: 1110 ),
                    new (position: 874, @class: ClassType.Warrior, level: 475, strength: 89498, dexterity: 31377, intelligence: 31158, constitution: 121568, luck: 19665, health: 289331840, minWeaponDmg: 526, maxWeaponDmg: 1111 ),
                    new (position: 875, @class: ClassType.Warrior, level: 476, strength: 89810, dexterity: 31482, intelligence: 31264, constitution: 122001, luck: 19731, health: 290972384, minWeaponDmg: 525, maxWeaponDmg: 1113 ),
                    new (position: 876, @class: ClassType.Warrior, level: 476, strength: 89913, dexterity: 31518, intelligence: 31299, constitution: 122140, luck: 19754, health: 291303904, minWeaponDmg: 526, maxWeaponDmg: 1112 ),
                    new (position: 877, @class: ClassType.Warrior, level: 476, strength: 90015, dexterity: 31554, intelligence: 31335, constitution: 122280, luck: 19776, health: 291637792, minWeaponDmg: 527, maxWeaponDmg: 1114 ),

                    // Not confirmed: 
                    new (position: 878, @class: ClassType.Mage, level: 476, strength: 22_442, dexterity: 22_617, intelligence: 90_820, constitution: 91_305, luck: 32_117, health: 87_104_968, minWeaponDmg: 1, maxWeaponDmg: 1),
                    new (position: 879, @class: ClassType.Warrior, level: 476, strength: 90_221, dexterity: 31_626, intelligence: 31_407, constitution: 122_558, luck: 19_821, health: 292_300_832, minWeaponDmg: 1, maxWeaponDmg: 1),
                    new (position: 880, @class: ClassType.Scout, level: 477, strength: 22_722, dexterity: 90_534, intelligence: 22_634, constitution: 92_474, luck: 32_314, health: 176_810_288, minWeaponDmg: 1, maxWeaponDmg: 1),
                    new (position: 881, @class: ClassType.Warrior, level: 477, strength: 90_637, dexterity: 31_778, intelligence: 31_557, constitution: 123_134, luck: 19_911, health: 294_290_272, minWeaponDmg: 1, maxWeaponDmg: 1),
                    new (position: 882, @class: ClassType.Scout, level: 477, strength: 22_773, dexterity: 90_740, intelligence: 22_685, constitution: 92_685, luck: 32_387, health: 177_213_728, minWeaponDmg: 1, maxWeaponDmg: 1),
                    new (position: 883, @class: ClassType.Warrior, level: 477, strength: 90_843, dexterity: 31_850, intelligence: 31_629, constitution: 123_413, luck: 19_956, health: 294_957_056, minWeaponDmg: 1, maxWeaponDmg: 1),
                    new (position: 884, @class: ClassType.Scout, level: 477, strength: 22_825, dexterity: 90_946, intelligence: 22_736, constitution: 92_895, luck: 32_460, health: 177_615_232, minWeaponDmg: 1, maxWeaponDmg: 1),
                    new (position: 885, @class: ClassType.Scout, level: 478, strength: 22_904, dexterity: 91_261, intelligence: 22_815, constitution: 93_222, luck: 32_568, health: 178_613_344, minWeaponDmg: 1, maxWeaponDmg: 1),
                    new (position: 886, @class: ClassType.Warrior, level: 478, strength: 91_364, dexterity: 32_029, intelligence: 31_807, constitution: 124_130, luck: 20_059, health: 297_291_360, minWeaponDmg: 1, maxWeaponDmg: 1),
                    new (position: 887, @class: ClassType.Scout, level: 478, strength: 22_956, dexterity: 91_467, intelligence: 22_867, constitution: 93_433, luck: 32_642, health: 179_017_632, minWeaponDmg: 1, maxWeaponDmg: 1),
                    new (position: 888, @class: ClassType.Mage, level: 478, strength: 22_804, dexterity: 22_981, intelligence: 92_281, constitution: 92_793, luck: 32_634, health: 88_895_696, minWeaponDmg: 1, maxWeaponDmg: 1),
                    new (position: 889, @class: ClassType.Scout, level: 478, strength: 23_007, dexterity: 91_674, intelligence: 22_918, constitution: 93_643, luck: 32_715, health: 179_419_984, minWeaponDmg: 1, maxWeaponDmg: 1),
                    new (position: 890, @class: ClassType.Warrior, level: 479, strength: 91_990, dexterity: 32_254, intelligence: 32_031, constitution: 124_980, luck: 20_194, health: 299_952_000, minWeaponDmg: 1, maxWeaponDmg: 1),
                    new (position: 891, @class: ClassType.Warrior, level: 479, strength: 92_094, dexterity: 32_290, intelligence: 32_067, constitution: 125_120, luck: 20_217, health: 300_288_000, minWeaponDmg: 1, maxWeaponDmg: 1),
                    new (position: 892, @class: ClassType.Warrior, level: 479, strength: 92_197, dexterity: 32_326, intelligence: 32_103, constitution: 125_261, luck: 20_239, health: 300_626_400, minWeaponDmg: 1, maxWeaponDmg: 1),
                    new (position: 893, @class: ClassType.Warrior, level: 479, strength: 92_300, dexterity: 32_362, intelligence: 32_139, constitution: 125_402, luck: 20_262, health: 300_964_800, minWeaponDmg: 1, maxWeaponDmg: 1),
                    new (position: 894, @class: ClassType.Mage, level: 479, strength: 23_012, dexterity: 23_190, intelligence: 93_119, constitution: 93_644, luck: 32_935, health: 89_898_240, minWeaponDmg: 1, maxWeaponDmg: 1),
                    new (position: 895, @class: ClassType.Scout, level: 480, strength: 23_270, dexterity: 92_722, intelligence: 23_180, constitution: 94_727, luck: 33_088, health: 182_254_752, minWeaponDmg: 1, maxWeaponDmg: 1),
                    new (position: 896, @class: ClassType.Scout, level: 480, strength: 23_296, dexterity: 92_826, intelligence: 23_206, constitution: 94_833, luck: 33_125, health: 182_458_688, minWeaponDmg: 1, maxWeaponDmg: 1),
                    new (position: 897, @class: ClassType.Warrior, level: 480, strength: 92_929, dexterity: 32_579, intelligence: 32_355, constitution: 126_265, luck: 20_398, health: 303_667_328, minWeaponDmg: 1, maxWeaponDmg: 1),
                    new (position: 898, @class: ClassType.Warrior, level: 480, strength: 93_033, dexterity: 32_615, intelligence: 32_391, constitution: 126_405, luck: 20_421, health: 304_004_032, minWeaponDmg: 1, maxWeaponDmg: 1),
                    new (position: 899, @class: ClassType.Scout, level: 480, strength: 23_374, dexterity: 93_136, intelligence: 23_284, constitution: 95_150, luck: 33_236, health: 183_068_608, minWeaponDmg: 1, maxWeaponDmg: 1),
                    new (position: 900, @class: ClassType.Scout, level: 481, strength: 23454, dexterity: 93456, intelligence: 23364, constitution: 95483, luck: 33345, health: 184_091_232, minWeaponDmg: 661, maxWeaponDmg: 1322 ),
                    new (position: 901, @class: ClassType.Scout, level: 481, strength: 23_480, dexterity: 93_560, intelligence: 23_390, constitution: 95_589, luck: 33_382, health: 184_295_584, minWeaponDmg: 1, maxWeaponDmg: 1),
                    new (position: 902, @class: ClassType.Warrior, level: 481, strength: 93_664, dexterity: 32_833, intelligence: 32_607, constitution: 127_272, luck: 20_557, health: 306_725_504, minWeaponDmg: 1, maxWeaponDmg: 1),
                    new (position: 903, @class: ClassType.Warrior, level: 481, strength: 93_768, dexterity: 32_869, intelligence: 32_643, constitution: 127_412, luck: 20_579, health: 307_062_912, minWeaponDmg: 1, maxWeaponDmg: 1),
                    new (position: 904, @class: ClassType.Warrior, level: 481, strength: 93_871, dexterity: 32_906, intelligence: 32_680, constitution: 127_554, luck: 20_602, health: 307_405_152, minWeaponDmg: 1, maxWeaponDmg: 1),
                    new (position: 905, @class: ClassType.Scout, level: 482, strength: 23_639, dexterity: 94_192, intelligence: 23_548, constitution: 96_241, luck: 33_612, health: 185_937_616, minWeaponDmg: 1, maxWeaponDmg: 1),
                    new (position: 906, @class: ClassType.Scout, level: 482, strength: 23_665, dexterity: 94_296, intelligence: 23_574, constitution: 96_348, luck: 33_649, health: 186_144_336, minWeaponDmg: 1, maxWeaponDmg: 1),
                    new (position: 907, @class: ClassType.Warrior, level: 482, strength: 94_401, dexterity: 33_096, intelligence: 32_870, constitution: 128_282, luck: 20_716, health: 309_801_024, minWeaponDmg: 1, maxWeaponDmg: 1),
                    new (position: 908, @class: ClassType.Scout, level: 482, strength: 23_717, dexterity: 94_505, intelligence: 23_626, constitution: 96_560, luck: 33_723, health: 186_553_920, minWeaponDmg: 1, maxWeaponDmg: 1),
                    new (position: 909, @class: ClassType.Mage, level: 482, strength: 23_561, dexterity: 23_743, intelligence: 95_336, constitution: 95_903, luck: 33_715, health: 92_642_296, minWeaponDmg: 1, maxWeaponDmg: 1),
                    new (position: 910, @class: ClassType.Mage, level: 483, strength: 23_642, dexterity: 23_824, intelligence: 95_659, constitution: 96_238, luck: 33_825, health: 93_158_384, minWeaponDmg: 1, maxWeaponDmg: 1),
                    new (position: 911, @class: ClassType.Warrior, level: 483, strength: 95_036, dexterity: 33_315, intelligence: 33_088, constitution: 129_153, luck: 20_844, health: 312_550_272, minWeaponDmg: 1, maxWeaponDmg: 1),
                    new (position: 912, @class: ClassType.Mage, level: 483, strength: 23_694, dexterity: 23_876, intelligence: 95_869, constitution: 96_450, luck: 33_899, health: 93_363_600, minWeaponDmg: 1, maxWeaponDmg: 1),
                    new (position: 913, @class: ClassType.Scout, level: 483, strength: 23_902, dexterity: 95_244, intelligence: 23_811, constitution: 97_322, luck: 33_982, health: 188_415_392, minWeaponDmg: 1, maxWeaponDmg: 1),
                    new (position: 914, @class: ClassType.Warrior, level: 483, strength: 95_348, dexterity: 33_425, intelligence: 33_196, constitution: 129_578, luck: 20_912, health: 313_578_752, minWeaponDmg: 1, maxWeaponDmg: 1),
                    new (position: 915, @class: ClassType.Warrior, level: 484, strength: 95_672, dexterity: 33_544, intelligence: 33_315, constitution: 130_028, luck: 20_981, health: 315_317_888, minWeaponDmg: 1, maxWeaponDmg: 1),
                    new (position: 916, @class: ClassType.Scout, level: 484, strength: 24_036, dexterity: 95_777, intelligence: 23_944, constitution: 97_873, luck: 34_176, health: 189_873_616, minWeaponDmg: 1, maxWeaponDmg: 1),
                    new (position: 917, @class: ClassType.Scout, level: 484, strength: 24_062, dexterity: 95_882, intelligence: 23_970, constitution: 97_980, luck: 34_213, health: 190_081_200, minWeaponDmg: 1, maxWeaponDmg: 1),
                    new (position: 918, @class: ClassType.Mage, level: 484, strength: 23_905, dexterity: 24_088, intelligence: 96_720, constitution: 97_315, luck: 34_205, health: 94_395_552, minWeaponDmg: 1, maxWeaponDmg: 1),
                    new (position: 919, @class: ClassType.Mage, level: 484, strength: 23_931, dexterity: 24_115, intelligence: 96_826, constitution: 97_421, luck: 34_242, health: 94_498_368, minWeaponDmg: 1, maxWeaponDmg: 1),
                    new (position: 920, @class: ClassType.Scout, level: 485, strength: 24_196, dexterity: 96_416, intelligence: 24_104, constitution: 98_532, luck: 34_399, health: 191_546_208, minWeaponDmg: 1, maxWeaponDmg: 1),
                    new (position: 921, @class: ClassType.Scout, level: 485, strength: 24_222, dexterity: 96_521, intelligence: 24_130, constitution: 98_639, luck: 34_436, health: 191_754_208, minWeaponDmg: 1, maxWeaponDmg: 1),
                    new (position: 922, @class: ClassType.Warrior, level: 485, strength: 96_626, dexterity: 33_874, intelligence: 33_644, constitution: 131_333, luck: 21_188, health: 319_139_200, minWeaponDmg: 1, maxWeaponDmg: 1),
                    new (position: 923, @class: ClassType.Warrior, level: 485, strength: 96_730, dexterity: 33_911, intelligence: 33_680, constitution: 131_475, luck: 21_211, health: 319_484_256, minWeaponDmg: 1, maxWeaponDmg: 1),
                    new (position: 924, @class: ClassType.Scout, level: 485, strength: 24_301, dexterity: 96_835, intelligence: 24_209, constitution: 98_960, luck: 34_548, health: 192_378_240, minWeaponDmg: 1, maxWeaponDmg: 1),
                    new (position: 925, @class: ClassType.Scout, level: 486, strength: 24_383, dexterity: 97_162, intelligence: 24_290, constitution: 99_301, luck: 34_660, health: 193_438_352, minWeaponDmg: 1, maxWeaponDmg: 1),
                    new (position: 926, @class: ClassType.Warrior, level: 486, strength: 97_267, dexterity: 34_095, intelligence: 33_864, constitution: 132_213, luck: 21_326, health: 321_938_656, minWeaponDmg: 1, maxWeaponDmg: 1),
                    new (position: 927, @class: ClassType.Warrior, level: 486, strength: 97_372, dexterity: 34_132, intelligence: 33_900, constitution: 132_356, luck: 21_349, health: 322_286_848, minWeaponDmg: 1, maxWeaponDmg: 1),
                    new (position: 928, @class: ClassType.Warrior, level: 486, strength: 97_477, dexterity: 34_169, intelligence: 33_937, constitution: 132_498, luck: 21_372, health: 322_632_640, minWeaponDmg: 1, maxWeaponDmg: 1),
                    new (position: 929, @class: ClassType.Mage, level: 486, strength: 24_303, dexterity: 24_488, intelligence: 98_325, constitution: 98_950, luck: 34_763, health: 96_377_296, minWeaponDmg: 1, maxWeaponDmg: 1),
                    new (position: 930, @class: ClassType.Scout, level: 487, strength: 24_571, dexterity: 97_910, intelligence: 24_478, constitution: 100_071, luck: 34_931, health: 195_338_592, minWeaponDmg: 1, maxWeaponDmg: 1),
                    new (position: 931, @class: ClassType.Warrior, level: 487, strength: 98_016, dexterity: 34_363, intelligence: 34_130, constitution: 133_240, luck: 21_487, health: 325_105_600, minWeaponDmg: 1, maxWeaponDmg: 1),
                    new (position: 932, @class: ClassType.Mage, level: 487, strength: 24_437, dexterity: 24_623, intelligence: 98_867, constitution: 99_504, luck: 34_959, health: 97_115_904, minWeaponDmg: 1, maxWeaponDmg: 1),
                    new (position: 933, @class: ClassType.Scout, level: 487, strength: 24_650, dexterity: 98_226, intelligence: 24_557, constitution: 100_395, luck: 35_043, health: 195_971_040, minWeaponDmg: 1, maxWeaponDmg: 1),
                    new (position: 934, @class: ClassType.Warrior, level: 487, strength: 98_332, dexterity: 34_474, intelligence: 34_240, constitution: 133_669, luck: 21_557, health: 326_152_352, minWeaponDmg: 1, maxWeaponDmg: 1),
                    new (position: 935, @class: ClassType.Warrior, level: 488, strength: 98_661, dexterity: 34_586, intelligence: 34_352, constitution: 134_127, luck: 21_617, health: 327_940_512, minWeaponDmg: 1, maxWeaponDmg: 1),
                    new (position: 936, @class: ClassType.Warrior, level: 488, strength: 98_767, dexterity: 34_623, intelligence: 34_389, constitution: 134_270, luck: 21_640, health: 328_290_144, minWeaponDmg: 1, maxWeaponDmg: 1),
                    new (position: 937, @class: ClassType.Warrior, level: 488, strength: 98_872, dexterity: 34_660, intelligence: 34_425, constitution: 134_414, luck: 21_663, health: 328_642_240, minWeaponDmg: 1, maxWeaponDmg: 1),
                    new (position: 938, @class: ClassType.Warrior, level: 488, strength: 98_978, dexterity: 34_697, intelligence: 34_462, constitution: 134_558, luck: 21_687, health: 328_994_304, minWeaponDmg: 1, maxWeaponDmg: 1),
                    //

                    new (position: 939, @class: ClassType.Warrior, level: 488, strength: 99083, dexterity: 34734, intelligence: 34499, constitution: 134700, luck: 21710, health: 329341504, minWeaponDmg: 544, maxWeaponDmg: 1157 ),

                    // Not confirmed:
                    new (position: 940, @class: ClassType.Mage, level: 489, strength: 24_760, dexterity: 24_948, intelligence: 100_166, constitution: 100_832, luck: 35_419, health: 98_815_360, minWeaponDmg: 1, maxWeaponDmg: 1),
                    new (position: 941, @class: ClassType.Warrior, level: 489, strength: 99_520, dexterity: 34_892, intelligence: 34_657, constitution: 135_304, luck: 21_803, health: 331_494_784, minWeaponDmg: 1, maxWeaponDmg: 1),
                    new (position: 942, @class: ClassType.Mage, level: 489, strength: 24_812, dexterity: 25_001, intelligence: 100_380, constitution: 101_047, luck: 35_495, health: 99_026_064, minWeaponDmg: 1, maxWeaponDmg: 1),
                    new (position: 943, @class: ClassType.Mage, level: 489, strength: 24_839, dexterity: 25_027, intelligence: 100_486, constitution: 101_154, luck: 35_532, health: 99_130_920, minWeaponDmg: 1, maxWeaponDmg: 1),
                    new (position: 944, @class: ClassType.Scout, level: 489, strength: 25_054, dexterity: 99_837, intelligence: 24_959, constitution: 102_054, luck: 35_617, health: 200_025_840, minWeaponDmg: 1, maxWeaponDmg: 1),
                    new (position: 945, @class: ClassType.Scout, level: 490, strength: 25_137, dexterity: 100_170, intelligence: 25_042, constitution: 102_400, luck: 35_730, health: 201_113_600, minWeaponDmg: 1, maxWeaponDmg: 1),
                    new (position: 946, @class: ClassType.Scout, level: 490, strength: 25_164, dexterity: 100_276, intelligence: 25_069, constitution: 102_508, luck: 35_768, health: 201_325_712, minWeaponDmg: 1, maxWeaponDmg: 1),

                    //

                    new (position: 947, @class: ClassType.Warrior, level: 490, strength: 100382, dexterity: 35191, intelligence: 34954, constitution: 136484, luck: 21989, health: 335068224, minWeaponDmg: 544, maxWeaponDmg: 1162 ),

                    // Not confirmed:
                    new (position: 948, @class: ClassType.Scout, level: 490, strength: 25_217, dexterity: 100_488, intelligence: 25_122, constitution: 102_726, luck: 35_844, health: 201_753_856, minWeaponDmg: 1, maxWeaponDmg: 1),
                    new (position: 949, @class: ClassType.Scout, level: 490, strength: 25_243, dexterity: 100_594, intelligence: 25_149, constitution: 102_834, luck: 35_882, health: 201_965_984, minWeaponDmg: 1, maxWeaponDmg: 1),
                    new (position: 950, @class: ClassType.Warrior, level: 491, strength: 100_928, dexterity: 35_378, intelligence: 35_140, constitution: 137_236, luck: 22_106, health: 337_600_576, minWeaponDmg: 1, maxWeaponDmg: 1),
                    new (position: 951, @class: ClassType.Scout, level: 491, strength: 25_354, dexterity: 101_034, intelligence: 25_259, constitution: 103_290, luck: 36_033, health: 203_274_720, minWeaponDmg: 1, maxWeaponDmg: 1),
                    new (position: 952, @class: ClassType.Warrior, level: 491, strength: 101_140, dexterity: 35_452, intelligence: 35_214, constitution: 137_525, luck: 22_153, health: 338_311_488, minWeaponDmg: 1, maxWeaponDmg: 1),
                    new (position: 953, @class: ClassType.Warrior, level: 491, strength: 101_247, dexterity: 35_490, intelligence: 35_251, constitution: 137_670, luck: 22_176, health: 338_668_192, minWeaponDmg: 1, maxWeaponDmg: 1),
                    //

                    new (position: 954, @class: ClassType.Warrior, level: 491, strength: 101353, dexterity: 35527, intelligence: 35288, constitution: 137814, luck: 22200, health: 339022432, minWeaponDmg: 546, maxWeaponDmg: 1163 ),

                    // Not confirmed:
                    new (position: 955, @class: ClassType.Scout, level: 492, strength: 25_518, dexterity: 101_688, intelligence: 25_422, constitution: 103_965, luck: 36_271, health: 205_018_976, minWeaponDmg: 1, maxWeaponDmg: 1),
                    new (position: 956, @class: ClassType.Scout, level: 492, strength: 25_544, dexterity: 101_795, intelligence: 25_449, constitution: 104_074, luck: 36_309, health: 205_233_920, minWeaponDmg: 1, maxWeaponDmg: 1),
                    new (position: 957, @class: ClassType.Warrior, level: 492, strength: 101_901, dexterity: 35_725, intelligence: 35_486, constitution: 138_559, luck: 22_317, health: 341_547_936, minWeaponDmg: 1, maxWeaponDmg: 1),
                    new (position: 958, @class: ClassType.Warrior, level: 492, strength: 102_008, dexterity: 35_762, intelligence: 35_523, constitution: 138_704, luck: 22_341, health: 341_905_344, minWeaponDmg: 1, maxWeaponDmg: 1),
                    new (position: 959, @class: ClassType.Warrior, level: 492, strength: 102_114, dexterity: 35_799, intelligence: 35_560, constitution: 138_849, luck: 22_364, health: 342_262_784, minWeaponDmg: 1, maxWeaponDmg: 1),
                    new (position: 960, @class: ClassType.Scout, level: 493, strength: 25_709, dexterity: 102_451, intelligence: 25_613, constitution: 104_751, luck: 36_538, health: 206_987_968, minWeaponDmg: 1, maxWeaponDmg: 1),
                    new (position: 961, @class: ClassType.Scout, level: 493, strength: 25_736, dexterity: 102_558, intelligence: 25_639, constitution: 104_860, luck: 36_576, health: 207_203_360, minWeaponDmg: 1, maxWeaponDmg: 1),
                    //

                    new (position: 962, @class: ClassType.Warrior, level: 493, strength: 102665, dexterity: 35988, intelligence: 35748, constitution: 139606, luck: 22472, health: 344826816, minWeaponDmg: 546, maxWeaponDmg: 1174 ),

                    // Not confirmed:
                    new (position: 963, @class: ClassType.Mage, level: 493, strength: 25_597, dexterity: 25_789, intelligence: 103_542, constitution: 104_270, luck: 36_604, health: 103_018_760, minWeaponDmg: 1, maxWeaponDmg: 1),
                    new (position: 964, @class: ClassType.Warrior, level: 493, strength: 102_878, dexterity: 36_063, intelligence: 35_822, constitution: 139_896, luck: 22_519, health: 345_543_104, minWeaponDmg: 1, maxWeaponDmg: 1),
                    new (position: 965, @class: ClassType.Mage, level: 494, strength: 25_708, dexterity: 25_901, intelligence: 103_988, constitution: 104_729, luck: 36_767, health: 103_681_712, minWeaponDmg: 1, maxWeaponDmg: 1),
                    new (position: 966, @class: ClassType.Scout, level: 494, strength: 25_927, dexterity: 103_323, intelligence: 25_831, constitution: 105_650, luck: 36_853, health: 209_187_008, minWeaponDmg: 1, maxWeaponDmg: 1),
                    new (position: 967, @class: ClassType.Scout, level: 494, strength: 25_954, dexterity: 103_430, intelligence: 25_858, constitution: 105_759, luck: 36_891, health: 209_402_816, minWeaponDmg: 1, maxWeaponDmg: 1),
                    new (position: 968, @class: ClassType.Scout, level: 494, strength: 25_981, dexterity: 103_537, intelligence: 25_884, constitution: 105_868, luck: 36_929, health: 209_618_640, minWeaponDmg: 1, maxWeaponDmg: 1),
                    new (position: 969, @class: ClassType.Scout, level: 494, strength: 26_008, dexterity: 103_644, intelligence: 25_911, constitution: 105_978, luck: 36_967, health: 209_836_448, minWeaponDmg: 1, maxWeaponDmg: 1),
                    new (position: 970, @class: ClassType.Mage, level: 495, strength: 25_899, dexterity: 26_093, intelligence: 104_760, constitution: 105_517, luck: 37_035, health: 104_672_864, minWeaponDmg: 1, maxWeaponDmg: 1),
                    new (position: 971, @class: ClassType.Mage, level: 495, strength: 25_926, dexterity: 26_120, intelligence: 104_868, constitution: 105_626, luck: 37_073, health: 104_780_992, minWeaponDmg: 1, maxWeaponDmg: 1),
                    new (position: 972, @class: ClassType.Warrior, level: 495, strength: 104_198, dexterity: 36_528, intelligence: 36_285, constitution: 141_710, luck: 22_803, health: 351_440_800, minWeaponDmg: 1, maxWeaponDmg: 1),
                    new (position: 973, @class: ClassType.Scout, level: 495, strength: 26_174, dexterity: 104_306, intelligence: 26_076, constitution: 106_660, luck: 37_198, health: 211_613_440, minWeaponDmg: 1, maxWeaponDmg: 1),
                    new (position: 974, @class: ClassType.Warrior, level: 495, strength: 104_413, dexterity: 36_603, intelligence: 36_359, constitution: 142_002, luck: 22_850, health: 352_164_960, minWeaponDmg: 1, maxWeaponDmg: 1),
                    new (position: 975, @class: ClassType.Warrior, level: 496, strength: 104_754, dexterity: 36_718, intelligence: 36_475, constitution: 142_476, luck: 22_922, health: 354_052_864, minWeaponDmg: 1, maxWeaponDmg: 1),
                    new (position: 976, @class: ClassType.Mage, level: 496, strength: 26_118, dexterity: 26_313, intelligence: 105_642, constitution: 106_415, luck: 37_342, health: 105_776_512, minWeaponDmg: 1, maxWeaponDmg: 1),
                    new (position: 977, @class: ClassType.Scout, level: 496, strength: 26_340, dexterity: 104_969, intelligence: 26_242, constitution: 107_345, luck: 37_429, health: 213_401_856, minWeaponDmg: 1, maxWeaponDmg: 1),
                    new (position: 978, @class: ClassType.Warrior, level: 496, strength: 105_076, dexterity: 36_831, intelligence: 36_587, constitution: 142_913, luck: 22_993, health: 355_138_816, minWeaponDmg: 1, maxWeaponDmg: 1),
                    new (position: 979, @class: ClassType.Warrior, level: 496, strength: 105_184, dexterity: 36_869, intelligence: 36_624, constitution: 143_059, luck: 23_016, health: 355_501_600, minWeaponDmg: 1, maxWeaponDmg: 1),
                    new (position: 980, @class: ClassType.Warrior, level: 497, strength: 105_526, dexterity: 36_995, intelligence: 36_750, constitution: 143_535, luck: 23_089, health: 357_402_144, minWeaponDmg: 1, maxWeaponDmg: 1),
                    //

                    new (position: 981, @class: ClassType.Warrior, level: 497, strength: 105634, dexterity: 37033, intelligence: 36788, constitution: 143682, luck: 23112, health: 357768192, minWeaponDmg: 554, maxWeaponDmg: 1189 ),

                    // Not confirmed:
                    new (position: 982, @class: ClassType.Mage, level: 497, strength: 26_337, dexterity: 26_534, intelligence: 106_527, constitution: 107_317, luck: 37_660, health: 106_887_728, minWeaponDmg: 1, maxWeaponDmg: 1),
                    new (position: 983, @class: ClassType.Warrior, level: 497, strength: 105_849, dexterity: 37_108, intelligence: 36_863, constitution: 143_975, luck: 23_159, health: 358_497_760, minWeaponDmg: 1, maxWeaponDmg: 1),
                    new (position: 984, @class: ClassType.Mage, level: 497, strength: 26_391, dexterity: 26_588, intelligence: 106_744, constitution: 107_536, luck: 37_736, health: 107_105_856, minWeaponDmg: 1, maxWeaponDmg: 1),
                    new (position: 985, @class: ClassType.Scout, level: 498, strength: 26_674, dexterity: 106_301, intelligence: 26_575, constitution: 108_720, luck: 37_903, health: 217_005_120, minWeaponDmg: 1, maxWeaponDmg: 1),
                    new (position: 986, @class: ClassType.Warrior, level: 498, strength: 106_409, dexterity: 37_300, intelligence: 37_054, constitution: 144_746, luck: 23_270, health: 361_141_280, minWeaponDmg: 1, maxWeaponDmg: 1),
                    new (position: 987, @class: ClassType.Scout, level: 498, strength: 26_728, dexterity: 106_517, intelligence: 26_629, constitution: 108_941, luck: 37_980, health: 217_446_240, minWeaponDmg: 1, maxWeaponDmg: 1),
                    new (position: 988, @class: ClassType.Mage, level: 498, strength: 26_557, dexterity: 26_755, intelligence: 107_415, constitution: 108_221, luck: 37_969, health: 108_004_560, minWeaponDmg: 1, maxWeaponDmg: 1),
                    new (position: 989, @class: ClassType.Scout, level: 498, strength: 26_782, dexterity: 106_733, intelligence: 26_683, constitution: 109_162, luck: 38_057, health: 217_887_360, minWeaponDmg: 1, maxWeaponDmg: 1),
                    //

                    new (position: 990, @class: ClassType.Warrior, level: 499, strength: 107078, dexterity: 37541, intelligence: 37293, constitution: 145665, luck: 23414, health: 364162496, minWeaponDmg: 555, maxWeaponDmg: 1197 ),

                    // Not confirmed:
                    new (position: 991, @class: ClassType.Warrior, level: 499, strength: 107_187, dexterity: 37_579, intelligence: 37_331, constitution: 145_812, luck: 23_437, health: 364_529_984, minWeaponDmg: 1, maxWeaponDmg: 1),
                    new (position: 992, @class: ClassType.Scout, level: 499, strength: 26_923, dexterity: 107_295, intelligence: 26_824, constitution: 109_743, luck: 38_261, health: 219_486_000, minWeaponDmg: 1, maxWeaponDmg: 1),
                    new (position: 993, @class: ClassType.Scout, level: 499, strength: 26_950, dexterity: 107_403, intelligence: 26_851, constitution: 109_853, luck: 38_300, health: 219_706_000, minWeaponDmg: 1, maxWeaponDmg: 1),
                    new (position: 994, @class: ClassType.Warrior, level: 499, strength: 107_511, dexterity: 37_692, intelligence: 37_444, constitution: 146_253, luck: 23_508, health: 365_632_512, minWeaponDmg: 1, maxWeaponDmg: 1),
                    new (position: 995, @class: ClassType.Warrior, level: 500, strength: 107_858, dexterity: 37_810, intelligence: 37_561, constitution: 146_735, luck: 23_581, health: 367_571_168, minWeaponDmg: 1, maxWeaponDmg: 1),
                    new (position: 996, @class: ClassType.Mage, level: 500, strength: 26_892, dexterity: 27_091, intelligence: 108_763, constitution: 109_600, luck: 38_446, health: 109_819_200, minWeaponDmg: 1, maxWeaponDmg: 1),
                    new (position: 997, @class: ClassType.Scout, level: 500, strength: 27_118, dexterity: 108_075, intelligence: 27_019, constitution: 110_547, luck: 38_534, health: 221_536_192, minWeaponDmg: 1, maxWeaponDmg: 1),
                    //

                    new (position: 998, @class: ClassType.Warrior, level: 500, strength: 108183, dexterity: 37924, intelligence: 37674, constitution: 147177, luck: 23653, health: 368678400, minWeaponDmg: 556, maxWeaponDmg: 1201 ),
                    new (position: 999, @class: ClassType.Warrior, level: 500, strength: 108292, dexterity: 37962, intelligence: 37712, constitution: 147326, luck: 23676, health: 369051616, minWeaponDmg: 555, maxWeaponDmg: 1201 ),
                    new (position: 1000, @class: ClassType.Scout, level: 501, strength: 34075, dexterity: 135800, intelligence: 33950, constitution: 138915, luck: 48412, health: 278941312, minWeaponDmg: 872, maxWeaponDmg: 1334 )
                ]
            },

            #endregion

            #region Shadow World

            new Dungeon
            {
                Name = DungeonNames.DesecratedCatacombs,
                Position = 101,
                IsDefeated = false,
                IsUnlocked = true,
                UnlockResolve = c => c.Dungeons.First(d => d.Type == DungeonTypeEnum.Tower).IsUnlocked && c.CharacterLevel >= 140,
                Type = DungeonTypeEnum.Shadow,
                DungeonEnemies = new List<DungeonEnemy>
                {
                    new DungeonEnemy(position: 1, @class: ClassType.Mage, level: 172, strength: 825, dexterity: 894, intelligence: 1788, constitution: 7282, luck: 808, health: 2519572, minWeaponDmg: 4178, maxWeaponDmg: 8303, armor: 5150 ),
                    new DungeonEnemy(position: 2, @class: ClassType.Warrior, level: 174, strength: 1740, dexterity: 986, intelligence: 855, constitution: 8052, luck: 739, health: 7045500, minWeaponDmg: 1867, maxWeaponDmg: 3730, armor: 560 ),
                    new DungeonEnemy(position: 3, @class: ClassType.Warrior, level: 176, strength: 1873, dexterity: 980, intelligence: 867, constitution: 8569, luck: 817, health: 7583565, minWeaponDmg: 1890, maxWeaponDmg: 3771, armor: 3500 ),
                    new DungeonEnemy(position: 4, @class: ClassType.Scout, level: 178, strength: 934, dexterity: 2169, intelligence: 923, constitution: 8014, luck: 1045, health: 5738024, minWeaponDmg: 2388, maxWeaponDmg: 4767, armor: 660 ),
                    new DungeonEnemy(position: 5, @class: ClassType.Warrior, level: 180, strength: 2140, dexterity: 1010, intelligence: 890, constitution: 9295, luck: 930, health: 8411975, minWeaponDmg: 1934, maxWeaponDmg: 3858, armor: 1850 ),
                    new DungeonEnemy(position: 6, @class: ClassType.Mage, level: 182, strength: 802, dexterity: 819, intelligence: 2506, constitution: 9009, luck: 1133, health: 3297294, minWeaponDmg: 4401, maxWeaponDmg: 8783, armor: 2900 ),
                    new DungeonEnemy(position: 7, @class: ClassType.Warrior, level: 184, strength: 2540, dexterity: 955, intelligence: 863, constitution: 10120, luck: 1004, health: 9361000, minWeaponDmg: 1973, maxWeaponDmg: 3945, armor: 1650 ),
                    new DungeonEnemy(position: 8, @class: ClassType.Mage, level: 186, strength: 781, dexterity: 806, intelligence: 2852, constitution: 9510, luck: 1196, health: 3556740, minWeaponDmg: 4498, maxWeaponDmg: 8944, armor: 450 ),
                    new DungeonEnemy(position: 9, @class: ClassType.Warrior, level: 188, strength: 2885, dexterity: 972, intelligence: 897, constitution: 11500, luck: 1118, health: 10867500, minWeaponDmg: 2017, maxWeaponDmg: 4027, armor: 2450 ),
                    new DungeonEnemy(position: 10, @class: ClassType.Scout, level: 190, strength: 839, dexterity: 3218, intelligence: 809, constitution: 11720, luck: 1109, health: 8954080, minWeaponDmg: 2547, maxWeaponDmg: 5092, armor: 1500 )
                }
            },
            new Dungeon
            {
                Name = DungeonNames.MinesOfGloria,
                Position = 102,
                IsDefeated = false,
                IsUnlocked = true,
                UnlockResolve = c => c.Dungeons.First(d => d.Position == 101).DungeonEnemies.First(e => e.Position == 5).IsDefeated,
                Type = DungeonTypeEnum.Shadow,
                DungeonEnemies = new List<DungeonEnemy>
                {
                    new DungeonEnemy(position: 1, @class: ClassType.Scout, level: 192, strength: 969, dexterity: 2534, intelligence: 969, constitution: 9185, luck: 1142, health: 7090820, minWeaponDmg: 2598, maxWeaponDmg: 5104, armor: 630 ),
                    new DungeonEnemy(position: 2, @class: ClassType.Warrior, level: 194, strength: 2562, dexterity: 1018, intelligence: 945, constitution: 10576, luck: 1050, health: 10311600, minWeaponDmg: 2112, maxWeaponDmg: 4151, armor: 1920 ),
                    new DungeonEnemy(position: 3, @class: ClassType.Warrior, level: 196, strength: 2751, dexterity: 966, intelligence: 875, constitution: 10934, luck: 1064, health: 10769990, minWeaponDmg: 2110, maxWeaponDmg: 4200, armor: 1900 ),
                    new DungeonEnemy(position: 4, @class: ClassType.Scout, level: 198, strength: 832, dexterity: 3226, intelligence: 838, constitution: 9702, luck: 1257, health: 7722792, minWeaponDmg: 2653, maxWeaponDmg: 5305, armor: 2700 ),
                    new DungeonEnemy(position: 5, @class: ClassType.Warrior, level: 200, strength: 3115, dexterity: 936, intelligence: 852, constitution: 11517, luck: 1026, health: 11574585, minWeaponDmg: 2145, maxWeaponDmg: 4287, armor: 2000 ),
                    new DungeonEnemy(position: 6, @class: ClassType.Mage, level: 202, strength: 876, dexterity: 872, intelligence: 3580, constitution: 10373, luck: 1189, health: 4211438, minWeaponDmg: 5043, maxWeaponDmg: 9742, armor: 2980 ),
                    new DungeonEnemy(position: 7, @class: ClassType.Warrior, level: 204, strength: 3162, dexterity: 1032, intelligence: 977, constitution: 13156, luck: 1045, health: 13484900, minWeaponDmg: 2187, maxWeaponDmg: 4369, armor: 2570 ),
                    new DungeonEnemy(position: 8, @class: ClassType.Scout, level: 206, strength: 919, dexterity: 3531, intelligence: 882, constitution: 13756, luck: 1269, health: 11389968, minWeaponDmg: 2796, maxWeaponDmg: 5518, armor: 2150 ),
                    new DungeonEnemy(position: 9, @class: ClassType.Scout, level: 208, strength: 945, dexterity: 3655, intelligence: 913, constitution: 15252, luck: 1323, health: 12750672, minWeaponDmg: 2799, maxWeaponDmg: 5566, armor: 3070 ),
                    new DungeonEnemy(position: 10, @class: ClassType.Warrior, level: 210, strength: 3720, dexterity: 1155, intelligence: 1080, constitution: 15840, luck: 1020, health: 16711200, minWeaponDmg: 2293, maxWeaponDmg: 4412, armor: 4530 )
                }
            },
            new Dungeon
            {
                Name = DungeonNames.RuinsOfGnark,
                Position = 103,
                IsDefeated = false,
                IsUnlocked = true,
                Type = DungeonTypeEnum.Shadow,
                UnlockResolve = c => c.Dungeons.First(d => d.Position == 102).DungeonEnemies.First(e => e.Position == 5).IsDefeated,
                DungeonEnemies = new List<DungeonEnemy>
                {
                    new DungeonEnemy(position: 1, @class: ClassType.Scout, level: 212, strength: 1026, dexterity: 3219, intelligence: 1066, constitution: 10054, luck: 1358, health: 8566008, minWeaponDmg: 2876, maxWeaponDmg: 5648, armor: 1070 ),
                    new DungeonEnemy(position: 2, @class: ClassType.Scout, level: 214, strength: 838, dexterity: 3578, intelligence: 885, constitution: 11242, luck: 1367, health: 9668120, minWeaponDmg: 2874, maxWeaponDmg: 5717, armor: 3350 ),
                    new DungeonEnemy(position: 3, @class: ClassType.Scout, level: 216, strength: 1054, dexterity: 3733, intelligence: 1152, constitution: 11396, luck: 1270, health: 9891728, minWeaponDmg: 2924, maxWeaponDmg: 5743, armor: 1620 ),
                    new DungeonEnemy(position: 4, @class: ClassType.Warrior, level: 218, strength: 3639, dexterity: 1018, intelligence: 867, constitution: 14047, luck: 1180, health: 15381465, minWeaponDmg: 2340, maxWeaponDmg: 4669, armor: 2700 ),
                    new DungeonEnemy(position: 5, @class: ClassType.Warrior, level: 220, strength: 3748, dexterity: 1079, intelligence: 977, constitution: 14338, luck: 1059, health: 15843490, minWeaponDmg: 2402, maxWeaponDmg: 4680, armor: 3270 ),
                    new DungeonEnemy(position: 6, @class: ClassType.Scout, level: 222, strength: 999, dexterity: 3848, intelligence: 962, constitution: 15466, luck: 1387, health: 13795672, minWeaponDmg: 2987, maxWeaponDmg: 5911, armor: 2230 ),
                    new DungeonEnemy(position: 7, @class: ClassType.Warrior, level: 224, strength: 3920, dexterity: 1190, intelligence: 1102, constitution: 16170, luck: 1085, health: 18191250, minWeaponDmg: 2403, maxWeaponDmg: 4785, armor: 3650 ),
                    new DungeonEnemy(position: 8, @class: ClassType.Scout, level: 226, strength: 1040, dexterity: 4044, intelligence: 1011, constitution: 17660, luck: 1471, health: 16035280, minWeaponDmg: 3048, maxWeaponDmg: 6051, armor: 2500 ),
                    new DungeonEnemy(position: 9, @class: ClassType.Warrior, level: 228, strength: 4135, dexterity: 1338, intelligence: 1272, constitution: 18662, luck: 1113, health: 21367990, minWeaponDmg: 2447, maxWeaponDmg: 4864, armor: 4680 ),
                    new DungeonEnemy(position: 10, @class: ClassType.Warrior, level: 230, strength: 4191, dexterity: 1367, intelligence: 1303, constitution: 19112, luck: 1124, health: 22074360, minWeaponDmg: 2470, maxWeaponDmg: 4911, armor: 4730 )
                }
            },
            new Dungeon
            {
                Name = DungeonNames.CutthroatGrotto,
                Position = 104,
                IsDefeated = false,
                IsUnlocked = true,
                UnlockResolve = c => c.Dungeons.First(d => d.Position == 103).DungeonEnemies.First(e => e.Position == 5).IsDefeated,
                Type = DungeonTypeEnum.Shadow,
                DungeonEnemies = new List<DungeonEnemy>
                {
                    new DungeonEnemy(position: 1, @class: ClassType.Scout, level: 232, strength: 1026, dexterity: 3926, intelligence: 981, constitution: 14746, luck: 1405, health: 13743272, minWeaponDmg: 3135, maxWeaponDmg: 6199, armor: 1600 ),
                    new DungeonEnemy(position: 2, @class: ClassType.Scout, level: 234, strength: 1048, dexterity: 4034, intelligence: 1008, constitution: 15972, luck: 1452, health: 15013680, minWeaponDmg: 3138, maxWeaponDmg: 6252, armor: 2300 ),
                    new DungeonEnemy(position: 3, @class: ClassType.Warrior, level: 236, strength: 4110, dexterity: 1237, intelligence: 1141, constitution: 16748, luck: 1141, health: 19846380, minWeaponDmg: 2528, maxWeaponDmg: 5046, armor: 3720 ),
                    new DungeonEnemy(position: 4, @class: ClassType.Warrior, level: 238, strength: 4200, dexterity: 1295, intelligence: 1207, constitution: 17710, luck: 1155, health: 21163450, minWeaponDmg: 2559, maxWeaponDmg: 5078, armor: 3560 ),
                    new DungeonEnemy(position: 5, @class: ClassType.Warrior, level: 240, strength: 4281, dexterity: 1345, intelligence: 1264, constitution: 18546, luck: 1167, health: 22347930, minWeaponDmg: 2706, maxWeaponDmg: 5100, armor: 4320 ),
                    new DungeonEnemy(position: 6, @class: ClassType.Scout, level: 242, strength: 1121, dexterity: 4367, intelligence: 1091, constitution: 19476, luck: 1593, health: 18930672, minWeaponDmg: 3244, maxWeaponDmg: 6474, armor: 2630 ),
                    new DungeonEnemy(position: 7, @class: ClassType.Warrior, level: 244, strength: 4415, dexterity: 1423, intelligence: 1350, constitution: 19806, luck: 1190, health: 24262350, minWeaponDmg: 2613, maxWeaponDmg: 5210, armor: 5070 ),
                    new DungeonEnemy(position: 8, @class: ClassType.Warrior, level: 246, strength: 4510, dexterity: 1486, intelligence: 1422, constitution: 20856, luck: 1204, health: 25757160, minWeaponDmg: 2641, maxWeaponDmg: 5263, armor: 4930 ),
                    new DungeonEnemy(position: 9, @class: ClassType.Scout, level: 248, strength: 1167, dexterity: 4570, intelligence: 1142, constitution: 21395, luck: 1677, health: 21309420, minWeaponDmg: 3320, maxWeaponDmg: 6639, armor: 3650 ),
                    new DungeonEnemy(position: 10, @class: ClassType.Scout, level: 250, strength: 1181, dexterity: 4636, intelligence: 1159, constitution: 22000, luck: 1704, health: 22088000, minWeaponDmg: 3418, maxWeaponDmg: 6623, armor: 3400 )
                }
            },
            new Dungeon
            {
                Name = DungeonNames.EmeraldScaleAltar,
                Position = 105,
                IsDefeated = false,
                IsUnlocked = true,
                UnlockResolve = c => c.Dungeons.First(d => d.Position == 104).DungeonEnemies.First(e => e.Position == 5).IsDefeated,
                Type = DungeonTypeEnum.Shadow,
                DungeonEnemies = new List<DungeonEnemy>
                {
                    new DungeonEnemy(position: 1, @class: ClassType.Scout, level: 252, strength: 1155, dexterity: 4480, intelligence: 1120, constitution: 19250, luck: 1627, health: 19481000, minWeaponDmg: 3422, maxWeaponDmg: 6653, armor: 4460 ),
                    new DungeonEnemy(position: 2, @class: ClassType.Scout, level: 254, strength: 1172, dexterity: 4558, intelligence: 1139, constitution: 20058, luck: 1660, health: 20459160, minWeaponDmg: 3429, maxWeaponDmg: 6773, armor: 3000 ),
                    new DungeonEnemy(position: 3, @class: ClassType.Scout, level: 256, strength: 1184, dexterity: 4608, intelligence: 1152, constitution: 20416, luck: 1680, health: 20987648, minWeaponDmg: 3476, maxWeaponDmg: 6810, armor: 2950 ),
                    new DungeonEnemy(position: 4, @class: ClassType.Scout, level: 258, strength: 1202, dexterity: 4690, intelligence: 1172, constitution: 21285, luck: 1715, health: 22051260, minWeaponDmg: 3641, maxWeaponDmg: 6896, armor: 2900 ),
                    new DungeonEnemy(position: 5, @class: ClassType.Warrior, level: 260, strength: 4757, dexterity: 1562, intelligence: 1493, constitution: 21901, luck: 1272, health: 28580804, minWeaponDmg: 2786, maxWeaponDmg: 5562, armor: 5050 ),
                    new DungeonEnemy(position: 6, @class: ClassType.Scout, level: 262, strength: 1231, dexterity: 4820, intelligence: 1205, constitution: 22478, luck: 1768, health: 23646856, minWeaponDmg: 3508, maxWeaponDmg: 6997, armor: 2790 ),
                    new DungeonEnemy(position: 7, @class: ClassType.Warrior, level: 264, strength: 4888, dexterity: 1637, intelligence: 1576, constitution: 23122, luck: 1295, health: 30636650, minWeaponDmg: 2874, maxWeaponDmg: 5627, armor: 6030 ),
                    new DungeonEnemy(position: 8, @class: ClassType.Mage, level: 266, strength: 1213, dexterity: 1260, intelligence: 5133, constitution: 22583, luck: 1808, health: 12059322, minWeaponDmg: 6556, maxWeaponDmg: 12786, armor: 6220 ),
                    new DungeonEnemy(position: 9, @class: ClassType.Warrior, level: 268, strength: 5008, dexterity: 1702, intelligence: 1647, constitution: 24162, luck: 1318, health: 32497890, minWeaponDmg: 2874, maxWeaponDmg: 5726, armor: 6270 ),
                    new DungeonEnemy(position: 10, @class: ClassType.Scout, level: 270, strength: 1287, dexterity: 5067, intelligence: 1266, constitution: 24673, luck: 1869, health: 26745532, minWeaponDmg: 3619, maxWeaponDmg: 7224, armor: 4500 )
                }
            },
            new Dungeon
            {
                Name = DungeonNames.ToxicTree,
                Position = 106,
                IsDefeated = false,
                IsUnlocked = true,
                UnlockResolve = c => c.Dungeons.First(d => d.Position == 106).DungeonEnemies.First(e => e.Position == 5).IsDefeated,
                Type = DungeonTypeEnum.Shadow,
                DungeonEnemies = new List<DungeonEnemy>
                {
                    new DungeonEnemy(position: 1, @class: ClassType.Warrior, level: 272, strength: 4966, dexterity: 1626, intelligence: 1552, constitution: 22764, luck: 1330, health: 31072860, minWeaponDmg: 3013, maxWeaponDmg: 5695, armor: 4750 ),
                    new DungeonEnemy(position: 2, @class: ClassType.Scout, level: 274, strength: 1286, dexterity: 5032, intelligence: 1258, constitution: 23370, luck: 1845, health: 25707000, minWeaponDmg: 3681, maxWeaponDmg: 7276, armor: 3320 ),
                    new DungeonEnemy(position: 3, @class: ClassType.Mage, level: 276, strength: 1247, dexterity: 1300, intelligence: 5307, constitution: 22770, luck: 1857, health: 12614580, minWeaponDmg: 6792, maxWeaponDmg: 13218, armor: 3580 ),
                    new DungeonEnemy(position: 4, @class: ClassType.Scout, level: 278, strength: 1311, dexterity: 5140, intelligence: 1285, constitution: 24233, luck: 1888, health: 27044028, minWeaponDmg: 3740, maxWeaponDmg: 7390, armor: 4000 ),
                    new DungeonEnemy(position: 5, @class: ClassType.Scout, level: 280, strength: 1328, dexterity: 5220, intelligence: 1305, constitution: 25052, luck: 1922, health: 28158448, minWeaponDmg: 3747, maxWeaponDmg: 7490, armor: 5390 ),
                    new DungeonEnemy(position: 6, @class: ClassType.Warrior, level: 282, strength: 5276, dexterity: 1796, intelligence: 1739, constitution: 25514, luck: 1387, health: 36102312, minWeaponDmg: 3031, maxWeaponDmg: 6009, armor: 6226 ),
                    new DungeonEnemy(position: 7, @class: ClassType.Scout, level: 284, strength: 1353, dexterity: 5325, intelligence: 1331, constitution: 25866, luck: 1963, health: 29487240, minWeaponDmg: 3800, maxWeaponDmg: 7597, armor: 5510 ),
                    new DungeonEnemy(position: 8, @class: ClassType.Mage, level: 286, strength: 1324, dexterity: 1366, intelligence: 5551, constitution: 25443, luck: 1976, health: 14604282, minWeaponDmg: 6923, maxWeaponDmg: 13659, armor: 4440 ),
                    new DungeonEnemy(position: 9, @class: ClassType.Scout, level: 288, strength: 1380, dexterity: 5440, intelligence: 1360, constitution: 26840, luck: 2010, health: 31027040, minWeaponDmg: 3853, maxWeaponDmg: 7676, armor: 5840 ),
                    new DungeonEnemy(position: 10, @class: ClassType.Scout, level: 290, strength: 1392, dexterity: 5490, intelligence: 1372, constitution: 27220, luck: 2030, health: 31684080, minWeaponDmg: 3903, maxWeaponDmg: 7743, armor: 4570 )
                }
            },
            new Dungeon
            {
                Name = DungeonNames.MagmaStream,
                Position = 107,
                IsDefeated = false,
                IsUnlocked = true,
                UnlockResolve = c => c.Dungeons.First(d => d.Position == 106).DungeonEnemies.First(e => e.Position == 5).IsDefeated,
                Type = DungeonTypeEnum.Shadow,
                DungeonEnemies = new List<DungeonEnemy>
                {
                    new DungeonEnemy(position: 1, @class: ClassType.Scout, level: 292, strength: 1381, dexterity: 5422, intelligence: 1355, constitution: 25806, luck: 1994, health: 30244632, minWeaponDmg: 3908, maxWeaponDmg: 7812, armor: 2804 ),
                    new DungeonEnemy(position: 2, @class: ClassType.Scout, level: 294, strength: 1393, dexterity: 5474, intelligence: 1368, constitution: 26202, luck: 2014, health: 30918360, minWeaponDmg: 3935, maxWeaponDmg: 7864, armor: 2913 ),
                    new DungeonEnemy(position: 3, @class: ClassType.Mage, level: 296, strength: 1356, dexterity: 1406, intelligence: 5722, constitution: 25504, luck: 2022, health: 15149376, minWeaponDmg: 7161, maxWeaponDmg: 14254, armor: 1205 ),
                    new DungeonEnemy(position: 4, @class: ClassType.Warrior, level: 298, strength: 5581, dexterity: 1903, intelligence: 1844, constitution: 27054, luck: 1466, health: 40445728, minWeaponDmg: 3190, maxWeaponDmg: 6377, armor: 6308 ),
                    new DungeonEnemy(position: 5, @class: ClassType.Warrior, level: 300, strength: 5641, dexterity: 1936, intelligence: 1880, constitution: 27577, luck: 1477, health: 41503384, minWeaponDmg: 3213, maxWeaponDmg: 6419, armor: 6708 ),
                    new DungeonEnemy(position: 6, @class: ClassType.Warrior, level: 302, strength: 5689, dexterity: 1958, intelligence: 1903, constitution: 27924, luck: 1488, health: 42304860, minWeaponDmg: 3235, maxWeaponDmg: 6460, armor: 6903 ),
                    new DungeonEnemy(position: 7, @class: ClassType.Mage, level: 304, strength: 1412, dexterity: 1455, intelligence: 5908, constitution: 27313, luck: 2108, health: 16660930, minWeaponDmg: 7333, maxWeaponDmg: 14626, armor: 1430 ),
                    new DungeonEnemy(position: 8, @class: ClassType.Warrior, level: 306, strength: 5784, dexterity: 2001, intelligence: 1949, constitution: 28584, luck: 1509, health: 43876440, minWeaponDmg: 3277, maxWeaponDmg: 6544, armor: 7303 ),
                    new DungeonEnemy(position: 9, @class: ClassType.Warrior, level: 308, strength: 5827, dexterity: 2018, intelligence: 1966, constitution: 28842, luck: 1519, health: 44560888, minWeaponDmg: 3297, maxWeaponDmg: 6588, armor: 7418 ),
                    new DungeonEnemy(position: 10, @class: ClassType.Warrior, level: 310, strength: 5908, dexterity: 2069, intelligence: 2024, constitution: 29684, luck: 1531, health: 46158620, minWeaponDmg: 3317, maxWeaponDmg: 6624, armor: 8504 )
                }
            },
            new Dungeon
            {
                Name = DungeonNames.FrostBloodTemple,
                Position = 108,
                IsDefeated = false,
                IsUnlocked = true,
                UnlockResolve = c => c.Dungeons.First(d => d.Position == 107).DungeonEnemies.First(e => e.Position == 5).IsDefeated,
                Type = DungeonTypeEnum.Shadow,
                DungeonEnemies = new List<DungeonEnemy>
                {
                    new DungeonEnemy(position: 1, @class: ClassType.Warrior, level: 312, strength: 5861, dexterity: 2009, intelligence: 1950, constitution: 28600, luck: 1536, health: 44759000, minWeaponDmg: 3343, maxWeaponDmg: 6676, armor: 6643 ),
                    new DungeonEnemy(position: 2, @class: ClassType.Scout, level: 314, strength: 1502, dexterity: 5921, intelligence: 1480, constitution: 29112, luck: 2186, health: 36681120, minWeaponDmg: 4202, maxWeaponDmg: 8396, armor: 3542 ),
                    new DungeonEnemy(position: 3, @class: ClassType.Warrior, level: 316, strength: 5991, dexterity: 2082, intelligence: 2031, constitution: 29794, luck: 1559, health: 47223488, minWeaponDmg: 3383, maxWeaponDmg: 6762, armor: 7791 ),
                    new DungeonEnemy(position: 4, @class: ClassType.Scout, level: 318, strength: 1529, dexterity: 6037, intelligence: 1509, constitution: 30107, luck: 2234, health: 38416532, minWeaponDmg: 4253, maxWeaponDmg: 8490, armor: 4043 ),
                    new DungeonEnemy(position: 5, @class: ClassType.Scout, level: 320, strength: 1541, dexterity: 6087, intelligence: 1521, constitution: 30476, luck: 2253, health: 39131184, minWeaponDmg: 4281, maxWeaponDmg: 8558, armor: 4260 ),
                    new DungeonEnemy(position: 6, @class: ClassType.Mage, level: 322, strength: 1514, dexterity: 1552, intelligence: 6286, constitution: 29936, luck: 2261, health: 19338656, minWeaponDmg: 7752, maxWeaponDmg: 15493, armor: 1859 ),
                    new DungeonEnemy(position: 7, @class: ClassType.Warrior, level: 324, strength: 6178, dexterity: 2166, intelligence: 2119, constitution: 31080, luck: 1601, health: 50505000, minWeaponDmg: 3468, maxWeaponDmg: 6932, armor: 8626 ),
                    new DungeonEnemy(position: 8, @class: ClassType.Scout, level: 326, strength: 1575, dexterity: 6230, intelligence: 1557, constitution: 31476, luck: 2309, health: 41170608, minWeaponDmg: 4374, maxWeaponDmg: 8717, armor: 4584 ),
                    new DungeonEnemy(position: 9, @class: ClassType.Mage, level: 328, strength: 1551, dexterity: 1586, intelligence: 6418, constitution: 31009, luck: 2313, health: 20403922, minWeaponDmg: 7923, maxWeaponDmg: 15777, armor: 1876 ),
                    new DungeonEnemy(position: 10, @class: ClassType.Mage, level: 330, strength: 1567, dexterity: 1600, intelligence: 6468, constitution: 31581, luck: 2326, health: 20906622, minWeaponDmg: 7962, maxWeaponDmg: 15873, armor: 2119 )
                }
            },
            new Dungeon
            {
                Name = DungeonNames.PyramidsOfMadness,
                Position = 109,
                IsDefeated = false,
                IsUnlocked = true,
                UnlockResolve = c => c.Dungeons.First(d => d.Position == 108).DungeonEnemies.First(e => e.Position == 5).IsDefeated,
                Type = DungeonTypeEnum.Shadow,
                DungeonEnemies = new List<DungeonEnemy>
                {
                    new DungeonEnemy(position: 1, @class: ClassType.Warrior, level: 332, strength: 6290, dexterity: 2184, intelligence: 2129, constitution: 31229, luck: 1638, health: 51996284, minWeaponDmg: 3552, maxWeaponDmg: 7099, armor: 7621 ),
                    new DungeonEnemy(position: 2, @class: ClassType.Mage, level: 334, strength: 1562, dexterity: 1605, intelligence: 6508, constitution: 30613, luck: 2333, health: 20510710, minWeaponDmg: 8052, maxWeaponDmg: 16062, armor: 1605 ),
                    new DungeonEnemy(position: 3, @class: ClassType.Scout, level: 336, strength: 1617, dexterity: 6384, intelligence: 1596, constitution: 31878, luck: 2362, health: 42971544, minWeaponDmg: 4493, maxWeaponDmg: 8978, armor: 4029 ),
                    new DungeonEnemy(position: 4, @class: ClassType.Warrior, level: 338, strength: 6426, dexterity: 2242, intelligence: 2190, constitution: 32126, luck: 1669, health: 54453568, minWeaponDmg: 3618, maxWeaponDmg: 7226, armor: 8113 ),
                    new DungeonEnemy(position: 5, @class: ClassType.Mage, level: 340, strength: 1597, dexterity: 1638, intelligence: 6636, constitution: 31537, luck: 2386, health: 21508234, minWeaponDmg: 8187, maxWeaponDmg: 16361, armor: 1662 ),
                    new DungeonEnemy(position: 6, @class: ClassType.Warrior, level: 342, strength: 6525, dexterity: 2289, intelligence: 2240, constitution: 32862, luck: 1690, health: 56358328, minWeaponDmg: 3659, maxWeaponDmg: 7313, armor: 8711 ),
                    new DungeonEnemy(position: 7, @class: ClassType.Scout, level: 344, strength: 1661, dexterity: 6567, intelligence: 1641, constitution: 33110, luck: 2433, health: 45691800, minWeaponDmg: 4600, maxWeaponDmg: 9187, armor: 4427 ),
                    new DungeonEnemy(position: 8, @class: ClassType.Scout, level: 346, strength: 1671, dexterity: 6608, intelligence: 1652, constitution: 33352, luck: 2449, health: 46292576, minWeaponDmg: 4636, maxWeaponDmg: 9250, armor: 4462 ),
                    new DungeonEnemy(position: 9, @class: ClassType.Mage, level: 348, strength: 1648, dexterity: 1685, intelligence: 6813, constitution: 33038, luck: 2454, health: 23060524, minWeaponDmg: 8377, maxWeaponDmg: 16743, armor: 1932 ),
                    new DungeonEnemy(position: 10, @class: ClassType.Mirror, level: 350, strength: 0, dexterity: 0, intelligence: 0, constitution: 0, luck: 0, health: 0, minWeaponDmg: 0, maxWeaponDmg: 0, armor: 0)
                }
            },
            new Dungeon
            {
                Name = DungeonNames.BlackSkullFortress,
                Position = 110,
                IsDefeated = false,
                IsUnlocked = true,
                UnlockResolve = c => c.Dungeons.First(d => d.Position == 109).DungeonEnemies.First(e => e.Position == 5).IsDefeated,
                Type = DungeonTypeEnum.Shadow,
                DungeonEnemies = new List<DungeonEnemy>
                {
                    new DungeonEnemy(position: 1, @class: ClassType.Scout, level: 352, strength: 1708, dexterity: 6765, intelligence: 1691, constitution: 34562, luck: 2489, health: 48801544, minWeaponDmg: 4747, maxWeaponDmg: 9412 ),
                    new DungeonEnemy(position: 2, @class: ClassType.Warrior, level: 354, strength: 6810, dexterity: 2393, intelligence: 2351, constitution: 34859, luck: 1702, health: 61874724, minWeaponDmg: 3793, maxWeaponDmg: 7572 ),
                    new DungeonEnemy(position: 3, @class: ClassType.Warrior, level: 356, strength: 6855, dexterity: 2409, intelligence: 2367, constitution: 35150, luck: 1705, health: 62742752, minWeaponDmg: 3845, maxWeaponDmg: 7615 ),
                    new DungeonEnemy(position: 4,  @class: ClassType.Warrior, level: 358, strength: 6899, dexterity: 2424, intelligence: 2383, constitution: 35442, luck: 1708, health: 63618392, minWeaponDmg: 3829, maxWeaponDmg: 7653 ),
                    new DungeonEnemy(position: 5,  @class: ClassType.Scout, level: 360, strength: 1752, dexterity: 6944, intelligence: 1736, constitution: 35728, luck: 2544, health: 51591232, minWeaponDmg: 4813, maxWeaponDmg: 9626 ),
                    new DungeonEnemy(position: 6, @class: ClassType.Scout, level: 362, strength: 1762, dexterity: 6988, intelligence: 1747, constitution: 36008, luck: 2557, health: 52283616, minWeaponDmg: 4847, maxWeaponDmg: 9672 ),
                    new DungeonEnemy(position: 7, @class: ClassType.Mage, level: 364, strength: 1742, dexterity: 1773, intelligence: 7156, constitution: 35607, luck: 2563, health: 25993110, minWeaponDmg: 9198, maxWeaponDmg: 17512 ),
                    new DungeonEnemy(position: 8, @class: ClassType.Warrior, level: 366, strength: 7076, dexterity: 2485, intelligence: 2447, constitution: 36570, luck: 1723, health: 67105952, minWeaponDmg: 3916, maxWeaponDmg: 7591 ),
                    new DungeonEnemy(position: 9, @class: ClassType.Warrior, level: 368, strength: 7119, dexterity: 2500, intelligence: 2463, constitution: 36844, luck: 1727, health: 67977184, minWeaponDmg: 3952, maxWeaponDmg: 7833 ),
                    new DungeonEnemy(position: 10, @class: ClassType.Warrior, level: 370, strength: 7163, dexterity: 2516, intelligence: 2479, constitution: 37114, luck: 1731, health: 68846472, minWeaponDmg: 3964, maxWeaponDmg: 7873 )
                }
            },
            new Dungeon
            {
                Name = DungeonNames.CircusOfTerror,
                Position = 111,
                IsDefeated = false,
                IsUnlocked = true,
                UnlockResolve = c => c.Dungeons.First(d => d.Position == 110).DungeonEnemies.First(e => e.Position == 5).IsDefeated,
                Type = DungeonTypeEnum.Shadow,
                DungeonEnemies = new List<DungeonEnemy>
                {
                    new DungeonEnemy(position: 1, @class: ClassType.Warrior, level: 372, strength: 7206, dexterity: 2531, intelligence: 2494, constitution: 37389, luck: 1736, health: 69730488, minWeaponDmg: 3980, maxWeaponDmg: 7945, armor: 12750 ),
                    new DungeonEnemy(position: 2, @class: ClassType.Scout, level: 374, strength: 1826, dexterity: 7249, intelligence: 1812, constitution: 37658, luck: 2639, health: 56487000, minWeaponDmg: 5024, maxWeaponDmg: 9980, armor: 6500 ),
                    new DungeonEnemy(position: 3, @class: ClassType.Warrior, level: 376, strength: 7292, dexterity: 2561, intelligence: 2525, constitution: 37922, luck: 1745, health: 71482968, minWeaponDmg: 4082, maxWeaponDmg: 8000, armor: 13250 ),
                    new DungeonEnemy(position: 4, @class: ClassType.Warrior, level: 378, strength: 7336, dexterity: 2576, intelligence: 2541, constitution: 38192, luck: 1750, health: 72373840, minWeaponDmg: 4055, maxWeaponDmg: 8100, armor: 13500 ),
                    new DungeonEnemy(position: 5, @class: ClassType.Mage, level: 380, strength: 1830, dexterity: 1858, intelligence: 7489, constitution: 37846, luck: 2673, health: 28838652, minWeaponDmg: 9157, maxWeaponDmg: 18000, armor: 2750 ),
                    new DungeonEnemy(position: 6, @class: ClassType.Scout, level: 382, strength: 1869, dexterity: 7421, intelligence: 1855, constitution: 38714, luck: 2694, health: 59309848, minWeaponDmg: 5100, maxWeaponDmg: 10200, armor: 7000 ),
                    new DungeonEnemy(position: 7, @class: ClassType.Warrior, level: 384, strength: 7464, dexterity: 2620, intelligence: 2586, constitution: 38978, luck: 1765, health: 75032648, minWeaponDmg: 4115, maxWeaponDmg: 8191, armor: 14250 ),
                    new DungeonEnemy(position: 8, @class: ClassType.Scout, level: 386, strength: 1890, dexterity: 7507, intelligence: 1876, constitution: 39237, luck: 2721, health: 60738876, minWeaponDmg: 5162, maxWeaponDmg: 10319, armor: 7250 ),
                    new DungeonEnemy(position: 9, @class: ClassType.Mage, level: 388, strength: 1874, dexterity: 1900, intelligence: 7654, constitution: 38918, luck: 2729, health: 30278204, minWeaponDmg: 9600, maxWeaponDmg: 18600, armor: 2800 ),
                    new DungeonEnemy(position: 10, @class: ClassType.Warrior, level: 390, strength: 7592, dexterity: 2665, intelligence: 2632, constitution: 39754, luck: 1781, health: 77719072, minWeaponDmg: 4180, maxWeaponDmg: 8336, armor: 15000 )
                }
            },
            new Dungeon
            {
                Name = DungeonNames.Hell,
                Position = 112,
                IsDefeated = false,
                IsUnlocked = true,
                UnlockResolve = c => c.Dungeons.First(d => d.Position == 111).DungeonEnemies.First(e => e.Position == 5).IsDefeated,
                Type = DungeonTypeEnum.Shadow,
                DungeonEnemies = new List<DungeonEnemy>
                {
                    new DungeonEnemy(position: 1, @class: ClassType.Mage, level: 392, strength: 1895, dexterity: 1921, intelligence: 7737, constitution: 39440, luck: 2756, health: 30999840, minWeaponDmg: 9910, maxWeaponDmg: 18710 ),
                    new DungeonEnemy(position: 2, @class: ClassType.Mage, level: 394, strength: 1906, dexterity: 1931, intelligence: 7778, constitution: 39704, luck: 2770, health: 31366160, minWeaponDmg: 9918, maxWeaponDmg: 18819 ),
                    new DungeonEnemy(position: 3, @class: ClassType.Warrior, level: 396, strength: 7718, dexterity: 2709, intelligence: 2677, constitution: 40513, luck: 1797, health: 80418304, minWeaponDmg: 4237, maxWeaponDmg: 8464 ),
                    new DungeonEnemy(position: 4, @class: ClassType.Scout, level: 398, strength: 1952, dexterity: 7761, intelligence: 1940, constitution: 40766, luck: 2804, health: 65062536, minWeaponDmg: 5335, maxWeaponDmg: 10577 ),
                    new DungeonEnemy(position: 5, @class: ClassType.Mage, level: 400, strength: 1938, dexterity: 1963, intelligence: 7901, constitution: 40480, luck: 2812, health: 32464960, minWeaponDmg: 9869, maxWeaponDmg: 19019 ),
                    new DungeonEnemy(position: 6, @class: ClassType.Warrior, level: 404, strength: 7884, dexterity: 2766, intelligence: 2736, constitution: 41476, luck: 1824, health: 83988896, minWeaponDmg: 4343, maxWeaponDmg: 8638 ),
                    new DungeonEnemy(position: 7, @class: ClassType.Scout, level: 408, strength: 2003, dexterity: 7965, intelligence: 1991, constitution: 41932, luck: 2874, health: 68600752, minWeaponDmg: 5473, maxWeaponDmg: 10904 ),
                    new DungeonEnemy(position: 8, @class: ClassType.Warrior, level: 412, strength: 8046, dexterity: 2823, intelligence: 2793, constitution: 42383, luck: 1854, health: 87520896, minWeaponDmg: 4431, maxWeaponDmg: 8806 ),
                    new DungeonEnemy(position: 9, @class: ClassType.Warrior, level: 416, strength: 8127, dexterity: 2851, intelligence: 2821, constitution: 42840, luck: 1868, health: 89321400, minWeaponDmg: 4609, maxWeaponDmg: 8868 ),
                    new DungeonEnemy(position: 10, @class: ClassType.Warrior, level: 420, strength: 8208, dexterity: 2880, intelligence: 2850, constitution: 43296, luck: 1884, health: 91138080, minWeaponDmg: 4499, maxWeaponDmg: 8979 )
                }
            },
            new Dungeon
            {
                Name = DungeonNames.DragonsHoard,
                Position = 113,
                IsDefeated = false,
                IsUnlocked = true,
                UnlockResolve = c => c.CharacterLevel >= 320,
                Type = DungeonTypeEnum.Shadow,
                DungeonEnemies = new List<DungeonEnemy>
                {
                    new DungeonEnemy(position: 1, @class: ClassType.Scout, level: 320, strength: 9700, dexterity: 19400, intelligence: 9700, constitution: 65500, luck: 9700, health: 181000000, minWeaponDmg: 981, maxWeaponDmg: 1226, dungeonRuneBonuses: new DungeonEnemyRuneBonuses { DamageRuneType = RuneType.FireDamage, FireResistance = 25, ColdResistance = 0, LightningResistance = 0, DamageBonus = 50 }, armor: 19750 ),
                    new DungeonEnemy(position: 2, @class: ClassType.Warrior, level: 323, strength: 19760, dexterity: 9880, intelligence: 9880, constitution: 67000, luck: 9880, health: 185500000, minWeaponDmg: 994, maxWeaponDmg: 1242, dungeonRuneBonuses: new DungeonEnemyRuneBonuses { DamageRuneType = RuneType.LightningDamage, FireResistance = 0, ColdResistance = 0, LightningResistance = 25, DamageBonus = 50 }, armor: 20000 ),
                    new DungeonEnemy(position: 3, @class: ClassType.Mage, level: 326, strength: 10060, dexterity: 10060, intelligence: 20120, constitution: 68500, luck: 10060, health: 190000000, minWeaponDmg: 1005, maxWeaponDmg: 1252, dungeonRuneBonuses: new DungeonEnemyRuneBonuses { DamageRuneType = RuneType.LightningDamage, FireResistance = 0, ColdResistance = 0, LightningResistance = 25, DamageBonus = 50 }, armor: 20250 ),
                    new DungeonEnemy(position: 4, @class: ClassType.Scout, level: 329, strength: 10240, dexterity: 20480, intelligence: 10240, constitution: 70000, luck: 10240, health: 194500000, minWeaponDmg: 1019, maxWeaponDmg: 1265, dungeonRuneBonuses: new DungeonEnemyRuneBonuses { DamageRuneType = RuneType.ColdDamage, FireResistance = 0, ColdResistance = 25, LightningResistance = 0, DamageBonus = 50 }, armor: 20500 ),
                    new DungeonEnemy(position: 5, @class: ClassType.Scout, level: 332, strength: 10420, dexterity: 20840, intelligence: 10420, constitution: 71500, luck: 10420, health: 199000000, minWeaponDmg: 1031, maxWeaponDmg: 1277, dungeonRuneBonuses: new DungeonEnemyRuneBonuses { DamageRuneType = RuneType.FireDamage, FireResistance = 25, ColdResistance = 0, LightningResistance = 0, DamageBonus = 50 }, armor: 20750 ),
                    new DungeonEnemy(position: 6, @class: ClassType.Scout, level: 335, strength: 10600, dexterity: 21200, intelligence: 10600, constitution: 73000, luck: 10600, health: 203500000, minWeaponDmg: 1045, maxWeaponDmg: 1284, dungeonRuneBonuses: new DungeonEnemyRuneBonuses { DamageRuneType = RuneType.LightningDamage, FireResistance = 0, ColdResistance = 0, LightningResistance = 25, DamageBonus = 50 }, armor: 21000 ),
                    new DungeonEnemy(position: 7, @class: ClassType.Warrior, level: 338, strength: 21560, dexterity: 10780, intelligence: 10780, constitution: 74500, luck: 10780, health: 208000000, minWeaponDmg: 1053, maxWeaponDmg: 1301, dungeonRuneBonuses: new DungeonEnemyRuneBonuses { DamageRuneType = RuneType.LightningDamage, FireResistance = 0, ColdResistance = 0, LightningResistance = 25, DamageBonus = 50 }, armor: 21250 ),
                    new DungeonEnemy(position: 8, @class: ClassType.Warrior, level: 341, strength: 21920, dexterity: 10960, intelligence: 10960, constitution: 76000, luck: 10960, health: 212500000, minWeaponDmg: 1064, maxWeaponDmg: 1308, dungeonRuneBonuses: new DungeonEnemyRuneBonuses { DamageRuneType = RuneType.FireDamage, FireResistance = 25, ColdResistance = 0, LightningResistance = 0, DamageBonus = 50 }, armor: 21500 ),
                    new DungeonEnemy(position: 9, @class: ClassType.Scout, level: 344, strength: 11140, dexterity: 22280, intelligence: 11140, constitution: 77500, luck: 11140, health: 217000000, minWeaponDmg: 1084, maxWeaponDmg: 1324, dungeonRuneBonuses: new DungeonEnemyRuneBonuses { DamageRuneType = RuneType.ColdDamage, FireResistance = 0, ColdResistance = 25, LightningResistance = 0, DamageBonus = 50 }, armor: 21750 ),
                    new DungeonEnemy(position: 10, @class: ClassType.Mage, level: 350, strength: 11500, dexterity: 11500, intelligence: 23000, constitution: 80500, luck: 11500, health: 226000000, minWeaponDmg: 1100, maxWeaponDmg: 1350, dungeonRuneBonuses: new DungeonEnemyRuneBonuses { DamageRuneType = RuneType.ColdDamage, FireResistance = 0, ColdResistance = 25, LightningResistance = 0, DamageBonus = 50 }, armor: 22000 )
                }
            },
            new Dungeon
            {
                Name = DungeonNames.HouseOfHorrors,
                Position = 114,
                IsDefeated = false,
                IsUnlocked = true,
                UnlockResolve = c => c.CharacterLevel >= 350,
                Type = DungeonTypeEnum.Shadow,
                DungeonEnemies = new List<DungeonEnemy>
                {
                    new DungeonEnemy(position: 1, @class: ClassType.Mage, level: 350, strength: 11500, dexterity: 11500, intelligence: 23000, constitution: 80500, luck: 11500, health: 226000000, minWeaponDmg: 1102, maxWeaponDmg: 1349, dungeonRuneBonuses: new DungeonEnemyRuneBonuses { DamageRuneType = RuneType.ColdDamage, FireResistance = 0, ColdResistance = 25, LightningResistance = 0, DamageBonus = 50 }, armor: 22250 ),
                    new DungeonEnemy(position: 2, @class: ClassType.Warrior, level: 354, strength: 23480, dexterity: 11740, intelligence: 11740, constitution: 82500, luck: 11740, health: 232000000, minWeaponDmg: 1116, maxWeaponDmg: 1363, dungeonRuneBonuses: new DungeonEnemyRuneBonuses { DamageRuneType = RuneType.LightningDamage, FireResistance = 0, ColdResistance = 0, LightningResistance = 25, DamageBonus = 50 }, armor: 22500 ),
                    new DungeonEnemy(position: 3, @class: ClassType.Warrior, level: 358, strength: 23960, dexterity: 11980, intelligence: 11980, constitution: 84500, luck: 11980, health: 238000000, minWeaponDmg: 1140, maxWeaponDmg: 1380, dungeonRuneBonuses: new DungeonEnemyRuneBonuses { DamageRuneType = RuneType.LightningDamage, FireResistance = 0, ColdResistance = 0, LightningResistance = 25, DamageBonus = 50 }, armor: 22750 ),
                    new DungeonEnemy(position: 4, @class: ClassType.Warrior, level: 362, strength: 24440, dexterity: 12220, intelligence: 12220, constitution: 86500, luck: 12220, health: 244000000, minWeaponDmg: 1151, maxWeaponDmg: 1397, dungeonRuneBonuses: new DungeonEnemyRuneBonuses { DamageRuneType = RuneType.FireDamage, FireResistance = 25, ColdResistance = 0, LightningResistance = 0, DamageBonus = 50 }, armor: 23000 ),
                    new DungeonEnemy(position: 5, @class: ClassType.Scout, level: 366, strength: 12460, dexterity: 24920, intelligence: 12460, constitution: 88500, luck: 12460, health: 250000000, minWeaponDmg: 1167, maxWeaponDmg: 1404, dungeonRuneBonuses: new DungeonEnemyRuneBonuses { DamageRuneType = RuneType.ColdDamage, FireResistance = 0, ColdResistance = 25, LightningResistance = 0, DamageBonus = 50 }, armor: 23250 ),
                    new DungeonEnemy(position: 6, @class: ClassType.Mage, level: 370, strength: 12700, dexterity: 12700, intelligence: 25400, constitution: 90500, luck: 12700, health: 256000000, minWeaponDmg: 1181, maxWeaponDmg: 1429, dungeonRuneBonuses: new DungeonEnemyRuneBonuses { DamageRuneType = RuneType.FireDamage, FireResistance = 25, ColdResistance = 0, LightningResistance = 0, DamageBonus = 50 }, armor: 23500 ),
                    new DungeonEnemy(position: 7, @class: ClassType.Scout, level: 374, strength: 12940, dexterity: 25880, intelligence: 12940, constitution: 92500, luck: 12940, health: 262000000, minWeaponDmg: 1199, maxWeaponDmg: 1441, dungeonRuneBonuses: new DungeonEnemyRuneBonuses { DamageRuneType = RuneType.LightningDamage, FireResistance = 0, ColdResistance = 0, LightningResistance = 25, DamageBonus = 50 }, armor: 23750 ),
                    new DungeonEnemy(position: 8, @class: ClassType.Mage, level: 378, strength: 13180, dexterity: 13180, intelligence: 26360, constitution: 94500, luck: 13180, health: 268000000, minWeaponDmg: 1212, maxWeaponDmg: 1461, dungeonRuneBonuses: new DungeonEnemyRuneBonuses { DamageRuneType = RuneType.FireDamage, FireResistance = 25, ColdResistance = 0, LightningResistance = 0, DamageBonus = 50 }, armor: 24000 ),
                    new DungeonEnemy(position: 9, @class: ClassType.Mage, level: 382, strength: 13420, dexterity: 13420, intelligence: 26840, constitution: 96500, luck: 13420, health: 274000000, minWeaponDmg: 1230, maxWeaponDmg: 1474, dungeonRuneBonuses: new DungeonEnemyRuneBonuses { DamageRuneType = RuneType.FireDamage, FireResistance = 25, ColdResistance = 0, LightningResistance = 0, DamageBonus = 50 }, armor: 24250 ),
                    new DungeonEnemy(position: 10, @class: ClassType.Scout, level: 390, strength: 13900, dexterity: 27800, intelligence: 13900, constitution: 100500, luck: 13900, health: 286000000, minWeaponDmg: 1276, maxWeaponDmg: 1510, dungeonRuneBonuses: new DungeonEnemyRuneBonuses { DamageRuneType = RuneType.ColdDamage, FireResistance = 0, ColdResistance = 25, LightningResistance = 0, DamageBonus = 50 }, armor: 24500 )
                }
            },
            new Dungeon
            {
                Name = DungeonNames.ThirteenthFloor,
                Position = 115,
                IsDefeated = false,
                IsUnlocked = true,
                UnlockResolve = c => c.Dungeons.First(d => d.Position == 112).DungeonEnemies.First(e => e.Position == 5).IsDefeated,
                Type = DungeonTypeEnum.Shadow,
                DungeonEnemies = new List<DungeonEnemy>
                {
                    new DungeonEnemy(position: 1, @class: ClassType.Warrior, level: 424, strength: 9041, dexterity: 3171, intelligence: 3141, constitution: 47883, luck: 2049, health: 101751376, minWeaponDmg: 4535, maxWeaponDmg: 9045 ),
                    new DungeonEnemy(position: 2, @class: ClassType.Mage, level: 428, strength: 2342, dexterity: 2365, intelligence: 9511, constitution: 49434, luck: 3374, health: 42414372, minWeaponDmg: 10557, maxWeaponDmg: 20581 ),
                    new DungeonEnemy(position: 3, @class: ClassType.Warrior, level: 432, strength: 9811, dexterity: 3441, intelligence: 3411, constitution: 52140, luck: 2201, health: 112883104, minWeaponDmg: 4628, maxWeaponDmg: 9222 ),
                    new DungeonEnemy(position: 4, @class: ClassType.Mage, level: 436, strength: 2545, dexterity: 2568, intelligence: 10322, constitution: 53922, luck: 3657, health: 47127828, minWeaponDmg: 10533, maxWeaponDmg: 19240 ),
                    new DungeonEnemy(position: 5, @class: ClassType.Warrior, level: 440, strength: 10958, dexterity: 3842, intelligence: 3813, constitution: 58465, luck: 2428, health: 128915328, minWeaponDmg: 4709, maxWeaponDmg: 9401 ),
                    new DungeonEnemy(position: 6, @class: ClassType.Scout, level: 444, strength: 3133, dexterity: 12490, intelligence: 3122, constitution: 66896, luck: 4454, health: 119074880, minWeaponDmg: 5942, maxWeaponDmg: 11862 ),
                    new DungeonEnemy(position: 7, @class: ClassType.Mage, level: 448, strength: 3360, dexterity: 3383, intelligence: 13579, constitution: 71868, luck: 4796, health: 64537464, minWeaponDmg: 11207, maxWeaponDmg: 20640 ),
                    new DungeonEnemy(position: 8, @class: ClassType.Warrior, level: 452, strength: 14533, dexterity: 5093, intelligence: 5064, constitution: 78150, luck: 3140, health: 177009744, minWeaponDmg: 4879, maxWeaponDmg: 9249 ),
                    new DungeonEnemy(position: 9, @class: ClassType.Warrior, level: 456, strength: 15630, dexterity: 5477, intelligence: 5448, constitution: 84188, luck: 3359, health: 192369584, minWeaponDmg: 4880, maxWeaponDmg: 9743 ),
                    new DungeonEnemy(position: 10, @class: ClassType.Warrior, level: 460, strength: 19366, dexterity: 6785, intelligence: 6756, constitution: 104742, luck: 4105, health: 241430304, minWeaponDmg: 4919, maxWeaponDmg: 9823 )
                }
            },
            new Dungeon
            {
                Name = DungeonNames.ThirdLeagueOfSuperheroes,
                Position = 116,
                IsDefeated = false,
                IsUnlocked = true,
                UnlockResolve = c => c.CharacterLevel >= 370,
                Type = DungeonTypeEnum.Shadow,
                DungeonEnemies = new List<DungeonEnemy>
                {
                    new DungeonEnemy(position: 1, @class: ClassType.Mage, level: 370, strength: 12700, dexterity: 12700, intelligence: 25400, constitution: 90500, luck: 12700, health: 256000000, minWeaponDmg: 1181, maxWeaponDmg: 1427, dungeonRuneBonuses: new DungeonEnemyRuneBonuses { DamageRuneType = RuneType.LightningDamage, FireResistance = 0, ColdResistance = 0, LightningResistance = 25, DamageBonus = 50 }, armor: 23250 ),
                    new DungeonEnemy(position: 2, @class: ClassType.Warrior, level: 376, strength: 26120, dexterity: 13060, intelligence: 13060, constitution: 93500, luck: 13060, health: 265000000, minWeaponDmg: 1212, maxWeaponDmg: 1451, dungeonRuneBonuses: new DungeonEnemyRuneBonuses { DamageRuneType = RuneType.LightningDamage, FireResistance = 0, ColdResistance = 0, LightningResistance = 25, DamageBonus = 50 }, armor: 24000 ),
                    new DungeonEnemy(position: 3, @class: ClassType.Scout, level: 382, strength: 13420, dexterity: 26840, intelligence: 13420, constitution: 96500, luck: 13420, health: 274000000, minWeaponDmg: 1230, maxWeaponDmg: 1471, dungeonRuneBonuses: new DungeonEnemyRuneBonuses { DamageRuneType = RuneType.ColdDamage, FireResistance = 0, ColdResistance = 25, LightningResistance = 0, DamageBonus = 50 }, armor: 24750 ),
                    new DungeonEnemy(position: 4, @class: ClassType.Warrior, level: 388, strength: 27560, dexterity: 13780, intelligence: 13780, constitution: 99500, luck: 13780, health: 283000000, minWeaponDmg: 1254, maxWeaponDmg: 1500, dungeonRuneBonuses: new DungeonEnemyRuneBonuses { DamageRuneType = RuneType.FireDamage, FireResistance = 25, ColdResistance = 0, LightningResistance = 0, DamageBonus = 50 }, armor: 25500 ),
                    new DungeonEnemy(position: 5, @class: ClassType.Scout, level: 394, strength: 14140, dexterity: 28280, intelligence: 14140, constitution: 102500, luck: 14140, health: 292000000, minWeaponDmg: 1282, maxWeaponDmg: 1526, dungeonRuneBonuses: new DungeonEnemyRuneBonuses { DamageRuneType = RuneType.ColdDamage, FireResistance = 0, ColdResistance = 25, LightningResistance = 0, DamageBonus = 50 }, armor: 26250 ),
                    new DungeonEnemy(position: 6, @class: ClassType.Warrior, level: 400, strength: 30000, dexterity: 15000, intelligence: 15000, constitution: 110000, luck: 15000, health: 300000000, minWeaponDmg: 1505, maxWeaponDmg: 1747, dungeonRuneBonuses: new DungeonEnemyRuneBonuses { DamageRuneType = RuneType.LightningDamage, FireResistance = 0, ColdResistance = 0, LightningResistance = 25, DamageBonus = 50 }, armor: 27000 ),
                    new DungeonEnemy(position: 7, @class: ClassType.Scout, level: 406, strength: 16500, dexterity: 32700, intelligence: 16500, constitution: 114800, luck: 16500, health: 318000000, minWeaponDmg: 1532, maxWeaponDmg: 1779, dungeonRuneBonuses: new DungeonEnemyRuneBonuses { DamageRuneType = RuneType.LightningDamage, FireResistance = 0, ColdResistance = 0, LightningResistance = 25, DamageBonus = 50 }, armor: 27750 ),
                    new DungeonEnemy(position: 8, @class: ClassType.Mage, level: 412, strength: 18000, dexterity: 18000, intelligence: 35400, constitution: 119600, luck: 18000, health: 336000000, minWeaponDmg: 1563, maxWeaponDmg: 1809, dungeonRuneBonuses: new DungeonEnemyRuneBonuses { DamageRuneType = RuneType.FireDamage, FireResistance = 25, ColdResistance = 0, LightningResistance = 0, DamageBonus = 50 }, armor: 28500 ),
                    new DungeonEnemy(position: 9, @class: ClassType.Mage, level: 418, strength: 19500, dexterity: 19500, intelligence: 38100, constitution: 124400, luck: 19500, health: 354000000, minWeaponDmg: 1590, maxWeaponDmg: 1834, dungeonRuneBonuses: new DungeonEnemyRuneBonuses { DamageRuneType = RuneType.FireDamage, FireResistance = 25, ColdResistance = 0, LightningResistance = 0, DamageBonus = 50 }, armor: 29250 ),
                    new DungeonEnemy(position: 10, @class: ClassType.Scout, level: 430, strength: 22500, dexterity: 43500, intelligence: 22500, constitution: 134000, luck: 22500, health: 390000000, minWeaponDmg: 1660, maxWeaponDmg: 1899, dungeonRuneBonuses: new DungeonEnemyRuneBonuses { DamageRuneType = RuneType.ColdDamage, FireResistance = 0, ColdResistance = 25, LightningResistance = 0, DamageBonus = 50 }, armor: 30000 )
                }
            },
            new Dungeon
            {
                Name = DungeonNames.TimeHonoredSchoolOfMagic,
                Position = 117,
                IsDefeated = false,
                IsUnlocked = true,
                UnlockResolve = c => c.CharacterLevel >= 350,
                Type = DungeonTypeEnum.Shadow,
                DungeonEnemies = new List<DungeonEnemy>
                {
                    new DungeonEnemy(position: 1, @class: ClassType.Scout, level: 410, strength: 8400, dexterity: 66000, intelligence: 8400, constitution: 157500, luck: 27000, health: 468750016, minWeaponDmg: 1500, maxWeaponDmg: 1624 ),
                    new DungeonEnemy(position: 2, @class: ClassType.Warrior, level: 420, strength: 58332, dexterity: 14424, intelligence: 14556, constitution: 196785, luck: 28584, health: 597611456, minWeaponDmg: 700, maxWeaponDmg: 1969 ),
                    new DungeonEnemy(position: 3, @class: ClassType.Scout, level: 430, strength: 9300, dexterity: 67740, intelligence: 9360, constitution: 198000, luck: 28800, health: 513187488, minWeaponDmg: 1705, maxWeaponDmg: 1819 ),
                    new DungeonEnemy(position: 4, @class: ClassType.Scout, level: 440, strength: 11790, dexterity: 73920, intelligence: 11880, constitution: 207000, luck: 35640, health: 554774976, minWeaponDmg: 1165, maxWeaponDmg: 2474 ),
                    new DungeonEnemy(position: 5, @class: ClassType.Mage, level: 450, strength: 23214, dexterity: 23190, intelligence: 73494, constitution: 221647, luck: 45972, health: 299962912, minWeaponDmg: 1911, maxWeaponDmg: 4818 ),
                    new DungeonEnemy(position: 6, @class: ClassType.Scout, level: 460, strength: 26790, dexterity: 74640, intelligence: 26580, constitution: 243450, luck: 47760, health: 706649984, minWeaponDmg: 1757, maxWeaponDmg: 2041 ),
                    new DungeonEnemy(position: 7, @class: ClassType.Scout, level: 470, strength: 23460, dexterity: 77880, intelligence: 23100, constitution: 245250, luck: 47820, health: 722437504, minWeaponDmg: 1005, maxWeaponDmg: 2919 ),
                    new DungeonEnemy(position: 8, @class: ClassType.Warrior, level: 480, strength: 89244, dexterity: 18792, intelligence: 11916, constitution: 262327, luck: 48132, health: 1039800576, minWeaponDmg: 1410, maxWeaponDmg: 1689 ),
                    new DungeonEnemy(position: 9, @class: ClassType.Warrior, level: 490, strength: 86820, dexterity: 33240, intelligence: 33414, constitution: 315225, luck: 61182, health: 1307133056, minWeaponDmg: 1115, maxWeaponDmg: 2364 ),
                    new DungeonEnemy(position: 10, @class: ClassType.Mage, level: 500, strength: 35466, dexterity: 26028, intelligence: 73044, constitution: 315225, luck: 51240, health: 534621568, minWeaponDmg: 2567, maxWeaponDmg: 5151 )
                }
            },
            new Dungeon
            {
                Name = DungeonNames.Hemorridor,
                Position = 118,
                IsDefeated = false,
                IsUnlocked = true,
                UnlockResolve = c => c.Dungeons.First(d => d.Position == 112).DungeonEnemies.First(e => e.Position == 5).IsDefeated,
                Type = DungeonTypeEnum.Shadow,
                DungeonEnemies = new List<DungeonEnemy>
                {
                    new DungeonEnemy(position: 1, @class: ClassType.Warrior, level: 328, strength: 52800, dexterity: 6720, intelligence: 6720, constitution: 126000, luck: 21600, health: 207270000, minWeaponDmg: 1118, maxWeaponDmg: 1891 ),
                    new DungeonEnemy(position: 2, @class: ClassType.Warrior, level: 349, strength: 48416, dexterity: 11972, intelligence: 12081, constitution: 163332, luck: 23725, health: 285831008, minWeaponDmg: 1190, maxWeaponDmg: 3248 ),
                    new DungeonEnemy(position: 3, @class: ClassType.Mage, level: 370, strength: 7998, dexterity: 7998, intelligence: 58256, constitution: 170280, luck: 24768, health: 126347760, minWeaponDmg: 2845, maxWeaponDmg: 4764 ),
                    new DungeonEnemy(position: 4, @class: ClassType.Scout, level: 392, strength: 10493, dexterity: 65789, intelligence: 10573, constitution: 184230, luck: 31720, health: 289609568, minWeaponDmg: 1670, maxWeaponDmg: 2816 ),
                    new DungeonEnemy(position: 5, @class: ClassType.Scout, level: 414, strength: 21357, dexterity: 67614, intelligence: 21357, constitution: 203916, luck: 42294, health: 338500544, minWeaponDmg: 1794, maxWeaponDmg: 2970 ),
                    new DungeonEnemy(position: 6, @class: ClassType.Warrior, level: 437, strength: 70908, dexterity: 25251, intelligence: 25251, constitution: 231278, luck: 45372, health: 506498816, minWeaponDmg: 1498, maxWeaponDmg: 2511 ),
                    new DungeonEnemy(position: 7, @class: ClassType.Scout, level: 461, strength: 22991, dexterity: 76322, intelligence: 22638, constitution: 240345, luck: 46864, health: 444157568, minWeaponDmg: 2064, maxWeaponDmg: 3262 ),
                    new DungeonEnemy(position: 8, @class: ClassType.Mage, level: 485, strength: 18980, dexterity: 18980, intelligence: 90136, constitution: 264951, luck: 48613, health: 257532368, minWeaponDmg: 3755, maxWeaponDmg: 6243 ),
                    new DungeonEnemy(position: 9, @class: ClassType.BattleMage, level: 505, strength: 89425, dexterity: 34237, intelligence: 34416, constitution: 324682, luck: 63017, health: 821445440, minWeaponDmg: 1725, maxWeaponDmg: 2908, armor: 14000 ),
                    new DungeonEnemy(position: 10, @class: ClassType.Scout, level: 525, strength: 37239, dexterity: 76696, intelligence: 37239, constitution: 330986, luck: 53802, health: 696394560, minWeaponDmg: 2236, maxWeaponDmg: 3778 )
                }
            },
            new Dungeon
            {
                Name = DungeonNames.Easteros,
                Position = 119,
                IsDefeated = false,
                IsUnlocked = true,
                UnlockResolve = c => c.Dungeons.First(d => d.Position == 115).DungeonEnemies.First(e => e.Position == 5).IsDefeated,
                Type = DungeonTypeEnum.Shadow,
                DungeonEnemies = new List<DungeonEnemy>
                {
                    new DungeonEnemy(position: 1, @class: ClassType.Warrior, level: 464, strength: 9040, dexterity: 3173, intelligence: 3135, constitution: 47416, luck: 2110, health: 110242200, minWeaponDmg: 4966, maxWeaponDmg: 9792, armor: 18000 ),
                    new DungeonEnemy(position: 2, @class: ClassType.Warrior, level: 468, strength: 9126, dexterity: 3202, intelligence: 3166, constitution: 47938, luck: 2120, health: 112414608, minWeaponDmg: 5030, maxWeaponDmg: 9944, armor: 50000 ),
                    new DungeonEnemy(position: 3, @class: ClassType.Mage, level: 472, strength: 4576, dexterity: 4634, intelligence: 18651, constitution: 95656, luck: 6636, health: 90490576, minWeaponDmg: 11355, maxWeaponDmg: 22703, armor: 40000 ),
                    new DungeonEnemy(position: 4, @class: ClassType.Warrior, level: 476, strength: 18592, dexterity: 6524, intelligence: 6454, constitution: 97944, luck: 4284, health: 233596448, minWeaponDmg: 5090, maxWeaponDmg: 10175, armor: 48000 ),
                    new DungeonEnemy(position: 5, @class: ClassType.Warrior, level: 480, strength: 18761, dexterity: 6582, intelligence: 6514, constitution: 98962, luck: 4306, health: 238003616, minWeaponDmg: 5133, maxWeaponDmg: 10259 ),
                    new DungeonEnemy(position: 6, @class: ClassType.Mage, level: 484, strength: 7945, dexterity: 8026, intelligence: 32266, constitution: 167706, luck: 11446, health: 162674816, minWeaponDmg: 11672, maxWeaponDmg: 23265 ),
                    new DungeonEnemy(position: 7, @class: ClassType.Scout, level: 488, strength: 8625, dexterity: 34344, intelligence: 8586, constitution: 182798, luck: 12301, health: 357552896, minWeaponDmg: 6520, maxWeaponDmg: 13024 ),
                    new DungeonEnemy(position: 8, @class: ClassType.Warrior, level: 492, strength: 41522, dexterity: 14558, intelligence: 14457, constitution: 222387, luck: 9089, health: 548183936, minWeaponDmg: 5260, maxWeaponDmg: 10509 ),
                    new DungeonEnemy(position: 9, @class: ClassType.Warrior, level: 496, strength: 47500, dexterity: 16500, intelligence: 16000, constitution: 270000, luck: 11000, health: 670950016, minWeaponDmg: 6337, maxWeaponDmg: 10105 ),
                    new DungeonEnemy(position: 10, @class: ClassType.Mage, level: 500, strength: 9500, dexterity: 11000, intelligence: 40200, constitution: 190000, luck: 29800, health: 454106400, minWeaponDmg: 12038, maxWeaponDmg: 24020 )
                }
            },
            new Dungeon
            {
                Name = DungeonNames.DojoOfChildhoodHeros,
                Position = 120,
                IsDefeated = false,
                IsUnlocked = true,
                UnlockResolve = c => c.CharacterLevel >= 500,
                Type = DungeonTypeEnum.Shadow,
                DungeonEnemies = new List<DungeonEnemy>
                {
                    new DungeonEnemy(position: 1, @class: ClassType.Warrior, level: 500, strength: 80000, dexterity: 40000, intelligence: 40000, constitution: 190000, luck: 40000, health: 600000000, minWeaponDmg: 2007, maxWeaponDmg: 2244, dungeonRuneBonuses: new DungeonEnemyRuneBonuses { DamageRuneType = RuneType.LightningDamage, FireResistance = 0, ColdResistance = 0, LightningResistance = 25, DamageBonus = 50 }, armor: 35000 ),
                    new DungeonEnemy(position: 2, @class: ClassType.Warrior, level: 507, strength: 83500, dexterity: 41750, intelligence: 41750, constitution: 195600, luck: 41750, health: 621000000, minWeaponDmg: 2038, maxWeaponDmg: 2283, dungeonRuneBonuses: new DungeonEnemyRuneBonuses { DamageRuneType = RuneType.FireDamage, FireResistance = 25, ColdResistance = 0, LightningResistance = 0, DamageBonus = 50 }, armor: 35000 ),
                    new DungeonEnemy(position: 3, @class: ClassType.Mage, level: 514, strength: 43500, dexterity: 43500, intelligence: 87000, constitution: 201200, luck: 43500, health: 642000000, minWeaponDmg: 2071, maxWeaponDmg: 2310, dungeonRuneBonuses: new DungeonEnemyRuneBonuses { DamageRuneType = RuneType.ColdDamage, FireResistance = 0, ColdResistance = 25, LightningResistance = 0, DamageBonus = 50 }, armor: 35000 ),
                    new DungeonEnemy(position: 4, @class: ClassType.Scout, level: 521, strength: 45250, dexterity: 90500, intelligence: 45250, constitution: 206800, luck: 45250, health: 663000000, minWeaponDmg: 2115, maxWeaponDmg: 2341, dungeonRuneBonuses: new DungeonEnemyRuneBonuses { DamageRuneType = RuneType.ColdDamage, FireResistance = 0, ColdResistance = 25, LightningResistance = 0, DamageBonus = 50 }, armor: 35000 ),
                    new DungeonEnemy(position: 5, @class: ClassType.Mage, level: 528, strength: 47000, dexterity: 47000, intelligence: 94000, constitution: 212400, luck: 47000, health: 684000000, minWeaponDmg: 2144, maxWeaponDmg: 2377, dungeonRuneBonuses: new DungeonEnemyRuneBonuses { DamageRuneType = RuneType.LightningDamage, FireResistance = 0, ColdResistance = 0, LightningResistance = 25, DamageBonus = 50 }, armor: 35000 ),
                    new DungeonEnemy(position: 6, @class: ClassType.Scout, level: 535, strength: 48750, dexterity: 97500, intelligence: 48750, constitution: 218000, luck: 48750, health: 705000000, minWeaponDmg: 2179, maxWeaponDmg: 2419, dungeonRuneBonuses: new DungeonEnemyRuneBonuses { DamageRuneType = RuneType.LightningDamage, FireResistance = 0, ColdResistance = 0, LightningResistance = 25, DamageBonus = 50 }, armor: 35000 ),
                    new DungeonEnemy(position: 7, @class: ClassType.Mage, level: 542, strength: 50500, dexterity: 50500, intelligence: 101000, constitution: 223600, luck: 50500, health: 726000000, minWeaponDmg: 2222, maxWeaponDmg: 2459, dungeonRuneBonuses: new DungeonEnemyRuneBonuses { DamageRuneType = RuneType.FireDamage, FireResistance = 25, ColdResistance = 0, LightningResistance = 0, DamageBonus = 50 }, armor: 35000 ),
                    new DungeonEnemy(position: 8, @class: ClassType.Warrior, level: 549, strength: 104500, dexterity: 52250, intelligence: 52250, constitution: 229200, luck: 52250, health: 747000000, minWeaponDmg: 2247, maxWeaponDmg: 2485, dungeonRuneBonuses: new DungeonEnemyRuneBonuses { DamageRuneType = RuneType.FireDamage, FireResistance = 25, ColdResistance = 0, LightningResistance = 0, DamageBonus = 50 }, armor: 35000 ),
                    new DungeonEnemy(position: 9, @class: ClassType.Warrior, level: 556, strength: 108000, dexterity: 54000, intelligence: 54000, constitution: 234800, luck: 54000, health: 768000000, minWeaponDmg: 2302, maxWeaponDmg: 2530, dungeonRuneBonuses: new DungeonEnemyRuneBonuses { DamageRuneType = RuneType.ColdDamage, FireResistance = 0, ColdResistance = 25, LightningResistance = 0, DamageBonus = 50 }, armor: 35000 ),
                    new DungeonEnemy(position: 10, @class: ClassType.Mage, level: 570, strength: 57500, dexterity: 57500, intelligence: 115000, constitution: 246000, luck: 57500, health: 810000000, minWeaponDmg: 2412, maxWeaponDmg: 2591, dungeonRuneBonuses: new DungeonEnemyRuneBonuses { DamageRuneType = RuneType.LightningDamage, FireResistance = 0, ColdResistance = 0, LightningResistance = 25, DamageBonus = 50 }, armor: 35000 )
                }
            },
            new Dungeon
            {
                Name = DungeonNames.TavernOfDarkDoppelgangers,
                Position = 121,
                IsDefeated = false,
                IsUnlocked = true,
                UnlockResolve = c => c.CharacterLevel >= 350,
                Type = DungeonTypeEnum.Shadow,
                DungeonEnemies = new List<DungeonEnemy>
                {
                    new DungeonEnemy(position: 1, @class: ClassType.Mage, level: 400, strength: 35000, dexterity: 15000, intelligence: 35000, constitution: 180000, luck: 20000, health: 6074999808, minWeaponDmg: 1351, maxWeaponDmg: 2249, armor: 7000 ),
                    new DungeonEnemy(position: 2, @class: ClassType.Warrior, level: 400, strength: 45000, dexterity: 15000, intelligence: 45000, constitution: 200000, luck: 23500, health: 3037499904, minWeaponDmg: 600, maxWeaponDmg: 999, armor: 12500 ),
                    new DungeonEnemy(position: 3, @class: ClassType.Warrior, level: 400, strength: 60000, dexterity: 20000, intelligence: 20000, constitution: 220000, luck: 31500, health: 8099999744, minWeaponDmg: 600, maxWeaponDmg: 999, armor: 20000 ),
                    new DungeonEnemy(position: 4, @class: ClassType.Scout, level: 400, strength: 25000, dexterity: 75000, intelligence: 25000, constitution: 240000, luck: 45000, health: 8099999744, minWeaponDmg: 750, maxWeaponDmg: 1249, armor: 15000 ),
                    new DungeonEnemy(position: 5, @class: ClassType.Warrior, level: 400, strength: 90000, dexterity: 30000, intelligence: 30000, constitution: 260000, luck: 48500, health: 5062499840, minWeaponDmg: 600, maxWeaponDmg: 999, armor: 25000 ),
                    new DungeonEnemy(position: 6, @class: ClassType.Mage, level: 400, strength: 35000, dexterity: 35000, intelligence: 105000, constitution: 280000, luck: 53500, health: 6074999808, minWeaponDmg: 1350, maxWeaponDmg: 2249, armor: 7000 ),
                    new DungeonEnemy(position: 7, @class: ClassType.Scout, level: 450, strength: 40000, dexterity: 120000, intelligence: 40000, constitution: 300000, luck: 71500, health: 16199999488, minWeaponDmg: 850, maxWeaponDmg: 1399, armor: 15000 ),
                    new DungeonEnemy(position: 8, @class: ClassType.Mage, level: 500, strength: 50000, dexterity: 50000, intelligence: 150000, constitution: 320000, luck: 93500, health: 16199999488, minWeaponDmg: 1700, maxWeaponDmg: 2799, armor: 7000 ),
                    new DungeonEnemy(position: 9, @class: ClassType.Warrior, level: 550, strength: 200000, dexterity: 75000, intelligence: 75000, constitution: 330000, luck: 100000, health: 18225000448, minWeaponDmg: 850, maxWeaponDmg: 1399, armor: 35000 ),
                    new DungeonEnemy(position: 10, @class: ClassType.Mage, level: 600, strength: 100000, dexterity: 100000, intelligence: 250000, constitution: 430000, luck: 137000, health: 12149999616, minWeaponDmg: 2050, maxWeaponDmg: 3398, armor: 25000 )
                }
            },
            new Dungeon
            {
                Name = DungeonNames.CityOfIntrigues,
                Position = 122,
                IsDefeated = false,
                IsUnlocked = true,
                UnlockResolve = c => c.Dungeons.First(d => d.Position == 119).IsDefeated,
                Type = DungeonTypeEnum.Shadow,
                DungeonEnemies = new List<DungeonEnemy>
                {
                    new DungeonEnemy(position: 1, @class: ClassType.Scout, level: 504, strength: 10719, dexterity: 42680, intelligence: 10670, constitution: 227166, luck: 15287, health: 458875328, minWeaponDmg: 7833, maxWeaponDmg: 12252 ),
                    new DungeonEnemy(position: 2, @class: ClassType.Mage, level: 508, strength: 13425, dexterity: 13546, intelligence: 54428, constitution: 284718, luck: 19279, health: 289842912, minWeaponDmg: 12223, maxWeaponDmg: 24428 ),
                    new DungeonEnemy(position: 3, @class: ClassType.Warrior, level: 512, strength: 54533, dexterity: 19122, intelligence: 18973, constitution: 290763, luck: 12109, health: 745807104, minWeaponDmg: 5652, maxWeaponDmg: 10901 ),
                    new DungeonEnemy(position: 4, @class: ClassType.Scout, level: 516, strength: 13838, dexterity: 55118, intelligence: 13779, constitution: 294118, luck: 19707, health: 608236032, minWeaponDmg: 6901, maxWeaponDmg: 13768 ),
                    new DungeonEnemy(position: 5, @class: ClassType.Warrior, level: 520, strength: 66837, dexterity: 23434, intelligence: 23261, constitution: 356928, luck: 14768, health: 929797440, minWeaponDmg: 5563, maxWeaponDmg: 11110, armor: 50000 ),
                    new DungeonEnemy(position: 6, @class: ClassType.Warrior, level: 524, strength: 67527, dexterity: 23675, intelligence: 23504, constitution: 360872, luck: 14886, health: 947289024, minWeaponDmg: 5860, maxWeaponDmg: 10685 ),
                    new DungeonEnemy(position: 7, @class: ClassType.Mage, level: 528, strength: 16985, dexterity: 17120, intelligence: 68752, constitution: 361823, luck: 24319, health: 382808736, minWeaponDmg: 14601, maxWeaponDmg: 23926 ),
                    new DungeonEnemy(position: 8, @class: ClassType.Mage, level: 532, strength: 20016, dexterity: 20171, intelligence: 80997, constitution: 426706, luck: 28643, health: 454868608, minWeaponDmg: 14576, maxWeaponDmg: 24519 ),
                    new DungeonEnemy(position: 9, @class: ClassType.Warrior, level: 536, strength: 81165, dexterity: 28453, intelligence: 28262, constitution: 434616, luck: 17779, health: 1166944000, minWeaponDmg: 6172, maxWeaponDmg: 11183 ),
                    new DungeonEnemy(position: 10, @class: ClassType.Scout, level: 540, strength: 20563, dexterity: 81950, intelligence: 20487, constitution: 439082, luck: 29219, health: 950173440, minWeaponDmg: 7214, maxWeaponDmg: 13561 )
                }
            },
            new Dungeon
            {
                Name = DungeonNames.SchoolOfMagicExpress,
                Position = 123,
                IsDefeated = false,
                IsUnlocked = true,
                UnlockResolve = c => c.Dungeons.First(d => d.Position == 117).IsDefeated,
                Type = DungeonTypeEnum.Shadow,
                DungeonEnemies = new List<DungeonEnemy>
                {
                    new DungeonEnemy(position: 1, @class: ClassType.Mage, level: 510, strength: 19326, dexterity: 19326, intelligence: 99462, constitution: 278055, luck: 53784, health: 483444928, minWeaponDmg: 2152, maxWeaponDmg: 6080 ),
                    new DungeonEnemy(position: 2, @class: ClassType.Mage, level: 520, strength: 36768, dexterity: 36342, intelligence: 102168, constitution: 345127, luck: 65430, health: 622523712, minWeaponDmg: 3234, maxWeaponDmg: 5251 ),
                    new DungeonEnemy(position: 3, @class: ClassType.Mage, level: 530, strength: 30906, dexterity: 31422, intelligence: 111318, constitution: 359460, luck: 72780, health: 676144192, minWeaponDmg: 2481, maxWeaponDmg: 6267 ),
                    new DungeonEnemy(position: 4, @class: ClassType.Scout, level: 540, strength: 35250, dexterity: 121020, intelligence: 35220, constitution: 325350, luck: 55920, health: 1264800000, minWeaponDmg: 1350, maxWeaponDmg: 3599 ),
                    new DungeonEnemy(position: 5, @class: ClassType.Mage, level: 550, strength: 33900, dexterity: 34464, intelligence: 119256, constitution: 389340, luck: 75114, health: 785428544, minWeaponDmg: 3892, maxWeaponDmg: 5330 ),
                    new DungeonEnemy(position: 6, @class: ClassType.Warrior, level: 560, strength: 224978, dexterity: 55368, intelligence: 54462, constitution: 433845, luck: 80154, health: 1196667008, minWeaponDmg: 1226, maxWeaponDmg: 2952 ),
                    new DungeonEnemy(position: 7, @class: ClassType.Mage, level: 570, strength: 46638, dexterity: 46632, intelligence: 116148, constitution: 410917, luck: 79146, health: 876589696, minWeaponDmg: 3627, maxWeaponDmg: 5890 ),
                    new DungeonEnemy(position: 8, @class: ClassType.Mage, level: 580, strength: 34644, dexterity: 34650, intelligence: 147120, constitution: 433935, luck: 82506, health: 943157696, minWeaponDmg: 3980, maxWeaponDmg: 5964 ),
                    new DungeonEnemy(position: 9, @class: ClassType.Scout, level: 590, strength: 45021, dexterity: 165800, intelligence: 75600, constitution: 44342, luck: 101840, health: 1545725056, minWeaponDmg: 2120, maxWeaponDmg: 5229 ),
                    new DungeonEnemy(position: 10, @class: ClassType.Mage, level: 600, strength: 50682, dexterity: 50268, intelligence: 183510, constitution: 569115, luck: 110250, health: 1427435264, minWeaponDmg: 4776, maxWeaponDmg: 7369 )
                }
            },
            new Dungeon
            {
                Name = DungeonNames.NordicGods,
                Position = 124,
                IsDefeated = false,
                IsUnlocked = true,
                UnlockResolve = c => c.CharacterLevel >= 280,
                Type = DungeonTypeEnum.Shadow,
                DungeonEnemies = new List<DungeonEnemy>
                {
                    new DungeonEnemy(position: 1, @class: ClassType.Warrior, level: 345, strength: 48500, dexterity: 12000, intelligence: 12000, constitution: 163000, luck: 23500, health: 318257504, dungeonRuneBonuses: new DungeonEnemyRuneBonuses { DamageRuneType = RuneType.ColdDamage, FireResistance = 5, ColdResistance = 60, LightningResistance = 5, DamageBonus = 60 }, minWeaponDmg: 1190, maxWeaponDmg: 2011, armor: 12250),
                    new DungeonEnemy(position: 2, @class: ClassType.Warrior, level: 390, strength: 65500, dexterity: 10500, intelligence: 10500, constitution: 184000, luck: 31500, health: 404800000, dungeonRuneBonuses: new DungeonEnemyRuneBonuses { DamageRuneType = RuneType.LightningDamage, FireResistance = 10, ColdResistance = 10, LightningResistance = 60, DamageBonus = 60 }, minWeaponDmg: 1365, maxWeaponDmg: 2339, armor: 19500),
                    new DungeonEnemy(position: 3, @class: ClassType.Mage, level: 435, strength: 25000, dexterity: 25000, intelligence: 70500, constitution: 231000, luck: 45000, health: 226149008, dungeonRuneBonuses: new DungeonEnemyRuneBonuses { DamageRuneType = RuneType.FireDamage, FireResistance = 35, ColdResistance = 35, LightningResistance = 35, DamageBonus = 60 }, minWeaponDmg: 3425, maxWeaponDmg: 5871, armor: 21750),
                    new DungeonEnemy(position: 4, @class: ClassType.Warrior, level: 485, strength: 90000, dexterity: 19000, intelligence: 18500, constitution: 265000, luck: 48500, health: 721462528, dungeonRuneBonuses: new DungeonEnemyRuneBonuses { DamageRuneType = RuneType.LightningDamage, FireResistance = 15, ColdResistance = 15, LightningResistance = 60, DamageBonus = 60 }, minWeaponDmg: 1697, maxWeaponDmg: 2909, armor: 24250),
                    new DungeonEnemy(position: 5, @class: ClassType.Warrior, level: 525, strength: 76500, dexterity: 37000, intelligence: 37000, constitution: 330500, luck: 53500, health: 972496256, dungeonRuneBonuses: new DungeonEnemyRuneBonuses { DamageRuneType = RuneType.FireDamage, FireResistance = 60, ColdResistance = 30, LightningResistance = 30, DamageBonus = 60 }, minWeaponDmg: 1837, maxWeaponDmg: 3149, armor: 26250),
                    new DungeonEnemy(position: 6, @class: ClassType.Assassin, level: 565, strength: 39500, dexterity: 111000, intelligence: 39500, constitution: 376000, luck: 71500, health: 951280000, dungeonRuneBonuses: new DungeonEnemyRuneBonuses { DamageRuneType = RuneType.ColdDamage, FireResistance = 40, ColdResistance = 40, LightningResistance = 40, DamageBonus = 60 }, minWeaponDmg: 3953, maxWeaponDmg: 6780, armor: 14125),
                    new DungeonEnemy(position: 7, @class: ClassType.Berserker, level: 600, strength: 136500, dexterity: 39500, intelligence: 39500, constitution: 417500, luck: 63000, health: 1120570112, dungeonRuneBonuses: new DungeonEnemyRuneBonuses { DamageRuneType = RuneType.ColdDamage, FireResistance = 10, ColdResistance = 50, LightningResistance = 10, DamageBonus = 60 }, minWeaponDmg: 2105, maxWeaponDmg: 3600, armor: 30000),
                    new DungeonEnemy(position: 8, @class: ClassType.Scout, level: 625, strength: 63500, dexterity: 163000, intelligence: 63500, constitution: 507500, luck: 93500, health: 1417955072, dungeonRuneBonuses: new DungeonEnemyRuneBonuses { DamageRuneType = RuneType.LightningDamage, FireResistance = 50, ColdResistance = 50, LightningResistance = 50, DamageBonus = 60 }, minWeaponDmg: 2734, maxWeaponDmg: 4686, armor: 15625),
                    new DungeonEnemy(position: 9, @class: ClassType.Berserker, level: 645, strength: 182000, dexterity: 42000, intelligence: 42000, constitution: 575000, luck: 100000, health: 1657149952, dungeonRuneBonuses: new DungeonEnemyRuneBonuses { DamageRuneType = RuneType.LightningDamage, FireResistance = 50, ColdResistance = 50, LightningResistance = 50, DamageBonus = 60 }, minWeaponDmg: 2260, maxWeaponDmg: 3877, armor: 32250),
                    new DungeonEnemy(position: 10, @class: ClassType.Berserker, level: 660, strength: 229500, dexterity: 62500, intelligence: 62500, constitution: 711000, luck: 137000, health: 2096028032, dungeonRuneBonuses: new DungeonEnemyRuneBonuses { DamageRuneType = RuneType.FireDamage, FireResistance = 60, ColdResistance = 60, LightningResistance = 60, DamageBonus = 60 }, minWeaponDmg: 2312, maxWeaponDmg: 3899, armor: 33000)
                }
            },
            new Dungeon
            {
                Name = DungeonNames.AshMountain,
                Position = 125,
                IsDefeated = false,
                IsUnlocked = true,
                UnlockResolve = c => c.Dungeons.First(d => d.Position == 118).IsDefeated,
                Type = DungeonTypeEnum.Shadow,
                DungeonEnemies = new List<DungeonEnemy>
                {
                    new DungeonEnemy(position: 1, @class: ClassType.BattleMage, level: 546, strength: 106424, dexterity: 20679, intelligence: 20679, constitution: 347519, luck: 57549, health: 950464448, minWeaponDmg: 1870, maxWeaponDmg: 3141, armor: 14000 ),
                    new DungeonEnemy(position: 2, @class: ClassType.Warrior, level: 567, strength: 111363, dexterity: 39613, intelligence: 39613, constitution: 376189, luck: 71319, health: 1068376768, minWeaponDmg: 1935, maxWeaponDmg: 3264 ),
                    new DungeonEnemy(position: 3, @class: ClassType.Assassin, level: 588, strength: 34306, dexterity: 123563, intelligence: 34306, constitution: 399001, luck: 80786, health: 940046336, minWeaponDmg: 4030, maxWeaponDmg: 6764 ),
                    new DungeonEnemy(position: 4, @class: ClassType.Mage, level: 600, strength: 39832, dexterity: 39832, intelligence: 136753, constitution: 417645, luck: 63190, health: 502009280, minWeaponDmg: 4609, maxWeaponDmg: 7769 ),
                    new DungeonEnemy(position: 5, @class: ClassType.Warrior, level: 612, strength: 137144, dexterity: 39634, intelligence: 39634, constitution: 447741, luck: 86381, health: 1372326144, minWeaponDmg: 2084, maxWeaponDmg: 3520 ),
                    new DungeonEnemy(position: 6, @class: ClassType.Warrior, level: 624, strength: 163224, dexterity: 64781, intelligence: 63721, constitution: 507599, luck: 93780, health: 1586246912, minWeaponDmg: 2127, maxWeaponDmg: 3587 ),
                    new DungeonEnemy(position: 7, @class: ClassType.Scout, level: 636, strength: 55499, dexterity: 174216, intelligence: 55499, constitution: 538992, luck: 94184, health: 1373351680, minWeaponDmg: 2722, maxWeaponDmg: 4531 ),
                    new DungeonEnemy(position: 8, @class: ClassType.BattleMage, level: 648, strength: 182015, dexterity: 41927, intelligence: 41927, constitution: 575061, luck: 99832, health: 1866072960, minWeaponDmg: 2212, maxWeaponDmg: 3727, armor: 14000 ),
                    new DungeonEnemy(position: 9, @class: ClassType.Warrior, level: 660, strength: 195334, dexterity: 50000, intelligence: 50000, constitution: 651487, luck: 117563, health: 2153164544, minWeaponDmg: 2251, maxWeaponDmg: 3798 ),
                    new DungeonEnemy(position: 10, @class: ClassType.Warrior, level: 666, strength: 229388, dexterity: 62835, intelligence: 62835, constitution: 711394, luck: 137813, health: 2372498944, minWeaponDmg: 2268, maxWeaponDmg: 3834 )
                }
            },
            new Dungeon
            {
                Name = DungeonNames.PlayaHQ,
                Position = 126,
                IsDefeated = false,
                IsUnlocked = true,
                UnlockResolve = c => c.Dungeons.First(d => d.Position == 115).IsDefeated,
                Type = DungeonTypeEnum.Shadow,
                DungeonEnemies = new List<DungeonEnemy>
                {
                    new DungeonEnemy(position: 1, @class: ClassType.Scout, level: 468, strength: 8140, dexterity: 23230, intelligence: 8100, constitution: 115210, luck: 4920, health: 216133952, minWeaponDmg: 7441, maxWeaponDmg: 14439 ),
                    new DungeonEnemy(position: 2, @class: ClassType.Warrior, level: 476, strength: 27870, dexterity: 9760, intelligence: 9720, constitution: 126730, luck: 5900, health: 302251040, minWeaponDmg: 7086, maxWeaponDmg: 14148 ),
                    new DungeonEnemy(position: 3, @class: ClassType.Mage, level: 484, strength: 11660, dexterity: 11710, intelligence: 33440, constitution: 139400, luck: 7080, health: 135218000, minWeaponDmg: 19460, maxWeaponDmg: 34885 ),
                    new DungeonEnemy(position: 4, @class: ClassType.Warrior, level: 492, strength: 40120, dexterity: 14050, intelligence: 13990, constitution: 153340, luck: 8490, health: 377983104, minWeaponDmg: 10206, maxWeaponDmg: 20378 ),
                    new DungeonEnemy(position: 5, @class: ClassType.Mage, level: 500, strength: 16780, dexterity: 16860, intelligence: 48140, constitution: 168670, luck: 10180, health: 169007344, minWeaponDmg: 28462, maxWeaponDmg: 52706 ),
                    new DungeonEnemy(position: 6, @class: ClassType.Warrior, level: 508, strength: 57760, dexterity: 20230, intelligence: 20130, constitution: 185530, luck: 12210, health: 472173856, minWeaponDmg: 14735, maxWeaponDmg: 29356 ),
                    new DungeonEnemy(position: 7, @class: ClassType.Warrior, level: 516, strength: 69310, dexterity: 24270, intelligence: 24150, constitution: 204080, luck: 14650, health: 527546816, minWeaponDmg: 17974, maxWeaponDmg: 35111 ),
                    new DungeonEnemy(position: 8, @class: ClassType.Warrior, level: 524, strength: 83170, dexterity: 29120, intelligence: 28980, constitution: 224480, luck: 17580, health: 589260032, minWeaponDmg: 21311, maxWeaponDmg: 41831 ),
                    new DungeonEnemy(position: 9, @class: ClassType.Scout, level: 532, strength: 34940, dexterity: 99800, intelligence: 34770, constitution: 246920, luck: 21090, health: 526433440, minWeaponDmg: 31926, maxWeaponDmg: 62970 ),
                    new DungeonEnemy(position: 10, @class: ClassType.Mage, level: 540, strength: 41920, dexterity: 41720, intelligence: 119760, constitution: 271610, luck: 25300, health: 293882016, minWeaponDmg: 70776, maxWeaponDmg: 136537 )
                }
            },
            new Dungeon
            {
                Name = DungeonNames.MountOlympus,
                Position = 127,
                IsDefeated = false,
                IsUnlocked = true,
                UnlockResolve = c => c.CharacterLevel >= 300,
                Type = DungeonTypeEnum.Shadow,
                DungeonEnemies = new List<DungeonEnemy>
                {
                    new DungeonEnemy(position: 1, @class: ClassType.Mage, level: 345, strength: 15000, dexterity: 15000, intelligence: 45000, constitution: 200000, luck: 23500, health: 2875500032, dungeonRuneBonuses: new DungeonEnemyRuneBonuses { DamageRuneType = RuneType.ColdDamage, FireResistance = 5, ColdResistance = 60, LightningResistance = 5, DamageBonus = 30 }, minWeaponDmg: 1038, maxWeaponDmg: 2069, armor: 12250),
                    new DungeonEnemy(position: 2, @class: ClassType.Warrior, level: 390, strength: 60000, dexterity: 20000, intelligence: 20000, constitution: 220000, luck: 31500, health: 8910000128, dungeonRuneBonuses: new DungeonEnemyRuneBonuses { DamageRuneType = RuneType.LightningDamage, FireResistance = 10, ColdResistance = 10, LightningResistance = 60, DamageBonus = 35 }, minWeaponDmg: 520, maxWeaponDmg: 1049, armor: 19500),
                    new DungeonEnemy(position: 3, @class: ClassType.Scout, level: 435, strength: 25000, dexterity: 75000, intelligence: 25000, constitution: 240000, luck: 45000, health: 8650800128, dungeonRuneBonuses: new DungeonEnemyRuneBonuses { DamageRuneType = RuneType.FireDamage, FireResistance = 60, ColdResistance = 15, LightningResistance = 15, DamageBonus = 35 }, minWeaponDmg: 725, maxWeaponDmg: 1459, armor: 21750),
                    new DungeonEnemy(position: 4, @class: ClassType.Mage, level: 485, strength: 30000, dexterity: 30000, intelligence: 90000, constitution: 260000, luck: 48500, health: 5212349952, dungeonRuneBonuses: new DungeonEnemyRuneBonuses { DamageRuneType = RuneType.ColdDamage, FireResistance = 20, ColdResistance = 60, LightningResistance = 20, DamageBonus = 40 }, minWeaponDmg: 1460, maxWeaponDmg: 2919, armor: 24250),
                    new DungeonEnemy(position: 5, @class: ClassType.Mage, level: 525, strength: 35000, dexterity: 35000, intelligence: 105000, constitution: 280000, luck: 53500, health: 6066899968, dungeonRuneBonuses: new DungeonEnemyRuneBonuses { DamageRuneType = RuneType.FireDamage, FireResistance = 60, ColdResistance = 25, LightningResistance = 25, DamageBonus = 40 }, minWeaponDmg: 1578, maxWeaponDmg: 3159, armor: 26250),
                    new DungeonEnemy(position: 6, @class: ClassType.Warrior, level: 565, strength: 120000, dexterity: 40000, intelligence: 40000, constitution: 300000, luck: 71500, health: 17465624576, dungeonRuneBonuses: new DungeonEnemyRuneBonuses { DamageRuneType = RuneType.LightningDamage, FireResistance = 40, ColdResistance = 40, LightningResistance = 40, DamageBonus = 45 }, minWeaponDmg: 750, maxWeaponDmg: 1509, armor: 14125),
                    new DungeonEnemy(position: 7, @class: ClassType.Warrior, level: 600, strength: 135000, dexterity: 45000, intelligence: 45000, constitution: 320000, luck: 63000, health: 19764000768, dungeonRuneBonuses: new DungeonEnemyRuneBonuses { DamageRuneType = RuneType.FireDamage, FireResistance = 60, ColdResistance = 60, LightningResistance = 60, DamageBonus = 45 }, minWeaponDmg: 800, maxWeaponDmg: 1599, armor: 30000),
                    new DungeonEnemy(position: 8, @class: ClassType.Scout, level: 625, strength: 50000, dexterity: 150000, intelligence: 50000, constitution: 340000, luck: 93500, health: 17487900672, dungeonRuneBonuses: new DungeonEnemyRuneBonuses { DamageRuneType = RuneType.ColdDamage, FireResistance = 60, ColdResistance = 60, LightningResistance = 60, DamageBonus = 50 }, minWeaponDmg: 1045, maxWeaponDmg: 2089, armor: 15625),
                    new DungeonEnemy(position: 9, @class: ClassType.Scout, level: 645, strength: 75000, dexterity: 200000, intelligence: 75000, constitution: 360000, luck: 100000, health: 19099799552, dungeonRuneBonuses: new DungeonEnemyRuneBonuses { DamageRuneType = RuneType.FireDamage, FireResistance = 60, ColdResistance = 60, LightningResistance = 60, DamageBonus = 60 }, minWeaponDmg: 1075, maxWeaponDmg: 2159, armor: 32250),
                    new DungeonEnemy(position: 10, @class: ClassType.Mage, level: 660, strength: 100000, dexterity: 100000, intelligence: 250000, constitution: 500000, luck: 137000, health: 13567500288, dungeonRuneBonuses: new DungeonEnemyRuneBonuses { DamageRuneType = RuneType.LightningDamage, FireResistance = 60, ColdResistance = 60, LightningResistance = 60, DamageBonus = 60 }, minWeaponDmg: 2000, maxWeaponDmg: 4000, armor: 33000)
                }
            },
            new Dungeon
            {
                Name = DungeonNames.MonsterGrotto,
                Position = 128,
                IsDefeated = false,
                IsUnlocked = true,
                UnlockResolve = c => c.CharacterLevel >= 600,
                Type = DungeonTypeEnum.Shadow,
                DungeonEnemies = new List<DungeonEnemy>
                {
                    new DungeonEnemy(position: 1, @class: ClassType.Warrior, level: 600, strength: 280000, dexterity: 140000, intelligence: 140000, constitution: 430000, luck: 115000, health: 15000000000, minWeaponDmg: 3500, maxWeaponDmg: 3750, dungeonRuneBonuses: new DungeonEnemyRuneBonuses { DamageRuneType = RuneType.LightningDamage, FireResistance = 0, ColdResistance = 0, LightningResistance = 25, DamageBonus = 50 }, armor: 35000 ),
                    new DungeonEnemy(position: 2, @class: ClassType.Mage, level: 605, strength: 142500, dexterity: 142500, intelligence: 285000, constitution: 434000, luck: 116250, health: 15350000000, minWeaponDmg: 3625, maxWeaponDmg: 3875, dungeonRuneBonuses: new DungeonEnemyRuneBonuses { DamageRuneType = RuneType.ColdDamage, FireResistance = 0, ColdResistance = 25, LightningResistance = 0, DamageBonus = 50 }, armor: 35000 ),
                    new DungeonEnemy(position: 3, @class: ClassType.Scout, level: 610, strength: 157500, dexterity: 315000, intelligence: 157500, constitution: 458000, luck: 123750, health: 17450000000, minWeaponDmg: 4375, maxWeaponDmg: 4625, dungeonRuneBonuses: new DungeonEnemyRuneBonuses { DamageRuneType = RuneType.LightningDamage, FireResistance = 0, ColdResistance = 0, LightningResistance = 25, DamageBonus = 50 }, armor: 35000 ),
                    new DungeonEnemy(position: 4, @class: ClassType.Warrior, level: 615, strength: 320000, dexterity: 160000, intelligence: 160000, constitution: 462000, luck: 125000, health: 17800000000, minWeaponDmg: 4500, maxWeaponDmg: 4750, dungeonRuneBonuses: new DungeonEnemyRuneBonuses { DamageRuneType = RuneType.FireDamage, FireResistance = 25, ColdResistance = 0, LightningResistance = 0, DamageBonus = 50 }, armor: 35000 ),
                    new DungeonEnemy(position: 5, @class: ClassType.Mage, level: 620, strength: 175000, dexterity: 175000, intelligence: 350000, constitution: 486000, luck: 132500, health: 19900000000, minWeaponDmg: 5250, maxWeaponDmg: 5500, dungeonRuneBonuses: new DungeonEnemyRuneBonuses { DamageRuneType = RuneType.ColdDamage, FireResistance = 0, ColdResistance = 25, LightningResistance = 0, DamageBonus = 50 }, armor: 35000 ),
                    new DungeonEnemy(position: 6, @class: ClassType.Warrior, level: 625, strength: 355000, dexterity: 177500, intelligence: 177500, constitution: 490000, luck: 133750, health: 20250000000, minWeaponDmg: 5375, maxWeaponDmg: 5625, dungeonRuneBonuses: new DungeonEnemyRuneBonuses { DamageRuneType = RuneType.FireDamage, FireResistance = 25, ColdResistance = 0, LightningResistance = 0, DamageBonus = 50 }, armor: 35000 ),
                    new DungeonEnemy(position: 7, @class: ClassType.Scout, level: 630, strength: 203000, dexterity: 406000, intelligence: 203000, constitution: 514000, luck: 141250, health: 22350000000, minWeaponDmg: 6125, maxWeaponDmg: 6375, dungeonRuneBonuses: new DungeonEnemyRuneBonuses { DamageRuneType = RuneType.LightningDamage, FireResistance = 0, ColdResistance = 0, LightningResistance = 25, DamageBonus = 50 }, armor: 35000 ),
                    new DungeonEnemy(position: 8, @class: ClassType.Scout, level: 635, strength: 206000, dexterity: 412000, intelligence: 206000, constitution: 518000, luck: 142500, health: 22700000000, minWeaponDmg: 6250, maxWeaponDmg: 6500, dungeonRuneBonuses: new DungeonEnemyRuneBonuses { DamageRuneType = RuneType.FireDamage, FireResistance = 25, ColdResistance = 0, LightningResistance = 0, DamageBonus = 50 }, armor: 35000 ),
                    new DungeonEnemy(position: 9, @class: ClassType.Warrior, level: 640, strength: 448000, dexterity: 224000, intelligence: 224000, constitution: 542000, luck: 150000, health: 24800000000, minWeaponDmg: 7000, maxWeaponDmg: 7250, dungeonRuneBonuses: new DungeonEnemyRuneBonuses { DamageRuneType = RuneType.ColdDamage, FireResistance = 0, ColdResistance = 25, LightningResistance = 0, DamageBonus = 50 }, armor: 35000 ),
                    new DungeonEnemy(position: 10, @class: ClassType.Warrior, level: 650, strength: 460000, dexterity: 230000, intelligence: 230000, constitution: 550000, luck: 152500, health: 25500000000, minWeaponDmg: 7250, maxWeaponDmg: 7500, dungeonRuneBonuses: new DungeonEnemyRuneBonuses { DamageRuneType = RuneType.FireDamage, FireResistance = 25, ColdResistance = 0, LightningResistance = 0, DamageBonus = 50 }, armor: 35000 )
                }
            },

            #endregion

            #region Loop of Idols

            new Dungeon {
                Name = DungeonNames.ContinousLoopOfIdols,
                Position = 130,
                IsDefeated = false,
                IsUnlocked = true,
                UnlockResolve = c => c.Dungeons.First(d => d.Type == DungeonTypeEnum.Tower).IsUnlocked && c.CharacterLevel >= 222,
                Type = DungeonTypeEnum.LoopOfIdols,
                DungeonEnemies = new List<DungeonEnemy>
                {
                    new DungeonEnemy(position: 1, @class: ClassType.Warrior, level: 222, strength: 90000, dexterity: 2000, intelligence: 2000, constitution: 74000, luck: 5000, health: 80000000, minWeaponDmg: 2000, maxWeaponDmg: 2996 ),
                    new DungeonEnemy(position: 2, @class: ClassType.Warrior, level: 244, strength: 100000, dexterity: 2500, intelligence: 2500, constitution: 84000, luck: 6000, health: 100000000, minWeaponDmg: 2400, maxWeaponDmg: 3399 ),
                    new DungeonEnemy(position: 3, @class: ClassType.Mage, level: 266, strength: 3000, dexterity: 3000, intelligence: 120000, constitution: 100000, luck: 7200, health: 120000000, minWeaponDmg: 6304, maxWeaponDmg: 8544 ),
                    new DungeonEnemy(position: 4, @class: ClassType.Scout, level: 288, strength: 3500, dexterity: 140000, intelligence: 3500, constitution: 120000, luck: 8600, health: 144000000, minWeaponDmg: 4001, maxWeaponDmg: 5249 ),
                    new DungeonEnemy(position: 5, @class: ClassType.Mage, level: 333, strength: 4000, dexterity: 4000, intelligence: 180000, constitution: 144000, luck: 10300, health: 172000000, minWeaponDmg: 7885, maxWeaponDmg: 10348 ),
                    new DungeonEnemy(position: 6, @class: ClassType.Mage, level: 366, strength: 4500, dexterity: 4500, intelligence: 215000, constitution: 172000, luck: 12400, health: 206000000, minWeaponDmg: 9015, maxWeaponDmg: 11236 ),
                    new DungeonEnemy(position: 7, @class: ClassType.Mage, level: 399, strength: 5000, dexterity: 5000, intelligence: 240000, constitution: 206000, luck: 15000, health: 248000000, minWeaponDmg: 10125, maxWeaponDmg: 13498 ),
                    new DungeonEnemy(position: 8, @class: ClassType.BattleMage, level: 444, strength: 290000, dexterity: 6000, intelligence: 6000, constitution: 248000, luck: 18000, health: 300000000, minWeaponDmg: 5001, maxWeaponDmg: 6499 ),
                    new DungeonEnemy(position: 9, @class: ClassType.Berserker, level: 488, strength: 350000, dexterity: 7000, intelligence: 7000, constitution: 300000, luck: 21500, health: 360000000, minWeaponDmg: 4465, maxWeaponDmg: 6102 ),
                    new DungeonEnemy(position: 10, @class: ClassType.Assassin, level: 555, strength: 10000, dexterity: 400000, intelligence: 10000, constitution: 360000, luck: 24000, health: 430000000, minWeaponDmg: 12001, maxWeaponDmg: 15998 ),
                    new DungeonEnemy(position: 11, @class: ClassType.Mage, level: 557, strength: 10100, dexterity: 10100, intelligence: 404000, constitution: 361800, luck: 24240, health: 403768800, minWeaponDmg: 13636, maxWeaponDmg: 18177 ),
                    new DungeonEnemy(position: 12, @class: ClassType.Scout, level: 559, strength: 10200, dexterity: 408040, intelligence: 10200, constitution: 363600, luck: 24480, health: 814464000, minWeaponDmg: 7653, maxWeaponDmg: 10198 ),
                    new DungeonEnemy(position: 13, @class: ClassType.BattleMage, level: 561, strength: 412120, dexterity: 10300, intelligence: 10300, constitution: 365410, luck: 24720, health: 1026802112, minWeaponDmg: 6182, maxWeaponDmg: 8241 ),
                    new DungeonEnemy(position: 14, @class: ClassType.Warrior, level: 563, strength: 416240, dexterity: 10400, intelligence: 10400, constitution: 367230, luck: 24960, health: 1035588608, minWeaponDmg: 6244, maxWeaponDmg: 8322 ),
                    new DungeonEnemy(position: 15, @class: ClassType.Berserker, level: 565, strength: 420400, dexterity: 10500, intelligence: 10500, constitution: 369060, luck: 25210, health: 835551872, minWeaponDmg: 5110, maxWeaponDmg: 7507 ),
                    new DungeonEnemy(position: 16, @class: ClassType.Mage, level: 567, strength: 10600, dexterity: 10600, intelligence: 424600, constitution: 370900, luck: 25460, health: 421342400, minWeaponDmg: 14335, maxWeaponDmg: 19104 ),
                    new DungeonEnemy(position: 17, @class: ClassType.Warrior, level: 569, strength: 428840, dexterity: 10700, intelligence: 10700, constitution: 372750, luck: 25710, health: 1062337472, minWeaponDmg: 6432, maxWeaponDmg: 8569 ),
                    new DungeonEnemy(position: 18, @class: ClassType.Mage, level: 571, strength: 10800, dexterity: 10800, intelligence: 433120, constitution: 374610, luck: 25960, health: 428553856, minWeaponDmg: 14620, maxWeaponDmg: 19487 ),
                    new DungeonEnemy(position: 19, @class: ClassType.DemonHunter, level: 573, strength: 10900, dexterity: 437450, intelligence: 10900, constitution: 376480, luck: 26220, health: 864398080, minWeaponDmg: 8201, maxWeaponDmg: 10935 ),
                    new DungeonEnemy(position: 20, @class: ClassType.Berserker, level: 575, strength: 441820, dexterity: 11000, intelligence: 11000, constitution: 378360, luck: 26480, health: 871741440, minWeaponDmg: 5315, maxWeaponDmg: 7685 ),
                    new DungeonEnemy(position: 21, @class: ClassType.Mage, level: 580, strength: 12000, dexterity: 12000, intelligence: 450000, constitution: 380000, luck: 27000, health: 450000000, minWeaponDmg: 15000, maxWeaponDmg: 20000, armor: 30000 )
                }
            }

            #endregion
        };

        foreach (var dungeon in dungeons)
        {
            foreach (var dungeonEnemy in dungeon.DungeonEnemies)
            {
                dungeonEnemy.Dungeon = dungeon;
            }
        }

        return dungeons;
    }
}

internal static class DungeonProviderExtensions
{
    internal static IEnumerable<Dungeon> InitMirrorEnemy(this IEnumerable<Dungeon> dungeons, SimulationContext simulationContext)
    {
        _ = dungeons.SelectMany(d => d.DungeonEnemies).InitMirrorEnemy(simulationContext);
        return dungeons;
    }

    internal static IEnumerable<DungeonEnemy> InitMirrorEnemy(this IEnumerable<DungeonEnemy> dungeonEnemies, SimulationContext simulationContext)
    {
        var mirrorEnemies = dungeonEnemies.Where(e => e.IsDefeated == false && e.Dungeon.Position is 9 or 109 && e.Position is 10);
        foreach (var mirrorEnemy in mirrorEnemies)
        {
            mirrorEnemy.Class = simulationContext.Class;
            mirrorEnemy.Strength = simulationContext.Strength;
            mirrorEnemy.Dexterity = simulationContext.Dexterity;
            mirrorEnemy.Intelligence = simulationContext.Intelligence;
            mirrorEnemy.Constitution = simulationContext.Constitution;
            mirrorEnemy.Luck = simulationContext.Luck;
            if (simulationContext.FirstWeapon is not null)
                mirrorEnemy.FirstWeapon = EquipmentBuilder.ToRawWeapon(simulationContext.FirstWeapon);
            if (simulationContext.SecondWeapon is not null)
                mirrorEnemy.SecondWeapon = EquipmentBuilder.ToRawWeapon(simulationContext.SecondWeapon);
            mirrorEnemy.Armor = simulationContext.Armor;
            mirrorEnemy.Reaction = simulationContext.Reaction;
            mirrorEnemy.HealthRune = simulationContext.HealthRune;
            mirrorEnemy.LightningResistance = simulationContext.LightningResistance;
            mirrorEnemy.ColdResistance = simulationContext.ColdResistance;
            mirrorEnemy.FireResistance = simulationContext.FireResistance;
            mirrorEnemy.Health = simulationContext.Health / (simulationContext.Level + 1) * (mirrorEnemy.Level + 1);
        }

        return dungeonEnemies;
    }
}