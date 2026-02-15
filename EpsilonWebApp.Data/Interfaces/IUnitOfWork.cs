namespace EpsilonWebApp.Data.Interfaces
{
    public interface IUnitOfWork : IDisposable
    {
        ICustomerRepository Customers { get; }
        Task<int> CompleteAsync();
    }
}
