using Microsoft.EntityFrameworkCore;
using RedisApp.WebApi.Domain.Entities;
using RedisApp.WebApi.Domain.Interfaces;
using System.Linq.Expressions;

namespace RedisApp.WebApi.Infrastructure.Data.Repositories;

public sealed class ToDoRepository : IToDoRepository
{
    private readonly ToDoListContext _context;
    private readonly DbSet<ToDo> _dbSet;

    public ToDoRepository(ToDoListContext context)
    {
        _context = context;
        _dbSet = _context.Set<ToDo>();
    }

    public async Task Add(ToDo toDo)
        => await _dbSet.AddAsync(toDo);

    public async Task<IEnumerable<ToDo>> GetAll()
        => await _dbSet.ToListAsync();

    public async Task<ToDo> GetOne(Expression<Func<ToDo, bool>> where)
        => (await _dbSet.FirstOrDefaultAsync(where))!;

    public async Task<IEnumerable<ToDo>> GetWhere(Expression<Func<ToDo, bool>> where)
        => await _dbSet.Where(where).ToListAsync();

    public void Update(ToDo toDo)
        => _dbSet.Update(toDo);
}