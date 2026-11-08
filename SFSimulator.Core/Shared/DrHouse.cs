using System.Diagnostics;
using System.Text.Json;

namespace SFSimulator.Core;
public static class DrHouse
{
    public static bool IsDebugging { get; set; } = Debugger.IsAttached;
    public static bool IsDungeonDebugging { get; set; } = Debugger.IsAttached;
    public static void Differential(string value)
    {
        if (IsDebugging)
        {
            Debug.WriteLine(value);
            Console.WriteLine(value);
        }
    }

    public static void DungeonDifferential(string value)
    {
        if (IsDungeonDebugging)
        {
            Debug.WriteLine(value);
            Console.WriteLine(value);
        }
    }

    public static void DifferentialTemp(object obj)
    {
        Console.WriteLine(JsonSerializer.Serialize(obj));
    }
}