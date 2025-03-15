using Microsoft.AspNetCore.Components;

namespace SFSimulator.Frontend.Components;

public partial class SfIcon
{
    [Parameter(CaptureUnmatchedValues = true)]
    public Dictionary<string, object>? Attributes { get; set; }

    [Parameter, EditorRequired]
    public required string Icon { get; set; }
    private string Style =>
    @$"
    width: 25px; 
    height: 25px; 
    background-size: contain; 
    background-image: url(./icons/{IconPaths[Icon]});
    ";

    private Dictionary<string, string> IconPaths = new()
    {
       { "Level", "level_icon.png" },
       { "XP", "xp_icon.png" },
       { "Academy", "academy_icon.png" },
       { "GemMine", "gemmine_icon.png" },
       { "Hydra", "hydra_icon.png" },
       { "BaseStat", "basestats_icon.png" },
       { "Mount", "mount_icon.png" },
       { "GoldPit", "goldpit_icon.png" },
       { "Treasury", "treasury_icon.png" },
       { "Scrapbook", "scrapbook_icon.png" },
       { "Instructor", "guildxp_icon.png" },
       { "XPRune", "xprune_icon.png" },
       { "Tower", "tower_icon.png" },
       { "Treasure", "guildgold_icon.png" },
       { "GoldRune", "goldrune_icon.png" },
       { "Thirst", "thirst_icon.png" },
       { "ExpeditionChest", "expeditionchest_icon.png" },
       { "ExpeditionStar", "expeditionstar_icon.png" },
       { "Switch", "switch_icon.png" },
       { "Calendar", "calendar_icon.png" },
       { "Hourglass", "hourglass_icon.png" },
       { "Guard", "guard_icon.png" },
       { "Arena", "arena_icon.png" },
       { "Wheel", "wheel_icon.png" },
       { "Hybrid", "hybrid_icon.png" },
       { "Character", "character_icon.png" },
       { "Bonuses", "bonuses_icon.png" },
       { "Playstyle", "playstyle_icon.png" },
       { "Dungeon", "dungeon_icon.png" },
       { "Endpoint", "endpoint_icon.png" },
       { "Load", "load_icon.png" },
       { "Chart", "chart_icon.png" },
       { "Quickview", "Quickview_icon.png" },
       { "Pets", "pets_icon.png" },
       { "ShadowFruit", "shadowfruit_icon.png" },
       { "LightFruit", "lightfruit_icon.png" },
       { "EarthFruit", "earthfruit_icon.png" },
       { "FireFruit", "firefruit_icon.png" },
       { "WaterFruit", "waterfruit_icon.png" },
       { "Fortress", "fortress_icon.png" },
       { "Toilet", "toilet_icon.png" },
       { "Mana", "mana_icon.png" },
    };
}
