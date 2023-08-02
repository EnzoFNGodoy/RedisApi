namespace RedisApp.WebApi.Infrastructure.Data.Transactions;

public interface IUnitOfWork
{
    Task<int> SaveChanges();
}