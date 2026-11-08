namespace SFSimulator.Core;

public static class DungeonNames
{
    public static string Get(int index)
    {
        return index switch {
            1 or 101 => "Desacrated Catacombs",
            2 or 102 => "The Mines of Gloria",
            3 or 103 => "The Ruins of Gnark",
            4 or 104 => "The Cutthroat Grotto",
            5 or 105 => "The Emrald Scale Altar",
            6 or 106 => "The Toxic Tree",
            7 or 107 => "The Magma Stream",
            8 or 108 => "The Frost Blood Temple",
            9 or 109 => "The Pyramid of Madness",
            10 or 110 => "Black Skull Fortress",
            11 or 111 => "Circus Of Terror",
            12 or 112 => "Hell",
            13 or 113 => "Dragon's Hoard",
            14 or 114 => "House of Horrors",
            15 or 115 => "The 13th floor",
            16 or 116 => "The 3rd League of Superheroes",
            17 or 117 => "Time-honored School of Magic",
            18 or 118 => "Hemorridor",
            19 or 119 => "Easteros",
            20 or 120 => "Dojo of Childhood Heroes",
            21 or 121 => "Tavern of the Dark Doppelgangers",
            22 or 122 => "City of Intrigues",
            23 or 123 => "School of Magic Express",
            24 or 124 => "Nordic Gods",
            25 or 125 => "Ash Mountain",
            26 or 126 => "Playa HQ",
            27 or 127 => "Mount Olympus",
            28 or 128 => "Monster Grotto",
            29 or 129 => "Arcade of the Old Pixel Icons",
            30 or 130 => "The Server Room",
            31 or 131 => "Workshop of the Hunters of the Undead",
            32 or 132 => "Retro TV Legends",
            33 or 133 => "The Meeting Room",
            201 => "Unwavering Warrior Guild",
            202 => "Inquisitive Mage Guild",
            203 => "Acrobatic Scout Guild",
            204 => "Lively Necromancer Guild",
            -1 => "Tower",
            -2 => "Twister",
            -3 => "Continous Loop of Idols",
            -4 => "Sandstorm",
            _ => $"Unknown ({index})"
        };
    }
}