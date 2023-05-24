namespace SurnameSim;

public class Stats
{
    private readonly IReadOnlyList<Person> _people;
    public List<double> Deaths;
    public List<double> DeltaDeaths;
    public List<double> DeltaNewPeople;
    public List<double> MeanAge;
    public List<double> NetPopDelta;
    public List<double> NewPeople;
    public double NewPersonCount;
    public DateTime OldDate;
    public double OldDeaths;
    public double OldPeople;
    public List<double> Population;
    public double StatYear;
    public List<int> Surnames;
    public List<double> TimeTaken;
    public double Year;

    public Stats(DateTime oldDate, IReadOnlyList<Person> people, double statYear = 100)
    {
        TimeTaken = new List<double>();
        Population = new List<double>();
        MeanAge = new List<double>();
        NewPeople = new List<double>();
        Deaths = new List<double>();
        DeltaNewPeople = new List<double>();
        Surnames = new List<int>();
        DeltaDeaths = new List<double>();
        NetPopDelta = new List<double>();
        Year = 0;
        StatYear = statYear;
        OldDate = oldDate;
        _people = people;
        NewPersonCount = 0;
        OldDeaths = 0;
        OldPeople = 0;
    }

    public void PrintStats()
    {
        var statYear = StatYear;

        if (Year % statYear != 0)
        {
            statYear = Year % StatYear;
        }

        var delta = StatsHelper.GetTimeDelta(OldDate, DateTime.Now);
        var timeTaken = (int)Math.Floor(delta / 1000.0);

        if (timeTaken < 1)
        {
            Thread.Sleep(1000); // don't spam the console output
        }

        TimeTaken.Add(timeTaken);

        var population = _people.Count;
        Population.Add(population);

        var meanAge = _people.Count > 0 ? (int)Math.Floor(_people.Select(p => p.CurrentAge).Average()) : 0;
        MeanAge.Add(meanAge);

        NewPeople.Add(NewPersonCount);
        var peopleDelta = NewPersonCount - OldPeople;
        OldPeople = NewPersonCount;
        DeltaNewPeople.Add(peopleDelta);

        var surnameCount = _people.Count > 0 ? StatsHelper.CountItems(_people.Select(p => p.Surname)) : 0;
        Surnames.Add(surnameCount);

        Deaths.Add(Person.Deaths);
        var deathsDelta = Person.Deaths - OldDeaths;
        OldDeaths = Person.Deaths;
        DeltaDeaths.Add(deathsDelta);

        var netPop = peopleDelta / statYear - deathsDelta / statYear;
        NetPopDelta.Add(netPop);

        var pairCount = _people.Count(p => p.Partner != null);

        Console.WriteLine(DateTime.Now);
        Console.WriteLine($"Time taken: {timeTaken} seconds ({(int)Math.Floor(delta / statYear):F1} milliseconds / year)");
        Console.WriteLine($"Current population size at year {Year}: {population}");
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
        OldDate = DateTime.Now;
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