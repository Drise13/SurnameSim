namespace SurnameSim;

using MoreLinq;
using System;
using System.Net.NetworkInformation;

/// <summary>
///     Convenience class for dealing with randomness.
/// </summary>
public static class ThreadLocalRandom
{
    /// <summary>
    ///     Random number generator used to generate seeds,
    ///     which are then used to create new random number
    ///     generators on a per-thread basis.
    /// </summary>
    private static readonly Random globalRandom = new();

    private static readonly object globalLock = new();

    /// <summary>
    ///     Random number generator
    /// </summary>
    private static readonly ThreadLocal<Random> threadRandom = new(NewRandom);

    /// <summary>
    ///     Returns an instance of Random which can be used freely
    ///     within the current thread.
    /// </summary>
    public static Random Instance => threadRandom.Value;

    /// <summary>
    ///     Creates a new instance of Random. The seed is derived
    ///     from a global (static) instance of Random, rather
    ///     than time.
    /// </summary>
    public static Random NewRandom()
    {
        lock (globalLock)
        {
            return new Random(globalRandom.Next());
        }
    }

    /// <summary>See <see cref="Random.Next()" /></summary>
    public static int Next()
    {
        return Instance.Next();
    }

    /// <summary>See <see cref="Random.Next(int)" /></summary>
    public static int Next(int maxValue)
    {
        return Instance.Next(maxValue);
    }

    /// <summary>See <see cref="Random.Next(int, int)" /></summary>
    public static int Next(int minValue, int maxValue)
    {
        return Instance.Next(minValue, maxValue);
    }

    /// <summary>See <see cref="Random.NextDouble()" /></summary>
    public static double NextDouble()
    {
        return Instance.NextDouble();
    }

    /// <summary>See <see cref="Random.NextBytes(byte[])" /></summary>
    public static void NextBytes(byte[] buffer)
    {
        Instance.NextBytes(buffer);
    }
}

public static class RandomGenerator
{
    private static readonly List<int> MaxChildDistribution = new()
    {
        0,
        1,
        1,
        2,
        2,
        2,
        3,
        3,
        3,
        3,
        4,
        4,
        4,
        5,
        5
    };

    private static readonly List<int> MaxAgeDistribution;
    private static readonly double[] DeathPercentageDistribution;
    private static int deathPercentagePosition;

    static RandomGenerator()
    {
        MaxAgeDistribution = new List<int>();

        foreach (var _ in Enumerable.Range(0, 10000))
        {
            MaxAgeDistribution.Add(GetRandomAgeTo80());
        }

        foreach (var _ in Enumerable.Range(0, 2000))
        {
            MaxAgeDistribution.Add(GetRandomAgeFrom80());
        }

        MaxAgeDistribution = MaxAgeDistribution.Shuffle().ToList();
        MaxChildDistribution = MaxChildDistribution.Shuffle().ToList();

        MaxAgeDistribution.TrimExcess();
        MaxChildDistribution.TrimExcess();

        DeathPercentageDistribution = Enumerable.Range(0, 100000).Select(_ => 1.0 - GetRandomExponential(0.05) * 2.0).ToArray();
    }

    private static int GetRandomAgeTo80()
    {
        var ageTo80 = 80 - GetRandomExponential(50) * .4;

        while (ageTo80 < 0)
        {
            ageTo80 = 80 - GetRandomExponential(50) * .4;
        }

        return (int)Math.Truncate(ageTo80);
    }

    private static int GetRandomAgeFrom80()
    {
        var ageFrom80 = 80 + GetRandomExponential(50.0) * .1;

        while (ageFrom80 > 100.0)
        {
            ageFrom80 = 80 + GetRandomExponential(50.0) * .1;
        }

        return (int)Math.Truncate(ageFrom80);
    }

    public static int GetRandomInteger(int start, int stop)
    {
        return ThreadLocalRandom.Next(start, stop);
    }

    public static double GetRandomDouble(double start, double stop)
    {
        return ThreadLocalRandom.NextDouble() * (stop - start) + start;
    }

    public static double GetRandomExponential(double scale)
    {
        return -Math.Log(ThreadLocalRandom.NextDouble()) * scale;
    }

    // get a random [0, 1] with an exponential distribution.
    // Magic numbers were determined experimentally, seeing what allowed population growth, and then by checking the distribution using pyplot
    public static double GetRandomDeathChance()
    {
        Interlocked.Increment(ref deathPercentagePosition);

        return DeathPercentageDistribution[deathPercentagePosition % DeathPercentageDistribution.Length];
    }

    public static double GenerateGaussianRandomNumber(double mean, double standardDeviation)
    {
        var u1 = 1.0 - ThreadLocalRandom.NextDouble();
        var u2 = 1.0 - ThreadLocalRandom.NextDouble();

        var z0 = Math.Sqrt(-2.0 * Math.Log(u1)) * Math.Cos(2.0 * Math.PI * u2); // Box-Muller transform
        var randomNumber = mean + z0 * standardDeviation; // Apply mean and standard deviation

        return randomNumber;
    }

    public static int GetMaxChildCount()
    {
        return MaxChildDistribution[GetRandomInteger(0, MaxChildDistribution.Count)];
    }

    public static int GetRandomDeathAge()
    {
        return MaxAgeDistribution[GetRandomInteger(0, MaxAgeDistribution.Count)];
    }

    public static T GetRandomFromEnum<T>() where T : Enum
    {
        var v = Enum.GetValues(typeof(T)) ?? throw new InvalidOperationException();

        return (T)(v.GetValue(ThreadLocalRandom.Next(v.Length)) ?? throw new InvalidOperationException());
    }
}