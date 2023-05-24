# Population Simulation

This is a population simulation program written in C#. It simulates the dynamics of a population by modeling individuals and their interactions. The simulation tracks various statistics and provides insights into the population's characteristics over time.

This is a C# conversion from my original work [published on CodeReview](https://codereview.stackexchange.com/q/189168/13738)

## Features

- Age simulation: The simulation ages the population and removes individuals who reach a certain age (determined by the `age` method of the `Person` class).
- Partner matching: Eligible individuals are paired up based on their gender and eligibility criteria (defined in the `GainPartners` method).
- Childbirth: Individuals with a child attribute set to `true` (determined by the `withChild` property of the `Person` class) can have children. The `HaveChildren` method handles childbirth and adds the new children to the population.
- Children acquisition: Individuals attempt to gain new children, and the `GainChildren` method handles this process.
- Statistics tracking: The program tracks various statistics such as population size, mean age, new people, deaths, delta new people, delta deaths, surnames count, net population change, and partner percentage (calculated in the `PrintStats` method).

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

## Contributing

Contributions are welcome! If you have suggestions, bug reports, or feature requests, please open an issue or submit a pull request.

