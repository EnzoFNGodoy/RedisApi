namespace RedisApp.WebApi.Application.ViewModels;

public sealed record ToDoRequestViewModel
{
    public string Description { get; set; } = string.Empty;
}