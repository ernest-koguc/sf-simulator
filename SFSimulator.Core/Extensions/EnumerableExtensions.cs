namespace SFSimulator.Core;
public static class EnumerableExtensions
{
    public static IEnumerable<T> Iterate<T>(this IEnumerable<T> source, Func<T, T, T> func)
    {
        if (source.Count() is 1 or 0)
        {
            return source;
        }

        var prev = source.First();
        var newEnumerable = Enumerable.Empty<T>();

        foreach (var item in source.Skip(1))
        {
            newEnumerable.Append(func(prev, item));
            prev = item;
        }

        return newEnumerable;
    }
}
