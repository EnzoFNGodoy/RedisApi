using AutoMapper;
using Newtonsoft.Json;
using RedisApp.WebApi.Application.Interfaces;
using RedisApp.WebApi.Application.ViewModels;
using RedisApp.WebApi.Domain.Entities;
using RedisApp.WebApi.Domain.Interfaces;
using RedisApp.WebApi.Infrastructure.Caching;
using RedisApp.WebApi.Infrastructure.Data.Transactions;
using System.Data;

namespace RedisApp.WebApi.Application.Services;

public sealed class ToDoServices : IToDoServices
{
    private readonly IMapper _mapper;
    private readonly IUnitOfWork _unitOfWork;
    private readonly ICachingServices _cache;
    private readonly IToDoRepository _todoRepository;

    private readonly string _cacheKey = CachingKeys.TODO_COLLECTION_KEY;

    public ToDoServices(IMapper mapper, IUnitOfWork unitOfWork, ICachingServices cache, IToDoRepository todoRepository)
    {
        _mapper = mapper;
        _unitOfWork = unitOfWork;
        _cache = cache;
        _todoRepository = todoRepository;
    }

    #region Queries
    public async Task<IEnumerable<ToDoResponseViewModel>> GetAll()
    {
        IEnumerable<ToDo>? todos;

        var todosCache = await _cache.GetAsync(_cacheKey);

        if (!string.IsNullOrEmpty(todosCache))
        {
            var todosDeserialized = JsonConvert.DeserializeObject<IEnumerable<ToDo>>(todosCache);

            return _mapper.Map<IEnumerable<ToDoResponseViewModel>>
                (todosDeserialized);
        }
        else
        {
            todos = await _todoRepository.GetAll();

            if (todos is null || !todos.Any())
                return new List<ToDoResponseViewModel>();

            var todosSerialized = JsonConvert.SerializeObject(todos);
            await _cache.SetAsync(_cacheKey, todosSerialized);

            return _mapper.Map<IEnumerable<ToDoResponseViewModel>>(todos);
        }
    }

    public async Task<ToDoResponseViewModel> GetById(Guid id)
    {
        ToDo? todo = null;

        var todoCache = await _cache.GetAsync(id.ToString());

        if (!string.IsNullOrEmpty(todoCache))
        {
            var todoDeserialized = JsonConvert.DeserializeObject<ToDo>(todoCache);

            return _mapper.Map<ToDoResponseViewModel>
                (todoDeserialized);
        }
        else
        {
            todo = await _todoRepository.GetOne(x => x.Id == id);

            if (todo is null)
                throw new Exception($"The Todo with this Id {id} does not exists.");

            var todoSerialized = JsonConvert.SerializeObject(todo);
            await _cache.SetAsync(id.ToString(), todoSerialized);

            return _mapper.Map<ToDoResponseViewModel>
                     (todo);
        }
    }
    #endregion

    #region Commands
    public async Task Activate(Guid id)
    {
        var todo = await _todoRepository.GetOne(x => x.Id == id)
            ?? throw new InvalidOperationException("The Todo specified does not exists.");

        todo.Activate();

        _todoRepository.Update(todo);

        await CheckAndRemoveCache(id);

        if (await _unitOfWork.SaveChanges() <= 0)
            throw new DataException("An error ocurred while activating the Todo. Please try again.");
    }

    public async Task Create(ToDoRequestViewModel viewModel)
    {
        var todoExists = await _todoRepository.GetOne(x => x.Description == viewModel.Description);

        if (todoExists is not null)
            throw new InvalidOperationException("The Todo with this description already exists.");

        var todo = _mapper.Map<ToDo>(viewModel);

        await _todoRepository.Add(todo);

        await CheckAndRemoveCache(id);

        if (await _unitOfWork.SaveChanges() <= 0)
            throw new DataException("An error ocurred while saving the Todo. Please try again.");
    }

    public async Task Deactivate(Guid id)
    {
        var todo = await _todoRepository.GetOne(x => x.Id == id)
          ?? throw new InvalidOperationException("The Todo specified does not exists.");

        todo.Deactivate();

        _todoRepository.Update(todo);

        await CheckAndRemoveCache(id);

        if (await _unitOfWork.SaveChanges() <= 0)
            throw new DataException("An error ocurred while deactivating the Todo. Please try again.");
    }

    public async Task Update(Guid id, ToDoRequestViewModel viewModel)
    {
        var todo = await _todoRepository.GetOne(x => x.Id == id)
         ?? throw new InvalidOperationException("The Todo specified does not exists.");

        todo.SetDescription(viewModel.Description);

        _todoRepository.Update(todo);

        await CheckAndRemoveCache(id);

        if (await _unitOfWork.SaveChanges() <= 0)
            throw new DataException("An error ocurred while deactivating the Todo. Please try again.");
    }
    #endregion

    #region Private Methods
    private async Task CheckAndRemoveCache(Guid? id = null)
    {
        string todoCache;
        if (id is not null)
        {
            todoCache = await _cache.GetAsync(id.ToString()!);
            if (!string.IsNullOrEmpty(todoCache))
                await _cache.RemoveAsync(id.ToString()!);
        }

        todoCache = await _cache.GetAsync(_cacheKey);
        if (!string.IsNullOrEmpty(todoCache))
            await _cache.RemoveAsync(_cacheKey);
    }
    #endregion
}