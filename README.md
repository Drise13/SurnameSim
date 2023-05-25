# Population and Surname Convergence Simulation

This is a population simulation program written in C#. It simulates the dynamics of a population by modeling individuals and their interactions. The simulation tracks various statistics and provides insights into the population's characteristics over time.

This is a C# conversion from my original work [published on CodeReview](https://codereview.stackexchange.com/q/189168/13738)

## Features

- Age simulation: The simulation ages the population and removes individuals who reach a certain age (or die by other causes not related to their max age) (determined by the `Age` method of the `Person` class).
- Partner matching: Eligible individuals are paired up based on their gender and eligibility criteria (defined in the `GainPartners` method).
- Childbirth: Individuals with a child attribute set to `true` (determined by the `WithChild` property of the `Person` class) can have children. The `HaveChildren` method handles childbirth and adds the new children to the population.
- Children acquisition: Individuals attempt to gain new children, and the `GainChildren` method handles this process.
- Statistics tracking: The program tracks various statistics such as population size, mean age, new people, deaths, delta new people, delta deaths, surnames count, net population change, and partner percentage (calculated in the `PrintStats` method).

## Assumptions

1. I didn't base this simulation on any actual population statistics, though I think it does a decent job at producing some interesting results.
2. Surnames do not regenerate. The starting population each gets a unique "name" (an integer one greater than the last), and that forms the maximum amount of names in the simulation.
3. This is paternally biased. To keep things simple, females of the population take the male surname when paired. Surnames are never combined (see point 2), or modified in any way. This simulation would work the same (mostly?) if maternally biased, though I suppose at that point males are only needed for population growth (topic not relevant for this simulation, just some side thoughts).
4. Surnames are inherited from the parent. I didn't originally consider this an assumption, but this is not always true in every culture, with Iceland being the example (Icelandic surnames are paternal first names appended with `-son` or `-dattir` depending on gender. If Oluf had a son Jón, he would be named Jón Olufsson).
5. People die when they reach their maximum lifespan, but can also die early due to "accidents", being more prone the older they are.
6. When a person with a partner dies, the surviving partner does not gain a new partner.
7. Exponential distributions are used here to help move away from uniform distributions, since I found that uniform populations die really easily. An example would be that a Person is more likely to have a life span of 80 than 20, but that drops off significantly past 80.


## Getting Started

1. Clone the repository: `git clone https://github.com/your/repository.git`
2. Open the project in your preferred C# IDE.
3. Build the solution to ensure all dependencies are resolved.
4. Modify the simulation logic, eligibility criteria, and other aspects as needed.
5. Run the program to simulate the population dynamics and observe the tracked statistics.

## Usage

The program consists of the following components:

- `Person` class: Represents an individual in the population. It contains properties and methods for age, partner matching, childbirth, and other relevant attributes.
- `Stats` class: Manages the tracking of statistics and provides the `PrintStats` method to display the current population statistics.
- `Sim` method: Implements the simulation logic, including aging, partner matching, childbirth, and children acquisition. It returns the count of new people added during the simulation.
- `Main` method: Serves as the entry point of the program. It initializes the population and `Stats` object, performs the simulation over a specified number of years, and calls the `PrintStats` method at regular intervals.

Customize the simulation by modifying the `Sim` method and the relevant logic within the `Person` class.

## Example run (copied from the original runset)

```
2018-03-08 15:59:02.250965
Time taken: 0 seconds (0 milliseconds / year)
Current population size at year 0: 10000
Current mean age: 0
Current new people: 0
Current deaths: 0
Current delta new people: 0 (0 people / year)
Current delta deaths: 0 (0 deaths / year)
Current surname count: 10000
Current partner percentage: 0
Net population per year: 0

2018-03-08 15:59:11.135088
Time taken: 8 seconds (88 milliseconds / year)
Current population size at year 100: 26898
Current mean age: 25
Current new people: 48750
Current deaths: 31852
Current delta new people: 48750 (487 people / year)
Current delta deaths: 31852 (318 deaths / year)
Current surname count: 2044
Current partner percentage: 51
Net population per year: 168

2018-03-08 15:59:24.385102
Time taken: 13 seconds (132 milliseconds / year)
Current population size at year 200: 32645
Current mean age: 25
Current new people: 113218
Current deaths: 90573
Current delta new people: 64468 (644 people / year)
Current delta deaths: 58721 (587 deaths / year)
Current surname count: 1085
Current partner percentage: 52
Net population per year: 57

2018-03-08 15:59:41.181756
Time taken: 16 seconds (167 milliseconds / year)
Current population size at year 300: 39817
Current mean age: 25
Current new people: 191768
Current deaths: 161951
Current delta new people: 78550 (785 people / year)
Current delta deaths: 71378 (713 deaths / year)
Current surname count: 806
Current partner percentage: 53
Net population per year: 71

2018-03-08 16:00:02.352846
Time taken: 21 seconds (211 milliseconds / year)
Current population size at year 400: 47657
Current mean age: 25
Current new people: 286314
Current deaths: 248657
Current delta new people: 94546 (945 people / year)
Current delta deaths: 86706 (867 deaths / year)
Current surname count: 659
Current partner percentage: 50
Net population per year: 78

2018-03-08 16:00:28.200274
Time taken: 25 seconds (257 milliseconds / year)
Current population size at year 500: 58446
Current mean age: 25
Current new people: 400610
Current deaths: 352164
Current delta new people: 114296 (1142 people / year)
Current delta deaths: 103507 (1035 deaths / year)
Current surname count: 578
Current partner percentage: 53
Net population per year: 107
```

## Contributing

Contributions are welcome! If you have suggestions, bug reports, or feature requests, please open an issue or submit a pull request.

