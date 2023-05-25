# Population and Surname Convergence Simulation

This is a population simulation program written in C#. It simulates the dynamics of a population by modeling individuals and their interactions. The simulation tracks various statistics and provides insights into the population's characteristics over time.

This is a C# conversion from my original work [published on CodeReview](https://codereview.stackexchange.com/q/189168/13738) with a few upgrades and tweaks for performance opprtunities provided by C#, mostly using [AsParallel](https://learn.microsoft.com/en-us/dotnet/api/system.linq.parallelenumerable.asparallel?view=net-7.0).

## Features

- Age simulation: The simulation ages the population and removes individuals who reach a certain age (or die by other causes not related to their max age) (determined by the `Age` method of the `Person` class).
- Partner matching: Eligible individuals are paired up based on their gender and eligibility criteria (defined in the `GainPartners` method).
- Childbirth: Individuals with a child attribute set to `true` (determined by the `WithChild` property of the `Person` class) can have children. The `HaveChildren` method handles childbirth and adds the new children to the population.
- Children acquisition: Individuals attempt to gain new children, and the `GainChildren` method handles this process.
- Statistics tracking: The program tracks various statistics such as population size, mean age, new people, deaths, delta new people, delta deaths, surnames count, net population change, and partner percentage (calculated in the `PrintStats` method).

## Why?

I was curious about how surnames come and go (mostly on the go). I wanted to know how quickly surnames die out, because (in my simplified simulation world) once the last person with a surname dies, it's gone forever. I started writing a small simulation to see this happen in "real time". I started with a simple "person" representation that behaved exactly like all the other "persons" in the simulation. However, as I got into the project, I started coming up with certain traits, since not everyone lives to the same age, not everyone has the same number of children, not everyone pairs up to even have children, etc.

[The Atlantic notes that for China, with 1.3 *billion* people, *87%* of the population shares *only 100 unique surnames.*](https://www.theatlantic.com/china/archive/2013/10/the-geographic-distribution-of-chinas-last-names-in-maps/280776/)

 >For a country of 1.3 billion people, there is a remarkably small number of common last names in China. An estimated 87 percent of the population shares one of 100 surnames, and more than one in five Chinese citizens is surnamed Li, Wang, or Zhang: more than 275 million people in all. 

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

## Upgrade ideas

- **Advanced Data Analysis**: Implement a data analysis module that collects and analyzes lifetime statistics, tracks the lifespan of individuals, monitors the occurrence of surname extinction, estimates birth rates, and provides insights into various population dynamics. This module can generate reports, charts, and visualizations to help understand the patterns and trends within the simulated population.

- **Realism Tweaks**: Provide an easy-to-use interface or configuration options to adjust the simulation parameters and make the population dynamics more realistic. Users can tweak parameters such as birth rates, lifespan distributions, partner matching criteria, and other factors to observe how these changes impact the simulated population. This flexibility allows for experimentation and fine-tuning to achieve desired results or reflect real-world scenarios.

- **Social Dynamics**: Enhance the simulation by incorporating social dynamics and relationships between individuals. Introduce factors such as friendship networks, social influence, and social interactions that affect partner selection, childbirth decisions, and overall population dynamics. This addition can provide a more realistic representation of how social relationships impact the growth and evolution of a population.

- **Genetic Inheritance**: Extend the simulation to include genetic inheritance. Introduce genetic traits with inheritance rules, such as eye color or genetic predispositions to certain diseases. Individuals can pass down these traits to their offspring, allowing for the study of genetic variations within the simulated population and how they evolve over time.

- **Environmental Factors**: Integrate environmental factors into the simulation to explore their impact on population dynamics. Consider factors such as resource availability, natural disasters, climate change, or disease outbreaks. These factors can influence birth rates, mortality rates, migration patterns, and other population behaviors, adding another layer of complexity to the simulation.

- **Interactive Visualization**: Develop an interactive visualization component that allows users to observe the population simulation in real-time or explore historical data. This can include visual representations of population size, age distributions, surname trends, and other relevant statistics. Users can interact with the visualization to gain insights into the population dynamics and explore different scenarios.

- **Machine Learning Integration**: Integrate machine learning algorithms to enhance the simulation's capabilities. For example, use machine learning techniques to train models that predict future population trends, identify patterns of surname extinction, or optimize parameters for more realistic simulation outcomes. Machine learning can add a predictive and adaptive aspect to the simulation, enabling it to learn from data and make informed decisions and provide interesting insights that I wouldn't come up with myself.

## Concerns

One notable concern in the current simulation is the rapid extinction of surnames during the early stages of the simulation. This phenomenon raises questions about the accuracy and realism of the simulated population dynamics. When printing statistics at 1 year intervals, I noticed that the simulation has a lot of population decline as the simulation "primes" itself for growth later on. This has a side effect of a large portion of the surname pool going extinct very early in the sim. 

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

