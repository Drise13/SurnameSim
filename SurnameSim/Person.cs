namespace SurnameSim;

public enum Gender
{
    Male,
    Female
}

public class Person
{
    private readonly int _yearBorn;
    private readonly (Person maleParent, Person femaleParent) _parents;
    public static int CurrentName;
    public static int Deaths;

    public readonly double ChildDifficulty;
    public readonly int ChildYearsEnd;
    public readonly int ChildYearsStart;
    public readonly Gender Gender;
    public readonly int MaxAge;
    public readonly int MaxChildren;
    public readonly int PartnerEligibleAge;

    public int CurrentAge;
    public int CurrentChildCount;

    public bool HasHadPartner;
    public bool IsDead;

    public Person? Partner;

    public int CurrentSurname;
    public int OriginalSurname;
    public bool WithChild;

    public Person(int yearBorn, (Person maleParent, Person femaleParent) parents = default)
    {
        _yearBorn = yearBorn;
        _parents = parents;
        MaxAge = RandomGenerator.GetRandomDeathAge();

        if (parents == default)
        {
            CurrentName += 1;
            CurrentSurname = CurrentName;

            // Start the initial population out with a random age, no greater than 80% of their death age
            // Real-world global average age is ~30
            // Setting this to 20 since the child / partner age is around there
            CurrentAge = (int)Math.Min(MaxAge * 0.8, RandomGenerator.GenerateGaussianRandomNumber(20, 3));
        }
        else
        {
            CurrentAge = 0;
            CurrentSurname = parents.maleParent.CurrentSurname;
        }

        OriginalSurname = CurrentSurname;

        Gender = RandomGenerator.GetRandomFromEnum<Gender>();

        CurrentChildCount = 0;
        MaxChildren = RandomGenerator.GetMaxChildCount();
        ChildDifficulty = RandomGenerator.GetRandomInteger(0, 100 + 1) / 100.0;
        ChildYearsStart = RandomGenerator.GetRandomInteger(10, 15 + 1);
        ChildYearsEnd = RandomGenerator.GetRandomInteger(30, 50 + 1);
        Partner = null;
        WithChild = false;
        HasHadPartner = false;

        // determine age person is eligible to gain a partner. Having this too high (on average) leads to excessive population death
        if (ChildYearsStart < MaxAge)
        {
            PartnerEligibleAge = ChildYearsStart +
                                 Convert.ToInt32(ChildYearsStart * RandomGenerator.GetRandomExponential(0.2));

            if (PartnerEligibleAge > MaxAge)
            {
                PartnerEligibleAge = MaxAge;
            }
        }
        else
        {
            PartnerEligibleAge = MaxAge;
        }
    }

    public override string ToString()
    {
        return
            $"Person(Gender: {Gender,6}, Surname: {CurrentSurname,5}, Age: {CurrentAge,2}, MaxAge: {MaxAge,2}, Children: {CurrentChildCount,3}, " +
            $"MaxChildren: {MaxChildren,3}, Partner: {Partner != null,6}, PartnerEligibleAge: {PartnerEligibleAge,3})";
    }

    public void GainPartner(Person newPartner)
    {
        // gain a partner if both persons are eligible, female taking the surname of the other partner
        if (IsPartnerEligible())
        {
            if (Gender == Gender.Female)
            {
                CurrentSurname = newPartner.CurrentSurname;
            }

            Partner = newPartner;
            HasHadPartner = true;
        }
    }

    public bool Age()
    {
        CurrentAge += 1;

        if (CurrentAge >= MaxAge)
        {
            // if they outlive their lifespan, they die
            return Die();
        }

        // normalize age to [0, 1]
        var normalizedAge = 1.0 - (MaxAge - CurrentAge) / (float)MaxAge;


        var randDeathPercent = RandomGenerator.GetRandomDeathChance();

        // random accidents happen, so even if they haven't reached their max age,
        // there is some risk of death, especially as they get closer to their max age
        return normalizedAge > randDeathPercent && Die();
    }

    public void GainChild()
    {
        // gain a child if eligible and passes a "difficulty" check
        if (IsChildEligible() && Partner != null)
        {
            var chance = 1.0 - ChildDifficulty * Partner.ChildDifficulty;

            if (chance > RandomGenerator.GetRandomDouble(0, 1))
            {
                WithChild = true;
            }
        }
    }

    // if this modifier is set too low, not enough children are created and the population dies
    public Person? HaveChild(int currentYear, double modifier = 2.0)
    {
        // produce child if they currently have one and pass a modified "difficulty" check
        if (WithChild && Partner != null)
        {
            WithChild = false;
            var chance = 1.0 - ChildDifficulty;

            if (chance * modifier > RandomGenerator.GetRandomDouble(0, 1))
            {
                CurrentChildCount += 1;

                if (Partner != null)
                {
                    return new Person(currentYear, (Partner, this));
                }
            }
        }

        return null;
    }

    public bool Die()
    {
        IsDead = true;

        Deaths += 1;

        return true;
    }

    // check if person can have a child
    public bool IsChildEligible()
    {
        // Criteria:
        // only females can have children
        // they must have a partner
        // that partner must be alive
        // they don't currently have a child
        // they haven't had more than their max children
        // they are within child bearing years, and so is their partner
        return Gender == Gender.Female &&
               Partner is { IsDead: false } &&
               !WithChild &&
               CurrentChildCount < MaxChildren &&
               ChildYearsStart <= CurrentAge &&
               CurrentAge <= ChildYearsEnd &&
               Partner.ChildYearsStart <= Partner.CurrentAge &&
               Partner.CurrentAge <= Partner.ChildYearsEnd;
    }

    // check if person can gain a partner
    public bool IsPartnerEligible()
    {
        // Criteria:
        // they don't currently have a partner
        // they are old enough to have a partner
        // they haven't had a partner in the past
        return Partner == null && CurrentAge >= PartnerEligibleAge && !HasHadPartner;
    }
}