using SFSimulator.Core;

namespace SFSimulator.Frontend;

public static class DropDownFactory
{
    public static List<EnumItem<TEnum>> ToDropDown<TEnum>() where TEnum : notnull, Enum
        => Enum
            .GetValues(typeof(TEnum))
            .OfType<TEnum>()
            .Select(e => new EnumItem<TEnum>(e, e.GetDisplayName()))
            .ToList();
}

public record EnumItem<TEnum>(TEnum Value, string Text);
