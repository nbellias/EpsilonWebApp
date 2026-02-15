namespace EpsilonWebApp.Shared.Models
{
    /// <summary>
    /// Abstraction to follow Dependency Inversion and Open/Closed principles.
    /// </summary>
    public interface INamedPerson
    {
        /// <summary>Gets the name of the person.</summary>
        string Name { get; }
    }

    /// <summary>
    /// Represents an employee in the organization.
    /// </summary>
    public class Employee : INamedPerson
    {
        /// <summary>Gets or sets the name of the employee.</summary>
        public string Name { get; set; } = string.Empty;
    }

    /// <summary>
    /// Represents a manager in the organization.
    /// </summary>
    public class Manager : INamedPerson
    {
        /// <summary>Gets or sets the name of the manager.</summary>
        public string Name { get; set; } = string.Empty;
    }
}
