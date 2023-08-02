using RedisApp.WebApi.Application.ViewModels;

namespace RedisApp.WebApi.Application.Interfaces;

public interface IToDoServices
{
    Task<IEnumerable<ToDoResponseViewModel>> GetAll();
    Task<ToDoResponseViewModel> GetById(Guid id);

    Task Create(ToDoRequestViewModel viewModel);
    Task Update(Guid id, ToDoRequestViewModel viewModel);
    Task Activate(Guid id);
    Task Deactivate(Guid id);
}