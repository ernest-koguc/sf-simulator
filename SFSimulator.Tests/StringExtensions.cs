using System;
namespace SFSimulator.Tests;
public static class StringExtensions
{
    public static TimeSpan ToTimeSpan(this string time)
    {
        var timeParts = time.Split(['d', 'h', 'm', 's'], StringSplitOptions.RemoveEmptyEntries);

        var days = int.Parse(timeParts[0]);
        var hours = int.Parse(timeParts[1]);
        var minutes = int.Parse(timeParts[2]);
        var seconds = int.Parse(timeParts[3]);
        var timeSpan = new TimeSpan(days, hours, minutes, seconds); ;

        return timeSpan;
    }
}
