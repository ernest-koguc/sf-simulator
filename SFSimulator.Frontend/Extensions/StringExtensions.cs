using System.Text.RegularExpressions;

namespace SFSimulator.Frontend;

public static class StringExtensions
{
    public static string MakePrettier(this string str)
    {
        var prettier = str.Replace("_", " ");
        return Regex.Replace(prettier, "([A-Z])", " $1", RegexOptions.Compiled).Trim();
    }
}
