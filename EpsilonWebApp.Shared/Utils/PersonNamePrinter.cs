using EpsilonWebApp.Shared.Models;

namespace EpsilonWebApp.Shared.Utils
{
    /// <summary>
    /// Requirement 2: Strictly following SOLID principles.
    /// </summary>
    public class PersonNamePrinter
    {
        /// <summary>
        /// [S]ingle Responsibility: This method only handles printing names.
        /// [O]pen/Closed: We can add new person types (e.g. Consultant) without modifying this method.
        /// [L]iskov Substitution: Any INamedPerson implementation can be used here.
        /// [I]nterface Segregation: We depend on the smallest possible interface (INamedPerson).
        /// [D]ependency Inversion: We depend on an abstraction (INamedPerson) rather than concrete classes.
        /// </summary>
        public void PrintName(INamedPerson person)
        {
            if (person == null) throw new ArgumentNullException(nameof(person));
            
            Console.WriteLine(person.Name);
        }

        /// <summary>
        /// Helper for UI demonstration following the same SOLID principles.
        /// </summary>
        public string GetName(INamedPerson person)
        {
            return person?.Name ?? "Unknown";
        }
    }
}
