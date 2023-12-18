namespace SFSimulator.Core;

public class DungeonProvider : IDungeonProvider
{
    private List<Dungeon> Dungeons { get; set; } = null!;
    public DungeonProvider()
    {
        InitDungeons();
    }

    public List<DungeonEnemy> GetFightablesDungeonEnemies()
    {
        return Dungeons.Where(d => d.IsUnlocked && !d.IsDefeated).SelectMany(d => d.DungeonEnemies).Where(e => !e.IsDefeated).ToList();
    }
    public List<Dungeon> GetAllDungeons()
    {
        return Dungeons;
    }
    public bool IsValidEnemy(int dungeonPosition, int dungeonEnemyPosition)
    {
        return Dungeons.FirstOrDefault(d => d.Position == dungeonPosition)?.DungeonEnemies.FirstOrDefault(e => e.Position == dungeonEnemyPosition) is not null;
    }

    public DungeonEnemy GetDungeonEnemy(int dungeonPositon, int dungeonEnemyPosition)
    {
        var dungeon = Dungeons.FirstOrDefault(d => d.Position == dungeonPositon) ?? throw new ArgumentOutOfRangeException(nameof(dungeonPositon));
        var enemy = dungeon.DungeonEnemies.FirstOrDefault(e => e.Position == dungeonEnemyPosition) ?? throw new ArgumentOutOfRangeException(nameof(dungeonEnemyPosition));
        return enemy;
    }

