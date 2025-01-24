using System.ComponentModel.DataAnnotations;

namespace SFSimulator.Core;

public enum SimulationFinishConditionType
{
    [Display(Name = "Days")]
    UntilDays = 0,
    [Display(Name = "Level")]
    UntilLevel = 1,
    [Display(Name = "Base Stats")]
    UntilBaseStats = 2
}