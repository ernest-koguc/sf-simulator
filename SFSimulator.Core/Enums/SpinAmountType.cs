using System.ComponentModel.DataAnnotations;

namespace SFSimulator.Core;

public enum SpinAmountType
{
    [Display(Name = "Only Free")]
    OnlyFree,
    Max
}