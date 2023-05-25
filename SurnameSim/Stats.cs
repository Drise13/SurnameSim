namespace SurnameSim;

public class Stats
{
    public readonly int YearToPrintStats;
    private readonly IReadOnlyList<Person> _people;

    public int CurrentYear = 0;
    public List<int> Deaths = new();
    public List<int> DeltaDeaths = new();
    public List<int> DeltaNewPeople = new();
    public List<int> Lifespans = new();
    public List<double> MeanAge = new();
    public List<int> NetPopDelta = new();
    public List<int> NewPeople = new();
    public int NewPersonCount = 0;
    public List<double> PopulationCount = new();
    public List<int> Surnames = new();
    public List<double> TimeTaken = new();

    private DateTime LastTimeStatsPrinted;
    private int OldDeaths;
    private int OldPeople;

    public Stats(DateTime initialTime, IReadOnlyList<Person> people, int yearToPrintStats = 100)
    {
        YearToPrintStats = yearToPrintStats;
        LastTimeStatsPrinted = initialTime;
        _people = people;
    }

    public void PrintStats()
    {
        var statYear = YearToPrintStats;

        if (CurrentYear % statYear != 0)
        {
            statYear = CurrentYear % YearToPrintStats;
        }

        var delta = StatsHelper.GetTimeDelta(LastTimeStatsPrinted, DateTime.Now);
        var timeTaken = (int)Math.Floor(delta / 1000.0);

        if (timeTaken < 1)
        {
            Thread.Sleep(1000); // don't spam the console output
        }

        TimeTaken.Add(timeTaken);

        var population = _people.Count;
        PopulationCount.Add(population);

        var meanAge = _people.Count > 0 ? (int)Math.Floor(_people.AsParallel().Select(p => p.CurrentAge).Average()) : 0;
        MeanAge.Add(meanAge);

        NewPeople.Add(NewPersonCount);
        var peopleDelta = NewPersonCount - OldPeople;
        OldPeople = NewPersonCount;
        DeltaNewPeople.Add(peopleDelta);

        var surnameCount =
            _people.Count > 0 ? _people.AsParallel().Select(p => p.CurrentSurname).Distinct().Count() : 0;

        Surnames.Add(surnameCount);

        Deaths.Add(Person.Deaths);
        var deathsDelta = Person.Deaths - OldDeaths;
        OldDeaths = Person.Deaths;
        DeltaDeaths.Add(deathsDelta);

        var netPop = peopleDelta / statYear - deathsDelta / statYear;
        NetPopDelta.Add(netPop);

        var pairCount = _people.Count(p => p.Partner != null);

        Console.WriteLine(DateTime.Now);

        Console.WriteLine(
            $"Time taken: {timeTaken} seconds ({(int)Math.Floor(delta / statYear):F1} milliseconds / year)");

        Console.WriteLine($"Current population size at year {CurrentYear}: {population}");
        Console.WriteLine($"Current mean age: {meanAge:F1}");
        Console.WriteLine($"Current new people: {NewPersonCount}");
        Console.WriteLine($"Current deaths: {Person.Deaths}");
        Console.WriteLine($"Current delta new people: {peopleDelta} ({peopleDelta / statYear:F1} people / year)");
        Console.WriteLine($"Current delta deaths: {deathsDelta} ({deathsDelta / statYear:F1} deaths / year)");
        Console.WriteLine($"Current surname count: {surnameCount}");

        Console.WriteLine(
            $"Current partner percentage: {(_people.Count > 0 ? pairCount / (double)population * 100 : 0):F1}");

        Console.WriteLine($"Net population per year: {netPop:F1}");
        Console.WriteLine();
        LastTimeStatsPrinted = DateTime.Now;
    }
}

public static class StatsHelper
{
    public static double GetTimeDelta(DateTime start, DateTime end)
    {
        var span = end - start;

        return span.TotalMilliseconds;
    }

    public static int CountItems<T>(IEnumerable<T> items)
    {
        return items.Distinct().Count();
    }
}