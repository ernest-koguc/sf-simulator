using System.Numerics;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;

namespace SFSimulator.Core;

public sealed class CombatRandom
{
    private const double DoubleUnit = 1.0 / (1UL << 53);

    private ulong _s0;
    private ulong _s1;
    private ulong _s2;
    private ulong _s3;

    public CombatRandom()
    {
        Span<ulong> state = stackalloc ulong[4];
        do
        {
            Random.Shared.NextBytes(MemoryMarshal.AsBytes(state));
        } while ((state[0] | state[1] | state[2] | state[3]) == 0);

        _s0 = state[0];
        _s1 = state[1];
        _s2 = state[2];
        _s3 = state[3];
    }

    public CombatRandom(ulong seed)
    {
        _s0 = SplitMix64(ref seed);
        _s1 = SplitMix64(ref seed);
        _s2 = SplitMix64(ref seed);
        _s3 = SplitMix64(ref seed);
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public double NextDouble() => (NextUInt64() >> 11) * DoubleUnit;

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public int Next(int minValue, int maxValue)
    {
        ArgumentOutOfRangeException.ThrowIfGreaterThan(minValue, maxValue);

        var range = unchecked((uint)(maxValue - minValue));
        var product = (ulong)range * (uint)(NextUInt64() >> 32);
        var low = unchecked((uint)product);
        if (low < range)
        {
            var threshold = unchecked(0U - range) % range;
            while (low < threshold)
            {
                product = (ulong)range * (uint)(NextUInt64() >> 32);
                low = unchecked((uint)product);
            }
        }

        return unchecked((int)(product >> 32) + minValue);
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public void NextDoublePair(out double first, out double second)
    {
        var s0 = _s0;
        var s1 = _s1;
        var s2 = _s2;
        var s3 = _s3;

        first = (unchecked(BitOperations.RotateLeft(s1 * 5, 7) * 9) >> 11) * DoubleUnit;
        var t = s1 << 17;
        s2 ^= s0;
        s3 ^= s1;
        s1 ^= s2;
        s0 ^= s3;
        s2 ^= t;
        s3 = BitOperations.RotateLeft(s3, 45);

        second = (unchecked(BitOperations.RotateLeft(s1 * 5, 7) * 9) >> 11) * DoubleUnit;
        t = s1 << 17;
        s2 ^= s0;
        s3 ^= s1;
        s1 ^= s2;
        s0 ^= s3;
        s2 ^= t;
        s3 = BitOperations.RotateLeft(s3, 45);

        _s0 = s0;
        _s1 = s1;
        _s2 = s2;
        _s3 = s3;
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    private ulong NextUInt64()
    {
        var s0 = _s0;
        var s1 = _s1;
        var s2 = _s2;
        var s3 = _s3;

        var result = unchecked(BitOperations.RotateLeft(s1 * 5, 7) * 9);
        var t = s1 << 17;
        s2 ^= s0;
        s3 ^= s1;
        s1 ^= s2;
        s0 ^= s3;
        s2 ^= t;
        s3 = BitOperations.RotateLeft(s3, 45);

        _s0 = s0;
        _s1 = s1;
        _s2 = s2;
        _s3 = s3;
        return result;
    }

    private static ulong SplitMix64(ref ulong seed)
    {
        var value = unchecked(seed += 0x9E3779B97F4A7C15UL);
        value = unchecked((value ^ (value >> 30)) * 0xBF58476D1CE4E5B9UL);
        value = unchecked((value ^ (value >> 27)) * 0x94D049BB133111EBUL);
        return value ^ (value >> 31);
    }
}