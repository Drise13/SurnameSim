// See https://aka.ms/new-console-template for more information

using MoreLinq;

using SurnameSim;

var people = new List<Person>(Enumerable.Range(0, 10000).AsParallel().Select(_ => new Person(0)));
var stats = new Stats(DateTime.Now, people, 100);

int Sim()
{
    AgeSim(); // Age the whole simulation, removing those that die
    GainPartners(); // Pair up eligible people objects
    var newCount = HaveChildren(); // Have children before gaining them to "develop" them for a year
    GainChildren(); // Gain new children to attempt being created next year

    return newCount; // Return how many total new people have been added
}

void AgeSim()
{
    var deadPersons = people.AsParallel().Where(p => p.Age()).ToList();
    var exclusionSet = MoreLinq.Extensions.ToHashSetExtension.ToHashSet(deadPersons);

    people.RemoveAll(exclusionSet.Contains);
    stats.Lifespans.AddRange(deadPersons.Select(p => p.CurrentAge));
}

void GainPartners()
{
    // get a list of eligible partners, split by gender
    var mPartners = people.AsParallel().Where(x => x.Gender == Gender.Male && x.IsPartnerEligible()).Shuffle().ToList();
    var fPartners = people.AsParallel().Where(x => x.Gender == Gender.Female && x.IsPartnerEligible()).Shuffle().ToList();

    var numPartners = Math.Min(mPartners.Count, fPartners.Count);

    if (numPartners > 0)
    {
        //select subset to gain a partner
        var numChoices = RandomGenerator.GetRandomInteger(0, numPartners + 1);

        if (numChoices > 0)
        {
            for (var i = 0; i < numChoices; i++)
            {
                var m = mPartners[i];
                var f = fPartners[i];
                m.GainPartner(f);
                f.GainPartner(m);
            }
        }
    }
}

int HaveChildren()
{
    var newPeople = 0;

    foreach (var newChild in people.ToList()
                 .AsParallel()
                 .Where(p => p.WithChild)
                 .Select(p => p.HaveChild(stats.CurrentYear, 1.0)))
    {
        if (newChild != null)
        {
            people.Add(newChild);

            newPeople++;
        }
    }

    return newPeople;
}

void GainChildren()
{
    people.AsParallel().ForEach(p => p.GainChild());
}

foreach (var year in Enumerable.Range(0, 10000))
{
    stats.CurrentYear = year;

    if (people.Count == 0)
    {
        Console.WriteLine("Population has died! Final stats below:\n");
        stats.PrintStats();

        break;
    }

    if (year % stats.YearToPrintStats == 0)
    {
        stats.PrintStats();
    }

    stats.NewPersonCount += Sim();
}

stats.PrintStats();
