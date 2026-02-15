namespace EpsilonWebApp.Shared.Models
{
    /// <summary>
    /// Abstraction to follow Dependency Inversion and Open/Closed principles.
    /// </summary>
    public interface INamedPerson
    {
        string Name { get; }
    }

    public class Employee : INamedPerson
    {
        public string Name { get; set; } = string.Empty;
    }

    public class Manager : INamedPerson
    {
        public string Name { get; set; } = string.Empty;
    }
}
