using Bogus;
using RedisApp.WebApi.Domain.Entities;
using RedisApp.WebApi.Domain.Enums;
using RedisApp.WebApi.Domain.Helpers;

namespace RedisApp.WebApi.Infrastructure.Data;

public static class InitialData
{
    public static async void SeedData(this IServiceProvider appServices)
    {
        var todo = new PrivateFaker<ToDo>()
            .UsePrivateConstructor()
            .RuleForPrivate("Id", f => Guid.NewGuid())
            .RuleForPrivate("Description", f => f.Lorem.Word())
            .RuleForPrivate("Status", f => f.PickRandom<EToDoStatus>())
            .RuleForPrivate("CreatedAt", f => f.Date.Past(1))
            .RuleForPrivate("UpdatedAt", f => f.Date.Recent())
            .RuleForPrivate("IsActive", f => f.Random.Bool());

        var todos = todo.GenerateBetween(100,100);

        using var scope = appServices.CreateScope();
        var context = scope.ServiceProvider.GetRequiredService<ToDoListContext>();

        await context.ToDos.AddRangeAsync(todos);
        await context.SaveChangesAsync();
    }
}
