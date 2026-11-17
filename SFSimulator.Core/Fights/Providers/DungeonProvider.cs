using System.Text.Json;

namespace SFSimulator.Core;

public class DungeonProvider : IDungeonProvider
{
    private static readonly Dictionary<int, DungeonData> DungeonDefinitions = LoadDungeonDefinitions();

    private List<Dungeon> Dungeons { get; } = InitDungeons();
    public void InitCharacterDungeonState(SimulationContext simulationContext)
    {
        var dungeonData = simulationContext.DungeonsData;
        if (dungeonData is null) return;

        var lightWorld = dungeonData.Normal
            .Select((currentPosition, dungeonIndex) => (currentPosition, MapDungeonIndex(dungeonIndex + 1)));
        var shadowWorld = dungeonData.Shadow
            .Select((currentPosition, dungeonIndex) => (currentPosition, MapDungeonIndex(dungeonIndex + 1) + 100));
        var classWorld = dungeonData.Class
            .Select((currentPosition, dungeonIndex) => (currentPosition, dungeonIndex + 1 + 200));
        var tower = (dungeonData.Tower, -1);
        var twister = (dungeonData.Twister, -2);
        var loopOfIdols = (dungeonData.Youtube, -3);
        var sandstorm = (dungeonData.Sandstorm, -4);
        var combinedDungeons = lightWorld.Union(shadowWorld).Union(classWorld).Append(tower).Append(loopOfIdols).Append(twister).Append(sandstorm);

        var dungeons = GetAllDungeons(simulationContext);

        foreach (var (beatenEnemyPosition, dungeonPosition) in combinedDungeons)
        {
            var dungeon = dungeons.FirstOrDefault(d => d.Position == dungeonPosition);
            if (dungeon is null) continue;
            var beatenEnemy = dungeon.DungeonEnemies.FirstOrDefault(e => e.Position == beatenEnemyPosition);
            if (beatenEnemy is null)
            {
                dungeon.IsUnlocked = false;
                continue;
            }

            foreach (var enemy in dungeon.DungeonEnemies.Where(e => e.Position <= beatenEnemy.Position))
            {
                enemy.IsDefeated = true;
            }

            if (dungeon.DungeonEnemies.All(e => e.IsDefeated)) dungeon.IsDefeated = true;
        }
    }

    public static int MapDungeonIndex(int sfToolsIndex)
    {
        return (sfToolsIndex) switch
        {
            >= 1 and <= 12 => sfToolsIndex,
            13 => 15,
            14 => 19,
            15 => 17,
            16 => 18,
            17 => 24,
            18 => 27,
            19 => 21,
            20 => 13,
            21 => 14,
            22 => 16,
            23 => 20,
            24 => 28,
            25 => 22,
            26 => 23,
            27 => 25,
            28 => 26,
            29 => 29,
            30 => 30,
            31 => 31,
            32 => 32,
            33 => 33,
            _ => 0
        };
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

    public DungeonEnemy? GetDungeonEnemySafe(int dungeonPosition, int dungeonEnemyPosition)
    {
        return Dungeons.FirstOrDefault(d => d.Position == dungeonPosition)?.DungeonEnemies.FirstOrDefault(e => e.Position == dungeonEnemyPosition);
    }

    public static List<Dungeon> InitDungeons()
    {
        var list = new List<Dungeon>(DungeonDefinitions.Values.Count);
        foreach (var dungeonDefinition in DungeonDefinitions.Values)
        {
            var position = MapFromJsonId(dungeonDefinition.Id);
            var dungeon = new Dungeon
            {
                Position = position,
                IsUnlocked = true,
                Type = DungeonTypeEnumExtensions.FromPosition(position),
                Name = DungeonNames.Get(position),
                UnlockResolve = DungeonUnlockResolvers.Get(position),
                DungeonEnemies = dungeonDefinition.Floors.Values.OrderBy(f => f.Position)
                .Select(f => new DungeonEnemy(f.Position, f.Class ?? ClassType.Mirror, f.Level, f.Strength, f.Dexterity, f.Intelligence,
                    f.Constitution, f.Luck, f.Health, f.MinWeaponDamage, f.MaxWeaponDamage,
                    f.Runes is { } runes ? new DungeonEnemyRuneBonuses
                    {
                        DamageRuneType = runes.Type switch
                        {
                            40 => RuneType.FireDamage,
                            41 => RuneType.ColdDamage,
                            42 => RuneType.LightningDamage,
                            _ => throw new InvalidOperationException($"Unknown dungeon damage rune type: {runes.Type}.")
                        },
                        DamageBonus = runes.Damage,
                        FireResistance = runes.Resistances[0],
                        ColdResistance = runes.Resistances[1],
                        LightningResistance = runes.Resistances[2]
                    } : null,
                    armor: f.Armor,
                    armorMultiplier: dungeonDefinition.ArmorMultiplier))
                .ToList()
            };

            foreach (var enemy in dungeon.DungeonEnemies)
            {
                enemy.Dungeon = dungeon;
            }
            list.Add(dungeon);

        }

        return list;
    }

    private static Dictionary<int, DungeonData> LoadDungeonDefinitions()
    {
        using var stream = typeof(DungeonProvider).Assembly.GetManifestResourceStream("SFSimulator.Core.Fights.Providers.DungeonsData.json")
            ?? throw new InvalidOperationException("The embedded dungeon data could not be found.");

        return JsonSerializer.Deserialize(stream, DungeonDataJsonContext.Default.DictionaryInt32DungeonData)
            ?? throw new InvalidOperationException("The embedded dungeon data is empty.");
    }

    private static int MapFromJsonId(int id)
    {
        return id switch
        {
            // Light World
            1 => 1,
            2 => 2,
            3 => 3,
            4 => 4,
            5 => 5,
            6 => 6,
            7 => 7,
            8 => 8,
            9 => 9,
            10 => 10,
            11 => 11,
            12 => 12,
            13 => 15,
            14 => 19,
            16 => 17,
            17 => 18,
            19 => 24,
            20 => 27,
            21 => 21,
            22 => 13,
            23 => 14,
            24 => 16,
            25 => 20,
            26 => 28,
            27 => 22,
            28 => 23,
            29 => 25,
            30 => 26,
            31 => 29,
            32 => 30,
            33 => 31,
            34 => 32,
            35 => 33,

            // Shadow
            101 => 101,
            102 => 102,
            103 => 103,
            104 => 104,
            105 => 105,
            106 => 106,
            107 => 107,
            108 => 108,
            109 => 109,
            110 => 110,
            111 => 111,
            112 => 112,
            113 => 115,
            114 => 119,
            116 => 117,
            117 => 118,
            119 => 124,
            120 => 127,
            121 => 121,
            122 => 113,
            123 => 114,
            124 => 116,
            125 => 120,
            126 => 128,
            127 => 122,
            128 => 123,
            129 => 125,
            130 => 126,
            131 => 129,
            132 => 130,
            133 => 131,
            134 => 132,
            135 => 133,

            // Class world
            300 => 201,
            301 => 202,
            302 => 203,
            303 => 204,

            201 => -1,
            202 => -3,
            203 => -2,
            204 => -4,
            _ => throw new ArgumentOutOfRangeException(nameof(id))
        };
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
            mirrorEnemy.LightningResistance = simulationContext.LightningResistance;
            mirrorEnemy.ColdResistance = simulationContext.ColdResistance;
            mirrorEnemy.FireResistance = simulationContext.FireResistance;
            mirrorEnemy.Health = simulationContext.Health / (simulationContext.Level + 1) * (mirrorEnemy.Level + 1);
        }

        return dungeonEnemies;
    }
}