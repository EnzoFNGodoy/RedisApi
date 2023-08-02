using RedisApp.WebApi.Application.AutoMapper;
using RedisApp.WebApi.Application.Interfaces;
using RedisApp.WebApi.Application.Services;
using RedisApp.WebApi.Domain.Interfaces;
using RedisApp.WebApi.Infrastructure.Caching;
using RedisApp.WebApi.Infrastructure.Data;
using RedisApp.WebApi.Infrastructure.Data.Repositories;
using RedisApp.WebApi.Infrastructure.Data.Transactions;

namespace RedisApp.WebApi.Infrastructure.CrossCutting;

public static class NativeInjector
{
    public static void RegisterServices(this IServiceCollection services)
    {
        services.AddDomain();
        services.AddApplication();
        services.AddInfrastructure();
    }

    private static void AddDomain(this IServiceCollection services)
    { }

    private static void AddInfrastructure(this IServiceCollection services)
    {
        // Persistence
        services.AddScoped<ToDoListContext>();
        services.AddScoped<IUnitOfWork, UnitOfWork>();

        // Caching
        services.AddScoped<ICachingServices, CachingServices>();

        // Repositories
        services.AddScoped<IToDoRepository, ToDoRepository>();
    }

    private static void AddApplication(this IServiceCollection services)
    {
        // AutoMapper
        services.AddAutoMapper(typeof(ViewModelToDomainProfile), typeof(DomainToViewModelProfile));

        // Services
        services.AddScoped<IToDoServices, ToDoServices>();
    }
}