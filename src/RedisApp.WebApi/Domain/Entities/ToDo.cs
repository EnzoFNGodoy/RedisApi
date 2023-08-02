using RedisApp.WebApi.Domain.Enums;

namespace RedisApp.WebApi.Domain.Entities;

public sealed class ToDo
{
    private ToDo() 
    { }

    public ToDo(Guid id, string description)
    {
        Id = id;
        Description = description;
        Status = EToDoStatus.NotStarted;
        CreatedAt = DateTime.UtcNow;
        UpdatedAt = CreatedAt;
        IsActive = true;
    }

    public Guid Id { get; private set; }
    public string Description { get; private set; } = string.Empty;
    public EToDoStatus Status { get; private set; }
    public DateTime CreatedAt { get; private set; }
    public DateTime UpdatedAt { get; private set; }
    public bool IsActive { get; private set; }

    public void SetToInProgress()
    {
        Status = EToDoStatus.InProgress;
        Update();
    }
    public void Finish()
    {
        Status = EToDoStatus.Finished;
        Update();
    }
    public void Cancel()
    {
        Status = EToDoStatus.Cancelled;
        Update();
    }

    public void SetDescription(string description)
    {
        Description = description;
        Update();
    }

    public void Activate()
    {
        IsActive = true;
        Update();
    }

    public void Deactivate()
    {
        IsActive = false;
        Update();
    }

    private void Update() =>
        UpdatedAt = DateTime.UtcNow;
}