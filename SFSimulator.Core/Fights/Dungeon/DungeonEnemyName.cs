namespace SFSimulator.Core;

public static class DungeonEnemyName
{
    public static string GetName(string dungeonName, int dungeonEnemyPosition)
        => EnemyNames[dungeonName].FirstOrDefault(e => e.Position == dungeonEnemyPosition).Name ?? string.Empty;

    private static Dictionary<string, List<(int Position, string Name)>> EnemyNames { get; set; } = new()
    {
        {
            DungeonNames.DesecratedCatacombs,
            new()
            {
            (1, "Ghost"),
            (2, "Skeleton"),
            (3, "Undead"),
            (4, "Devious Vampire"),
            (5, "Night Ghoul"),
            (6, "Banshee"),
            (7, "Skeleton Soldier"),
            (8, "Voodoo Master"),
            (9, "Flesh Golem"),
            (10, "Lord of Darkness")
            }
        },
        {
            DungeonNames.MinesOfGloria,
            new()
            {
            (1, "Water Glompf"),
            (2, "Yeti"),
            (3, "Skeleton"),
            (4, "Ugly Gremlin"),
            (5, "Stone Giant"),
            (6, "Fire Elemental"),
            (7, "Stone Troll"),
            (8, "Redlight Succubus"),
            (9, "Abhorrent Demon"),
            (10, "Hell Beast")
            }
        },
        {
            DungeonNames.RuinsOfGnark,
            new()
            {
            (1, "Sewer Rat"),
            (2, "Dusty Bat"),
            (3, "Terror Tarantula"),
            (4, "Rowdy Robber"),
            (5, "Dirty Rotten Scoundrel"),
            (6, "Grim Wolf"),
            (7, "Bad Bandit"),
            (8, "Beastie"),
            (9, "Grave Robber"),
            (10, "Robber Chief")
            }
        },
        {
            DungeonNames.CutthroatGrotto,
            new()
            {
            (1, "Wind Elemental"),
            (2, "Pirate Dark Beard"),
            (3, "Rowdy Robber"),
            (4, "Shadow Alligator"),
            (5, "Sturdy Swashbuckler"),
            (6, "Mean Monster Rabbit"),
            (7, "Cutthroat"),
            (8, "Pirate Blood Nose"),
            (9, "Octopus"),
            (10, "Pirate Leader")
            }
        },
        {
            DungeonNames.EmeraldScaleAltar,
            new()
            {
            (1, "Rattling Cobra"),
            (2, "Slashing Saurus"),
            (3, "Roaring Raptor"),
            (4, "Swamp Warrior"),
            (5, "Green Rex"),
            (6, "Saurus Rogue"),
            (7, "Swamp Dragon"),
            (8, "Swamp Gorgon"),
            (9, "Toxic Dragon"),
            (10, "King Saurus")
            }
        },
        {
            DungeonNames.ToxicTree,
            new()
            {
            (1, "Toxic Tree"),
            (2, "Ugly Gremlin"),
            (3, "Rabid Wolf"),
            (4, "Slime Blob"),
            (5, "Greenish Gremlin"),
            (6, "Infected Brown Bear"),
            (7, "Greedy Gremlin"),
            (8, "Swamp Muncher"),
            (9, "Cruel Gremlin"),
            (10, "Terrible Toxic Gremlin")
            }
        },
        {
            DungeonNames.MagmaStream,
            new()
            {
            (1, "Fire Scorpion"),
            (2, "Fire Basilisk"),
            (3, "Lava Blob"),
            (4, "Lava Giant"),
            (5, "Dragon of Darkness"),
            (6, "Hell Cyclops"),
            (7, "Fire Elemental"),
            (8, "Lava Giant"),
            (9, "Giant Dragon"),
            (10, "Ghost of the Volcano")
            }
        },
        {
            DungeonNames.FrostBloodTemple,
            new()
            {
            (1, "Yeti"),
            (2, "Black Phantom"),
            (3, "Dragon of Cold"),
            (4, "Unholy Monk"),
            (5, "Hell Alien"),
            (6, "The Extraterrestrial"),
            (7, "Dragon of Madness"),
            (8, "Twilight Alien"),
            (9, "Out of State Alien"),
            (10, "Killing Machine")
            }
        },
        {
            DungeonNames.PyramidsOfMadness,
            new()
            {
            (1, "Cave Cyclops"),
            (2, "Sandstorm"),
            (3, "Hell Alien"),
            (4, "Bigfoot"),
            (5, "Ghost"),
            (6, "Timmy Suprino"),
            (7, "Demoralizing Demon"),
            (8, "Pink Monster Rabbit"),
            (9, "Banshee"),
            (10, "Yourself")
            }
        },
        {
            DungeonNames.BlackSkullFortress,
            new()
            {
            (1, "Dark Rider"),
            (2, "Skeleton Warrior"),
            (3, "Black Skull Warrior"),
            (4, "Night Troll"),
            (5, "Panther"),
            (6, "Man-Eater"),
            (7, "Swamp Dragon"),
            (8, "Black Skull Warrior"),
            (9, "Dragon of Darkness"),
            (10, "Knight of the Black Skull")
            }
        },
        {
            DungeonNames.CircusOfTerror,
            new()
            {
            (1, "Happy Slappy the Clown"),
            (2, "The Blind Knife Thrower"),
            (3, "Miniature Gnome"),
            (4, "The Bearded Lady"),
            (5, "The Psycho Juggler"),
            (6, "Siamese Twins"),
            (7, "Bronco the Joker"),
            (8, "The Snake-man"),
            (9, "Madame Mystique"),
            (10, "Bozo the Terror Clown")
            }
        },
        {
            DungeonNames.Hell,
            new()
            {
            (1, "Restless Soul"),
            (2, "Furious Soul"),
            (3, "Old Soul"),
            (4, "Pest"),
            (5, "Soul Clump"),
            (6, "Scourge"),
            (7, "Hellhound"),
            (8, "The Fuehrer's Heap"),
            (9, "The Devil's Advocate"),
            (10, "Beelzeboss")
            }
        },
        {
            DungeonNames.DragonsHoard,
            new()
            {
            (1, "Not So Snappy Dragon"),
            (2, "Golden Dragon with Fiery-Eyed Glance"),
            (3, "Seductive Mother of Drankeys"),
            (4, "Undead Saurian Fossil"),
            (5, "Shenlong"),
            (6, "Baby Dragon"),
            (7, "New Year's Dragon"),
            (8, "Pet Out of Control"),
            (9, "Dragon with Concussion"),
            (10, "Annoyed Mother of Dragons")
            }
        },
        {
            DungeonNames.HouseOfHorrors,
            new()
            {
            (1, "Cutlery Head"),
            (2, "Conceptual Proof"),
            (3, "Dracula"),
            (4, "Master of Phone Pranks"),
            (5, "Lovable Twins"),
            (6, "Friendly Sewer Clown"),
            (7, "Therapist's Doll"),
            (8, "Your Worst Nightmare"),
            (9, "Butcher Next Door"),
            (10, "Tube Girl")
            }
        },
        {
            DungeonNames.ThirteenthFloor,
            new()
            {
            (1, "Hellgore the Hellish"),
            (2, "Henry the Magic Fairy"),
            (3, "Jet the Panty Raider"),
            (4, "Clapper van Hellsing"),
            (5, "The KOma KOmmander"),
            (6, "Roughian the Ruthless"),
            (7, "Ben the Marketeer"),
            (8, "Hector the Contractor"),
            (9, "Motu with a Club"),
            (10, "Jack the Hammerer")
            }
        },
        {
            DungeonNames.ThirdLeagueOfSuperheroes,
            new()
            {
            (1, "1,000 Punch Man"),
            (2, "Mister X-Ray"),
            (3, "Toilet Tracker"),
            (4, "Iron Man"),
            (5, "Rain Man"),
            (6, "Invisible Woman"),
            (7, "Candy Man"),
            (8, "Captain Caffeine"),
            (9, "Forgetful Fist"),
            (10, "Captain Monday")
            }
        },
        {
            DungeonNames.TimeHonoredSchoolOfMagic,
            new()
            {
            (1, "Eloquent Hat"),
            (2, "Sour Argus"),
            (3, "Fluffy Friend"),
            (4, "A. van Blame"),
            (5, "Phony Locky"),
            (6, "Killer Stare"),
            (7, "Bad Kisser"),
            (8, "Guardian of the Golden Egg"),
            (9, "Gentle Giant"),
            (10, "Pedigree Bad Boy")
            }
        },
        {
            DungeonNames.Hemorridor,
            new()
            {
            (1, "Orc on Warg"),
            (2, "Troll Trio"),
            (3, "The King"),
            (4, "Smollum"),
            (5, "Spiders again"),
            (6, "Orc Boss"),
            (7, "Bezog"),
            (8, "Smoulder"),
            (9, "Nazguls...Nazgulses?"),
            (10, "Monster in the Lake")
            }
        },
        {
            DungeonNames.Easteros,
            new()
            {
            (1, "Robert Drunkatheon"),
            (2, "Lefty Lennister"),
            (3, "Petyr the Pimp"),
            (4, "Holundor"),
            (5, "Drogo the Threatening"),
            (6, "The Ginger Slowworm"),
            (7, "Queen Mother"),
            (8, "The Miniature Poodle"),
            (9, "The Riding Mountainrange"),
            (10, "Joffrey the Kid Despot")
            }
        },
        {
            DungeonNames.DojoOfChildhoodHeros,
            new()
            {
            (1, "Magical Schoolgirl"),
            (2, "Master of Doppelgangers"),
            (3, "Friendly Helper from the Neighborhood"),
            (4, "Student Shinigami"),
            (5, "Wannabe Detective"),
            (6, "Rubber Pirate"),
            (7, "Pet Trainer"),
            (8, "Joseph Joker"),
            (9, "Supreme Alien"),
            (10, "Cheating Brat")
            }
        },
        {
            DungeonNames.TavernOfDarkDoppelgangers,
            new()
            {
            (1, "Dancing Mushroom"),
            (2, "Deserted City Guard"),
            (3, "Baneful Bartender"),
            (4, "Sepulchral Scammer"),
            (5, "Brutal Blacksmith"),
            (6, "Wily Witch"),
            (7, "Sinister Salesman"),
            (8, "Atrocious Abawuwu"),
            (9, "Skullsplitter Shakes"),
            (10, "Frightening Fidget")
            }
        },
        {
            DungeonNames.CityOfIntrigues,
            new()
            {
            (1, "Cool Villain"),
            (2, "Brygitte"),
            (3, "Snowman and Shadow Wolf"),
            (4, "The Woman in Red"),
            (5, "Boyish Brienne"),
            (6, "Ramsay the Degrader"),
            (7, "Faceless"),
            (8, "Vicious Gnome"),
            (9, "The Protector"),
            (10, "The Hard to Burn")
            }
        },
        {
            DungeonNames.SchoolOfMagicExpress,
            new()
            {
            (1, "Unrepentant Penitent"),
            (2, "The Gifted One (and Ron)"),
            (3, "Stumbledoor"),
            (4, "Petey Rat"),
            (5, "Diabolical Dolores"),
            (6, "Inconveniently Infinite Inferi"),
            (7, "Bella the Beastly"),
            (8, "Lucius the Pure-Blood"),
            (9, "Cutsie Cuddler"),
            (10, "You Should Know Who ...")
            }
        },
        {
            DungeonNames.NordicGods,
            new()
            {
            (1, "Heimdall"),
            (2, "Valkyries"),
            (3, "Hel"),
            (4, "Thor"),
            (5, "Odin"),
            (6, "Loki"),
            (7, "Ymir"),
            (8, "Midgard Serpent"),
            (9, "Fenris Wolf"),
            (10, "Surtr")
            }
        },
        {
            DungeonNames.AshMountain,
            new()
            {
            (1, "Valaraukar"),
            (2, "Urcsi the Uruk"),
            (3, "Prompter Splittongue"),
            (4, "Samowar the Pale"),
            (5, "Oliphaunt Tamer"),
            (6, "Undead Army"),
            (7, "Shelantula"),
            (8, "Ruler of the Nine"),
            (9, "Mauron's Maw"),
            (10, "The Necromancer")
            }
        },
        {
            DungeonNames.PlayaHQ,
            new()
            {
            (1, "Thesi the Terrible"),
            (2, "Peter the Impaler"),
            (3, "Gert the Ghastly"),
            (4, "Leander, Lord of Lies"),
            (5, "Hannah the Hellish"),
            (6, "Alex, Angel of Abyss"),
            (7, "Jan the Hacker"),
            (8, "Four-handed Peter"),
            (9, "The Artist Andreas"),
            (10, "Marv the Minor Evil")
            }
        },
        {
            DungeonNames.MountOlympus,
            new()
            {
            (1, "Singing Sirens"),
            (2, "Polyphemus the Cyclops"),
            (3, "Cerberus"),
            (4, "Medusa"),
            (5, "Athena"),
            (6, "Hercules"),
            (7, "Ares"),
            (8, "Poseidon"),
            (9, "Hades"),
            (10, "Zeus")
            }
        },
        {
            DungeonNames.MonsterGrotto,
            new()
            {
            (1, "Doltopus"),
            (2, "Hairy Groom"),
            (3, "Monster Truck"),
            (4, "Nudist Giant"),
            (5, "Marshmallow Man"),
            (6, "Sandbox Worm"),
            (7, "Optimist Supreme"),
            (8, "Three-headed Dragon God"),
            (9, "Angry Giant Saurian"),
            (10, "Berlin Scene Monster")
            }
        },
        {
            DungeonNames.Tower,
            new()
            {
            (1, "Living Cake Man"),
            (2, "Green Fairy Drinkerbell"),
            (3, "Tinvalid"),
            (4, "Harmless Teddy Bear"),
            (5, "Flowerlina"),
            (6, "Tooth Fairy"),
            (7, "Ugly Chick"),
            (8, "Warbling Birdie"),
            (9, "Well-Meaning Fairy"),
            (10, "Trickeribook's Cheatinchild"),
            (11, "Singing Dumpling"),
            (12, "Puppeteer's Right"),
            (13, "Grinning Cat"),
            (14, "Ambitious Frog"),
            (15, "Pinociwhatsit"),
            (16, "3x3 Wishes"),
            (17, "Bootlegged Puss"),
            (18, "Dotty from Kansas"),
            (19, "The Last Airgazer"),
            (20, "A Rabbit and a Hedgehog"),
            (21, "Holger Nilsson"),
            (22, "High-Spirited Ghost"),
            (23, "Blood-Red Riding Hood"),
            (24, "Snowflake"),
            (25, "Star Money"),
            (26, "Miss Match"),
            (27, "Ice Queen"),
            (28, "Badly Raised Boys"),
            (29, "Lambikins and Fishy"),
            (30, "Donkey Shot"),
            (31, "Street Thief with Monkey"),
            (32, "Alice in Wonder"),
            (33, "Penterabbit"),
            (34, "Dynamic Peter"),
            (35, "Foolish Princess"),
            (36, "Pleasure Addict"),
            (37, "Amnastasia Rubliovka"),
            (38, "Useless Livestock"),
            (39, "Humpty Dumpty"),
            (40, "King Chinbeard"),
            (41, "Sandman"),
            (42, "John or Tom?"),
            (43, "Scarecrow"),
            (44, "Mirrored Fool"),
            (45, "Three Little Pigs"),
            (46, "Goose in Luck"),
            (47, "Simpleminded Chicken Thief"),
            (48, "Baba Yaga"),
            (49, "Merlin"),
            (50, "Julio and Romy"),
            (51, "Prince in Shepherd's Skin"),
            (52, "Robin the Redistributor"),
            (53, "Ali the Sesame Opener"),
            (54, "Freshly Dressed Emperor"),
            (55, "Dumbo"),
            (56, "Hansel and Gretel"),
            (57, "Bear Fear"),
            (58, "Pokerhontas"),
            (59, "Mass Fly Murderer"),
            (60, "Cinderella"),
            (61, "The Enchanting Genie"),
            (62, "Bronycorn"),
            (63, "Hulda the Cloud Fairy"),
            (64, "Leprechore"),
            (65, "Robber Hopsenplops"),
            (66, "Thorny Lion"),
            (67, "Aquirella the Dazzler"),
            (68, "Prince Charming"),
            (69, "B. O. Wolf"),
            (70, "Peter the Wolf"),
            (71, "Beautiful Princess"),
            (72, "Fearless Wanderer"),
            (73, "Red&White Forever"),
            (74, "Friendly Snowman"),
            (75, "Parsifal"),
            (76, "Brother Barfly"),
            (77, "King Arthur"),
            (78, "Sigi Musclehead"),
            (79, "Pied Piper"),
            (80, "The Guys from Oz"),
            (81, "'Little' John"),
            (82, "The Easter Bunny"),
            (83, "Honey Robbear"),
            (84, "Shirk the Ogre"),
            (85, "Cozy Bear"),
            (86, "Number Nip"),
            (87, "Three Hungry Bears"),
            (88, "Seven Hostages"),
            (89, "Seven Dwarfs"),
            (90, "Respectable Dragon Slayer"),
            (91, "Ducat Donkey"),
            (92, "Bean Counter"),
            (93, "Happy Dragon"),
            (94, "Shockheaded Jack"),
            (95, "Papa Frost"),
            (96, "Dream Couple"),
            (97, "Three Ghosts"),
            (98, "Sleepy Princess"),
            (99, "Nanobot Porridge"),
            (100, "Barbpunzel")
            }
        },
        {
            DungeonNames.Twister,
            new()
            {
            (486, "Grey Ghoul"),
            (658, "Blood Occultist"),
            (711, "Gragosh the Destroyer"),
            (777, "Puppeteer's Right"),
            (830, "Sewer Rat"),
            (867, "Dirty Bat"),
            (868, "Dirty Bat"),
            (869, "Dirty Bat"),
            (870, "Dirty Bat"),
            (871, "Dirty Bat"),
            (872, "Dirty Bat"),
            (873, "Dirty Bat"),
            (874, "Dirty Bat"),
            (875, "Bat Out of hell"),
            (876, "Undead"),
            (877, "Restless Soul"),
            (939, "Dragon of Hell"),
            (947, "Dynamic Peter"),
            (954, "Jungle Scorpion"),
            (962, "Miniature Gnome"),
            (981, "Butthead"),
            (990, "Three Ghosts"),
            (998, "Nevorfull"),
            (999, "Swamp Nymphomaniac"),
            (1000, "Shniva")
            }
        },
        {
            DungeonNames.ContinousLoopOfIdols,
            new()
            {
            (1, "DoctorBenx"),
            (2, "Zakreble"),
            (3, "Golden Gianpy"),
            (4, "Willyrex"),
            (5, "Alvaro845"),
            (6, "Paul Terra"),
            (7, "Spieletrend"),
            (8, "Fatty Pillow"),
            (9, "Gimper"),
            (10, "Unge"),
            (11, "KeysJore"),
            (12, "Aypierre"),
            (13, "Mandzio"),
            (14, "Boruciak"),
            (15, "Fifqo"),
            (16, "Zsoze"),
            (17, "ZeboPL"),
            (18, "Dhalucard"),
            (19, "Earliboy"),
            (20, "Skate702"),
            (21, "Dorzer")
            }
        },
    };
}