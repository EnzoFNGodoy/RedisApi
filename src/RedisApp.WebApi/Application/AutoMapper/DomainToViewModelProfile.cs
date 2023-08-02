using AutoMapper;
using RedisApp.WebApi.Application.ViewModels;
using RedisApp.WebApi.Domain.Entities;

namespace RedisApp.WebApi.Application.AutoMapper;

public sealed class DomainToViewModelProfile : Profile
{
    public DomainToViewModelProfile()
    {
        CreateMap<ToDo, ToDoResponseViewModel>();
    }
}