    private void InitDungeons()
    {
        Dungeons = new List<Dungeon>
        {
            #region Light World
            new Dungeon
            {
                Name = DungeonNames.DesecratedCatacombs,
                Position = 1,
                IsDefeated = false,
                IsUnlocked = true,
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
                    new DungeonEnemy(position: 9, @class: ClassType.Mage, level: 190, strength: 900, dexterity: 920, intelligence: 3720, constitution: 3280, luck: 1340, health: 1252960, minWeaponDmg: 439, maxWeaponDmg: 621 )
                }
            },
            new Dungeon
            {
                Name = DungeonNames.BlackSkullFortress,
                Position = 10,
                IsDefeated = false,
                IsUnlocked = true,
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
                DungeonEnemies = new List<DungeonEnemy>
                {
                    new DungeonEnemy(position: 1, @class: ClassType.Scout, level: 210, strength: 3100, dexterity: 6200, intelligence: 3100, constitution: 10500, luck: 3100, health: 16000000, minWeaponDmg: 559, maxWeaponDmg: 787, dungeonRuneBonuses: new DungeonEnemyRuneBonuses { DamageRuneType = RuneType.FireDamage, FireResistance = 25, ColdResistance = 25, LightningResistance = 25, DamageBonus = 25 }, armor: 10750 ),
                    new DungeonEnemy(position: 2, @class: ClassType.Warrior, level: 213, strength: 6560, dexterity: 3280, intelligence: 3280, constitution: 12000, luck: 3280, health: 20500000, minWeaponDmg: 561, maxWeaponDmg: 772, dungeonRuneBonuses: new DungeonEnemyRuneBonuses { DamageRuneType = RuneType.LightningDamage, FireResistance = 25, ColdResistance = 25, LightningResistance = 25, DamageBonus = 25 }, armor: 11000 ),
                    new DungeonEnemy(position: 3, @class: ClassType.Mage, level: 216, strength: 3460, dexterity: 3460, intelligence: 6920, constitution: 13500, luck: 3460, health: 25000000, minWeaponDmg: 567, maxWeaponDmg: 787, dungeonRuneBonuses: new DungeonEnemyRuneBonuses { DamageRuneType = RuneType.LightningDamage, FireResistance = 25, ColdResistance = 25, LightningResistance = 25, DamageBonus = 25 }, armor: 11250 ),
                    new DungeonEnemy(position: 4, @class: ClassType.Scout, level: 219, strength: 3640, dexterity: 7280, intelligence: 3640, constitution: 15000, luck: 3640, health: 29500000, minWeaponDmg: 582, maxWeaponDmg: 819, dungeonRuneBonuses: new DungeonEnemyRuneBonuses { DamageRuneType = RuneType.ColdDamage, FireResistance = 25, ColdResistance = 25, LightningResistance = 25, DamageBonus = 25 }, armor: 11500 ),
                    new DungeonEnemy(position: 5, @class: ClassType.Scout, level: 222, strength: 3820, dexterity: 7640, intelligence: 3820, constitution: 16500, luck: 3820, health: 34000000, minWeaponDmg: 598, maxWeaponDmg: 837, dungeonRuneBonuses: new DungeonEnemyRuneBonuses { DamageRuneType = RuneType.FireDamage, FireResistance = 25, ColdResistance = 25, LightningResistance = 25, DamageBonus = 25 }, armor: 11750 ),
                    new DungeonEnemy(position: 6, @class: ClassType.Scout, level: 225, strength: 4000, dexterity: 8000, intelligence: 4000, constitution: 18000, luck: 4000, health: 38500000, minWeaponDmg: 601, maxWeaponDmg: 850, dungeonRuneBonuses: new DungeonEnemyRuneBonuses { DamageRuneType = RuneType.LightningDamage, FireResistance = 25, ColdResistance = 25, LightningResistance = 25, DamageBonus = 25 }, armor: 12000 ),
                    new DungeonEnemy(position: 7, @class: ClassType.Warrior, level: 228, strength: 8360, dexterity: 4180, intelligence: 4180, constitution: 19500, luck: 4180, health: 43000000, minWeaponDmg: 614, maxWeaponDmg: 862, dungeonRuneBonuses: new DungeonEnemyRuneBonuses { DamageRuneType = RuneType.LightningDamage, FireResistance = 25, ColdResistance = 25, LightningResistance = 25, DamageBonus = 25 }, armor: 12250 ),
                    new DungeonEnemy(position: 8, @class: ClassType.Warrior, level: 231, strength: 8720, dexterity: 4360, intelligence: 4360, constitution: 21000, luck: 4360, health: 47500000, minWeaponDmg: 626, maxWeaponDmg: 874, dungeonRuneBonuses: new DungeonEnemyRuneBonuses { DamageRuneType = RuneType.FireDamage, FireResistance = 25, ColdResistance = 25, LightningResistance = 25, DamageBonus = 25 }, armor: 12500 ),
                    new DungeonEnemy(position: 9, @class: ClassType.Scout, level: 234, strength: 4540, dexterity: 9080, intelligence: 4540, constitution: 22500, luck: 4540, health: 52000000, minWeaponDmg: 637, maxWeaponDmg: 885, dungeonRuneBonuses: new DungeonEnemyRuneBonuses { DamageRuneType = RuneType.ColdDamage, FireResistance = 25, ColdResistance = 25, LightningResistance = 25, DamageBonus = 25 }, armor: 12750 ),
                    new DungeonEnemy(position: 10, @class: ClassType.Mage, level: 240, strength: 4900, dexterity: 4900, intelligence: 9800, constitution: 25500, luck: 4900, health: 61000000, minWeaponDmg: 663, maxWeaponDmg: 909, dungeonRuneBonuses: new DungeonEnemyRuneBonuses { DamageRuneType = RuneType.ColdDamage, FireResistance = 25, ColdResistance = 25, LightningResistance = 25, DamageBonus = 25 }, armor: 13000 )
                }
            },
            new Dungeon
            {
                Name = DungeonNames.HouseOfHorrors,
                Position = 14,
                IsDefeated = false,
                IsUnlocked = true,
                DungeonEnemies = new List<DungeonEnemy>
                {
                    new DungeonEnemy(position: 1, @class: ClassType.Mage, level: 240, strength: 4900, dexterity: 4900, intelligence: 9800, constitution: 25500, luck: 4900, health: 61000000, minWeaponDmg: 662, maxWeaponDmg: 908, dungeonRuneBonuses: new DungeonEnemyRuneBonuses { DamageRuneType = RuneType.ColdDamage, FireResistance = 25, ColdResistance = 25, LightningResistance = 25, DamageBonus = 25 }, armor: 13250 ),
                    new DungeonEnemy(position: 2, @class: ClassType.Warrior, level: 243, strength: 10160, dexterity: 5080, intelligence: 5080, constitution: 27000, luck: 5080, health: 65500000, minWeaponDmg: 673, maxWeaponDmg: 922, dungeonRuneBonuses: new DungeonEnemyRuneBonuses { DamageRuneType = RuneType.LightningDamage, FireResistance = 25, ColdResistance = 25, LightningResistance = 25, DamageBonus = 25 }, armor: 13500 ),
                    new DungeonEnemy(position: 3, @class: ClassType.Warrior, level: 246, strength: 10520, dexterity: 5260, intelligence: 5260, constitution: 28500, luck: 5260, health: 70000000, minWeaponDmg: 685, maxWeaponDmg: 933, dungeonRuneBonuses: new DungeonEnemyRuneBonuses { DamageRuneType = RuneType.LightningDamage, FireResistance = 25, ColdResistance = 25, LightningResistance = 25, DamageBonus = 25 }, armor: 13750 ),
                    new DungeonEnemy(position: 4, @class: ClassType.Warrior, level: 249, strength: 10880, dexterity: 5440, intelligence: 5440, constitution: 30000, luck: 5440, health: 74500000, minWeaponDmg: 697, maxWeaponDmg: 946, dungeonRuneBonuses: new DungeonEnemyRuneBonuses { DamageRuneType = RuneType.FireDamage, FireResistance = 25, ColdResistance = 25, LightningResistance = 25, DamageBonus = 25 }, armor: 14000 ),
                    new DungeonEnemy(position: 5, @class: ClassType.Scout, level: 252, strength: 5620, dexterity: 11240, intelligence: 5620, constitution: 31500, luck: 5620, health: 79000000, minWeaponDmg: 709, maxWeaponDmg: 958, dungeonRuneBonuses: new DungeonEnemyRuneBonuses { DamageRuneType = RuneType.ColdDamage,FireResistance = 25, ColdResistance = 25, LightningResistance = 25, DamageBonus = 25 }, armor: 14250 ),
                    new DungeonEnemy(position: 6, @class: ClassType.Mage, level: 255, strength: 5800, dexterity: 5800, intelligence: 11600, constitution: 33000, luck: 5800, health: 83500000, minWeaponDmg: 721, maxWeaponDmg: 966, dungeonRuneBonuses: new DungeonEnemyRuneBonuses { DamageRuneType = RuneType.FireDamage, FireResistance = 25, ColdResistance = 25, LightningResistance = 25, DamageBonus = 25 }, armor: 14500 ),
                    new DungeonEnemy(position: 7, @class: ClassType.Scout, level: 258, strength: 5980, dexterity: 11960, intelligence: 5980, constitution: 34500, luck: 5980, health: 88000000, minWeaponDmg: 733, maxWeaponDmg: 982, dungeonRuneBonuses: new DungeonEnemyRuneBonuses { DamageRuneType = RuneType.LightningDamage, FireResistance = 25, ColdResistance = 25, LightningResistance = 25, DamageBonus = 25 }, armor: 14750 ),
                    new DungeonEnemy(position: 8, @class: ClassType.Mage, level: 261, strength: 6160, dexterity: 6160, intelligence: 12320, constitution: 36000, luck: 6160, health: 92500000, minWeaponDmg: 744, maxWeaponDmg: 994, dungeonRuneBonuses: new DungeonEnemyRuneBonuses { DamageRuneType = RuneType.FireDamage, FireResistance = 25, ColdResistance = 25, LightningResistance = 25, DamageBonus = 25 }, armor: 15000 ),
                    new DungeonEnemy(position: 9, @class: ClassType.Mage, level: 264, strength: 6340, dexterity: 6340, intelligence: 12680, constitution: 37500, luck: 6340, health: 97000000, minWeaponDmg: 756, maxWeaponDmg: 1002, dungeonRuneBonuses: new DungeonEnemyRuneBonuses { DamageRuneType = RuneType.FireDamage, FireResistance = 25, ColdResistance = 25, LightningResistance = 25, DamageBonus = 25 }, armor: 15250 ),
                    new DungeonEnemy(position: 10, @class: ClassType.Scout, level: 270, strength: 6700, dexterity: 13400, intelligence: 6700, constitution: 40500, luck: 6700, health: 106000000, minWeaponDmg: 781, maxWeaponDmg: 1029, dungeonRuneBonuses: new DungeonEnemyRuneBonuses { DamageRuneType = RuneType.ColdDamage, FireResistance = 25, ColdResistance = 25, LightningResistance = 25, DamageBonus = 25 }, armor: 15500 )
                }
            },
            new Dungeon
            {
                Name = DungeonNames.ThirteenthFloor,
                Position = 15,
                IsDefeated = false,
                IsUnlocked = true,
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
                DungeonEnemies = new List<DungeonEnemy>
                {
                    new DungeonEnemy(position: 1, @class: ClassType.Mage, level: 280, strength: 7300, dexterity: 7300, intelligence: 14600, constitution: 45500, luck: 7300, health: 121000000, minWeaponDmg: 820, maxWeaponDmg: 1069, dungeonRuneBonuses: new DungeonEnemyRuneBonuses { DamageRuneType = RuneType.LightningDamage, FireResistance = 25, ColdResistance = 25, LightningResistance = 25, DamageBonus = 25 }, armor: 16500 ),
                    new DungeonEnemy(position: 2, @class: ClassType.Warrior, level: 283, strength: 14960, dexterity: 7480, intelligence: 7480, constitution: 47000, luck: 7480, health: 125500000, minWeaponDmg: 832, maxWeaponDmg: 1080, dungeonRuneBonuses: new DungeonEnemyRuneBonuses { DamageRuneType = RuneType.LightningDamage, FireResistance = 25, ColdResistance = 25, LightningResistance = 25, DamageBonus = 25 }, armor: 16750 ),
                    new DungeonEnemy(position: 3, @class: ClassType.Scout, level: 286, strength: 7660, dexterity: 15320, intelligence: 7660, constitution: 48500, luck: 7660, health: 130000000, minWeaponDmg: 847, maxWeaponDmg: 1094, dungeonRuneBonuses: new DungeonEnemyRuneBonuses { DamageRuneType = RuneType.ColdDamage, FireResistance = 25, ColdResistance = 25, LightningResistance = 25, DamageBonus = 25 }, armor: 17000 ),
                    new DungeonEnemy(position: 4, @class: ClassType.Warrior, level: 289, strength: 15680, dexterity: 7840, intelligence: 7840, constitution: 50000, luck: 7840, health: 134500000, minWeaponDmg: 857, maxWeaponDmg: 1106, dungeonRuneBonuses: new DungeonEnemyRuneBonuses { DamageRuneType = RuneType.FireDamage, FireResistance = 25, ColdResistance = 25, LightningResistance = 25, DamageBonus = 25 }, armor: 17250 ),
                    new DungeonEnemy(position: 5, @class: ClassType.Scout, level: 292, strength: 8020, dexterity: 16040, intelligence: 8020, constitution: 51500, luck: 8020, health: 139000000, minWeaponDmg: 868, maxWeaponDmg: 1118, dungeonRuneBonuses: new DungeonEnemyRuneBonuses { DamageRuneType = RuneType.ColdDamage, FireResistance = 25, ColdResistance = 25, LightningResistance = 25, DamageBonus = 25 }, armor: 17500 ),
                    new DungeonEnemy(position: 6, @class: ClassType.Warrior, level: 295, strength: 16400, dexterity: 8200, intelligence: 8200, constitution: 53000, luck: 8200, health: 143500000, minWeaponDmg: 880, maxWeaponDmg: 1130, dungeonRuneBonuses: new DungeonEnemyRuneBonuses { DamageRuneType = RuneType.LightningDamage, FireResistance = 25, ColdResistance = 25, LightningResistance = 25, DamageBonus = 25 }, armor: 17750 ),
                    new DungeonEnemy(position: 7, @class: ClassType.Scout, level: 298, strength: 8380, dexterity: 16760, intelligence: 8380, constitution: 54500, luck: 8380, health: 148000000, minWeaponDmg: 894, maxWeaponDmg: 1142, dungeonRuneBonuses: new DungeonEnemyRuneBonuses { DamageRuneType = RuneType.LightningDamage, FireResistance = 25, ColdResistance = 25, LightningResistance = 25, DamageBonus = 25 }, armor: 18000 ),
                    new DungeonEnemy(position: 8, @class: ClassType.Mage, level: 301, strength: 8560, dexterity: 8560, intelligence: 17120, constitution: 56000, luck: 8560, health: 152500000, minWeaponDmg: 908, maxWeaponDmg: 1152, dungeonRuneBonuses: new DungeonEnemyRuneBonuses { DamageRuneType = RuneType.FireDamage, FireResistance = 25, ColdResistance = 25, LightningResistance = 25, DamageBonus = 25 }, armor: 18250 ),
                    new DungeonEnemy(position: 9, @class: ClassType.Mage, level: 304, strength: 8740, dexterity: 8740, intelligence: 17480, constitution: 57500, luck: 8740, health: 157000000, minWeaponDmg: 918, maxWeaponDmg: 1162, dungeonRuneBonuses: new DungeonEnemyRuneBonuses { DamageRuneType = RuneType.FireDamage, FireResistance = 25, ColdResistance = 25, LightningResistance = 25, DamageBonus = 25 }, armor: 18500 ),
                    new DungeonEnemy(position: 10, @class: ClassType.Scout, level: 310, strength: 9100, dexterity: 18200, intelligence: 9100, constitution: 60500, luck: 9100, health: 166000000, minWeaponDmg: 941, maxWeaponDmg: 1189, dungeonRuneBonuses: new DungeonEnemyRuneBonuses { DamageRuneType = RuneType.ColdDamage, FireResistance = 25, ColdResistance = 25, LightningResistance = 25, DamageBonus = 25 }, armor: 18750 )
                }
            },
            new Dungeon
            {
                Name = DungeonNames.TimeHonoredSchoolOfMagic,
                Position = 17,
                IsDefeated = false,
                IsUnlocked = true,
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
                DungeonEnemies = new List<DungeonEnemy>
                {
                    new DungeonEnemy(position: 1, @class: ClassType.Warrior, level: 313, strength: 18560, dexterity: 9280, intelligence: 9280, constitution: 62000, luck: 9280, health: 170500000, minWeaponDmg: 953, maxWeaponDmg: 1199, dungeonRuneBonuses: new DungeonEnemyRuneBonuses { DamageRuneType = RuneType.LightningDamage, FireResistance = 25, ColdResistance = 25, LightningResistance = 25, DamageBonus = 25 }, armor: 19000 ),
                    new DungeonEnemy(position: 2, @class: ClassType.Warrior, level: 316, strength: 18920, dexterity: 9460, intelligence: 9460, constitution: 63500, luck: 9460, health: 175000000, minWeaponDmg: 964, maxWeaponDmg: 1211, dungeonRuneBonuses: new DungeonEnemyRuneBonuses { DamageRuneType = RuneType.FireDamage, FireResistance = 25, ColdResistance = 25, LightningResistance = 25, DamageBonus = 25 }, armor: 19250 ),
                    new DungeonEnemy(position: 3, @class: ClassType.Mage, level: 319, strength: 9640, dexterity: 9640, intelligence: 19280, constitution: 65000, luck: 9640, health: 179500000, minWeaponDmg: 977, maxWeaponDmg: 1225, dungeonRuneBonuses: new DungeonEnemyRuneBonuses { DamageRuneType = RuneType.ColdDamage, FireResistance = 25, ColdResistance = 25, LightningResistance = 25, DamageBonus = 25 }, armor: 19500 ),
                    new DungeonEnemy(position: 4, @class: ClassType.Warrior, level: 322, strength: 19640, dexterity: 9820, intelligence: 9820, constitution: 66500, luck: 9820, health: 184000000, minWeaponDmg: 989, maxWeaponDmg: 1237, dungeonRuneBonuses: new DungeonEnemyRuneBonuses { DamageRuneType = RuneType.ColdDamage, FireResistance = 25, ColdResistance = 25, LightningResistance = 25, DamageBonus = 25 }, armor: 19750 ),
                    new DungeonEnemy(position: 5, @class: ClassType.Mage, level: 325, strength: 10000, dexterity: 10000, intelligence: 20000, constitution: 68000, luck: 10000, health: 188500000, minWeaponDmg: 1000, maxWeaponDmg: 1247, dungeonRuneBonuses: new DungeonEnemyRuneBonuses { DamageRuneType = RuneType.LightningDamage, FireResistance = 25, ColdResistance = 25, LightningResistance = 25, DamageBonus = 25 }, armor: 20000 ),
                    new DungeonEnemy(position: 6, @class: ClassType.Scout, level: 328, strength: 10180, dexterity: 20360, intelligence: 10180, constitution: 69500, luck: 10180, health: 193000000, minWeaponDmg: 1016, maxWeaponDmg: 1254, dungeonRuneBonuses: new DungeonEnemyRuneBonuses { DamageRuneType = RuneType.LightningDamage, FireResistance = 25, ColdResistance = 25, LightningResistance = 25, DamageBonus = 25 }, armor: 20250 ),
                    new DungeonEnemy(position: 7, @class: ClassType.Mage, level: 331, strength: 10360, dexterity: 10360, intelligence: 20720, constitution: 71000, luck: 10360, health: 197500000, minWeaponDmg: 1024, maxWeaponDmg: 1274, dungeonRuneBonuses: new DungeonEnemyRuneBonuses { DamageRuneType = RuneType.FireDamage, FireResistance = 25, ColdResistance = 25, LightningResistance = 25, DamageBonus = 25 }, armor: 20500 ),
                    new DungeonEnemy(position: 8, @class: ClassType.Mage, level: 334, strength: 10540, dexterity: 10540, intelligence: 21080, constitution: 72500, luck: 10540, health: 202000000, minWeaponDmg: 1037, maxWeaponDmg: 1286, dungeonRuneBonuses: new DungeonEnemyRuneBonuses { DamageRuneType = RuneType.FireDamage, FireResistance = 25, ColdResistance = 25, LightningResistance = 25, DamageBonus = 25 }, armor: 20750 ),
                    new DungeonEnemy(position: 9, @class: ClassType.Warrior, level: 337, strength: 21440, dexterity: 10720, intelligence: 10720, constitution: 74000, luck: 10720, health: 206500000, minWeaponDmg: 1050, maxWeaponDmg: 1298, dungeonRuneBonuses: new DungeonEnemyRuneBonuses { DamageRuneType = RuneType.ColdDamage, FireResistance = 25, ColdResistance = 25, LightningResistance = 25, DamageBonus = 25 }, armor: 21000 ),
                    new DungeonEnemy(position: 10, @class: ClassType.Mage, level: 340, strength: 10900, dexterity: 10900, intelligence: 21800, constitution: 75500, luck: 10900, health: 211000000, minWeaponDmg: 1061, maxWeaponDmg: 1310, dungeonRuneBonuses: new DungeonEnemyRuneBonuses { DamageRuneType = RuneType.LightningDamage, FireResistance = 25, ColdResistance = 25, LightningResistance = 25, DamageBonus = 25 }, armor: 21250 )
                }
            },
            new Dungeon
            {
                Name = DungeonNames.TavernOfDarkDoppelgangers,
                Position = 21,
                IsDefeated = false,
                IsUnlocked = true,
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
                DungeonEnemies = new List<DungeonEnemy>
                {
                    new DungeonEnemy(position: 1, @class: ClassType.Warrior, level: 210, strength: 8000, dexterity: 2000, intelligence: 2000, constitution: 36000, luck: 4000, health: 43560000, dungeonRuneBonuses: new DungeonEnemyRuneBonuses { DamageRuneType = RuneType.ColdDamage, FireResistance = 0, ColdResistance = 5, LightningResistance = 0, DamageBonus = 30 }, minWeaponDmg: 728, maxWeaponDmg: 1229 ),
                    new DungeonEnemy(position: 2, @class: ClassType.Warrior, level: 240, strength: 10965, dexterity: 1762, intelligence: 12000, constitution: 40500, luck: 5000, health: 55687500, dungeonRuneBonuses: new DungeonEnemyRuneBonuses { DamageRuneType = RuneType.LightningDamage, FireResistance = 10, ColdResistance = 10, LightningResistance = 60, DamageBonus = 30 }, minWeaponDmg: 840, maxWeaponDmg: 1439 ),
                    new DungeonEnemy(position: 3, @class: ClassType.Mage, level: 270, strength: 4000, dexterity: 4000, intelligence: 11500, constitution: 51000, luck: 7500, health: 31416000, dungeonRuneBonuses: new DungeonEnemyRuneBonuses { DamageRuneType = RuneType.FireDamage, FireResistance = 35, ColdResistance = 35, LightningResistance = 35, DamageBonus = 30 }, minWeaponDmg: 2126, maxWeaponDmg: 3643 ),
                    new DungeonEnemy(position: 4, @class: ClassType.Warrior, level: 305, strength: 15000, dexterity: 3500, intelligence: 3500, constitution: 58500, luck: 8000, health: 101351256, dungeonRuneBonuses: new DungeonEnemyRuneBonuses { DamageRuneType = RuneType.LightningDamage, FireResistance = 15, ColdResistance = 25, LightningResistance = 60, DamageBonus = 30 }, minWeaponDmg: 1067, maxWeaponDmg: 1829 ),
                    new DungeonEnemy(position: 5, @class: ClassType.Warrior, level: 330, strength: 12500, dexterity: 6000, intelligence: 6000, constitution: 73500, luck: 9000, health: 137445008, dungeonRuneBonuses: new DungeonEnemyRuneBonuses { DamageRuneType = RuneType.FireDamage, FireResistance = 60, ColdResistance = 30, LightningResistance = 30, DamageBonus = 40 }, minWeaponDmg: 1155, maxWeaponDmg: 1979 ),
                    new DungeonEnemy(position: 6, @class: ClassType.Assassin, level: 360, strength: 6500, dexterity: 18500, intelligence: 6500, constitution: 83500, luck: 11500, health: 135938000, dungeonRuneBonuses: new DungeonEnemyRuneBonuses { DamageRuneType = RuneType.ColdDamage, FireResistance = 40, ColdResistance = 50, LightningResistance = 50, DamageBonus = 45 }, minWeaponDmg: 2524, maxWeaponDmg: 4319 ),
                    new DungeonEnemy(position: 7, @class: ClassType.Berserker, level: 390, strength: 22500, dexterity: 6500, intelligence: 6500, constitution: 81500, luck: 10500, health: 143440000, dungeonRuneBonuses: new DungeonEnemyRuneBonuses { DamageRuneType = RuneType.ColdDamage, FireResistance = 50, ColdResistance = 50, LightningResistance = 10, DamageBonus = 45 }, minWeaponDmg: 1092, maxWeaponDmg: 2080, armor: 17500 ),
                    new DungeonEnemy(position: 8, @class: ClassType.Scout, level: 420, strength: 10500, dexterity: 22500, intelligence: 10500, constitution: 112500, luck: 15500, health: 212850000, dungeonRuneBonuses: new DungeonEnemyRuneBonuses { DamageRuneType = RuneType.LightningDamage, FireResistance = 50, ColdResistance = 50, LightningResistance = 50, DamageBonus = 50 }, minWeaponDmg: 1837, maxWeaponDmg: 3148 ),
                    new DungeonEnemy(position: 9, @class: ClassType.Berserker, level: 455, strength: 29500, dexterity: 7000, intelligence: 7000, constitution: 115000, luck: 16000, health: 235290000, dungeonRuneBonuses: new DungeonEnemyRuneBonuses { DamageRuneType = RuneType.LightningDamage, FireResistance = 50, ColdResistance = 50, LightningResistance = 50, DamageBonus = 55 }, minWeaponDmg: 1296, maxWeaponDmg: 2441, armor: 17500 ),
                    new DungeonEnemy(position: 10, @class: ClassType.Berserker, level: 500, strength: 38500, dexterity: 10500, intelligence: 10500, constitution: 158000, luck: 23000, health: 354552000, dungeonRuneBonuses: new DungeonEnemyRuneBonuses { DamageRuneType = RuneType.FireDamage, FireResistance = 60, ColdResistance = 50, LightningResistance = 50, DamageBonus = 60 }, minWeaponDmg: 1413, maxWeaponDmg: 2699, armor: 17500 )
                }
            },
            new Dungeon
            {
                Name = DungeonNames.AshMountain,
                Position = 25,
                IsDefeated = false,
                IsUnlocked = true,
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
                DungeonEnemies = new List<DungeonEnemy>
            {
                    new DungeonEnemy(position: 1, @class: ClassType.Mage, level: 210, strength: 2000, dexterity: 2000, intelligence: 8000, constitution: 80000, luck: 4000, health: 52800000, dungeonRuneBonuses: new DungeonEnemyRuneBonuses { DamageRuneType = RuneType.ColdDamage, FireResistance = 5, ColdResistance = 60, LightningResistance = 5, DamageBonus = 30 }, minWeaponDmg: 630, maxWeaponDmg: 1265 ),
                    new DungeonEnemy(position: 2, @class: ClassType.Warrior, level: 240, strength: 12000, dexterity: 4000, intelligence: 4000, constitution: 100000, luck: 5000, health: 187500000, dungeonRuneBonuses: new DungeonEnemyRuneBonuses { DamageRuneType = RuneType.LightningDamage, FireResistance = 60, ColdResistance = 60, LightningResistance = 60, DamageBonus = 35 }, minWeaponDmg: 320, maxWeaponDmg: 639 ),
                    new DungeonEnemy(position: 3, @class: ClassType.Scout, level: 270, strength: 6000, dexterity: 16000, intelligence: 6000, constitution: 120000, luck: 6000, health: 201600000, dungeonRuneBonuses: new DungeonEnemyRuneBonuses { DamageRuneType = RuneType.FireDamage, FireResistance = 35, ColdResistance = 35, LightningResistance = 35, DamageBonus = 35 }, minWeaponDmg: 450, maxWeaponDmg: 899 ),
                    new DungeonEnemy(position: 4, @class: ClassType.Mage, level: 305, strength: 8000, dexterity: 8000, intelligence: 20000, constitution: 140000, luck: 7000, health: 132300000, dungeonRuneBonuses: new DungeonEnemyRuneBonuses { DamageRuneType = RuneType.ColdDamage, FireResistance = 60, ColdResistance = 20, LightningResistance = 20, DamageBonus = 40 }, minWeaponDmg: 920, maxWeaponDmg: 1829 ),
                    new DungeonEnemy(position: 5, @class: ClassType.Mage, level: 330, strength: 10000, dexterity: 10000, intelligence: 24000, constitution: 160000, luck: 8000, health: 163200000, dungeonRuneBonuses: new DungeonEnemyRuneBonuses { DamageRuneType = RuneType.FireDamage, FireResistance = 25, ColdResistance = 60, LightningResistance = 25, DamageBonus = 40 }, minWeaponDmg: 1000, maxWeaponDmg: 1989 ),
                    new DungeonEnemy(position: 6, @class: ClassType.Warrior, level: 360, strength: 28000, dexterity: 12000, intelligence: 12000, constitution: 180000, luck: 10000, health: 499500000, dungeonRuneBonuses: new DungeonEnemyRuneBonuses { DamageRuneType = RuneType.LightningDamage, FireResistance = 40, ColdResistance = 40, LightningResistance = 40, DamageBonus = 45 }, minWeaponDmg: 480, maxWeaponDmg: 959 ),
                    new DungeonEnemy(position: 7, @class: ClassType.Warrior, level: 390, strength: 32000, dexterity: 14000, intelligence: 14000, constitution: 200000, luck: 11000, health: 600000000, dungeonRuneBonuses: new DungeonEnemyRuneBonuses { DamageRuneType = RuneType.FireDamage, FireResistance = 60, ColdResistance = 20, LightningResistance = 60, DamageBonus = 45 }, minWeaponDmg: 520, maxWeaponDmg: 1049 ),
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
                DungeonEnemies = new List<DungeonEnemy>
                {
                    new DungeonEnemy(position: 1, @class: ClassType.Warrior, level: 200, strength: 4194, dexterity: 1697, intelligence: 1665, constitution: 15940, luck: 2589, health: 16019700, minWeaponDmg: 268, maxWeaponDmg: 534, armorMultiplier: 1.5),
                    new DungeonEnemy(position: 2, @class: ClassType.Mage, level: 202, strength: 1714, dexterity: 1678, intelligence: 4242, constitution: 16140, luck: 2622, health: 6552840, minWeaponDmg: 610, maxWeaponDmg: 1217, armorMultiplier: 1.5),
                    new DungeonEnemy(position: 3, @class: ClassType.Scout, level: 204, strength: 1730, dexterity: 4292, intelligence: 1695, constitution: 16328, luck: 2654, health: 13388960, minWeaponDmg: 342, maxWeaponDmg: 681, armorMultiplier: 1.5),
                    new DungeonEnemy(position: 4, @class: ClassType.Warrior, level: 206, strength: 4340, dexterity: 1746, intelligence: 1715, constitution: 16512, luck: 2690, health: 17089920, minWeaponDmg: 276, maxWeaponDmg: 551, armorMultiplier: 1.5),
                    new DungeonEnemy(position: 5, @class: ClassType.Warrior, level: 208, strength: 4385, dexterity: 1763, intelligence: 1733, constitution: 16712, luck: 2726, health: 17464040, minWeaponDmg: 279, maxWeaponDmg: 556, armorMultiplier: 1.5),
                    new DungeonEnemy(position: 6, @class: ClassType.Mage, level: 210, strength: 1782, dexterity: 1747, intelligence: 4434, constitution: 16896, luck: 2757, health: 7130112, minWeaponDmg: 634, maxWeaponDmg: 1265, armorMultiplier: 1.5),
                    new DungeonEnemy(position: 7, @class: ClassType.Warrior, level: 212, strength: 4482, dexterity: 1794, intelligence: 1766, constitution: 17100, luck: 2790, health: 18211500, minWeaponDmg: 284, maxWeaponDmg: 568, armorMultiplier: 1.5),
                    new DungeonEnemy(position: 8, @class: ClassType.Mage, level: 214, strength: 1812, dexterity: 1786, intelligence: 4529, constitution: 17284, luck: 2822, health: 7432120, minWeaponDmg: 646, maxWeaponDmg: 1289, armorMultiplier: 1.5),
                    new DungeonEnemy(position: 9, @class: ClassType.Mage, level: 216, strength: 1828, dexterity: 1800, intelligence: 4578, constitution: 17484, luck: 2858, health: 7588056, minWeaponDmg: 651, maxWeaponDmg: 1300, armorMultiplier: 1.5),
                    new DungeonEnemy(position: 10, @class: ClassType.Warrior, level: 218, strength: 4627, dexterity: 1846, intelligence: 1818, constitution: 17680, luck: 2890, health: 19359600, minWeaponDmg: 292, maxWeaponDmg: 583, armorMultiplier: 1.5),
                    new DungeonEnemy(position: 11, @class: ClassType.Mage, level: 220, strength: 1861, dexterity: 1835, intelligence: 4674, constitution: 17860, luck: 2926, health: 7894120, minWeaponDmg: 663, maxWeaponDmg: 1324, armorMultiplier: 1.5),
                    new DungeonEnemy(position: 12, @class: ClassType.Mage, level: 222, strength: 1878, dexterity: 1854, intelligence: 4722, constitution: 18064, luck: 2957, health: 8056544, minWeaponDmg: 670, maxWeaponDmg: 1337, armorMultiplier: 1.5),
                    new DungeonEnemy(position: 13, @class: ClassType.Warrior, level: 224, strength: 4771, dexterity: 1898, intelligence: 1869, constitution: 18240, luck: 2991, health: 20520000, minWeaponDmg: 299, maxWeaponDmg: 600, armorMultiplier: 1.5),
                    new DungeonEnemy(position: 14, @class: ClassType.Warrior, level: 226, strength: 4820, dexterity: 1909, intelligence: 1887, constitution: 18440, luck: 3027, health: 20929400, minWeaponDmg: 303, maxWeaponDmg: 604, armorMultiplier: 1.5),
                    new DungeonEnemy(position: 15, @class: ClassType.Warrior, level: 228, strength: 4870, dexterity: 1928, intelligence: 1907, constitution: 18620, luck: 3060, health: 21319900, minWeaponDmg: 306, maxWeaponDmg: 609, armorMultiplier: 1.5),
                    new DungeonEnemy(position: 16, @class: ClassType.Mage, level: 230, strength: 1943, dexterity: 1921, intelligence: 4916, constitution: 18824, luck: 3094, health: 8696688, minWeaponDmg: 694, maxWeaponDmg: 1385, armorMultiplier: 1.5),
                    new DungeonEnemy(position: 17, @class: ClassType.Warrior, level: 232, strength: 4964, dexterity: 1962, intelligence: 1940, constitution: 19020, luck: 3126, health: 22158300, minWeaponDmg: 311, maxWeaponDmg: 620, armorMultiplier: 1.5),
                    new DungeonEnemy(position: 18, @class: ClassType.Scout, level: 234, strength: 1977, dexterity: 5012, intelligence: 1956, constitution: 19204, luck: 3160, health: 18051760, minWeaponDmg: 392, maxWeaponDmg: 783, armorMultiplier: 1.5),
                    new DungeonEnemy(position: 19, @class: ClassType.Warrior, level: 236, strength: 5059, dexterity: 1993, intelligence: 1975, constitution: 19392, luck: 3198, health: 22979520, minWeaponDmg: 316, maxWeaponDmg: 631, armorMultiplier: 1.5),
                    new DungeonEnemy(position: 20, @class: ClassType.Scout, level: 238, strength: 2009, dexterity: 5109, intelligence: 1990, constitution: 19584, luck: 3230, health: 18722304, minWeaponDmg: 399, maxWeaponDmg: 796, armorMultiplier: 1.5),
                    new DungeonEnemy(position: 21, @class: ClassType.Warrior, level: 240, strength: 5157, dexterity: 2024, intelligence: 2009, constitution: 19780, luck: 3262, health: 23834900, minWeaponDmg: 322, maxWeaponDmg: 641, armorMultiplier: 1.5),
                    new DungeonEnemy(position: 22, @class: ClassType.Warrior, level: 242, strength: 5206, dexterity: 2043, intelligence: 2028, constitution: 19960, luck: 3295, health: 24251400, minWeaponDmg: 324, maxWeaponDmg: 647, armorMultiplier: 1.5),
                    new DungeonEnemy(position: 23, @class: ClassType.Warrior, level: 244, strength: 5252, dexterity: 2058, intelligence: 2042, constitution: 20160, luck: 3330, health: 24696000, minWeaponDmg: 327, maxWeaponDmg: 652, armorMultiplier: 1.5),
                    new DungeonEnemy(position: 24, @class: ClassType.Warrior, level: 246, strength: 5302, dexterity: 2077, intelligence: 2061, constitution: 20360, luck: 3362, health: 25144600, minWeaponDmg: 330, maxWeaponDmg: 657, armorMultiplier: 1.5),
                    new DungeonEnemy(position: 25, @class: ClassType.Mage, level: 248, strength: 2090, dexterity: 2078, intelligence: 5348, constitution: 20544, luck: 3398, health: 10230912, minWeaponDmg: 747, maxWeaponDmg: 1492, armorMultiplier: 1.5),
                    new DungeonEnemy(position: 26, @class: ClassType.Scout, level: 250, strength: 2109, dexterity: 5398, intelligence: 2098, constitution: 20728, luck: 3430, health: 20810912, minWeaponDmg: 419, maxWeaponDmg: 836, armorMultiplier: 1.5),
                    new DungeonEnemy(position: 27, @class: ClassType.Scout, level: 252, strength: 2139, dexterity: 5448, intelligence: 2128, constitution: 20944, luck: 3476, health: 21195328, minWeaponDmg: 422, maxWeaponDmg: 841, armorMultiplier: 1.5),
                    new DungeonEnemy(position: 28, @class: ClassType.Warrior, level: 254, strength: 5498, dexterity: 2173, intelligence: 2162, constitution: 21160, luck: 3522, health: 26979000, minWeaponDmg: 340, maxWeaponDmg: 679, armorMultiplier: 1.5),
                    new DungeonEnemy(position: 29, @class: ClassType.Warrior, level: 256, strength: 5549, dexterity: 2207, intelligence: 2198, constitution: 21356, luck: 3567, health: 27442460, minWeaponDmg: 343, maxWeaponDmg: 684, armorMultiplier: 1.5),
                    new DungeonEnemy(position: 30, @class: ClassType.Warrior, level: 258, strength: 5596, dexterity: 2241, intelligence: 2228, constitution: 21572, luck: 3613, health: 27935740, minWeaponDmg: 346, maxWeaponDmg: 690, armorMultiplier: 1.5),
                    new DungeonEnemy(position: 31, @class: ClassType.Warrior, level: 260, strength: 5646, dexterity: 2275, intelligence: 2263, constitution: 21792, luck: 3657, health: 28438560, minWeaponDmg: 348, maxWeaponDmg: 695, armorMultiplier: 1.5),
                    new DungeonEnemy(position: 32, @class: ClassType.Mage, level: 262, strength: 2306, dexterity: 2296, intelligence: 5694, constitution: 21992, luck: 3705, health: 11567792, minWeaponDmg: 790, maxWeaponDmg: 1577, armorMultiplier: 1.5),
                    new DungeonEnemy(position: 33, @class: ClassType.Warrior, level: 264, strength: 5744, dexterity: 2340, intelligence: 2331, constitution: 22204, luck: 3751, health: 29420300, minWeaponDmg: 354, maxWeaponDmg: 705, armorMultiplier: 1.5),
                    new DungeonEnemy(position: 34, @class: ClassType.Warrior, level: 266, strength: 5796, dexterity: 2377, intelligence: 2362, constitution: 22412, luck: 3794, health: 29920020, minWeaponDmg: 356, maxWeaponDmg: 711, armorMultiplier: 1.5),
                    new DungeonEnemy(position: 35, @class: ClassType.Warrior, level: 268, strength: 5846, dexterity: 2405, intelligence: 2396, constitution: 22628, luck: 3840, health: 30434660, minWeaponDmg: 359, maxWeaponDmg: 716, armorMultiplier: 1.5),
                    new DungeonEnemy(position: 36, @class: ClassType.Scout, level: 270, strength: 2442, dexterity: 5894, intelligence: 2430, constitution: 22828, luck: 3885, health: 24745552, minWeaponDmg: 452, maxWeaponDmg: 903, armorMultiplier: 1.5),
                    new DungeonEnemy(position: 37, @class: ClassType.Scout, level: 272, strength: 2472, dexterity: 5945, intelligence: 2465, constitution: 23044, luck: 3934, health: 25164048, minWeaponDmg: 455, maxWeaponDmg: 908, armorMultiplier: 1.5),
                    new DungeonEnemy(position: 38, @class: ClassType.Warrior, level: 274, strength: 5995, dexterity: 2507, intelligence: 2498, constitution: 23264, luck: 3975, health: 31988000, minWeaponDmg: 367, maxWeaponDmg: 732, armorMultiplier: 1.5),
                    new DungeonEnemy(position: 39, @class: ClassType.Warrior, level: 276, strength: 6046, dexterity: 2538, intelligence: 2531, constitution: 23452, luck: 4022, health: 32481020, minWeaponDmg: 370, maxWeaponDmg: 737, armorMultiplier: 1.5),
                    new DungeonEnemy(position: 40, @class: ClassType.Warrior, level: 278, strength: 6092, dexterity: 2572, intelligence: 2566, constitution: 23668, luck: 4069, health: 33016860, minWeaponDmg: 372, maxWeaponDmg: 743, armorMultiplier: 1.5),
                    new DungeonEnemy(position: 41, @class: ClassType.Mage, level: 280, strength: 2609, dexterity: 2597, intelligence: 6144, constitution: 23872, luck: 4113, health: 13416064, minWeaponDmg: 843, maxWeaponDmg: 1684, armorMultiplier: 1.5),
                    new DungeonEnemy(position: 42, @class: ClassType.Mage, level: 282, strength: 2638, dexterity: 2632, intelligence: 6195, constitution: 24088, luck: 4161, health: 13633808, minWeaponDmg: 850, maxWeaponDmg: 1697, armorMultiplier: 1.5),
                    new DungeonEnemy(position: 43, @class: ClassType.Warrior, level: 284, strength: 6245, dexterity: 2671, intelligence: 2668, constitution: 24292, luck: 4203, health: 34616100, minWeaponDmg: 380, maxWeaponDmg: 759, armorMultiplier: 1.5),
                    new DungeonEnemy(position: 44, @class: ClassType.Scout, level: 286, strength: 2704, dexterity: 6293, intelligence: 2700, constitution: 24508, luck: 4252, health: 28135184, minWeaponDmg: 479, maxWeaponDmg: 956, armorMultiplier: 1.5),
                    new DungeonEnemy(position: 45, @class: ClassType.Scout, level: 288, strength: 2738, dexterity: 6346, intelligence: 2731, constitution: 24716, luck: 4294, health: 28571696, minWeaponDmg: 482, maxWeaponDmg: 961, armorMultiplier: 1.5),
                    new DungeonEnemy(position: 46, @class: ClassType.Warrior, level: 290, strength: 6396, dexterity: 2770, intelligence: 2765, constitution: 24924, luck: 4340, health: 36264420, minWeaponDmg: 388, maxWeaponDmg: 775, armorMultiplier: 1.5),
                    new DungeonEnemy(position: 47, @class: ClassType.Scout, level: 292, strength: 2806, dexterity: 6442, intelligence: 2802, constitution: 25132, luck: 4386, health: 29454704, minWeaponDmg: 488, maxWeaponDmg: 975, armorMultiplier: 1.5),
                    new DungeonEnemy(position: 48, @class: ClassType.Mage, level: 294, strength: 2841, dexterity: 2832, intelligence: 6492, constitution: 25344, luck: 4431, health: 14952960, minWeaponDmg: 886, maxWeaponDmg: 1769, armorMultiplier: 1.5),
                    new DungeonEnemy(position: 49, @class: ClassType.Mage, level: 296, strength: 2870, dexterity: 2867, intelligence: 6543, constitution: 25556, luck: 4479, health: 15180264, minWeaponDmg: 891, maxWeaponDmg: 1780, armorMultiplier: 1.5),
                    new DungeonEnemy(position: 50, @class: ClassType.Warrior, level: 298, strength: 6593, dexterity: 2905, intelligence: 2902, constitution: 25764, luck: 4523, health: 38517180, minWeaponDmg: 399, maxWeaponDmg: 796, armorMultiplier: 1.5),
                    new DungeonEnemy(position: 51, @class: ClassType.Warrior, level: 300, strength: 6640, dexterity: 2937, intelligence: 2932, constitution: 25976, luck: 4569, health: 39093880, minWeaponDmg: 402, maxWeaponDmg: 801, armorMultiplier: 1.5),
                    new DungeonEnemy(position: 52, @class: ClassType.Scout, level: 302, strength: 2974, dexterity: 6711, intelligence: 2971, constitution: 26224, luck: 4610, health: 31783488, minWeaponDmg: 506, maxWeaponDmg: 1009, armorMultiplier: 1.5),
                    new DungeonEnemy(position: 53, @class: ClassType.Warrior, level: 304, strength: 6776, dexterity: 3010, intelligence: 3013, constitution: 26464, luck: 4654, health: 40357600, minWeaponDmg: 407, maxWeaponDmg: 812, armorMultiplier: 1.5),
                    new DungeonEnemy(position: 54, @class: ClassType.Mage, level: 306, strength: 3048, dexterity: 3053, intelligence: 6840, constitution: 26728, luck: 4697, health: 16410992, minWeaponDmg: 922, maxWeaponDmg: 1841, armorMultiplier: 1.5),
                    new DungeonEnemy(position: 55, @class: ClassType.Warrior, level: 308, strength: 6906, dexterity: 3089, intelligence: 3091, constitution: 26964, luck: 4741, health: 41659380, minWeaponDmg: 412, maxWeaponDmg: 823, armorMultiplier: 1.5),
                    new DungeonEnemy(position: 56, @class: ClassType.Warrior, level: 310, strength: 6973, dexterity: 3121, intelligence: 3132, constitution: 27220, luck: 4784, health: 42327100, minWeaponDmg: 415, maxWeaponDmg: 828, armorMultiplier: 1.5),
                    new DungeonEnemy(position: 57, @class: ClassType.Warrior, level: 312, strength: 7040, dexterity: 3160, intelligence: 3173, constitution: 27456, luck: 4828, health: 42968640, minWeaponDmg: 418, maxWeaponDmg: 833, armorMultiplier: 1.5),
                    new DungeonEnemy(position: 58, @class: ClassType.Scout, level: 314, strength: 3196, dexterity: 7105, intelligence: 3212, constitution: 27708, luck: 4875, health: 34912080, minWeaponDmg: 526, maxWeaponDmg: 1049, armorMultiplier: 1.5),
                    new DungeonEnemy(position: 59, @class: ClassType.Mage, level: 316, strength: 3236, dexterity: 3250, intelligence: 7173, constitution: 27948, luck: 4914, health: 17719032, minWeaponDmg: 951, maxWeaponDmg: 1900, armorMultiplier: 1.5),
                    new DungeonEnemy(position: 60, @class: ClassType.Warrior, level: 318, strength: 7240, dexterity: 3270, intelligence: 3291, constitution: 28196, luck: 4958, health: 44972620, minWeaponDmg: 426, maxWeaponDmg: 849, armorMultiplier: 1.5),
                    new DungeonEnemy(position: 61, @class: ClassType.Warrior, level: 320, strength: 7303, dexterity: 3309, intelligence: 3331, constitution: 28448, luck: 5005, health: 45659040, minWeaponDmg: 428, maxWeaponDmg: 855, armorMultiplier: 1.5),
                    new DungeonEnemy(position: 62, @class: ClassType.Warrior, level: 322, strength: 7370, dexterity: 3348, intelligence: 3368, constitution: 28692, luck: 5043, health: 46337580, minWeaponDmg: 431, maxWeaponDmg: 860, armorMultiplier: 1.5),
                    new DungeonEnemy(position: 63, @class: ClassType.Warrior, level: 324, strength: 7436, dexterity: 3382, intelligence: 3409, constitution: 28944, luck: 5088, health: 47034000, minWeaponDmg: 434, maxWeaponDmg: 865, armorMultiplier: 1.5),
                    new DungeonEnemy(position: 64, @class: ClassType.Scout, level: 326, strength: 3422, dexterity: 7501, intelligence: 3448, constitution: 29184, luck: 5132, health: 38172672, minWeaponDmg: 546, maxWeaponDmg: 1089, armorMultiplier: 1.5),
                    new DungeonEnemy(position: 65, @class: ClassType.Warrior, level: 328, strength: 7567, dexterity: 3458, intelligence: 3487, constitution: 29436, luck: 5177, health: 48422220, minWeaponDmg: 439, maxWeaponDmg: 876, armorMultiplier: 1.5),
                    new DungeonEnemy(position: 66, @class: ClassType.Warrior, level: 330, strength: 7634, dexterity: 3495, intelligence: 3528, constitution: 29696, luck: 5217, health: 49146880, minWeaponDmg: 442, maxWeaponDmg: 881, armorMultiplier: 1.5),
                    new DungeonEnemy(position: 67, @class: ClassType.Scout, level: 332, strength: 3532, dexterity: 7700, intelligence: 3567, constitution: 29936, luck: 5262, health: 39874752, minWeaponDmg: 555, maxWeaponDmg: 1108, armorMultiplier: 1.5),
                    new DungeonEnemy(position: 68, @class: ClassType.Mage, level: 334, strength: 3568, dexterity: 3609, intelligence: 7768, constitution: 30188, luck: 5305, health: 20225960, minWeaponDmg: 1006, maxWeaponDmg: 2009, armorMultiplier: 1.5),
                    new DungeonEnemy(position: 69, @class: ClassType.Warrior, level: 336, strength: 7833, dexterity: 3609, intelligence: 3645, constitution: 30424, luck: 5347, health: 51264440, minWeaponDmg: 450, maxWeaponDmg: 897, armorMultiplier: 1.5),
                    new DungeonEnemy(position: 70, @class: ClassType.Warrior, level: 338, strength: 7900, dexterity: 3641, intelligence: 3687, constitution: 30676, luck: 5392, health: 51995820, minWeaponDmg: 452, maxWeaponDmg: 903, armorMultiplier: 1.5),
                    new DungeonEnemy(position: 71, @class: ClassType.Warrior, level: 340, strength: 7967, dexterity: 3680, intelligence: 3728, constitution: 30912, luck: 5436, health: 52704960, minWeaponDmg: 455, maxWeaponDmg: 908, armorMultiplier: 1.5),
                    new DungeonEnemy(position: 72, @class: ClassType.Warrior, level: 342, strength: 8031, dexterity: 3717, intelligence: 3764, constitution: 31168, luck: 5480, health: 53453120, minWeaponDmg: 458, maxWeaponDmg: 913, armorMultiplier: 1.5),
                    new DungeonEnemy(position: 73, @class: ClassType.Mage, level: 344, strength: 3756, dexterity: 3805, intelligence: 8100, constitution: 31408, luck: 5522, health: 21671520, minWeaponDmg: 1035, maxWeaponDmg: 2068, armorMultiplier: 1.5),
                    new DungeonEnemy(position: 74, @class: ClassType.Warrior, level: 346, strength: 8167, dexterity: 3790, intelligence: 3844, constitution: 31656, luck: 5566, health: 54923160, minWeaponDmg: 462, maxWeaponDmg: 924, armorMultiplier: 1.5),
                    new DungeonEnemy(position: 75, @class: ClassType.Warrior, level: 348, strength: 8229, dexterity: 3829, intelligence: 3886, constitution: 31908, luck: 5611, health: 55679460, minWeaponDmg: 466, maxWeaponDmg: 929, armorMultiplier: 1.5),
                    new DungeonEnemy(position: 76, @class: ClassType.Warrior, level: 350, strength: 8297, dexterity: 3868, intelligence: 3923, constitution: 32152, luck: 5651, health: 56426760, minWeaponDmg: 468, maxWeaponDmg: 935, armorMultiplier: 1.5),
                    new DungeonEnemy(position: 77, @class: ClassType.Warrior, level: 352, strength: 8541, dexterity: 3976, intelligence: 4007, constitution: 33072, luck: 5767, health: 58372080, minWeaponDmg: 471, maxWeaponDmg: 940, armorMultiplier: 1.5),
                    new DungeonEnemy(position: 78, @class: ClassType.Warrior, level: 354, strength: 8787, dexterity: 4088, intelligence: 4093, constitution: 33976, luck: 5881, health: 60307400, minWeaponDmg: 474, maxWeaponDmg: 945, armorMultiplier: 1.5),
                    new DungeonEnemy(position: 79, @class: ClassType.Scout, level: 356, strength: 4199, dexterity: 9029, intelligence: 4175, constitution: 34896, luck: 5997, health: 49831488, minWeaponDmg: 595, maxWeaponDmg: 1188, armorMultiplier: 1.5),
                    new DungeonEnemy(position: 80, @class: ClassType.Warrior, level: 358, strength: 9274, dexterity: 4313, intelligence: 4256, constitution: 35824, luck: 6107, health: 64304080, minWeaponDmg: 479, maxWeaponDmg: 956, armorMultiplier: 1.5),
                    new DungeonEnemy(position: 81, @class: ClassType.Warrior, level: 360, strength: 9996, dexterity: 4642, intelligence: 4556, constitution: 38556, luck: 6534, health: 69593584, minWeaponDmg: 482, maxWeaponDmg: 961, armorMultiplier: 1.5),
                    new DungeonEnemy(position: 82, @class: ClassType.Warrior, level: 362, strength: 10761, dexterity: 4999, intelligence: 4877, constitution: 41494, luck: 6989, health: 75311608, minWeaponDmg: 484, maxWeaponDmg: 967, armorMultiplier: 1.5),
                    new DungeonEnemy(position: 83, @class: ClassType.Warrior, level: 364, strength: 11583, dexterity: 5380, intelligence: 5213, constitution: 44629, luck: 7466, health: 81447928, minWeaponDmg: 487, maxWeaponDmg: 972, armorMultiplier: 1.5),
                    new DungeonEnemy(position: 84, @class: ClassType.Warrior, level: 366, strength: 12460, dexterity: 5781, intelligence: 5578, constitution: 47979, luck: 7980, health: 88041464, minWeaponDmg: 490, maxWeaponDmg: 977, armorMultiplier: 1.5),
                    new DungeonEnemy(position: 85, @class: ClassType.Warrior, level: 368, strength: 13397, dexterity: 6214, intelligence: 5967, constitution: 51532, luck: 8525, health: 95076544, minWeaponDmg: 492, maxWeaponDmg: 983, armorMultiplier: 1.5),
                    new DungeonEnemy(position: 86, @class: ClassType.Mage, level: 370, strength: 6418, dexterity: 6128, intelligence: 13843, constitution: 53237, luck: 8757, health: 39501856, minWeaponDmg: 1114, maxWeaponDmg: 2225, armorMultiplier: 1.5),
                    new DungeonEnemy(position: 87, @class: ClassType.Warrior, level: 372, strength: 14300, dexterity: 6631, intelligence: 6299, constitution: 54963, luck: 8990, health: 102505992, minWeaponDmg: 498, maxWeaponDmg: 993, armorMultiplier: 1.5),
                    new DungeonEnemy(position: 88, @class: ClassType.Warrior, level: 374, strength: 14765, dexterity: 6840, intelligence: 6470, constitution: 56701, luck: 9233, health: 106314376, minWeaponDmg: 500, maxWeaponDmg: 999, armorMultiplier: 1.5),
                    new DungeonEnemy(position: 89, @class: ClassType.Warrior, level: 376, strength: 15233, dexterity: 7059, intelligence: 6649, constitution: 58485, luck: 9478, health: 110244224, minWeaponDmg: 503, maxWeaponDmg: 1004, armorMultiplier: 1.5),
                    new DungeonEnemy(position: 90, @class: ClassType.Warrior, level: 378, strength: 15716, dexterity: 7280, intelligence: 6822, constitution: 60298, luck: 9716, health: 114264712, minWeaponDmg: 506, maxWeaponDmg: 1009, armorMultiplier: 1.5),
                    new DungeonEnemy(position: 91, @class: ClassType.Warrior, level: 380, strength: 16203, dexterity: 7500, intelligence: 7006, constitution: 62140, luck: 9973, health: 118376704, minWeaponDmg: 508, maxWeaponDmg: 1015, armorMultiplier: 1.5),
                    new DungeonEnemy(position: 92, @class: ClassType.Warrior, level: 382, strength: 16700, dexterity: 7728, intelligence: 7191, constitution: 64001, luck: 10226, health: 122561912, minWeaponDmg: 511, maxWeaponDmg: 1020, armorMultiplier: 1.5),
                    new DungeonEnemy(position: 93, @class: ClassType.Warrior, level: 384, strength: 17199, dexterity: 7960, intelligence: 7373, constitution: 65912, luck: 10492, health: 126880600, minWeaponDmg: 514, maxWeaponDmg: 1025, armorMultiplier: 1.5),
                    new DungeonEnemy(position: 94, @class: ClassType.Warrior, level: 386, strength: 17717, dexterity: 8198, intelligence: 7566, constitution: 67861, luck: 10748, health: 131311032, minWeaponDmg: 516, maxWeaponDmg: 1031, armorMultiplier: 1.5),
                    new DungeonEnemy(position: 95, @class: ClassType.Warrior, level: 388, strength: 18240, dexterity: 8432, intelligence: 7757, constitution: 69809, luck: 11021, health: 135778512, minWeaponDmg: 519, maxWeaponDmg: 1036, armorMultiplier: 1.5),
                    new DungeonEnemy(position: 96, @class: ClassType.Warrior, level: 390, strength: 18766, dexterity: 8678, intelligence: 7954, constitution: 71816, luck: 11296, health: 140400288, minWeaponDmg: 522, maxWeaponDmg: 1041, armorMultiplier: 1.5),
                    new DungeonEnemy(position: 97, @class: ClassType.Warrior, level: 392, strength: 19306, dexterity: 8927, intelligence: 8151, constitution: 73848, luck: 11569, health: 145111328, minWeaponDmg: 524, maxWeaponDmg: 1047, armorMultiplier: 1.5),
                    new DungeonEnemy(position: 98, @class: ClassType.Warrior, level: 394, strength: 19856, dexterity: 9175, intelligence: 8354, constitution: 75919, luck: 11850, health: 149940032, minWeaponDmg: 527, maxWeaponDmg: 1052, armorMultiplier: 1.5),
                    new DungeonEnemy(position: 99, @class: ClassType.Warrior, level: 396, strength: 20412, dexterity: 9432, intelligence: 8563, constitution: 78007, luck: 12138, health: 154843888, minWeaponDmg: 530, maxWeaponDmg: 1057, armorMultiplier: 1.5),
                    new DungeonEnemy(position: 100, @class: ClassType.Warrior, level: 398, strength: 20977, dexterity: 9686, intelligence: 8768, constitution: 80151, luck: 12429, health: 159901248, minWeaponDmg: 532, maxWeaponDmg: 1063, armorMultiplier: 1.5),
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
                DungeonEnemies = new List<DungeonEnemy>
                {
                    new DungeonEnemy(position: 486,  @class: ClassType.Warrior, level: 385, strength: 56327, dexterity: 19741, intelligence: 19625, constitution: 76759, luck: 12247, health: 148144864, minWeaponDmg: 426, maxWeaponDmg: 808 ),
                    new DungeonEnemy(position: 658, @class: ClassType.Warrior, level: 428, strength: 59957, dexterity: 21023, intelligence: 20859, constitution: 81160, luck: 13318, health: 174088192, minWeaponDmg: 469, maxWeaponDmg: 943 ),
                    new DungeonEnemy(position: 711, @class: ClassType.Warrior, level: 441, strength: 67005, dexterity: 23491, intelligence: 23314, constitution: 90796, luck: 14839, health: 200659168, minWeaponDmg: 483, maxWeaponDmg: 990 ),
                    new DungeonEnemy(position: 777, @class: ClassType.Warrior, level: 456, strength: 76022, dexterity: 26651, intelligence: 26457, constitution: 103124, luck: 16775, health: 235638336, minWeaponDmg: 503, maxWeaponDmg: 1044 ),
                    new DungeonEnemy(position: 830,  @class: ClassType.Warrior, level: 467, strength: 83398, dexterity: 29241, intelligence: 29033, constitution: 113216, luck: 18360, health: 264925440, minWeaponDmg: 519, maxWeaponDmg: 1082 ),
                    new DungeonEnemy(position: 867, @class: ClassType.Warrior, level: 474, strength: 88573, dexterity: 31056, intelligence: 30839, constitution: 120303, luck: 19464, health: 285719616, minWeaponDmg: 528, maxWeaponDmg: 1108 ),
                    new DungeonEnemy(position: 868, @class: ClassType.Warrior, level: 474, strength: 88675, dexterity: 31092, intelligence: 30875, constitution: 120441, luck: 19487, health: 286047360, minWeaponDmg: 524, maxWeaponDmg: 1108 ),
                    new DungeonEnemy(position: 869, @class: ClassType.Warrior, level: 474, strength: 88777, dexterity: 31128, intelligence: 30910, constitution: 120580, luck: 19509, health: 286377504, minWeaponDmg: 525, maxWeaponDmg: 1108 ),
                    new DungeonEnemy(position: 870, @class: ClassType.Warrior, level: 475, strength: 89088, dexterity: 31233, intelligence: 31015, constitution: 121011, luck: 19575, health: 288006176, minWeaponDmg: 525, maxWeaponDmg: 1110 ),
                    new DungeonEnemy(position: 871, @class: ClassType.Warrior, level: 475, strength: 89190, dexterity: 31269, intelligence: 31051, constitution: 121150, luck: 19598, health: 288336992, minWeaponDmg: 527, maxWeaponDmg: 1110 ),
                    new DungeonEnemy(position: 872, @class: ClassType.Warrior, level: 475, strength: 89293, dexterity: 31305, intelligence: 31087, constitution: 121290, luck: 19620, health: 288670208, minWeaponDmg: 528, maxWeaponDmg: 1111 ),
                    new DungeonEnemy(position: 873, @class: ClassType.Warrior, level: 475, strength: 89395, dexterity: 31341, intelligence: 31122, constitution: 121428, luck: 19642, health: 288998656, minWeaponDmg: 525, maxWeaponDmg: 1110 ),
                    new DungeonEnemy(position: 874, @class: ClassType.Warrior, level: 475, strength: 89498, dexterity: 31377, intelligence: 31158, constitution: 121568, luck: 19665, health: 289331840, minWeaponDmg: 526, maxWeaponDmg: 1111 ),
                    new DungeonEnemy(position: 875, @class: ClassType.Warrior, level: 476, strength: 89810, dexterity: 31482, intelligence: 31264, constitution: 122001, luck: 19731, health: 290972384, minWeaponDmg: 525, maxWeaponDmg: 1113 ),
                    new DungeonEnemy(position: 876, @class: ClassType.Warrior, level: 476, strength: 89913, dexterity: 31518, intelligence: 31299, constitution: 122140, luck: 19754, health: 291303904, minWeaponDmg: 526, maxWeaponDmg: 1112 ),
                    new DungeonEnemy(position: 877, @class: ClassType.Warrior, level: 476, strength: 90015, dexterity: 31554, intelligence: 31335, constitution: 122280, luck: 19776, health: 291637792, minWeaponDmg: 527, maxWeaponDmg: 1114 ),
                    new DungeonEnemy(position: 939, @class: ClassType.Warrior, level: 488, strength: 99083, dexterity: 34734, intelligence: 34499, constitution: 134700, luck: 21710, health: 329341504, minWeaponDmg: 544, maxWeaponDmg: 1157 ),
                    new DungeonEnemy(position: 947, @class: ClassType.Warrior, level: 490, strength: 100382, dexterity: 35191, intelligence: 34954, constitution: 136484, luck: 21989, health: 335068224, minWeaponDmg: 544, maxWeaponDmg: 1162 ),
                    new DungeonEnemy(position: 954,  @class: ClassType.Warrior, level: 491, strength: 101353, dexterity: 35527, intelligence: 35288, constitution: 137814, luck: 22200, health: 339022432, minWeaponDmg: 546, maxWeaponDmg: 1163 ),
                    new DungeonEnemy(position: 962, @class: ClassType.Warrior, level: 493, strength: 102665, dexterity: 35988, intelligence: 35748, constitution: 139606, luck: 22472, health: 344826816, minWeaponDmg: 546, maxWeaponDmg: 1174 ),
                    new DungeonEnemy(position: 981, @class: ClassType.Warrior, level: 497, strength: 105634, dexterity: 37033, intelligence: 36788, constitution: 143682, luck: 23112, health: 357768192, minWeaponDmg: 554, maxWeaponDmg: 1189 ),
                    new DungeonEnemy(position: 990, @class: ClassType.Warrior, level: 499, strength: 107078, dexterity: 37541, intelligence: 37293, constitution: 145665, luck: 23414, health: 364162496, minWeaponDmg: 555, maxWeaponDmg: 1197 ),
                    new DungeonEnemy(position: 998, @class: ClassType.Warrior, level: 500, strength: 108183, dexterity: 37924, intelligence: 37674, constitution: 147177, luck: 23653, health: 368678400, minWeaponDmg: 556, maxWeaponDmg: 1201 ),
                    new DungeonEnemy(position: 999,  @class: ClassType.Warrior, level: 500, strength: 108292, dexterity: 37962, intelligence: 37712, constitution: 147326, luck: 23676, health: 369051616, minWeaponDmg: 555, maxWeaponDmg: 1201 ),
                    new DungeonEnemy(position: 1000, @class: ClassType.Scout, level: 501, strength: 34075, dexterity: 135800, intelligence: 33950, constitution: 138915, luck: 48412, health: 278941312, minWeaponDmg: 872, maxWeaponDmg: 1334 )
                }
            },

            #endregion

            #region Shadow World

            new Dungeon
            {
                Name = DungeonNames.DesecratedCatacombs,
                Position = 101,
                IsDefeated = false,
                IsUnlocked = true,
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
                    new DungeonEnemy(position: 9, @class: ClassType.Mage, level: 348, strength: 1648, dexterity: 1685, intelligence: 6813, constitution: 33038, luck: 2454, health: 23060524, minWeaponDmg: 8377, maxWeaponDmg: 16743, armor: 1932 )
                }
            },
            new Dungeon
            {
                Name = DungeonNames.BlackSkullFortress,
                Position = 110,
                IsDefeated = false,
                IsUnlocked = true,
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
                Type = DungeonTypeEnum.Shadow,
                DungeonEnemies = new List<DungeonEnemy>
                {
                    new DungeonEnemy(position: 1, @class: ClassType.Warrior, level: 372, strength: 7206, dexterity: 2531, intelligence: 2494, constitution: 37389, luck: 1736, health: 69730488, minWeaponDmg: 3980, maxWeaponDmg: 7945 ),
                    new DungeonEnemy(position: 2, @class: ClassType.Scout, level: 374, strength: 1826, dexterity: 7249, intelligence: 1812, constitution: 37658, luck: 2639, health: 56487000, minWeaponDmg: 5024, maxWeaponDmg: 9980 ),
                    new DungeonEnemy(position: 3, @class: ClassType.Warrior, level: 376, strength: 7292, dexterity: 2561, intelligence: 2525, constitution: 37922, luck: 1745, health: 71482968, minWeaponDmg: 4082, maxWeaponDmg: 7966 ),
                    new DungeonEnemy(position: 4, @class: ClassType.Warrior, level: 378, strength: 7336, dexterity: 2576, intelligence: 2541, constitution: 38192, luck: 1750, health: 72373840, minWeaponDmg: 4055, maxWeaponDmg: 8051 ),
                    new DungeonEnemy(position: 5, @class: ClassType.Mage, level: 380, strength: 1830, dexterity: 1858, intelligence: 7489, constitution: 37846, luck: 2673, health: 28838652, minWeaponDmg: 9157, maxWeaponDmg: 17530 ),
                    new DungeonEnemy(position: 6, @class: ClassType.Scout, level: 382, strength: 1869, dexterity: 7421, intelligence: 1855, constitution: 38714, luck: 2694, health: 59309848, minWeaponDmg: 5163, maxWeaponDmg: 9965 ),
                    new DungeonEnemy(position: 7, @class: ClassType.Warrior, level: 384, strength: 7464, dexterity: 2620, intelligence: 2586, constitution: 38978, luck: 1765, health: 75032648, minWeaponDmg: 4115, maxWeaponDmg: 8191 ),
                    new DungeonEnemy(position: 8, @class: ClassType.Scout, level: 386, strength: 1890, dexterity: 7507, intelligence: 1876, constitution: 39237, luck: 2721, health: 60738876, minWeaponDmg: 5162, maxWeaponDmg: 10319 ),
                    new DungeonEnemy(position: 9, @class: ClassType.Mage, level: 388, strength: 1874, dexterity: 1900, intelligence: 7654, constitution: 38918, luck: 2729, health: 30278204, minWeaponDmg: 9868, maxWeaponDmg: 18597 ),
                    new DungeonEnemy(position: 10, @class: ClassType.Warrior, level: 390, strength: 7592, dexterity: 2665, intelligence: 2632, constitution: 39754, luck: 1781, health: 77719072, minWeaponDmg: 4180, maxWeaponDmg: 8336 )
                }
            },
            new Dungeon
            {
                Name = DungeonNames.Hell,
                Position = 112,
                IsDefeated = false,
                IsUnlocked = true,
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
                Type = DungeonTypeEnum.Shadow,
                DungeonEnemies = new List<DungeonEnemy>
                {
                    new DungeonEnemy(position: 1, @class: ClassType.Scout, level: 320, strength: 9700, dexterity: 19400, intelligence: 9700, constitution: 65500, luck: 9700, health: 181000000, minWeaponDmg: 981, maxWeaponDmg: 1226, dungeonRuneBonuses: new DungeonEnemyRuneBonuses { DamageRuneType = RuneType.FireDamage, FireResistance = 50, ColdResistance = 50, LightningResistance = 50, DamageBonus = 50 }, armor: 19750 ),
                    new DungeonEnemy(position: 2, @class: ClassType.Warrior, level: 323, strength: 19760, dexterity: 9880, intelligence: 9880, constitution: 67000, luck: 9880, health: 185500000, minWeaponDmg: 994, maxWeaponDmg: 1242, dungeonRuneBonuses: new DungeonEnemyRuneBonuses { DamageRuneType = RuneType.LightningDamage, FireResistance = 50, ColdResistance = 50, LightningResistance = 50, DamageBonus = 50 }, armor: 20000 ),
                    new DungeonEnemy(position: 3, @class: ClassType.Mage, level: 326, strength: 10060, dexterity: 10060, intelligence: 20120, constitution: 68500, luck: 10060, health: 190000000, minWeaponDmg: 1005, maxWeaponDmg: 1252, dungeonRuneBonuses: new DungeonEnemyRuneBonuses { DamageRuneType = RuneType.LightningDamage, FireResistance = 50, ColdResistance = 50, LightningResistance = 50, DamageBonus = 50 }, armor: 20250 ),
                    new DungeonEnemy(position: 4, @class: ClassType.Scout, level: 329, strength: 10240, dexterity: 20480, intelligence: 10240, constitution: 70000, luck: 10240, health: 194500000, minWeaponDmg: 1019, maxWeaponDmg: 1265, dungeonRuneBonuses: new DungeonEnemyRuneBonuses { DamageRuneType = RuneType.ColdDamage, FireResistance = 50, ColdResistance = 50, LightningResistance = 50, DamageBonus = 50 }, armor: 20500 ),
                    new DungeonEnemy(position: 5, @class: ClassType.Scout, level: 332, strength: 10420, dexterity: 20840, intelligence: 10420, constitution: 71500, luck: 10420, health: 199000000, minWeaponDmg: 1031, maxWeaponDmg: 1277, dungeonRuneBonuses: new DungeonEnemyRuneBonuses { DamageRuneType = RuneType.FireDamage, FireResistance = 50, ColdResistance = 50, LightningResistance = 50, DamageBonus = 50 }, armor: 20750 ),
                    new DungeonEnemy(position: 6, @class: ClassType.Scout, level: 335, strength: 10600, dexterity: 21200, intelligence: 10600, constitution: 73000, luck: 10600, health: 203500000, minWeaponDmg: 1045, maxWeaponDmg: 1284, dungeonRuneBonuses: new DungeonEnemyRuneBonuses { DamageRuneType = RuneType.LightningDamage, FireResistance = 50, ColdResistance = 50, LightningResistance = 50, DamageBonus = 50 }, armor: 21000 ),
                    new DungeonEnemy(position: 7, @class: ClassType.Warrior, level: 338, strength: 21560, dexterity: 10780, intelligence: 10780, constitution: 74500, luck: 10780, health: 208000000, minWeaponDmg: 1053, maxWeaponDmg: 1301, dungeonRuneBonuses: new DungeonEnemyRuneBonuses { DamageRuneType = RuneType.LightningDamage, FireResistance = 50, ColdResistance = 50, LightningResistance = 50, DamageBonus = 50 }, armor: 21250 ),
                    new DungeonEnemy(position: 8, @class: ClassType.Warrior, level: 341, strength: 21920, dexterity: 10960, intelligence: 10960, constitution: 76000, luck: 10960, health: 212500000, minWeaponDmg: 1064, maxWeaponDmg: 1308, dungeonRuneBonuses: new DungeonEnemyRuneBonuses { DamageRuneType = RuneType.FireDamage, FireResistance = 50, ColdResistance = 50, LightningResistance = 50, DamageBonus = 50 }, armor: 21500 ),
                    new DungeonEnemy(position: 9, @class: ClassType.Scout, level: 344, strength: 11140, dexterity: 22280, intelligence: 11140, constitution: 77500, luck: 11140, health: 217000000, minWeaponDmg: 1084, maxWeaponDmg: 1324, dungeonRuneBonuses: new DungeonEnemyRuneBonuses { DamageRuneType = RuneType.ColdDamage, FireResistance = 50, ColdResistance = 50, LightningResistance = 50, DamageBonus = 50 }, armor: 21750 ),
                    new DungeonEnemy(position: 10, @class: ClassType.Mage, level: 350, strength: 11500, dexterity: 11500, intelligence: 23000, constitution: 80500, luck: 11500, health: 226000000, minWeaponDmg: 1100, maxWeaponDmg: 1350, dungeonRuneBonuses: new DungeonEnemyRuneBonuses { DamageRuneType = RuneType.ColdDamage, FireResistance = 50, ColdResistance = 50, LightningResistance = 50, DamageBonus = 50 }, armor: 22000 )
                }
            },
            new Dungeon
            {
                Name = DungeonNames.HouseOfHorrors,
                Position = 114,
                IsDefeated = false,
                IsUnlocked = true,
                Type = DungeonTypeEnum.Shadow,
                DungeonEnemies = new List<DungeonEnemy>
                {
                    new DungeonEnemy(position: 1, @class: ClassType.Mage, level: 350, strength: 11500, dexterity: 11500, intelligence: 23000, constitution: 80500, luck: 11500, health: 226000000, minWeaponDmg: 1102, maxWeaponDmg: 1349, dungeonRuneBonuses: new DungeonEnemyRuneBonuses { DamageRuneType = RuneType.ColdDamage, FireResistance = 50, ColdResistance = 50, LightningResistance = 50, DamageBonus = 50 }, armor: 22250 ),
                    new DungeonEnemy(position: 2, @class: ClassType.Warrior, level: 354, strength: 23480, dexterity: 11740, intelligence: 11740, constitution: 82500, luck: 11740, health: 232000000, minWeaponDmg: 1116, maxWeaponDmg: 1363, dungeonRuneBonuses: new DungeonEnemyRuneBonuses { DamageRuneType = RuneType.LightningDamage, FireResistance = 50, ColdResistance = 50, LightningResistance = 50, DamageBonus = 50 }, armor: 22500 ),
                    new DungeonEnemy(position: 3, @class: ClassType.Warrior, level: 358, strength: 23960, dexterity: 11980, intelligence: 11980, constitution: 84500, luck: 11980, health: 238000000, minWeaponDmg: 1140, maxWeaponDmg: 1380, dungeonRuneBonuses: new DungeonEnemyRuneBonuses { DamageRuneType = RuneType.LightningDamage, FireResistance = 50, ColdResistance = 50, LightningResistance = 50, DamageBonus = 50 }, armor: 22750 ),
                    new DungeonEnemy(position: 4, @class: ClassType.Warrior, level: 362, strength: 24440, dexterity: 12220, intelligence: 12220, constitution: 86500, luck: 12220, health: 244000000, minWeaponDmg: 1151, maxWeaponDmg: 1397, dungeonRuneBonuses: new DungeonEnemyRuneBonuses { DamageRuneType = RuneType.FireDamage, FireResistance = 50, ColdResistance = 50, LightningResistance = 50, DamageBonus = 50 }, armor: 23000 ),
                    new DungeonEnemy(position: 5, @class: ClassType.Scout, level: 366, strength: 12460, dexterity: 24920, intelligence: 12460, constitution: 88500, luck: 12460, health: 250000000, minWeaponDmg: 1167, maxWeaponDmg: 1404, dungeonRuneBonuses: new DungeonEnemyRuneBonuses { DamageRuneType = RuneType.ColdDamage, FireResistance = 50, ColdResistance = 50, LightningResistance = 50, DamageBonus = 50 }, armor: 23250 ),
                    new DungeonEnemy(position: 6, @class: ClassType.Mage, level: 370, strength: 12700, dexterity: 12700, intelligence: 25400, constitution: 90500, luck: 12700, health: 256000000, minWeaponDmg: 1181, maxWeaponDmg: 1429, dungeonRuneBonuses: new DungeonEnemyRuneBonuses { DamageRuneType = RuneType.FireDamage, FireResistance = 50, ColdResistance = 50, LightningResistance = 50, DamageBonus = 50 }, armor: 23500 ),
                    new DungeonEnemy(position: 7, @class: ClassType.Scout, level: 374, strength: 12940, dexterity: 25880, intelligence: 12940, constitution: 92500, luck: 12940, health: 262000000, minWeaponDmg: 1199, maxWeaponDmg: 1441, dungeonRuneBonuses: new DungeonEnemyRuneBonuses { DamageRuneType = RuneType.LightningDamage, FireResistance = 50, ColdResistance = 50, LightningResistance = 50, DamageBonus = 50 }, armor: 23750 ),
                    new DungeonEnemy(position: 8, @class: ClassType.Mage, level: 378, strength: 13180, dexterity: 13180, intelligence: 26360, constitution: 94500, luck: 13180, health: 268000000, minWeaponDmg: 1212, maxWeaponDmg: 1461, dungeonRuneBonuses: new DungeonEnemyRuneBonuses { DamageRuneType = RuneType.FireDamage, FireResistance = 50, ColdResistance = 50, LightningResistance = 50, DamageBonus = 50 }, armor: 24000 ),
                    new DungeonEnemy(position: 9, @class: ClassType.Mage, level: 382, strength: 13420, dexterity: 13420, intelligence: 26840, constitution: 96500, luck: 13420, health: 274000000, minWeaponDmg: 1230, maxWeaponDmg: 1474, dungeonRuneBonuses: new DungeonEnemyRuneBonuses { DamageRuneType = RuneType.FireDamage, FireResistance = 50, ColdResistance = 50, LightningResistance = 50, DamageBonus = 50 }, armor: 24250 ),
                    new DungeonEnemy(position: 10, @class: ClassType.Scout, level: 390, strength: 13900, dexterity: 27800, intelligence: 13900, constitution: 100500, luck: 13900, health: 286000000, minWeaponDmg: 1276, maxWeaponDmg: 1510, dungeonRuneBonuses: new DungeonEnemyRuneBonuses { DamageRuneType = RuneType.ColdDamage, FireResistance = 50, ColdResistance = 50, LightningResistance = 50, DamageBonus = 50 }, armor: 24500 )
                }
            },
            new Dungeon
            {
                Name = DungeonNames.ThirteenthFloor,
                Position = 115,
                IsDefeated = false,
                IsUnlocked = true,
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
                Type = DungeonTypeEnum.Shadow,
                DungeonEnemies = new List<DungeonEnemy>
                {
                    new DungeonEnemy(position: 1, @class: ClassType.Mage, level: 370, strength: 12700, dexterity: 12700, intelligence: 25400, constitution: 90500, luck: 12700, health: 256000000, minWeaponDmg: 1181, maxWeaponDmg: 1427, dungeonRuneBonuses: new DungeonEnemyRuneBonuses { DamageRuneType = RuneType.LightningDamage, FireResistance = 50, ColdResistance = 50, LightningResistance = 50, DamageBonus = 50 }, armor: 23250 ),
                    new DungeonEnemy(position: 2, @class: ClassType.Warrior, level: 376, strength: 26120, dexterity: 13060, intelligence: 13060, constitution: 93500, luck: 13060, health: 265000000, minWeaponDmg: 1212, maxWeaponDmg: 1451, dungeonRuneBonuses: new DungeonEnemyRuneBonuses { DamageRuneType = RuneType.LightningDamage, FireResistance = 50, ColdResistance = 50, LightningResistance = 50, DamageBonus = 50 }, armor: 24000 ),
                    new DungeonEnemy(position: 3, @class: ClassType.Scout, level: 382, strength: 13420, dexterity: 26840, intelligence: 13420, constitution: 96500, luck: 13420, health: 274000000, minWeaponDmg: 1230, maxWeaponDmg: 1471, dungeonRuneBonuses: new DungeonEnemyRuneBonuses { DamageRuneType = RuneType.ColdDamage, FireResistance = 50, ColdResistance = 50, LightningResistance = 50, DamageBonus = 50 }, armor: 24750 ),
                    new DungeonEnemy(position: 4, @class: ClassType.Warrior, level: 388, strength: 27560, dexterity: 13780, intelligence: 13780, constitution: 99500, luck: 13780, health: 283000000, minWeaponDmg: 1254, maxWeaponDmg: 1500, dungeonRuneBonuses: new DungeonEnemyRuneBonuses { DamageRuneType = RuneType.FireDamage, FireResistance = 50, ColdResistance = 50, LightningResistance = 50, DamageBonus = 50 }, armor: 25500 ),
                    new DungeonEnemy(position: 5, @class: ClassType.Scout, level: 394, strength: 14140, dexterity: 28280, intelligence: 14140, constitution: 102500, luck: 14140, health: 292000000, minWeaponDmg: 1282, maxWeaponDmg: 1526, dungeonRuneBonuses: new DungeonEnemyRuneBonuses { DamageRuneType = RuneType.ColdDamage, FireResistance = 50, ColdResistance = 50, LightningResistance = 50, DamageBonus = 50 }, armor: 26250 ),
                    new DungeonEnemy(position: 6, @class: ClassType.Warrior, level: 400, strength: 30000, dexterity: 15000, intelligence: 15000, constitution: 110000, luck: 15000, health: 300000000, minWeaponDmg: 1505, maxWeaponDmg: 1747, dungeonRuneBonuses: new DungeonEnemyRuneBonuses { DamageRuneType = RuneType.LightningDamage, FireResistance = 50, ColdResistance = 50, LightningResistance = 50, DamageBonus = 50 }, armor: 27000 ),
                    new DungeonEnemy(position: 7, @class: ClassType.Scout, level: 406, strength: 16500, dexterity: 32700, intelligence: 16500, constitution: 114800, luck: 16500, health: 318000000, minWeaponDmg: 1532, maxWeaponDmg: 1779, dungeonRuneBonuses: new DungeonEnemyRuneBonuses { DamageRuneType = RuneType.LightningDamage, FireResistance = 50, ColdResistance = 50, LightningResistance = 50, DamageBonus = 50 }, armor: 27750 ),
                    new DungeonEnemy(position: 8, @class: ClassType.Mage, level: 412, strength: 18000, dexterity: 18000, intelligence: 35400, constitution: 119600, luck: 18000, health: 336000000, minWeaponDmg: 1563, maxWeaponDmg: 1809, dungeonRuneBonuses: new DungeonEnemyRuneBonuses { DamageRuneType = RuneType.FireDamage, FireResistance = 50, ColdResistance = 50, LightningResistance = 50, DamageBonus = 50 }, armor: 28500 ),
                    new DungeonEnemy(position: 9, @class: ClassType.Mage, level: 418, strength: 19500, dexterity: 19500, intelligence: 38100, constitution: 124400, luck: 19500, health: 354000000, minWeaponDmg: 1590, maxWeaponDmg: 1834, dungeonRuneBonuses: new DungeonEnemyRuneBonuses { DamageRuneType = RuneType.FireDamage, FireResistance = 50, ColdResistance = 50, LightningResistance = 50, DamageBonus = 50 }, armor: 29250 ),
                    new DungeonEnemy(position: 10, @class: ClassType.Scout, level: 430, strength: 22500, dexterity: 43500, intelligence: 22500, constitution: 134000, luck: 22500, health: 390000000, minWeaponDmg: 1660, maxWeaponDmg: 1899, dungeonRuneBonuses: new DungeonEnemyRuneBonuses { DamageRuneType = RuneType.ColdDamage, FireResistance = 50, ColdResistance = 50, LightningResistance = 50, DamageBonus = 50 }, armor: 30000 )
                }
            },
            new Dungeon
            {
                Name = DungeonNames.TimeHonoredSchoolOfMagic,
                Position = 117,
                IsDefeated = false,
                IsUnlocked = true,
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
                Type = DungeonTypeEnum.Shadow,
                DungeonEnemies = new List<DungeonEnemy>
                {
                    new DungeonEnemy(position: 1, @class: ClassType.Warrior, level: 500, strength: 80000, dexterity: 40000, intelligence: 40000, constitution: 190000, luck: 40000, health: 600000000, minWeaponDmg: 2007, maxWeaponDmg: 2244, dungeonRuneBonuses: new DungeonEnemyRuneBonuses { DamageRuneType = RuneType.LightningDamage, FireResistance = 50, ColdResistance = 50, LightningResistance = 50, DamageBonus = 50 }, armor: 35000 ),
                    new DungeonEnemy(position: 2, @class: ClassType.Warrior, level: 507, strength: 83500, dexterity: 41750, intelligence: 41750, constitution: 195600, luck: 41750, health: 621000000, minWeaponDmg: 2038, maxWeaponDmg: 2283, dungeonRuneBonuses: new DungeonEnemyRuneBonuses { DamageRuneType = RuneType.FireDamage, FireResistance = 50, ColdResistance = 50, LightningResistance = 50, DamageBonus = 50 }, armor: 35000 ),
                    new DungeonEnemy(position: 3, @class: ClassType.Mage, level: 514, strength: 43500, dexterity: 43500, intelligence: 87000, constitution: 201200, luck: 43500, health: 642000000, minWeaponDmg: 2071, maxWeaponDmg: 2310, dungeonRuneBonuses: new DungeonEnemyRuneBonuses { DamageRuneType = RuneType.ColdDamage, FireResistance = 50, ColdResistance = 50, LightningResistance = 50, DamageBonus = 50 }, armor: 35000 ),
                    new DungeonEnemy(position: 4, @class: ClassType.Scout, level: 521, strength: 45250, dexterity: 90500, intelligence: 45250, constitution: 206800, luck: 45250, health: 663000000, minWeaponDmg: 2115, maxWeaponDmg: 2341, dungeonRuneBonuses: new DungeonEnemyRuneBonuses { DamageRuneType = RuneType.ColdDamage, FireResistance = 50, ColdResistance = 50, LightningResistance = 50, DamageBonus = 50 }, armor: 35000 ),
                    new DungeonEnemy(position: 5, @class: ClassType.Mage, level: 528, strength: 47000, dexterity: 47000, intelligence: 94000, constitution: 212400, luck: 47000, health: 684000000, minWeaponDmg: 2144, maxWeaponDmg: 2377, dungeonRuneBonuses: new DungeonEnemyRuneBonuses { DamageRuneType = RuneType.LightningDamage, FireResistance = 50, ColdResistance = 50, LightningResistance = 50, DamageBonus = 50 }, armor: 35000 ),
                    new DungeonEnemy(position: 6, @class: ClassType.Scout, level: 535, strength: 48750, dexterity: 97500, intelligence: 48750, constitution: 218000, luck: 48750, health: 705000000, minWeaponDmg: 2179, maxWeaponDmg: 2419, dungeonRuneBonuses: new DungeonEnemyRuneBonuses { DamageRuneType = RuneType.LightningDamage, FireResistance = 50, ColdResistance = 50, LightningResistance = 50, DamageBonus = 50 }, armor: 35000 ),
                    new DungeonEnemy(position: 7, @class: ClassType.Mage, level: 542, strength: 50500, dexterity: 50500, intelligence: 101000, constitution: 223600, luck: 50500, health: 726000000, minWeaponDmg: 2222, maxWeaponDmg: 2459, dungeonRuneBonuses: new DungeonEnemyRuneBonuses { DamageRuneType = RuneType.FireDamage, FireResistance = 50, ColdResistance = 50, LightningResistance = 50, DamageBonus = 50 }, armor: 35000 ),
                    new DungeonEnemy(position: 8, @class: ClassType.Warrior, level: 549, strength: 104500, dexterity: 52250, intelligence: 52250, constitution: 229200, luck: 52250, health: 747000000, minWeaponDmg: 2247, maxWeaponDmg: 2485, dungeonRuneBonuses: new DungeonEnemyRuneBonuses { DamageRuneType = RuneType.FireDamage, FireResistance = 50, ColdResistance = 50, LightningResistance = 50, DamageBonus = 50 }, armor: 35000 ),
                    new DungeonEnemy(position: 9, @class: ClassType.Warrior, level: 556, strength: 108000, dexterity: 54000, intelligence: 54000, constitution: 234800, luck: 54000, health: 768000000, minWeaponDmg: 2302, maxWeaponDmg: 2530, dungeonRuneBonuses: new DungeonEnemyRuneBonuses { DamageRuneType = RuneType.ColdDamage, FireResistance = 50, ColdResistance = 50, LightningResistance = 50, DamageBonus = 50 }, armor: 35000 ),
                    new DungeonEnemy(position: 10, @class: ClassType.Mage, level: 570, strength: 57500, dexterity: 57500, intelligence: 115000, constitution: 246000, luck: 57500, health: 810000000, minWeaponDmg: 2412, maxWeaponDmg: 2591, dungeonRuneBonuses: new DungeonEnemyRuneBonuses { DamageRuneType = RuneType.LightningDamage, FireResistance = 50, ColdResistance = 50, LightningResistance = 50, DamageBonus = 50 }, armor: 35000 )
                }
            },
            new Dungeon
            {
                Name = DungeonNames.TavernOfDarkDoppelgangers,
                Position = 121,
                IsDefeated = false,
                IsUnlocked = true,
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
                Type = DungeonTypeEnum.Shadow,
                DungeonEnemies = new List<DungeonEnemy>
                {
                    new DungeonEnemy(position: 1, @class: ClassType.Warrior, level: 345, strength: 48500, dexterity: 12000, intelligence: 12000, constitution: 163000, luck: 23500, health: 318257504, dungeonRuneBonuses: new DungeonEnemyRuneBonuses { DamageRuneType = RuneType.ColdDamage, FireResistance = 0, ColdResistance = 0, LightningResistance = 0, DamageBonus = 60 }, minWeaponDmg: 1190, maxWeaponDmg: 2011 ),
                    new DungeonEnemy(position: 2, @class: ClassType.Warrior, level: 390, strength: 65500, dexterity: 10500, intelligence: 10500, constitution: 184000, luck: 31500, health: 404800000, dungeonRuneBonuses: new DungeonEnemyRuneBonuses { DamageRuneType = RuneType.LightningDamage, FireResistance = 10, ColdResistance = 10, LightningResistance = 60, DamageBonus = 60 }, minWeaponDmg: 1365, maxWeaponDmg: 2339 ),
                    new DungeonEnemy(position: 3, @class: ClassType.Mage, level: 435, strength: 25000, dexterity: 25000, intelligence: 70500, constitution: 231000, luck: 45000, health: 226149008, dungeonRuneBonuses: new DungeonEnemyRuneBonuses { DamageRuneType = RuneType.FireDamage, FireResistance = 35, ColdResistance = 35, LightningResistance = 35, DamageBonus = 60 }, minWeaponDmg: 3425, maxWeaponDmg: 5871 ),
                    new DungeonEnemy(position: 4, @class: ClassType.Warrior, level: 485, strength: 90000, dexterity: 19000, intelligence: 18500, constitution: 265000, luck: 48500, health: 721462528, dungeonRuneBonuses: new DungeonEnemyRuneBonuses { DamageRuneType = RuneType.LightningDamage, FireResistance = 15, ColdResistance = 25, LightningResistance = 60, DamageBonus = 60 }, minWeaponDmg: 1697, maxWeaponDmg: 2909 ),
                    new DungeonEnemy(position: 5, @class: ClassType.Warrior, level: 525, strength: 76500, dexterity: 37000, intelligence: 37000, constitution: 330500, luck: 53500, health: 972496256, dungeonRuneBonuses: new DungeonEnemyRuneBonuses { DamageRuneType = RuneType.FireDamage, FireResistance = 60, ColdResistance = 30, LightningResistance = 30, DamageBonus = 60 }, minWeaponDmg: 1837, maxWeaponDmg: 3149 ),
                    new DungeonEnemy(position: 6, @class: ClassType.Assassin, level: 565, strength: 39500, dexterity: 111000, intelligence: 39500, constitution: 376000, luck: 71500, health: 951280000, dungeonRuneBonuses: new DungeonEnemyRuneBonuses { DamageRuneType = RuneType.ColdDamage, FireResistance = 40, ColdResistance = 50, LightningResistance = 50, DamageBonus = 60 }, minWeaponDmg: 3955, maxWeaponDmg: 6779, armor: 12500 ),
                    new DungeonEnemy(position: 7, @class: ClassType.Berserker, level: 600, strength: 136500, dexterity: 39500, intelligence: 39500, constitution: 417500, luck: 63000, health: 1120570112, dungeonRuneBonuses: new DungeonEnemyRuneBonuses { DamageRuneType = RuneType.ColdDamage, FireResistance = 50, ColdResistance = 50, LightningResistance = 10, DamageBonus = 60 }, minWeaponDmg: 1692, maxWeaponDmg: 3703, armor: 12500 ),
                    new DungeonEnemy(position: 8, @class: ClassType.Scout, level: 625, strength: 63500, dexterity: 163000, intelligence: 63500, constitution: 507500, luck: 93500, health: 1417955072, dungeonRuneBonuses: new DungeonEnemyRuneBonuses { DamageRuneType = RuneType.LightningDamage, FireResistance = 50, ColdResistance = 50, LightningResistance = 50, DamageBonus = 60 }, minWeaponDmg: 2734, maxWeaponDmg: 4686, armor: 12500 ),
                    new DungeonEnemy(position: 9, @class: ClassType.Berserker, level: 645, strength: 182000, dexterity: 42000, intelligence: 42000, constitution: 575000, luck: 100000, health: 1657149952, dungeonRuneBonuses: new DungeonEnemyRuneBonuses { DamageRuneType = RuneType.LightningDamage, FireResistance = 50, ColdResistance = 50, LightningResistance = 50, DamageBonus = 60 }, minWeaponDmg: 1847, maxWeaponDmg: 3877, armor: 12500 ),
                    new DungeonEnemy(position: 10, @class: ClassType.Berserker, level: 660, strength: 229500, dexterity: 62500, intelligence: 62500, constitution: 711000, luck: 137000, health: 2096028032, dungeonRuneBonuses: new DungeonEnemyRuneBonuses { DamageRuneType = RuneType.FireDamage, FireResistance = 60, ColdResistance = 60, LightningResistance = 60, DamageBonus = 60 }, minWeaponDmg: 1852, maxWeaponDmg: 3899, armor: 12500 )
                }
            },
            new Dungeon
            {
                Name = DungeonNames.AshMountain,
                Position = 125,
                IsDefeated = false,
                IsUnlocked = true,
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
                Type = DungeonTypeEnum.Shadow,
                DungeonEnemies = new List<DungeonEnemy>
                {
                    new DungeonEnemy(position: 1, @class: ClassType.Mage, level: 345, strength: 15000, dexterity: 15000, intelligence: 45000, constitution: 200000, luck: 23500, health: 2875500032, dungeonRuneBonuses: new DungeonEnemyRuneBonuses { DamageRuneType = RuneType.ColdDamage, FireResistance = 5, ColdResistance = 60, LightningResistance = 5, DamageBonus = 30 }, minWeaponDmg: 1038, maxWeaponDmg: 2069 ),
                    new DungeonEnemy(position: 2, @class: ClassType.Warrior, level: 390, strength: 60000, dexterity: 20000, intelligence: 20000, constitution: 220000, luck: 31500, health: 8910000128, dungeonRuneBonuses: new DungeonEnemyRuneBonuses { DamageRuneType = RuneType.LightningDamage, FireResistance = 10, ColdResistance = 10, LightningResistance = 60, DamageBonus = 35 }, minWeaponDmg: 520, maxWeaponDmg: 1049 ),
                    new DungeonEnemy(position: 3, @class: ClassType.Scout, level: 435, strength: 25000, dexterity: 75000, intelligence: 25000, constitution: 240000, luck: 45000, health: 8650800128, dungeonRuneBonuses: new DungeonEnemyRuneBonuses { DamageRuneType = RuneType.FireDamage, FireResistance = 60, ColdResistance = 15, LightningResistance = 15, DamageBonus = 35 }, minWeaponDmg: 725, maxWeaponDmg: 1459 ),
                    new DungeonEnemy(position: 4, @class: ClassType.Mage, level: 485, strength: 30000, dexterity: 30000, intelligence: 90000, constitution: 260000, luck: 48500, health: 5212349952, dungeonRuneBonuses: new DungeonEnemyRuneBonuses { DamageRuneType = RuneType.ColdDamage, FireResistance = 20, ColdResistance = 60, LightningResistance = 20, DamageBonus = 40 }, minWeaponDmg: 1460, maxWeaponDmg: 2919 ),
                    new DungeonEnemy(position: 5, @class: ClassType.Mage, level: 525, strength: 35000, dexterity: 35000, intelligence: 105000, constitution: 280000, luck: 53500, health: 6066899968, dungeonRuneBonuses: new DungeonEnemyRuneBonuses { DamageRuneType = RuneType.FireDamage, FireResistance = 60, ColdResistance = 25, LightningResistance = 25, DamageBonus = 40 }, minWeaponDmg: 1578, maxWeaponDmg: 3159 ),
                    new DungeonEnemy(position: 6, @class: ClassType.Warrior, level: 565, strength: 120000, dexterity: 40000, intelligence: 40000, constitution: 300000, luck: 71500, health: 17465624576, dungeonRuneBonuses: new DungeonEnemyRuneBonuses { DamageRuneType = RuneType.LightningDamage, FireResistance = 40, ColdResistance = 40, LightningResistance = 40, DamageBonus = 45 }, minWeaponDmg: 750, maxWeaponDmg: 1509, armor: 15000 ),
                    new DungeonEnemy(position: 7, @class: ClassType.Warrior, level: 600, strength: 135000, dexterity: 45000, intelligence: 45000, constitution: 320000, luck: 63000, health: 19764000768, dungeonRuneBonuses: new DungeonEnemyRuneBonuses { DamageRuneType = RuneType.FireDamage, FireResistance = 60, ColdResistance = 20, LightningResistance = 60, DamageBonus = 45 }, minWeaponDmg: 800, maxWeaponDmg: 1599 ),
                    new DungeonEnemy(position: 8, @class: ClassType.Scout, level: 625, strength: 50000, dexterity: 150000, intelligence: 50000, constitution: 340000, luck: 93500, health: 17487900672, dungeonRuneBonuses: new DungeonEnemyRuneBonuses { DamageRuneType = RuneType.ColdDamage, FireResistance = 60, ColdResistance = 60, LightningResistance = 60, DamageBonus = 50 }, minWeaponDmg: 1045, maxWeaponDmg: 2089, armor: 12500 ),
                    new DungeonEnemy(position: 9, @class: ClassType.Scout, level: 645, strength: 75000, dexterity: 200000, intelligence: 75000, constitution: 360000, luck: 100000, health: 19099799552, dungeonRuneBonuses: new DungeonEnemyRuneBonuses { DamageRuneType = RuneType.FireDamage, FireResistance = 60, ColdResistance = 60, LightningResistance = 60, DamageBonus = 60 }, minWeaponDmg: 1075, maxWeaponDmg: 2159, armor: 12500 ),
                    new DungeonEnemy(position: 10, @class: ClassType.Mage, level: 660, strength: 100000, dexterity: 100000, intelligence: 250000, constitution: 500000, luck: 137000, health: 13567500288, dungeonRuneBonuses: new DungeonEnemyRuneBonuses { DamageRuneType = RuneType.LightningDamage, FireResistance = 60, ColdResistance = 60, LightningResistance = 60, DamageBonus = 60 }, minWeaponDmg: 2000, maxWeaponDmg: 4000 )
                }
            },
            new Dungeon
            {
                Name = DungeonNames.MonsterGrotto,
                Position = 128,
                IsDefeated = false,
                IsUnlocked = true,
                Type = DungeonTypeEnum.Shadow,
                DungeonEnemies = new List<DungeonEnemy>
                {
                    new DungeonEnemy(position: 1, @class: ClassType.Warrior, level: 600, strength: 280000, dexterity: 140000, intelligence: 140000, constitution: 430000, luck: 115000, health: 15000000000, minWeaponDmg: 3500, maxWeaponDmg: 3750, dungeonRuneBonuses: new DungeonEnemyRuneBonuses { DamageRuneType = RuneType.LightningDamage, FireResistance = 0, ColdResistance = 0, LightningResistance = 50, DamageBonus = 50 }, armor: 35000 ),
                    new DungeonEnemy(position: 2, @class: ClassType.Mage, level: 605, strength: 142500, dexterity: 142500, intelligence: 285000, constitution: 434000, luck: 116250, health: 15350000000, minWeaponDmg: 3625, maxWeaponDmg: 3875, dungeonRuneBonuses: new DungeonEnemyRuneBonuses { DamageRuneType = RuneType.ColdDamage, FireResistance = 0, ColdResistance = 50, LightningResistance = 0, DamageBonus = 50 }, armor: 35000 ),
                    new DungeonEnemy(position: 3, @class: ClassType.Scout, level: 610, strength: 157500, dexterity: 315000, intelligence: 157500, constitution: 458000, luck: 123750, health: 17450000000, minWeaponDmg: 4375, maxWeaponDmg: 4625, dungeonRuneBonuses: new DungeonEnemyRuneBonuses { DamageRuneType = RuneType.LightningDamage, FireResistance = 0, ColdResistance = 0, LightningResistance = 50, DamageBonus = 50 }, armor: 35000 ),
                    new DungeonEnemy(position: 4, @class: ClassType.Warrior, level: 615, strength: 320000, dexterity: 160000, intelligence: 160000, constitution: 462000, luck: 125000, health: 17800000000, minWeaponDmg: 4500, maxWeaponDmg: 4750, dungeonRuneBonuses: new DungeonEnemyRuneBonuses { DamageRuneType = RuneType.FireDamage, FireResistance = 50, ColdResistance = 0, LightningResistance = 0, DamageBonus = 50 }, armor: 35000 ),
                    new DungeonEnemy(position: 5, @class: ClassType.Mage, level: 620, strength: 175000, dexterity: 175000, intelligence: 350000, constitution: 486000, luck: 132500, health: 19900000000, minWeaponDmg: 5250, maxWeaponDmg: 5500, dungeonRuneBonuses: new DungeonEnemyRuneBonuses { DamageRuneType = RuneType.ColdDamage, FireResistance = 0, ColdResistance = 50, LightningResistance = 0, DamageBonus = 50 }, armor: 35000 ),
                    new DungeonEnemy(position: 6, @class: ClassType.Warrior, level: 625, strength: 355000, dexterity: 177500, intelligence: 177500, constitution: 490000, luck: 133750, health: 20250000000, minWeaponDmg: 5375, maxWeaponDmg: 5625, dungeonRuneBonuses: new DungeonEnemyRuneBonuses { DamageRuneType = RuneType.FireDamage, FireResistance = 50, ColdResistance = 0, LightningResistance = 0, DamageBonus = 50 }, armor: 35000 ),
                    new DungeonEnemy(position: 7, @class: ClassType.Scout, level: 630, strength: 203000, dexterity: 406000, intelligence: 203000, constitution: 514000, luck: 141250, health: 22350000000, minWeaponDmg: 6125, maxWeaponDmg: 6375, dungeonRuneBonuses: new DungeonEnemyRuneBonuses { DamageRuneType = RuneType.LightningDamage, FireResistance = 0, ColdResistance = 0, LightningResistance = 50, DamageBonus = 50 }, armor: 35000 ),
                    new DungeonEnemy(position: 8, @class: ClassType.Scout, level: 635, strength: 206000, dexterity: 412000, intelligence: 206000, constitution: 518000, luck: 142500, health: 22700000000, minWeaponDmg: 6250, maxWeaponDmg: 6500, dungeonRuneBonuses: new DungeonEnemyRuneBonuses { DamageRuneType = RuneType.FireDamage, FireResistance = 50, ColdResistance = 0, LightningResistance = 0, DamageBonus = 50 }, armor: 35000 ),
                    new DungeonEnemy(position: 9, @class: ClassType.Warrior, level: 640, strength: 448000, dexterity: 224000, intelligence: 224000, constitution: 542000, luck: 150000, health: 24800000000, minWeaponDmg: 7000, maxWeaponDmg: 7250, dungeonRuneBonuses: new DungeonEnemyRuneBonuses { DamageRuneType = RuneType.ColdDamage, FireResistance = 0, ColdResistance = 50, LightningResistance = 0, DamageBonus = 50 }, armor: 35000 ),
                    new DungeonEnemy(position: 10, @class: ClassType.Warrior, level: 650, strength: 460000, dexterity: 230000, intelligence: 230000, constitution: 550000, luck: 152500, health: 25500000000, minWeaponDmg: 7250, maxWeaponDmg: 7500, dungeonRuneBonuses: new DungeonEnemyRuneBonuses { DamageRuneType = RuneType.FireDamage, FireResistance = 50, ColdResistance = 0, LightningResistance = 0, DamageBonus = 50 }, armor: 35000 )
                }
            },

            #endregion

            #region Loop of Idols

            new Dungeon {
                Name = DungeonNames.ContinousLoopOfIdols,
                Position = 130,
                IsDefeated = false,
                IsUnlocked = true,
                Type = DungeonTypeEnum.Shadow,
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

        foreach (var dungeon in Dungeons)
        {
            foreach (var dungeonEnemy in dungeon.DungeonEnemies)
            {
                dungeonEnemy.Dungeon = dungeon;
            }
        }
    }
}
