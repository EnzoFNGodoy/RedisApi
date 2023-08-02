using RedisApp.WebApi.Domain.Entities;
using System.Linq.Expressions;

namespace RedisApp.WebApi.Domain.Interfaces;

public interface IToDoRepository
{
    Task<IEnumerable<ToDo>> GetAll();
    Task<IEnumerable<ToDo>> GetWhere(Expression<Func<ToDo, bool>> where);
    Task<ToDo> GetOne(Expression<Func<ToDo, bool>> where);

    Task Add(ToDo toDo);
    void Update(ToDo toDo);
}