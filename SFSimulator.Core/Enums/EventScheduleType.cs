using System.ComponentModel.DataAnnotations;

namespace SFSimulator.Core;
public enum EventScheduleType
{
    [Display(Name = "Simple")]
    SimpleCycle,
    [Display(Name = "2024 Cycle")]
    Year2024Cycle,
}
