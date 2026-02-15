using EpsilonWebApp.Shared.Models;

namespace EpsilonWebApp.Shared.Utils
{
    /// <summary>
    /// Requirement 2: Strictly following SOLID principles to print names of people.
    /// </summary>
    public class PersonNamePrinter
    {
        /// <summary>
        /// Prints the name of the given person to the console.
        /// [S]ingle Responsibility: This method only handles printing names.
        /// [O]pen/Closed: We can add new person types (e.g. Consultant) without modifying this method.
        /// [L]iskov Substitution: Any INamedPerson implementation can be used here.
        /// [I]nterface Segregation: We depend on the smallest possible interface (INamedPerson).
        /// [D]ependency Inversion: We depend on an abstraction (INamedPerson) rather than concrete classes.
        /// </summary>
        /// <param name="person">The person whose name should be printed.</param>
        /// <exception cref="ArgumentNullException">Thrown when person is null.</exception>
        public void PrintName(INamedPerson person)
        {
            if (person == null) throw new ArgumentNullException(nameof(person));
            
            Console.WriteLine(person.Name);
        }

        /// <summary>
        /// Gets the name of the given person.
        /// </summary>
        /// <param name="person">The person whose name should be retrieved.</param>
        /// <returns>The name of the person or "Unknown" if null.</returns>
        public string GetName(INamedPerson person)
        {
            return person?.Name ?? "Unknown";
        }
    }
}
