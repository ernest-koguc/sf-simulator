using System.ComponentModel.DataAnnotations;

namespace SFSimulator.Core;

public enum AuraProgressStrategyType
{
    [Display(Name = "Normal items")]
    OnlyNormalItems,
    [Display(Name = "Epic items")]
    OnlyEpicItems,
    [Display(Name = "Normal items and epic items during event")]
    NormalItemsAndEpicDuringEvent
}
