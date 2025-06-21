using System.Diagnostics;

namespace SFSimulator.Core;
public static class DrHouse
{
    public static bool IsDebugging { get; set; } = Debugger.IsAttached;
    public static void Differential(string value)
    {
        if (IsDebugging)
        {
            Console.WriteLine(value);
        }
    }
}
