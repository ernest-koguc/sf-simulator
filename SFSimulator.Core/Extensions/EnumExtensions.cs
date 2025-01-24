using System.ComponentModel.DataAnnotations;
using System.Reflection;

namespace SFSimulator.Core;
public static class EnumExtensions
{
    public static string GetDisplayName<T>(this T type) where T : Enum
    {
        var member = type.GetType().GetMember(type.ToString()).FirstOrDefault();

        if (member == null)
        {
            return type.ToString();
        }

        var displayAttribute = member.GetCustomAttribute<DisplayAttribute>();
        if (displayAttribute == null)
        {
            return type.ToString();
        }

        return displayAttribute.Name!;
    }
}
