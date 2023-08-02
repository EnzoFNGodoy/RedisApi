using AutoMapper;
using RedisApp.WebApi.Application.ViewModels;
using RedisApp.WebApi.Domain.Entities;

namespace RedisApp.WebApi.Application.AutoMapper;

public sealed class ViewModelToDomainProfile : Profile
{
    public ViewModelToDomainProfile()
    {
        CreateMap<ToDoRequestViewModel, ToDo>()
            .ConstructUsing(x => new ToDo(Guid.NewGuid(), x.Description));
    }
}