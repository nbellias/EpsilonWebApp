namespace EpsilonWebApp.Data.Interfaces
{
    /// <summary>
    /// Interface for the Unit of Work pattern to manage repositories and transactions.
    /// </summary>
    public interface IUnitOfWork : IDisposable
    {
        /// <summary>Gets the customer repository.</summary>
        ICustomerRepository Customers { get; }
        /// <summary>Persists all changes made in the context to the database.</summary>
        Task<int> CompleteAsync();
    }
}
