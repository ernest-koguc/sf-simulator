using SFSimulator.Core;

namespace SFSimulator.Frontend;

public static class DropDownFactory
{
    public static List<EnumItem<TEnum>> ToDropDown<TEnum>(Func<TEnum, bool>? filter = null) where TEnum : notnull, Enum
        => Enum
            .GetValues(typeof(TEnum))
            .OfType<TEnum>()
            .Where(filter ?? (e => true))
            .Select(e => new EnumItem<TEnum>(e, e.GetDisplayName()))
            .ToList();
}

public record EnumItem<TEnum>(TEnum Value, string Text);
