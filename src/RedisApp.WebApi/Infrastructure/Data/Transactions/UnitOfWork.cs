namespace RedisApp.WebApi.Infrastructure.Data.Transactions;

public sealed class UnitOfWork : IUnitOfWork
{
    private readonly ToDoListContext _context;

    public UnitOfWork(ToDoListContext context)
    {
        _context = context;
    }

    public async Task<int> SaveChanges()
        => await _context.SaveChangesAsync();
}